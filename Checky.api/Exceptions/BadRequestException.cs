using System;
namespace Checky.api.Exceptions
{
    public class BadRequestException : HttpException
    {
        private const int errorCode = 400;

        public BadRequestException(string subject, string message)
            : base(errorCode, subject, message)
        {
        }
    }
}
