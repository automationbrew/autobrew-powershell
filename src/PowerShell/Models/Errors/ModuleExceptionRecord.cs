namespace AutoBrew.PowerShell.Models.Errors
{
    using System;
    using System.Management.Automation;

    /// <summary>
    /// Represents an error that occurred during command execution.
    /// </summary>
    public class ModuleExceptionRecord : ModuleErrorRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleExceptionRecord" /> class.
        /// </summary>
        /// <param name="exception">An instance of <see cref="System.Exception" /> that represents the exception that occurred.</param>
        /// <param name="record">An instance of <see cref="ErrorRecord" /> that represents the record for the error.</param>
        public ModuleExceptionRecord(Exception exception, ErrorRecord record) : base(record)
        {
            if (exception != null)
            {
                Message = exception.Message;
                HelpLink = exception.HelpLink;
                StackTrace = exception.StackTrace;
            }

            Exception = exception;
        }

        /// <summary>
        /// Gets the error that occur during command execution.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Gets the link to the help file associated with this exception.
        /// </summary>
        public string HelpLink { get; }

        /// <summary>
        /// Gets the message that describes the current exception.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets the string representation of the immediate frames on the call stack.
        /// </summary>
        public string StackTrace { get; }
    }
}