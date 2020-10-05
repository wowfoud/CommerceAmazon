using Commerce.Amazon.Domain.Models.Response.Auth.Enum;
using System.ComponentModel.DataAnnotations;

namespace Commerce.Amazon.Domain.Models.Request.Auth
{
    public class RegisterRequest
	{
		[EmailAddress]
		[Required]
		public string Email { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Required]
		public string Nombre { get; set; }
		[Required]
		public string Apellidos { get; set; }
		public string TelUsuario { get; set; }

		public int IdSociete { get; set; }
		public string Foto { get; set; }
		[Required]
		public EnumRole? Role { get; set; }
		public string Token { get; set; }
		[Required]
		[MaxLength(6)]
		public string UserId { get; set; }
        public int? IdEstablecimiento { get; set; }
    }
}
