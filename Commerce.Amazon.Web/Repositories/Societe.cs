using Commerce.Amazon.Domain.Models.Response.Auth.Enum;

namespace Commerce.Amazon.Web.Repositories
{
    public class Societe
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Logo { get; set; }
		public StateCompany? State { get; set; }


	}
}
