using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace Framework.Common
{
    public class MailHelper
    {
        static String EmailFrom = "olympus.social.uit@gmail.com";
        static String FromName = "olympus-auto-mail";
        static String EmailFromPassword = "Thinh1541995";
        static String AdminMail = "olympus.social.uit@gmail.com";
        public static void SendToAdmin(String subject,String content)
        {
            SendMail(AdminMail, "Admin", subject, content);
        }
        public static void SendMail(String to,String toName,String subject,String content)
        {

            var fromAddress = new MailAddress(EmailFrom, FromName);
            var toAddress = new MailAddress(to, toName);
            string fromPassword = EmailFromPassword;
            string body = content;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                IsBodyHtml = true,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }
}
