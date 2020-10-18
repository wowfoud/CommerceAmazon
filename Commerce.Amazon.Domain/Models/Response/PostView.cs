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
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public int CountPlanifie { get; set; }
        public int CountNotified { get; set; }
        public int CountCommented { get; set; }
        public int CountExpired { get; set; }
        public int Total { get; set; }
        public bool IsExpired { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateNotified { get; set; }
        public DateTime? DateLimite { get; set; }
        public int DaysRemaining { get; set; }
        public string Comment { get; set; }
        public DateTime? DateComment { get; set; }
    }
}
