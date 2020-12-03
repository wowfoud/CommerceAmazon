using Commerce.Amazon.Domain.Entities.Enum;
using System;

namespace Commerce.Amazon.Domain.Models.Response
{
    public class PostPlaningView
    {
        public int IdPost { get; set; }
        public int IdUser { get; set; }
        public DateTime? DatePlanifie { get; set; }
        public DateTime? DateNotified { get; set; }
        public DateTime? DateLimite { get; set; }
        public DateTime? DateComment { get; set; }
        public string State { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
    }
}
