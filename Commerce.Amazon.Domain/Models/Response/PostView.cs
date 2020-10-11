using System;
using System.Collections.Generic;
using System.Text;

namespace Commerce.Amazon.Domain.Models.Response
{
    public class PostView
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public decimal? Prix { get; set; }
        public DateTime? DateCreate { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public int CountCreated { get; set; }
        public int CountNotified { get; set; }
        public int CountCommented { get; set; }
        public int CountExpired { get; set; }
    }
}
