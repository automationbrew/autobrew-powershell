namespace AutoBrew.PowerShell.Factories
{
    using System.Threading;
    using System.Threading.Tasks;
    using Authenticators;
    using Microsoft.Rest;
    using Models;
    using Models.Authentication;
    using Models.Parameters;
    using Network;

    /// <summary>
    /// Factory used to perform authentication requests.
    /// </summary>
    public sealed class AuthenticationFactory : IAuthenticationFactory
    {
        /// <summary>
        /// The identifier for the client used to request a bulk refresh token.
        /// </summary>
        private const string BulkRefreshTokenClientId = "1b730954-1685-4b74-9bfd-dac224a7b894";

        /// <summary>
        /// The scope for the access token request to access the bulk refresh token request.
        /// </summary>
        private const string BulkRefreshTokenScope = "urn:ms-drs:enterpriseregistration.windows.net/.default";

        /// <summary>
        /// The value for the common tenant.
        /// </summary>
        private const string CommonTenant = "organizations";

        /// <summary>
        /// The client used to perform HTTP operations.
        /// </summary>
        private static readonly HttpClient httpClient = new();

        /// <summary>
        /// Acquires a bulk refresh token from the authority.
        /// </summary>
        /// <param name="environment">The enivornment that will provide metadata used to acquire the bulk refresh token.</param>
        /// <param name="outputAction">The action that encapsulates the method used to write output.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An instance of the <see cref="BulkRefreshToken" /> class that represents the acquired bulk refresh.</returns>
        public async Task<BulkRefreshToken> AcquireBulkRefreshTokenAsync(ModuleEnvironment environment, Action<string> outputAction, CancellationToken cancellationToken = default)
        {
            ModuleAccount account = new()
            {
                AccountType = ModuleAccountType.User,
                Tenant = CommonTenant
            };

            account.SetProperty(ExtendedPropertyType.ApplicationId, BulkRefreshTokenClientId);
            account.SetProperty(ExtendedPropertyType.UseDeviceAuth, true.ToString());

            ModuleAuthenticationResult authResult = await AcquireTokenAsync(
                new TokenRequestData(
                    account,
                    environment,
                    new[] { BulkRefreshTokenScope }), outputAction, cancellationToken).ConfigureAwait(false);

            IRestServiceClient client = new RestServiceClient(
                new TokenCredentials(authResult.AccessToken, authResult.TokenType),
                httpClient,
                false);

            string packageId = Guid.NewGuid().ToString();

            BulkRefreshTokenResponse response = await client.PostAsync<BulkRefreshTokenRequest, BulkRefreshTokenResponse>(
                new Uri(environment.BulkRefreshTokenBeginEndpoint),
                new BulkRefreshTokenRequest
                {
                    DisplayName = $"package_{packageId}",
                    Expiration = DateTime.Now.AddDays(180).ToShortDateString(),
                    PackageId = packageId
                },
                cancellationToken);

            response = await client.GetAsync<BulkRefreshTokenResponse>(
                new Uri(environment.BulkRefreshTokenPollEndpoint),
                new Dictionary<string, string>() { { "flowToken", response.FlowToken } },
                cancellationToken);

            return new BulkRefreshToken(response);
        }

        /// <summary>
        /// Acquires an access token from the authority.
        /// </summary>
        /// <param name="requestData">The data that will be used as part of the authentication request.</param>
        /// <param name="outputAction">The action that encapsulates the method used to write output.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An instance of the <see cref="ModuleAuthenticationResult" /> class that represents the acquired access token.</returns>
        /// <exception cref="ArgumentNullException">
        /// The requestData parameter is null.
        /// </exception>
        public async Task<ModuleAuthenticationResult> AcquireTokenAsync(TokenRequestData requestData, Action<string> outputAction = null, CancellationToken cancellationToken = default)
        {
            requestData.AssertNotNull(nameof(requestData));

            AuthenticationParameters parameters = GetParameters(requestData, outputAction);
            ModuleAuthenticationResult result = await GetAuthenticator(parameters).AuthenticateAsync(parameters, cancellationToken);

            if (result != null)
            {
                if (string.IsNullOrEmpty(result.HomeAccountId) == false)
                {
                    requestData.Account.SetProperty(ExtendedPropertyType.HomeAccountId, result.HomeAccountId);
                }

                if (string.IsNullOrEmpty(requestData.Account.Username) && string.IsNullOrEmpty(result.Username) == false)
                {
                    requestData.Account.Username = result.Username;
                }

                if (requestData.Account.Tenant.Equals(CommonTenant, StringComparison.InvariantCultureIgnoreCase) &&
                    !string.IsNullOrEmpty(result.Tenant))
                {
                    requestData.Account.Tenant = result.Tenant;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the appropriate authenticator based on the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters for the authentication request.</param>
        /// <returns>The appropriate authenticator based on the specified parameters.</returns>
        /// <exception cref="ArgumentNullException">
        /// The parameters parameter is null.
        /// </exception>
        private IAuthenticator GetAuthenticator(AuthenticationParameters parameters)
        {
            parameters.AssertNotNull(nameof(parameters));

            if (parameters is DeviceCodeParameters)
            {
                return new DeviceCodeAuthenticator();
            }
            else if (parameters is InteractiveUserParameters)
            {
                return new InteractiveUserAuthenticator();
            }
            else if (parameters is RefreshTokenParameters)
            {
                return new RefreshTokenAuthenticator();
            }
            else if (parameters is SilentParameters)
            {
                return new SilentAuthenticator();
            }
            else if (parameters is UsernamePasswordParameters)
            {
                return new UsernamePasswordAuthenticator();
            }

            return new InteractiveUserAuthenticator();
        }

        /// <summary>
        /// Gets the authentication parameters based on the specified request data.
        /// </summary>
        /// <param name="requestData">The data that will be used as part of the authentication request.</param>
        /// <param name="outputAction">The action that encapsulates the method used to write output.</param>
        /// <returns>The authentication parameters based on the specified request data.</returns>
        private AuthenticationParameters GetParameters(TokenRequestData requestData, Action<string> outputAction)
        {
            requestData.AssertNotNull(nameof(requestData));

            if (requestData.Account.IsPropertySet(ExtendedPropertyType.UseAuthCode))
            {
                return new InteractiveUserParameters(requestData);
            }
            else if (requestData.Account.IsPropertySet(ExtendedPropertyType.UseDeviceAuth))
            {
                return new DeviceCodeParameters(requestData, outputAction);
            }
            else if (requestData.Account.AccountType == ModuleAccountType.User)
            {
                if (string.IsNullOrEmpty(requestData.Account.Username) == false && requestData.Password != null)
                {
                    return new UsernamePasswordParameters(requestData);
                }
                else if (requestData.RefreshToken != null)
                {
                    return new RefreshTokenParameters(requestData);
                }
                if (requestData.Account.IsPropertySet(ExtendedPropertyType.HomeAccountId))
                {
                    return new SilentParameters(requestData);
                }

                return new InteractiveUserParameters(requestData);
            }

            throw new ModuleException("Failed to resolve the authentication parameters.", ModuleExceptionCategory.Authentication);
        }
    }
}