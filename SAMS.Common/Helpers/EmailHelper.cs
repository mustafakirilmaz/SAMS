using SAMS.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SAMS.Common.Helpers
{
    public static class EmailHelper
    {
        public static async Task SendMail(string toMail, string subject, string body)
        {
            try
            {
                SmtpSettings settings = AppSettingsHelper.Current.SmtpSettings;
                SmtpClient client = new SmtpClient(settings.Host);
                client.Port = settings.Port;
                client.EnableSsl = false;
                client.UseDefaultCredentials = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new NetworkCredential(settings.UserName, settings.Password);

                MailMessage message = new MailMessage();
                message.From = new MailAddress(settings.FromAddress);
                message.To.Add(toMail);
                message.Body = body;
                message.Subject = subject;
                message.IsBodyHtml = true;
                await client.SendMailAsync(message);
            }
            catch (Exception ex)
            {

            }
        }

        public static async Task SendMail(List<string> toMails, string subject, string body)
        {
            foreach (var toMail in toMails)
            {
                await SendMail(toMail, subject, body);
            }
        }
    }
}
