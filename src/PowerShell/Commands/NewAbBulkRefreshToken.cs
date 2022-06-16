namespace AutoBrew.PowerShell.Commands
{
    using System.Management.Automation;
    using System.Threading.Tasks;
    using Models;
    using Models.Authentication;

    /// <summary>
    /// Cmdlet that requests a new bulk refresh token from Azure Active Directory.
    /// </summary>
    [Cmdlet(VerbsCommon.New, "AbBulkRefreshToken")]
    [OutputType(typeof(BulkRefreshToken))]
    public class NewAbBulkRefreshToken : ModuleAsyncCmdlet
    {
        /// <summary>
        /// Gets or sets the enivornment that will provide metadata used to acquire the bulk refresh token.
        /// </summary>
        [Alias("EnvironmentName")]
        [Parameter(HelpMessage = "The enivornment that will provide metadata used to acquire the bulk refresh token.", Mandatory = false)]
        [ValidateSet(nameof(ModuleEnvironmentName.Public))]
        public ModuleEnvironmentName Environment { get; set; }

        /// <summary>
        /// Performs the actions associated with the command.
        /// </summary>
        /// <returns>An instance of the <see cref="Task" /> class that represents the asynchronous operation.</returns>
        protected override async Task PerformCmdletAsync()
        {
            WriteObject(await ModuleSession.Instance.AuthenticationFactory.AcquireBulkRefreshTokenAsync(
                ModuleEnvironment.KnownEnvironments[Environment], (string value) => WriteWarning(value), CancellationToken));
        }
    }
}