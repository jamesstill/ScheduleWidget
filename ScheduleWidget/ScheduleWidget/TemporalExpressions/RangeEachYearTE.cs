using System;

namespace ScheduleWidget.TemporalExpressions
{
    /// <summary>
    /// Compares a range of time within a year. For example, any date that
    /// falls between April and October:
    /// var nonWinterMonths = new RangeEachYearTE(4, 10);
    /// </summary>
    public class RangeEachYearTE : TemporalExpression
    {
        public int StartMonth { get; set; }
        public int EndMonth { get; set; }
        public int StartDay { get; set; }
        public int EndDay { get; set; }

        public RangeEachYearTE(int startMonth, int endMonth, int startDay, int endDay)
        {
            StartMonth = startMonth;
            EndMonth = endMonth;
            StartDay = startDay;
            EndDay = endDay;
        }

        public RangeEachYearTE(int startMonth, int endMonth)
        {
            StartMonth = startMonth;
            EndMonth = endMonth;
            StartDay = 0;
            EndDay = 0;
        }

        /// <summary>
        /// Returns true if the date is included in the expression
        /// </summary>
        /// <param name="aDate"></param>
        /// <returns></returns>
        public override bool Includes(DateTime aDate)
        {
            return MonthsInclude(aDate) ||
                   StartMonthIncludes(aDate) ||
                   EndMonthIncludes(aDate);
        }

        private bool MonthsInclude(DateTime aDate)
        {
            return (aDate.Month > StartMonth && aDate.Month < EndMonth);
        }

        private bool StartMonthIncludes(DateTime aDate)
        {
            if (aDate.Month != StartMonth)
                return false;

            if (StartDay == 0)
                return true;

            return (aDate.Day >= StartDay);
        }

        private bool EndMonthIncludes(DateTime aDate)
        {
            if (aDate.Month != EndMonth)
                return false;

            if (EndDay == 0)
                return true;

            return (aDate.Day <= EndDay);
        }
    }
}
