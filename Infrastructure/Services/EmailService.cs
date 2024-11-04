using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using Application.Interfaces;
using Domain.Entities;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            string smtpServer = _smtpSettings.Host;
            int smtpPort = _smtpSettings.Port;
            string smtpUser = _smtpSettings.UserName;
            string smtpPass = _smtpSettings.Password;

            using (var client = new SmtpClient(smtpServer, smtpPort))
            {
                client.Credentials = new NetworkCredential(smtpUser, smtpPass);
                client.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpUser),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(to);

                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
