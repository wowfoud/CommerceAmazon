using System.ComponentModel.DataAnnotations;

namespace Commerce.Amazon.Domain.Entities
{
	public class UsuarioRequest
	{

		public string Email { get; set; }
		public string UserGuid { get; set; }
		public int? IdEstablecimiento { get; set; }
		public int? Id { get; set; }
		public string IdUsuario { get; set; }



	}

	public class FindUsuariosRequest
	{

		public string Email { get; set; }
		public string UserGuid { get; set; }
		public int? IdEstablecimiento { get; set; }
		public int? Id { get; set; }
		public string IdUsuario { get; set; }

	}

	public class Destino
	{
		public int? Id { get; set; }
		[Required(ErrorMessage = "El campo es obligatorio.")]
		[MaxLength(125, ErrorMessage = "La longitud del campo es incorrecta.")]
		public string RazonSocial { get; set; }
		[Required(ErrorMessage = "El campo es obligatorio.")]
		public string TipoDocumento { get; set; }
		[Required(ErrorMessage = "El campo es obligatorio.")]
		[MaxLength(15, ErrorMessage = "La longitud del campo es incorrecta.")]
		public string NumeroDocumento { get; set; }

		[StringLength(13, ErrorMessage = "La longitud del campo debe ser 13.")]
		[RegularExpression("[A-Z]{2}[0]{3}[0-9]{2}[A-Z0-9]{5}[A-Z0-9]", ErrorMessage = "CAEEs formato inválido")]
		public string Caee { get; set; }

	}
	public class Origen
	{
		public int? Id { get; set; }
		[Required(ErrorMessage = "El campo es obligatorio.")]
		public string RazonSocial { get; set; }
		[Required(ErrorMessage = "El campo es obligatorio.")]
		[MaxLength(125, ErrorMessage = "La longitud del campo es incorrecta.")]
		public string TipoDocumento { get; set; }
		[Required(ErrorMessage = "El campo es obligatorio.")]
		[MaxLength(15)]
		public string NumeroDocumento { get; set; }

		[StringLength(13, ErrorMessage = "La longitud del campo debe ser 13.")]
		[RegularExpression("[A-Z]{2}[0]{3}[0-9]{2}[A-Z0-9]{5}[A-Z0-9]", ErrorMessage = "CAEEs formato inválido")]
		public string Caee { get; set; }

	}


}
