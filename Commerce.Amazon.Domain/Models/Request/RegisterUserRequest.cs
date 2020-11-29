using Commerce.Amazon.Domain.Models.Response.Auth.Enum;

namespace Commerce.Amazon.Domain.Models.Request
{
    public class RegisterUserRequest
    {
		public int Id { get; set; }
		public string UserId { get; set; }
		public int GroupId { get; set; }
		public string Email { get; set; }
		public string Nom { get; set; }
		public string Prenom { get; set; }
		public string Telephon { get; set; }
		public string Photo { get; set; }
		public UserState State { get; set; }
		public EnumRole? Role { get; set; }
		public int SocieteId { get; set; }
		public string Password { get; set; }
		public int[] Groupes { get; set; }

	}
}
