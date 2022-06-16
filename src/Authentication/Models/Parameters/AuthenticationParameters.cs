namespace AutoBrew.PowerShell.Models.Parameters
{
    using Authentication;
    using Cache;

    /// <summary>
    /// Represents the parameters used for authentication requests.
    /// </summary>
    internal abstract class AuthenticationParameters
    {
        /// <summary>
        /// Get the account details to be used for the authentication request.
        /// </summary>
        public ModuleAccount Account { get; }

        /// <summary>
        /// Gets the environment details to be used for the authentication request.
        /// </summary>
        public ModuleEnvironment Environment { get; }

        /// <summary>
        /// Gets or sets the flag that indicates the refresh token should be included in the response.
        /// </summary>
        public bool IncludeRefreshToken { get; set; }

        /// <summary>
        /// Gets the scopes to be used for the authentication request.
        /// </summary>
        public IEnumerable<string> Scopes { get; }

        /// <summary>
        /// Gets the the token cache provider to be used for the authentication request.
        /// </summary>
        public TokenCacheProvider TokenCacheProvider { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationParameters" /> class.
        /// </summary>
        /// <param name="requestData">The request data to be used when requesting an access token.</param>
        /// <exception cref="ArgumentNullException">
        /// The requestData parameter is null.
        /// </exception>
        protected AuthenticationParameters(TokenRequestData requestData)
        {
            requestData.AssertNotNull(nameof(requestData));

            Account = requestData.Account;
            Environment = requestData.Environment;
            IncludeRefreshToken = requestData.IncludeRefreshToken;
            Scopes = requestData.Scopes;
            TokenCacheProvider = requestData.TokenCacheProvider;
        }
    }
}