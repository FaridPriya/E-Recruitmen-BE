using System;
using TimeZoneConverter;

namespace ERecruitmentBE.Helper
{
    public class UserCultureInfo
    {
        public static string DateTimeFormat { get; set; }
        public static TimeZoneInfo TimeZone { get; set; }
        public static string TimeZoneName { get; set; }
        public static DateTimeOffset GetUtcTime(DateTime datetime)
        {
            if (string.IsNullOrEmpty(TimeZoneName))
                TimeZoneName = "Asia/Jakarta";

            TimeZone = TZConvert.GetTimeZoneInfo(TimeZoneName);
            return TimeZoneInfo.ConvertTimeToUtc(datetime, TimeZone);
        }

        public static DateTimeOffset ConvertToUtc(DateTimeOffset datetime)
        {
            TimeZone = TimeZoneInfo.FindSystemTimeZoneById("UTC");
            return TimeZoneInfo.ConvertTime(datetime, TimeZone).ToUniversalTime();
        }
    }
}
