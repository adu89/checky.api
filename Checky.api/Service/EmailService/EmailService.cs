using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Checky.api.Service.EmailService
{
    public class EmailService : IEmailService
    {
            private readonly string fromEmail;
            private readonly string replyEmail;
            private readonly SendGridClient client;

            public EmailService(IConfiguration config)
            {
                client = new SendGridClient(config.GetValue<string>("SendGridClientKey"));
                fromEmail = config.GetValue<string>("FromEmail");
                replyEmail = config.GetValue<string>("ReplyEmail");
            }

            public async Task SendEmailAsync(string emailAddress, string subject, string emailBody)
            {
                var message = new SendGridMessage();

                message.SetFrom(new EmailAddress(fromEmail, "Checky"));

                message.SetReplyTo(new EmailAddress(replyEmail));

                message.AddTo(emailAddress);

                message.SetSubject(subject);

                message.AddContent(MimeType.Html, emailBody);

                await client.SendEmailAsync(message);
            }     
    }
}
