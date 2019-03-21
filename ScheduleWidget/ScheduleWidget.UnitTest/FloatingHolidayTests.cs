using System;
using NUnit.Framework;
using ScheduleWidget.Enums;
using ScheduleWidget.ScheduledEvents;
using ScheduleWidget.TemporalExpressions;

namespace ScheduleWidget.UnitTest
{
    [TestFixture]
    public class FloatingHolidayTests
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
