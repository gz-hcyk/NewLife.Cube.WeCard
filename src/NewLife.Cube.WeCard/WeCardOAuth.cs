using NewLife;

namespace NewLife.Cube.WeCard;

/// <summary>
/// H5 / PC / 小程序 OAuth 与管理员登录态。
/// 文档：
/// https://wiki.weixiao.qq.com/api/selfbuilt/authentication/h5_oauth.html
/// https://wiki.weixiao.qq.com/api/selfbuilt/authentication/pc_oauth.html
/// https://wiki.weixiao.qq.com/api/selfbuilt/authentication/mnp_oauth.html
/// https://wiki.weixiao.qq.com/api/selfbuilt/guides/standard.html
/// </summary>
public class WeCardOAuth
{
    private readonly WeCardClient _client;

    /// <summary>小程序插件提供方 AppId，文档当前版本 4.0.1。</summary>
    public const String MiniProgramPluginProvider = "wx1549b963bbae9931";

    /// <summary>小程序插件版本文档记载值。</summary>
    public const String MiniProgramPluginVersion = "4.0.1";

    /// <summary>实例化</summary>
    /// <param name="client">微卡客户端</param>
    public WeCardOAuth(WeCardClient client) => _client = client ?? throw new ArgumentNullException(nameof(client));

    /// <summary>配置</summary>
    public WeCardOptions Options => _client.Options;

    /// <summary>H5 授权地址。scope：snsapi_base / snsapi_userinfo / snsapi_sso。</summary>
    /// <param name="redirectUri">回调地址，方法内做 UrlEncode</param>
    /// <param name="scope">授权范围</param>
    /// <param name="state">透传，可空</param>
    /// <param name="ocode">主体代码，可空。空则跳到选主体页</param>
    /// <returns>授权 URL</returns>
    public String BuildH5AuthorizeUrl(String redirectUri, String scope = "snsapi_base", String state = null, String ocode = null)
    {
        return BuildAuthorize("/connect/oauth/authorize", redirectUri, scope, state, ocode, null);
    }

    /// <summary>PC 授权地址。</summary>
    /// <param name="redirectUri">回调地址</param>
    /// <param name="scope">授权范围，文档示例 snsapi_userinfo</param>
    /// <param name="state">透传</param>
    /// <param name="ocode">主体代码</param>
    /// <param name="connect">文档示例 curLogin，可空</param>
    /// <returns>授权 URL</returns>
    public String BuildPcAuthorizeUrl(String redirectUri, String scope = "snsapi_userinfo", String state = null, String ocode = null, String connect = "curLogin")
    {
        return BuildAuthorize("/connect/oauth/pc-authorize", redirectUri, scope, state, ocode, connect);
    }

    /// <summary>snsapi_sso 注销地址。</summary>
    /// <param name="redirectUri">注销后跳转</param>
    /// <param name="ocode">主体代码</param>
    /// <returns>注销 URL</returns>
    public String BuildLogoutUrl(String redirectUri, String ocode = null)
    {
        if (redirectUri.IsNullOrEmpty()) throw new ArgumentNullException(nameof(redirectUri));
        ocode = ocode.IsNullOrEmpty() ? Options.Ocode : ocode;
        var url = _client.BuildUrl(WeCardHost.Open, "/connect/sso/logout");
        url = WeCardClient.AppendQuery(url, "app_key", Options.AppKey ?? "");
        if (!ocode.IsNullOrEmpty()) url = WeCardClient.AppendQuery(url, "ocode", ocode);
        return WeCardClient.AppendQuery(url, "redirect_uri", redirectUri);
    }

    /// <summary>管理后台管理员登录态。文档为 uni.weixiao.qq.com/open/auth/manager-connect 。</summary>
    /// <param name="redirectUri">管理后台地址</param>
    /// <returns>跳转 URL</returns>
    public String BuildManagerConnectUrl(String redirectUri)
    {
        if (redirectUri.IsNullOrEmpty()) throw new ArgumentNullException(nameof(redirectUri));
        var root = Options.GetRoot(WeCardHost.Uni);
        var url = root + "/open/auth/manager-connect";
        url = WeCardClient.AppendQuery(url, "app_key", Options.AppKey ?? "");
        return WeCardClient.AppendQuery(url, "redirect_uri", redirectUri);
    }

    /// <summary>微卡公众号 openid 跳转地址。</summary>
    /// <param name="subAppId">商户号</param>
    /// <param name="schoolCode">主体代码</param>
    /// <param name="redirectUri">回跳地址</param>
    /// <returns>跳转 URL</returns>
    public String BuildMediaOpenIdUrl(String subAppId, String schoolCode, String redirectUri)
    {
        var url = _client.BuildUrl(WeCardHost.PayApi, "/midas/app/oauth-openid");
        url = WeCardClient.AppendQuery(url, "sub_appid", subAppId ?? "");
        url = WeCardClient.AppendQuery(url, "school_code", schoolCode ?? "");
        url = WeCardClient.AppendQuery(url, "app_key", Options.AppKey ?? "");
        return WeCardClient.AppendQuery(url, "redirect_uri", redirectUri ?? "");
    }

    /// <summary>小程序用 wxcode 换票时的 redirect_uri：mnp:// + 小程序 AppId。</summary>
    /// <param name="miniProgramAppId">小程序 AppId</param>
    /// <returns>redirect_uri</returns>
    public static String MiniProgramRedirectUri(String miniProgramAppId)
    {
        if (miniProgramAppId.IsNullOrEmpty()) throw new ArgumentNullException(nameof(miniProgramAppId));
        return "mnp://" + miniProgramAppId;
    }

    /// <summary>wxcode 换 access_token</summary>
    /// <param name="wxcode">授权码，约 5 分钟有效</param>
    /// <param name="redirectUri">与授权请求一致</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>令牌响应</returns>
    public Task<IDictionary<String, Object>> ExchangeCodeAsync(String wxcode, String redirectUri, CancellationToken cancellationToken = default)
        => _client.ExchangeWxCodeAsync(wxcode, redirectUri, cancellationToken);

    /// <summary>用户 access_token 换成员信息</summary>
    /// <param name="userAccessToken">用户凭证</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <returns>成员信息</returns>
    public Task<IDictionary<String, Object>> GetUserInfoAsync(String userAccessToken, CancellationToken cancellationToken = default)
        => _client.GetOAuthUserInfoAsync(userAccessToken, cancellationToken);

    private String BuildAuthorize(String path, String redirectUri, String scope, String state, String ocode, String connect)
    {
        if (redirectUri.IsNullOrEmpty()) throw new ArgumentNullException(nameof(redirectUri));
        if (scope.IsNullOrEmpty()) throw new ArgumentNullException(nameof(scope));
        ocode = ocode.IsNullOrEmpty() ? Options.Ocode : ocode;
        var url = _client.BuildUrl(WeCardHost.Open, path);
        url = WeCardClient.AppendQuery(url, "app_key", Options.AppKey ?? "");
        url = WeCardClient.AppendQuery(url, "response_type", "code");
        url = WeCardClient.AppendQuery(url, "scope", scope);
        if (!ocode.IsNullOrEmpty()) url = WeCardClient.AppendQuery(url, "ocode", ocode);
        url = WeCardClient.AppendQuery(url, "redirect_uri", redirectUri);
        if (!state.IsNullOrEmpty()) url = WeCardClient.AppendQuery(url, "state", state);
        if (!connect.IsNullOrEmpty()) url = WeCardClient.AppendQuery(url, "connect", connect);
        return url;
    }
}
