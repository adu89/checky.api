using System;
namespace Checky.api.Exceptions
{
    public class HttpException : Exception
    {
        public int StatusCode { get; private set; }
        public string Subject { get; private set; }

        public HttpException(int statusCode, string subject, string message) : base(message)
        {
            StatusCode = statusCode;
            Subject = subject;
        }
    }
}
