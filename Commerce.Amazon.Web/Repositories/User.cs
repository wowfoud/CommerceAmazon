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
		public virtual Societe Societe { get; set; }
        public virtual IEnumerable<Post> Posts { get; set; }
        public virtual IEnumerable<PostPlaning> PostsAchat { get; set; }
        public virtual IEnumerable<GroupUser> Groups { get; set; }
    }
}
