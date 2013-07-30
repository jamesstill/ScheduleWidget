using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ScheduleWidget.ScheduledEvents;
using ScheduleWidgetSampleProject.Models;
using ScheduleWidgetSampleProject.Repository;

namespace ScheduleWidgetSampleProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateSchedule(DateTime eventDate)
        {
            var scheduleDTO = new ScheduleDTO()
            {
                StartDate = eventDate,
                StartTime = new TimeSpan(DateTime.Now.Hour, 0, 0),
                EndTime = new TimeSpan(DateTime.Now.Hour + 1, 0, 0),
                NumberOfOccurrences = 0
            };

            LoadViewBag();
            return View(scheduleDTO);
        }

        [HttpPost]
        public ActionResult CreateSchedule(ScheduleDTO scheduleDTO)
        {
            // TODO: validate the DTO client-side and with FluentValidation!
            if (string.IsNullOrEmpty(scheduleDTO.Title))
            {
                return View(scheduleDTO);
            }

            var repository = new ScheduleRepository();
            repository.Save(scheduleDTO);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ScheduleOccurrence(int id, DateTime occurrenceDate)
        {
            var scheduleOccurrenceDTO = new ScheduleOccurrenceDTO()
            {
                ScheduleID = id,
                OccurrenceDate = occurrenceDate
            };

            return View(scheduleOccurrenceDTO);
        }

        public JsonResult GetSchedules(double start, double end)
        {
            var calendarObjects = new List<object>();  

            var range = new DateRange()
            {
                StartDateTime = FromUnixTimestamp(start),
                EndDateTime = FromUnixTimestamp(end)
            };
            
            var repository = new ScheduleRepository();
            var schedules = repository.GetAllSchedules().ToList();
            foreach (var schedule in schedules)
            {
                calendarObjects
                    .AddRange(schedule.NumberOfOccurrences > 0 
                    ? GetSpecificNumberOfOccurrencesForDateRange(schedule, range)
                    : GetAllOccurrencesForDateRange(schedule, range));
            }

            return Json(calendarObjects.ToArray(), JsonRequestBehavior.AllowGet);
        }


        private IEnumerable<object> GetSpecificNumberOfOccurrencesForDateRange(ScheduleDTO scheduleDTO, DateRange range)
        {
            var calendarObjects = new List<object>();
            var occurrences = scheduleDTO.Schedule.Occurrences(range).ToArray();
            for (var i = 0; i < scheduleDTO.NumberOfOccurrences; i++)
            {
                var date = occurrences.ElementAtOrDefault(i);
                calendarObjects.Add(new
                {
                    id = scheduleDTO.ID,
                    title = scheduleDTO.Title,
                    start = ToUnixTimestamp(date + scheduleDTO.StartTime),
                    end = ToUnixTimestamp(date + scheduleDTO.EndTime),
                    url = Url.Action("ScheduleOccurrence", "Home", new { id = scheduleDTO.ID, occurrenceDate = date }),
                    allDay = false
                });
            }
            return calendarObjects;
        }

        private IEnumerable<object> GetAllOccurrencesForDateRange(ScheduleDTO scheduleDTO, DateRange range)
        {
            var calendarObjects = new List<object>();  
            foreach (var date in scheduleDTO.Schedule.Occurrences(range))
            {
                calendarObjects.Add(new
                {
                    id = scheduleDTO.ID,
                    title = scheduleDTO.Title,
                    start = ToUnixTimestamp(date + scheduleDTO.StartTime),
                    end = ToUnixTimestamp(date + scheduleDTO.EndTime),
                    url = Url.Action("ScheduleOccurrence", "Home", new { id = scheduleDTO.ID, occurrenceDate = date }),
                    allDay = false
                });
            }

            return calendarObjects;
        }

        private void LoadViewBag()
        {
            LoadFrequencyChoices();
            LoadDaysOfWeekChoices();
        }

        private void LoadFrequencyChoices()
        {
            var list = new List<object>()
            {
                new { ID = 1, Name = "Daily" },
                new { ID = 2, Name = "Weekly" },
                new { ID = 3, Name = "Biweekly" },
                new { ID = 4, Name = "Monthly" }
            };

            ViewBag.FrequencyChoices = new SelectList(list, "ID", "Name");
        }

        private void LoadDaysOfWeekChoices()
        {
            var daysOfWeek = new List<object>()
            {
                new { Name = "Sat" },
                new { Name = "Sun" },
                new { Name = "Mon" },
                new { Name = "Tue" },
                new { Name = "Wed" },
                new { Name = "Thu" },
                new { Name = "Fri" }
            };

            ViewBag.DaysOfWeekChoices = new SelectList(daysOfWeek, "Name", "Name");
        }

        private static DateTime FromUnixTimestamp(double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0); 
            return origin.AddSeconds(timestamp);
        } 
        
        private static long ToUnixTimestamp(DateTime date)
        {
            var ts = date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0)); 
            return (long)Math.Truncate(ts.TotalSeconds);
        } 
    }
}
