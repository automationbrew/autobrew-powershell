namespace AutoBrew.PowerShell.Models.Authentication
{
    using System.Collections.Concurrent;

    /// <summary>
    /// Represents the metadata for an environment for the module.
    /// </summary>
    public sealed class ModuleEnvironment : IExtensibleModel
    {
        /// <summary>
        /// Gets or sets the Active Directory authority for the environment.
        /// </summary>
        public string ActiveDirectoryAuthority { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the application for the environment.
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        /// Gets the extended properties for the environment.
        /// </summary>
        public IDictionary<string, string> ExtendedProperties { get; private set; } = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// Gets or sets the Microsoft Graph endpoint for the environment.
        /// </summary>
        public string MicrosoftGraphEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the name for the environment.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the tenant for the environment.
        /// </summary>
        public string Tenant { get; set; }

        /// <summary>
        /// Gets or sets the type for the enviornment.
        /// </summary>
        public ModuleEnvironmentType Type { get; set; }
    }
}