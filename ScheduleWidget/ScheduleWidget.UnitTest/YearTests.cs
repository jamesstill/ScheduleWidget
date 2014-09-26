using System;
using System.Linq;
using NUnit.Framework;
using ScheduleWidget.ScheduledEvents;
using ScheduleWidget.Enums;

namespace ScheduleWidget.UnitTest
{
    [TestFixture]
    public class YearTests
    {
        [Test]
        public void YearTest1()
        {
            var aEvent = new Event()
            {
                ID = 1,
                Title = "Event 1",
                FrequencyTypeOptions = FrequencyTypeEnum.Yearly,
                Anniversary = new Anniversary()
                {
                    Month = 9,
                    Day = 27
                }
            };

            var schedule = new Schedule(aEvent);
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2014, 9, 27)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2015, 9, 27)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2016, 9, 27)));
        }

        [Test]
        public void YearTest2()
        {
            var aEvent = new Event()
            {
                ID = 1,
                Title = "Event 2",
                FrequencyTypeOptions = FrequencyTypeEnum.Yearly,
                Anniversary = new Anniversary()
                {
                    Month = 9,
                    Day = 27
                }
            };

            var schedule = new Schedule(aEvent);
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2014, 9, 27)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2015, 9, 27)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2016, 9, 27)));

            var range = new DateRange()
            {
                StartDateTime = new DateTime(2012, 1, 1),
                EndDateTime = new DateTime(2016, 12, 31)
            };


            var occurrences = schedule.Occurrences(range);

            Assert.AreEqual(5, occurrences.Count());
            Assert.AreEqual(new DateTime(2012, 9, 27), occurrences.First());
            Assert.AreEqual(new DateTime(2016, 9, 27), occurrences.Last());
        }

        [Test]
        public void YearTest3()
        {
            var aEvent = new Event()
            {
                ID = 1,
                Title = "Event 3",
                FrequencyTypeOptions = FrequencyTypeEnum.Yearly,
                StartDateTime = new DateTime(2013, 9, 27),
                RepeatInterval = 2,
                Anniversary = new Anniversary()
                {
                    Month = 9,
                    Day = 27
                }
            };

            var schedule = new Schedule(aEvent);
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2014, 9, 27)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2015, 9, 27)));
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2016, 9, 27)));
        }
        [Test]
        public void YearTest4()
        {
            var aEvent = new Event()
            {
                ID = 1,
                Title = "Event 4",
                FrequencyTypeOptions = FrequencyTypeEnum.Yearly,
                StartDateTime = new DateTime(2013, 9, 28),
                RepeatInterval = 2,
                Anniversary = new Anniversary()
                {
                    Month = 9,
                    Day = 27
                }
            };

            var schedule = new Schedule(aEvent);
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2014, 9, 27)));
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2015, 9, 27)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2016, 9, 27)));
        }
    }
}
