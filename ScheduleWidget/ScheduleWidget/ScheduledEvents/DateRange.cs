using System;

namespace ScheduleWidget.ScheduledEvents
{
    [Serializable]
    public class DateRange
    {
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public DateRange() { }

        public DateRange(DateTime start, DateTime end)
        {
            StartDateTime = start;
            EndDateTime = end;
        }



        public bool IsOneTimeEvent
        {
            get
            {
                var ts = (EndDateTime - StartDateTime);
                return (ts.Days == 0);
            }
        }

        public int GetNumberOfDaysInRange()
        {
            TimeSpan interval = EndDateTime - StartDateTime;
            double totalDays = interval.TotalDays;
            return (int)Math.Ceiling(totalDays);
        }

        /// <summary>
        /// DoDateRangesOverlap,
        /// This returns true if there is some overlap between two ranges, otherwise false.
        /// 
        /// Implementation notes:
        /// To help clarify why this function is implemented in the way that it is, here is a 
        /// listing of all positional possibilities for the start and the end of two date ranges:
        /// No overlap (some of the beginnings are after some of the ends)
        /// f1 f2 s1 s2
        /// s1 s2 f1 f2 
        /// Some overlap (all the beginnings are before all the ends)
        /// f1 s1 f2 s2
        /// f1 s1 s2 f2 
        /// s1 f1 f2 s2
        /// s1 f1 s2 f2 
        /// </summary>
        static public bool DoDateRangesOverlap(DateRange first, DateRange second)
        {
            if (first.EndDateTime < second.StartDateTime) { return false; }
            if (second.EndDateTime < first.StartDateTime) { return false; }
            return true;
        }

        /// <summary>
        /// GetCompressedDateRange,
        /// This will compress the specified date range to fit inside of the specified limits.
        /// If these two date ranges do not overlap, this will throw an ArgumentOutOfRangeException.
        /// </summary>
        static public DateRange GetCompressedDateRange(DateRange suppliedRange, DateRange newLimits)
        {
            if (!DoDateRangesOverlap(suppliedRange, newLimits))
                throw new ArgumentOutOfRangeException();
            DateRange resultRange = new DateRange();
            resultRange.StartDateTime = (suppliedRange.StartDateTime < newLimits.StartDateTime) ?
                newLimits.StartDateTime : suppliedRange.StartDateTime;
            if (resultRange.StartDateTime > newLimits.EndDateTime)
                resultRange.StartDateTime = newLimits.EndDateTime;
            resultRange.EndDateTime = (suppliedRange.EndDateTime > newLimits.EndDateTime) ?
                newLimits.EndDateTime : suppliedRange.EndDateTime;
            if (resultRange.EndDateTime < newLimits.StartDateTime)
                resultRange.EndDateTime = newLimits.StartDateTime;
            return resultRange;
        }
    }
}
