namespace AutoBrew.PowerShell.Models.Configuration
{
    /// <summary>
    /// Represents a strongly typed definition for a configuration.
    /// </summary>
    /// <typeparam name="TValue">The type for the value of the configuration.</typeparam>
    public sealed class TypedConfigurationDefinition<TValue> : ConfigurationDefinition
    {
        /// <summary>
        /// Gets the category for the configuration definition.
        /// </summary>
        public override string Category { get; }

        /// <summary>
        /// Gets the default value for the configuration definition.
        /// </summary>
        public override object DefaultValue { get; }

        /// <summary>
        /// Gets the description for the configuration definition. 
        /// </summary>
        public override string Description { get; }

        /// <summary>
        /// Gets the key for the configuration definition.
        /// </summary>
        public override string Key { get; }

        /// <summary>
        /// Gets the type of the value for the configuration definition.
        /// </summary>
        public override Type ValueType => typeof(TValue);

        /// <summary>
        /// Initializes a new instance of the <see cref="TypedConfigurationDefinition{TValue}" /> class.
        /// </summary>
        /// <param name="category">The category for the configuration definition.</param>
        /// <param name="defaultValue">The default value for the configuration definition.</param>
        /// <param name="description">The description for the configuration definition.</param>
        /// <param name="key">The key for the configuration definition.</param>
        public TypedConfigurationDefinition(string category, TValue defaultValue, string description, string key)
        {
            category.AssertNotEmpty(nameof(category));
            defaultValue.AssertNotNull(nameof(defaultValue));
            description.AssertNotEmpty(nameof(description));
            key.AssertNotEmpty(nameof(key));

            Category = category;
            DefaultValue = defaultValue;
            Description = description;
            Key = key;
        }

        /// <summary>
        /// Validates the value for the configuration definition.
        /// </summary>
        /// <param name="value">The value to be validated.</param>
        /// <exception cref="ArgumentException">
        /// Unexpected value type [{value.GetType()}]. The value of the configuration [{Key}] should be of type [{ValueType}].
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// The value parameter is null.
        /// </exception>
        public override void Validate(object value)
        {
            value.AssertNotNull(nameof(value));

            if (value is not TValue)
            {
                throw new ArgumentException($"Unexpected value type [{value.GetType()}]. The value of the configuration [{Key}] should be of type [{ValueType}].", nameof(value));
            }
        }
    }
}