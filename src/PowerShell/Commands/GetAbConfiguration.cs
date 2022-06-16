namespace AutoBrew.PowerShell.Commands
{
    using System.Management.Automation;
    using Configuration;
    using Models;
    using Models.Configuration;

    /// <summary>
    /// Cmdlet that provides the ability to list configurations.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "AbConfiguration")]
    [OutputType(typeof(PSConfiguration))]
    public class GetAbConfiguration : ModuleCmdlet
    {
        /// <summary>
        /// Performs the actions associated with the command.
        /// </summary>
        protected override void PerformCmdlet()
        {
            if (ModuleSession.Instance.TryGetComponent(ComponentType.Configuration, out IConfigurationProvider provider) == false)
            {
                throw new ModuleException("Unable to locate the configuration provider.", ModuleExceptionCategory.Configuration);
            }

            WriteObject(provider.ListConfiguration().Select(c => new PSConfiguration(c)), true);
        }
    }
}