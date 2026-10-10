namespace NewLife.Cube.WeCard;

/// <summary>解密或验签之后的回调上下文。</summary>
public class WeCardEventContext
{
    /// <summary>来源：Callback、PayNotify、PayCallback、GuardNotify、Arrear、ArrearNotify、UserSync</summary>
    public String Source { get; set; }

    /// <summary>事件名。成员/组织为 addUser 等，支付通知为 event_type。</summary>
    public String Event { get; set; }

    /// <summary>主体代码</summary>
    public String SchoolCode { get; set; }

    /// <summary>报文里的 appkey / app_key</summary>
    public String AppKey { get; set; }

    /// <summary>解密后的 JSON 文本。没有 raw_data 时为原始报文。</summary>
    public String RawJson { get; set; }

    /// <summary>外层报文</summary>
    public IDictionary<String, Object> Envelope { get; set; }

    /// <summary>解密后的对象。不是 JSON 对象时为 null。</summary>
    public IDictionary<String, Object> Data { get; set; }

    /// <summary>签名已核对</summary>
    public Boolean SignatureVerified { get; set; }
}

/// <summary>事件处理结果。需要回写数据的入站接口使用 <see cref="ResponseData"/>。</summary>
public class WeCardHandleResult
{
    /// <summary>true 表示已消费，不再交给后续处理器</summary>
    public Boolean Handled { get; set; }

    /// <summary>同步用户或待缴查询要加密回给微卡的数据</summary>
    public Object ResponseData { get; set; }

    /// <summary>已消费且不回写业务数据</summary>
    /// <returns>结果</returns>
    public static WeCardHandleResult Consume() => new() { Handled = true };
}

/// <summary>微卡事件处理器。验签解密之后按注册顺序分发，第一个返回 true 的处理器消费该事件。</summary>
public interface IWeCardEventHandler
{
    /// <summary>处理一条事件</summary>
    /// <param name="context">上下文</param>
    /// <returns>true 表示已消费</returns>
    Task<Boolean> HandleAsync(WeCardEventContext context);
}

/// <summary>需要回写数据的入站接口（全量同步、待缴查询）。</summary>
public interface IWeCardInboundHandler : IWeCardEventHandler
{
    /// <summary>处理并返回要回给微卡的数据</summary>
    /// <param name="context">上下文</param>
    /// <returns>处理结果。null 视为未消费</returns>
    Task<WeCardHandleResult> HandleInboundAsync(WeCardEventContext context);
}

/// <summary>回调处理器注册表。也可通过依赖注入注册 <see cref="IWeCardEventHandler"/>。</summary>
public static class WeCardEvents
{
    /// <summary>已注册处理器</summary>
    public static IList<IWeCardEventHandler> Handlers { get; } = new List<IWeCardEventHandler>();
}
