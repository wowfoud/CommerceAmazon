using System.ComponentModel.DataAnnotations;

namespace Commerce.Amazon.Domain.Models.Request.Auth
{
	public class CheckLinkResetCodeRequest
	{
		[Required]
		public string ResetCode { get; set; }
	}
}
