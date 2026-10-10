using NewLife.Cube.WeCard;
using Xunit;

namespace WeCard.Tests;

public class WeCardSignTests
{
    [Fact]
    public void Sign_SortsAndSkipsEmpty()
    {
        var sign = WeCardSign.Sign(new Dictionary<String, String>
        {
            ["b"] = "2",
            ["a"] = "1",
            ["empty"] = "",
            ["signature"] = "ignored",
        }, "SECRET");
        Assert.Equal("404401AD9CD3F5C0F447F93F45E7A162", sign);
    }

    [Fact]
    public void Verify_RejectsMismatchAndMissing()
    {
        var pairs = new Dictionary<String, String> { ["a"] = "1", ["b"] = "2" };
        var sign = WeCardSign.Sign(pairs, "SECRET");
        pairs["signature"] = sign;
        Assert.True(WeCardSign.Verify(pairs, "SECRET"));
        pairs["signature"] = "00";
        Assert.False(WeCardSign.Verify(pairs, "SECRET"));
        pairs.Remove("signature");
        Assert.False(WeCardSign.Verify(pairs, "SECRET"));
    }

    [Fact]
    public void Describe_KnownErrorCode()
    {
        Assert.Contains("app_secret", WeCardErrorCodes.Describe(WeCardErrorCodes.InvalidApp));
        Assert.Equal("", WeCardErrorCodes.Describe(1));
    }
}
