namespace AutoBrew.PowerShell.Authenticators
{
    using Azure.Core;
    using Azure.Identity;
    using Microsoft.Rest;
    using Models;
    using Models.Authentication;
    using Models.Parameters;

    /// <summary>
    /// Authenticator that acquires an access token using the username/password flow.
    /// </summary>
    internal class UsernamePasswordAuthenticator : IAuthenticator
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

            if (parameters is not UsernamePasswordParameters)
            {
                return null;
            }

            UsernamePasswordParameters usernamePasswordParameters = parameters as UsernamePasswordParameters;

            UsernamePasswordCredentialOptions options = new()
            {
                AuthorityHost = new Uri(parameters.Environment.ActiveDirectoryAuthority),
                TokenCachePersistenceOptions = parameters.TokenCacheProvider.GetPersistenceOptions()
            };

            UsernamePasswordCredential usernamePasswordCredential = new(
                usernamePasswordParameters.Account.Username,
                usernamePasswordParameters.Password.AsString(),
                usernamePasswordParameters.Account.Tenant,
                usernamePasswordParameters.Account.GetProperty(ExtendedPropertyType.ApplicationId),
                options);

            TokenRequestContext requestContext = new(
                parameters.Scopes.ToArray(),
                null,
                null,
                parameters.Account.Tenant);

            Task<AuthenticationRecord> authRecordTask = usernamePasswordCredential.AuthenticateAsync(
                requestContext,
                cancellationToken);

            ServiceClientTracing.Information($"{DateTime.Now:T} - [UsernamePasswordAuthenticator] Calling AuthenticateAsync - TenantId:'{parameters.Account.Tenant}', Scopes:'{string.Join(",", parameters.Scopes)}', AuthorityHost:'{parameters.Environment.ActiveDirectoryAuthority}', Username:'{usernamePasswordParameters.Account.Username}'");

            return await ModuleAuthenticationResult.AcquireTokenAsync(
                authRecordTask,
                requestContext,
                usernamePasswordCredential,
                parameters.IncludeRefreshToken,
                cancellationToken).ConfigureAwait(false);
        }
    }
}