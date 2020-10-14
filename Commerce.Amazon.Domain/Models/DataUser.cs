using System;
using System.Collections.Generic;
using System.Text;

namespace Commerce.Amazon.Domain.Models
{
    public class DataUser
    {
        public string UserId { get; set; }
        public int IdUser { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsUser { get; set; }
    }
}
