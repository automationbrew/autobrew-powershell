namespace AutoBrew.PowerShell.Commands
{
    using System.Management.Automation;
    using Models;
    using Properties;

    /// <summary>
    /// Cmdlet that provides the ability to remove a specific environment.
    /// </summary>
    [Cmdlet(VerbsCommon.Remove, "AbEnvironment", SupportsShouldProcess = true)]
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
            ConfirmAction(Resources.RemoveEnvironmentTarget, Name, () => ModuleSession.Instance.UnregisterEnvironment(Name));
        }
    }
}