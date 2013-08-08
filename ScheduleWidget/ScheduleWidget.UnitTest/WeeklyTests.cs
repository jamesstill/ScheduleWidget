﻿using System;
using NUnit.Framework;
using ScheduleWidget.ScheduledEvents;

namespace ScheduleWidget.UnitTest
{
    [TestFixture]
    public class WeeklyTests
    {
        [Test]
        public void WeeklyEventTest1()
        {
            var aEvent = new Event()
            {
                ID = 1,
                Title = "Every Mon and Wed",
                Frequency = 2,        // weekly
                MonthlyInterval = 0,  // not applicable
                DaysOfWeek = 10      // every Mon and Wed
            };

            var schedule = new Schedule(aEvent);

            // NOTE: If daily then date range doesn't matter
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2016, 2, 8)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2015, 11, 25)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2014, 6, 16)));

            Assert.IsFalse(schedule.IsOccurring(new DateTime(2012, 8, 9))); // Thu
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2013, 2, 5))); // Tue
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2014, 7, 5))); // Sat
        }

        [Test]
        public void WeeklyEventTest2()
        {
            var aEvent = new Event()
            {
                ID = 1,
                Title = "Every Mon Wed Fri",
                Frequency = 2,        // weekly
                MonthlyInterval = 0,  // not applicable
                DaysOfWeek = 42      // every Mon, Wed and Fri
            };

            var schedule = new Schedule(aEvent);

            Assert.IsTrue(schedule.IsOccurring(new DateTime(2016, 2, 8)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2015, 11, 25)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2014, 6, 16)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2012, 10, 12)));

            Assert.IsFalse(schedule.IsOccurring(new DateTime(2012, 8, 9))); // Thu
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2013, 2, 5))); // Tue
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2014, 7, 5))); // Sat
        }

        [Test]
        public void WeeklyEventTest3()
        {
            var aEvent = new Event()
            {
                ID = 1,
                Title = "Every 2nd week on Mon Wed Fri",
                Frequency = 2,        // weekly
                MonthlyInterval = 0,  // not applicable
                DaysOfWeek = 44,      // every Tue, Wed and Fri
                WeeklyInterval = 2, // every 2nd week
                FirstDateTime = new DateTime(2013, 1, 1) // start date on a Tue
            };

            var schedule = new Schedule(aEvent);

            Assert.IsTrue(schedule.IsOccurring(new DateTime(2013, 1, 1)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2013, 1, 15)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2013, 2, 12)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2013, 12, 17)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2014, 1, 14)));

            Assert.IsFalse(schedule.IsOccurring(new DateTime(2013, 1, 8))); // 1st week
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2013, 1, 14))); // 2nd week but Mon
        }

        [Test]
        public void WeeklyEventTest4()
        {
            var aEvent = new Event()
            {
                ID = 1,
                Title = "Every 3rd week on Mon Wed Fri",
                Frequency = 2,        // weekly
                MonthlyInterval = 0,  // not applicable
                DaysOfWeek = 44,      // every Tue, Wed and Fri
                WeeklyInterval = 3, // every 2nd week
                FirstDateTime = new DateTime(2013, 1, 1) // start date on a Tue
            };

            var schedule = new Schedule(aEvent);

            Assert.IsTrue(schedule.IsOccurring(new DateTime(2013, 1, 1)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2013, 1, 22)));

            Assert.IsFalse(schedule.IsOccurring(new DateTime(2013, 1, 8))); // 1st week
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2013, 1, 15))); // 2nd week
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2013, 1, 21))); // 3rd week but Mon
        }

        [Test]
        public void WeeklyEventWeeklyOnWeekdaysTest()
        {
            var aEvent = new Event()
            {
                ID = 1,
                Title = "Every weekday (Monday to Friday)",
                Frequency = 32,        // every weekday (Monday to Friday)
                DaysOfWeek = 62,      // every Mon, Tue, Wed, Thu and Fri
                FirstDateTime = new DateTime(2013, 8, 1) 
            };

            var schedule = new Schedule(aEvent);

            Assert.IsTrue(schedule.IsOccurring(new DateTime(2013, 8, 1))); 
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2013, 8, 2)));

            Assert.IsFalse(schedule.IsOccurring(new DateTime(2013, 8, 3)));
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2013, 8, 4)));

            Assert.IsTrue(schedule.IsOccurring(new DateTime(2013, 8, 5)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2013, 8, 6)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2013, 8, 7)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2013, 8, 8)));
            Assert.IsTrue(schedule.IsOccurring(new DateTime(2013, 8, 9)));

            Assert.IsFalse(schedule.IsOccurring(new DateTime(2013, 8, 10)));

        }

        [Test]
        public void WeeklyEventOnEveryMonWedFriTest()
        {
            var aEvent = new Event()
            {
                ID = 1,
                Title = "Weekly on Monday, Wednesday, Friday",
                Frequency = 64,        // weekly on Monday, Wednesday, Friday
                DaysOfWeek = 42,      // every Mon, Wed and Fri
                FirstDateTime = new DateTime(2013, 8, 2)
            };

            var schedule = new Schedule(aEvent);

            Assert.IsTrue(schedule.IsOccurring(new DateTime(2013, 8, 2)));

            Assert.IsFalse(schedule.IsOccurring(new DateTime(2013, 8, 3)));
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2013, 8, 4)));

            Assert.IsTrue(schedule.IsOccurring(new DateTime(2013, 8, 5)));

            Assert.IsFalse(schedule.IsOccurring(new DateTime(2013, 8, 6)));

            Assert.IsTrue(schedule.IsOccurring(new DateTime(2013, 8, 7)));

            Assert.IsFalse(schedule.IsOccurring(new DateTime(2013, 8, 8)));

            Assert.IsTrue(schedule.IsOccurring(new DateTime(2013, 8, 9)));

            Assert.IsFalse(schedule.IsOccurring(new DateTime(2013, 8, 10)));
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2013, 8, 11)));

        }

        [Test]
        public void WeeklyEventOnEveryTueThuTest()
        {
            var aEvent = new Event()
            {
                ID = 1,
                Title = "Weekly on Tuesday and Thursday",
                Frequency = 128,        // weekly on Tuesday and Thursday
                DaysOfWeek = 20,      // every Tuesday and Thursday
                FirstDateTime = new DateTime(2013, 8, 1)
            };

            var schedule = new Schedule(aEvent);

            Assert.IsTrue(schedule.IsOccurring(new DateTime(2013, 8, 1)));

            Assert.IsFalse(schedule.IsOccurring(new DateTime(2013, 8, 2)));
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2013, 8, 3)));
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2013, 8, 4)));
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2013, 8, 5)));

            Assert.IsTrue(schedule.IsOccurring(new DateTime(2013, 8, 6)));

            Assert.IsFalse(schedule.IsOccurring(new DateTime(2013, 8, 7)));

            Assert.IsTrue(schedule.IsOccurring(new DateTime(2013, 8, 8)));

            Assert.IsFalse(schedule.IsOccurring(new DateTime(2013, 8, 9)));
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2013, 8, 10)));
            Assert.IsFalse(schedule.IsOccurring(new DateTime(2013, 8, 11)));

        }
    }
}
