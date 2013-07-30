using System.Collections.Generic;
using System.Linq;
using ScheduleWidgetSampleProject.Models;

namespace ScheduleWidgetSampleProject.Repository
{
    public class ScheduleRepository
    {
        public ScheduleDTO GetSchedule(int id)
        {
            return MockDatabase.Schedules.FirstOrDefault(s => s.ID == id);
        }

        public IEnumerable<ScheduleDTO> GetAllSchedules()
        {
            return MockDatabase.Schedules;
        }

        public bool Save(ScheduleDTO scheduleDTO)
        {
            if (scheduleDTO.ID == 0)
            {
                scheduleDTO.ID = MockDatabase.UniqueID;
                MockDatabase.UniqueID++;
                MockDatabase.Schedules.Add(scheduleDTO);
            }
            else
            {
                var schedule = MockDatabase.Schedules.FirstOrDefault(s => s.ID == scheduleDTO.ID);

                if (schedule == null)
                    return false;

                schedule.Title = scheduleDTO.Title;
                schedule.Frequency = scheduleDTO.Frequency;
                schedule.DaysOfWeek = scheduleDTO.DaysOfWeek;
                schedule.WeeklyInterval = scheduleDTO.WeeklyInterval;
                schedule.MonthlyInterval = scheduleDTO.MonthlyInterval;
                schedule.NumberOfOccurrences = scheduleDTO.NumberOfOccurrences;
                schedule.StartDate = scheduleDTO.StartDate;
                schedule.EndDate = scheduleDTO.EndDate;
                schedule.StartTime = scheduleDTO.StartTime;
                schedule.EndTime = scheduleDTO.EndTime;
            }

            return true;
        }
    }

    public static class MockDatabase
    {
        public static int UniqueID = 1;
        public static List<ScheduleDTO> Schedules = new List<ScheduleDTO>();
    }
}