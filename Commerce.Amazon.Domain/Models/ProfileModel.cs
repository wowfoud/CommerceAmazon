using Commerce.Amazon.Domain.Models.Response.Auth.Enum;

namespace Commerce.Amazon.Domain.Models
{
    public class ProfileModel
    {
        public string ImagePath { get; set; }
        public string FullName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLogo { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public int IdSociete { get; set; }
        public string Token { get; set; }
        public string LogoCompany
        {
            get
            {
                return CompanyLogo ?? "/images/default pic.jpg";
            }
        }

        public EnumRole? Role { get; set; }

        public bool IsAdmin
        {
            get
            {
                bool b = Role == EnumRole.Admin;
                return b;
            }
        }

        public bool IsUser
        {
            get
            {
                bool b = Role == EnumRole.User;
                return b;
            }
        }
    }
}
