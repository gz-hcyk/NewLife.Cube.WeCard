using NewLife;

namespace NewLife.Cube.WeCard;

/// <summary>微卡网关种类。私有化时只替换对应 BaseUrl，路径与公有云文档一致。</summary>
public enum WeCardHost
{
    /// <summary>开放平台，默认 https://open.wecard.qq.com 。私有化为「私有化域名 + /kong」。</summary>
    Open = 0,

    /// <summary>机具支付，默认 https://payapi.weixiao.qq.com</summary>
    PayApi = 1,

    /// <summary>电子卡 / 电子码，默认 https://weixiao.qq.com</summary>
    School = 2,

    /// <summary>缴费大厅，默认 https://jf.weixiao.qq.com</summary>
    Arrear = 3,

    /// <summary>管理后台登录，默认 https://uni.weixiao.qq.com</summary>
    Uni = 4,
}

/// <summary>调用开放接口时如何附带凭证。</summary>
public enum WeCardAuth
{
    /// <summary>POST JSON，query 附带 access_token。</summary>
    AccessTokenBody = 0,

    /// <summary>参数放在 query，并附带 access_token。</summary>
    AccessTokenQuery = 1,

    /// <summary>按签名算法补 app_key、timestamp、nonce、signature 后 POST JSON。</summary>
    SignedBody = 2,

    /// <summary>按签名算法把参数放在 query。</summary>
    SignedQuery = 3,

    /// <summary>原样 POST。支付机具接口由调用方按文档组包，本库不默认开通支付。</summary>
    RawBody = 4,
}

/// <summary>
/// 腾讯微卡自建应用配置。凭据放配置或 Options，不要写进源码。
/// 公有云 BaseUrl 为 https://open.wecard.qq.com ；私有化用 <see cref="UsePrivateGateway"/>。
/// </summary>
public class WeCardOptions
{
    /// <summary>公有云开放平台根地址。</summary>
    public const String PublicOpenRoot = "https://open.wecard.qq.com";

    /// <summary>公有云机具支付根地址。</summary>
    public const String PublicPayApiRoot = "https://payapi.weixiao.qq.com";

    /// <summary>电子卡 / 电子码根地址。</summary>
    public const String PublicSchoolRoot = "https://weixiao.qq.com";

    /// <summary>缴费大厅根地址。</summary>
    public const String PublicArrearRoot = "https://jf.weixiao.qq.com";

    /// <summary>管理后台 OAuth 根地址。</summary>
    public const String PublicUniRoot = "https://uni.weixiao.qq.com";

    /// <summary>应用 AppKey。自建应用后台分配，16 位。</summary>
    public String AppKey { get; set; }

    /// <summary>应用 AppSecret。自建应用后台分配，32 位。不要提交到仓库。</summary>
    public String AppSecret { get; set; }

    /// <summary>主体代码 ocode / school_code。</summary>
    public String Ocode { get; set; }

    /// <summary>开放平台根地址。私有化设为「私有化域名 + /kong」。</summary>
    public String BaseUrl { get; set; } = PublicOpenRoot;

    /// <summary>机具支付根地址。私有化时按主体文档单独替换，不自动加 /kong。</summary>
    public String PayApiBaseUrl { get; set; } = PublicPayApiRoot;

    /// <summary>电子卡 / 电子码根地址。私有化文档要求换成私有化域名。</summary>
    public String SchoolApiBaseUrl { get; set; } = PublicSchoolRoot;

    /// <summary>缴费大厅根地址。</summary>
    public String ArrearBaseUrl { get; set; } = PublicArrearRoot;

    /// <summary>管理后台 OAuth 根地址。</summary>
    public String UniBaseUrl { get; set; } = PublicUniRoot;

    /// <summary>access_token 提前刷新的秒数。官方有效期默认 7200 秒。</summary>
    public Int32 TokenRefreshSkewSeconds { get; set; } = 60;

    /// <summary>
    /// 支付通知密钥 notify_key，32 字节。仅在接入「实时数据推送」时填写。
    /// 空表示不解密支付通知。
    /// </summary>
    public String NotifyKey { get; set; }

    /// <summary>回调未配置 AppKey/AppSecret 时端点返回 404。</summary>
    public Boolean EnableCallback { get; set; } = true;

    /// <summary>把开放平台根地址改成私有化网关（域名 + /kong）。</summary>
    /// <param name="domain">私有化域名，可带或不带协议</param>
    public void UsePrivateGateway(String domain)
    {
        BaseUrl = ToPrivateGateway(domain);
    }

    /// <summary>拼私有化网关：私有化域名 + /kong。</summary>
    /// <param name="domain">域名或完整地址</param>
    /// <returns>以 /kong 结尾的根地址</returns>
    public static String ToPrivateGateway(String domain)
    {
        if (domain.IsNullOrEmpty()) throw new ArgumentNullException(nameof(domain));
        var text = domain.Trim().TrimEnd('/');
        if (!text.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
            && !text.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            text = "https://" + text;
        if (text.EndsWith("/kong", StringComparison.OrdinalIgnoreCase)) return text;
        return text + "/kong";
    }

    /// <summary>按网关种类取根地址。</summary>
    /// <param name="host">网关</param>
    /// <returns>去掉末尾斜杠的根地址</returns>
    public String GetRoot(WeCardHost host)
    {
        var root = host switch
        {
            WeCardHost.PayApi => PayApiBaseUrl,
            WeCardHost.School => SchoolApiBaseUrl,
            WeCardHost.Arrear => ArrearBaseUrl,
            WeCardHost.Uni => UniBaseUrl,
            _ => BaseUrl,
        };
        if (root.IsNullOrEmpty()) root = PublicOpenRoot;
        return root.TrimEnd('/');
    }
}
