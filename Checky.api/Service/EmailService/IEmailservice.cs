using System;
using System.Threading.Tasks;

namespace Checky.api.Service.EmailService
{
    public interface IEmailService
    {
        Task SendEmailAsync(string emailAddress, string subject, string emailBody);
    }
}
