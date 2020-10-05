using Gesisa.SiiCore.Tools.Contracts;
using System.Net.Mail;

namespace Gesisa.SiiCore.Tools.Tools
{

    public class MailSender : IMailSender
    {
        private SmtpClient _smtpServer;
        //SmtpClient SmtpServer = new SmtpClient(Resource.SmtpServer);
        private readonly MailMessage _vMail = new MailMessage();
        private readonly MailConfig _mailConfig;

        public MailSender(MailConfig mailConfig)
        {
            _mailConfig = mailConfig;
            SetConfig();
        }

        private void SetConfig()
        {
            _smtpServer = new SmtpClient(_mailConfig.SmtpServer);
            _vMail.IsBodyHtml = _mailConfig.IsBodyHtml;
            _smtpServer.Port = _mailConfig.Port;
            _smtpServer.UseDefaultCredentials = _mailConfig.UseDefaultCredentials;
            _smtpServer.Credentials = new System.Net.NetworkCredential(_mailConfig.UserName, _mailConfig.Password);
            _smtpServer.EnableSsl = _mailConfig.EnableSsl;
        }

        public void SendMail(IdentityMessage message)
        {
            
            if (!string.IsNullOrEmpty(_mailConfig.Destination))
            {
                message.Destination = _mailConfig.Destination;
            }
            _vMail.From = new MailAddress(_mailConfig.SenderMailAddress);
            _vMail.To.Clear();
            _vMail.To.Add(string.IsNullOrEmpty(message.Destination) ? _mailConfig.SenderMailAddress : message.Destination);
            _vMail.Subject = message.Subject;
            _vMail.Body = message.Body;
            _smtpServer.Send(_vMail);
        }

        public void SendMail(IdentityMessage message, string mailAddressFrom)
        {
            if (!string.IsNullOrEmpty(_mailConfig.Destination))
            {
                message.Destination = _mailConfig.Destination;
            }
            _vMail.From = new MailAddress(mailAddressFrom);

            _vMail.To.Add(string.IsNullOrEmpty(message.Destination) ? _mailConfig.SenderMailAddress : message.Destination);
            _vMail.Subject = message.Subject;
            _vMail.Body = message.Body;

            //SmtpServer = new SmtpClient("smtp.live.com");
            //vMail.From = new MailAddress("omar.hassani@hotmail.com");
            //SmtpServer.Port = 587;
            //SmtpServer.UseDefaultCredentials = false;
            //SmtpServer.Credentials = new System.Net.NetworkCredential("omar.hassani@hotmail.com", "Verano14..");
            //SmtpServer.EnableSsl = false;

            _smtpServer.Send(_vMail);
        }

    }
}