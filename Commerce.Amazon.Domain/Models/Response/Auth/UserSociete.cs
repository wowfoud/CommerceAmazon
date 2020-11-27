using Commerce.Amazon.Domain.Models.Response.Auth.Enum;

namespace Commerce.Amazon.Domain.Models.Response.Auth
{
    public class UserSociete
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Telephon { get; set; }
        public string Photo { get; set; }
        public UserState State { get; set; }
        public EnumRole? Role { get; set; }
        public int SocieteId { get; set; }
        public Societe Societe { get; set; }
        public string RoleName { get { return Role.ToString(); } }
    }
}
