namespace AutoBrew.PowerShell.Commands
{
    using System.Management.Automation;
    using Models;
    using Models.Authentication;
    using Properties;

    /// <summary>
    /// Cmdlet that provides the ability to update an environment.
    /// </summary>
    [Cmdlet(VerbsCommon.Set, "AbEnvironment", SupportsShouldProcess = true)]
    public class SetAbEnvironment : ModuleCmdlet
    {
        /// <summary>
        /// Gets or sets the Active Directory authority for the environment.
        /// </summary>
        [Parameter(HelpMessage = "The Active Directory authority for the environment.", Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public string ActiveDirectoryAuthority { get; set; }

        /// <summary>
        /// Gets or sets the bulk refresh token begin endpoint for the environment.
        /// </summary>
        [Parameter(HelpMessage = "The bulk refresh token begin endpoint for the environment.", Mandatory = false, ValueFromPipelineByPropertyName = true)]
        public string BulkRefreshTokenBeginEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the bulk refresh token poll endpoint for the environment.
        /// </summary>
        [Parameter(HelpMessage = "The bulk refresh token poll endpoint for the environment.", Mandatory = false, ValueFromPipelineByPropertyName = true)]
        public string BulkRefreshTokenPollEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the Microsoft Graph endpoint for the environment.
        /// </summary>
        [Parameter(HelpMessage = "The Microsoft Graph endpoint for the environment.", Mandatory = false, ValueFromPipelineByPropertyName = true)]
        public string MicrosoftGraphEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the name for the environment.
        /// </summary>
        [Parameter(HelpMessage = "The name for the environment.", Mandatory = true, ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        /// <summary>
        /// Performs the actions associated with the command.
        /// </summary>
        protected override void PerformCmdlet()
        {
            ModuleSession.Instance.TryGetEnvironment(Name, out ModuleEnvironment environment);

            if (environment == null)
            {
                environment = new()
                {
                    ActiveDirectoryAuthority = ActiveDirectoryAuthority,
                    BulkRefreshTokenBeginEndpoint = BulkRefreshTokenBeginEndpoint,
                    BulkRefreshTokenPollEndpoint = BulkRefreshTokenPollEndpoint,
                    MicrosoftGraphEndpoint = MicrosoftGraphEndpoint,
                    Name = Name,
                    Type = ModuleEnvironmentType.UserDefined
                };
            }
            else
            {
                foreach (var property in typeof(ModuleEnvironment).GetProperties().Where(p => p.Name.Equals(nameof(environment.Name)) == false))
                {
                    SetValue(environment, property.Name, GetType().GetProperty(property.Name).GetValue(this).ToString());
                }
            }

            ConfirmAction(Resources.UpdateEnvironmentTarget, Name, () => ModuleSession.Instance.RegisterEnvironment(Name, environment, true));
        }

        /// <summary>
        /// Sets the value for the specified property.
        /// </summary>
        /// <param name="environment">The instance of the <see cref="ModuleEnvironment" /> class where the property should be updated.</param>
        /// <param name="property">The name for the property to update.</param>
        /// <param name="value">The value for the property.</param>
        /// <exception cref="ArgumentException">
        /// The property parameter is empty or null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// The environment parameter is null.
        /// </exception>
        private void SetValue(ModuleEnvironment environment, string property, string value)
        {
            environment.AssertNotNull(nameof(environment));
            property.AssertNotEmpty(nameof(property));

            if (string.IsNullOrEmpty(value) == false)
            {
                typeof(ModuleEnvironment).GetProperty(property).SetValue(environment, value);
            }
        }
    }
}