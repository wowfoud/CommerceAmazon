﻿using Commerce.Amazon.Domain.Entities.Enum;
using System;

namespace Commerce.Amazon.Domain.Models.Request
{
    public class FilterPost
    {
        public DateTime? Date { get; set; }
        public int? IdGroup { get; set; }
        public EnumStatePlaning? StatePlan { get; set; }
    }
}