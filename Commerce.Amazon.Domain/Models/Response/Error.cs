using System;
using System.Collections.Generic;
using System.Text;

namespace Commerce.Amazon.Domain.Models.Response
{
    public class Error
    {
        public int Code { get; set; }
        public int Index { get; set; }
        public string Champ { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return $"{Description} '{Champ}'";
        }
    }

    public class ErrorCode
    {
        public static Dictionary<int, string> ErroresDescription { get; set; } = new Dictionary<int, string>
        {
            {0, "le champ obligatoire" },
            {1, "valeur de champ invalid" },
            {2, "valeur de champ doit etre unique" },
            {3, "calcul de champ incorrect" },
        };
    }
}
