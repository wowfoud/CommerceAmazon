using Commerce.Amazon.Domain.Models.Response.Auth.Enum;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Commerce.Amazon.Web.Repositories
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string UserId { get; set; }
		public string Email { get; set; }
		public string Nom { get; set; }
		public string Prenom { get; set; }
		public string Telephon { get; set; }
		public string Photo { get; set; }
		public UserState State { get; set; }
		public EnumRole? Role { get; set; }
		public int IdSociete { get; set; }
		public string RoleName { get { return Role.ToString(); } }
        public string Password { get; set; }
        public int? IdGroup { get; set; }
		public virtual Societe Societe { get; set; }
		public virtual Group Group { get; set; }
        public virtual IEnumerable<Post> Posts { get; internal set; }
        public virtual IEnumerable<PostPlaning> PostsAchat { get; internal set; }
    }
}
