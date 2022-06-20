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
        /// The endpoint of Microosft Graph for the Azure cloud environment.
        /// </summary>
        public const string MicrosoftGraphEndpoint = "https://graph.microsoft.com";
    }
}