using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NewLife;
using NewLife.Log;
using NewLife.Serialization;

namespace NewLife.Cube.WeCard.Controllers;

/// <summary>
/// 微卡回调。匿名端点，安全性靠签名与 AES。
/// 未配置 AppKey/AppSecret 或关闭 EnableCallback 时返回 404。
/// 支付通知另需 NotifyKey，不配置则支付通知返回 FAIL，不会默认开通支付。
/// </summary>
[Route("/WeCard")]
public class WeCardCallbackController : ControllerBase
{
    private readonly IOptionsMonitor<WeCardOptions> _options;
    private readonly IEnumerable<IWeCardEventHandler> _handlers;

    /// <summary>实例化</summary>
    /// <param name="options">配置</param>
    /// <param name="handlers">依赖注入的处理器</param>
    public WeCardCallbackController(IOptionsMonitor<WeCardOptions> options, IEnumerable<IWeCardEventHandler> handlers)
    {
        _options = options;
        _handlers = handlers ?? Array.Empty<IWeCardEventHandler>();
    }

    /// <summary>成员 / 组织 / 应用可用范围事件。应答 {"code":0,"message":"ok"}。</summary>
    /// <returns>应答</returns>
    [HttpPost("Callback")]
    public Task<IActionResult> Callback() => ReceiveContactAsync("Callback");

    /// <summary>支付实时通知（AEAD_AES_256_GCM）。应答 code=SUCCESS。</summary>
    /// <returns>应答</returns>
    [HttpPost("PayNotify")]
    public async Task<IActionResult> PayNotify()
    {
        var options = _options.CurrentValue;
        if (!Ready(options)) return NotFound();
        if (options.NotifyKey.IsNullOrEmpty())
            return PayAck(false, "未配置 NotifyKey");

        IDictionary<String, Object> map;
        try
        {
            map = await ReadJsonAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            XTrace.WriteException(ex);
            return PayAck(false, "invalid json");
        }

        try
        {
            var resource = AsMap(map.TryGetValue("resource", out var raw) ? raw : null);
            var nonce = Read(resource, "nonce");
            var aad = Read(resource, "associated_data");
            var cipher = Read(resource, "ciphertext");
            var plain = WeCardPayCrypt.DecryptAesGcm(cipher, options.NotifyKey, nonce, aad);
            var ctx = new WeCardEventContext
            {
                Source = "PayNotify",
                Event = Read(map, "event_type"),
                AppKey = options.AppKey,
                RawJson = plain,
                Envelope = map,
                Data = TryMap(plain),
                SignatureVerified = true,
            };
            await DispatchAsync(ctx).ConfigureAwait(false);
            return PayAck(true, "ok");
        }
        catch (Exception ex)
        {
            XTrace.WriteException(ex);
            return PayAck(false, "decrypt failed");
        }
    }

    /// <summary>应用支付回调。表单或 JSON，必须带 signature。应答 {"code":0,"message":"ok"}。</summary>
    /// <returns>应答</returns>
    [HttpPost("PayCallback")]
    public async Task<IActionResult> PayCallback()
    {
        var options = _options.CurrentValue;
        if (!Ready(options)) return NotFound();

        IDictionary<String, Object> map;
        try
        {
            map = await ReadBodyMapAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            XTrace.WriteException(ex);
            return JsonCode(40010, "invalid body");
        }

        if (!Verify(map, options, out var error))
            return JsonCode(40010, error);

        var ctx = BuildContext("PayCallback", map, Read(map, "order_id"), map, true);
        await DispatchAsync(ctx).ConfigureAwait(false);
        return JsonCode(0, "ok");
    }

    /// <summary>门禁通行通知。应答 {"result":0,"msg":"ok"}。</summary>
    /// <returns>应答</returns>
    [HttpPost("GuardNotify")]
    public async Task<IActionResult> GuardNotify()
    {
        var options = _options.CurrentValue;
        if (!Ready(options)) return NotFound();

        IDictionary<String, Object> map;
        try
        {
            map = await ReadJsonAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            XTrace.WriteException(ex);
            return Content("{\"result\":-120,\"msg\":\"invalid json\"}", "application/json");
        }

        var signed = HasSign(map);
        if (signed && !Verify(map, options, out _))
            return Content("{\"result\":-120,\"msg\":\"invalid signature\"}", "application/json");

        var dataText = DecryptRaw(map, options);
        var ctx = BuildContext("GuardNotify", map, "guard", TryMap(dataText) ?? map, signed);
        ctx.RawJson = dataText ?? map.ToJson();
        await DispatchAsync(ctx).ConfigureAwait(false);
        return Content("{\"result\":0,\"msg\":\"ok\"}", "application/json");
    }

    /// <summary>缴费大厅待缴明细查询（学校侧入站）。</summary>
    /// <returns>应答</returns>
    [HttpPost("Arrear")]
    public Task<IActionResult> Arrear() => ReceiveSignedDataAsync("Arrear");

