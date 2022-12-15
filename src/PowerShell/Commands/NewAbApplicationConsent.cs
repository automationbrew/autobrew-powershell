namespace AutoBrew.PowerShell.Commands
{
    using System.Management.Automation;
    using System.Threading.Tasks;

    [Cmdlet(VerbsCommon.New, "AbApplicationConsent")]
    public class NewAbApplicationConsent : ModuleAsyncCmdlet
    {
        /// <inheritdoc />
        protected override async Task PerformCmdletAsync()
        {
            await Task.CompletedTask.ConfigureAwait(false);
        }
    }
}