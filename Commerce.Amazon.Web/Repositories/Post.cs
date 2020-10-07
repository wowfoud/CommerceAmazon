using Commerce.Amazon.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Commerce.Amazon.Web.Repositories
{
    public class Post
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
