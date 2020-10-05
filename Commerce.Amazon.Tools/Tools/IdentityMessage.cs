using System.Collections.Generic;

namespace Gesisa.SiiCore.Tools.Tools
{
    public class IdentityMessage
    {
        public string Destination { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<string> Attachments { get; set; }
    }
}