using Commerce.Amazon.Domain.Entities.Enum;
using System;
using System.Collections.Generic;

namespace Commerce.Amazon.Web.Repositories
{
    public class Post
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public decimal? Prix { get; set; }
        public DateTime? DateCreate { get; set; }
        public EnumStatePost State { get; set; }
        public virtual User User { get; set; }
    }

}
