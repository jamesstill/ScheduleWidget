using System;
using ScheduleWidget.Enums;
using ScheduleWidget.ScheduledEvents;

namespace ScheduleWidget.Sandbox
{
    public static class MonthlyScenario1
    {
        /// <summary>
        /// The canonical Critical Mass monthly bicycle ride
        /// </summary>
        public static void Run()
        {
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Running monthly scenario 1.");
            Console.WriteLine(Environment.NewLine);

            var aEvent = new Event()
            {
                ID = 1,
                Title = "Critical Mass",
                FrequencyTypeOptions = FrequencyTypeEnum.Monthly,
                MonthlyIntervalOptions = MonthlyIntervalEnum.Last,
                DaysOfWeekOptions = DayOfWeekEnum.Fri
            };

            var during = new DateRange()
            {
                StartDateTime = new DateTime(2013, 12, 1),
                EndDateTime = new DateTime(2013, 12, 31)
            };

            var schedule = new Schedule(aEvent);
            var dates = schedule.Occurrences(during);
            foreach (var date in dates)
            {
                Console.WriteLine(date.ToShortDateString());
            }

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Printed out all dates between {0} and {1}", 
                during.StartDateTime.ToShortDateString(), 
                during.EndDateTime.ToShortDateString());
        }
    }
}
