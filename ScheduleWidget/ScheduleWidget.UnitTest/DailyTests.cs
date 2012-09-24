using System;
using NUnit.Framework;
using ScheduleWidget.ScheduledEvents;

namespace ScheduleWidget.UnitTest
{
    [TestFixture]
    public class DailyTests
    {
        [Test]
        public void DailyEventTest1()
        {
            var aEvent = new Event()
            {
                ID = 1,
                Title = "Event 1",
                Frequency = 1,        // daily
                MonthlyInterval = 0,  // not applicable
                DaysOfWeek = 127      // every day of week
            };

            var schedule = new Schedule(aEvent);

            Assert.IsTrue(schedule.IsOccurring(new DateTime(2013, 2, 10)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2013, 4, 29)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2013, 11, 17)));
        }

        [Test]
        public void DailyEventTest2()
        {
            var aEvent = new Event()
            {
                ID = 1,
                Title = "Event 2",
                RangeInYear = null,
                Frequency = 1,        // daily
                MonthlyInterval = 0,  // not applicable
                DaysOfWeek = 16       // Thursday
            };

            var schedule = new Schedule(aEvent);

            Assert.IsTrue(schedule.IsOccurring(new DateTime(2013, 2, 14)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2013, 4, 25)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2013, 11, 7)));
        }
    }
}
