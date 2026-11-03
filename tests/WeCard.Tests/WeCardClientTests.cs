using System.Reflection;
using NewLife.Cube.WeCard;
using NewLife.Serialization;
using Xunit;

namespace WeCard.Tests;

public class WeCardClientTests
{
    private static WeCardOptions Options() => new()
    {
        AppKey = "BE30B2BA30ABFADB",
        AppSecret = "CF8CDD8D0B7B1ACDA76201F406BED81E",
        Ocode = "1011789061",
    };

    [Fact]
    public void BuildUrl_PublicAndPrivateGateway()
    {
        var options = Options();
        var client = new WeCardClient(options, new StubHandler());
        Assert.Equal("https://open.wecard.qq.com/cgi-bin/oauth2/token", client.BuildUrl(WeCardHost.Open, "/cgi-bin/oauth2/token"));
        Assert.Equal("https://payapi.weixiao.qq.com/mixpay/pay", client.BuildUrl(WeCardHost.PayApi, "/mixpay/pay"));
        Assert.Equal("https://weixiao.qq.com/apps/school-api/campus-code", client.BuildUrl(WeCardHost.School, "apps/school-api/campus-code"));

        options.UsePrivateGateway("priv.example.com");
        Assert.Equal("https://priv.example.com/kong", options.BaseUrl);
        Assert.Equal("https://priv.example.com/kong/cgi-bin/user/search", client.BuildUrl(WeCardHost.Open, "/cgi-bin/user/search"));
        Assert.Equal("https://priv.example.com/kong", WeCardOptions.ToPrivateGateway("https://priv.example.com/kong/"));
        Assert.Equal("https://payapi.weixiao.qq.com/v2/pappay", client.BuildUrl(WeCardHost.PayApi, "/v2/pappay"));
    }

    [Fact]
    public void OAuthUrls_MatchDocumentedQuery()
    {
        var oauth = new WeCardOAuth(new WeCardClient(Options(), new StubHandler()));
        var h5 = oauth.BuildH5AuthorizeUrl("http://example.com", "snsapi_base", "STATE", "1011789061");
        Assert.StartsWith("https://open.wecard.qq.com/connect/oauth/authorize?", h5);
        Assert.Contains("response_type=code", h5);
        Assert.Contains("scope=snsapi_base", h5);
        Assert.Contains("ocode=1011789061", h5);
        Assert.Contains("redirect_uri=http%3A%2F%2Fexample.com", h5);
        Assert.Contains("state=STATE", h5);
        Assert.DoesNotContain("/api/third", h5);

        var pc = oauth.BuildPcAuthorizeUrl("http://xxx.com", "snsapi_userinfo", "STATE", "xxxxxx");
        Assert.Contains("/connect/oauth/pc-authorize?", pc);
        Assert.Contains("connect=curLogin", pc);

        var logout = oauth.BuildLogoutUrl("http://example.com", "1011789061");
        Assert.Contains("/connect/sso/logout?", logout);

        var manager = oauth.BuildManagerConnectUrl("http://www.example.com/index");
        Assert.StartsWith("https://uni.weixiao.qq.com/open/auth/manager-connect?", manager);
        Assert.Contains("redirect_uri=", manager);
        Assert.Equal("mnp://wx123", WeCardOAuth.MiniProgramRedirectUri("wx123"));
    }

    [Fact]
    public async Task AccessToken_IsCachedPerOcode()
    {
        var handler = new StubHandler();
        var client = new WeCardClient(Options(), handler);
        var now = new DateTime(2026, 10, 10, 0, 0, 0, DateTimeKind.Utc);
        client.UtcNow = () => now;

        var first = await client.GetAccessTokenAsync();
        var second = await client.GetAccessTokenAsync();
        Assert.Equal("tok", first);
        Assert.Equal(first, second);
        Assert.Equal(1, handler.TokenCalls);

        await client.GetAccessTokenAsync("200");
        Assert.Equal(2, handler.TokenCalls);

        now = now.AddSeconds(7140);
        await client.GetAccessTokenAsync();
        Assert.Equal(3, handler.TokenCalls);
    }

    [Fact]
    public async Task ParseBody_ThrowsOnBusinessError()
    {
        var handler = new ErrorHandler();
        var client = new WeCardClient(Options(), handler);
        var ex = await Assert.ThrowsAsync<WeCardException>(() => client.GetSchoolInfoAsync());
        Assert.Equal(WeCardErrorCodes.InvalidApp, ex.Code);
    }

    [Fact]
    public async Task Apis_UseDocumentedPathsAndDoNotCallThirdParty()
    {
        var handler = new StubHandler();
        var client = new WeCardClient(Options(), handler);
        var methods = typeof(WeCardClient).GetMethods(BindingFlags.Instance | BindingFlags.Public)
            .Select(m => (Method: m, Api: m.GetCustomAttribute<WeCardApiAttribute>()))
            .Where(x => x.Api != null)
            .ToList();

        Assert.True(methods.Count >= 100, "自建接口方法过少：" + methods.Count);
        foreach (var item in methods)
        {
            Assert.StartsWith("https://wiki.weixiao.qq.com/api/selfbuilt/", item.Api.Doc);
            Assert.DoesNotContain("/api/third", item.Api.Doc);
            Assert.StartsWith("/", item.Api.Path);
            var args = new Object[] { null, null, CancellationToken.None };
            var task = (Task)item.Method.Invoke(client, args);
            await task;
            Assert.Contains(item.Api.Path, handler.LastUri.AbsolutePath);
            Assert.DoesNotContain("third", handler.LastUri.AbsolutePath);
            if (item.Api.Auth is WeCardAuth.AccessTokenBody or WeCardAuth.AccessTokenQuery)
                Assert.Contains("access_token=", handler.LastUri.Query);
        }

        Assert.Equal(1, handler.TokenCalls);
        Assert.Contains(methods, x => x.Api.Host == WeCardHost.PayApi);
        Assert.Contains(methods, x => x.Api.Host == WeCardHost.School);
        Assert.Contains(methods, x => x.Api.Host == WeCardHost.Arrear);
    }

    [Fact]
    public async Task SignedCall_AddsSignatureWithoutToken()
    {
        var handler = new StubHandler();
        var options = Options();
        var client = new WeCardClient(options, handler);
        await client.DecodeCampusCodeAsync(new Dictionary<String, Object> { ["scene"] = "1", ["device_no"] = "d1" });
        Assert.Equal(0, handler.TokenCalls);
        Assert.Contains("/apps/school-api/campus-code", handler.LastUri.AbsolutePath);
        var map = handler.LastBody.ToJsonEntity<Dictionary<String, Object>>();
        Assert.True(map.ContainsKey("signature"));
        var pairs = map.Select(e => new KeyValuePair<String, String>(e.Key, e.Value + ""));
        Assert.True(WeCardSign.Verify(pairs, options.AppSecret));
    }

    private sealed class ErrorHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var json = request.RequestUri.AbsolutePath.EndsWith("/cgi-bin/oauth2/token", StringComparison.Ordinal)
                ? "{\"access_token\":\"tok\",\"expires_in\":7200}"
                : "{\"code\":40001,\"message\":\"bad\"}";
            return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json"),
            });
        }
    }
}
