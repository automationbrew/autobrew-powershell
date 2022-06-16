namespace AutoBrew.PowerShell.Models.Authentication
{
    /// <summary>
    /// Represents the type of account used for authentication.
    /// </summary>
    public enum ModuleAccountType
    {
        /// <summary>
        /// Represents an account type where an access token was used for authentication.
        /// </summary>
        AccessToken,

        /// <summary>
        /// Represents an account type where a user account was used for authentication.
        /// </summary>
        User,

        /// <summary>
        /// Represent an account type that is a future value or unknown.
        /// </summary>
        UnknownFutureValue
    }
}
