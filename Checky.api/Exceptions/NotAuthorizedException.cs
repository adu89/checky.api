using System;
namespace Checky.api.Exceptions
{
    public class NotAuthorizedException : HttpException
    {
        private const string message = "Access to this resource denied.";
        private const int errorCode = 401;

        public NotAuthorizedException(string subject)
            : base(errorCode, subject, message)
        {
        }
    }
}
