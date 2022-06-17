namespace AutoBrew.PowerShell.Credentials
{
    using System.Security;
    using System.Threading;
    using System.Threading.Tasks;
    using Azure.Core;
    using Azure.Identity;
    using Cache;
    using Factories;
    using Microsoft.Identity.Client;
    using Microsoft.Rest;
    using Models;
    using Models.Authentication;

    /// <summary>
    /// Provides the ability to request an access token from Azure Active Directory using the refresh token flow.
    /// </summary>
    internal class RefreshTokenCredential : TokenCredential
    {
        /// <summary>
        /// The client used to request access tokens from Azure Active Directory.
        /// </summary>
        private readonly IPublicClientApplication client;

        /// <summary>
        /// The refresh token to use when requesting an access token.
        /// </summary>
        private readonly SecureString refreshToken;

        /// <summary>
        /// The account information relating to an authentication request.
        /// </summary>
        private AuthenticationRecord record;

        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshTokenCredential" /> class.
        /// </summary>
        /// <param name="options">The options to be used for requesting an access token.</param>
        /// <param name="refreshToken">The refresh token to use when requesting an access token.</param>
        /// <exception cref="ArgumentNullException">
        /// The options parameter is null.
        /// or
        /// The refreshToken parameter is null.
        /// </exception>
        public RefreshTokenCredential(RefreshTokenCredentialOptions options, SecureString refreshToken)
        {
            options.AssertNotNull(nameof(options));
            refreshToken.AssertNotNull(nameof(refreshToken));

            client = PublicClientApplicationBuilder
                .Create(options.ClientId)
                .WithAuthority(options.AuthorityHost.AbsoluteUri, options.TenantId)
                .WithHttpClientFactory(MsalHttpClientFactory.GetInstance(MsalHttpClientFactoryType.RefreshToken))
                .WithLogging(LoggingCallback)
                .Build();

            if (ModuleSession.Instance.TryGetComponent(ComponentType.TokenCache, out TokenCacheProvider tokenCache))
            {
                tokenCache.RegisterCacheAsync(client.UserTokenCache).ConfigureAwait(false).GetAwaiter().GetResult();
            }

            this.refreshToken = refreshToken;
        }

        /// <summary>
        /// Acquires an access token from an existing refresh token.
        /// </summary>
        /// <param name="requestContext">The details of an authentication token request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An instance of the <see cref="AuthenticationRecord" /> class that represents the account information relating to an authentication.</returns>
        /// <exception cref="ArgumentNullException">
        /// The requestContext parameter is null.
        /// </exception>
        public async Task<AuthenticationRecord> AuthenticateAsync(TokenRequestContext requestContext, CancellationToken cancellationToken = default)
        {
            requestContext.AssertNotNull(nameof(requestContext));

            AuthenticationResult authResult = await (client as IByRefreshToken)
                .AcquireTokenByRefreshToken(requestContext.Scopes, refreshToken.AsString())
                .WithClaims(requestContext.Claims)
                .ExecuteAsync(cancellationToken)
                .ConfigureAwait(false);

            record = IdentityModelFactory.AuthenticationRecord(
                authResult.Account.Username,
                authResult.Account.Environment,
                authResult.Account.HomeAccountId.Identifier,
                authResult.TenantId,
                client.AppConfig.ClientId);

            return record;
        }

        /// <summary>
        /// Gets an access token from the security token service.
        /// </summary>
        /// <param name="requestContext">The request context for acquiring a token.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An access token that can be used to access protected services.</returns>
        public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken)
        {
            return GetTokenAsync(requestContext, cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Gets an access token from the security token service.
        /// </summary>
        /// <param name="requestContext">The request context for acquiring a token.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An access token that can be used to access protected services.</returns>
        public override async ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken = default)
        {
            requestContext.AssertNotNull(nameof(requestContext));

            AuthenticationResult authResult = await client
                .AcquireTokenSilent(requestContext.Scopes, (AuthenticationAccount)record)
                .WithClaims(requestContext.Claims)
                .ExecuteAsync(cancellationToken)
                .ConfigureAwait(false);

            return new AccessToken(authResult.AccessToken, authResult.ExpiresOn);
        }

        /// <summary>
        /// Callback that writes the specified message to the log.
        /// </summary>
        /// <param name="logLevel">The level for the log enty.</param>
        /// <param name="message">The message for the log entry.</param>
        /// <param name="isPiiLoggingEnabled">A flag indicating whether personal identifiable information logging is enabled.</param>
        private void LoggingCallback(LogLevel logLevel, string message, bool isPiiLoggingEnabled)
        {
            ServiceClientTracing.Information($"[MSAL] {logLevel} {message}");
        }
    }
}