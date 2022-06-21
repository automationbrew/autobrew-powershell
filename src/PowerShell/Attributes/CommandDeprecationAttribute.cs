namespace AutoBrew.PowerShell
{
    using Properties;

    /// <summary>
    /// Attribute used by classes that inherit the <see cref="Commands.ModuleCmdlet" /> class to inform the user a command is being deprecated.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class CommandDeprecationAttribute : BreakingChangeAttribute
    {
        /// <summary>
        /// Gets or sets the name for the replacement command.
        /// </summary>
        public string ReplacementCmdletName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandDeprecationAttribute" /> class.
        /// </summary>
        public CommandDeprecationAttribute() : base(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandDeprecationAttribute" /> class.
        /// </summary>
        /// <param name="deprecateByVersion">The version where the change will be required.</param>
        public CommandDeprecationAttribute(string deprecateByVersion) :
             base(string.Empty, deprecateByVersion)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandDeprecationAttribute" /> class.
        /// </summary>
        /// <param name="deprecateByVersion">The version where the change will be required.</param>
        /// <param name="changeInEfectByDate">The date when the change will be required.</param>
        public CommandDeprecationAttribute(string deprecateByVersion, string changeInEfectByDate) :
             base(string.Empty, deprecateByVersion, changeInEfectByDate)
        {
        }

        /// <summary>
        /// Gets the specified message for the attribute.
        /// </summary>
        /// <returns>The specified message for the attribute.</returns>
        protected override string GetAttributeSpecificMessage()
        {
            if (string.IsNullOrEmpty(ReplacementCmdletName))
            {
                return Resources.BreakingChangesAttributesCommandDeprecationMessageNoReplacement;
            }

            return string.Format(Resources.BreakingChangesAttributesCommandDeprecationMessageWithReplacement, ReplacementCmdletName);
        }
    }
}