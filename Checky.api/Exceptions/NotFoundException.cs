using System;
namespace Checky.api.Exceptions
{
    public class NotFoundException : HttpException
    {
        private const string message = "'{0}' was not found.";
        private const int errorCode = 404;

        public NotFoundException(string subject)
            : base(errorCode, subject, string.Format(message, subject))
        {
        }
    }

}
