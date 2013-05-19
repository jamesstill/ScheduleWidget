using System;
using ScheduleWidget.Enums;

namespace ScheduleWidget.TemporalExpressions
{
    /// <summary>
    /// Floating holiday is one where the month never changes from year to year but the day does,
    /// e.g., the first Monday of September (Labor Day). So Sep = 9, Mon = 1, and first week = 1
    /// </summary>
    public class FloatingHolidayTE : DayInMonthTE
    {
        private readonly int _month;

        /// <summary>
        /// The holiday month, day, and count where the holiday falls on a different
        /// date every year, e.g., first Monday of September (Labor Day):
        /// 
        /// var laborDay = new FloatingHolidayTE(9, 1, 1);
        /// </summary>
        /// <param name="month"></param>
        /// <param name="dayOfWeekOption"></param>
        /// <param name="monthlyIntervalOption"></param>
        public FloatingHolidayTE(int month, DayOfWeekEnum dayOfWeekOption, MonthlyIntervalEnum monthlyIntervalOption) : base(dayOfWeekOption, monthlyIntervalOption)
        {
            _month = month;
        }

        /// <summary>
        /// Returns true if the date falls on a holiday
        /// </summary>
        /// <param name="aDate"></param>
        /// <returns></returns>
        public override bool Includes(DateTime aDate)
        {   
            return MonthMatches(aDate) && TEHelpers.DayMatches(aDate, _dayOfWeek) && TEHelpers.WeekMatches(aDate, _monthlyInterval);
        }

        private bool MonthMatches(DateTime aDate)
        {
            return aDate.Month == _month;
        }
    }
}