    /// <summary>缴费结果通知（学校侧入站）。</summary>
    /// <returns>应答</returns>
    [HttpPost("ArrearNotify")]
    public Task<IActionResult> ArrearNotify() => ReceiveSignedDataAsync("ArrearNotify");

    /// <summary>全量用户定时同步（学校侧入站）。成功时回 app_key 与加密 raw_data。</summary>
    /// <returns>应答</returns>
    [HttpPost("UserSync")]
    public async Task<IActionResult> UserSync()
    {
        var options = _options.CurrentValue;
        if (!Ready(options)) return NotFound();

        IDictionary<String, Object> map;
        try
        {
            map = await ReadJsonAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            XTrace.WriteException(ex);
            return JsonCode(40010, "invalid json");
        }

        if (HasSign(map) && !Verify(map, options, out var error))
            return JsonCode(40010, error);

        String plain;
        try
        {
            plain = DecryptRaw(map, options) ?? "";
        }
        catch (Exception ex)
        {
            XTrace.WriteException(ex);
            return JsonCode(40010, "decrypt failed");
        }

        var ctx = BuildContext("UserSync", map, "sync", TryMap(plain), HasSign(map));
        ctx.RawJson = plain;
        var result = await DispatchAsync(ctx).ConfigureAwait(false);
        if (result?.ResponseData == null)
            return JsonCode(1, "no handler");

        var crypt = new WeCardCrypt(options.AppKey, options.AppSecret);
        var payload = new Dictionary<String, Object>
        {
            ["app_key"] = options.AppKey,
            ["raw_data"] = crypt.Encrypt(result.ResponseData.ToJson()),
        };
        return Content(payload.ToJson(), "application/json");
    }

    private async Task<IActionResult> ReceiveContactAsync(String source)
    {
        var options = _options.CurrentValue;
        if (!Ready(options)) return NotFound();

        IDictionary<String, Object> map;
        try
        {
            map = await ReadJsonAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            XTrace.WriteException(ex);
            return JsonCode(40010, "invalid json");
        }

        if (!MatchApp(map, options, out var error))
            return JsonCode(40010, error);
        if (HasSign(map) && !Verify(map, options, out error))
            return JsonCode(40010, error);

        String plain = null;
        if (map.ContainsKey("raw_data"))
        {
            try
            {
                plain = DecryptRaw(map, options);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return JsonCode(40010, "decrypt failed");
            }
        }

        var ctx = BuildContext(source, map, Read(map, "event"), TryMap(plain), HasSign(map) && WeCardSign.Verify(Pairs(map), options.AppSecret));
        ctx.RawJson = plain ?? map.ToJson();
        await DispatchAsync(ctx).ConfigureAwait(false);
        return JsonCode(0, "ok");
    }

    private async Task<IActionResult> ReceiveSignedDataAsync(String source)
    {
        var options = _options.CurrentValue;
        if (!Ready(options)) return NotFound();
        IDictionary<String, Object> map;
        try
        {
            map = await ReadBodyMapAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            XTrace.WriteException(ex);
            return JsonCode(40010, "invalid body");
        }

        if (!Verify(map, options, out var error))
            return JsonCode(40010, error);

        var ctx = BuildContext(source, map, source, map, true);
        var result = await DispatchAsync(ctx).ConfigureAwait(false);
        var ack = new Dictionary<String, Object>
        {
            ["code"] = 0,
            ["message"] = "ok",
        };
        if (result?.ResponseData != null) ack["data"] = result.ResponseData;
        return Content(ack.ToJson(), "application/json");
    }

