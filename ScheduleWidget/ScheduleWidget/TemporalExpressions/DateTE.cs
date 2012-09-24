using System;

namespace ScheduleWidget.TemporalExpressions
{
    /// <summary>
    /// Compares two specific dates exactly
    /// </summary>
    public class DateTE : TemporalExpression
    {
        private DateTime _specificDate;

        /// <summary>
        /// The date value
        /// </summary>
        /// <param name="aDate"></param>
        public DateTE(DateTime aDate)
        {
            _specificDate = aDate;
        }

        /// <summary>
        /// Returns true if the date matches this date value
        /// </summary>
        /// <param name="aDate"></param>
        /// <returns></returns>
        public override bool Includes(DateTime aDate)
        {
            return (_specificDate.Date == aDate.Date);
        }
    }
}
