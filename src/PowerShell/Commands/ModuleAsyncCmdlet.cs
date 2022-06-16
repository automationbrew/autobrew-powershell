namespace AutoBrew.PowerShell.Commands
{
    using Runtime;

    /// <summary>
    /// Base type for cmdlets that should be performed asynchronously.
    /// </summary>
    public abstract class ModuleAsyncCmdlet : ModuleCmdlet
    {
        /// <summary>
        /// Performs the actions associated with the command.
        /// </summary>
        protected override void PerformCmdlet()
        {
            using AsyncCommandRuntime asyncCommandRuntime = new(this, CancellationToken);
            asyncCommandRuntime.Wait(PerformCmdletAsync());
        }

        /// <summary>
        /// Performs the actions associated with the command.
        /// </summary>
        /// <returns>An instance of the <see cref="Task" /> class that represents the asynchronous operation.</returns>
        protected abstract Task PerformCmdletAsync();
    }
}