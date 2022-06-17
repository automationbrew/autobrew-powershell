namespace AutoBrew.PowerShell.Authenticators
{
    using Azure.Core;
    using Azure.Identity;
    using Microsoft.Rest;
    using Models;
    using Models.Authentication;
    using Models.Parameters;

    /// <summary>
    /// Authenticator that acquires an access token using the device code flow.
    /// </summary>
    internal class DeviceCodeAuthenticator : IAuthenticator
    {
        /// <summary>
        /// The action that handles the output for the device code callback.
        /// </summary>
        private Action<string> deviceCodeCallbackOutput;

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

            if (parameters is not DeviceCodeParameters)
            {
                return null;
            }

            deviceCodeCallbackOutput = ((DeviceCodeParameters)parameters).DeviceCodeCallbackOutput;

            DeviceCodeCredentialOptions options = new()
            {
                AuthorityHost = new Uri(parameters.Environment.ActiveDirectoryAuthority),
                ClientId = parameters.Account.GetProperty(ExtendedPropertyType.ApplicationId),
                DeviceCodeCallback = DeviceCodeCallback,
                TenantId = parameters.Account.Tenant,
                TokenCachePersistenceOptions = parameters.TokenCacheProvider.GetPersistenceOptions()
            };

            DeviceCodeCredential deviceCodeCredential = new(options);
            TokenRequestContext requestContext = new(
                parameters.Scopes.ToArray(),
                null,
                AuthenticationConstants.MultiFactorAuthenticationClaim,
                parameters.Account.Tenant);

            Task<AuthenticationRecord> authTask = deviceCodeCredential.AuthenticateAsync(requestContext, cancellationToken);

            ServiceClientTracing.Information($"{DateTime.Now:T} - [DeviceCodeAuthenticator] Calling AcquireTokenAsync - TenantId:'{options.TenantId}', AuthorityHost:'{options.AuthorityHost}', Scopes:'{string.Join(",", parameters.Scopes)}'");

            return await ModuleAuthenticationResult.AcquireTokenAsync(
                authTask,
                requestContext,
                deviceCodeCredential,
                parameters.IncludeRefreshToken,
                cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Write the device code message prompt to the console.
        /// </summary>
        /// <param name="info">Details of the device code to present to a user to allow them to authenticate through the device code authentication flow.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An instance of the <see cref="Task" /> class that represents the a task that has completed successfully.</returns>
        private Task DeviceCodeCallback(DeviceCodeInfo info, CancellationToken cancellationToken = default)
        {
            deviceCodeCallbackOutput(info.Message);

            return Task.CompletedTask;
        }
    }
}