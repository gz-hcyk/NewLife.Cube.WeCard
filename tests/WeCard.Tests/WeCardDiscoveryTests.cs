using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using NewLife.Cube.WeCard.Controllers;
using Xunit;

namespace WeCard.Tests;

/// <summary>Web SDK 宿主直接引用本库时，回调控制器进入路由。Mvc / WebApi 两种 CubeHost 各跑一遍。</summary>
public class WeCardDiscoveryTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public WeCardDiscoveryTests(WebApplicationFactory<Program> factory) => _factory = factory;

    [Fact]
    public void Host_DiscoversCallbackController()
    {
        var parts = typeof(Program).Assembly.GetCustomAttributes<ApplicationPartAttribute>();
        Assert.Contains(parts, part => part.AssemblyName == typeof(WeCardCallbackController).Assembly.GetName().Name);

        using var client = _factory.CreateClient();
        var provider = _factory.Services.GetRequiredService<IActionDescriptorCollectionProvider>();
        var actions = provider.ActionDescriptors.Items.OfType<ControllerActionDescriptor>()
            .Where(d => d.ControllerTypeInfo == typeof(WeCardCallbackController).GetTypeInfo())
            .ToList();

        Assert.Equal(7, actions.Count);
        Assert.Contains(actions, d => d.AttributeRouteInfo?.Template == "WeCard/Callback");
        Assert.Contains(actions, d => d.AttributeRouteInfo?.Template == "WeCard/PayNotify");
        Assert.Contains(actions, d => d.AttributeRouteInfo?.Template == "WeCard/PayCallback");
        Assert.Contains(actions, d => d.AttributeRouteInfo?.Template == "WeCard/GuardNotify");
        Assert.Contains(actions, d => d.AttributeRouteInfo?.Template == "WeCard/Arrear");
        Assert.Contains(actions, d => d.AttributeRouteInfo?.Template == "WeCard/ArrearNotify");
        Assert.Contains(actions, d => d.AttributeRouteInfo?.Template == "WeCard/UserSync");

        var endpoints = _factory.Services.GetRequiredService<EndpointDataSource>().Endpoints.OfType<RouteEndpoint>().ToList();
        Assert.Contains(endpoints, e => e.RoutePattern.RawText == "WeCard/Callback");
    }

    [Fact]
    public async Task UnconfiguredCallback_Returns404()
    {
        var client = _factory.CreateClient();
        var response = await client.PostAsync("/WeCard/Callback", new StringContent("{}", System.Text.Encoding.UTF8, "application/json"));
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }
}
