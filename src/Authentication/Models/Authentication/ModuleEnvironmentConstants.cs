namespace AutoBrew.PowerShell.Models.Authentication
{
    /// <summary>
    /// Represents the constant values used to populate the environment properties.
    /// </summary>
    public sealed class ModuleEnvironmentConstants
    {
        /// <summary>
        /// The endpoint for the Azure Active Directory authority.
        /// </summary>
        public const string AzureActiveDirectoryAuthority = "https://login.microsoftonline.com/";

        /// <summary>
        /// The endpoint for the begin bulk refresh token request in the commerical cloud.
        /// </summary>
        public const string BulkRefreshTokenBeginEndpoint = "https://login.microsoftonline.com/webapp/bulkaadjtoken/begin";

        /// <summary>
        /// The endpoint for the poll bulk refresh token request in the commerical cloud.
        /// </summary>
        public const string BulkRefreshTokenPollEndpoint = "https://login.microsoftonline.com/webapp/bulkaadjtoken/poll";

        /// <summary>
        /// The endpoint for Microsoft Graph in the commerical cloud.
        /// </summary>
        public const string MicrosoftGraphEndpoint = "https://graph.microsoft.com";
    }
}