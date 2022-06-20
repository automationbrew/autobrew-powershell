namespace AutoBrew.PowerShell.Models.Authentication
{
    /// <summary>
    /// Represents the metadata for an environment for the module.
    /// </summary>
    public sealed class ModuleEnvironment
    {
        /// <summary>
        /// Gets or sets the Active Directory authority for the environment.
        /// </summary>
        public string ActiveDirectoryAuthority { get; set; }

        /// <summary>
        /// Gets or sets the Microsoft Graph endpoint for the environment.
        /// </summary>
        public string MicrosoftGraphEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the name for the environment.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type for the enviornment.
        /// </summary>
        public ModuleEnvironmentType Type { get; set; }
    }
}