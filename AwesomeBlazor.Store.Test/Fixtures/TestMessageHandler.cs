using System.Net;

namespace AwesomeBlazor.Store.Test.Fixtures;

internal class TestMessageHandler(string stringContent) : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(stringContent)
        });
    }
}
