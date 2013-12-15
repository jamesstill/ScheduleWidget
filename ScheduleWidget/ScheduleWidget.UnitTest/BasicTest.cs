using System;
using System.Collections.Generic;
using NUnit.Framework;
using ScheduleWidget.ScheduledEvents;

namespace ScheduleWidget.UnitTest
{
    [TestFixture]
    public class ScheduleTests
    {
        [Test]
        public void SimpleScheduleIsOccurringTest1()
        {
            var aEvent = CreateTodayEvent();
            var schedule = new Schedule(aEvent);
            Assert.IsNotNull(schedule);
            Assert.IsTrue(schedule.IsOccurring(DateTime.Today));
        }

        [Test]
        public void SimpleScheduleIsOccurringTest2()
        {
            var aEvent = CreateTodayEvent();
            var schedule = new Schedule(aEvent);
            Assert.IsNotNull(schedule);
            Assert.IsFalse(schedule.IsOccurring(DateTime.Today.AddDays(1)));
        }

        [Test]
        public void RecurringScheduleIsOccurringTest1()
        {
            var aEvent = CreateRecurringEvent();
            var schedule = new Schedule(aEvent);
            Assert.IsNotNull(schedule);
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2012, 10, 1)));
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2013, 1, 18)));
        }

        [Test]
        public void RecurringSchedulePreviousOccurrenceTest1()
        {
            var aEvent = CreateRecurringEvent();
            var schedule = new Schedule(aEvent);
            Assert.IsNotNull(schedule);
            var previousOccurrence = schedule.PreviousOccurrence(new DateTime(2013, 1, 2));
            Assert.IsFalse(previousOccurrence == new DateTime(2012, 12, 17));
            Assert.IsTrue(previousOccurrence == new DateTime(2012, 12, 19));
        }

        [Test]
        public void RecurringScheduleNextOccurrenceTest1()
        {
            var aEvent = CreateRecurringEvent();
            var schedule = new Schedule(aEvent);
            Assert.IsNotNull(schedule);
            var nextOccurrence = schedule.NextOccurrence(new DateTime(2012, 12, 17));
            Assert.IsFalse(nextOccurrence == new DateTime(2013, 1, 2));
        }

        [Test]
        public void RecurringScheduleNextOccurrenceTest2()
        {
            var aEvent = CreateRecurringEvent();
            var schedule = new Schedule(aEvent);
            Assert.IsNotNull(schedule);
            var nextOccurrence = schedule.NextOccurrence(new DateTime(2012, 12, 19));
            Assert.IsTrue(nextOccurrence == new DateTime(2013, 1, 2));
        }

        [Test]
        public void RecurringScheduleExclusionsTest1()
        {
            var aEvent = CreateRecurringEvent();
            var excludedDays = new List<DateTime>()
            {
                new DateTime(2012, 12, 5),
                new DateTime(2012, 12, 25),
                new DateTime(2013, 1, 2),
                new DateTime(2013, 2, 18)
            };

            var schedule = new Schedule(aEvent, excludedDays);
            Assert.IsNotNull(schedule);
            var nextOccurrence = schedule.NextOccurrence(new DateTime(2012, 12, 19));
            Assert.IsTrue(nextOccurrence == new DateTime(2013, 1, 7));
        }

        [Test]
        public void RecurringScheduleExclusionsTest2()
        {
            var aEvent = CreateRecurringEvent();
            var excludedDays = new List<DateTime>()
            {
                new DateTime(2012, 12, 5),
                new DateTime(2012, 12, 25),
                new DateTime(2013, 1, 2),
                new DateTime(2013, 2, 18)
            };

            var schedule = new Schedule(aEvent, excludedDays);
            Assert.IsNotNull(schedule);
            var nextOccurrence = schedule.NextOccurrence(new DateTime(2013, 1, 1));
            Assert.IsTrue(nextOccurrence == new DateTime(2013, 1, 7));
        }

        private static Event CreateTodayEvent()
        {
            return new Event()
            {
                ID = 1,
                Title = "My One-Time Event Today",
                OneTimeOnlyEventDate = DateTime.Today,
                DaysOfWeek = 0,
                Frequency = 0,
                MonthlyInterval = 0
            };
        }

        private static Event CreateRecurringEvent()
        {
            return new Event()
            {
                ID = 1,
                Title = "My Recurring Event",
                Frequency = 4,        // monthly
                MonthlyInterval = 5,  // first and third of month
                DaysOfWeek = 10       // Mon and Wed
            };
        }
    }
}
