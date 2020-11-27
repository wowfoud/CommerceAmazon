namespace Commerce.Amazon.Web.Repositories
{
    public class GroupUser
	{
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public User User { get; internal set; }
        public virtual Group Group { get; set; }
    }
}
