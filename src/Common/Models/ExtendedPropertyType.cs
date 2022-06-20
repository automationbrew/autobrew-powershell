namespace AutoBrew.PowerShell.Models
{
    /// <summary>
    /// Represents the supported types for the extended properties.
    /// </summary>
    public enum ExtendedPropertyType
    {
        /// <summary>
        /// Represents the type of extended property is an access token.
        /// </summary>
        AccessToken,

        /// <summary>
        /// Represents the type of extended property is an application identifier.
        /// </summary>
        ApplicationId,

        /// <summary>
        /// Represents the type of extended property is a home account identifier.
        /// </summary>
        HomeAccountId,

        /// <summary>
        /// Represents the type of extended property is a boolean that indicates the authorization code flow should be used.
        /// </summary>
        UseAuthorizationCode,

        /// <summary>
        /// Represents the type of extended property is a boolean that indicates the device code flow should be used.
        /// </summary>
        UseDeviceCode,

        /// <summary>
        /// Represent the type of extended property is a future or unknown value.
        /// </summary>
        UnknownFutureValue
    }
}