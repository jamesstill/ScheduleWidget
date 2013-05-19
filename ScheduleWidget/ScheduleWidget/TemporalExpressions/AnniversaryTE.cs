using System;

namespace ScheduleWidget.TemporalExpressions
{
    /// <summary>
    /// An anniversary temporal expression is a day that commemorates or 
    /// celebrates a past event that occurs on the same day of the year as 
    /// the initial event. For example, a birthday that occurs every Aug 1 
    /// can migrate over days of the week year after year but always falls 
    /// on the same month and day. 
    /// </summary>
    public class AnniversaryTE : TemporalExpression
    {
        private readonly int _month;
        private readonly int _day;

        /// <summary>
        /// The month and day, e.g., "Aug 1 (Birthday)":
        /// var birthday = new AnniversaryTE(8, 1);
        /// </summary>
        /// <param name="month"></param>
        /// <param name="day"></param>
        public AnniversaryTE(int month, int day)
        {
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
            return (aDate.Month == _month && aDate.Day == _day);
        }
    }
}
