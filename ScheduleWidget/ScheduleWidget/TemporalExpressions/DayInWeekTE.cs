using System;
using ScheduleWidget.Enums;

namespace ScheduleWidget.TemporalExpressions
{
    /// <summary>
    /// Compares two specific days of week exactly
    /// </summary>
    public class DayInWeekTE : TemporalExpression
    {
        private readonly DayOfWeekEnum _dayOfWeek;
        private readonly DateTime _firstDayOfWeek;
        private readonly int _weeklyIntervals;

        /// <summary>
        /// The day of week value
        /// </summary>
        /// <param name="aDayOfWeek"></param>
        public DayInWeekTE(DayOfWeekEnum aDayOfWeek, DateTime aFirstDateTime, int aWeeklyInterval)
        {
            _dayOfWeek = aDayOfWeek;
             _firstDayOfWeek = StartOfWeek(aFirstDateTime, DayOfWeek.Sunday);
            _weeklyIntervals = aWeeklyInterval;
        }

        /// <summary>
        /// Returns true if the weekly interval and the day matches.
        /// </summary>
        /// <param name="aDate"></param>
        /// <returns></returns>
        public override bool Includes(DateTime aDate)
        {
            return WeekMatches(aDate) && DayMatches(aDate);
        }
       
        /// <summary>
        /// Returns if the day matches the specified day of week.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        protected bool DayMatches(DateTime aDate)
        {
            switch (aDate.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return ((int)_dayOfWeek == 1);

                case DayOfWeek.Monday:
                    return ((int)_dayOfWeek == 2);

                case DayOfWeek.Tuesday:
                    return ((int)_dayOfWeek == 4);

                case DayOfWeek.Wednesday:
                    return ((int)_dayOfWeek == 8);

                case DayOfWeek.Thursday:
                    return ((int)_dayOfWeek == 16);

                case DayOfWeek.Friday:
                    return ((int)_dayOfWeek == 32);

                case DayOfWeek.Saturday:
                    return ((int)_dayOfWeek == 64);

                default:
                    return false;
            }
        }

        /// <summary>
        /// Returns true if the date falls in a week that matches the weekly interval.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        protected bool WeekMatches(DateTime aDate)
        {
            var startOfWeek = StartOfWeek(aDate, DayOfWeek.Sunday);
            double weeks = Math.Round((startOfWeek - _firstDayOfWeek).TotalDays / 7);

            if (weeks % _weeklyIntervals == 0)
            {
                var endOfWeek = startOfWeek.AddDays(7);

                if (aDate >= startOfWeek && aDate <= endOfWeek)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns the first day of the week based on the provided starting day.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        protected DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }
    }
}
