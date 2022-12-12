namespace AutoBrew.PowerShell
{
    using System.Text;
    using Properties;

    /// <summary>
    /// Attribute used by classes that inherit the <see cref="Commands.ModuleCmdlet" /> class to define a breaking change with the output.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    internal sealed class OutputBreakingChangeAttribute : BreakingChangeAttribute
    {
        /// <summary>
        /// Gets or sets the output properties that will be deprecated.
        /// </summary>
        public string[] DeprecatedOutputProperties { get; set; }

        /// <summary>
        /// Gets the output type that will be deprecated.
        /// </summary>
        public Type DeprecatedOutputType { get; }

        /// <summary>
        /// Gets or sets the new properties that will be introduced.
        /// </summary>
        public string[] NewOutputProperties { get; set; }

        /// <summary>
        /// Gets or sets the name of the replacement output type.
        /// </summary>
        public string ReplacementOutputTypeName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutputBreakingChangeAttribute" /> class.
        /// </summary>
        /// <param name="deprecatedOutputType">The type for the output type that will be deprecated.</param>
        public OutputBreakingChangeAttribute(Type deprecatedOutputType) : base(string.Empty)
        {
            DeprecatedOutputType = deprecatedOutputType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutputBreakingChangeAttribute" /> class.
        /// </summary>
        /// <param name="deprecatedOutputType">The type for the output type that will be deprecated.</param>
        /// <param name="deprecateByVersion">The version where the change will be required.</param>
        public OutputBreakingChangeAttribute(Type deprecatedOutputType, string deprecateByVersion) :
            base(string.Empty, deprecateByVersion)
        {
            DeprecatedOutputType = deprecatedOutputType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutputBreakingChangeAttribute" /> class.
        /// </summary>
        /// <param name="deprecatedOutputType">The type for the output type that will be deprecated.</param>
        /// <param name="deprecateByVersion">The version where the change will be required.</param>
        /// <param name="ChangeInEffectByDate">The date when the change will be required.</param>
        public OutputBreakingChangeAttribute(Type deprecatedOutputType, string deprecateByVersion, string ChangeInEffectByDate) :
            base(string.Empty, deprecateByVersion, ChangeInEffectByDate)
        {
            DeprecatedOutputType = deprecatedOutputType;
        }

        /// <summary>
        /// Gets the specified message for the attribute.
        /// </summary>
        /// <returns>The specified message for the attribute.</returns>
        protected override string GetAttributeSpecificMessage()
        {
            StringBuilder message = new();

            if (DeprecatedOutputProperties == null && NewOutputProperties == null && string.IsNullOrEmpty(ChangeDescription) && string.IsNullOrEmpty(ReplacementOutputTypeName))
            {
                message.Append(string.Format(Resources.BreakingChangesAttributesOutputTypeDeprecated, DeprecatedOutputType.FullName));
            }
            else
            {
                if (string.IsNullOrEmpty(ReplacementOutputTypeName) == false)
                {
                    message.Append(string.Format(Resources.BreakingChangesAttributesOutputChange1, DeprecatedOutputType.FullName, ReplacementOutputTypeName));
                }
                else
                {
                    message.Append(string.Format(Resources.BreakingChangesAttributesOutputChange2, DeprecatedOutputType.FullName));
                }

                if (DeprecatedOutputProperties != null && DeprecatedOutputProperties.Length > 0)
                {
                    message.Append(Resources.BreakingChangesAttributesOutputPropertiesRemoved);

                    foreach (string property in DeprecatedOutputProperties)
                    {
                        message.Append($" '{property}'");
                    }
                }

                if (NewOutputProperties != null && NewOutputProperties.Length > 0)
                {
                    message.Append(Resources.BreakingChangesAttributesOutputPropertiesAdded);

                    foreach (string property in NewOutputProperties)
                    {
                        message.Append($" '{property}'");
                    }
                }
            }

            return message.ToString();
        }
    }
}