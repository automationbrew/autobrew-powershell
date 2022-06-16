namespace AutoBrew.PowerShell.Models
{
    /// <summary>
    /// Represents the type of components that can be registered.
    /// </summary>
    public enum ComponentType
    {
        /// <summary>
        /// Represents the type of component is the configuration configuration runtime.
        /// </summary>
        Configuration,

        /// <summary>
        /// Represents the type of component is a token cache.
        /// </summary>
        TokenCache,

        /// <summary>
        /// Represents the type of component that is a future value or unknown.
        /// </summary>
        UnknownFutureValue
    }
}