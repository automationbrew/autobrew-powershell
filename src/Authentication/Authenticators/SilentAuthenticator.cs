namespace AutoBrew.PowerShell.Authenticators
{
    using System.Text;
    using System.Text.Json.Nodes;
    using Azure.Core;
    using Azure.Identity;
    using Microsoft.Rest;
    using Models;
    using Models.Authentication;
    using Models.Parameters;

    /// <summary>
    /// Authenticator that acquires an access token without user interaction.
    /// </summary>
    internal class SilentAuthenticator : IAuthenticator
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

            if (parameters is not SilentParameters)
            {
                return null;
            }

            SharedTokenCacheCredentialOptions options = new(parameters.TokenCacheProvider.GetPersistenceOptions())
            {
                AuthorityHost = new Uri(parameters.Environment.ActiveDirectoryAuthority),
                ClientId = parameters.Account.GetProperty(ExtendedPropertyType.ApplicationId),
                EnableGuestTenantAuthentication = true,
                TenantId = parameters.Account.Tenant,
                Username = parameters.Account.Username
            };

            SharedTokenCacheCredential tokenCacheCredential = new(options);

            JsonObject recordObject = new()
            {
                ["clientId"] = parameters.Account.GetProperty(ExtendedPropertyType.ApplicationId),
                ["homeAccountId"] = parameters.Account.GetProperty(ExtendedPropertyType.HomeAccountId),
                ["tenantId"] = parameters.Account.Tenant,
                ["username"] = parameters.Account.Username
            };

            using MemoryStream stream = new(Encoding.UTF8.GetBytes(recordObject.ToJsonString()));

            Task<AuthenticationRecord> authRecordTask = AuthenticationRecord.DeserializeAsync(stream, cancellationToken);

            ServiceClientTracing.Information($"{DateTime.Now:T} - [SilentAuthenticator] Calling AcquireTokenAsync - TenantId:'{options.TenantId}', AuthorityHost:'{options.AuthorityHost}', Scopes:'{string.Join(",", parameters.Scopes)}'");

            return await ModuleAuthenticationResult.AcquireTokenAsync(
                authRecordTask,
                new TokenRequestContext(parameters.Scopes.ToArray(), null, null, parameters.Account.Tenant),
                tokenCacheCredential,
                parameters.IncludeRefreshToken,
                cancellationToken).ConfigureAwait(false);
        }
    }
}