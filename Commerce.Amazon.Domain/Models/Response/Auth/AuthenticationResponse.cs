using Commerce.Amazon.Domain.Models.Response.Base;

namespace Commerce.Amazon.Domain.Models.Response.Auth
{
	public class AuthenticationResponse : ModelBase
	{
		public ProfileModel Profile { get; set; }

	}
}
