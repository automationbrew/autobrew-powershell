namespace AutoBrew.PowerShell.Configuration
{
    using Models.Configuration;

    /// <summary>
    /// Represents a provider for configurations used throughout the module.
    /// </summary>
    public interface IConfigurationProvider
    {
        /// <summary>
        /// Gets the value for the configuration with the specified key.
        /// </summary>
        /// <typeparam name="TValue">Tye type for the value of the configuration.</typeparam>
        /// <param name="key">The key for the configuration.</param>
        /// <returns>
        /// The value for the configuration with the specified key if defined; otherwise, the default value for the configuration.
        /// </returns>
        (Type ProviderType, TValue Value) GetConfigurationValue<TValue>(string key);

        /// <summary>
        /// Gets the list of configurations that have been defined.
        /// </summary>
        /// <returns>The list of configuations that have been defined.</returns>
        IList<ConfigurationData> ListConfiguration();

        /// <summary>
        /// Builds the configuration from the set of registered sources.
        /// </summary>
        void Build();

        /// <summary>
        /// Registers the configuration definition for use throughout the module.
        /// </summary>
        /// <param name="definition">The defintion of the configuration to be registered.</param>
        void RegisterDefinition(ConfigurationDefinition definition);

        /// <summary>
        /// Updates the specified configuration.
        /// </summary>
        /// <typeparam name="TValue">The type for the value of the configuration.</typeparam>
        /// <param name="key">The key for the configuration.</param>
        /// <param name="scope">The scope for the configuration.</param>
        /// <param name="value">The value for the configuration.</param>
        void UpdateConfiguration<TValue>(string key, ConfigurationScope scope, TValue value);
    }
}