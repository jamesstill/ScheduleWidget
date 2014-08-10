using System;
using System.Linq;
using NUnit.Framework;
using ScheduleWidget.ScheduledEvents;
using ScheduleWidget.Enums;
using ScheduleWidget.TemporalExpressions;
using System.Collections.Generic;

namespace ScheduleWidget.UnitTest
{
    [TestFixture]
    public class AAMultifunctionTests
    {
        [Test]
        public void AAMultifunctionTest1()
        {
            var aEvent = new Event()
            {
                FrequencyTypeOptions = FrequencyTypeEnum.Yearly,
                RepeatInterval = 2,
                StartDateTime = new DateTime(2000, 9, 27), // even years only
                Anniversary = new Anniversary()
                {
                    Month = 9,
                    Day = 27
                }
            };
            // Occurs 2000,2002,2004,2006,2008,2010
            aEvent.SetEndDateWithNumberOfOccurrences(6);

            // Check that the ending date was set correctly.
            Assert.IsTrue(aEvent.EndDateTime == new DateTime(2010, 9, 27));

            // Check that the number of occurrences is retrievable.
            Assert.IsTrue(aEvent.NumberOfOccurrencesThatWasLastSet == 6);

            // Exclude 2000,2006,2010.
            List<DateTime> excludedDates = new List<DateTime>();
            excludedDates.Add(new DateTime(2000, 9, 27));
            excludedDates.Add(new DateTime(2005, 9, 27));
            excludedDates.Add(new DateTime(2005, 9, 28));
            excludedDates.Add(new DateTime(2006, 9, 27));
            excludedDates.Add(new DateTime(2010, 9, 27));
            var schedule = new Schedule(aEvent, excludedDates);

            // Make sure it is not occurring on excluded dates.
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2000, 9, 27)));
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2006, 9, 27)));
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2010, 9, 27)));

            // Make sure it is not occurring outside the set range.
            Assert.IsFalse(schedule.IsOccurring(new DateTime(1998, 9, 27)));
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2012, 9, 27)));

            // Make sure it is occurring on desired dates.
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2002, 9, 27)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2004, 9, 27)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2008, 9, 27)));

            // Check the occurrences function.
            DateRange during = new DateRange(new DateTime(1995, 1, 1),new DateTime(2015, 1, 1));
            var occurrences = schedule.Occurrences(during);
            Assert.IsTrue(occurrences.Count() == 3);

            // Check the last occurrence date function.
            DateTime? lastDate = schedule.GetLastOccurrenceDate();
            Assert.IsTrue(lastDate == new DateTime(2008, 9, 27));

            // Check the next occurrence (date only) function.
            Assert.IsTrue(schedule.NextOccurrence(new DateTime(1995, 1, 1)) == new DateTime(2002, 9, 27));
            Assert.IsTrue(schedule.NextOccurrence(new DateTime(2004, 9, 26)) == new DateTime(2004, 9, 27));
            Assert.IsTrue(schedule.NextOccurrence(new DateTime(2004, 9, 27)) == new DateTime(2008, 9, 27));
            Assert.IsTrue(schedule.NextOccurrence(new DateTime(2008, 9, 27)) == null);

            // Check the previous occurrence (date only) function.
            Assert.IsTrue(schedule.PreviousOccurrence(new DateTime(1995, 1, 1)) == null);
            Assert.IsTrue(schedule.PreviousOccurrence(new DateTime(2004, 9, 26)) == new DateTime(2002, 9, 27));
            Assert.IsTrue(schedule.PreviousOccurrence(new DateTime(2004, 9, 28)) == new DateTime(2004, 9, 27));
            Assert.IsTrue(schedule.PreviousOccurrence(new DateTime(2013, 9, 27)) == new DateTime(2008, 9, 27));

            // Check the next occurrence ranged function.
            DateRange range1 = new DateRange(new DateTime(2004, 9, 1), new DateTime(2004, 10, 1));
            DateRange range2 = new DateRange(new DateTime(2004, 11, 1), new DateTime(2002, 9, 27));
            Assert.IsTrue(schedule.NextOccurrence(new DateTime(1995, 1, 1), range1) == new DateTime(2004, 9, 27));
            Assert.IsTrue(schedule.NextOccurrence(new DateTime(1995, 1, 1), range2) == null);

            // Check the previous occurrence ranged function.
            Assert.IsTrue(schedule.PreviousOccurrence(new DateTime(2015, 1, 1), range1) == new DateTime(2004, 9, 27));
            Assert.IsTrue(schedule.PreviousOccurrence(new DateTime(2015, 1, 1), range2) == null);
        }
    }
}
