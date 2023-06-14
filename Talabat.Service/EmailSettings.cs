using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.EmailSettings;
using Talabat.Core.Services;

namespace Talabat.Service
{
    public class EmailSettings : IEmailService
    {
        private readonly MailSettings options;

        public EmailSettings(IOptions<MailSettings> options)
        {
            this.options = options.Value;
        }

        public async Task SendEmailAsync(Email email)
        {
            var mail = new MimeMessage();
            mail.Sender = MailboxAddress.Parse(options.Email);
            mail.Subject = email.Subject;
            mail.To.Add(MailboxAddress.Parse(email.To));

            var builder = new BodyBuilder();
            if (email.IsHtml)
            {
                builder.HtmlBody = email.Body; // Set the email body as HTML
            }
            else
            {
                builder.TextBody = email.Body; // Set the email body as plain text
            }
            mail.Body = builder.ToMessageBody();

            mail.From.Add(new MailboxAddress(options.DisplayName, options.Email));

            using (var smtp = new SmtpClient())
            {
                smtp.Connect(options.Host, options.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(options.Email, options.Password);
                smtp.Send(mail);
                smtp.Disconnect(true);
            }
        }
    }
}
