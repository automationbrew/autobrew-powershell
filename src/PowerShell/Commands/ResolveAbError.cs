namespace AutoBrew.PowerShell.Commands
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management.Automation;
    using Models.Errors;

    /// <summary>
    /// Cmdlet displays detailed information about PowerShell errors, with extended details for cmdlet errors.
    /// </summary>
    [Cmdlet(VerbsDiagnostic.Resolve, "AbError")]
    [OutputType(typeof(ModuleErrorRecord))]
    [OutputType(typeof(ModuleExceptionRecord))]
    public class ResolveAbError : PSCmdlet
    {
        /// <summary>
        /// The name of the any error parameter set.
        /// </summary>
        private const string AnyErrorParameterSet = "AnyErrorParameterSet";

        /// <summary>
        /// The name of the last error parameter set.
        /// </summary>
        private const string LastErrorParameterSet = "LastErrorParameterSet";

        /// <summary>
        /// Gets or sets the error records to be resolved.
        /// </summary>
        [Parameter(HelpMessage = "The error records to be resolved.", Mandatory = false, ParameterSetName = AnyErrorParameterSet, Position = 0, ValueFromPipeline = true)]
        public ErrorRecord[] Error { get; set; }

        /// <summary>
        /// Gets or set A flag indicating whether only detailed information for the last error should be shown.
        /// </summary>
        [Parameter(HelpMessage = "A flag indicating whether only detailed information for the last error should be shown.", Mandatory = true, ParameterSetName = LastErrorParameterSet)]
        public SwitchParameter Last { get; set; }

        /// <summary>
        /// Performs the execution of the command.
        /// </summary>
        protected override void ProcessRecord()
        {
            IEnumerable<ErrorRecord> records = null;

            if (ParameterSetName.Equals(LastErrorParameterSet, StringComparison.InvariantCultureIgnoreCase))
            {
                IEnumerable<ErrorRecord> errors = GetErrorVariable();

                if (errors != null && errors.FirstOrDefault() != null)
                {
                    records = new List<ErrorRecord> { { errors.FirstOrDefault() } };
                }
            }
            else
            {
                records = Error ?? GetErrorVariable();
            }


            if (records != null)
            {
                foreach (ErrorRecord record in records)
                {
                    HandleException(record.Exception, record);
                }
            }
        }

        /// <summary>
        /// Gets the collection of errors in the error variable.
        /// </summary>
        /// <returns>A collection of errors in the error variable.</returns>
        private IEnumerable<ErrorRecord> GetErrorVariable()
        {
            IEnumerable<ErrorRecord> result = null;

            if (GetVariableValue("global:Error", null) is IEnumerable errors)
            {
                result = errors.OfType<ErrorRecord>();
            }

            return result;
        }

        /// <summary>
        /// Handles the output operations for the specified error records and exceptions.
        /// </summary>
        /// <param name="exception">An instance of <see cref="Exception" /> that represents an issue that was encountered/</param>
        /// <param name="record">An instance of <see cref="ErrorRecord" /> that represents a PowerShell error record.</param>
        private void HandleException(Exception exception, ErrorRecord record)
        {
            if (exception == null)
            {
                WriteObject(new ModuleErrorRecord(record));
            }

            if (exception is AggregateException aggregateException)
            {
                foreach (Exception innerException in aggregateException.InnerExceptions.Where(e => e != null))
                {
                    HandleException(innerException, record);
                }
            }
            else
            {
                WriteObject(new ModuleExceptionRecord(exception, record));

                if (exception.InnerException != null)
                {
                    HandleException(exception.InnerException, record);
                }
            }
        }
    }
}