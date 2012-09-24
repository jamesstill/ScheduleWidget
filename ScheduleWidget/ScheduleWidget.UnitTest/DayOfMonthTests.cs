using System;
using ScheduleWidget.Enums;
using ScheduleWidget.ScheduledEvents;
using ScheduleWidget.TemporalExpressions;
using NUnit.Framework;

namespace ScheduleWidget.UnitTest
{
    [TestFixture]
    public class DayOfMonthTests
    {
        [Test]
        public void Day24()
        {
            var day = new DayOfMonthTE(24);

            Assert.IsTrue(day.Includes(new DateTime(2010, 6, 24)));
            Assert.IsTrue(day.Includes(new DateTime(2011, 1, 24)));
            Assert.IsTrue(day.Includes(new DateTime(2012, 9, 24)));

            Assert.IsFalse(day.Includes(new DateTime(2010, 8, 14)));
            Assert.IsFalse(day.Includes(new DateTime(2011, 3, 15)));
            Assert.IsFalse(day.Includes(new DateTime(2012, 8, 1)));
        }

        [Test]
        public void Day31()
        {
            var day = new DayOfMonthTE(24);

            var aEvent = CreateDay31Event();
            var schedule = new Schedule(aEvent);

            schedule.


            

            Assert.IsTrue(day.Includes(new DateTime(2010, 6, 24)));
            Assert.IsTrue(day.Includes(new DateTime(2011, 1, 24)));
            Assert.IsTrue(day.Includes(new DateTime(2012, 9, 24)));

            Assert.IsFalse(day.Includes(new DateTime(2010, 8, 14)));
            Assert.IsFalse(day.Includes(new DateTime(2011, 3, 15)));
            Assert.IsFalse(day.Includes(new DateTime(2012, 8, 1)));
        }

        private static Event CreateDay31Event()
        {
            return new Event()
            {
                ID = 1,
                Title = "Day 31 of Every Month",
                Frequency = 4       // monthly       
            };
        }
    }
}
