namespace AutoBrew.PowerShell.Models.Authentication
{
    using System.Security;
    using Cache;

    /// <summary>
    /// Represents the data to be used when requesting an access token.
    /// </summary>
    public sealed class TokenRequestData
    {
        /// <summary>
        /// Gets or sets account details to be used when requesting an access token.
        /// </summary>
        public ModuleAccount Account { get; set; }

        /// <summary>
        /// Gets or sets the environment to be used when requesting an access token.
        /// </summary>
        public ModuleEnvironment Environment { get; set; }

        /// <summary>
        /// Gets or sets the flag that indicates the refresh token should be included in the response.
        /// </summary>
        public bool IncludeRefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the password to be used when requesting an access token.
        /// </summary>
        public SecureString Password { get; set; }

        /// <summary>
        /// Gets or sets the refresh token to be used when requesting an access token.
        /// </summary>
        public SecureString RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the scopes to be used when requesting an access token.
        /// </summary>
        public string[] Scopes { get; set; }

        /// <summary>
        /// Gets the token cache provider to be used when requesting an access token.
        /// </summary>
        public TokenCacheProvider TokenCacheProvider
        {
            get
            {
                if (ModuleSession.Instance.TryGetComponent(ComponentType.TokenCache, out TokenCacheProvider tokenCacheProvider) == false)
                {
                    throw new ModuleException("There is not a token cache provider registered.", ModuleExceptionCategory.Authentication);
                }

                return tokenCacheProvider;
            }
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="TokenRequestData" /> class.
        /// </summary>
        /// <param name="account">The account details to be used when requesting an access token.</param>
        /// <param name="environment">The environment to be used when requesting an access token.</param>
        /// <param name="scopes">The scopes to be used to when requesting an access token.</param>
        /// <exception cref="ArgumentNullException">
        /// The account parameter is null.
        /// or
        /// The environment parameter is null.
        /// or 
        /// The scopes parameter is null.
        /// </exception>
        public TokenRequestData(ModuleAccount account, ModuleEnvironment environment, string[] scopes)
        {
            account.AssertNotNull(nameof(account));
            environment.AssertNotNull(nameof(environment));
            scopes.AssertNotNull(nameof(scopes));

            Account = account;
            Environment = environment;
            Scopes = scopes;
        }
    }
}