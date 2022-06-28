namespace AutoBrew.PowerShell.Factories
{
    using System.Net;
    using Microsoft.Graph;
    using Models.Authentication;
    using Network;

    /// <summary>
    /// The factory used to create clients used to communicate with online services.
    /// </summary>
    public class ClientFactory : IClientFactory
    {
        /// <summary>
        /// Creates a new instance of the <see cref="GraphServiceClient" /> used to communicate with Microsoft Graph.
        /// </summary>
        /// <param name="account">An instance of the <see cref="ModuleAccount" /> class that provides information used to authenticate.</param>
        /// <returns>An instance of the <see cref="GraphServiceClient" /> used to communicate with Microsoft Graph.</returns>
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
    }
}