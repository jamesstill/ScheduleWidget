using System;
using System.Xml;
using NUnit.Framework;
using ScheduleWidget.ScheduledEvents;
using ScheduleWidget.TemporalExpressions;

namespace ScheduleWidget.UnitTest
{
    [TestFixture]
    public class ScheduleDeserializationTests
    {
        [Test]
        public void DeserializeScheduleTest()
        {
            var schedule = GetSchedule();
            var doc = schedule.ToXml();
            
            // given the doc deserialize it back into a Schedule object
            var anotherSchedule = new Schedule(doc);
        }

        [Test]
        public void ScheduleDocCreationTest()
        {
            
            var schedule = GetSchedule();
            var doc = schedule.ToXml();
            Assert.IsNotNull(doc);
            Assert.IsTrue(doc.HasChildNodes);
        }

        [Test]
        public void ScheduleElementNotNullTest()
        {
            var element = GetScheduleElement();
            var xml = element.ToXml();
            Assert.IsNotNull(xml);
        }

        [Test]
        public void ScheduleElementDocEventCreationTest()
        {
            var element = GetScheduleElement();
            var xml = element.ToXml();
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.ToString().Contains("ID"));
            Assert.IsTrue(xml.ToString().Contains("Street Cleaning"));
        }

        [Test]
        public void ScheduleElementDocEventDifferenceTest()
        {
            var element = GetScheduleElement();
            var xml = element.ToXml();
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.ToString().Contains("Difference"));
        }

        [Test]
        public void ScheduleElementDocEventExclusionTest()
        {
            var element = GetScheduleElement();
            var xml = element.ToXml();
            Assert.IsNotNull(xml);
            Assert.IsTrue(xml.ToString().Contains("Exclusions"));
        }


        private static ISchedule GetSchedule()
        {
            var schedule = new Schedule();
            var firstExpression = BuildStreetCleaningTemporalExpression();
            var firstScheduledEvent = new Event()
            {
                ID = 1,
                Title = "Street Cleaning",
                DateRange = new DateRange()
                {
                    StartDateTime = new DateTime(2011, 3, 1, 0, 0, 0),
                    EndDateTime = new DateTime(2011, 10, 30, 18, 0, 0)
                }
            };

            var secondExpression = BuildWitchingHourPrepTemporalExpression();
            var secondScheduledEvent = new Event()
            {
                ID = 2,
                Title = "Witching Hour Preparation",
                DateRange = new DateRange()
                {
                    StartDateTime = new DateTime(2012, 10, 1, 0, 0, 0),
                    EndDateTime = new DateTime(2012, 10, 31, 0, 0, 0)
                }
            };

            schedule.Add(firstScheduledEvent, firstExpression);
            schedule.Add(secondScheduledEvent, secondExpression);
            return schedule;
        }

        private static ISchedule GetWitchingHourPrepSchedule()
        {
            var schedule = new Schedule();
            var expression = BuildWitchingHourPrepTemporalExpression();
            var scheduledEvent = new Event()
            {
                ID = 2,
                Title = "Witching Hour Preparation",
                DateRange = new DateRange()
                {
                    StartDateTime = new DateTime(2012, 10, 1, 0, 0, 0),
                    EndDateTime = new DateTime(2012, 10, 31, 0, 0, 0)
                }
            };

            schedule.Add(scheduledEvent, expression);
            return schedule;
        }

        private static IScheduleElement GetScheduleElement()
        {
            return new ScheduleElement()
            {
                TemporalExpression = BuildStreetCleaningTemporalExpression(),
                Event = new Event()
                {
                    ID = 1,
                    Title = "Street Cleaning",
                    DateRange = new DateRange()
                    {
                        StartDateTime = new DateTime(2011, 3, 1, 0, 0, 0),
                        EndDateTime = new DateTime(2011, 10, 30, 18, 0, 0)
                    }
                }
            };
        }
 
        /// <summary>
        /// Street cleaning occurs from April to October on the first and third Monday of the
        /// month, excluding holidays. In this test there are two holidays: July 4 and Labor
        /// Day (first Monday in Sep). The street cleaning example is taken directly from 
        /// Martin Fowler's white paper "Recurring Events for Calendars".
        /// </summary>
        private static TemporalExpression BuildStreetCleaningTemporalExpression()
        {
            var maHolidays = new UnionTE();
            var independenceDay = new FixedHolidayTE(7, 4);
            var laborDay = new FloatingHolidayTE(9, 1, 1);

            maHolidays.Add(independenceDay);
            maHolidays.Add(laborDay);

            var firstMondayFirstWeek = new DayInMonthTE(1, 1);
            var firstMondayThirdWeek = new DayInMonthTE(1, 3);

            var firstAndThirdMonday = new UnionTE();
            firstAndThirdMonday.Add(firstMondayThirdWeek);
            firstAndThirdMonday.Add(firstMondayFirstWeek);

            var streetCleaningMonths = new IntersectionTE();
            streetCleaningMonths.Add(firstAndThirdMonday);
            streetCleaningMonths.Add(new RangeEachYearTE(3, 9));
            var diff = new DifferenceTE(streetCleaningMonths, maHolidays);
            return diff;
        }

        /// <summary>
        /// The witching hour prep is every Saturday night at midnight in October 
        /// except on Halloween when the main event occurs (separate event).
        /// </summary>
        /// <returns></returns>
        private static TemporalExpression BuildWitchingHourPrepTemporalExpression()
        {
            var halloween = new FixedHolidayTE(10, 31);
            var holidays = new UnionTE();
            holidays.Add(halloween);

            var everySaturdayNight = new DayOfMonthTE(6);
            var october = new RangeEachYearTE(10);

            var saturdays = new UnionTE();
            saturdays.Add(everySaturdayNight);

            var intersection = new IntersectionTE();
            intersection.Add(october);
            intersection.Add(saturdays);

            var diff = new DifferenceTE(intersection, holidays);
            return diff;
        }
    }
}
