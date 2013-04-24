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
    public class AnniversaryTE : YearTE
    {
        /// <summary>
        /// The month and day, e.g., "Aug 1 (Birthday)":
        /// var birthday = new AnniversaryTE(8, 1);
        /// </summary>
        /// <param name="month"></param>
        /// <param name="day"></param>
        public AnniversaryTE(int month, int day)
            : base(1, DateTime.Now.Year, month, day)
        { }
    }
}
