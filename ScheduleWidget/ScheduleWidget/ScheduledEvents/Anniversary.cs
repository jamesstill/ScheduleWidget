
using System;

namespace ScheduleWidget.ScheduledEvents
{
    /// <summary>
    /// An anniversary is a day that commemorates or celebrates a past event that 
    /// occurs on the same day of the year as the initial event. For example, a
    /// birthday that occurs every Aug 1 can migrate over days of the week year 
    /// after year but always falls on the same month and day. 
    /// </summary>
    [Serializable]
    public class Anniversary
    {
        public int Month { get; set; }
        public int Day { get; set; }
    }
}
