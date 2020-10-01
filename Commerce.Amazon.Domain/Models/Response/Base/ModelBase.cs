using Commerce.Amazon.Domain.Entities.CoreBase;

namespace Commerce.Amazon.Domain.Models.Response.Base
{
	public abstract class ModelBase
	{
		public StatusResponse Status { get; set; }
		public string Message { get; set; }
		public string Token { get; set; }
		public bool? NoToken { get; set; }
		public bool Authorize { get; set; }

	}
}
