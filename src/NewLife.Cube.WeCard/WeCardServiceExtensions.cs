using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NewLife.Cube.WeCard;
using NewLife.Log;

namespace NewLife.Cube;

/// <summary>微卡扩展注册。宿主在 AddCube / UseCube 旁调用，不改魔方框架源码。</summary>
public static class WeCardServiceExtensions
{
    /// <summary>注册微卡客户端、OAuth 与 token 缓存。</summary>
    /// <param name="services">服务集合</param>
    /// <param name="configure">额外配置。配置节 WeCard 由宿主 <c>Configure&lt;WeCardOptions&gt;</c> 绑定</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddWeCard(this IServiceCollection services, Action<WeCardOptions> configure = null)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));
        services.AddOptions<WeCardOptions>();
        if (configure != null) services.Configure(configure);
        services.AddHttpClient(WeCardClient.HttpClientName, client =>
        {
            client.Timeout = TimeSpan.FromSeconds(100);
        });
        services.AddSingleton<WeCardTokenCache>();
        services.AddSingleton<WeCardClient>();
        services.AddSingleton<WeCardOAuth>();
        return services;
    }

    /// <summary>
    /// 启用微卡回调。端点由 MVC / WebApi 的控制器发现挂上，这里只记录回调地址。
    /// 支付、门禁不会因调用本方法而开通。
    /// </summary>
    /// <param name="app">应用</param>
    /// <returns>应用</returns>
    public static IApplicationBuilder UseWeCard(this IApplicationBuilder app)
    {
        if (app == null) throw new ArgumentNullException(nameof(app));
        XTrace.WriteLine("微卡回调：/WeCard/Callback  支付通知：/WeCard/PayNotify  门禁通知：/WeCard/GuardNotify");
        return app;
    }
}
