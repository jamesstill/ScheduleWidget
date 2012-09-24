using System;
using System.Xml.Linq;
using NUnit.Framework;
using ScheduleWidget.Enums;
using ScheduleWidget.ScheduledEvents;
using ScheduleWidget.TemporalExpressions;

namespace ScheduleWidget.UnitTest
{
    [TestFixture]
    public class FixedHolidayTests
    {
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
                DaysOfWeek = 2       // Mon
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
