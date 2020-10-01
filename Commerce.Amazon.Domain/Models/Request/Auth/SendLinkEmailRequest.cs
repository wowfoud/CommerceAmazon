using System.ComponentModel.DataAnnotations;

namespace Commerce.Amazon.Domain.Models.Request.Auth
{
	public class SendLinkEmailRequest
	{
		[EmailAddress]
		[Required]
		public string Email { get; set; }
	}
}