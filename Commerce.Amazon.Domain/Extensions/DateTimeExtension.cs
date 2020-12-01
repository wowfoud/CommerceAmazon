using System;

namespace Commerce.Amazon.Domain.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime TrimTime(this DateTime datetime)
        {
            datetime = datetime.AddHours(-datetime.Hour).AddMinutes(-datetime.Minute).AddSeconds(-datetime.Second);
            return datetime;
        }
        public static DateTime LastTimeDay(this DateTime datetime)
        {
            datetime = datetime.TrimTime().AddDays(1).AddSeconds(-1);
            return datetime;
        }
        public static DateTime TimeNowIFTime0(this DateTime datetime)
        {
            if (datetime.Hour == 0 && datetime.Minute == 0 && datetime.Second == 0)
            {
                datetime = datetime.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second);
            }
            return datetime;
        }
    }
}
