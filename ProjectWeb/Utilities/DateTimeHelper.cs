using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibData.Utilities
{
    public class DateTimeHelper
    {
        public const string DATETIME_FORMAT_VIETNAMESE = "dd/MM/yyyy HH:mm";
        private const string CHECKTIMEBETWEEN_CHOOSEFROMTIME = "Vui lòng chọn thời gian bắt đầu.";
        private const string CHECKTIMEBETWEEN_CHOOSETOTIME = "Vui lòng chọn thời gian kết thúc.";
        private const string CHECKTIMEBETWEEN_INVALIDDURINGTIME = "Thời gian bắt đầu phải không được muộn hơn thời gian kết thúc.";
        public static CultureInfo DATETIME_CULTUREINFO_US = new CultureInfo("en-US");

        public class CheckTimeBetweenResult
        {
            public bool Key { get; set; }
            public DateTime FromTime { get; set; }
            public DateTime ToTime { get; set; }
            public string Warning { get; set; }

            public CheckTimeBetweenResult()
            {
                this.Key = false;
                this.Warning = String.Empty;
            }

            public CheckTimeBetweenResult(DateTime fromTime, DateTime toTime, string warning)
            {
                FromTime = fromTime;
                ToTime = toTime;
                Warning = warning;
            }
        }
        private static readonly DateTime UnixEpoch =
         new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long GetCurrentUnixTimestampMillis()
        {
            return (long)(DateTime.UtcNow - UnixEpoch).TotalMilliseconds;
        }
        public static long GetUnixTimestampMillis(DateTime date)
        {
            return (long)(date - UnixEpoch).TotalMilliseconds;
        }
        public static DateTime DateTimeFromUnixTimestampMillis(long millis)
        {
            return UnixEpoch.AddMilliseconds(millis).ToLocalTime();
        }

        public static long GetCurrentUnixTimestampSeconds()
        {
            return (long)(DateTime.UtcNow - UnixEpoch).TotalSeconds;
        }
        public static long GetUnixTimestampSeconds(DateTime date)
        {
            return (long)(date - UnixEpoch).TotalSeconds;
        }

        public static DateTime DateTimeFromUnixTimestampSeconds(long seconds)
        {
            return UnixEpoch.AddSeconds(seconds);
        }
        //public static bool CheckTimeBetween(string from, string to)
        //{
        //    bool key = true;
        //    fromTime = DateTime.Now;
        //    toTime = DateTime.Now;
        //    warning = "";
        //    try
        //    {
        //        fromTime = DateTime.ParseExact(from, DATETIME_FORMAT_VIETNAMESE, DATETIME_CULTUREINFO_US);
        //    }
        //    catch (Exception)
        //    {
        //        warning = CHECKTIMEBETWEEN_CHOOSEFROMTIME + " ";
        //        key = false;
        //    }
        //    try
        //    {
        //        toTime = DateTime.ParseExact(to, DATETIME_FORMAT_VIETNAMESE, DATETIME_CULTUREINFO_US);
        //    }
        //    catch (Exception)
        //    {
        //        warning += CHECKTIMEBETWEEN_CHOOSETOTIME + " ";
        //        key = false;
        //    }

        //    if (key)
        //    {
        //        if (fromTime.CompareTo(toTime) == 1)
        //        {
        //            warning += CHECKTIMEBETWEEN_INVALIDDURINGTIME + " ";
        //            key = false;
        //        }
        //    }

        //    return key;
        //}
        public static int GetIso8601WeekOfYear(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
        public static List<DateTime> GetDaysIWeek(DateTime time)
        {
            int dem2 = 0;
            if (time.DayOfWeek == DayOfWeek.Monday)
            {
                dem2 += 5;
            }
            else if (time.DayOfWeek == DayOfWeek.Tuesday)
            {
                dem2 += 4;
            }
            else if (time.DayOfWeek == DayOfWeek.Wednesday)
            {
                dem2 += 3;
            }
            else if (time.DayOfWeek == DayOfWeek.Thursday)
            {
                dem2 += 2;
            }
            else if (time.DayOfWeek == DayOfWeek.Friday)
            {
                dem2 += 1;
            }
            else if (time.DayOfWeek == DayOfWeek.Saturday)
            {
                dem2 += 0;
            }
            else if (time.DayOfWeek == DayOfWeek.Sunday)
            {
                dem2 += -1;
            }
            time = time.AddDays(dem2);
            List<DateTime> dates = new List<DateTime>();
            dates.Add(time.StartOfWeek(DayOfWeek.Monday));
            dates.Add(time.StartOfWeek(DayOfWeek.Tuesday));
            dates.Add(time.StartOfWeek(DayOfWeek.Wednesday));
            dates.Add(time.StartOfWeek(DayOfWeek.Thursday));
            dates.Add(time.StartOfWeek(DayOfWeek.Friday));
            dates.Add(time.StartOfWeek(DayOfWeek.Saturday));
            dates.Add(time.StartOfWeek(DayOfWeek.Sunday).AddDays(7));
            return dates;
        }
        public static Dictionary<string, string> HourToDictionary = new Dictionary<string, string>()
        {    
             {"00", "00 giờ" },
             {"01", "01 giờ" },
             {"02", "02 giờ" },
             {"03", "03 giờ" },
             {"04", "04 giờ" },
             {"05", "05 giờ" },
             {"06", "06 giờ" },
             {"07", "07 giờ" },
             {"08", "08 giờ" },
             {"09", "09 giờ" },
             {"10", "10 giờ" },
             {"11", "11 giờ" },
             {"12", "12 giờ" },
             {"13", "13 giờ" },
             {"14", "14 giờ" },
             {"15", "15 giờ" },
             {"16", "16 giờ" },
             {"17", "17 giờ" },
             {"18", "18 giờ" },
             {"19", "19 giờ" },
             {"20", "20 giờ" },
             {"21", "21 giờ" },
             {"22", "22 giờ" },
             {"23", "23 giờ" },
        };
        public static Dictionary<string, string> MinuteToDictionary = new Dictionary<string, string>()
        {
             {"00", "00 phút" },
             {"05", "05 phút" },
             {"10", "10 phút" },
             {"15", "15 phút" },
             {"20", "20 phút" },
             {"25", "25 phút" },
             {"30", "30 phút" },
             {"35", "35 phút" },
             {"40", "40 phút" },
             {"45", "45 phút" },
             {"50", "50 phút" },
             {"55", "55 phút" },
        };
    }
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}
