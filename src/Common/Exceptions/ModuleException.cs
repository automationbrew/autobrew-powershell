namespace AutoBrew.PowerShell
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The exception that is thrown when an error occurs with the module.
    /// </summary>
    public class ModuleException : Exception
    {
        /// <summary>
        /// The error category field name used in serialization.
        /// </summary>
        private const string ErrorCategoryFieldName = "ErrorCategory";

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleException" /> class.
        /// </summary>
        public ModuleException()
        {
            ErrorCategory = ModuleExceptionCategory.NotSpecified;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ModuleException(string message)
            : base(message)
        {
            ErrorCategory = ModuleExceptionCategory.NotSpecified;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="errorCategory">The error classification that resulted in this exception.</param>
        public ModuleException(string message, ModuleExceptionCategory errorCategory)
            : base(message)
        {
            ErrorCategory = errorCategory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.
        /// </param>
        public ModuleException(string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorCategory = ModuleExceptionCategory.NotSpecified;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.
        /// </param>
        /// <param name="errorCategory">The error classification that resulted in this exception.</param>
        public ModuleException(string message, Exception innerException, ModuleExceptionCategory errorCategory)
            : base(message, innerException)
        {
            ErrorCategory = errorCategory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected ModuleException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ErrorCategory = (ModuleExceptionCategory)info.GetInt32(nameof(ErrorCategory));
        }

        /// <summary>
        /// Gets the error classification that resulted in this exception.
        /// </summary>
        public ModuleExceptionCategory ErrorCategory { get; }

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="SerializationInfo" /> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AssertNotNull(nameof(info));

            info.AddValue(ErrorCategoryFieldName, ErrorCategory);

            base.GetObjectData(info, context);
        }
    }
}