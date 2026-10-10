namespace NewLife.Cube.WeCard;

/// <summary>微卡接口返回的业务错误。code / errcode 非成功时抛出。</summary>
public class WeCardException : Exception
{
    /// <summary>官方错误码。非数字应答（如 FAIL）时为 null。</summary>
    public Int32? Code { get; }

    /// <summary>官方错误文本。</summary>
    public String Error { get; }

    /// <summary>原始响应文本。</summary>
    public String Raw { get; }

    /// <summary>实例化</summary>
    /// <param name="message">异常消息</param>
    /// <param name="code">错误码</param>
    /// <param name="error">错误说明</param>
    /// <param name="raw">原始响应</param>
    public WeCardException(String message, Int32? code, String error, String raw)
        : base(message)
    {
        Code = code;
        Error = error;
        Raw = raw;
    }
}

/// <summary>获取应用凭证文档列出的错误码。</summary>
public static class WeCardErrorCodes
{
    /// <summary>app_key 或 app_secret 不合法</summary>
    public const Int32 InvalidApp = 40001;

    /// <summary>access_token 过期</summary>
    public const Int32 TokenExpired = 40002;

    /// <summary>wxcode 不合法</summary>
    public const Int32 InvalidWxCode = 40003;

    /// <summary>access_token 无效</summary>
    public const Int32 TokenInvalid = 40005;

    /// <summary>ocode 不存在</summary>
    public const Int32 OcodeNotFound = 40006;

    /// <summary>电子卡号不存在</summary>
    public const Int32 CardNotFound = 40007;

    /// <summary>权限不足</summary>
    public const Int32 Forbidden = 40008;

    /// <summary>参数缺失</summary>
    public const Int32 MissingParameter = 40009;

    /// <summary>参数错误</summary>
    public const Int32 BadParameter = 40010;

    /// <summary>该主体没有开启应用</summary>
    public const Int32 AppDisabled = 40018;

    /// <summary>文档中的解决说明</summary>
    /// <param name="code">错误码</param>
    /// <returns>说明。未知码返回空串</returns>
    public static String Describe(Int32 code) => code switch
    {
        InvalidApp => "app_key 或 app_secret 不合法",
        TokenExpired => "access_token 过期",
        InvalidWxCode => "wxcode 不合法",
        TokenInvalid => "access_token 无效",
        OcodeNotFound => "ocode 不存在",
        CardNotFound => "电子卡号不存在",
        Forbidden => "权限不足",
        MissingParameter => "参数缺失",
        BadParameter => "参数错误",
        AppDisabled => "该主体没有开启应用",
        _ => "",
    };
}
