namespace AutoBrew.PowerShell.Commands
{
    using System.Management.Automation;

    [Cmdlet(VerbsCommon.New, "AbApplicationGrant")]
    public class NewAbApplicationGrant : ModuleAsyncCmdlet
    {
        /// <inheritdoc />
        protected override async Task PerformCmdletAsync()
        {
            await Task.CompletedTask.ConfigureAwait(false);
        }
    }
}