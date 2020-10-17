namespace Commerce.Amazon.Tools.Tools
{
    public class MailConfig
    {
        public MailConfig()
        {
        }
        public string SmtpServer { get; set; }
        public string IsBodyHtml { get; set; }
        public string Port { get; set; }
        public string UseDefaultCredentials { get; set; }
        public string CredentialsPass { get; set; }
        public string CredentialsEmail { get; set; }
        public string EnableSsl { get; set; }
        public string MailAddressFrom { get; set; }

    }
}