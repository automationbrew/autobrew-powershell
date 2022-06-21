namespace AutoBrew.PowerShell.Models.Configuration
{
    /// <summary>
    /// Represents the known categories for the configurations.
    /// </summary>
    internal static class ConfigurationCategory
    {
        /// <summary>
        /// The category for all configurations that do not have a specific categorization.
        /// </summary>
        public const string Module = "Module";

        /// <summary>
        /// The category for all configurations related to telemetry.
        /// </summary>
        public const string Telemetry = "Telemetry";
    }
}