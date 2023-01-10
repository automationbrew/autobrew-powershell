namespace AutoBrew.PowerShell.Commands
{
    using System.Management.Automation;
    using Models;
    using Models.Authentication;
    using Properties;

    /// <summary>
    /// Cmdlet that provides the ability to remove a specific environment.
    /// </summary>
    [Cmdlet(VerbsCommon.Remove, "AbEnvironment", SupportsShouldProcess = true)]
    [OutputType(typeof(ModuleEnvironment))]
    public class RemoveAbEnvironment : ModuleCmdlet
    {
        /// <summary>
        /// Gets or sets the name for the environment.
        /// </summary>
        [EnvironmentCompleter]
        [Parameter(HelpMessage = "The name for the environment.", Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public string Name { get; set; }

        /// <summary>
        /// Performs the actions associated with the command.
        /// </summary>
        protected override void PerformCmdlet()
        {
            // TODO - Missing out
            ConfirmAction(Resources.RemoveEnvironmentAction, Name, () => ModuleSession.Instance.UnregisterEnvironment(Name));
        }
    }
}