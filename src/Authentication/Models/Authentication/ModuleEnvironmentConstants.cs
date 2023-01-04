namespace AutoBrew.PowerShell.Models.Authentication
{
    /// <summary>
    /// Provides constants that are used to populate known values used by environments.
    /// </summary>
    internal static class ModuleEnvironmentConstants
    {
        /// <summary>
        /// The authority for the Azure cloud environment.
        /// </summary>
        public const string ActiveDirectoryAuthority = "https://login.microsoftonline.com/";

        /// <summary>
        /// The default identifier for the application used when requesting an access token.
        /// </summary>
        public const string ApplicationId = "04b07795-8ddb-461a-bbee-02f9e1bf7b46";

        /// <summary>
        /// The name for the Azure cloud environment.
        /// </summary>
        public const string AzureCloud = "AzureCloud";

        /// <summary>
        /// The relative URI for to begin a request for a bulk refresh token.
        /// </summary>
        public const string BulkRefreshTokenBeginUri = "webapp/bulkaadjtoken/begin";

        /// <summary>
        /// The relative URI for to poll for a for a bulk refresh token.
        /// </summary>
        public const string BulkRefreshTokenPollUri = "webapp/bulkaadjtoken/poll";

        /// <summary>
        /// The default scope value used when requesting an access token.
        /// </summary>
        public const string DefaultScope = ".default";

        /// <summary>
        /// The endpoint of Microsoft Graph for the Azure cloud environment.
        /// </summary>
        public const string MicrosoftGraphEndpoint = "https://graph.microsoft.com";

        /// <summary>
        /// The endpoint of Microsoft Partner Center for the Azure cloud environment.
        /// </summary>
        public const string MicrosoftPartnerCenterEndpoint = "https://api.partnercenter.microsoft.com";

        /// <summary>
        /// The tenant used when requesting an access token.
        /// </summary>
        public const string Tenant = "organizations";
    }
}