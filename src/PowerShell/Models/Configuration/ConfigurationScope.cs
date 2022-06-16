namespace AutoBrew.PowerShell.Models.Configuration
{
    /// <summary>
    /// Represents the scope for the configuration.
    /// </summary>
    public enum ConfigurationScope
    {
        /// <summary>
        /// Represents the scope for the configuration is the current user which means the configuration will be persisted.
        /// </summary>
        CurrentUser,

        /// <summary>
        /// Represents the scope for the configuration is the default scope.
        /// </summary>
        Default,

        /// <summary>
        /// Represents the scope for the configuration is the current PowerShell session and it will not be persisted.
        /// </summary>
        Process
    }
}