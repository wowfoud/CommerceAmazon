using System.ComponentModel.DataAnnotations;

namespace Commerce.Amazon.Domain.Models.Request.Auth
{
	public class AuthenticationRequest
	{
		[EmailAddress]
		[Required]
		public string Email { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
