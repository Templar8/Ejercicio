using Scheduler.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Scheduler
{
    public class DateCalculator
    {
        public DateResult GetNextExecutionDate(SchedulerConfiguration Configuration)
        {
            this.CommonValidations(Configuration);
            return Configuration.Type == RecurringType.Once ? this.GetOnceResult(Configuration) : this.GetRecurringResult(Configuration);
        }

        private DateResult GetOnceResult(SchedulerConfiguration Configuration)
        {
            this.OnceValidations(Configuration);
            return new DateResult()
            {
                NextDate = this.GetDailyConfiguration(Configuration.ConfigurationDate.Value, Configuration.DailyConfiguration),
                Description = this.GetDescription(true, this.GetDailyConfiguration(Configuration.ConfigurationDate.Value, Configuration.DailyConfiguration), Configuration)
            };
        }

        private DateResult GetRecurringResult(SchedulerConfiguration Configuration)
        {
            this.RecurringValidation(Configuration);
            DateTime ResultDate = this.GetRecurringResultDate(Configuration);
            return new DateResult()
            {
                NextDate = ResultDate,
                Description = this.GetDescription(false, ResultDate, Configuration)
            };
        }

        private DateTime GetRecurringResultDate(SchedulerConfiguration Configuration)
        {
            DateTime ResultDate = new DateTime();
            switch (Configuration.SchedulerFrecuency)
            {
                case SchedulerFrecuency.Daily:
                    ResultDate = this.GetDailyConfiguration(Configuration.CurrentDate.AddDays(Configuration.Frecuency.Value), Configuration.DailyConfiguration);
                    break;
                case SchedulerFrecuency.Weekly:
                    this.WeeklyFrecuencyValidations(Configuration);
                    ResultDate = this.GetWeekDate(Configuration.CurrentDate, Configuration.WeekConfiguration, Configuration.DailyConfiguration);
                    break;
                case SchedulerFrecuency.Monthly:
                    this.MonthlyFrecuencyValidations(Configuration);
                    ResultDate = this.GetMonthlyConfiguration(Configuration.CurrentDate, Configuration.MonthlyConfiguration, Configuration.DailyConfiguration);                    
                    break;
                case SchedulerFrecuency.Yearly:
                    ResultDate = this.GetDailyConfiguration(Configuration.CurrentDate.AddYears(Configuration.Frecuency.Value), Configuration.DailyConfiguration);
                    break;
            }
            return ResultDate;
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
                return this.GetWeeklyDescription(Configuration);
            }
            if (Configuration.SchedulerFrecuency == SchedulerFrecuency.Monthly)
            {
                return this.GetMonthlyDescription(Configuration);
            }
            return $"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {ResultDate} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty);
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

        private string GetDailyDescription(SchedulerConfiguration Configuration)
        {
            StringBuilder Description = new StringBuilder();
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

        private string GetWeeklyDescription(SchedulerConfiguration Configuration)
        {
            StringBuilder WeekDays = new StringBuilder();
            StringBuilder Description = new StringBuilder();
            Array.ForEach(Configuration.WeekConfiguration.Days, D =>
            {
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
            Description.Append(this.GetDailyDescription(Configuration));
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

        private string GetMonthlyDescription(SchedulerConfiguration Configuration)
        {
            StringBuilder Description = new StringBuilder();
            if (Configuration.MonthlyConfiguration.DayFrecuency)
            {
                string ordinal;
                switch (Configuration.MonthlyConfiguration.DayOfMonth)
                {
                    case 1:
                        ordinal = "st";
                        break;
                    case 2:
                        ordinal = "nd";
                        break;
                    case 3:
                        ordinal = "rd";
                        break;
                    default:
                        ordinal = "th";
                        break;
                }
                Description.Append($"Occurs the {Configuration.MonthlyConfiguration.DayOfMonth}{ordinal} of every {Configuration.MonthlyConfiguration.MonthFrecuency} months");
                return Description.Append(" " + this.GetDailyDescription(Configuration)).ToString();
            }
            Description.Append($"Occurs the {Configuration.MonthlyConfiguration.MonthlyDayFrecuency} {Configuration.MonthlyConfiguration.MonthlyWeekDayFrecuency} of every {Configuration.MonthlyConfiguration.MonthFrecuency} months");
            return Description.Append(" " + this.GetDailyDescription(Configuration)).ToString();
        }

        private DateTime GetMonthlyConfiguration(DateTime Date, MonthlyConfiguration MonthlyConfiguration, DailyConfiguration DailyConfiguration)
        {
            DateTime Result = new DateTime();
            Result = Date.AddMonths(MonthlyConfiguration.MonthFrecuency);
            if (MonthlyConfiguration.DayFrecuency)
            {
                if (DateTime.DaysInMonth(Result.Year, Result.Month) < MonthlyConfiguration.DayOfMonth)
                {
                    Result = new DateTime(Result.Year, Result.Month, DateTime.DaysInMonth(Result.Year, Result.Month), Result.Hour, Result.Minute, Result.Second);
                }
                else
                {
                    Result = new DateTime(Result.Year, Result.Month, MonthlyConfiguration.DayOfMonth.Value, Result.Hour, Result.Minute, Result.Second);
                }
            }
            else
            {
                Result = this.GetMonthDay(Result, MonthlyConfiguration.MonthlyDayFrecuency, MonthlyConfiguration.MonthlyWeekDayFrecuency);
            }
            return this.GetDailyConfiguration(Result, DailyConfiguration);
        }

        private DateTime GetMonthDay(DateTime Result, MonthlyDayFrecuency DayFrecuency, MonthlyWeekDayFrecuency WeekDayFrecuency)
        {
            bool LastOcurrence = DayFrecuency == MonthlyDayFrecuency.Last;
            int cont = LastOcurrence ? 5 : 1;
            List<DayOfWeek> Days = new List<DayOfWeek>();
            int ocurrence = this.GetDayPosition(DayFrecuency);
            Days.AddRange(this.GetWeekDays(WeekDayFrecuency));
            DateTime DayOfMonth = LastOcurrence ? new DateTime(Result.Year, Result.Month, DateTime.DaysInMonth(Result.Year, Result.Month)) : new DateTime(Result.Year, Result.Month, 1);
            DateTime ResultDay = new DateTime();
            do
            {
                if (Days.Contains(DayOfMonth.DayOfWeek))
                {
                    ResultDay = DayOfMonth;
                    if (ocurrence == cont)
                    {
                        break;
                    }
                    cont++;
                }
                DayOfMonth = LastOcurrence ? DayOfMonth.AddDays(-1) : DayOfMonth.AddDays(1);
            } while (DayOfMonth.Month == Result.Month);
            return ResultDay;
        }

        private int GetDayPosition(MonthlyDayFrecuency DayFrecuency)
        {
            int ocurrence = 0;
            switch (DayFrecuency)
            {
                case MonthlyDayFrecuency.First:
                    ocurrence = 1;
                    break;
                case MonthlyDayFrecuency.Second:
                    ocurrence = 2;
                    break;
                case MonthlyDayFrecuency.Third:
                    ocurrence = 3;
                    break;
                case MonthlyDayFrecuency.Fourth:
                    ocurrence = 4;
                    break;
                case MonthlyDayFrecuency.Last:
                    ocurrence = 5;
                    break;
            }
            return ocurrence;
        }

        private DayOfWeek[] GetWeekDays(MonthlyWeekDayFrecuency WeekDayFrecuency)
        {
            List<DayOfWeek> Days = new List<DayOfWeek>();
            switch (WeekDayFrecuency)
            {
                case MonthlyWeekDayFrecuency.Monday:
                    Days.Add(DayOfWeek.Monday);
                    break;
                case MonthlyWeekDayFrecuency.Tuesday:
                    Days.Add(DayOfWeek.Tuesday);
                    break;
                case MonthlyWeekDayFrecuency.Wednesday:
                    Days.Add(DayOfWeek.Wednesday);
                    break;
                case MonthlyWeekDayFrecuency.Thursday:
                    Days.Add(DayOfWeek.Thursday);
                    break;
                case MonthlyWeekDayFrecuency.Friday:
                    Days.Add(DayOfWeek.Friday);
                    break;
                case MonthlyWeekDayFrecuency.Saturday:
                    Days.Add(DayOfWeek.Saturday);
                    break;
                case MonthlyWeekDayFrecuency.Sunday:
                    Days.Add(DayOfWeek.Sunday);
                    break;
                case MonthlyWeekDayFrecuency.Day:
                    Days.AddRange(new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday });
                    break;
                case MonthlyWeekDayFrecuency.Weekday:
                    Days.AddRange(new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday });
                    break;
                case MonthlyWeekDayFrecuency.Weekendday:
                    Days.AddRange(new DayOfWeek[] { DayOfWeek.Saturday, DayOfWeek.Sunday });
                    break;
            }
            return Days.ToArray();
        }

        private void CommonValidations(SchedulerConfiguration Configuration)
        {
            if (Configuration.CurrentDate == DateTime.MinValue || Configuration.CurrentDate == DateTime.MaxValue)
            {
                throw new SchedulerException("The current date can't be date min or max values");
            }
            if (Configuration.StartDate == DateTime.MinValue || Configuration.StartDate == DateTime.MaxValue)
            {
                throw new SchedulerException("The start date can't be date min or max values");
            }
            if (Configuration.EndDate == DateTime.MinValue || Configuration.EndDate == DateTime.MaxValue)
            {
                throw new SchedulerException("The end date can't be date min or max values");
            }
            if (Configuration.StartDate > Configuration.EndDate)
            {
                throw new SchedulerException("Start date cannot be greater than end date");
            }
            if (Configuration.DailyConfiguration == null || (Configuration.DailyConfiguration.OccursOnceDaily && Configuration.DailyConfiguration.DailyHour == null) ||
                (Configuration.DailyConfiguration.OccursOnceDaily == false &&
                (Configuration.DailyConfiguration.DailyFrecuency.HasValue == false || Configuration.DailyConfiguration.DailyStartHour.HasValue == false || Configuration.DailyConfiguration.DailyEndHour.HasValue == false)))
            {
                throw new SchedulerException("You must set a Daily Configuration indicating if occurs once in the day (especifying the hour when it happens) or if it occurs multiple times " +
                    "(indicating how many hours, minutes or seconds between executions and the start and end time)");
            }
        }

        private void OnceValidations(SchedulerConfiguration Configuration)
        {
            if (Configuration.ConfigurationDate.HasValue == false)
            {
                throw new SchedulerException("If 'Once' type is selected you must indicate a Configuration DateTime in order to start the process");
            }
            if (Configuration.ConfigurationDate.HasValue && (Configuration.ConfigurationDate == DateTime.MinValue || Configuration.ConfigurationDate == DateTime.MaxValue))
            {
                throw new SchedulerException("The configuration date can't be date min or max values");
            }
        }

        private void RecurringValidation(SchedulerConfiguration Configuration)
        {
            if (Configuration.Frecuency.HasValue == false)
            {
                throw new SchedulerException("If 'Recurring' type is selected you must indicate a frecuency");
            }
            if (Configuration.Frecuency.HasValue && (Configuration.Frecuency <= 0 || Configuration.Frecuency < Int32.MinValue || Configuration.Frecuency > Int32.MaxValue))
            {
                throw new SchedulerException("Frecuency can neither be negative nor exceed integer max or min values");
            }
        }

        private void WeeklyFrecuencyValidations(SchedulerConfiguration Configuration)
        {
            if (Configuration.SchedulerFrecuency == SchedulerFrecuency.Weekly &&
                    (Configuration.WeekConfiguration == null || Configuration.WeekConfiguration.WeekFrecuency.HasValue == false || Configuration.WeekConfiguration.Days == null
                    || Configuration.WeekConfiguration.Days.Length == 0 || (Configuration.WeekConfiguration.WeekFrecuency.HasValue && Configuration.WeekConfiguration.WeekFrecuency <= 0)))
            {
                throw new SchedulerException("If weekly frecuency is selected you must set a week frecuency and select at least one day of the week");
            }
        }

        private void MonthlyFrecuencyValidations(SchedulerConfiguration Configuration)
        {
            if (Configuration.MonthlyConfiguration == null)
            {
                throw new SchedulerException("If Monthly frecuency is selected you must set a monthly configuration");
            }
            if (Configuration.MonthlyConfiguration.DayFrecuency && (Configuration.MonthlyConfiguration.DayOfMonth.HasValue == false || Configuration.MonthlyConfiguration.DayOfMonth <= 0))
            {
                throw new SchedulerException("You must indicate a day if monthly day frecuency is selected");
            }
            if (Configuration.MonthlyConfiguration.MonthFrecuency <= 0)
            {
                throw new SchedulerException("You must set a positive month frecuency");
            }
        }
    }
}
