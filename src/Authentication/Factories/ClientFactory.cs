namespace AutoBrew.PowerShell.Factories
{
    using System.Net;
    using AutoBrew.PowerShell.Models;
    using Microsoft.Graph;
    using Microsoft.Store.PartnerCenter;
    using Microsoft.Store.PartnerCenter.Extensions;
    using Models.Authentication;
    using Network;

    /// <summary>
    /// The factory used to create clients used to communicate with online services.
    /// </summary>
    public class ClientFactory : IClientFactory
    {
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
        /// Creates a new instance of the <see cref="PartnerOperations" /> class used to communicate with Partner Center.
        /// </summary>
        /// <param name="account">An instance of the <see cref="ModuleAccount" /> class that provides information used to authenticate.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An instance of the <see cref="PartnerOperations" /> class used to communicate with partner Center.</returns>
        public async Task<IPartner> CreatePartnerOperationsAsync(ModuleAccount account, CancellationToken cancellationToken = default)
        {
            ModuleAuthenticationResult authResult = await ModuleSession.Instance.AuthenticationFactory.AcquireTokenAsync(
                new TokenRequestData(
                    account,
                    ModuleSession.Instance.Context.Environment,
                    new[] { $"{ModuleSession.Instance.Context.Environment.MicrosoftPartnerCenterEndpoint}/user_impersonation" }),
                null,
                cancellationToken).ConfigureAwait(false);

            IPartnerCredentials credentials = await PartnerCredentials.Instance.GenerateByUserCredentialsAsync(
                ModuleSession.Instance.Context.Environment.ApplicationId,
                new AuthenticationToken(authResult.AccessToken, authResult.ExpiresOn),
                async (AuthenticationToken token) =>
                {
                    ModuleAuthenticationResult renewedAuthResult = await ModuleSession.Instance.AuthenticationFactory.AcquireTokenAsync(
                        new TokenRequestData(
                        account,
                        ModuleSession.Instance.Context.Environment,
                        new[] { $"{ModuleSession.Instance.Context.Environment.MicrosoftPartnerCenterEndpoint}/user_impersonation" }),
                        null,
                        cancellationToken).ConfigureAwait(false);

                    return new AuthenticationToken(renewedAuthResult.AccessToken, renewedAuthResult.ExpiresOn);
                }).ConfigureAwait(false);

            return PartnerService.Instance.CreatePartnerOperations(credentials);
        }
    }
}