namespace AutoBrew.PowerShell.Models.Configuration
{
    /// <summary>
    /// Represents a configuration used by the module.
    /// </summary>
    public sealed class PSConfiguration
    {
        /// <summary>
        /// Gets the category for the configuration.
        /// </summary>
        public string Category { get; }

        /// <summary>
        /// Gets the default value for the configuration.
        /// </summary>
        public object DefaultValue { get; }

        /// <summary>
        /// Gets the description for the configuration.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the key for the configuration.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Gets the scope for the configuration.
        /// </summary>
        public ConfigurationScope Scope { get; }

        /// <summary>
        /// Gets the value for the configuration.
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PSConfiguration" /> class.
        /// </summary>
        /// <param name="data">An instance of the <see cref="ConfigurationData" /> that provides information for the configuration.</param>
        /// <exception cref="ArgumentNullException">
        /// The data parameter is null.
        /// </exception>
        public PSConfiguration(ConfigurationData data)
        {
            data.AssertNotNull(nameof(data));

            Category = data.Definition.Category;
            DefaultValue = data.Definition.DefaultValue;
            Description = data.Definition.Description;
            Key = data.Definition.Key;
            Scope = data.Scope;
            Value = data.Value;
        }
    }
}