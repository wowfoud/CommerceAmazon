using Commerce.Amazon.Domain.Entities.Enum;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Commerce.Amazon.Web.Repositories
{
    public class Group
	{
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public EnumStateGroup State { get; set; }
        public int MaxDays { get; set; }
        public int CountNotifyPerDay { get; set; }
        public int CountUsersCanNotify { get; set; }
        public int CoutUsers { get; set; }
        public virtual IEnumerable<GroupUser> Users { get; set; }
    }
}
