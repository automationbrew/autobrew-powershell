namespace AutoBrew.PowerShell.Authenticators
{
    using Azure.Core;
    using Azure.Identity;
    using Credentials;
    using Microsoft.Rest;
    using Models;
    using Models.Authentication;
    using Models.Parameters;

    /// <summary>
    /// Authenticator that acquires an access token using the refresh token flow.
    /// </summary>
    internal class RefreshTokenAuthenticator : IAuthenticator
    {
        /// <summary>
        /// Acquires an access token from the authority based on the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameter that will be used as part of the authentication request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>
        /// An instance of the <see cref="ModuleAuthenticationResult" /> class that represents the acquired access token.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The parameters parameter is null.
        /// </exception>
        public async Task<ModuleAuthenticationResult> AuthenticateAsync(AuthenticationParameters parameters, CancellationToken cancellationToken = default)
        {
            parameters.AssertNotNull(nameof(parameters));

            if (parameters is not RefreshTokenParameters)
            {
                return null;
            }

            RefreshTokenCredentialOptions options = new()
            {
                AuthorityHost = new Uri(parameters.Environment.ActiveDirectoryAuthority),
                ClientId = parameters.Account.GetProperty(ExtendedPropertyType.ApplicationId),
                TenantId = parameters.Account.Tenant,
                TokenCachePersistenceOptions = parameters.TokenCacheProvider.GetPersistenceOptions()
            };

            RefreshTokenCredential refreshTokenCredential = new(
                options,
                ((RefreshTokenParameters)parameters).RefreshToken);

            TokenRequestContext requestContext = new(
                parameters.Scopes.ToArray(),
                null,
                AuthenticationConstants.MultiFactorAuthenticationClaim,
                parameters.Account.Tenant);

            Task<AuthenticationRecord> authRecordTask = refreshTokenCredential.AuthenticateAsync(
                requestContext,
                cancellationToken);

            ServiceClientTracing.Information($"{DateTime.Now:T} - [RefreshTokenAuthenticator] Calling AcquireTokenAsync - TenantId:'{options.TenantId}', AuthorityHost:'{options.AuthorityHost}', Scopes:'{string.Join(",", parameters.Scopes)}'");

            return await ModuleAuthenticationResult.AcquireTokenAsync(
                authRecordTask,
                requestContext,
                refreshTokenCredential,
                parameters.IncludeRefreshToken,
                cancellationToken).ConfigureAwait(false);
        }
    }
}