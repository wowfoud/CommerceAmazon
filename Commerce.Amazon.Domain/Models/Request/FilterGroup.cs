using Commerce.Amazon.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Commerce.Amazon.Domain.Models.Request
{
    public class FilterGroup
    {
        public EnumStateGroup? StateGroup { get; set; }
    }
}
