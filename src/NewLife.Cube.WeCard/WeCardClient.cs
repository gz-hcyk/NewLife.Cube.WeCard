using System.Collections;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using NewLife;
using NewLife.Log;
using NewLife.Serialization;

namespace NewLife.Cube.WeCard;

/// <summary>
/// 腾讯微卡自建应用 HTTP 客户端。access_token 按 ocode 缓存。
/// 单测注入 <see cref="HttpMessageHandler"/>，默认不访问公网。
/// </summary>
public partial class WeCardClient
{
    /// <summary>IHttpClientFactory 注册名</summary>
    public const String HttpClientName = "NewLife.Cube.WeCard";

    private readonly HttpClient _http;
    private readonly Boolean _ownHttp;
    private readonly ConcurrentDictionary<String, SemaphoreSlim> _gates = new(StringComparer.Ordinal);

    /// <summary>配置</summary>
    public WeCardOptions Options { get; }

    /// <summary>token 缓存</summary>
    public WeCardTokenCache Tokens { get; }

    /// <summary>当前 UTC 时间。单测可替换。</summary>
    public Func<DateTime> UtcNow { get; set; } = () => DateTime.UtcNow;

    /// <summary>日志。为空时用 XTrace。</summary>
    public ILog Log { get; set; } = XTrace.Log;

    /// <summary>用依赖注入构造</summary>
    /// <param name="options">配置</param>
    /// <param name="factory">HTTP 客户端工厂</param>
    /// <param name="tokens">token 缓存</param>
    public WeCardClient(IOptions<WeCardOptions> options, IHttpClientFactory factory, WeCardTokenCache tokens)
        : this(options?.Value ?? new WeCardOptions(), factory?.CreateClient(HttpClientName) ?? throw new ArgumentNullException(nameof(factory)), tokens, false)
    {
    }

    /// <summary>用指定 Handler 构造，便于单测。</summary>
    /// <param name="options">配置</param>
    /// <param name="handler">消息处理程序。客户端负责释放</param>
    /// <param name="tokens">缓存，可空</param>
    public WeCardClient(WeCardOptions options, HttpMessageHandler handler, WeCardTokenCache tokens = null)
        : this(options, new HttpClient(handler, disposeHandler: true), tokens, true)
    {
    }

    private WeCardClient(WeCardOptions options, HttpClient http, WeCardTokenCache tokens, Boolean ownHttp)
    {
        Options = options ?? throw new ArgumentNullException(nameof(options));
        _http = http ?? throw new ArgumentNullException(nameof(http));
        _ownHttp = ownHttp;
        Tokens = tokens ?? new WeCardTokenCache();
        if (_http.Timeout == Timeout.InfiniteTimeSpan) _http.Timeout = TimeSpan.FromSeconds(30);
    }

    /// <summary>拼绝对地址。path 已是 http(s) 时原样返回。</summary>
    /// <param name="host">网关</param>
    /// <param name="path">以 / 开头的路径</param>
    /// <returns>绝对 URL</returns>
    public String BuildUrl(WeCardHost host, String path)
    {
        if (path.IsNullOrEmpty()) throw new ArgumentNullException(nameof(path));
        if (path.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
            || path.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            return path;
        var root = Options.GetRoot(host);
        if (!path.StartsWith("/")) path = "/" + path;
        return root + path;
    }

    /// <summary>
    /// 获取应用 access_token。POST /cgi-bin/oauth2/token ，grant_type=client_credentials。
    /// 文档 https://wiki.weixiao.qq.com/api/selfbuilt/guides/accessToken.html
    /// </summary>
    /// <param name="ocode">主体代码。空则用 Options.Ocode</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>access_token</returns>
    public Task<String> GetAccessTokenAsync(String ocode = null, CancellationToken cancellationToken = default)
        => GetAccessTokenAsync(ocode, false, cancellationToken);

    /// <summary>获取 access_token，force 为 true 时忽略缓存。</summary>
    /// <param name="ocode">主体代码</param>
    /// <param name="force">true 时强制向网关刷新</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>access_token</returns>
    public async Task<String> GetAccessTokenAsync(String ocode, Boolean force, CancellationToken cancellationToken)
    {
        ocode = ocode.IsNullOrEmpty() ? Options.Ocode : ocode;
        if (ocode.IsNullOrEmpty()) throw new ArgumentNullException(nameof(ocode), "未配置主体代码 ocode");
        if (Options.AppKey.IsNullOrEmpty() || Options.AppSecret.IsNullOrEmpty())
            throw new InvalidOperationException("未配置 AppKey 或 AppSecret");

        var key = Options.AppKey + "|" + ocode;
        var now = UtcNow();
        if (!force && Tokens.TryGet(key, now, out var cached)) return cached;

        var gate = _gates.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));
        await gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            now = UtcNow();
            if (!force && Tokens.TryGet(key, now, out cached)) return cached;

