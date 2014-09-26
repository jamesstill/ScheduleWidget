using System;
using System.ComponentModel.DataAnnotations;
using ScheduleWidget.Enums;
using ScheduleWidget.ScheduledEvents;

namespace ScheduleWidgetSampleProject.Models
{
    public class ScheduleDTO
    {
        private int _frequencyChoice;
        private DateTime? _endDate;

        public int ID { get; set; }
        public string Title { get; set; }

        public RecurrencePattern ScheduleRecurrence { get; set; }

        [Display(Name = "Schedule")]
        public int FrequencyChoice
        {
            get { return _frequencyChoice; }
            set
            {
                _frequencyChoice = value;
                CalculateFrequency();
                CalculateWeeklyInterval();
                CalculateRecurrencePattern();
            }
        }

        public int Frequency { get; set; }

        [Display(Name = "Days Of Week")]
        public int DaysOfWeek { get; set; }

        public int WeeklyInterval { get; set; }
        public int MonthlyInterval { get; set; }
        public int? NumberOfOccurrences { get; set; }

        [Display(Name = "Start Date")]
        [Required(ErrorMessage = @"Please provide a start date.")]
        public DateTime StartDate { get; set; }

        public DateTime StartDateTime
        {
            get
            {
                return StartDate + StartTime;
            }
            set
            {
                StartDate = value.Date;
                StartTime = value.TimeOfDay;
            }
        }

        public DateTime? EndDateTime
        {
            get
            {
                if (Frequency == 0) // one-time only 
                    return (StartDate + EndTime);

                return (_endDate.HasValue) ? _endDate : null;
            }
            set
            {
                _endDate = value;

                var ts = (EndDateTime - StartDate);
                if (!ts.HasValue)
                {
                    return;
                }

                if (ts.Value.Days == 0)
                    Frequency = 0;
            }
        }

        [Display(Name = "Start Time")]
        [Required(ErrorMessage = @"Please provide a start time.")]
        [RegularExpression(@"(20|21|22|23|[01]\d|\d)(([:][0-5]\d){1,2})", ErrorMessage = @"Please provide a valid time.")]
        public TimeSpan StartTime { get; set; }

        [Display(Name = "End Time")]
        [Required(ErrorMessage = @"Please provide an end time.")]
        [RegularExpression(@"(20|21|22|23|[01]\d|\d)(([:][0-5]\d){1,2})", ErrorMessage = @"Please provide a valid time.")]
        public TimeSpan EndTime { get; set; }

        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        public bool IsSundaySelected
        {
            get
            {
                return DaysOfWeekOptions.HasFlag(DayOfWeekEnum.Sun);
            }
            set
            {
                if (!value) return;

                if (!DaysOfWeekOptions.HasFlag(DayOfWeekEnum.Sun))
                {
                    DaysOfWeekOptions |= DayOfWeekEnum.Sun;
                }
            }
        }

        public bool IsMondaySelected
        {
            get
            {
                return DaysOfWeekOptions.HasFlag(DayOfWeekEnum.Mon);
            }
            set
            {
                if (!value) return;

                if (!DaysOfWeekOptions.HasFlag(DayOfWeekEnum.Mon))
                {
                    DaysOfWeekOptions |= DayOfWeekEnum.Mon;
                }
            }
        }

        public bool IsTuesdaySelected
        {
            get
            {
                return DaysOfWeekOptions.HasFlag(DayOfWeekEnum.Tue);
            }
            set
            {
                if (!value) return;

                if (!DaysOfWeekOptions.HasFlag(DayOfWeekEnum.Tue))
                {
                    DaysOfWeekOptions |= DayOfWeekEnum.Tue;
                }
            }
        }

        public bool IsWednesdaySelected
        {
            get
            {
                return DaysOfWeekOptions.HasFlag(DayOfWeekEnum.Wed);
            }
            set
            {
                if (!value) return;

                if (!DaysOfWeekOptions.HasFlag(DayOfWeekEnum.Wed))
                {
                    DaysOfWeekOptions |= DayOfWeekEnum.Wed;
                }
            }
        }

        public bool IsThursdaySelected
        {
            get
            {
                return DaysOfWeekOptions.HasFlag(DayOfWeekEnum.Thu);
            }
            set
            {
                if (!value) return;

                if (!DaysOfWeekOptions.HasFlag(DayOfWeekEnum.Thu))
                {
                    DaysOfWeekOptions |= DayOfWeekEnum.Thu;
                }
            }
        }

        public bool IsFridaySelected
        {
            get
            {
                return DaysOfWeekOptions.HasFlag(DayOfWeekEnum.Fri);
            }
            set
            {
                if (!value) return;

                if (!DaysOfWeekOptions.HasFlag(DayOfWeekEnum.Fri))
                {
                    DaysOfWeekOptions |= DayOfWeekEnum.Fri;
                }
            }
        }

        public bool IsSaturdaySelected
        {
            get
            {
                return DaysOfWeekOptions.HasFlag(DayOfWeekEnum.Sat);
            }
            set
            {
                if (!value) return;

                if (!DaysOfWeekOptions.HasFlag(DayOfWeekEnum.Sat))
                {
                    DaysOfWeekOptions |= DayOfWeekEnum.Sat;
                }
            }
        }

