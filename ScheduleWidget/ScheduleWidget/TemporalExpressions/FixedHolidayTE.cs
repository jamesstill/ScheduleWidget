using System;

namespace ScheduleWidget.TemporalExpressions
{
    /// <summary>
    /// Fixed holiday is one where the month and day never change
    /// from year to year, e.g., Jul 4 (Independence Day)
    /// </summary>
    public class FixedHolidayTE : TemporalExpression
    {
        private readonly int _month;
        private readonly int _day;

        /// <summary>
        /// The holiday month and day, e.g., "July 4 (Independence Day)":
        /// var independenceDay = new FixedHolidayTE(7, 4);
        /// </summary>
        /// <param name="month"></param>
        /// <param name="day"></param>
        public FixedHolidayTE(int month, int day)
        {
            _month = month;
            _day = day;
        }
        
        /// <summary>
        /// Returns true if the date falls on a holiday
        /// </summary>
        /// <param name="aDate"></param>
        /// <returns></returns>
        public override bool Includes(DateTime aDate)
        {
            return (aDate.Month == _month && aDate.Day == _day);
        }
    }
}
