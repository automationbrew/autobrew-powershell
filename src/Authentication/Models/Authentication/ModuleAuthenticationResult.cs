namespace AutoBrew.PowerShell.Models.Authentication
{
    using System.Security;
    using Azure.Core;
    using Azure.Identity;
    using Cache;
    using Microsoft.Identity.Client;

    /// <summary>
    /// Represents the authentication results from a request for an access token.
    /// </summary>
    public sealed class ModuleAuthenticationResult
    {
        /// <summary>
        /// The type of token obtained from the authority.
        /// </summary>
        private const string DefaultTokenTYpe = "Bearer";

        /// <summary>
        /// Gets the access token for the authentication result.
        /// </summary>
        public string AccessToken { get; }

        /// <summary>
        /// Gets the expires on value for the authentication result.
        /// </summary>
        public DateTimeOffset ExpiresOn { get; }

        /// <summary>
        /// Gets the home account identifier for the authentication result.
        /// </summary>
        public string HomeAccountId { get; }

        /// <summary>
        /// Gets the refresh token for the authentication result.
        /// </summary>
        public SecureString RefreshToken { get; }

        /// <summary>
        /// Gets the tenant for the authentication result.
        /// </summary>
        public string Tenant { get; }

        /// <summary>
        /// Gets the type of token for the authentication result.
        /// </summary>
        public string TokenType { get; }

        /// <summary>
        /// Gets the username for the authentication result.
        /// </summary>
        public string Username { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleAuthenticationResult" /> class.
        /// </summary>
        /// <param name="accessToken">The bearer token obtained from the authority.</param>
        /// <exception cref="ArgumentNullException">
        /// The accessToken parameter is null.
        /// </exception>
        public ModuleAuthenticationResult(AccessToken accessToken)
        {
            accessToken.AssertNotNull(nameof(accessToken));

            AccessToken = accessToken.Token;
            ExpiresOn = accessToken.ExpiresOn;
            TokenType = DefaultTokenTYpe;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleAuthenticationResult" /> class.
        /// </summary>
        /// <param name="authRecord">The account information relating to the authentication request.</param>
        /// <param name="refreshToken">the refresh token for the authentication result.</param>
        /// <param name="token">The bearer token obtained from the authority.</param>
        /// <exception cref="ArgumentNullException">
        /// The authRecord parameter is null.
        /// or
        /// The accessToken parameter is null.
        /// </exception>
        public ModuleAuthenticationResult(AuthenticationRecord authRecord, SecureString refreshToken, AccessToken token)
        {
            authRecord.AssertNotNull(nameof(authRecord));
            token.AssertNotNull(nameof(token));

            AccessToken = token.Token;
            ExpiresOn = token.ExpiresOn;
            HomeAccountId = authRecord.HomeAccountId;
            RefreshToken = refreshToken;
            Tenant = authRecord.TenantId;
            TokenType = DefaultTokenTYpe;
            Username = authRecord.Username;
        }

        /// <summary>
        /// Acquires an access token from the authority.
        /// </summary>
        /// <param name="authenticationTask">The task to request account information.</param>
        /// <param name="requestContext">The details of an authentication token request.</param>
        /// <param name="tokenCredential">The credential capable of requesting a token.</param>
        /// <param name="includeRefreshToken">The flag that indicates the refresh token should be included in the response.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An instance of the <see cref="ModuleAuthenticationResult" /> that represents the authentication result.</returns>
        internal static async Task<ModuleAuthenticationResult> AcquireTokenAsync(Task<AuthenticationRecord> authenticationTask, TokenRequestContext requestContext, TokenCredential tokenCredential, bool includeRefreshToken, CancellationToken cancellationToken = default)
        {
            AuthenticationRecord authRecord = await authenticationTask.ConfigureAwait(false);
            AccessToken token = await tokenCredential.GetTokenAsync(requestContext, cancellationToken).ConfigureAwait(false);
            SecureString refreshToken = null;

            if (includeRefreshToken)
            {
                refreshToken = await GetRefreshTokenAsync(authRecord.ClientId, authRecord.HomeAccountId).ConfigureAwait(false);
            }

            return new ModuleAuthenticationResult(authRecord, refreshToken, token);
        }

        /// <summary>
        /// Gets the refresh token associated with the specified client and home account.
        /// </summary>
        /// <param name="clientId">The identifier for the client used to request the access token.</param>
        /// <param name="homeAccountId">The identifier of the home account for the user.</param>
        /// <returns>The refresh token associated with the specified client and home account if discovered; otherwise, null.</returns>
        /// <exception cref="ArgumentException">
        /// The clientId parameter is empty or null.
        /// or
        /// The homeAccountId parameter is empty or null.
        /// </exception>
        private static async Task<SecureString> GetRefreshTokenAsync(string clientId, string homeAccountId)
        {
            clientId.AssertNotEmpty(nameof(clientId));
            homeAccountId.AssertNotEmpty(nameof(homeAccountId));

            IPublicClientApplication client = PublicClientApplicationBuilder.Create(clientId).Build();

            if (ModuleSession.Instance.TryGetComponent(ComponentType.TokenCache, out TokenCacheProvider tokenCache))
            {
                await tokenCache.RegisterCacheAsync(client.UserTokenCache).ConfigureAwait(false);
            }

            return tokenCache == null ? null : await tokenCache.GetRefreshTokenAsync(clientId, homeAccountId).ConfigureAwait(false);
        }
    }
}
