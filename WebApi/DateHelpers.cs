using System;

namespace WebApi
{
    public static class DateHelpers
    {
        public static DateTime MonthStart()
        {
            var today = DateTime.Today;
            return new DateTime(today.Year, today.Month, 1);
        }

        public static DateTime MonthEnd()
        {
            var nextMonth = DateTime.Today.AddMonths(1);
            return new DateTime(nextMonth.Year, nextMonth.Month, 1);
        }
    }
}
