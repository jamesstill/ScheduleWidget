using System;
using NUnit.Framework;
using ScheduleWidget.ScheduledEvents;
using ScheduleWidget.Enums;

namespace ScheduleWidget.UnitTest
{
    [TestFixture]
    public class LastOccurrenceTests
    {
        [Test]
        public void LastOccurrenceTest1()
        {
            var aEvent = new Event()
            {
                ID = 1,
                Title = "Daily, 3 times",
                FrequencyTypeOptions = FrequencyTypeEnum.Daily,
                NumberOfOccurrences = 3,
                StartDateTime = new DateTime(2013, 7, 29)
            };

            var schedule = new Schedule(aEvent);
            DateTime? endDate = schedule.GetLastOccurrenceDate();
            Assert.IsTrue(endDate == new DateTime(2013, 7, 31));
        }

        [Test]
        public void LastOccurrenceTest2()
        {
            var aEvent = new Event()
            {
                ID = 2,
                Title = "Every 2 days, 3 times",
                FrequencyTypeOptions = FrequencyTypeEnum.Daily,
                NumberOfOccurrences = 3,
                RepeatInterval = 2,
                StartDateTime = new DateTime(2013, 7, 29)
            };

            var schedule = new Schedule(aEvent);
            DateTime? endDate = schedule.GetLastOccurrenceDate();
            Assert.IsTrue(endDate == new DateTime(2013, 8, 2));
        }

        [Test]
        public void LastOccurrenceTest3()
        {
            var aEvent = new Event()
            {
                ID = 3,
                Title = "Weekly on Monday, Tuesday, 3 times",
                FrequencyTypeOptions = FrequencyTypeEnum.Weekly,
                DaysOfWeek = 6,
                NumberOfOccurrences = 3,
                StartDateTime = new DateTime(2013, 7, 29)
            };

            var schedule = new Schedule(aEvent);
            DateTime? endDate = schedule.GetLastOccurrenceDate();
            Assert.IsTrue(endDate == new DateTime(2013, 8, 5));
        }

        [Test]
        public void LastOccurrenceTest4()
        {
            var aEvent = new Event()
            {
                ID = 4,
                Title = "Every 2 weeks on Monday, Tuesday, Wednesday, 10 times",
                FrequencyTypeOptions = FrequencyTypeEnum.Weekly,
                DaysOfWeek = 14,
                RepeatInterval = 2,
                NumberOfOccurrences = 10,
                StartDateTime = new DateTime(2013, 7, 29)
            };

            var schedule = new Schedule(aEvent);
            DateTime? endDate = schedule.GetLastOccurrenceDate();
            Assert.IsTrue(endDate == new DateTime(2013, 9, 9));
        }

        [Test]
        public void LastOccurrenceTest5()
        {
            var aEvent = new Event()
            {
                ID = 5,
                Title = "Monthly on day 29, 3 times",
                FrequencyTypeOptions = FrequencyTypeEnum.Monthly,
                DayOfMonth = 29,
                NumberOfOccurrences = 3,
                StartDateTime = new DateTime(2013, 7, 29)
            };

            var schedule = new Schedule(aEvent);
            DateTime? endDate = schedule.GetLastOccurrenceDate();
            Assert.IsTrue(endDate == new DateTime(2013, 9, 29));
        }

        [Test]
        public void LastOccurrenceTest6()
        {
            var aEvent = new Event()
            {
                ID = 6,
                Title = "Every 2 months on day 29, 3 times",
                FrequencyTypeOptions = FrequencyTypeEnum.Monthly,
                DayOfMonth = 29,
                NumberOfOccurrences = 3,
                RepeatInterval = 2,
                StartDateTime = new DateTime(2013, 7, 29)
            };

            var schedule = new Schedule(aEvent);
            DateTime? endDate = schedule.GetLastOccurrenceDate();
            Assert.IsTrue(endDate == new DateTime(2013, 11, 29));
        }


        [Test]
        public void LastOccurrenceTest7()
        {
            var aEvent = new Event()
            {
                ID = 7,
                Title = "Monthly on the last Monday, 3 times",
                FrequencyTypeOptions = FrequencyTypeEnum.Monthly,
                DaysOfWeek = 2,
                NumberOfOccurrences = 3,
                MonthlyIntervalOptions = MonthlyIntervalEnum.Last,
                StartDateTime = new DateTime(2013, 7, 29)
            };

            var schedule = new Schedule(aEvent);
            DateTime? endDate = schedule.GetLastOccurrenceDate();
            Assert.IsTrue(endDate == new DateTime(2013, 9, 30));
        }

        [Test]
        public void LastOccurrenceTest8()
        {
            var aEvent = new Event()
            {
                ID = 8,
                Title = "Every 2 months on the last Monday, 3 times",
                FrequencyTypeOptions = FrequencyTypeEnum.Monthly,
                DaysOfWeek = 2,
                NumberOfOccurrences = 3,
                RepeatInterval = 2,
                MonthlyIntervalOptions = MonthlyIntervalEnum.Last,
                StartDateTime = new DateTime(2013, 7, 29)
            };

            var schedule = new Schedule(aEvent);
            DateTime? endDate = schedule.GetLastOccurrenceDate();
            Assert.IsTrue(endDate == new DateTime(2013, 11, 25));
        }

        [Test]
        public void LastOccurrenceTest9()
        {
            var aEvent = new Event()
            {
                ID = 9,
                Title = "Every 2 months on the last Monday, 3 times",
                FrequencyTypeOptions = FrequencyTypeEnum.Monthly,
                DaysOfWeek = 2,
                NumberOfOccurrences = 3,
                RepeatInterval = 2,
                MonthlyIntervalOptions = MonthlyIntervalEnum.Last,
                StartDateTime = new DateTime(2013, 7, 29)
            };

            var schedule = new Schedule(aEvent);
            DateTime? endDate = schedule.GetLastOccurrenceDate();
            Assert.IsTrue(endDate == new DateTime(2013, 11, 25));
        }

        [Test]
        public void LastOccurrenceTest10()
        {
            var aEvent = new Event()
            {
                ID = 10,
                Title = "Annually on July 29, 2 times",
                FrequencyTypeOptions = FrequencyTypeEnum.Yearly,
                NumberOfOccurrences = 2,
                Anniversary = new Anniversary{ Day = 29,Month = 7},
                StartDateTime = new DateTime(2013, 7, 29)
            };

            var schedule = new Schedule(aEvent);
            DateTime? endDate = schedule.GetLastOccurrenceDate();
            Assert.IsTrue(endDate == new DateTime(2014, 7, 29));
        }

        [Test]
        public void LastOccurrenceTest11()
        {
            var aEvent = new Event()
            {
                ID = 10,
                Title = "Every 2 years on July 29, 2 times",
                FrequencyTypeOptions = FrequencyTypeEnum.Yearly,
                NumberOfOccurrences = 2,
                RepeatInterval = 2,
                Anniversary = new Anniversary { Day = 29, Month = 7 },
                StartDateTime = new DateTime(2013, 7, 29)
            };

            var schedule = new Schedule(aEvent);
            DateTime? endDate = schedule.GetLastOccurrenceDate();
            Assert.IsTrue(endDate == new DateTime(2015, 7, 29));
        }
    }
}


