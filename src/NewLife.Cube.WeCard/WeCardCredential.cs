using NewLife;
using NewLife.Cube.Entity;
using NewLife.Log;

namespace NewLife.Cube.WeCard;

/// <summary>
/// 凭据解析：显式 Options 优先，其次魔方 OAuth 配置里 Provider=WeCard 的条目。
/// 没有数据库时返回 null，不抛给调用方。
/// </summary>
public static class WeCardCredential
{
    /// <summary>按显式配置创建客户端。不读数据库。</summary>
    /// <param name="options">AppKey、AppSecret、Ocode</param>
    /// <param name="handler">测试用 Handler，可空</param>
    /// <returns>客户端</returns>
    public static WeCardClient Create(WeCardOptions options, HttpMessageHandler handler = null)
    {
        if (options == null) throw new ArgumentNullException(nameof(options));
        if (handler != null) return new WeCardClient(options, handler);
        return new WeCardClient(options, new HttpClientHandler(), new WeCardTokenCache());
    }

    /// <summary>解析客户端。显式 AppKey+AppSecret 优先，否则查魔方 OAuth。</summary>
    /// <param name="options">显式配置，可空</param>
    /// <returns>客户端与来源。没有凭据时客户端为 null</returns>
    public static (WeCardClient Client, String Source) Resolve(WeCardOptions options)
    {
        if (options != null && !options.AppKey.IsNullOrEmpty() && !options.AppSecret.IsNullOrEmpty())
            return (Create(options), "显式参数");

        try
        {
            var cfg = FindOAuthConfig();
            if (cfg != null && !cfg.AppId.IsNullOrEmpty() && !cfg.Secret.IsNullOrEmpty())
            {
                var merged = options ?? new WeCardOptions();
                merged.AppKey = cfg.AppId;
                merged.AppSecret = cfg.Secret;
                return (Create(merged), "魔方OAuth配置");
            }
        }
        catch (Exception ex)
        {
            XTrace.WriteException(ex);
        }

        return (null, null);
    }

    /// <summary>查找已启用的微卡 OAuth 配置。Provider=WeCard，或名称包含「微卡」。</summary>
    /// <returns>配置。没有时为 null</returns>
    public static OAuthConfig FindOAuthConfig()
    {
        var list = OAuthConfig.FindAllByTenantId(0);
        foreach (var item in list)
        {
            if (!item.Enable) continue;
            if (item.Provider.EqualIgnoreCase("WeCard")) return item;
        }

        foreach (var item in list)
        {
            if (!item.Enable) continue;
            if (!item.Name.IsNullOrEmpty() && (item.Name.Contains("微卡") || item.Name.EqualIgnoreCase("WeCard")))
                return item;
        }

        return null;
    }
}
