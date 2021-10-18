using Scheduler.Exceptions;
using System;
using System.Linq;
using System.Text;

namespace Scheduler
{
    public class DateCalculator
    {
        public DateResult GetNextExecutionDate(SchedulerConfiguration Configuration)
        {
            this.ParameterValidation(Configuration);
            if (Configuration.Type == RecurringType.Once)
            {
                if (Configuration.ConfigurationDate.HasValue == false)
                {
                    throw new SchedulerException("If 'Once' type is selected you must indicate a Configuration DateTime in order to start the process");
                }
                return new DateResult() {
                    NextDate = this.GetDailyConfiguration(Configuration.ConfigurationDate.Value, Configuration.DailyConfiguration),
                    Description = this.GetDescription(true, this.GetDailyConfiguration(Configuration.ConfigurationDate.Value, Configuration.DailyConfiguration), Configuration)
                };                
            }
            DateTime ResultDate = new DateTime();
            if (Configuration.Frecuency.HasValue == false)
            {
                throw new SchedulerException("If 'Recurring' type is selected you must indicate a frecuency");
            }
            switch (Configuration.SchedulerFrecuency)
            {
                case SchedulerFrecuency.Daily:
                    ResultDate = this.GetDailyConfiguration(Configuration.CurrentDate.AddDays(Configuration.Frecuency.Value), Configuration.DailyConfiguration);
                    break;
                case SchedulerFrecuency.Weekly:
                    ResultDate = this.GetWeekDate(Configuration.CurrentDate, Configuration.WeekConfiguration, Configuration.DailyConfiguration);
                    break;
                case SchedulerFrecuency.Monthly:
                    ResultDate = this.GetDailyConfiguration(Configuration.CurrentDate.AddMonths(Configuration.Frecuency.Value), Configuration.DailyConfiguration);
                    break;
                case SchedulerFrecuency.Yearly:
                    ResultDate = this.GetDailyConfiguration(Configuration.CurrentDate.AddYears(Configuration.Frecuency.Value), Configuration.DailyConfiguration);
                    break;
            }

            return new DateResult() { 
                NextDate = ResultDate,
                Description = this.GetDescription(false, ResultDate,Configuration)
            };
        }

        private string GetDescription(bool Once, DateTime ResultDate, SchedulerConfiguration Configuration)
        {
            if (Once)
            {
                return $"Occurs once. Schedule will be used on {ResultDate} starting on {Configuration.StartDate}"
                    + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty);
            }
            
            if (Configuration.SchedulerFrecuency == SchedulerFrecuency.Weekly)
            {
                return this.GetWeeklyDescription(ResultDate, Configuration);
            }
            return $"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {ResultDate} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty);
        }

        private string GetWeeklyDescription(DateTime ResultDate, SchedulerConfiguration Configuration)
        {
            StringBuilder WeekDays = new StringBuilder();
            StringBuilder Description = new StringBuilder();
            Array.ForEach(Configuration.WeekConfiguration.Days, D => {
                WeekDays.Append(D + ", ");
            });
            WeekDays = WeekDays.Remove(WeekDays.ToString().LastIndexOf(","), 1);
            int LastComma = WeekDays.ToString().LastIndexOf(',');
            string days = WeekDays.ToString().Trim();
            if (LastComma != -1)
            {
                days = WeekDays.ToString().Remove(LastComma, 1).Insert(LastComma, " and").Trim();
            }
            Description.Append($"Occurs every {Configuration.WeekConfiguration.WeekFrecuency} weeks on {days} ");
            if (Configuration.DailyConfiguration.OccursOnceDaily)
            {
                Description.Append($"at {Configuration.DailyConfiguration.DailyHour}");
            }
            else
            {
                Description.Append($"every {Configuration.DailyConfiguration.DailyFrecuency} {Configuration.DailyConfiguration.TimeFrecuency} ");
                Description.Append($"between {Configuration.DailyConfiguration.DailyStartHour} and {Configuration.DailyConfiguration.DailyEndHour} starting on {Configuration.StartDate}").ToString();
            }
            return Description.ToString();
        }

        private DateTime GetWeekDate(DateTime CurrentDate, WeekConfiguration WeekConfiguration, DailyConfiguration DailyConfiguration)
        {
            DateTime Date = CurrentDate.AddDays(WeekConfiguration.WeekFrecuency.Value * 7);
            do
            {
                if (WeekConfiguration.Days.Contains(Date.DayOfWeek) == false)
                {
                    Date = Date.AddDays(1);
                }                
            } while (WeekConfiguration.Days.Contains(Date.DayOfWeek) == false);
            Date = this.GetDailyConfiguration(Date, DailyConfiguration);
            return Date;
        }

