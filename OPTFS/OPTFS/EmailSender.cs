using System.Net.Mail;

namespace OPTFS
{
    public class EmailSender
    {
        public static Task Send(string Subject, string Message, string To,bool IsBodyHtml=false)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");            
            mail.From = new MailAddress("ontutors2030@gmail.com");
            mail.To.Add(To);
            mail.Subject = Subject;
            mail.Body = Message;
            mail.IsBodyHtml = IsBodyHtml;
            SmtpServer.Port = 587;            
            SmtpServer.Credentials = new System.Net.NetworkCredential("ontutors2030@gmail.com", "ptndpnulqifesodr");
            SmtpServer.EnableSsl = true;
            var result = SmtpServer.SendMailAsync(mail);
            return result;
        }
    }
}