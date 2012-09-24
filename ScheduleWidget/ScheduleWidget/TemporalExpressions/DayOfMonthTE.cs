using System;
using ScheduleWidget.Enums;

namespace ScheduleWidget.TemporalExpressions
{
    /// <summary>
    /// Temporal expression describing recurrence on a particular day of the month.
    /// For example, the 24th of every month or the 13th.
    /// </summary>
    public class DayOfMonthTE : TemporalExpression
    {
        private readonly int _day;

        /// <summary>
        /// Temporal expression describing recurrence on a particular day of the month:
        /// 
        /// var day24 = new DayOfMonthTE(24);
        /// </summary>
        /// <param name="day"></param>
        public DayOfMonthTE(int day)
        {
            _day = day;
        }

        /// <summary>
        /// Returns true if the day exists in the current month or false if it does not.
        /// For instance, day 31 exists for March but not for February or November.
        /// </summary>
        /// <param name="aDate"></param>
        /// <returns></returns>
        public override bool Includes(DateTime aDate)
        {
            return (aDate.Day == _day);
        }
    }
}
