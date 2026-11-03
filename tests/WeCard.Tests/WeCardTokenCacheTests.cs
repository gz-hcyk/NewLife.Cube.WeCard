using NewLife.Cube.WeCard;
using Xunit;

namespace WeCard.Tests;

public class WeCardTokenCacheTests
{
    [Fact]
    public void SeparatesOcodeAndHonorsSkew()
    {
        var cache = new WeCardTokenCache();
        var now = new DateTime(2026, 10, 10, 0, 0, 0, DateTimeKind.Utc);
        cache.Set("app|100", "t100", 7200, now, 60);
        cache.Set("app|200", "t200", 7200, now, 60);

        Assert.True(cache.TryGet("app|100", now.AddSeconds(7139), out var token));
        Assert.Equal("t100", token);
        Assert.False(cache.TryGet("app|100", now.AddSeconds(7140), out _));
        Assert.True(cache.TryGet("app|200", now.AddSeconds(10), out token));
        Assert.Equal("t200", token);
    }

    [Fact]
    public void ExpiresImmediately_WhenTtlNotPositive()
    {
        var cache = new WeCardTokenCache();
        var now = new DateTime(2026, 10, 10, 0, 0, 0, DateTimeKind.Utc);
        cache.Set("app|100", "soon", 30, now, 60);
        Assert.False(cache.TryGet("app|100", now, out _));
    }

    [Fact]
    public void Clear_DropsEntries()
    {
        var cache = new WeCardTokenCache();
        var now = DateTime.UtcNow;
        cache.Set("k", "t", 100, now, 0);
        cache.Clear();
        Assert.False(cache.TryGet("k", now, out _));
    }
}
