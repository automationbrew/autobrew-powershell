namespace AutoBrew.PowerShell.Network
{
    using System.Net.Http.Headers;

    /// <summary>
    /// The handler used when requesting an access token using the refresh token flow.
    /// </summary>
    internal sealed class RefreshTokenDelegatingHandler : DelegatingHandler
    {
        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An instance of the <see cref="HttpResponseMessage" /> that represents the response from the server.</returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string contentAsString;

            if (request.Method == HttpMethod.Post && request.RequestUri.AbsoluteUri.Contains("oauth2/v2.0/token"))
            {
                contentAsString = await request.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (contentAsString.Contains("client_info") == false)
                {
                    contentAsString += "&client_info=1";

                    request.Content = new StringContent(contentAsString);
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                }
            }

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}