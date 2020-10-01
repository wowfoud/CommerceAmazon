using Commerce.Amazon.Domain.Models.Request.Auth;

namespace Commerce.Amazon.Domain.Models
{
    public class AccountProcessRequest
	{
		public AuthenticationRequest AuthenticationRequest { get; set; }
		public SendLinkEmailRequest SendLinkEmailRequest { get; set; }
		public CheckLinkResetCodeRequest CheckLinkResetCodeRequest { get; set; }
		public ResetPasswordRequest ResetPasswordRequest { get; set; }
		public RegisterRequest RegisterRequest { get; set; }
	}
}
