
namespace ScheduleWidget.ScheduledEvents
{
    /// <summary>
    /// Supports schedules that occur only part of the year. The street cleaning
    /// example occurs only in the non-winter months (April through October). So
    /// the range would be a start month of 4 and end month of 10. Start and end
    /// days are optional since very fine-grained ranges are rare.
    /// </summary>
    public class RangeInYear
    {
        /// <summary>
        /// The start month for a schedule occurring during only part of the year
        /// </summary>
        public int StartMonth { get; set; }

        /// <summary>
        /// Optional start day of the month for fine-grained schedules (e.g., Apr 15).
        /// </summary>
        public int? StartDayOfMonth { get; set; }

        /// <summary>
        /// The end month for a schedule occurring during only part of the year
        /// </summary>
        public int EndMonth { get; set; }

        /// <summary>
        /// Optional end day of the month for fine-grained schedules (e.g., Oct 15).
        /// </summary>
        public int? EndDayOfMonth { get; set; }
    }
}
