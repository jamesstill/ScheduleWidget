using System;
using ScheduleWidget.Enums;

namespace ScheduleWidget.TemporalExpressions
{
    public class DayIntervalTE : TemporalExpression
    {
        private readonly int _dayIntervals;
        private readonly DateTime _firstDateTime;

        /// <summary>
        /// Every n day(s)
        /// </summary>
        /// <param name="dayIntervals">Day interval. E.g., Every 1 day, every 2 days, .... , every n days.</param>
        /// <param name="firstDateTime">To check a day that comes under expected day interval.</param>
        public DayIntervalTE(int dayIntervals, DateTime firstDateTime)
        {
            _dayIntervals = dayIntervals;
            _firstDateTime = firstDateTime.Date;
        }

        public override bool Includes(DateTime aDate)
        {
            return (aDate - _firstDateTime).Days % _dayIntervals == 0;
        }
    }
}
