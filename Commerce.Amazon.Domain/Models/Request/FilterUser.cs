using Commerce.Amazon.Domain.Entities.Enum;

namespace Commerce.Amazon.Domain.Models.Request
{
    public class FilterUser
    {
        public EnumStateUser? StateUser { get; set; }
        public int? IdGroup { get; set; }
    }
}
