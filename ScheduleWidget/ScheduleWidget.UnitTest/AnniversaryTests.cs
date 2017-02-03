using System;
using System.Linq;
using NUnit.Framework;
using ScheduleWidget.ScheduledEvents;
using ScheduleWidget.Enums;

namespace ScheduleWidget.UnitTest
{
    [TestFixture]
    public class AnniversaryTests
    {
        [Test]
        public void AnniversaryTest1()
        {
            var anniversary = new Anniversary()
            {
                Day = 1,
                Month = 8
            };

            var aEvent = new Event()
            {
                ID = 1,
                Title = "Daughter's Birthday",
                FrequencyTypeOptions = FrequencyTypeEnum.Yearly,
                Anniversary = anniversary
            };

            var schedule = new Schedule(aEvent);
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2008, 8, 1)));
        }

        [Test]
        public void AnniversaryTest2()
        {
            var anniversary = new Anniversary()
            {
                Day = 5,
                Month = 6
            };

            var aEvent = new Event()
            {
                ID = 1,
                Title = "Give Flowers to Wife",
                FrequencyTypeOptions = FrequencyTypeEnum.Yearly,
                Anniversary = anniversary
            };

            var schedule = new Schedule(aEvent);
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2009, 6, 4)));

            var range = new DateRange()
            {
                StartDateTime = new DateTime(2010, 1, 1),
                EndDateTime = new DateTime(2020, 12, 31)
            };

            var occurrences = schedule.Occurrences(range).ToList();
            Assert.IsTrue(occurrences.Count.Equals(11));
        }
    }
}
