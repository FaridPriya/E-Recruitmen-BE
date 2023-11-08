namespace ERecruitmentBE.Helper
{
    public static class FilterDateHelper
    {
        public static DateTimeOffset GetDateFromUtc(DateTime date)
        {
            date = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            return UserCultureInfo.GetUtcTime(date);
        }

        public static DateTimeOffset GetDateToUtc(DateTime date)
        {
            date = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            date = date.AddDays(1);
            return UserCultureInfo.GetUtcTime(date);
        }
    }
}
