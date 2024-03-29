﻿namespace AutoBrew.PowerShell.Commands
{
    using System.Management.Automation;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using Models;
    using Models.Authentication;
    using Properties;

    /// <summary>
    /// Cmdlet that provides the ability to add a new environment.
    /// </summary>
    [Cmdlet(VerbsCommon.Add, "AbEnvironment", SupportsShouldProcess = true)]
    [OutputType(typeof(ModuleEnvironment))]
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
        /// Gets or sets the name of the DevTest Lab instance for the environment.
        /// </summary>
        [Parameter(HelpMessage = "The name of the DevTest Lab instance for the environment.", Mandatory = false)]
        [ValidateNotNullOrEmpty]
        public string DevTestLabName { get; set; }

        /// <summary>
        /// Gets or sets the name of the Key Vault instance for the environment.
        /// </summary>
        [Parameter(HelpMessage = "The name of the Key Vault instance for the environment.", Mandatory = false)]
        [ValidateNotNullOrEmpty]
        public string KeyVaultName { get; set; }

        /// <summary>
        /// Gets or sets the endpoint of Microsoft Graph for the environment.
        /// </summary>
        [Parameter(HelpMessage = "The endpoint of Microsoft Graph for the environment.", Mandatory = true)]
        public string MicrosoftGraphEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the endpoint of Microsoft Graph for the environment.
        /// </summary>
        [ParameterBreakingChange(nameof(MicrosoftPartnerCenterEndpoint), ChangeDescription = "This parameter was introduced as not required to avoid breaking any existing scripts. It will become mandatory with the next release, please update your scripts to avoid any issues.", IsBecomingMandatory = true)]
        [Parameter(HelpMessage = "The endpoint of Microsoft Partner Center for the environment.", Mandatory = false)]
        public string MicrosoftPartnerCenterEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the name for the environment.
        /// </summary>
        [Parameter(HelpMessage = "The name for the environment.", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the resource group for the environment.
        /// </summary>
        [Parameter(HelpMessage = "The name of the resource group for the environment.", Mandatory = false)]
        [ValidateNotNullOrEmpty]
        public string ResourceGroupName { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the Azure subscription for the environment.
        /// </summary>
        [Parameter(HelpMessage = "The identifier of the Azure subscription for the environment.", Mandatory = false)]
        [ValidatePattern(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", Options = RegexOptions.Compiled | RegexOptions.IgnoreCase)]
        public string SubscriptionId { get; set; }

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
            ConfirmAction(Resources.AddEnvironmentAction, Name, () =>
            {
                ModuleEnvironment environment = new()
                {
                    ActiveDirectoryAuthority = ActiveDirectoryAuthority,
                    ApplicationId = ApplicationId,
                    MicrosoftGraphEndpoint = MicrosoftGraphEndpoint,
                    MicrosoftPartnerCenterEndpoint = MicrosoftPartnerCenterEndpoint,
                    Name = Name,
                    Tenant = Tenant,
                    Type = ModuleEnvironmentType.UserDefined
                };

                SetExtendedProperties(environment);

                ModuleSession.Instance.RegisterEnvironment(Name, environment);

                WriteObject(environment);
            });
        }

        /// <summary>
        /// Sets the extended properties for the environment.
        /// </summary>
        /// <param name="environment">The instance of the <see cref="ModuleEnvironment" /> class where the extended properties should be updated.</param>
        /// <exception cref="ArgumentNullException">
        /// The environment parameter is null.
        /// </exception>
        private void SetExtendedProperties(ModuleEnvironment environment)
        {
            environment.AssertNotNull(nameof(environment));

            string[] extendedProperties = { nameof(DevTestLabName), nameof(KeyVaultName), nameof(ResourceGroupName), nameof(SubscriptionId) };
            string propertyValue;

            foreach (PropertyInfo property in GetType().GetProperties().Where(p => extendedProperties.Contains(p.Name)))
            {
                propertyValue = property.GetValue(this, null)?.ToString();

                if (string.IsNullOrEmpty(propertyValue) == false)
                {
                    environment.SetProperty(property.Name, propertyValue);
                }
            }
        }
    }
}