        public bool IsFirstWeekOfMonthSelected
        {
            get
            {
                return MonthlyIntervalOptions.HasFlag(MonthlyIntervalEnum.First);
            }
            set
            {
                if (!value) return;

                if (!MonthlyIntervalOptions.HasFlag(MonthlyIntervalEnum.First))
                {
                    MonthlyIntervalOptions |= MonthlyIntervalEnum.First;
                }
            }
        }

        public bool IsSecondWeekOfMonthSelected
        {
            get
            {
                return MonthlyIntervalOptions.HasFlag(MonthlyIntervalEnum.Second);
            }
            set
            {
                if (!value) return;

                if (!MonthlyIntervalOptions.HasFlag(MonthlyIntervalEnum.Second))
                {
                    MonthlyIntervalOptions |= MonthlyIntervalEnum.Second;
                }
            }
        }

        public bool IsThirdWeekOfMonthSelected
        {
            get
            {
                return MonthlyIntervalOptions.HasFlag(MonthlyIntervalEnum.Third);
            }
            set
            {
                if (!value) return;

                if (!MonthlyIntervalOptions.HasFlag(MonthlyIntervalEnum.Third))
                {
                    MonthlyIntervalOptions |= MonthlyIntervalEnum.Third;
                }
            }
        }

        public bool IsFourthWeekOfMonthSelected
        {
            get
            {
                return MonthlyIntervalOptions.HasFlag(MonthlyIntervalEnum.Fourth);
            }
            set
            {
                if (!value) return;

                if (!MonthlyIntervalOptions.HasFlag(MonthlyIntervalEnum.Fourth))
                {
                    MonthlyIntervalOptions |= MonthlyIntervalEnum.Fourth;
                }
            }
        }

        public bool IsLastWeekOfMonthSelected
        {
            get
            {
                return MonthlyIntervalOptions.HasFlag(MonthlyIntervalEnum.Last);
            }
            set
            {
                if (!value) return;

                if (!MonthlyIntervalOptions.HasFlag(MonthlyIntervalEnum.Last))
                {
                    MonthlyIntervalOptions |= MonthlyIntervalEnum.Last;
                }
            }
        }

        /// <summary>
        /// The frequency expressed as enumeration.
        /// </summary>
        public FrequencyTypeEnum FrequencyTypeOptions
        {
            get
            {
                return (FrequencyTypeEnum)Frequency;
            }
            set
            {
                Frequency = (int)value;
            }
        }

        /// <summary>
        /// The monthly interval expressed as enumeration
        /// </summary>
        public MonthlyIntervalEnum MonthlyIntervalOptions
        {
            get
            {
                return (MonthlyIntervalEnum)MonthlyInterval;
            }
            set
            {
                MonthlyInterval = (int)value;
            }
        }

        /// <summary>
        /// The days of the week expressed as enumeration.
        /// </summary>
        public DayOfWeekEnum DaysOfWeekOptions
        {
            get
            {
                return (DayOfWeekEnum)DaysOfWeek;
            }
            set
            {
                DaysOfWeek = (int)value;
            }
        }

        public Schedule Schedule
        {
            get
            {
                return BuildSchedule();
            }
        }

        /// <summary>
        /// Returns a schedule from the ScheduleWidget engine based on the 
        /// properties of this recurring schedule.
        /// </summary>
        /// <returns></returns>
        private Schedule BuildSchedule()
        {
            // create a new instance of each recurring event
            var recurringEvent = new Event()
            {
                ID = ID,
                Title = Title,
                Frequency = Frequency,
                RepeatInterval = WeeklyInterval,
                MonthlyInterval = MonthlyInterval,
                StartDateTime = StartDate,
                EndDateTime = EndDate,
                DaysOfWeek = DaysOfWeek
            };

            if (IsOneTimeEvent())
            {
                recurringEvent.OneTimeOnlyEventDate = StartDate;
            }

            return new Schedule(recurringEvent);
        }

        private bool IsOneTimeEvent()
        {
            if (Frequency == 0 && DaysOfWeek == 0 && MonthlyInterval == 0)
                return true;

            return false;
        }

        private void CalculateFrequency()
        {
            switch (_frequencyChoice)
            {
                case 1:
                    Frequency = 1; // daily
                    break;

                case 2:
                    Frequency = 2; // weekly
                    break;

                case 3:
                    Frequency = 2; // biweekly
                    break;

                case 4:
                    Frequency = 4; // monthly
                    break;

                default:
                    Frequency = 0; // one-time only
                    break;
            }
        }

        private void CalculateWeeklyInterval()
        {
            switch (_frequencyChoice)
            {
                case 2:
                    WeeklyInterval = 1; // weekly
                    break;

                case 3:
                    WeeklyInterval = 2; // biweekly
                    break;

                default:
                    WeeklyInterval = 0;
                    break;
            }
        }

        private void CalculateRecurrencePattern()
        {
            // determine frequency from recurrence pattern
            if (ScheduleRecurrence == RecurrencePattern.OneTime)
            {
                Frequency = 0;
                EndDate = null;
            }
            else // repeat pattern
            {
                if (FrequencyTypeOptions == FrequencyTypeEnum.Daily)
                {
                    DaysOfWeekOptions =
                        DayOfWeekEnum.Sun |
                        DayOfWeekEnum.Mon |
                        DayOfWeekEnum.Tue |
                        DayOfWeekEnum.Wed |
                        DayOfWeekEnum.Thu |
                        DayOfWeekEnum.Fri |
                        DayOfWeekEnum.Sat;
                }
            }
        }
    }
}