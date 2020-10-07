using System.Collections.Generic;

namespace Commerce.Amazon.Web.Repositories
{
    public class Group
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public int MaxDays { get; set; }
        public int CountNotifyPerDay { get; set; }
        public int CountUsersCanNotify { get; set; }
		public virtual IEnumerable<User> Users { get; set; }
    }
}
