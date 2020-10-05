namespace Gesisa.SiiCore.Tools.Tools
{
    public class MailConfig
	{
		public MailConfig()
		{
		}
		public string SmtpServer { get; set; }
		public bool IsBodyHtml { get; set; }
		public int Port { get; set; }
		public bool UseDefaultCredentials { get; set; }
		public string Password { get; set; }
		public string UserName { get; set; }
		public bool EnableSsl { get; set; }
		public string SenderMailAddress { get; set; }
		public string MailAddressTo { get; set; }
		public string Destination { get; set; }

	}
}