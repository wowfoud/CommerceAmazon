using System.ComponentModel.DataAnnotations;

namespace Commerce.Amazon.Domain.Models.Request.Auth
{
	public class ResetPasswordRequest
	{
		[Required(ErrorMessage = "Nueva contraseña requerida", AllowEmptyStrings = false)]
		[DataType(DataType.Password)]
		[Display(Name = "Nueva contraseña")]
		public string NewPassword { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirmar contraseña")]
		[Compare("NewPassword", ErrorMessage = "La nueva contraseña y la contraseña de confirmación no coinciden")]
		public string ConfirmPassword { get; set; }

		[Required]
		public string ResetCode { get; set; }
	}
}
