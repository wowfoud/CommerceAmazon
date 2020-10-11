using System;
using System.Collections;

namespace Commerce.Amazon.Domain.Extensions
{
    public static class ConvertExtension
    {
        public static decimal ToDecimal(this object input)
        {
            if (input == null)
            {
                return 0;
            }
            input = input.ToString().Replace(",", ".");
            decimal output;

            if (decimal.TryParse(input.ToString(), out decimal number))
            {
                output = number;
            }
            else
            {
                input = input.ToString().Replace(".", ",");
                if (decimal.TryParse(input.ToString(), out number))
                {
                    output = number;
                }
                else
                {
                    output = 0;
                }
            }
            return output;
        }

        public static decimal? ToDecimalNullable(this object input)
        {
            if (input == null)
            {
                return null;
            }
            input = input.ToString().Replace(",", ".");
            decimal? output;

            if (decimal.TryParse(input.ToString(), out decimal number))
            {
                output = number;
            }
            else
            {
                input = input.ToString().Replace(".", ",");
                if (decimal.TryParse(input.ToString(), out number))
                {
                    output = number;
                }
                else
                {
                    output = (decimal?)null;

                }
            }
            return output;
        }

        public static DateTime? ToDatetimeNullable(this string input)
        {
            DateTime? output;
            if (DateTime.TryParse(input, out DateTime dateTime))
            {
                return dateTime;
            }
            string[] dateFormats = new string[]
                {
                    "d-M-yyyy", "M-d-yyyy", "d/M/yyyy", "M/d/yyyy","yyyy-M-d","yyyy/M/d", "yyyy-MM-ddThh:mm:ss.fff",
                    "d-M-yyyy hh:M:ss", "M-d-yyyy hh:M:ss","yyyy-M-d hh:M:ss",
                    "d/M/yyyy hh:M:ss", "M/d/yyyy hh:mm:ss","yyyy/M/d hh:mm:ss"
                };
            if (DateTime.TryParseExact(
                input,
                dateFormats,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out DateTime date))
            {
                if (date.Year == 1)
                {
                    output = null;
                }
                else
                {
                    output = date;
                }
            }
            else
            {
                output = (DateTime?)null;

            }
            return output;
        }
        
        public static DateTime ToDatetime(this string input)
        {
            DateTime output;
            string[] dateFormats = new string[]
                {
                    "d-M-yyyy", "M-d-yyyy", "d/M/yyyy", "M/d/yyyy","yyyy-M-d","yyyy/M/d",
                    "d-M-yyyy hh:M:ss", "M-d-yyyy hh:M:ss","yyyy-M-d hh:M:ss",
                    "d/M/yyyy hh:M:ss", "M/d/yyyy hh:mm:ss","yyyy/M/d hh:mm:ss"
                };
            if (DateTime.TryParseExact(
                input,
                dateFormats,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out DateTime date))
            {
                if (date.Year == 1)
                {
                    output = default;
                }
                else
                {
                    output = date;
                }
            }
            else
            {
                output = default;

            }
            return output;
        }

        public static int? ToInteger(this string input, bool nullable = false)
        {
            int? output;
            if (int.TryParse(input, out int integer))
            {
                output = integer;
            }
            else
            {
                output = nullable ? (int?)null : 0;
            }
            return output;
        }
        
        public static string ToJson(this object obj)
        {
            string objjson = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            return objjson;
        }

        public static string ListToString(this IEnumerable enumerable)
        {
            string str = "";
            foreach (var item in enumerable)
            {
                if (str == "")
                {
                    str = item.ToString();
                }
                else
                {
                    str += $", {item}";
                }
            }
            return str;
        }
    }
}
