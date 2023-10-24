using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Services
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(string to, string subject, string body, bool isBodyHtml = true);
        Task SendEmailAsync(string[] tos, string subject, string body, bool isBodyHtml = true);
    }
}
