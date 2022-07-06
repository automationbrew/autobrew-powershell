namespace AutoBrew.PowerShell.Commands
{
    using System.Management.Automation;
    using System.Text.RegularExpressions;
    using Models;
    using Models.Authentication;
    using Properties;

    /// <summary>
    /// Cmdlet that provides the ability to add a new environment.
    /// </summary>
    [Cmdlet(VerbsCommon.Add, "AbEnvironment", SupportsShouldProcess = true)]
    public class AddAbEnvironment : ModuleCmdlet
    {
        /// <summary>
        /// Gets or sets the Active Directory authority for the environment.
        /// </summary>
        [Parameter(HelpMessage = "The Active Directory authority for the environment.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string ActiveDirectoryAuthority { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the application for the environment.
        /// </summary>
        [Parameter(HelpMessage = "The identifier of the application for the environment.", Mandatory = false)]
        [ValidatePattern(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", Options = RegexOptions.Compiled | RegexOptions.IgnoreCase)]
        public string ApplicationId { get; set; }

        /// <summary>
        /// Gets or sets the endpoint of Microsoft Graph for the environment.
        /// </summary>
        [Parameter(HelpMessage = "The endpoint of Microsoft Graph for the environment.", Mandatory = true)]
        public string MicrosoftGraphEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the name for the environment.
        /// </summary>
        [Parameter(HelpMessage = "The name for the environment.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the tenant for the environment.
        /// </summary>
        [Parameter(HelpMessage = "The tenant for the environment.", Mandatory = false)]
        [ValidateNotNullOrEmpty]
        public string Tenant { get; set; }

        /// <summary>
        /// Performs the actions associated with the command.
        /// </summary>
        protected override void PerformCmdlet()
        {
            ModuleEnvironment environment = new()
            {
                ActiveDirectoryAuthority = ActiveDirectoryAuthority,
                ApplicationId = ApplicationId,
                MicrosoftGraphEndpoint = MicrosoftGraphEndpoint,
                Name = Name,
                Tenant = Tenant,
                Type = ModuleEnvironmentType.UserDefined
            };

            ConfirmAction(Resources.AddEnvironmentTarget, Name, () => ModuleSession.Instance.RegisterEnvironment(Name, environment));
        }
    }
}