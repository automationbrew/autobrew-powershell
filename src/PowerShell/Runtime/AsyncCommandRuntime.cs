namespace AutoBrew.PowerShell.Runtime
{
    using System.Management.Automation;
    using System.Management.Automation.Host;

    /// <summary>
    /// Provides the runtime to invoke a cmdlet asynchronously.
    /// </summary>
    public class AsyncCommandRuntime : ICommandRuntime2, IDisposable
    {
        /// <summary>
        /// The cancellation token that is used by other objects or threads to receive notice of cancellation
        /// </summary>
        private readonly CancellationToken cancellationToken;

        /// <summary>
        /// The original command runtime for the cmdlet that will be reset when this command runtime is disposed.
        /// </summary>
        private readonly ICommandRuntime2 commandRuntime;

        /// <summary>
        /// The cmdlet that is being invoked.
        /// </summary>
        private readonly PSCmdlet cmdlet;

        /// <summary>
        /// The thread synchronization event that is used signal the cmdlet has been invoked.
        /// </summary>
        private readonly ManualResetEventSlim invokeDone = new(false);

        /// <summary>
        /// The thread synchronization event that is used to signal when the cmdlet is ready to be invoked.
        /// </summary>
        private readonly ManualResetEventSlim invokeReady = new(false);

        /// <summary>
        /// The lightweight semaphore that is used to limited the number of threads that can access resource concurrently.
        /// </summary>
        private readonly SemaphoreSlim semaphore = new(1, 1);

        /// <summary>
        /// The reference to the primary thread used to determine if the action should be performed or queued.
        /// </summary>
        private readonly Thread thread;

        /// <summary>
        /// The method that should be invoked on the primary thread.
        /// </summary>
        private Action invokeOnMainThread;

        /// <summary>
        /// A flag indicating whether this instance has already been disposed.
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Gets an object that surfaces the current PowerShell transaction.
        /// </summary>
        public PSTransactionContext CurrentPSTransaction => cmdlet.CommandRuntime.CurrentPSTransaction;

        /// <summary>
        /// Gets the instance of the <see cref="PSHost" /> class for this environment.
        /// </summary>
        public PSHost Host => cmdlet.CommandRuntime.Host;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncCommandRuntime" /> class.
        /// </summary>
        /// <param name="cmdlet">The cmdlet that is being invoked.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <exception cref="ArgumentNullException">
        /// The cmdlet parameter is null.
        /// or
        /// The cancellationToken parameter is null.
        /// </exception>
        public AsyncCommandRuntime(PSCmdlet cmdlet, CancellationToken cancellationToken)
        {
            cmdlet.AssertNotNull(nameof(cmdlet));
            cancellationToken.AssertNotNull(nameof(cancellationToken));

            commandRuntime = cmdlet.CommandRuntime as ICommandRuntime2;

            this.cancellationToken = cancellationToken;
            this.cmdlet = cmdlet;
            thread = Thread.CurrentThread;

            cmdlet.CommandRuntime = this;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                if (cmdlet != null)
                {
                    cmdlet.CommandRuntime = commandRuntime;
                }

                invokeDone?.Dispose();
                invokeReady?.Dispose();
                semaphore?.Dispose();
            }

            disposed = true;
        }

        /// <summary>
        /// Confirms an operation or grouping of operations with the user.
        /// </summary>
        /// <param name="query">Textual query of whether the action should be performed, usually in the form of a question.</param>
        /// <param name="caption">Caption of the window which may be displayed when the user is prompted whether or not to perform the action.</param>
        /// <param name="hasSecurityImpact"><c>true</c> if the operation being confirmed has a security impact.</param>
        /// <param name="yesToAll"><c>true</c> if the user selects YesToAll. If this is already <c>true</c>, ShouldContinue will bypass the prompt and return true.</param>
        /// <param name="noToAll"><c>true</c> if user selects NoToAll. If this is already <c>true</c>, ShouldContinue will bypass the prompt and return false.</param>
        /// <returns><c>true</c> if the operation should continue; otherwise, <c>false</c>.</returns>
        public bool ShouldContinue(string query, string caption, bool hasSecurityImpact, ref bool yesToAll, ref bool noToAll)
        {
            bool nta, result = false, yta;

            if (thread == Thread.CurrentThread)
            {
                return commandRuntime.ShouldContinue(query, caption, hasSecurityImpact, ref yesToAll, ref noToAll);
            }

            WaitUntilReady();

            nta = noToAll;
            yta = yesToAll;

            invokeOnMainThread = () => result = commandRuntime.ShouldContinue(query, caption, hasSecurityImpact, ref yta, ref nta);
            invokeReady.Set();

            WaitUntilDone();

            yesToAll = yta;
            noToAll = nta;

            return result;
        }

        /// <summary>
        /// Confirms an operation or grouping of operations with the user.
        /// </summary>
        /// <param name="query">Textual query of whether the action should be performed, usually in the form of a question.</param>
        /// <param name="caption">Caption of the window which may be displayed when the user is prompted whether or not to perform the action. It may be displayed by some hosts, but not all.</param>
        /// <param name="yesToAll">A flag indicating whether the user has selected yes to all.</param>
        /// <param name="noToAll">A flag indicating whether the user has select no to all.</param>
        /// <returns><c>true</c> if the operation should continue; otherwise, <c>false</c>.</returns>
        public bool ShouldContinue(string query, string caption, ref bool yesToAll, ref bool noToAll)
        {
            bool nta, result = false, yta;

            if (thread == Thread.CurrentThread)
            {
                return commandRuntime.ShouldContinue(query, caption, ref yesToAll, ref noToAll);
            }

            WaitUntilReady();

            nta = noToAll;
            yta = yesToAll;

            invokeOnMainThread = () => result = commandRuntime.ShouldContinue(query, caption, ref yta, ref nta);
            invokeReady.Set();

            WaitUntilDone();

            yesToAll = yta;
            noToAll = nta;

            return result;
        }

        /// <summary>
        /// Confirms an operation or grouping of operations with the user.
        /// </summary>
        /// <param name="query">Textual query of whether the action should be performed, usually in the form of a question.</param>
        /// <param name="caption">Caption of the window which may be displayed when the user is prompted whether or not to perform the action.</param>
        /// <returns><c>true</c> if the operation should continue; otherwise, <c>false</c>.</returns>
        public bool ShouldContinue(string query, string caption)
        {
            bool result = false;

            if (thread == Thread.CurrentThread)
            {
                return commandRuntime.ShouldContinue(query, caption);
            }

            WaitUntilReady();

            invokeOnMainThread = () => result = commandRuntime.ShouldContinue(query, caption);
            invokeReady.Set();

            WaitUntilDone();

            return result;
        }

        /// <summary>
        /// Confirms the operation with the user.
        /// </summary>
        /// <param name="verboseDescription">Textual description of the action to be performed.</param>
        /// <param name="verboseWarning">Textual query of whether the action should be performed, usually in the form of a question.</param>
        /// <param name="caption">Caption of the window which may be displayed if the user is prompted whether or not to perform the action.</param>
        /// <param name="shouldProcessReason">Indicates the reason(s) why ShouldProcess returned what it returned.</param>
        /// <returns><c>true</c> if the operation should process; otherwise, <c>false</c>.</returns>
        public bool ShouldProcess(string verboseDescription, string verboseWarning, string caption, out ShouldProcessReason shouldProcessReason)
        {
            ShouldProcessReason reason = ShouldProcessReason.None;
            bool result = false;

            if (thread == Thread.CurrentThread)
            {
                return commandRuntime.ShouldProcess(verboseDescription, verboseWarning, caption, out shouldProcessReason);
            }

            WaitUntilReady();

            invokeOnMainThread = () => result = commandRuntime.ShouldProcess(verboseDescription, verboseWarning, caption, out reason);
            invokeReady.Set();

            WaitUntilDone();

            shouldProcessReason = reason;

            return result;
        }

        /// <summary>
        /// Confirm the operation with the user.
        /// </summary>
        /// <param name="verboseDescription">Textual description of the action to be performed.</param>
        /// <param name="verboseWarning">Textual query of whether the action should be performed, usually in the form of a question.</param>
        /// <param name="caption">Caption of the window which may be displayed if the user is prompted whether or not to perform the action.</param>
        /// <returns><c>true</c> if the operation should process; otherwise, <c>false</c>.</returns>
        public bool ShouldProcess(string verboseDescription, string verboseWarning, string caption)
        {
            bool result = false;

            if (thread == Thread.CurrentThread)
            {
                return commandRuntime.ShouldProcess(verboseDescription, verboseWarning, caption);
            }

            WaitUntilReady();

            invokeOnMainThread = () => result = commandRuntime.ShouldProcess(verboseDescription, verboseWarning, caption);
            invokeReady.Set();

            WaitUntilDone();

            return result;
        }

        /// <summary>
        /// Confirm the operation with the user.
        /// </summary>
        /// <param name="target">Name of the target resource being acted upon.</param>
        /// <param name="action">Name of the action which is being performed. </param>
        /// <returns><c>true</c> if the operation should process; otherwise, <c>false</c>.</returns>
        public bool ShouldProcess(string target, string action)
        {
            bool result = false;

            if (thread == Thread.CurrentThread)
            {
                return commandRuntime.ShouldProcess(target, action);
            }

            WaitUntilReady();

            invokeOnMainThread = () => result = commandRuntime.ShouldProcess(target, action);
            invokeReady.Set();

            WaitUntilDone();

            return result;
        }

        /// <summary>
        /// Confirm the operation with the user.
        /// </summary>
        /// <param name="target">Name of the target resource being acted upon.</param>
        /// <returns><c>true</c> if the operation should process; otherwise, <c>false</c>.</returns>
        public bool ShouldProcess(string target)
        {
            bool result = false;

            if (thread == Thread.CurrentThread)
            {
                return commandRuntime.ShouldProcess(target);
            }

            WaitUntilReady();

            invokeOnMainThread = () => result = commandRuntime.ShouldProcess(target);
            invokeReady.Set();

            WaitUntilDone();

            return result;
        }

        /// <summary>
        /// Routes fatal errors from a cmdlet.
        /// </summary>
        /// <param name="errorRecord">The error which caused the command to be terminated.</param>
        public void ThrowTerminatingError(ErrorRecord errorRecord)
        {
            if (thread == Thread.CurrentThread)
            {
                commandRuntime.ThrowTerminatingError(errorRecord);
                return;
            }

            WaitUntilReady();

            invokeOnMainThread = () => commandRuntime.ThrowTerminatingError(errorRecord);
            invokeReady.Set();

            WaitUntilDone();
        }

        /// <summary>
        /// Determines if a transaction is available and action.
        /// </summary>
        /// <returns><c>true</c> if a transaction is available and action; otherwise, <c>false</c>.</returns>
        public bool TransactionAvailable()
        {
            bool result = false;

            if (thread == Thread.CurrentThread)
            {
                return commandRuntime.TransactionAvailable();
            }

            WaitUntilReady();

            invokeOnMainThread = () => result = commandRuntime.TransactionAvailable();
            invokeReady.Set();

            WaitUntilDone();

            return result;
        }

        /// <summary>
        /// Waits until the specified task has completed.
        /// </summary>
        /// <param name="task">The task to be invoked.</param>
        public void Wait(Task task)
        {
            WaitHandle.WaitAny(new[] { invokeReady.WaitHandle, ((IAsyncResult)task).AsyncWaitHandle });

            do
            {
                if (invokeReady.IsSet)
                {
                    invokeReady.Reset();
                    invokeOnMainThread();
                    invokeDone.Set();
                }
            } while (task.IsCompleted == false);

            if (task.IsFaulted)
            {
                throw task.Exception;
            }
        }

        /// <summary>
        /// Writes the command detail to the appropriate pipeline using the command runtime.
        /// </summary>
        /// <param name="text">The text to be written to the pipeline.</param>
        public void WriteCommandDetail(string text)
        {
            if (thread == Thread.CurrentThread)
            {
                commandRuntime.WriteCommandDetail(text);
                return;
            }

            WaitUntilReady();

            invokeOnMainThread = () => commandRuntime.WriteCommandDetail(text);
            invokeReady.Set();

            WaitUntilDone();
        }

        /// <summary>
        /// Writes the debug text to the appropriate pipeline using the command runtime.
        /// </summary>
        /// <param name="text">The text to be written to the pipeline.</param>
        public void WriteDebug(string text)
        {
            if (thread == Thread.CurrentThread)
            {
                commandRuntime.WriteDebug(text);
                return;
            }

            WaitUntilReady();

            invokeOnMainThread = () => commandRuntime.WriteDebug(text);
            invokeReady.Set();

            WaitUntilDone();
        }

        /// <summary>
        /// Writes the error record to the appropriate pipeline using the command runtime.
        /// </summary>
        /// <param name="errorRecord">The error record to be written to the pipeline.</param>
        public void WriteError(ErrorRecord errorRecord)
        {
            if (thread == Thread.CurrentThread)
            {
                commandRuntime.WriteError(errorRecord);
                return;
            }

            WaitUntilReady();

            invokeOnMainThread = () => commandRuntime.WriteError(errorRecord);
            invokeReady.Set();

            WaitUntilDone();
        }

        /// <summary>
        /// Writes the information record to the appropriate pipeline using the command runtime.
        /// </summary>
        /// <param name="informationRecord">The information record to be written to the pipeline.</param>
        public void WriteInformation(InformationRecord informationRecord)
        {
            if (thread == Thread.CurrentThread)
            {
                commandRuntime.WriteInformation(informationRecord);
                return;
            }

            WaitUntilReady();

            invokeOnMainThread = () => commandRuntime.WriteInformation(informationRecord);
            invokeReady.Set();

            WaitUntilDone();
        }

        /// <summary>
        /// Writes the object to the appropriate pipeline using the command runtime.
        /// </summary>
        /// <param name="sendToPipeline">The object that needs to be written to the pipeline.</param>
        /// <param name="enumerateCollection">A flag indicating whether the collection should be enumerated.</param>
        public void WriteObject(object sendToPipeline, bool enumerateCollection)
        {
            if (thread == Thread.CurrentThread)
            {
                commandRuntime.WriteObject(sendToPipeline, enumerateCollection);
                return;
            }

            WaitUntilReady();

            invokeOnMainThread = () => commandRuntime.WriteObject(sendToPipeline, enumerateCollection);
            invokeReady.Set();

            WaitUntilDone();
        }

        /// <summary>
        /// Writes the object to the appropriate pipeline using the command runtime.
        /// </summary>
        /// <param name="sendToPipeline">The object that needs to be written to the pipeline.</param>
        public void WriteObject(object sendToPipeline)
        {
            if (thread == Thread.CurrentThread)
            {
                commandRuntime.WriteObject(sendToPipeline);
                return;
            }

            WaitUntilReady();

            invokeOnMainThread = () => commandRuntime.WriteObject(sendToPipeline);
            invokeReady.Set();

            WaitUntilDone();
        }

        /// <summary>
        /// Writes the progress to the appropriate pipeline using the command runtime.
        /// </summary>
        /// <param name="sourceId">Identifies which command is reporting progress.</param>
        /// <param name="progressRecord">The progress record that represents the progress to report.</param>
        public void WriteProgress(long sourceId, ProgressRecord progressRecord)
        {
            if (thread == Thread.CurrentThread)
            {
                commandRuntime.WriteProgress(sourceId, progressRecord);
                return;
            }

            WaitUntilReady();

            invokeOnMainThread = () => commandRuntime.WriteProgress(sourceId, progressRecord);
            invokeReady.Set();

            WaitUntilDone();
        }

        /// <summary>
        /// Writes the progress to the appropriate pipeline using the command runtime.
        /// </summary>
        /// <param name="progressRecord">The progress record that represents the progress to report.</param>
        public void WriteProgress(ProgressRecord progressRecord)
        {
            if (thread == Thread.CurrentThread)
            {
                commandRuntime.WriteProgress(progressRecord);
                return;
            }

            WaitUntilReady();

            invokeOnMainThread = () => commandRuntime.WriteProgress(progressRecord);
            invokeReady.Set();

            WaitUntilDone();
        }

        /// <summary>
        /// Writes the verbose text to the appropriate pipeline using the command runtime.
        /// </summary>
        /// <param name="text">The text to be written to the pipeline.</param>
        public void WriteVerbose(string text)
        {
            if (thread == Thread.CurrentThread)
            {
                commandRuntime.WriteVerbose(text);
                return;
            }

            WaitUntilReady();

            invokeOnMainThread = () => commandRuntime.WriteVerbose(text);
            invokeReady.Set();

            WaitUntilDone();
        }

        /// <summary>
        /// Writes the warning to the appropriate pipeline using the command runtime.
        /// </summary>
        /// <param name="text">The text to be written to the pipeline.</param>
        public void WriteWarning(string text)
        {
            if (thread == Thread.CurrentThread)
            {
                commandRuntime.WriteWarning(text);
                return;
            }

            WaitUntilReady();

            invokeOnMainThread = () => commandRuntime.WriteWarning(text);
            invokeReady.Set();

            WaitUntilDone();
        }

        /// <summary>
        /// Blocks the current thread until the command has been invoked.
        /// </summary>
        private void WaitUntilDone()
        {
            WaitHandle.WaitAny(new[] { cancellationToken.WaitHandle, invokeDone?.WaitHandle });
            semaphore?.Release();
        }

        /// <summary>
        /// Blocks the current thread until it can enter the semaphore.
        /// </summary>
        private void WaitUntilReady()
        {
            semaphore?.Wait(cancellationToken);
            invokeDone.Reset();
        }
    }
}