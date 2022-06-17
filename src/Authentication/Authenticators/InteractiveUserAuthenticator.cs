namespace AutoBrew.PowerShell.Authenticators
{
    using System.Net;
    using System.Net.Sockets;
    using Azure.Core;
    using Azure.Identity;
    using Microsoft.Rest;
    using Models;
    using Models.Authentication;
    using Models.Parameters;

    /// <summary>
    /// Authenticator that acquires an access token using an interactive approach.
    /// </summary>
    internal class InteractiveUserAuthenticator : IAuthenticator
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

            if (parameters is not InteractiveUserParameters)
            {
                return null;
            }

            InteractiveUserParameters interactiveParameters = parameters as InteractiveUserParameters;

            InteractiveBrowserCredentialOptions options = new()
            {
                AuthorityHost = new Uri(interactiveParameters.Environment.ActiveDirectoryAuthority),
                ClientId = interactiveParameters.Account.GetProperty(ExtendedPropertyType.ApplicationId),
                RedirectUri = new Uri($"http://localhost:{GetReplyUrlPort()}"),
                TenantId = interactiveParameters.Account.Tenant,
                TokenCachePersistenceOptions = parameters.TokenCacheProvider.GetPersistenceOptions()
            };

            InteractiveBrowserCredential browserCredential = new(options);

            TokenRequestContext requestContext = new(
                parameters.Scopes.ToArray(),
                null,
                AuthenticationConstants.MultiFactorAuthenticationClaim,
                parameters.Account.Tenant);

            Task<AuthenticationRecord> authRecordTask = browserCredential.AuthenticateAsync(
                requestContext,
                cancellationToken);

            ServiceClientTracing.Information($"{DateTime.Now:T} - [InteractiveUserAuthenticator] Calling AcquireTokenAsync with TenantId:'{options.TenantId}', AuthorityHost:'{options.AuthorityHost}', Scopes:'{string.Join(",", parameters.Scopes)}', RedirectUri:'{options.RedirectUri}'");

            return await ModuleAuthenticationResult.AcquireTokenAsync(
                authRecordTask,
                requestContext,
                browserCredential,
                parameters.IncludeRefreshToken,
                cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the TCP port to be used as part of the reply URL.
        /// </summary>
        /// <returns>The TCP port to be used as part of the reply URL.</returns>
        /// <exception cref="ModuleException">
        /// Unabled to find a TCP port between 8400 and 8999 that is available for use.
        /// </exception>
        private int GetReplyUrlPort()
        {
            TcpListener listener = null;
            int port = 8399;

            while (++port < 9000)
            {
                try
                {
                    listener = new TcpListener(IPAddress.Loopback, port);

                    listener.Start();
                    listener.Stop();

                    return port;
                }
                catch
                {
                    listener?.Stop();
                }
            }

            throw new ModuleException("Unabled to find a TCP port between 8400 and 8999 that is available for use.", ModuleExceptionCategory.Network);
        }
    }
}