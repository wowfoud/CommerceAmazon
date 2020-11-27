using Commerce.Amazon.Domain.Entities.Enum;

namespace Commerce.Amazon.Domain.Models.Response
{
    public class GroupView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public EnumStateGroup State { get; set; }
        public int MaxDays { get; set; }
        public int CountNotifyPerDay { get; set; }
        public int CountUsersCanNotify { get; set; }
        public int CoutUsers { get; set; }
    }

}
