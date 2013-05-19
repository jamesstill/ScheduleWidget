using System;
using System.Linq;
using NUnit.Framework;
using ScheduleWidget.Enums;
using ScheduleWidget.ScheduledEvents;
using ScheduleWidget.TemporalExpressions;

namespace ScheduleWidget.UnitTest
{
    [TestFixture]
    public class QuarterlyTests
    {
        [Test]
        public void MonthQuarterMatrixConstructed()
        {
            // arbitrary TE
            DayInQuarterTE te = new DayInQuarterTE(QuarterEnum.First, QuarterlyIntervalEnum.First, MonthlyIntervalEnum.First, DayOfWeekEnum.Fri);
            var mqMatrix = te.BuildMonthQuarterMatrix();

            // check all of the values of the matrix: length1 == quarters, length2 == quarter intervals (months 1, 2, 3)
            Assert.IsTrue((int)mqMatrix.GetValue(0, 0) == 0, "expect Month 0 (Jan)");
            Assert.IsTrue((int)mqMatrix.GetValue(0, 1) == 1, "expect Month 1 (Feb)");
            Assert.IsTrue((int)mqMatrix.GetValue(0, 2) == 2, "expect Month 2 (Mar)");

            Assert.IsTrue((int)mqMatrix.GetValue(1, 0) == 3, "expect Month 3 (Apr)");
            Assert.IsTrue((int)mqMatrix.GetValue(1, 1) == 4, "expect Month 4 (May)");
            Assert.IsTrue((int)mqMatrix.GetValue(1, 2) == 5, "expect Month 5 (Jun)");

            Assert.IsTrue((int)mqMatrix.GetValue(2, 0) == 6, "expect Month 6 (Jul)");
            Assert.IsTrue((int)mqMatrix.GetValue(2, 1) == 7, "expect Month 7 (Aug)");
            Assert.IsTrue((int)mqMatrix.GetValue(2, 2) == 8, "expect Month 8 (Sep)");

            Assert.IsTrue((int)mqMatrix.GetValue(3, 0) == 9, "expect Month 9 (Oct)");
            Assert.IsTrue((int)mqMatrix.GetValue(3, 1) == 10, "expect Month 10 (Nov)");
            Assert.IsTrue((int)mqMatrix.GetValue(3, 2) == 11, "expect Month 11 (Dec)");
        }

        [Test]
        public void CanFindSetTEInMatrix()
        {
            // te for Q1, Month 1 (Jan == 0)
            DayInQuarterTE te1 = new DayInQuarterTE(QuarterEnum.First, QuarterlyIntervalEnum.First, MonthlyIntervalEnum.First, DayOfWeekEnum.Fri);
            var mqMatrix = te1.BuildMonthQuarterMatrix();

            int expectedMonth = (int)mqMatrix.GetValue(0, 0);
            Assert.IsTrue(expectedMonth == 0, "expect Jan (0)");

            // now loop 'em all:
            DayInQuarterTE te = null;
            QuarterEnum quarter;
            QuarterlyIntervalEnum quarterInterval;
            int monthCounter = 0;

            for (int i = 0; i <= 3; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    quarter = (QuarterEnum)i;
                    quarterInterval = (QuarterlyIntervalEnum)j;
                    te = new DayInQuarterTE(quarter, quarterInterval, MonthlyIntervalEnum.First, DayOfWeekEnum.Fri);

                    expectedMonth = (int)mqMatrix.GetValue(i, j);
                    Assert.IsTrue(expectedMonth == monthCounter, string.Format("expected {0}, returned {1}", monthCounter, expectedMonth));

                    monthCounter++;
                }
            }
        }

