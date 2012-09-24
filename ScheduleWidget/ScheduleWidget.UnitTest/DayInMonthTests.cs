using System;
using ScheduleWidget.Enums;
using ScheduleWidget.TemporalExpressions;
using NUnit.Framework;

namespace ScheduleWidget.UnitTest
{
    [TestFixture]
    public class DayInMonthTests
    {
        [Test]
        public void SundayTest()
        {
            var sunday = new DayInMonthTE(DayOfWeekEnum.Sun);

            Assert.IsTrue(sunday.Includes(new DateTime(2010, 6, 27)));
            Assert.IsTrue(sunday.Includes(new DateTime(2011, 1, 2)));
            Assert.IsTrue(sunday.Includes(new DateTime(2012, 9, 30)));

            Assert.IsFalse(sunday.Includes(new DateTime(2010, 8, 14)));
            Assert.IsFalse(sunday.Includes(new DateTime(2011, 3, 15)));
            Assert.IsFalse(sunday.Includes(new DateTime(2012, 8, 1)));
        }

        [Test]
        public void SaturdayTest()
        {
            var saturday = new DayInMonthTE(DayOfWeekEnum.Sat);

            Assert.IsTrue(saturday.Includes(new DateTime(2010, 6, 26)));
            Assert.IsTrue(saturday.Includes(new DateTime(2011, 1, 1)));
            Assert.IsTrue(saturday.Includes(new DateTime(2012, 9, 29)));

            Assert.IsFalse(saturday.Includes(new DateTime(2010, 4, 29)));
            Assert.IsFalse(saturday.Includes(new DateTime(2011, 9, 1)));
            Assert.IsFalse(saturday.Includes(new DateTime(2012, 2, 10)));
        }

        [Test]
        public void SecondTuesdayOfMonthTest()
        {
            var secondTuesday = new DayInMonthTE(DayOfWeekEnum.Tue, MonthlyIntervalEnum.Second);

            Assert.IsTrue(secondTuesday.Includes(new DateTime(2010, 6, 8)));
            Assert.IsTrue(secondTuesday.Includes(new DateTime(2011, 1, 11)));
            Assert.IsTrue(secondTuesday.Includes(new DateTime(2012, 11, 13)));

            Assert.IsFalse(secondTuesday.Includes(new DateTime(2012, 11, 19)));
            Assert.IsFalse(secondTuesday.Includes(new DateTime(2012, 1, 2)));
            Assert.IsFalse(secondTuesday.Includes(new DateTime(2012, 10, 29)));
        }

        [Test]
        public void LastFridayOfMonthTest()
        {
            var lastFriday = new DayInMonthTE(DayOfWeekEnum.Fri, MonthlyIntervalEnum.Last);

            Assert.IsTrue(lastFriday.Includes(new DateTime(2010, 1, 29)));
            Assert.IsTrue(lastFriday.Includes(new DateTime(2011, 6, 24)));
            Assert.IsTrue(lastFriday.Includes(new DateTime(2012, 9, 28)));

            Assert.IsFalse(lastFriday.Includes(new DateTime(2010, 1, 28)));
            Assert.IsFalse(lastFriday.Includes(new DateTime(2010, 1, 22)));
            Assert.IsFalse(lastFriday.Includes(new DateTime(2012, 7, 31)));
        }

        [Test]
        public void LastWeekendEveryMonthTest()
        {
            var weekend = new DayInMonthTE(DayOfWeekEnum.Sat | DayOfWeekEnum.Sun, MonthlyIntervalEnum.Last);
            Assert.LessOrEqual(weekend.GetHashCode(), 0);

            var lastWeekendAug2012 = new DayInMonthTE(DayOfWeekEnum.Sat | DayOfWeekEnum.Sun, MonthlyIntervalEnum.Last);
            Assert.IsTrue(weekend.Equals(lastWeekendAug2012));

            var firstWeekendAug2012 = new DayInMonthTE(DayOfWeekEnum.Sat | DayOfWeekEnum.Sun, MonthlyIntervalEnum.First);
            Assert.IsFalse(weekend.Equals(firstWeekendAug2012));

            Assert.IsFalse(weekend.Equals(null));

            Assert.IsFalse(weekend.Equals(1)); // unknown object
        }

        [Test]
        public void ThirdWeekendEveryMonthTest()
        {
            var weekend = new DayInMonthTE(DayOfWeekEnum.Sat | DayOfWeekEnum.Sun, MonthlyIntervalEnum.Third);
            Assert.IsTrue(weekend.GetHashCode().Equals(3));

            var thirdWeekendAug2012 = new DayInMonthTE(DayOfWeekEnum.Sat | DayOfWeekEnum.Sun, MonthlyIntervalEnum.Third);
            Assert.IsTrue(weekend.Equals(thirdWeekendAug2012));

            var firstWeekendAug2012 = new DayInMonthTE(DayOfWeekEnum.Sat | DayOfWeekEnum.Sun, MonthlyIntervalEnum.First);
            Assert.IsFalse(weekend.Equals(firstWeekendAug2012));

            Assert.IsFalse(weekend.Equals(null));
        }
    }
}
