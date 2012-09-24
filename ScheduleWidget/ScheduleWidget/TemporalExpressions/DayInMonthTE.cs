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
        private readonly int _dayOfWeek;
        private readonly int _monthlyInterval;
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
            
             _dayOfWeek = GetDayOfWeekValue(dayOfWeekOption);
             _ignoreWeek = true;
         }
        
        /// <summary>
        /// Creates a temporaral expression using day of the week 0 to 6 and day
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
            _dayOfWeek = GetDayOfWeekValue(dayOfWeekOption);
            _monthlyInterval = GetMonthlyIntervalValue(monthlyIntervalOption);
            _ignoreWeek = false;
        }

        public override bool Includes(DateTime aDate)
        {
            if (_ignoreWeek)
            {
                return DayMatches(aDate);
            }

            return DayMatches(aDate) && WeekMatches(aDate);
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

        protected bool DayMatches(DateTime aDate)
        {
            return GetDayOfWeek(aDate) == _dayOfWeek;
        }

        protected bool WeekMatches(DateTime aDate)
        {
            if (_monthlyInterval > 0)
            {
                return WeekFromStartMatches(aDate);
            }

            return WeekFromEndMatches(aDate);
        }

        private bool WeekFromStartMatches(DateTime aDate)
        {
            var week = GetWeekInMonth(aDate.Day);
            return (week == _monthlyInterval);
        }

        private bool WeekFromEndMatches(DateTime aDate)
        {
            var daysFromMonthEnd = GetDaysLeftInMonth(aDate) + 1;
            return GetWeekInMonth(daysFromMonthEnd) == Math.Abs(_monthlyInterval);
        }

        private static int GetDayOfWeekValue(DayOfWeekEnum dayOfWeekOption)
        {
            switch (dayOfWeekOption)
            {
                case DayOfWeekEnum.Sun:
                    return 0;

                case DayOfWeekEnum.Mon:
                    return 1;

                case DayOfWeekEnum.Tue:
                    return 2;

                case DayOfWeekEnum.Wed:
                    return 3;

                case DayOfWeekEnum.Thu:
                    return 4;

                case DayOfWeekEnum.Fri:
                    return 5;

                case DayOfWeekEnum.Sat:
                    return 6;

                default:
                    return 0;
            }
        }

        private static int GetMonthlyIntervalValue(MonthlyIntervalEnum monthlyIntervalOption)
        {
            switch(monthlyIntervalOption)
            {
                case MonthlyIntervalEnum.First:
                    return 1;

                case MonthlyIntervalEnum.Second:
                    return 2;

                case MonthlyIntervalEnum.Third:
                    return 3;

                case MonthlyIntervalEnum.Fourth:
                    return 4;

                case MonthlyIntervalEnum.Last:
                    return -1;

                default:
                    return 0;
            }
        }
    }
}
