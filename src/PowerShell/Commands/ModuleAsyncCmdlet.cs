namespace AutoBrew.PowerShell.Commands
{
    using Runtime;

    /// <summary>
    /// Base type for cmdlets that should be performed asynchronously.
    /// </summary>
    public abstract class ModuleAsyncCmdlet : ModuleCmdlet
    {
        /// <summary>
        /// Confirms the action should be performed.
        /// </summary>
        /// <param name="action">Name of the action which is being performed.</param>
        /// <param name="target">Name of the target resource being acted upon.</param>
        /// <param name="method">The method to be performed once the action has been confirmed.</param>
        /// <exception cref="ArgumentException">
        /// The action parameter is empty or null.
        /// or
        /// The target parameter is empty or null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// The method parameter is null.
        /// </exception>
        protected virtual async Task ConfirmActionAsync(string action, string target, Func<Task> asyncMethod)
        {
            action.AssertNotEmpty(nameof(action));
            asyncMethod.AssertNotNull(nameof(asyncMethod));
            target.AssertNotEmpty(nameof(target));

            if (QosEvent is not null)
            {
                QosEvent.PauseQoSTimer();
            }

            if (ShouldProcess(target, action) == false)
            {
                return;
            }

            if (QosEvent is not null)
            {
                QosEvent.ResumeQoSTimer();
            }

            await asyncMethod().ConfigureAwait(false);
        }

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