using System.Collections.Generic;

namespace Commerce.Amazon.Tools.Tools
{
    public class IdentityMessage
    {
        public IEnumerable<string> Destination { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<string> Attachments { get; set; }
    }
}