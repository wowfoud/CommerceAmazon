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
        public int State { get; set; }

        public List<PostPlaning> Planings { get; set; }
    }

}
