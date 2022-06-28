namespace AutoBrew.PowerShell.Network
{
    /// <summary>
    /// Delegating handler that modifies the request version for requests.
    /// </summary>
    public class HttpVersionHandler : DelegatingHandler
    {
        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An instance of the <see cref="HttpResponseMessage" /> that represents the response from the server.</returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Version = new Version("1.1");
            return base.SendAsync(request, cancellationToken);
        }
    }
}