using System.Net;

namespace WeCard.Tests;

public sealed class StubHandler : HttpMessageHandler
{
    public Uri LastUri { get; private set; }
    public String LastBody { get; private set; }
    public Int32 TokenCalls { get; private set; }
    public List<Uri> Uris { get; } = new();

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        LastUri = request.RequestUri;
        Uris.Add(request.RequestUri);
        LastBody = request.Content == null ? null : await request.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (request.RequestUri.AbsolutePath.EndsWith("/cgi-bin/oauth2/token", StringComparison.Ordinal))
        {
            TokenCalls++;
            var json = "{\"token_type\":\"bearer\",\"access_token\":\"tok\",\"ocode\":\"1011789061\",\"expires_in\":7200}";
            return Json(json);
        }

        if (LastBody != null && LastBody.Contains("authorization_code", StringComparison.Ordinal))
            return Json("{\"access_token\":\"user-token\",\"refresh_token\":\"r\",\"expires_in\":7200,\"ocode\":\"1011789061\",\"token_type\":\"bearer\"}");

        return Json("{\"code\":0,\"message\":\"ok\"}");
    }

    private static HttpResponseMessage Json(String json)
        => new(HttpStatusCode.OK) { Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json") };
}
