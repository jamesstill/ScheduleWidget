using System;
using System.Linq;
using NUnit.Framework;
using ScheduleWidget.ScheduledEvents;
using ScheduleWidget.Enums;
using ScheduleWidget.TemporalExpressions;

namespace ScheduleWidget.UnitTest
{
    [TestFixture]
    public class MonthTests
    {
        [Test]
        public void MonthTest1()
        {
            var aEvent = new Event()
            {
                ID = 1,
                Title = "Event 1",
                FrequencyTypeOptions = FrequencyTypeEnum.Monthly,
                MonthlyIntervalOptions = MonthlyIntervalEnum.EveryWeek,
                DaysOfWeekOptions = DayOfWeekEnum.Mon | DayOfWeekEnum.Fri
            };

            var range = new DateRange()
            {
                StartDateTime = new DateTime(2013, 1, 15),
                EndDateTime = new DateTime(2013, 4, 30)
            };

            var occurringDate = new DateTime(2013, 1, 21);

            var schedule = new Schedule(aEvent);
            Assert.IsTrue(schedule.IsOccurring(occurringDate));

            var previousOccurrence = schedule.PreviousOccurrence(occurringDate);
            Assert.AreEqual(new DateTime(2013, 1, 18), previousOccurrence.Value);

            var nextOccurrence = schedule.NextOccurrence(occurringDate);
            Assert.AreEqual(new DateTime(2013, 1, 25), nextOccurrence.Value);

            var occurrences = schedule.Occurrences(range);

            Assert.AreEqual(30, occurrences.Count());
            Assert.AreEqual(new DateTime(2013, 1, 18), occurrences.First());
            Assert.AreEqual(new DateTime(2013, 4, 29), occurrences.Last());
        }

        [Test]
        public void MonthTest2()
        {
            var aEvent = new Event()
            {
                ID = 1,
                Title = "Event 2",
                FrequencyTypeOptions = FrequencyTypeEnum.Monthly,
                RepeatInterval = 2,
                MonthlyIntervalOptions = MonthlyIntervalEnum.Last,
                DaysOfWeekOptions = DayOfWeekEnum.Mon | DayOfWeekEnum.Fri,
                StartDateTime = new DateTime(2013, 1, 15)
            };

            var range = new DateRange()
            {
                StartDateTime = aEvent.StartDateTime.Value,
                EndDateTime = new DateTime(2013, 4, 30)
            };

            var nonOccurringDate = new DateTime(2013, 1, 29);

            var schedule = new Schedule(aEvent);
            Assert.IsFalse(schedule.IsOccurring(nonOccurringDate));

            var previousOccurrence = schedule.PreviousOccurrence(nonOccurringDate);
            Assert.AreEqual(new DateTime(2013, 1, 28), previousOccurrence.Value);

            var nextOccurrence = schedule.NextOccurrence(nonOccurringDate);
            Assert.AreEqual(new DateTime(2013, 3, 25), nextOccurrence.Value);

            var occurrences = schedule.Occurrences(range);

            Assert.AreEqual(4, occurrences.Count());
            Assert.AreEqual(new DateTime(2013, 1, 25), occurrences.First());
            Assert.AreEqual(new DateTime(2013, 3, 29), occurrences.Last());
        }

        [Test]
        public void MonthTest3()
        {
            var aEvent = new Event()
            {
                ID = 1,
                Title = "Event 3",
                FrequencyTypeOptions = FrequencyTypeEnum.Monthly,
                DayOfMonth = 3
            };

            var range = new DateRange()
            {
                StartDateTime = new DateTime(2013, 1, 1),
                EndDateTime = new DateTime(2014, 12, 31)
            };

            var occurringDate = new DateTime(2013, 12, 3);

            var schedule = new Schedule(aEvent);
            Assert.IsTrue(schedule.IsOccurring(occurringDate));

            var previousOccurrence = schedule.PreviousOccurrence(occurringDate);
            Assert.AreEqual(new DateTime(2013, 11, 3), previousOccurrence.Value);

            var nextOccurrence = schedule.NextOccurrence(occurringDate);
            Assert.AreEqual(new DateTime(2014, 1, 3), nextOccurrence.Value);

            var occurrences = schedule.Occurrences(range);

            Assert.AreEqual(24, occurrences.Count());
            Assert.AreEqual(new DateTime(2013, 1, 3), occurrences.First());
            Assert.AreEqual(new DateTime(2014, 12, 3), occurrences.Last());
        }

        [Test]
        public void MonthTest4()
        {
            var aEvent = new Event()
            {
                ID = 1,
                Title = "Event 4",
                FrequencyTypeOptions = FrequencyTypeEnum.Monthly,
                RepeatInterval = 3,
                DayOfMonth = 30,
                StartDateTime = new DateTime(2013, 2, 1)
            };

            var range = new DateRange()
            {
                StartDateTime = aEvent.StartDateTime.Value,
                EndDateTime = new DateTime(2014, 12, 31)
            };

            var occurringDate = new DateTime(2014, 2, 28);

            var schedule = new Schedule(aEvent);
            Assert.IsTrue(schedule.IsOccurring(occurringDate));

            var previousOccurrence = schedule.PreviousOccurrence(occurringDate);
            Assert.AreEqual(new DateTime(2013, 11, 30), previousOccurrence.Value);

            var nextOccurrence = schedule.NextOccurrence(occurringDate);
            Assert.AreEqual(new DateTime(2014, 5, 30), nextOccurrence.Value);

            var occurrences = schedule.Occurrences(range);

            Assert.AreEqual(8, occurrences.Count());
            Assert.AreEqual(new DateTime(2013, 2, 28), occurrences.First());
            Assert.AreEqual(new DateTime(2014, 11, 30), occurrences.Last());
        }

        [Test]
        public void MonthTest5()
        {
            var holidays = new UnionTE();
            holidays.Add(new FixedHolidayTE(1, 25));
            holidays.Add(new FloatingHolidayTE(3, DayOfWeekEnum.Mon, MonthlyIntervalEnum.First));

            var aEvent = new Event()
            {
                ID = 1,
                Title = "Event 5",
                FrequencyTypeOptions = FrequencyTypeEnum.Monthly,
                MonthlyIntervalOptions = MonthlyIntervalEnum.EveryWeek,
                DaysOfWeekOptions = DayOfWeekEnum.Mon | DayOfWeekEnum.Fri
            };

            var range = new DateRange()
            {
                StartDateTime = new DateTime(2013, 1, 15),
                EndDateTime = new DateTime(2013, 4, 30)
            };

            var occurringDate = new DateTime(2013, 1, 21);

            var schedule = new Schedule(aEvent, holidays);
            Assert.IsTrue(schedule.IsOccurring(occurringDate));
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2013, 3, 4)));

            var previousOccurrence = schedule.PreviousOccurrence(occurringDate);
            Assert.AreEqual(new DateTime(2013, 1, 18), previousOccurrence.Value);

            var nextOccurrence = schedule.NextOccurrence(occurringDate);
            Assert.AreEqual(new DateTime(2013, 1, 28), nextOccurrence.Value);

            var occurrences = schedule.Occurrences(range);

            Assert.AreEqual(28, occurrences.Count());
            Assert.AreEqual(new DateTime(2013, 1, 18), occurrences.First());
            Assert.AreEqual(new DateTime(2013, 4, 29), occurrences.Last());
        }
    }
}
