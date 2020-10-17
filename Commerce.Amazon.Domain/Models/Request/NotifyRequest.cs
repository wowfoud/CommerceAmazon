using System.Collections.Generic;

namespace Commerce.Amazon.Domain.Models.Request
{
    public class NotifyRequest
    {
        public int IdPost { get; set; }
        public IEnumerable<int> Users { get; set; }
    }
}
