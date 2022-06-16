namespace AutoBrew.PowerShell.Models.Configuration
{
    /// <summary>
    /// Represents the definition for a configuration.
    /// </summary>
    public abstract class ConfigurationDefinition
    {
        /// <summary>
        /// Gets the category for the configuration definition.
        /// </summary>
        public abstract string Category { get; }

        /// <summary>
        /// Gets the default value for the configuration definition.
        /// </summary>
        public abstract object DefaultValue { get; }

        /// <summary>
        /// Gets the description for the configuration definition. 
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Gets the key for the configuration definition.
        /// </summary>
        public abstract string Key { get; }

        /// <summary>
        /// Gets the type of the value for the configuration definition.
        /// </summary>
        public abstract Type ValueType { get; }

        /// <summary>
        /// Validates the value for the configuration definition.
        /// </summary>
        /// <param name="value">The value to be validated.</param>
        public virtual void Validate(object value)
        {
        }
    }
}