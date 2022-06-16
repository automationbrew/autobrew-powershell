namespace AutoBrew.PowerShell.Commands
{
    using System.Management.Automation;
    using Configuration;
    using Models;
    using Models.Configuration;

    /// <summary>
    /// Cmdlet that provides the ability to list configurations.
    /// </summary>
    [Cmdlet(VerbsData.Update, "AbConfiguration")]
    public class UpdateAbConfiguration : ModuleCmdlet
    {
        /// <summary>
        /// Gets or sets a flag that indicates whether telemetry for the module is enabled.
        /// </summary>
        [Parameter(HelpMessage = "A flag that indicates whether telemetry for the module is enabled", Mandatory = false)]
        public bool? EnableTelemetry { get; set; }

        /// <summary>
        /// Gets or sets the scope for the configuration.
        /// </summary>
        [Parameter(HelpMessage = "The scope for the configuration", Mandatory = true)]
        [ValidateSet(nameof(ConfigurationScope.CurrentUser), nameof(ConfigurationScope.Process))]
        public ConfigurationScope Scope { get; set; }

        /// <summary>
        /// Performs the actions associated with the command.
        /// </summary>
        protected override void PerformCmdlet()
        {
            if (ModuleSession.Instance.TryGetComponent(ComponentType.Configuration, out IConfigurationProvider provider) == false)
            {
                throw new ModuleException("Unable to locate the configuration provider.", ModuleExceptionCategory.Configuration);
            }

            if (EnableTelemetry.HasValue)
            {
                provider.UpdateConfiguration(ConfigurationKey.DataCollection, Scope, EnableTelemetry.Value);
            }
        }
    }
}
