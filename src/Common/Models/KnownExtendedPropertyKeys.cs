namespace AutoBrew.PowerShell.Models
{
    /// <summary>
    /// Represents the known keys for extended properties.
    /// </summary>
    public static class KnownExtendedPropertyKeys
    {
        /// <summary>
        /// The key that represents the extended property is an access token.
        /// </summary>
        public const string AccessToken = "AccessToken";

        /// <summary>
        /// The key that represents the extended property is an identifier for an Azure Active Directory application.
        /// </summary>
        public const string ApplicationId = "ApplicationId";

        /// <summary>
        /// The key that represents the extended property is an identifier for an Azure subscription.
        /// </summary>
        public const string AzureSubscription = "AzureSubscription";

        /// <summary>
        /// The key that represents the extended property is the name of the DevTest Lab.
        /// </summary>
        public const string DevTestLabName = "DevTestLabName";

        /// <summary>
        /// The key that represents the extended property is the identifier for the home account.
        /// </summary>
        public const string HomeAccountId = "HomeAccountId";

        /// <summary>
        /// The key that represents the extended property is the name of the resource group.
        /// </summary>
        public const string ResourceGroupName = "ResourceGroupName";

        /// <summary>
        /// The key that represents the extended property is a boolean value that indicates the authorization code flow should be used for authentication.
        /// </summary>
        public const string UseAuthorizationCode = "UseAuthorizationCode";

        /// <summary>
        /// The key that represents the extended property is a boolean value that indicates the device code flow should be used for authentication.
        /// </summary>
        public const string UseDeviceCode = "UseDeviceCode";
    }
}