using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Octokit.Internal
{
    /// <summary>
    /// Generic Http client. Useful for those who want to swap out System.Net.HttpClient with something else.
    /// </summary>
    /// <remarks>
    /// Most folks won't ever need to swap this out. But if you're trying to run this on Windows Phone, you might.
    /// </remarks>
    public class HttpClientAdapter : IHttpClient
    {
        readonly HttpClient _http;

        public const string RedirectCountKey = "RedirectCount";

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public HttpClientAdapter(Func<HttpMessageHandler> getHandler)
        {
            Ensure.ArgumentNotNull(getHandler, "getHandler");

            _http = new HttpClient(new RedirectHandler { InnerHandler = getHandler() });
        }

        /// <summary>
        /// Sends the specified request and returns a response.
        /// </summary>
        /// <param name="request">A <see cref="IRequest"/> that represents the HTTP request</param>
        /// <param name="cancellationToken">Used to cancel the request</param>
        /// <returns>A <see cref="Task" /> of <see cref="IResponse"/></returns>
        public async Task<IResponse> Send(IRequest request, CancellationToken cancellationToken)
        {
            Ensure.ArgumentNotNull(request, "request");

            var cancellationTokenForRequest = GetCancellationTokenForRequest(request, cancellationToken);

            using (var requestMessage = request.BuildMessage())
            {
                var responseMessage = await SendAsync(requestMessage, cancellationTokenForRequest).ConfigureAwait(false);

                return await responseMessage.BuildResponse().ConfigureAwait(false);
            }
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        static CancellationToken GetCancellationTokenForRequest(IRequest request, CancellationToken cancellationToken)
        {
            var cancellationTokenForRequest = cancellationToken;

            if (request.Timeout != TimeSpan.Zero)
            {
                var timeoutCancellation = new CancellationTokenSource(request.Timeout);
                var unifiedCancellationToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCancellation.Token);

                cancellationTokenForRequest = unifiedCancellationToken.Token;
            }
            return cancellationTokenForRequest;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_http != null) _http.Dispose();
            }
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Clone the request/content incase we get a redirect
            var clonedRequest = await CloneHttpRequestMessageAsync(request).ConfigureAwait(false);

            // Send initial response
            var response = await _http.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(false);

            // Can't redirect without somewhere to redirect to.
            if (response.Headers.Location == null)
            {
                return response;
            }

            // Don't redirect if we exceed max number of redirects
            var redirectCount = 0;
            if (request.Properties.Keys.Contains(RedirectCountKey))
            {
                redirectCount = (int)request.Properties[RedirectCountKey];
            }
            if (redirectCount > 3)
            {
                throw new InvalidOperationException("The redirect count for this request has been exceeded. Aborting.");
            }

            if (response.StatusCode == HttpStatusCode.MovedPermanently
                        || response.StatusCode == HttpStatusCode.Redirect
                        || response.StatusCode == HttpStatusCode.Found
                        || response.StatusCode == HttpStatusCode.SeeOther
                        || response.StatusCode == HttpStatusCode.TemporaryRedirect
                        || (int)response.StatusCode == 308)
            {
                if (response.StatusCode == HttpStatusCode.SeeOther)
                {
                    clonedRequest.Content = null;
                    clonedRequest.Method = HttpMethod.Get;
                }

                // Increment the redirect count
                clonedRequest.Properties[RedirectCountKey] = ++redirectCount;

                // Set the new Uri based on location header
                clonedRequest.RequestUri = response.Headers.Location;

                // Clear authentication if redirected to a different host
                if (string.Compare(clonedRequest.RequestUri.Host, request.RequestUri.Host, StringComparison.OrdinalIgnoreCase) != 0)
                {
                    clonedRequest.Headers.Authorization = null;
                }

                // Send redirected request
                response = await SendAsync(clonedRequest, cancellationToken).ConfigureAwait(false);
            }

            return response;
        }

        public static async Task<HttpRequestMessage> CloneHttpRequestMessageAsync(HttpRequestMessage oldRequest)
        {
            var newRequest = new HttpRequestMessage(oldRequest.Method, oldRequest.RequestUri);

            // Copy the request's content (via a MemoryStream) into the cloned object
            var ms = new MemoryStream();
            if (oldRequest.Content != null)
            {
                await oldRequest.Content.CopyToAsync(ms).ConfigureAwait(false);
                ms.Position = 0;
                newRequest.Content = new StreamContent(ms);

                // Copy the content headers
                if (oldRequest.Content.Headers != null)
                {
                    foreach (var h in oldRequest.Content.Headers)
                    {
                        newRequest.Content.Headers.Add(h.Key, h.Value);
                    }
                }
            }

            newRequest.Version = oldRequest.Version;

            foreach (var header in oldRequest.Headers)
            {
                newRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            foreach (var property in oldRequest.Properties)
            {
                newRequest.Properties.Add(property);
            }

            return newRequest;
        }
    }

    internal class RedirectHandler : DelegatingHandler
    {
    }
}