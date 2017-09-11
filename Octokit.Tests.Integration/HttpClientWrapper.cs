using Octokit.Internal;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Octokit.Tests.Integration
{
    public class HttpClientWrapper : IHttpClient
    {
        private readonly HttpClient inner;

        public HttpClientWrapper(HttpClient inner)
        {
            this.inner = inner;
        }

        public void Dispose()
        {
            inner.Dispose();
        }

        public async Task<IResponse> Send(IRequest request, CancellationToken cancellationToken)
        {
            var fullUrl = new Uri(request.BaseAddress, request.Endpoint);
            var innerRequest = request.BuildMessage();
            var innerResponse = await inner.SendAsync(innerRequest);
            var response = await innerResponse.BuildResponse();
            return response;
        }
    }
}
