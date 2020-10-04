using System;

namespace Commerce.Amazon.Web.Repositories
{
    public class PostPlaning
    {
        public int IdPost { get; set; }
        public int IdUser { get; set; }
        public DateTime? DatePlanifie { get; set; }
        public DateTime? DateNotified { get; set; }
        public DateTime? DateLimite { get; set; }
        public DateTime? DateComment { get; set; }
        public int State { get; set; }
    }

}
