using Data.Model.Models;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace WebUI
{
    public class Emailer
    {
        public static void SendUserRegistrationMail(string from, AppUser user)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Dear {user.FirstName} {user.LastName}.<br/>");
            sb.AppendLine("You are registered.<br/>");
            sb.AppendLine($"User Name : {user.UserName}<br/>");
            sb.AppendLine($"Password : {user.Password}<br/>");
            sb.AppendLine("<br/>");
            sb.AppendLine("Regards, Administration.");
            SendMail(from,user.EmailAddress,"User Registration", sb.ToString());
        }

        public static void SendMail(string from, string recipients, string subject, string htmlText)
        {
            // отправить письмо на почту Admin
            var client = new SmtpClient("smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("449eb1aa358e46", "299db081ab95dc"),
                EnableSsl = true
            };
            client.Send(from:from, recipients: recipients, subject: subject, body: htmlText);
        }
    }
}
