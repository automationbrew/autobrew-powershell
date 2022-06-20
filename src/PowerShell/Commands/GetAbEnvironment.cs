namespace AutoBrew.PowerShell.Commands
{
    using System.Management.Automation;
    using Models;
    using Models.Authentication;

    /// <summary>
    /// Cmdlet that provides the ability to get the environment metadata.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "AbEnvironment")]
    [OutputType(typeof(ModuleEnvironment))]
    public class GetAbEnvironment : ModuleCmdlet
    {
        /// <summary>
        /// Gets or sets the name for the environment.
        /// </summary>
        [EnvironmentCompleter]
        [Parameter(HelpMessage = "The name for the environment.", Mandatory = false)]
        public string Name { get; set; }

        /// <summary>
        /// Performs the actions associated with the command.
        /// </summary>
        protected override void PerformCmdlet()
        {
            if (string.IsNullOrEmpty(Name))
            {
                WriteObject(ModuleSession.Instance.ListEnvironments(), true);
            }
            else
            {
                ModuleSession.Instance.TryGetEnvironment(Name, out ModuleEnvironment environment);
                WriteObject(environment);
            }
        }
    }
}