        private DateTime GetDailyConfiguration(DateTime Date, DailyConfiguration DailyConfiguration)
        {
            if (DailyConfiguration.OccursOnceDaily)
            {
                return Date.AddTicks(DailyConfiguration.DailyHour.Value.Ticks);
            }
            switch (DailyConfiguration.TimeFrecuency)
            {
                case TimeFrecuency.Hours:
                    Date = Date.AddTicks(DailyConfiguration.DailyStartHour.Value.Ticks).AddHours(DailyConfiguration.DailyFrecuency.Value);
                    break;
                case TimeFrecuency.Minutes:
                    Date = Date.AddTicks(DailyConfiguration.DailyStartHour.Value.Ticks).AddMinutes(DailyConfiguration.DailyFrecuency.Value);
                    break;
                case TimeFrecuency.Seconds:
                    Date = Date.AddTicks(DailyConfiguration.DailyStartHour.Value.Ticks).AddSeconds(DailyConfiguration.DailyFrecuency.Value);
                    break;                
            }
            return Date;
        }

        private void ParameterValidation(SchedulerConfiguration Configuration)
        {
            if (Configuration.CurrentDate < DateTime.MinValue || Configuration.CurrentDate > DateTime.MaxValue)
            {
                throw new SchedulerException("The current date exceeds the supported date min and max values");
            }
            if (Configuration.StartDate < DateTime.MinValue || Configuration.StartDate > DateTime.MaxValue)
            {
                throw new SchedulerException("The start date exceeds the supported date min and max values");
            }
            if (Configuration.EndDate < DateTime.MinValue || Configuration.EndDate > DateTime.MaxValue)
            {
                throw new SchedulerException("The end date exceeds the supported date min and max values");
            }
            if (Configuration.ConfigurationDate.HasValue && (Configuration.ConfigurationDate < DateTime.MinValue || Configuration.ConfigurationDate > DateTime.MaxValue))
            {
                throw new SchedulerException("The configuration date exceeds the supported date min and max values");
            }
            if (Configuration.StartDate > Configuration.EndDate)
            {
                throw new SchedulerException("Start date cannot be greater than end date");
            }
            if (Configuration.Frecuency.HasValue && (Configuration.Frecuency <= 0 || Configuration.Frecuency < Int32.MinValue || Configuration.Frecuency > Int32.MaxValue))
            {
                throw new SchedulerException("Frecuency can neither be negative nor exceed integer max or min values");
            }
            if (Configuration.SchedulerFrecuency == SchedulerFrecuency.Weekly && 
                (Configuration.WeekConfiguration == null || Configuration.WeekConfiguration.WeekFrecuency.HasValue == false || Configuration.WeekConfiguration.Days == null 
                || Configuration.WeekConfiguration.Days.Length == 0 || (Configuration.WeekConfiguration.WeekFrecuency.HasValue && Configuration.WeekConfiguration.WeekFrecuency <= 0)))
            {
                throw new SchedulerException("If weekly frecuency is selected you must set a week frecuency and select at least one day of the week");
            }
            if (Configuration.DailyConfiguration == null || (Configuration.DailyConfiguration.OccursOnceDaily && Configuration.DailyConfiguration.DailyHour == null) || 
                (Configuration.DailyConfiguration.OccursOnceDaily == false && 
                (Configuration.DailyConfiguration.DailyFrecuency.HasValue == false || Configuration.DailyConfiguration.DailyStartHour.HasValue == false || Configuration.DailyConfiguration.DailyEndHour.HasValue == false)))
            {
                throw new SchedulerException("You must set a Daily Configuration indicating if occurs once in the day (especifying the hour when it happens) or if it occurs multiple times " +
                    "(indicating how many hours, minutes or seconds between executions and the start and end time)");
            }
        }        
    }

    public class DateResult
    { 
        public DateTime NextDate { get; set; }
        public string Description { get; set; }
    }

    public enum SchedulerFrecuency
    {
        Daily = 0,
        Weekly = 1,
        Monthly = 2,
        Yearly = 3
    }

    public enum TimeFrecuency
    { 
        Hours = 0,
        Minutes = 1,
        Seconds = 2
    }

    public enum RecurringType
    {
        Once = 0,
        Recurring = 1
    }
}
