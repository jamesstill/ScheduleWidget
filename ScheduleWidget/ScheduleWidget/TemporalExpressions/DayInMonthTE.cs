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
    public class DayInMonthTE : MonthTE
    {
        /// <summary>
        /// Creates a temporal expression using day of the week. For example:
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
            : base(1, DateTime.MinValue, dayOfWeekOption)
        { }

        //Every 1 month, selected date
        /// <summary>
        /// Creates a temporal expression using day of the month 1 to 31.
        /// The date will be adjusted to the last date of a month if a 
        /// particular month doesn't have the day specified.
        /// 
        /// var example1= = new DayInMonthTE(2); //2nd day of the month
        /// </summary>
        /// <param name="date"></param>
        public DayInMonthTE(int date)
            : base(1, DateTime.MinValue, date)
        { }

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
            : base(1, DateTime.MinValue, dayOfWeekOption, monthlyIntervalOption)
        { }

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
