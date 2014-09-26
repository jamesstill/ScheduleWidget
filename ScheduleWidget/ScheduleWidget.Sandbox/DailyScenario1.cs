using System;
using ScheduleWidget.ScheduledEvents;

namespace ScheduleWidget.Sandbox
{
    public static class DailyScenario1
    {
        public static void Run()
        {
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Running daily scenario 1 with a StartDateTime value of 12/1/2013");
            Console.WriteLine(Environment.NewLine);

            var aEvent = new Event()
            {
                ID = 1,
                Title = "Event 1",
                Frequency = 1,        // daily
                MonthlyInterval = 0,  // not applicable
                DaysOfWeek = 127,     // every day of week
                StartDateTime = new DateTime(2013, 12, 1)
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
