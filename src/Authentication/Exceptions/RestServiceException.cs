namespace AutoBrew.PowerShell
{
    using System;
    using System.Runtime.Serialization;
    using Microsoft.Rest;

    /// <summary>
    /// Represents an exception through interactions with a REST service.
    /// </summary>
    [Serializable]
    public class RestServiceException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RestServiceException" /> class.
        /// </summary>
        public RestServiceException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestServiceException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public RestServiceException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestServiceException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference
        /// (Nothing in Visual Basic) if no inner exception is specified.
        /// </param>
        public RestServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Gets or sets information about the associated HTTP request.
        /// </summary>
        public HttpRequestMessageWrapper Request { get; set; }

        /// <summary>
        /// Gets or sets information about the associated HTTP response.
        /// </summary>
        public HttpResponseMessageWrapper Response { get; set; }

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="SerializationInfo" /> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            base.GetObjectData(info, context);
        }
    }
}