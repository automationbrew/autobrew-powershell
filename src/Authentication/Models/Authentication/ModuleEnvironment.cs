namespace AutoBrew.PowerShell.Models.Authentication
{
    using System.Collections.Concurrent;

    /// <summary>
    /// Represents environment details use for authentication.
    /// </summary>
    public sealed class ModuleEnvironment
    {
        /// <summary>
        /// Gets or sets the authentication endpoint.
        /// </summary>
        public string ActiveDirectoryAuthority { get; set; }

        /// <summary>
        /// Gets or sets the bulk refresh token begin request endpoint.
        /// </summary>
        public string BulkRefreshTokenBeginEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the bulk refresh token poll request endpoint.
        /// </summary>
        public string BulkRefreshTokenPollEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the environment name.
        /// </summary>
        public ModuleEnvironmentName EnvironmentName { get; set; }

        /// <summary>
        /// Gets a collection of known environments for use by the module.
        /// </summary>
        public static IDictionary<ModuleEnvironmentName, ModuleEnvironment> KnownEnvironments { get; } = InitializeEnvironments();

        /// <summary>
        /// Gets or sets the endpoint for Microsoft Graph.
        /// </summary>
        public string MicrosoftGraphEndpoint { get; set; }

        /// <summary>
        /// Initializes the known environments for use by the module.
        /// </summary>
        /// <returns>A collection of known environments for use by the module.</returns>
        private static IDictionary<ModuleEnvironmentName, ModuleEnvironment> InitializeEnvironments()
        {
            return new ConcurrentDictionary<ModuleEnvironmentName, ModuleEnvironment>
            {
                [ModuleEnvironmentName.Public] = new ModuleEnvironment
                {
                    ActiveDirectoryAuthority = ModuleEnvironmentConstants.AzureActiveDirectoryAuthority,
                    BulkRefreshTokenBeginEndpoint = ModuleEnvironmentConstants.BulkRefreshTokenBeginEndpoint,
                    BulkRefreshTokenPollEndpoint = ModuleEnvironmentConstants.BulkRefreshTokenPollEndpoint,
                    EnvironmentName = ModuleEnvironmentName.Public,
                    MicrosoftGraphEndpoint = ModuleEnvironmentConstants.MicrosoftGraphEndpoint
                }
            };
        }
    }
}