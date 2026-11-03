namespace NewLife.Cube.WeCard;

/// <summary>按主体缓存 access_token。过期时间提前 <c>skew</c> 秒。</summary>
public sealed class WeCardTokenCache
{
    private readonly Dictionary<String, Entry> _map = new(StringComparer.Ordinal);
    private readonly Object _lock = new();

    /// <summary>读取未过期的 token</summary>
    /// <param name="key">缓存键，一般为 appKey|ocode</param>
    /// <param name="utcNow">当前 UTC 时间</param>
    /// <param name="token">命中的 token</param>
    /// <returns>未过期时为 true</returns>
    public Boolean TryGet(String key, DateTime utcNow, out String token)
    {
        token = null;
        if (key == null) return false;
        lock (_lock)
        {
            if (!_map.TryGetValue(key, out var entry)) return false;
            if (utcNow >= entry.ExpireAt) return false;
            token = entry.Token;
            return true;
        }
    }

    /// <summary>写入 token。有效秒数小于等于提前量时立即过期，避免把快失效的凭证当成可用。</summary>
    /// <param name="key">缓存键</param>
    /// <param name="token">access_token</param>
    /// <param name="expiresInSeconds">官方 expires_in</param>
    /// <param name="utcNow">当前 UTC 时间</param>
    /// <param name="skewSeconds">提前刷新秒数</param>
    public void Set(String key, String token, Int32 expiresInSeconds, DateTime utcNow, Int32 skewSeconds)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        var ttl = expiresInSeconds - Math.Max(0, skewSeconds);
        var expireAt = ttl <= 0 ? utcNow : utcNow.AddSeconds(ttl);
        lock (_lock)
        {
            _map[key] = new Entry { Token = token, ExpireAt = expireAt };
        }
    }

    /// <summary>清空缓存</summary>
    public void Clear()
    {
        lock (_lock) _map.Clear();
    }

    private sealed class Entry
    {
        public String Token { get; set; }
        public DateTime ExpireAt { get; set; }
    }
}
