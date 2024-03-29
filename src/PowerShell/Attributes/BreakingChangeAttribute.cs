﻿namespace AutoBrew.PowerShell
{
    using System.Globalization;
    using System.Management.Automation;
    using Properties;

    /// <summary>
    /// Attribute used by classes that inherit the <see cref="Commands.ModuleCmdlet" /> class to define a breaking change.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    internal class BreakingChangeAttribute : Attribute
    {
        /// <summary>
        /// The message that describes the breaking change.
        /// </summary>
        private readonly string message;

        /// <summary>
        /// Gets or sets the description for the change.
        /// </summary>
        public string ChangeDescription { get; set; }

        /// <summary>
        /// Gets the date the change will be required.
        /// </summary>
        public DateTime? ChangeInEffectByDate { get; } = null;

        /// <summary>
        /// Gets the version when this change will be deprecated.
        /// </summary>
        public string DeprecateByVersion { get; }

        /// <summary>
        /// Gets or sets the new way the command should be invoked.
        /// </summary>
        public string NewWay { get; set; }

        /// <summary>
        /// Gets or sets the old way the command was invoked.
        /// </summary>
        public string OldWay { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BreakingChangeAttribute" /> class.
        /// </summary>
        /// <param name="message">The message that describes the breaking change.</param>
        public BreakingChangeAttribute(string message)
        {
            this.message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BreakingChangeAttribute" /> class.
        /// </summary>
        /// <param name="message">The message that describes the breaking change.</param>
        /// <param name="deprecateByVersion">The version where the change will be required.</param>
        public BreakingChangeAttribute(string message, string deprecateByVersion)
        {
            message.AssertNotEmpty(nameof(message));
            deprecateByVersion.AssertNotEmpty(nameof(deprecateByVersion));

            this.message = message;
            DeprecateByVersion = deprecateByVersion;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BreakingChangeAttribute" /> class.
        /// </summary>
        /// <param name="message">The message that describes the breaking change.</param>
        /// <param name="deprecateByVersion">The version where the change will be required.</param>
        /// <param name="changeInEffectByDate">The date when the change will be required.</param>
        public BreakingChangeAttribute(string message, string deprecateByVersion, string changeInEffectByDate)
        {
            deprecateByVersion.AssertNotEmpty(nameof(deprecateByVersion));
            changeInEffectByDate.AssertNotEmpty(nameof(changeInEffectByDate));

            this.message = message;
            DeprecateByVersion = deprecateByVersion;
            ChangeInEffectByDate = DateTime.Parse(changeInEffectByDate, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Gets the specified message for the attribute.
        /// </summary>
        /// <returns>The specified message for the attribute.</returns>
        protected virtual string GetAttributeSpecificMessage()
        {
            return message;
        }

        /// <summary>
        /// Gets the name of the command from the type.
        /// </summary>
        /// <param name="type">The type for the command being invoked.</param>
        /// <returns>The name of the command from the type.</returns>
        public static string GetNameFromCmdletType(Type type)
        {
            type.AssertNotNull(nameof(type));

            string cmdletName = null;
            CmdletAttribute cmdletAttrib = (CmdletAttribute)type.GetCustomAttributes(typeof(CmdletAttribute), false).FirstOrDefault();

            if (cmdletAttrib != null)
            {
                cmdletName = $"{cmdletAttrib.VerbName}-{cmdletAttrib.NounName}";
            }

            return cmdletName;
        }

        /// <summary>
        /// Checks if the breaking change applies to how or where the command was invoked.
        /// </summary>
        /// <param name="invocationInfo">The description of how and where this command was invoked.</param>
        /// <returns>
        /// <c>true</c> if the breaking change applies to how or where the command was invoked; otherwise <c>false</c>.
        /// </returns>
        public virtual bool IsApplicableToInvocation(InvocationInfo invocationInfo)
        {
            return true;
        }

        /// <summary>
        /// Writes the attribute information to the output.
        /// </summary>
        /// <param name="type">The type for the command being invoked.</param>
        /// <param name="withCmdletName">A flag indicating whether or not to write the command name.</param>
        /// <param name="output">The action used to write the output.</param>
        public void PrintCustomAttributeInfo(Type type, bool withCmdletName, Action<string> output)
        {
            if (withCmdletName == false)
            {
                output(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.BreakingChangesAttributesDeclarationMessage,
                        GetAttributeSpecificMessage()));
            }
            else
            {
                output(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.BreakingChangesAttributesDeclarationMessageWithCmdletName,
                        GetNameFromCmdletType(type),
                        GetAttributeSpecificMessage()));
            }

            if (!string.IsNullOrWhiteSpace(ChangeDescription))
            {
                output(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.BreakingChangesAttributesChangeDescriptionMessage,
                        ChangeDescription));
            }

            if (ChangeInEffectByDate.HasValue)
            {
                output(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.BreakingChangesAttributesInEffectByDateMessage,
                        ChangeInEffectByDate.Value));
            }

            if (!string.IsNullOrEmpty(DeprecateByVersion))
            {
                output(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.BreakingChangesAttributesInEffectByVersion,
                        DeprecateByVersion));
            }

            if (OldWay != null && NewWay != null)
            {
                output(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.BreakingChangesAttributesUsageChangeMessageConsole,
                        OldWay,
                        NewWay));
            }
        }
    }
}