using System;
using NUnit.Framework;
using ScheduleWidget.Enums;
using ScheduleWidget.ScheduledEvents;
using ScheduleWidget.TemporalExpressions;

namespace ScheduleWidget.UnitTest
{
    [TestFixture]
    public class StreetCleaningTests
    {
        [Test]
        public void StreetCleaningLaborDayTest()
        {
            var holidays = GetHolidays();
            var aEvent = CreateStreetCleaningEvent();
            var schedule = new Schedule(aEvent, holidays);

            Assert.IsNotNull(holidays);
            Assert.IsNotNull(aEvent);
            Assert.IsNotNull(schedule);

            // Labor Day
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2012, 9, 3)));
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2013, 9, 2)));
            // Third Monday in Sep
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2014, 9, 15)));
        }

        [Test]
        public void StreetCleaningIndependenceDayTest()
        {
            var holidays = GetHolidays();
            var aEvent = CreateStreetCleaningEvent();
            var schedule = new Schedule(aEvent, holidays);

            Assert.IsNotNull(holidays);
            Assert.IsNotNull(aEvent);
            Assert.IsNotNull(schedule);
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2012, 7, 4)));
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2016, 7, 4)));
        }

        [Test]
        public void StreetCleaningPreviousOccurrenceTest()
        {
            var holidays = GetHolidays();
            var aEvent = CreateStreetCleaningEvent();
            var schedule = new Schedule(aEvent, holidays);

            Assert.IsNotNull(holidays);
            Assert.IsNotNull(aEvent);
            Assert.IsNotNull(schedule);

            var prev = schedule.PreviousOccurrence(new DateTime(2016, 6, 6));
            Assert.IsTrue(prev.Equals(new DateTime(2016, 5, 16)));
            Assert.IsFalse(prev.Equals(new DateTime(2016, 6, 6)));
            Assert.IsFalse(prev.Equals(new DateTime(2016, 6, 20)));
        }

        [Test]
        public void StreetCleaningNextOccurrenceTest()
        {
            var holidays = GetHolidays();
            var aEvent = CreateStreetCleaningEvent();
            var schedule = new Schedule(aEvent, holidays);

            Assert.IsNotNull(holidays);
            Assert.IsNotNull(aEvent);
            Assert.IsNotNull(schedule);

            var next = schedule.NextOccurrence(new DateTime(2016, 6, 6));
            Assert.IsTrue(next.Equals(new DateTime(2016, 6, 20)));
            Assert.IsFalse(next.Equals(new DateTime(2016, 5, 16)));
            Assert.IsFalse(next.Equals(new DateTime(2016, 6, 6)));
        }


        [Test]
        public void StreetCleaningIsOccurringTest()
        {
            var holidays = GetHolidays();
            var aEvent = CreateStreetCleaningEvent();
            var schedule = new Schedule(aEvent, holidays);

            Assert.IsNotNull(holidays);
            Assert.IsNotNull(aEvent);
            Assert.IsNotNull(schedule);

            Assert.IsTrue(schedule.IsOccurring(new DateTime(2016, 6, 20)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2016, 5, 16)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2016, 6, 6)));

            Assert.IsFalse(schedule.IsOccurring(new DateTime(2018, 1, 1)));
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2018, 2, 5)));
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2018, 3, 19)));
        }

        /// <summary>
        /// Street cleaning occurs from April to October on the first and third Monday of the
        /// month, excluding holidays. In this test there are two holidays: July 4 and Labor
        /// Day (first Monday in Sep). The street cleaning example is taken directly from 
        /// Martin Fowler's white paper "Recurring Events for Calendars".
        /// </summary>
        private static Event CreateStreetCleaningEvent()
        {
            return new Event()
            {
                ID = 1,
                Title = "Fowler's Street Cleaning",
                RangeInYear = new RangeInYear()
                {
                    StartMonth = 4,      // April
                    EndMonth = 10,       // October
                },
                Frequency = 4,       // monthly       
                MonthlyInterval = 5, // first and third of month
                DaysOfWeek = 2,       // Mon
                StartDateTime = new DateTime(2000, 1, 1),
                EndDateTime = new DateTime(2020, 1, 1)
            };
        }

        private static UnionTE GetHolidays()
        {
            var union = new UnionTE();
            union.Add(new FixedHolidayTE(7, 4));
            union.Add(new FloatingHolidayTE(9, DayOfWeekEnum.Mon, MonthlyIntervalEnum.First));
            return union;
        }
    }
}