    private async Task<WeCardHandleResult> DispatchAsync(WeCardEventContext context)
    {
        var list = new List<IWeCardEventHandler>();
        lock (WeCardEvents.Handlers)
        {
            list.AddRange(WeCardEvents.Handlers);
        }

        foreach (var handler in _handlers)
        {
            if (!list.Contains(handler)) list.Add(handler);
        }

        foreach (var handler in list)
        {
            try
            {
                if (handler is IWeCardInboundHandler inbound)
                {
                    var result = await inbound.HandleInboundAsync(context).ConfigureAwait(false);
                    if (result != null && result.Handled) return result;
                }

                if (await handler.HandleAsync(context).ConfigureAwait(false))
                    return WeCardHandleResult.Consume();
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
        }

        return new WeCardHandleResult();
    }

    private static Boolean Ready(WeCardOptions options)
        => options != null && options.EnableCallback && !options.AppKey.IsNullOrEmpty() && !options.AppSecret.IsNullOrEmpty();

    private static Boolean MatchApp(IDictionary<String, Object> map, WeCardOptions options, out String error)
    {
        error = null;
        var appKey = Read(map, "appkey") ?? Read(map, "app_key");
        if (!appKey.IsNullOrEmpty() && !appKey.EqualIgnoreCase(options.AppKey))
        {
            error = "appkey mismatch";
            return false;
        }

        var school = Read(map, "school_code") ?? Read(map, "ocode");
        if (!options.Ocode.IsNullOrEmpty() && !school.IsNullOrEmpty() && !school.EqualIgnoreCase(options.Ocode))
        {
            error = "school_code mismatch";
            return false;
        }

        return true;
    }

    private static Boolean Verify(IDictionary<String, Object> map, WeCardOptions options, out String error)
    {
        error = null;
        if (!MatchApp(map, options, out error)) return false;
        if (!HasSign(map))
        {
            error = "missing signature";
            return false;
        }

        if (!WeCardSign.Verify(Pairs(map), options.AppSecret))
        {
            error = "invalid signature";
            return false;
        }

        return true;
    }

    private static Boolean HasSign(IDictionary<String, Object> map)
        => map != null && (map.ContainsKey("signature") || map.ContainsKey("sign"));

    private static String DecryptRaw(IDictionary<String, Object> map, WeCardOptions options)
    {
        var raw = Read(map, "raw_data");
        if (raw.IsNullOrEmpty()) return null;
        return new WeCardCrypt(options.AppKey, options.AppSecret).Decrypt(raw);
    }

    private static WeCardEventContext BuildContext(String source, IDictionary<String, Object> map, String evt, IDictionary<String, Object> data, Boolean signed)
    {
        return new WeCardEventContext
        {
            Source = source,
            Event = evt,
            SchoolCode = Read(map, "school_code") ?? Read(map, "ocode"),
            AppKey = Read(map, "appkey") ?? Read(map, "app_key"),
            Envelope = map,
            Data = data,
            RawJson = data?.ToJson() ?? map.ToJson(),
            SignatureVerified = signed,
        };
    }

    private async Task<IDictionary<String, Object>> ReadJsonAsync()
    {
        using var reader = new StreamReader(Request.Body, Encoding.UTF8);
        var text = await reader.ReadToEndAsync().ConfigureAwait(false);
        if (text.IsNullOrEmpty()) throw new InvalidDataException("empty body");
        return text.ToJsonEntity<Dictionary<String, Object>>() ?? throw new InvalidDataException("json");
    }

    private async Task<IDictionary<String, Object>> ReadBodyMapAsync()
    {
        using var reader = new StreamReader(Request.Body, Encoding.UTF8);
        var text = await reader.ReadToEndAsync().ConfigureAwait(false);
        if (text.IsNullOrEmpty()) throw new InvalidDataException("empty body");
        var trim = text.TrimStart();
        if (trim.StartsWith("{") || trim.StartsWith("["))
            return text.ToJsonEntity<Dictionary<String, Object>>() ?? throw new InvalidDataException("json");

        var map = new Dictionary<String, Object>();
        foreach (var pair in text.Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            var idx = pair.IndexOf('=');
            if (idx <= 0) continue;
            var key = Uri.UnescapeDataString(pair[..idx].Replace("+", " "));
            var value = Uri.UnescapeDataString(pair[(idx + 1)..].Replace("+", " "));
            map[key] = value;
        }

        return map;
    }

    private IActionResult JsonCode(Int32 code, String message)
    {
        var json = JsonSerializer.Serialize(new { code, message });
        return Content(json, "application/json");
    }

    private IActionResult PayAck(Boolean ok, String message)
    {
        var json = JsonSerializer.Serialize(new { code = ok ? "SUCCESS" : "FAIL", message });
        return Content(json, "application/json");
    }

    private static IDictionary<String, Object> AsMap(Object value)
    {
        if (value is IDictionary<String, Object> map) return map;
        if (value is System.Collections.IDictionary raw)
        {
            var copy = new Dictionary<String, Object>();
            foreach (System.Collections.DictionaryEntry entry in raw)
                copy[entry.Key + ""] = entry.Value;
            return copy;
        }

        if (value == null) return new Dictionary<String, Object>();
        var json = value.ToJson();
        return json.ToJsonEntity<Dictionary<String, Object>>() ?? new Dictionary<String, Object>();
    }

    private static IDictionary<String, Object> TryMap(String json)
    {
        if (json.IsNullOrEmpty()) return null;
        try
        {
            return json.ToJsonEntity<Dictionary<String, Object>>();
        }
        catch
        {
            return null;
        }
    }

    private static String Read(IDictionary<String, Object> map, String name)
    {
        if (map == null || !map.TryGetValue(name, out var value) || value == null) return null;
        return value.ToString();
    }

    private static IEnumerable<KeyValuePair<String, String>> Pairs(IDictionary<String, Object> map)
    {
        foreach (var item in map)
        {
            if (item.Value == null) continue;
            if (item.Value is not String && item.Value is not ValueType) continue;
            yield return new KeyValuePair<String, String>(item.Key, item.Value.ToString());
        }
    }
}
