using System;
using System.Net;
using System.Runtime.Serialization;

namespace ApiTools.Exceptions
{
    public class ServiceException : Exception
    {
        public HttpStatusCode StatusCode { get; init; }

        public ServiceException(HttpStatusCode statusCode): base()
        {
            StatusCode = statusCode;
        }
        
        public ServiceException(HttpStatusCode statusCode, string? message) : base(message)
        {
            StatusCode = statusCode;
        }

        public ServiceException(HttpStatusCode statusCode, string? message, Exception? innerException) : base(message, innerException)
        {
            StatusCode = statusCode;
        }

        protected ServiceException(HttpStatusCode statusCode, SerializationInfo info, StreamingContext context) : base(info, context)
        {
            StatusCode = statusCode;
        }
    }
}
