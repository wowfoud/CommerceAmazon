using System.Collections;

namespace Commerce.Amazon.Domain.Models.Request
{
    public class CommentRequest
    {
        public string ScreenComment { get; set; }
        public string Comment { get; set; }
        public int IdPost { get; set; }
    }
}
