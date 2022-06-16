namespace AutoBrew.PowerShell.Credentials
{
    using Azure.Identity;

    /// <summary>
    /// Represents the options to configure requests made to the identity service.
    /// </summary>
    internal sealed class RefreshTokenCredentialOptions : TokenCredentialOptions
    {
        /// <summary>
        /// Gets or sets the identifier for the client used when requesting a token.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the Azure Active Directory tenant identifier for the tenant.
        /// </summary>
        public string Tenant { get; set; }

        /// <summary>
        /// Gets or sets the persistence options for the token cache.
        /// </summary>
        public TokenCachePersistenceOptions TokenCachePersistenceOptions { get; set; }
    }
}