            var body = new Dictionary<String, Object>
            {
                ["app_key"] = Options.AppKey,
                ["app_secret"] = Options.AppSecret,
                ["grant_type"] = "client_credentials",
                ["scope"] = "base",
                ["ocode"] = ocode,
            };
            var url = BuildUrl(WeCardHost.Open, "/cgi-bin/oauth2/token");
            var map = await SendRawAsync(HttpMethod.Post, url, body.ToJson(), cancellationToken).ConfigureAwait(false);
            var token = map.TryGetValue("access_token", out var raw) ? raw + "" : null;
            if (token.IsNullOrEmpty())
                throw new WeCardException("获取 access_token 失败：响应缺少 access_token", null, null, map.ToJson());

            var expires = ReadInt(map, "expires_in") ?? 7200;
            Tokens.Set(key, token, expires, UtcNow(), Options.TokenRefreshSkewSeconds);
            return token;
        }
        finally
        {
            gate.Release();
        }
    }

    /// <summary>按文档路径调用自建接口</summary>
    /// <param name="httpMethod">GET 或 POST</param>
    /// <param name="host">网关</param>
    /// <param name="path">路径</param>
    /// <param name="body">请求体或查询参数</param>
    /// <param name="ocode">主体代码，仅 access_token 类接口使用</param>
    /// <param name="auth">凭证方式</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>响应字典</returns>
    public async Task<IDictionary<String, Object>> SendAsync(
        String httpMethod,
        WeCardHost host,
        String path,
        Object body,
        String ocode,
        WeCardAuth auth,
        CancellationToken cancellationToken = default)
    {
        var url = BuildUrl(host, path);
        var map = ToMap(body);
        if (auth == WeCardAuth.AccessTokenBody || auth == WeCardAuth.AccessTokenQuery)
        {
            var token = await GetAccessTokenAsync(ocode, cancellationToken).ConfigureAwait(false);
            url = AppendQuery(url, "access_token", token);
        }

        if (auth == WeCardAuth.SignedBody || auth == WeCardAuth.SignedQuery)
            ApplySignature(map);

        var method = httpMethod.EqualIgnoreCase("GET") ? HttpMethod.Get : HttpMethod.Post;
        if (method == HttpMethod.Get || auth == WeCardAuth.AccessTokenQuery || auth == WeCardAuth.SignedQuery)
        {
            url = AppendQuery(url, map);
            return await SendRawAsync(method, url, null, cancellationToken).ConfigureAwait(false);
        }

        var json = map.Count == 0 && body != null ? body.ToJson() : map.ToJson();
        return await SendRawAsync(HttpMethod.Post, url, json, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>wxcode 换用户 access_token。不写入应用凭证缓存。</summary>
    /// <param name="wxcode">授权码</param>
    /// <param name="redirectUri">须与授权时一致。小程序为 mnp://小程序AppId</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>响应字典，含 access_token、refresh_token、expires_in、ocode</returns>
    public Task<IDictionary<String, Object>> ExchangeWxCodeAsync(String wxcode, String redirectUri, CancellationToken cancellationToken = default)
    {
        if (wxcode.IsNullOrEmpty()) throw new ArgumentNullException(nameof(wxcode));
        if (redirectUri.IsNullOrEmpty()) throw new ArgumentNullException(nameof(redirectUri));
        var body = new Dictionary<String, Object>
        {
            ["wxcode"] = wxcode,
            ["app_key"] = Options.AppKey,
            ["app_secret"] = Options.AppSecret,
            ["grant_type"] = "authorization_code",
            ["redirect_uri"] = redirectUri,
        };
        var url = BuildUrl(WeCardHost.Open, "/connect/oauth2/token");
        return SendRawAsync(HttpMethod.Post, url, body.ToJson(), cancellationToken);
    }

    /// <summary>用用户 access_token 换成员信息。</summary>
    /// <param name="userAccessToken">授权凭证，不是应用 access_token</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>用户信息</returns>
    public Task<IDictionary<String, Object>> GetOAuthUserInfoAsync(String userAccessToken, CancellationToken cancellationToken = default)
    {
        if (userAccessToken.IsNullOrEmpty()) throw new ArgumentNullException(nameof(userAccessToken));
        var body = new Dictionary<String, Object> { ["access_token"] = userAccessToken };
        var url = BuildUrl(WeCardHost.Open, "/connect/oauth/get-user-info");
        return SendRawAsync(HttpMethod.Post, url, body.ToJson(), cancellationToken);
    }

    private void ApplySignature(IDictionary<String, Object> map)
    {
        if (Options.AppKey.IsNullOrEmpty() || Options.AppSecret.IsNullOrEmpty())
            throw new InvalidOperationException("签名接口需要 AppKey 与 AppSecret");
        if (!map.ContainsKey("app_key")) map["app_key"] = Options.AppKey;
        if (!map.ContainsKey("timestamp")) map["timestamp"] = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture);
        if (!map.ContainsKey("nonce")) map["nonce"] = Guid.NewGuid().ToString("N");
        map.Remove("signature");
        map.Remove("sign");
        var pairs = map.Select(e => new KeyValuePair<String, String>(e.Key, Format(e.Value)));
        map["signature"] = WeCardSign.Sign(pairs, Options.AppSecret);
    }

    private async Task<IDictionary<String, Object>> SendRawAsync(HttpMethod method, String url, String json, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(method, url);
        if (!json.IsNullOrEmpty())
        {
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json") { CharSet = "utf-8" };
        }

        using var response = await _http.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var text = response.Content == null ? "" : await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (Log != null && Log.Enable && !url.Contains("/oauth2/token", StringComparison.Ordinal))
            Log.Info("WeCard {0} {1} {2}", (Int32)response.StatusCode, method, url);

        if (!response.IsSuccessStatusCode)
            throw new WeCardException($"微卡 HTTP {(Int32)response.StatusCode} {url}", (Int32)response.StatusCode, text, text);

        return ParseBody(text);
    }

    /// <summary>解析响应。code / errcode 为 0，或 code 为 SUCCESS 时视为成功。</summary>
    /// <param name="text">响应文本</param>
    /// <returns>字典。空对象返回空字典</returns>
    public static IDictionary<String, Object> ParseBody(String text)
    {
        if (text.IsNullOrEmpty()) return new Dictionary<String, Object>();
        Dictionary<String, Object> map;
        try
        {
            map = text.ToJsonEntity<Dictionary<String, Object>>();
        }
        catch (Exception ex)
        {
            throw new WeCardException("微卡响应不是 JSON：" + ex.Message, null, text, text);
        }

        map ??= new Dictionary<String, Object>();
        EnsureSuccess(map, text);
        return map;
    }

    private static void EnsureSuccess(IDictionary<String, Object> map, String raw)
    {
        foreach (var name in new[] { "code", "errcode" })
        {
            if (!map.TryGetValue(name, out var value) || value == null) continue;
            var text = value.ToString();
            if (text.EqualIgnoreCase("0") || text.EqualIgnoreCase("SUCCESS") || text.EqualIgnoreCase("OK")) continue;
            var code = ReadInt(map, name);
            var message = First(map, "message", "errmsg", "msg") ?? text;
            var hint = code.HasValue ? WeCardErrorCodes.Describe(code.Value) : "";
            var display = hint.IsNullOrEmpty() ? message : message + "（" + hint + "）";
            throw new WeCardException($"微卡接口错误 {text} {display}", code, message, raw);
        }
    }

    internal static Int32? ReadInt(IDictionary<String, Object> map, String name)
    {
        if (map == null || !map.TryGetValue(name, out var value) || value == null) return null;
        switch (value)
        {
            case Int32 i: return i;
            case Int64 l: return (Int32)l;
            case Double d: return (Int32)d;
            case Single f: return (Int32)f;
            default:
                return Int32.TryParse(value.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var n) ? n : null;
        }
    }

    private static String First(IDictionary<String, Object> map, params String[] names)
    {
        foreach (var name in names)
        {
            if (map.TryGetValue(name, out var value) && value != null && !value.ToString().IsNullOrEmpty())
                return value.ToString();
        }
        return null;
    }

    internal static IDictionary<String, Object> ToMap(Object body)
    {
        if (body == null) return new Dictionary<String, Object>();
        if (body is IDictionary<String, Object> typed)
            return new Dictionary<String, Object>(typed);
        if (body is IDictionary raw)
        {
            var map = new Dictionary<String, Object>();
            foreach (DictionaryEntry entry in raw)
                map[entry.Key + ""] = entry.Value;
            return map;
        }

        var json = body.ToJson();
        return json.ToJsonEntity<Dictionary<String, Object>>() ?? new Dictionary<String, Object>();
    }

    internal static String AppendQuery(String url, String name, String value)
    {
        var sep = url.Contains('?') ? "&" : "?";
        return url + sep + Uri.EscapeDataString(name) + "=" + Uri.EscapeDataString(value ?? "");
    }

    internal static String AppendQuery(String url, IDictionary<String, Object> map)
    {
        if (map == null) return url;
        foreach (var item in map)
        {
            if (item.Value == null) continue;
            var text = Format(item.Value);
            if (text == null) continue;
            url = AppendQuery(url, item.Key, text);
        }
        return url;
    }

    private static String Format(Object value)
    {
        if (value == null) return null;
        if (value is String s) return s;
        if (value is Boolean b) return b ? "true" : "false";
        if (value is IFormattable formattable && value is not String)
            return formattable.ToString(null, CultureInfo.InvariantCulture);
        return value.ToString();
    }
}
