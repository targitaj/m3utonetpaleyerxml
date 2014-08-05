namespace Uma.Eservices.Common
{
    using System;

    /// <summary>
    /// Application exception with additional data
    /// </summary>
    [Serializable]
    public class EserviceException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EserviceException"/> class.
        /// </summary>
        public EserviceException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EserviceException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public EserviceException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EserviceException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public EserviceException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EserviceException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected EserviceException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
