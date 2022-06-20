namespace AutoBrew.PowerShell.Commands
{
    using System.Management.Automation;
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
        public string ActiveDirectoryAuthority { get; set; }

        /// <summary>
        /// Gets or sets the Microsoft Graph endpoint for the environment.
        /// </summary>
        [Parameter(HelpMessage = "The Microsoft Graph endpoint for the environment.", Mandatory = true)]
        public string MicrosoftGraphEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the name for the environment.
        /// </summary>
        [Parameter(HelpMessage = "The name for the environment.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        /// <summary>
        /// Performs the actions associated with the command.
        /// </summary>
        protected override void PerformCmdlet()
        {
            ModuleEnvironment environment = new()
            {
                ActiveDirectoryAuthority = ActiveDirectoryAuthority,
                MicrosoftGraphEndpoint = MicrosoftGraphEndpoint,
                Name = Name,
                Type = ModuleEnvironmentType.UserDefined
            };

            ConfirmAction(Resources.AddEnvironmentTarget, Name, () => ModuleSession.Instance.RegisterEnvironment(Name, environment));
        }
    }
}