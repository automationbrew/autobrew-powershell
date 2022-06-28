namespace AutoBrew.PowerShell.Network
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Microsoft.Graph;
    using Models;
    using Models.Authentication;

    /// <summary>
    /// Handles the authentication request for interacting with Microsoft Graph.
    /// </summary>
    public class GraphAuthenticationProvider : IAuthenticationProvider
    {
        /// <summary>
        /// The authentication scheme used when interacting with Microsoft Graph.
        /// </summary>
        private const string AuthenticationScheme = "Bearer";

        /// <summary>
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </summary>
        private readonly CancellationToken cancellationToken;

        /// <summary>
        /// An instance of <see cref="ModuleAccount" /> that contains values that will be used for authentication.
        /// </summary>
        private readonly ModuleAccount account;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphAuthenticationProvider" /> class.
        /// </summary>
        /// <param name="account">An aptly populated instance of <see cref="ModuleAccount" /> that will be used for authentication.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public GraphAuthenticationProvider(ModuleAccount account, CancellationToken cancellationToken = default)
        {
            account.AssertNotNull(nameof(account));

            this.account = account;
            this.cancellationToken = cancellationToken;
        }

        /// <summary>
        /// Adds the authorization headers to the specified instance of <see cref="HttpRequestMessage" />.
        /// </summary>
        /// <param name="request">An instance of <see cref="HttpRequestMessage" /> where the authorization header should be added.</param>
        /// <returns>An instance of the <see cref="Task" /> class that represents the asynchronous operation.</returns>
        public async Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            ModuleAuthenticationResult authResult = await ModuleSession.Instance.AuthenticationFactory.AcquireTokenAsync(
                new TokenRequestData(account, ModuleSession.Instance.Context.Environment, new[] { $"{ModuleSession.Instance.Context.Environment.MicrosoftGraphEndpoint}/.default" }),
                null, cancellationToken).ConfigureAwait(false);

            request.Headers.Authorization = new AuthenticationHeaderValue(AuthenticationScheme, authResult.AccessToken);
        }
    }
}