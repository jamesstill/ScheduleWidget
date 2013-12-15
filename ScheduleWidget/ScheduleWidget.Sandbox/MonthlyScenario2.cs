using System;
using ScheduleWidget.Enums;
using ScheduleWidget.ScheduledEvents;

namespace ScheduleWidget.Sandbox
{
    public static class MonthlyScenario2
    {
        public static void Run()
        {
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Running monthly scenario 2.");
            Console.WriteLine(Environment.NewLine);

            var aEvent = new Event()
            {
                ID = 1,
                Title = "Event 1",
                FrequencyTypeOptions = FrequencyTypeEnum.Monthly,
                MonthlyIntervalOptions = MonthlyIntervalEnum.First,
                DaysOfWeekOptions = DayOfWeekEnum.Mon | DayOfWeekEnum.Fri
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
