using Commerce.Amazon.Domain.Entities.Enum;
using System;

namespace Commerce.Amazon.Domain.Models.Request
{
    public class FilterPost
    {
        public DateTime? DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public int? GroupId { get; set; }
        public EnumStatePlaning? StatePlan { get; set; }
    }
}
