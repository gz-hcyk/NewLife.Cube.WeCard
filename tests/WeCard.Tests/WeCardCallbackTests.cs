using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewLife.Cube.WeCard;
using NewLife.Serialization;
using Xunit;

namespace WeCard.Tests;

public class WeCardCallbackTests : IClassFixture<WebApplicationFactory<Program>>
{
    private const String AppKey = "1234567890ABCDEF";
    private const String AppSecret = "0123456789abcdef0123456789abcdef";
    private const String Ocode = "1011789061";
    private const String NotifyKey = "abcdef0123456789abcdef0123456789";

    private readonly RecordingHandler _handler = new();
    private readonly WebApplicationFactory<Program> _factory;

    public WeCardCallbackTests(WebApplicationFactory<Program> factory)
    {
        WeCardEvents.Handlers.Clear();
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((_, cfg) =>
            {
                cfg.AddInMemoryCollection(new Dictionary<String, String>
                {
                    ["WeCard:AppKey"] = AppKey,
                    ["WeCard:AppSecret"] = AppSecret,
                    ["WeCard:Ocode"] = Ocode,
                    ["WeCard:NotifyKey"] = NotifyKey,
                    ["WeCard:EnableCallback"] = "true",
                });
            });
            builder.ConfigureServices(services => services.AddSingleton<IWeCardEventHandler>(_handler));
        });
    }

    [Fact]
    public async Task Callback_DecryptsAndDispatches()
    {
        var crypt = new WeCardCrypt(AppKey, AppSecret);
        var plain = "{\"card_number\":\"07302590\",\"name\":\"陈小明\"}";
        var body = new Dictionary<String, String>
        {
            ["appkey"] = AppKey,
            ["school_code"] = Ocode,
            ["event"] = WeCardEventNames.AddUser,
            ["raw_data"] = crypt.Encrypt(plain),
        };
        body["signature"] = WeCardSign.Sign(body, AppSecret);

        var response = await PostAsync("/WeCard/Callback", body);
        var json = await response.Content.ReadAsStringAsync();
        Assert.Contains("\"code\":0", json);
        Assert.Equal(WeCardEventNames.AddUser, _handler.Last.Event);
        Assert.Equal("07302590", _handler.Last.Data["card_number"] + "");
        Assert.True(_handler.Last.SignatureVerified);
    }

    [Fact]
    public async Task Callback_RejectsBadSignature()
    {
        var crypt = new WeCardCrypt(AppKey, AppSecret);
        var body = new Dictionary<String, String>
        {
            ["appkey"] = AppKey,
            ["school_code"] = Ocode,
            ["event"] = WeCardEventNames.AddOrg,
            ["raw_data"] = crypt.Encrypt("{\"id\":1}"),
            ["signature"] = "00000000000000000000000000000000",
        };
        var response = await PostAsync("/WeCard/Callback", body);
        var json = await response.Content.ReadAsStringAsync();
        Assert.Contains("40010", json);
    }

    [Fact]
    public async Task PayNotify_DecryptsResource()
    {
        var (nonce, cipher) = WeCardPayCrypt.EncryptAesGcm("{\"order_id\":\"o1\"}", NotifyKey, "pay");
        var payload = new
        {
            id = "EV-1",
            event_type = "TRANSACTION.PAY",
            resource = new
            {
                algorithm = "AEAD_AES_256_GCM",
                ciphertext = cipher,
                nonce,
                associated_data = "pay",
            },
        };
        var client = _factory.CreateClient();
        var response = await client.PostAsync("/WeCard/PayNotify", new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"));
        var json = await response.Content.ReadAsStringAsync();
        Assert.Contains("SUCCESS", json);
        Assert.Equal("TRANSACTION.PAY", _handler.Last.Event);
        Assert.Contains("o1", _handler.Last.RawJson);
    }

    [Fact]
    public async Task PayCallback_VerifiesFormSignature()
    {
        var fields = new Dictionary<String, String>
        {
            ["order_id"] = "20200417real0005",
            ["amount"] = "1",
            ["app_key"] = AppKey,
            ["nonce"] = "DnZ4XbphZD2roDp2",
            ["timestamp"] = "1587075796",
        };
        fields["signature"] = WeCardSign.Sign(fields, AppSecret);
        var form = String.Join("&", fields.Select(e => e.Key + "=" + Uri.EscapeDataString(e.Value)));
        var client = _factory.CreateClient();
        var response = await client.PostAsync("/WeCard/PayCallback", new StringContent(form, Encoding.UTF8, "application/x-www-form-urlencoded"));
        var json = await response.Content.ReadAsStringAsync();
        Assert.Contains("\"code\":0", json);
        Assert.Equal("PayCallback", _handler.Last.Source);
        Assert.True(_handler.Last.SignatureVerified);
    }

    [Fact]
    public async Task GuardNotify_ReturnsResultZero()
    {
        var crypt = new WeCardCrypt(AppKey, AppSecret);
        var body = new Dictionary<String, String>
        {
            ["app_key"] = AppKey,
            ["raw_data"] = crypt.Encrypt("{\"gate_id\":9}"),
        };
        body["signature"] = WeCardSign.Sign(body, AppSecret);
        var response = await PostAsync("/WeCard/GuardNotify", body);
        var json = await response.Content.ReadAsStringAsync();
        Assert.Contains("\"result\":0", json);
        Assert.Equal("GuardNotify", _handler.Last.Source);
    }

    [Fact]
    public async Task UserSync_EncryptsHandlerPayload()
    {
        _handler.ResponseData = new Dictionary<String, Object> { ["users"] = Array.Empty<Object>() };
        var crypt = new WeCardCrypt(AppKey, AppSecret);
        var body = new Dictionary<String, String>
        {
            ["app_key"] = AppKey,
            ["raw_data"] = crypt.Encrypt("{\"page\":1}"),
        };
        var response = await PostAsync("/WeCard/UserSync", body);
        var json = await response.Content.ReadAsStringAsync();
        var map = json.ToJsonEntity<Dictionary<String, Object>>();
        Assert.Equal(AppKey, map["app_key"] + "");
        var plain = crypt.Decrypt(map["raw_data"] + "");
        Assert.Contains("users", plain);
    }

    [Fact]
    public async Task Arrear_RequiresSignature()
    {
        var client = _factory.CreateClient();
        var response = await client.PostAsync("/WeCard/Arrear", new StringContent("{\"card_number\":\"1\"}", Encoding.UTF8, "application/json"));
        var json = await response.Content.ReadAsStringAsync();
        Assert.Contains("40010", json);
    }

    private async Task<HttpResponseMessage> PostAsync(String url, Object body)
    {
        var client = _factory.CreateClient();
        var json = JsonSerializer.Serialize(body);
        return await client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
    }
}
