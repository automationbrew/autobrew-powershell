namespace AutoBrew.PowerShell.Commands
{
    using System.Management.Automation;
    using System.Threading.Tasks;
    using Models;
    using Models.Authentication;
    using Properties;

    /// <summary>
    /// Cmdlet that requests a new bulk refresh token from Azure Active Directory.
    /// </summary>
    [Cmdlet(VerbsCommon.New, "AbBulkRefreshToken", SupportsShouldProcess = true)]
    [OutputType(typeof(BulkRefreshToken))]
    public class NewAbBulkRefreshToken : ModuleAsyncCmdlet
    {
        /// <summary>
        /// Gets or sets the name of the environment to be used for authentication.
        /// </summary>
        [EnvironmentCompleter]
        [Parameter(HelpMessage = "The name of the environment to be used for authentication.", Mandatory = false)]
        public string Environment { get; set; }

        /// <summary>
        /// Performs the actions associated with the command.
        /// </summary>
        /// <returns>An instance of the <see cref="Task" /> class that represents the asynchronous operation.</returns>
        protected override async Task PerformCmdletAsync()
        {
            await ConfirmActionAsync(Resources.NewBulkRefreshTokenAction, Environment, async () =>
            {
                ModuleSession.Instance.TryGetEnvironment(Environment, out ModuleEnvironment environment);

                WriteObject(await ModuleSession.Instance.AuthenticationFactory.AcquireBulkRefreshTokenAsync(
                    environment, WriteWarning, CancellationToken));
            }).ConfigureAwait(false);
        }
    }
}