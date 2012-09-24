using System;

namespace ScheduleWidget.ScheduledEvents
{
    public class DateRange
    {
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public bool IsOneTimeEvent
        {
            get
            {
                var ts = (EndDateTime - StartDateTime);
                return (ts.Days == 0);
            }
        }
    }
}