        [Test]
        public void CanMatchMonthWithMatrix()
        {
            // build a matrix:
            DayInQuarterTE te1 = new DayInQuarterTE(QuarterEnum.First, QuarterlyIntervalEnum.First, MonthlyIntervalEnum.First, DayOfWeekEnum.Fri);
            var mqMatrix = te1.BuildMonthQuarterMatrix();

            // test that we find quarters & intervals based on arbitrary dates
            DateTime dt;
            int expectedMonth;
            int ourMonth;
            int monthCounter = 1;
            for (int i = 0; i <= 3; i++) // quarters
            {
                for (int j = 0; j <= 2; j++) // quarter intervals (months)
                {
                    expectedMonth = (i * 3) + j; // zero-based month for the matrix coordinates we are at
                    dt = new DateTime(DateTime.Now.Year, monthCounter, 15);
                    ourMonth = (int)mqMatrix.GetValue(i, j) + 1; // zero-based

                    Assert.IsTrue(ourMonth == dt.Month, string.Format("date match wrong: {0}", dt.ToString("f")));

                    monthCounter++;
                }
            }
        }

        [Test]
        public void QuarterlyEventTest1()
        {
            var aEvent = new Event()
            {
                ID = 1,
                Title = "Quarterly, First Quarter, First Month, Last Week, Friday",
                Frequency = 8, // quarterly
                QuarterInterval = 1, // first quarter
                QuarterlyInterval = 1, // first month of quarter
                MonthlyInterval = 16, // last week of the month
                DaysOfWeek = 32 // Friday
            };

            var schedule = new Schedule(aEvent);

            // check 2014: expect Jan 31st 2014 (Friday)
            
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2014, 1, 31)), "expect match for Jan. 31st");
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2014, 1, 24)), "wrong match - 1W too early"); // Fri before
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2014, 2, 7)), "wrong match - 1W too late"); // Fri after

            Assert.IsTrue(schedule.IsOccurring(new DateTime(2015, 1, 30)), "expect match for Jan. 30th");
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2016, 1, 29)), "expect match for Jan. 29th");
        }

        [Test]
        public void QuarterlyEventTest2()
        {
            var aEvent = new Event()
            {
                ID = 1,
                Title = "Quarterly, Third Quarter, Second Month, Second Week, Wednesday",
                Frequency = 8, // quarterly
                QuarterInterval = 4, // third quarter
                QuarterlyInterval = 2, // 2nd month of quarter
                MonthlyInterval = 2, // 2nd week of the month
                DaysOfWeek = 8 // Wednesday
            };

            var schedule = new Schedule(aEvent);

            // check 2014: expect Aug. 13th 2014 (Wed)
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2014, 8, 13)), "expect match for Aug. 6th");
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2014, 8, 20)), "wrong match - 1W too late");
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2014, 7, 6)), "wrong match - 1W too early");

            Assert.IsTrue(schedule.IsOccurring(new DateTime(2015, 8, 12)), "expect match for Aug. 5th");
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2016, 8, 10)), "expect match for Aug. 10th");
        }

        [Test]
        public void QuarterlyOccurancesSingleEventCorrect()
        {
            var aEvent = new Event()
            {
                ID = 1,
                Title = "Quarterly, Third Quarter, Second Month, Second Week, Wednesday",
                Frequency = 8, // quarterly
                QuarterInterval = 4, // third quarter
                QuarterlyInterval = 2, // 2nd month of quarter
                MonthlyInterval = 2, // 2nd week of the month
                DaysOfWeek = 8 // Wednesday
            };

            var schedule = new Schedule(aEvent);

            // test # of occurrances
            DateTime start = new DateTime(2014, 1, 1);

            var occurances = schedule.Occurrences(
                new DateRange { StartDateTime = start, EndDateTime = new DateTime(2014, 12, 31) });

            Assert.IsTrue(occurances.Count() == 1, "expect only 1 occurance");

            var occurances2 = schedule.Occurrences(
                new DateRange { StartDateTime = start, EndDateTime = new DateTime(2015, 12, 31) });

            Assert.IsTrue(occurances2.Count() == 2, "expect 2 occurances");
        }

        [Test]
        public void MultiQuarters1()
        {
            // schedule an event for 2 different quarters
            var aEvent = new Event()
            {
                ID = 1,
                Title = "Quarterly, 2nd & 4th Quarter, Second Month, Second Week, Wednesday",
                Frequency = 8, // quarterly
                QuarterInterval = 10, // 2nd & 4th
                QuarterlyInterval = 2, // 2nd month of quarter
                MonthlyInterval = 2, // 2nd week of the month
                DaysOfWeek = 8 // Wednesday
            };

            var schedule = new Schedule(aEvent);

            // check 2014: expect Aug. 13th 2014 (Wed)
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2014, 5, 14)), "expect match for May. 14th");
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2014, 11, 12)), "expect match for Nov. 12th");

            var occurrences = schedule.Occurrences(new DateRange
            {
                StartDateTime = new DateTime(2014, 1, 1),
                EndDateTime = new DateTime(2014, 12, 31)
            });

            Assert.IsTrue(occurrences.Count() == 2, "expect 2 events");
        }

        [Test]
        public void MultiQuartersMultiMonths()
        {
            // schedule an event for 2 different quarters, 2 different months
            var aEvent = new Event()
            {
                ID = 1,
                Title = "Quarterly, 2nd & 4th Quarter, 2nd & 3rd Month, Second Week, Wednesday",
                Frequency = 8, // quarterly
                QuarterInterval = 10, // 2nd & 4th
                QuarterlyInterval = 6, // 2nd & 3rd month
                MonthlyInterval = 2, // 2nd week of the month
                DaysOfWeek = 8 // Wednesday
            };

            var schedule = new Schedule(aEvent);

            // check 2014:
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2014, 5, 14)), "expect match for May. 14th");
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2014, 6, 11)), "expect match for June. 11th");

            Assert.IsTrue(schedule.IsOccurring(new DateTime(2014, 11, 12)), "expect match for Nov. 12th");
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2014, 12, 10)), "expect match for Dec. 10th");

            var occurances = schedule.Occurrences(new DateRange
            {
                StartDateTime = new DateTime(2014, 1, 1),
                EndDateTime = new DateTime(2014, 12, 31)
            });

            Assert.IsTrue(occurances.Count() == 4, "expect 4 events");
        }

        [Test]
        public void MultiQuartersMultiMonthsMultiWeeks()
        {
            // schedule an event for 2 different quarters, 2 different months, 2 different weeks
            var aEvent = new Event()
            {
                ID = 1,
                Title = "Quarterly, 2nd & 4th Quarter, 2nd & 3rd Month, 2nd and 3rd Week, Wednesday",
                Frequency = 8, // quarterly
                QuarterInterval = 10, // 2nd & 4th
                QuarterlyInterval = 6, // 2nd & 3rd month
                MonthlyInterval = 6, // 2nd & 3rd week
                DaysOfWeek = 8 // Wednesday
            };

            var schedule = new Schedule(aEvent);

            var occurances = schedule.Occurrences(new DateRange
            {
                StartDateTime = new DateTime(2014, 1, 1),
                EndDateTime = new DateTime(2014, 12, 31)
            });

            Assert.IsTrue(occurances.Count() == 8, "expect 8 events");
        }

        [Test]
        public void MultiQuartersMultiMonthsMultiWeeksMultiDays()
        {
            // schedule an event for 2 different quarters, 2 different months, 2 different weeks, 2 different days!
            var aEvent = new Event()
            {
                ID = 1,
                Title = "Quarterly, 2nd & 4th Quarter, 2nd & 3rd Month, 2nd and 3rd Week, Monday & Tuesday",
                Frequency = 8, // quarterly
                QuarterInterval = 10, // 2nd & 4th
                QuarterlyInterval = 6, // 2nd & 3rd month
                MonthlyInterval = 6, // 2nd & 3rd week
                DaysOfWeek = 6 // Mon & Tues
            };

            var schedule = new Schedule(aEvent);

            var occurances = schedule.Occurrences(new DateRange
            {
                StartDateTime = new DateTime(2014, 1, 1),
                EndDateTime = new DateTime(2014, 12, 31)
            });

            Assert.IsTrue(occurances.Count() == 16, "expect 16 events");
        }

    }
}
