ScheduleWidget handles recurring events for calendars. Let's say we want to create a 
recurring event for the Critical Mass Bicycle Ride, which happens in many major cities
on the last Friday of every month. 

Code Sample:

// using ScheduleWidget.Enums; 
// using ScheduleWidget.ScheduledEvents; 
var aEvent = new Event() 
{ 
    ID = 1, 
	Title = "Critical Mass Bicycle Ride", 
	FrequencyTypeOptions = FrequencyTypeEnum.Monthly, 
	MonthlyIntervalOptions = MonthlyIntervalEnum.Last, 
	DaysOfWeekOptions = DayOfWeekEnum.Fri
};

var schedule = new Schedule(aEvent);

// give me all the upcoming dates for the next year
var range = new DateRange() 
{ 
    StartDateTime = DateTime.Now, 
	EndDateTime = DateTime.Now.AddYears(1) 
}; 

var occurrences = schedule.Occurrences(range);

foreach(var date in occurrences)
{
	System.Diagnostics.Debug.Print(date);
}

// see http://www.squarewidget.com/schedulewidget for more