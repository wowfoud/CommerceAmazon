using Commerce.Amazon.Domain.Models.Response.Auth.Enum;

namespace Commerce.Amazon.Domain.Models
{
    public class ProfileModel
    {
        public string ImagePath { get; set; }
        public string FullName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLogo { get; set; }
        public string IdUser { get; set; }
        public int IdSociete { get; set; }
        public string Token { get; set; }
        public string LogoCompany
        {
            get
            {
                return CompanyLogo ?? "/images/default pic.jpg";
            }
        }

        public Role? Role { get; set; }

        public bool IsUser { get { return Role == Response.Auth.Enum.Role.User; } }
        public bool IsAdmin { get { return Role == Response.Auth.Enum.Role.Admin; } }

    }
}
