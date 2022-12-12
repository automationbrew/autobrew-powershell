namespace AutoBrew.PowerShell
{
    using System.Management.Automation;
    using System.Text;
    using Properties;

    /// <summary>
    /// Attribute used by fields and properties in classes that inherit the <see cref="Commands.ModuleCmdlet" /> class to define a breaking change for a parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    internal class ParameterBreakingChangeAttribute : BreakingChangeAttribute
    {
        /// <summary>
        /// Gets or sets the flag that indicates if the parameter is becoming mandatory.
        /// </summary>
        public bool IsBecomingMandatory { get; set; }

        /// <summary>
        /// Gets or sets the name for the new parameter type.
        /// </summary>
        public string NewParameterTypeName { get; set; }

        /// <summary>
        /// Gets or sets the type for the old parameter.
        /// </summary>
        public Type OldParameterType { get; set; }

        /// <summary>
        /// Gets or sets the parameter that is changing.
        /// </summary>
        public string ParameterChanging { get; }

        /// <summary>
        /// Gets or sets the name for the replacement parameter.
        /// </summary>
        public string ReplacementParameterName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterBreakingChangeAttribute" /> class.
        /// </summary>
        /// <param name="parameterChanging">The parameter on the command that is changing.</param>
        public ParameterBreakingChangeAttribute(string parameterChanging) : base(string.Empty)
        {
            ParameterChanging = parameterChanging;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterBreakingChangeAttribute" /> class.
        /// </summary>
        /// <param name="parameterChanging">The parameter on the command that is changing.</param>
        /// <param name="deprecateByVersion">The version where the change will be required.</param>
        public ParameterBreakingChangeAttribute(string parameterChanging, string deprecateByVersion) :
            base(string.Empty, deprecateByVersion)
        {
            ParameterChanging = parameterChanging;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterBreakingChangeAttribute" /> class.
        /// </summary>
        /// <param name="parameterChanging">The parameter on the command that is changing.</param>
        /// <param name="deprecateByVersion">The version where the change will be required.</param>
        /// <param name="ChangeInEffectByDate">The date when the change will be required.</param>
        public ParameterBreakingChangeAttribute(string parameterChanging, string deprecateByVersion, string ChangeInEffectByDate) :
             base(string.Empty, deprecateByVersion, ChangeInEffectByDate)
        {
            ParameterChanging = parameterChanging;
        }

        /// <summary>
        /// Gets the specified message for the attribute.
        /// </summary>
        /// <returns>The specified message for the attribute.</returns>
        protected override string GetAttributeSpecificMessage()
        {
            StringBuilder message = new();

            if (string.IsNullOrEmpty(ReplacementParameterName) == false)
            {
                if (IsBecomingMandatory)
                {
                    message.Append(string.Format(Resources.BreakingChangeAttributeParameterReplacedMandatory, ParameterChanging, ReplacementParameterName));
                }
                else
                {
                    message.Append(string.Format(Resources.BreakingChangeAttributeParameterReplaced, ParameterChanging, ReplacementParameterName));
                }
            }
            else
            {
                if (IsBecomingMandatory)
                {
                    message.Append(string.Format(Resources.BreakingChangeAttributeParameterMandatoryNow, ParameterChanging));
                }
                else
                {
                    message.Append(string.Format(Resources.BreakingChangeAttributeParameterChanging, ParameterChanging));
                }
            }

            if (OldParameterType != null && string.IsNullOrEmpty(NewParameterTypeName) == false)
            {
                message.Append(string.Format(Resources.BreakingChangeAttributeParameterTypeChange, OldParameterType.FullName, NewParameterTypeName));
            }

            return message.ToString();
        }

        /// <summary>
        /// Checks if the breaking change applies to how or where the command was invoked.
        /// </summary>
        /// <param name="invocationInfo">The description of how and where this command was invoked.</param>
        /// <returns>
        /// <c>true</c> if the breaking change applies to how or where the command was invoked; otherwise <c>false</c>.
        /// </returns>
        public override bool IsApplicableToInvocation(InvocationInfo invocationInfo)
        {
            bool? applicable = invocationInfo == null ? true : invocationInfo.BoundParameters?.Keys?.Contains(ParameterChanging);

            return applicable ?? false;
        }
    }
}