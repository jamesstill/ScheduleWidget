using System;
using NUnit.Framework;
using ScheduleWidget.Enums;
using ScheduleWidget.ScheduledEvents;
using ScheduleWidget.TemporalExpressions;

namespace ScheduleWidget.UnitTest
{
    [TestFixture]
    public class RangeEachYearTests
    {
        [Test]
        public void BasicRangeInYearTest1()
        {
            var range = new RangeEachYearTE(6, 9);
            Assert.IsTrue(range.Includes(new DateTime(2012, 6, 1)));
        }

        [Test]
        public void BasicRangeInYearTest2()
        {
            var range = new RangeEachYearTE(6, 9, 15, 30);
            Assert.IsFalse(range.Includes(new DateTime(2012, 6, 1)));
            Assert.IsTrue(range.Includes(new DateTime(2012, 6, 15)));
        }

        [Test]
        public void NonWinterRangeInYearTest()
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

            Assert.IsFalse(schedule.IsOccurring(new DateTime(2014, 11, 1)));
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2014, 12, 1)));
        }

        [Test]
        public void FineGrainedRangeInYearTest()
        {
            var holidays = GetHolidays();
            var aEvent = CreateFineGrainedStreetCleaningEvent();
            var schedule = new Schedule(aEvent, holidays);

            Assert.IsNotNull(holidays);
            Assert.IsNotNull(aEvent);
            Assert.IsNotNull(schedule);

            Assert.IsTrue(schedule.IsOccurring(new DateTime(2016, 6, 20)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2016, 5, 16)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2016, 6, 6)));

            Assert.IsTrue(schedule.IsOccurring(new DateTime(2018, 10, 15)));
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2018, 11, 5)));

            Assert.IsFalse(schedule.IsOccurring(new DateTime(2018, 1, 1)));
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2018, 2, 5)));
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2018, 3, 19)));

            Assert.IsFalse(schedule.IsOccurring(new DateTime(2014, 11, 1)));
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2014, 12, 1)));
        }

        [Test]
        public void FineGrainedRangeInYearNullEndDayTest()
        {
            var holidays = GetHolidays();
            var aEvent = CreateFineGrainedStreetCleaningEvent();

            // put up a null end day
            aEvent.RangeInYear.EndDayOfMonth = null;

            var schedule = new Schedule(aEvent, holidays);

            Assert.IsNotNull(holidays);
            Assert.IsNotNull(aEvent);
            Assert.IsNotNull(schedule);

            Assert.IsTrue(schedule.IsOccurring(new DateTime(2016, 6, 20)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2016, 5, 16)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2016, 6, 6)));

            Assert.IsTrue(schedule.IsOccurring(new DateTime(2018, 10, 15)));
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2018, 11, 5)));

            Assert.IsFalse(schedule.IsOccurring(new DateTime(2018, 1, 1)));
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2018, 2, 5)));
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2018, 3, 19)));

            Assert.IsFalse(schedule.IsOccurring(new DateTime(2014, 11, 1)));
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2014, 12, 1)));
        }

        [Test]
        public void FineGrainedRangeInYearNullStartDayTest()
        {
            var holidays = GetHolidays();
            var aEvent = CreateFineGrainedStreetCleaningEvent();

            // put up a null end day
            aEvent.RangeInYear.StartDayOfMonth = null;

            var schedule = new Schedule(aEvent, holidays);

            Assert.IsNotNull(holidays);
            Assert.IsNotNull(aEvent);
            Assert.IsNotNull(schedule);

            Assert.IsTrue(schedule.IsOccurring(new DateTime(2016, 6, 20)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2016, 5, 16)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2016, 6, 6)));

            Assert.IsTrue(schedule.IsOccurring(new DateTime(2018, 10, 15)));
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2018, 11, 5)));

            Assert.IsFalse(schedule.IsOccurring(new DateTime(2018, 1, 1)));
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2018, 2, 5)));
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2018, 3, 19)));

            Assert.IsFalse(schedule.IsOccurring(new DateTime(2014, 11, 1)));
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2014, 12, 1)));
        }

        [Test]
        public void FineGrainedRangeInYearNullRangeInYearTest()
        {
            var holidays = GetHolidays();
            var aEvent = CreateFineGrainedStreetCleaningEvent();

            // put up a null end day
            aEvent.RangeInYear = null;

            var schedule = new Schedule(aEvent, holidays);

            Assert.IsNotNull(holidays);
            Assert.IsNotNull(aEvent);
            Assert.IsNotNull(schedule);

            Assert.IsTrue(schedule.IsOccurring(new DateTime(2016, 6, 20)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2016, 5, 16)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2016, 6, 6)));

            Assert.IsTrue(schedule.IsOccurring(new DateTime(2018, 10, 15)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2018, 11, 5)));

            Assert.IsTrue(schedule.IsOccurring(new DateTime(2018, 1, 1)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2018, 2, 5)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2018, 3, 19)));

            Assert.IsFalse(schedule.IsOccurring(new DateTime(2014, 11, 1)));
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2014, 12, 2)));
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
                FrequencyTypeOptions = FrequencyTypeEnum.Weekly,       
                MonthlyIntervalOptions = MonthlyIntervalEnum.First | MonthlyIntervalEnum.Third,
                DaysOfWeekOptions = DayOfWeekEnum.Mon
            };
        }

        /// <summary>
        /// Fine-grained street cleaning occurs from April 4 to October 15 on the first and 
        /// third Monday of the month, excluding holidays. In this test there are two holidays: 
        /// July 4 and Labor Day (first Monday in Sep). The street cleaning example is taken 
        /// directly from Martin Fowler's white paper "Recurring Events for Calendars".
        /// </summary>
        private static Event CreateFineGrainedStreetCleaningEvent()
        {
            return new Event()
            {
                ID = 1,
                Title = "Fowler's Street Cleaning",
                RangeInYear = new RangeInYear()
                {
                    StartMonth = 4,      // April
                    StartDayOfMonth = 4,
                    EndMonth = 10,       // October
                    EndDayOfMonth = 15
                },
                FrequencyTypeOptions = FrequencyTypeEnum.Monthly,       
                MonthlyIntervalOptions = MonthlyIntervalEnum.First | MonthlyIntervalEnum.Third,
                DaysOfWeekOptions = DayOfWeekEnum.Mon
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
