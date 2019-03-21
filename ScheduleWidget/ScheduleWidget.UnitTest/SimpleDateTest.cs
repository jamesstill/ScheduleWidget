using System;
using System.Collections.Generic;
using NUnit.Framework;
using ScheduleWidget.ScheduledEvents;
using ScheduleWidget.TemporalExpressions;

namespace ScheduleWidget.UnitTest
{
    [TestFixture]
    public class SimpleDateTests
    {
        [Test]
        public void SimpleExcludedDayTest1()
        {
            var date = new DateTE(DateTime.Today);

            Assert.IsTrue(date.Includes(DateTime.Today));
            Assert.IsFalse(date.Includes(DateTime.Today.AddDays(1)));
        }

        [Test]
        public void SimpleExcludedDayTest2()
        {
            // first Mon of Sep
            var excludedDay = new DateTime(2012, 9, 3);
            var aEvent = CreateRecurringEvent();
            var excludedDates = new List<DateTime>()
            {
                excludedDay
            };

            var schedule = new Schedule(aEvent, excludedDates);
            Assert.IsNotNull(schedule);
            Assert.IsFalse(schedule.IsOccurring(excludedDay));
        }

        private static Event CreateRecurringEvent()
        {
            return new Event()
            {
                ID = 1,
                Title = "My Recurring Event",
                FrequencyTypeOptions = Enums.FrequencyTypeEnum.Monthly,
                MonthlyIntervalOptions = Enums.MonthlyIntervalEnum.First | Enums.MonthlyIntervalEnum.Third,
                DaysOfWeekOptions = Enums.DayOfWeekEnum.Mon | Enums.DayOfWeekEnum.Wed
            };
        }
    }
}
