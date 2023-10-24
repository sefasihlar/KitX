using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using NLayer.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace NLayer.Service.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        readonly IConfiguration _configuration;

        public EmailSenderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendEmailAsync(new[] { to }, subject, body, isBodyHtml);
        }

        public async Task SendEmailAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
        {

            using var mail = new MailMessage();
            mail.IsBodyHtml = isBodyHtml;
            foreach (var to in tos)
                mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.From = new MailAddress(_configuration["Mail:Username"], "KitX", System.Text.Encoding.UTF8);

            using var smpt = new SmtpClient();

            smpt.Connect(_configuration["Mail:Host"], 465, SecureSocketOptions.SslOnConnect);

            // Kullanıcı adınızı ve şifrenizi girin.
            smpt.Authenticate("destek@kitxapp.com", "KaraKaya09.15.15");

            smpt.Send((MimeMessage)mail);

            smpt.Disconnect(true);

        }
    }
}
