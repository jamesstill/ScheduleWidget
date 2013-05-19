using System;
using ScheduleWidget.Enums;

namespace ScheduleWidget.TemporalExpressions
{
    /// <summary>
    /// Expression for day in month. Implements support for temporal expressions of
    /// the form: "last Friday of the month" or "first Tuesday of the month". The 
    /// dayOfWeek enum value is converted internally to Sun = 0 ... Sat = 6. The
    /// monthly interval is 1 (first), 2 (second) thru 4 (fourth) and -1 (last).
    /// </summary>
    public class DayInMonthTE : TemporalExpression
    {
        internal protected readonly int _dayOfWeek;
        internal protected readonly int _monthlyInterval;
        private readonly bool _ignoreWeek;

        /// <summary>
        /// Creates a temporaral expression using day of the week. For example:
        /// 
        /// var sunday = new DayInMonthTE(1);
        /// var monday = new DayInMonthTE(2);
        /// var tuesday = new DayInMonthTE(4);
        /// var wednesday = new DayInMonthTE(8);
        /// var thursday = new DayInMonthTE(16);
        /// var friday = new DayInMonthTE(32);
        /// var saturday = new DayInMonthTE(64);
        /// 
        /// </summary>
        /// <param name="dayOfWeekOption">Day of week</param>
        public DayInMonthTE(DayOfWeekEnum dayOfWeekOption)
         {

             _dayOfWeek = TEHelpers.GetDayOfWeekValue(dayOfWeekOption);
             _ignoreWeek = true;
         }
        
        /// <summary>
        /// Creates a temporal expression using day of the week 0 to 6 and day
        /// of week in a month which can be positive (counting from the beginning
        /// of the month) or negative (counting from the end of the month).
        /// 
        /// var example1 = new DayInMonthTE(4, 2);  // "second Tues of the month"
        /// var example2 = new DayInMonthTE(32, 16); // "last Friday of the month"
        /// 
        /// </summary>
        /// <param name="dayOfWeekOption">day of week</param>
        /// <param name="monthlyIntervalOption"></param>
        public DayInMonthTE(DayOfWeekEnum dayOfWeekOption, MonthlyIntervalEnum monthlyIntervalOption)
        {
            _dayOfWeek = TEHelpers.GetDayOfWeekValue(dayOfWeekOption);
            _monthlyInterval = TEHelpers.GetMonthlyIntervalValue(monthlyIntervalOption);
            _ignoreWeek = false;
        }

        public override bool Includes(DateTime aDate)
        {
            if (_ignoreWeek)
            {
                return TEHelpers.DayMatches(aDate, _dayOfWeek);
            }

            return TEHelpers.DayMatches(aDate, _dayOfWeek) && TEHelpers.WeekMatches(aDate, _monthlyInterval);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var d = obj as DayInMonthTE;
            if ((object)d == null)
                return false;

            return (_monthlyInterval == d._monthlyInterval) && (_dayOfWeek == d._dayOfWeek);
        }

        public override int GetHashCode()
        {
            return _monthlyInterval ^ _dayOfWeek;
        }
        
    }
}
