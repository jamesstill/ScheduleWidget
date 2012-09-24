using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using ScheduleWidget.ScheduledEvents;
using ScheduleWidget.TemporalExpressions;

namespace ScheduleWidget.UnitTest
{
    [TestFixture]
    public class OneTimeOnlyTests
    {
        [Test]
        public void OneTimeOnlyTest1()
        {
            var laborDay = new DateTime(2012, 9, 1);

            var aEvent = new Event()
            {
                ID = 1,
                Title = "Labor Day Extravaganza",
                Frequency = 0,        // one-time only
                OneTimeOnlyEventDate = laborDay,
                MonthlyInterval = 0,  // not applicable
                DaysOfWeek = 0        // not applicable
            };

            var schedule = new Schedule(aEvent);
            Assert.IsTrue(schedule.IsOccurring(laborDay));
        }

        [Test]
        public void OneTimeOnlyTest2()
        {
            var laborDay = new DateTime(2012, 9, 1);

            var aEvent = new Event()
            {
                ID = 1,
                Title = "Labor Day Extravaganza",
                Frequency = 0,        // one-time only
                OneTimeOnlyEventDate = laborDay,
                MonthlyInterval = 0,  // not applicable
                DaysOfWeek = 0        // not applicable
            };

            var schedule = new Schedule(aEvent);

            var range = new DateRange()
            {
                StartDateTime = new DateTime(2012, 8, 1),
                EndDateTime = new DateTime(2012, 10, 1)
            };

            var occurrences = schedule.Occurrences(range);
            var count = 0;
            using (var items = occurrences.GetEnumerator())
            {
                while (items.MoveNext())
                    count++;
            }

            Assert.IsTrue(count > 0);
        }
    }
}
