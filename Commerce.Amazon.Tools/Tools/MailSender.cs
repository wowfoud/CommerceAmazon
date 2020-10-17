using Commerce.Amazon.Tools.Contracts;
using System;
using System.IO;
using System.IO.Compression;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace Commerce.Amazon.Tools.Tools
{

    public class MailSender : IMailSender
    {
        private SmtpClient _smtpClient;
        private MailMessage mailMessage;
        private MailConfig _mailConfig;
        //string json = $@"""CredentialsEmail"": ""SMTPSender"",
        //    ""CredentialsPass"": ""@dm1nsmtp"",
        //    ""EnableSsl"": ""False"",
        //    ""IsBodyHtml"": ""True"",
        //    ""MailAddressFrom"": ""no-reply@gesisa.net"",
        //    ""Port"": ""25"",
        //    ""SmtpServer"": ""10.0.10.5"",
        //    ""UseDefaultCredentials"": ""True""";

        public MailSender()
        {
            _mailConfig = new MailConfig
            {
                CredentialsEmail = "abdrhmnhdd@gmail.com",
                CredentialsPass = "But4Paradis",
                EnableSsl = "True",
                IsBodyHtml = "True",
                MailAddressFrom = "abdrhmnhdd@gmail.com",
                Port = "587",
                SmtpServer = "smtp.gmail.com",
                UseDefaultCredentials = "True"
            };
            SetConfig(_mailConfig);
        }

        public MailSender(MailConfig mailConfig)
        {
            _mailConfig = mailConfig;
            SetConfig(mailConfig);
        }

        public void SetConfig(MailConfig mailConfig)
        {
            _smtpClient = new SmtpClient(mailConfig.SmtpServer)
            {
                Port = int.Parse(mailConfig.Port),
                UseDefaultCredentials = bool.Parse(mailConfig.UseDefaultCredentials),
                Credentials = new System.Net.NetworkCredential(mailConfig.CredentialsEmail, mailConfig.CredentialsPass),
                EnableSsl = bool.Parse(mailConfig.EnableSsl)
            };
        }

        public void SendMail(IdentityMessage message)
        {
            using (mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(_mailConfig.MailAddressFrom);
                mailMessage.To.Clear();
                foreach (string email in message.Destination)
                {
                    mailMessage.To.Add(email);
                }
                mailMessage.Subject = message.Subject;
                mailMessage.Body = message.Body;
                mailMessage.IsBodyHtml = bool.Parse(_mailConfig.IsBodyHtml);
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.Attachments.Clear();
                if (message.Attachments != null)
                {
                    SetAttachments(message);
                }
                _smtpClient.Send(mailMessage);
                mailMessage.Attachments.Dispose();
            }
        }

        private void SetAttachments(IdentityMessage message)
        {
            long fullsize = 0;
            long max = 2097152;
            foreach (string fileNamePath in message.Attachments)
            {
                fullsize += new FileInfo(fileNamePath).Length;
            }

            if (fullsize >= max)
            {
                string zipname = $"CompressedFiles_{DateTime.Now:ddMMyyyy_hhmmss}.zip";
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (ZipArchive zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Update))
                    {
                        foreach (string fileNamePath in message.Attachments)
                        {
                            string fileName = Path.GetFileName(fileNamePath);
                            byte[] report = File.ReadAllBytes(fileNamePath);

                            ZipArchiveEntry zipArchiveEntry = zipArchive.CreateEntry(fileName, CompressionLevel.Optimal);
                            using (StreamWriter streamWriter = new StreamWriter(zipArchiveEntry.Open()))
                            {
                                streamWriter.Write(Encoding.Default.GetString(report));
                            }

                        }
                    }
                    MemoryStream attachmentStream = new MemoryStream(memoryStream.ToArray());

                    Attachment attachment = new Attachment(attachmentStream, zipname, MediaTypeNames.Application.Zip);
                    mailMessage.Attachments.Add(attachment);
                }
            }
            else
            {
                foreach (var fileNamePath in message.Attachments)
                {
                    Attachment attachment = new Attachment(fileNamePath);
                    mailMessage.Attachments.Add(attachment);
                }
            }
        }
    }
}