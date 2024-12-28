using DEMO.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace DEMO.PL.Helpers
{
    public static class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            var Client = new SmtpClient("smtp.gmail.com" , 587 );
            Client.EnableSsl = true;
            Client.Credentials = new NetworkCredential("eltantawymostafa2@gmail.com", "qdgllbhtxxdhsmqf");
            Client.Send("eltantawymostafa2@gmail.com", email.To, email.Subject, email.Body);
        }
    }
}
