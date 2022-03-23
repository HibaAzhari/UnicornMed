using UnicornMed.Common.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace UnicornMed.BotLibrary.Helpers
{
    public class SendMailHelper
    {
        public static void SendMail(UserEntity user)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("hiba@cxunicorn.com"),
                Subject = "Registered Email",
                Body = "Hi " + user.Name + " from " + user.Department + ". Thank you for signing up!"
            };

            mailMessage.To.Add(user.AltEmail);

            var smtpClient = new SmtpClient
            {
                Host = "smtp.office365.com",
                Port = 587,
                UseDefaultCredentials = false, // This require to be before setting Credentials property
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential("hiba@cxunicorn.com", "Zi7Minni99"), // you must give a full email address for authentication
                TargetName = "STARTTLS/smtp.office365.com", // Set to avoid MustIssueStartTlsFirst exception
                EnableSsl = true // Set to avoid secure connection exception
            };
            smtpClient.Send(mailMessage);
        }

    }
}
