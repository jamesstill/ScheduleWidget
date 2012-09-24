using System;
using NUnit.Framework;
using ScheduleWidget.ScheduledEvents;

namespace ScheduleWidget.UnitTest
{
    [TestFixture]
    public class WeeklyTests
    {
        [Test]
        public void WeeklyEventTest1()
        {
            var aEvent = new Event()
            {
                ID = 1,
                Title = "Every Mon and Wed",
                Frequency = 2,        // weekly
                MonthlyInterval = 0,  // not applicable
                DaysOfWeek = 10      // every Mon and Wed
            };

            var schedule = new Schedule(aEvent);

            // NOTE: If daily then date range doesn't matter
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2016, 2, 8)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2015, 11, 25)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2014, 6, 16)));

            Assert.IsFalse(schedule.IsOccurring(new DateTime(2012, 8, 9))); // Thu
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2013, 2, 5))); // Tue
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2014, 7, 5))); // Sat
        }

        [Test]
        public void WeeklyEventTest2()
        {
            var aEvent = new Event()
            {
                ID = 1,
                Title = "Every Mon Wed Fri",
                Frequency = 2,        // weekly
                MonthlyInterval = 0,  // not applicable
                DaysOfWeek = 42      // every Mon, Wed and Fri
            };

            var schedule = new Schedule(aEvent);

            Assert.IsTrue(schedule.IsOccurring(new DateTime(2016, 2, 8)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2015, 11, 25)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2014, 6, 16)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2012, 10, 12)));

            Assert.IsFalse(schedule.IsOccurring(new DateTime(2012, 8, 9))); // Thu
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2013, 2, 5))); // Tue
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2014, 7, 5))); // Sat
        }
    }
}
