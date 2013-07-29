using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScheduleWidget.TemporalExpressions
{
    public class YearTE : TemporalExpression
    {
        private readonly int _yearIntervals;
        private readonly int _eventStartYear;
        private readonly int _month;
        private readonly int _day;

        public YearTE(int yearIntervals, int eventStartYear, int month, int day)
        {
            _yearIntervals = yearIntervals;
            _eventStartYear = eventStartYear;
            _month = month;
            _day = day;
        }

        /// <summary>
        /// Returns true if the date falls on the day of year
        /// </summary>
        /// <param name="aDate"></param>
        /// <returns></returns>
        public override bool Includes(DateTime aDate)
        {
            return IntervalMatches(aDate) && DayOfMonthMatches(aDate);
        }

        protected bool IntervalMatches(DateTime aDate)
        {
            return _yearIntervals > 1
                   ? (Math.Abs(aDate.Year - _eventStartYear)) % _yearIntervals == 0
                   : true;
        }

        protected bool DayOfMonthMatches(DateTime aDate)
        {
            return (aDate.Month == _month && aDate.Day == _day);
        }
    }
}
