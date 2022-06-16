namespace AutoBrew.PowerShell.Models.Configuration
{
    /// <summary>
    /// Represents the metadata for a specific configuration.
    /// </summary>
    public sealed class ConfigurationData
    {
        /// <summary>
        /// Gets the definition for the configuration.
        /// </summary>
        public ConfigurationDefinition Definition { get; }

        /// <summary>
        /// Gets the scope for the configuration.
        /// </summary>
        public ConfigurationScope Scope { get; }

        /// <summary>
        /// Gets the value for the configuration.
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationData" /> class.
        /// </summary>
        /// <param name="definition">The definition for the configuration.</param>
        /// <param name="scope">The scope for the configuration.</param>
        /// <param name="value">The value for the configuration.</param>
        /// <exception cref="ArgumentNullException">
        /// The definition parameter is null.
        /// or
        /// The scope parameter is null.
        /// or
        /// The value parameter is null.
        /// </exception>
        public ConfigurationData(ConfigurationDefinition definition, ConfigurationScope scope, object value)
        {
            definition.AssertNotNull(nameof(definition));
            scope.AssertNotNull(nameof(scope));
            value.AssertNotNull(nameof(value));

            Definition = definition;
            Scope = scope;
            Value = value;
        }
    }
}