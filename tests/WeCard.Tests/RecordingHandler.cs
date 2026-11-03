using NewLife.Cube.WeCard;

namespace WeCard.Tests;

public sealed class RecordingHandler : IWeCardInboundHandler
{
    public WeCardEventContext Last { get; private set; }
    public Object ResponseData { get; set; }

    public Task<Boolean> HandleAsync(WeCardEventContext context)
    {
        Last = context;
        return Task.FromResult(ResponseData == null);
    }

    public Task<WeCardHandleResult> HandleInboundAsync(WeCardEventContext context)
    {
        Last = context;
        if (ResponseData == null) return Task.FromResult<WeCardHandleResult>(null);
        return Task.FromResult(new WeCardHandleResult { Handled = true, ResponseData = ResponseData });
    }
}
