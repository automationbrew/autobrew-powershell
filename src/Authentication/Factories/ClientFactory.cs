namespace AutoBrew.PowerShell.Factories
{
    using System.Net;
    using System.Threading;
    using AutoBrew.PowerShell.Models;
    using Microsoft.Graph;
    using Microsoft.Rest;
    using Models.Authentication;
    using Network;

    /// <summary>
    /// The factory used to create clients used to communicate with online services.
    /// </summary>
    public class ClientFactory : IClientFactory
    {
        /// <summary>
        /// The client used to perform HTTP operations.
        /// </summary>
        private static readonly HttpClient httpClient = new();

        /// <summary>
        /// Creates a new instance of the <see cref="GraphServiceClient" /> class used to communicate with Microsoft Graph.
        /// </summary>
        /// <param name="account">An instance of the <see cref="ModuleAccount" /> class that provides information used to authenticate.</param>
        /// <returns>An instance of the <see cref="GraphServiceClient" /> class used to communicate with Microsoft Graph.</returns>
        /// <exception cref="ArgumentNullException">
        /// The account parameter is null.
        /// </exception>
        public GraphServiceClient CreateGraphServiceClient(ModuleAccount account)
        {
            account.AssertNotNull(nameof(account));

            return new(
                null,
                new HttpProvider(
                    GraphClientFactory.CreatePipeline(
                        GraphClientFactory.CreateDefaultHandlers(null),
                        new TracingHandler
                        {
                            InnerHandler = new HttpVersionHandler()
                            {
                                InnerHandler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip }
                            }
                        }), false, null))
            {
                AuthenticationProvider = new GraphAuthenticationProvider(account)
            };
        }

        /// <summary>
        /// Creates a new instance of the <see cref="RestServiceClient" /> class used to communicate with a REST endpoint.
        /// </summary>
        /// <param name="requestData">The data used to request an access token.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An instance of the <see cref="RestServiceClient" /> class used to communicate with a REST endpoint.</returns>
        /// <exception cref="ArgumentNullException">
        /// The requestData parameter is null.
        /// </exception>
        public async Task<IRestServiceClient> CreateRestServiceClientAsync(TokenRequestData requestData, CancellationToken cancellationToken = default)
        {
            requestData.AssertNotNull(nameof(requestData));

            ModuleAuthenticationResult authResult = await ModuleSession.Instance.AuthenticationFactory.AcquireTokenAsync(
                requestData,
                null,
                cancellationToken).ConfigureAwait(false);

            return new RestServiceClient(new TokenCredentials(authResult.AccessToken, authResult.TokenType), httpClient, false);
        }
    }
}