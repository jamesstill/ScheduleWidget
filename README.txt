ScheduleWidget handles recurring events for calendars.

ScheduleWidget is a .NET scheduling engine that handles recurring events for calendars. I was influenced by Martin Fowler's white paper "Recurring Events for Calendars" in which he describes the broad software architecture for a recurring events scheduling engine. But I did not find any implementation of his idea in the wild. So this led me to write it myself. It's pretty easy to use ScheduleWidget. Suppose you have an event that occurs every Monday and Wednesday. Here's the code:

var aEvent = new Event()
{
    ID = 1,
    Title = "Every Mon and Wed",
    FrequencyTypeOptions = FrequencyTypeEnum.Weekly,    
    DaysOfWeekOptions = DayOfWeekEnum.Mon | DayOfWeekEnum.Wed
};

var schedule = new Schedule(aEvent);

Now the schedule object can calculate the calendar dates for the event. Suppose I want to know if the event occurs today:

bool occursToday = schedule.IsOccurring(DateTime.Today);

Or maybe I want to get the dates for the next three months:

var range = new DateRange()
{    
    StartDateTime = DateTime.Today,
    EndDateTime = DateTime.Today.AddMonths(3)
};

IEnumerable<DateTime> dates = schedule.Occurrences(range);

You can model one-time, daily, weekly, bi-weekly, quarterly, monthly, and yearly (anniversary) intervals. You can make exceptions for holidays. It works great with FullCalendar or any other javascript-based calendar control. 
