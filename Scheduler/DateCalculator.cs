using Scheduler.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace Scheduler
{
    public class DateCalculator
    {
        private DateTime? LastResultDate = null;
        public DateResult[] GetNextExecutionDateRecurring(SchedulerConfiguration Configuration, int Repetitions)
        {
            List<DateResult> Result = new List<DateResult>();
            int cont = 0;
            while (cont < Repetitions)
            {
                Result.Add(this.GetNextExecutionDate(Configuration));
                cont++;
            }
            return Result.ToArray();
        }

        private void SetCulture(Language Language)
        {
            switch (Language)
            {
                case Language.EnglishUK:
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
                    break;
                case Language.EnglishUS:
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                    break;
                case Language.Español:
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
                    break;
                default:
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
                    break;
            }
        }

        public DateResult? GetNextExecutionDate(SchedulerConfiguration Configuration)
        {
            this.SetCulture(Configuration.Language);
            this.CommonValidations(Configuration);
            return this.GetResultDate(Configuration);
        }

        public DateResult? GetResultDate(SchedulerConfiguration Configuration)
        {
            DateResult Result = Configuration.Type == RecurringType.Once ? this.GetOnceResult(Configuration) : this.GetRecurringResult(Configuration);
            if (Configuration.EndDate.HasValue == false ||
                Result.NextDate <= Configuration.EndDate.Value)
            {
                this.LastResultDate = Result.NextDate;
                return Result;
            }            
            return null;
        }

        private DateResult GetOnceResult(SchedulerConfiguration Configuration)
        {
            this.OnceValidations(Configuration);
            return new DateResult()
            {
                NextDate = this.GetDailyConfiguration(Configuration.ConfigurationDate.Value, Configuration),
                Description = this.GetDescription(true, this.GetDailyConfiguration(Configuration.ConfigurationDate.Value, Configuration), Configuration)
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
            DateTime LastDate = this.LastResultDate.HasValue ? this.LastResultDate.Value : Configuration.CurrentDate;
            switch (Configuration.SchedulerFrecuency)
            {
                case SchedulerFrecuency.Daily:
                    ResultDate = this.LastResultDate.HasValue ? this.GetDailyConfiguration(LastDate, Configuration) :
                        this.GetDailyConfiguration(LastDate.AddDays(Configuration.Frecuency.Value), Configuration);
                    break;
                case SchedulerFrecuency.Weekly:
                    this.WeeklyFrecuencyValidations(Configuration);
                    ResultDate = this.GetWeekDate(LastDate, Configuration);
                    break;
                case SchedulerFrecuency.Monthly:
                    this.MonthlyFrecuencyValidations(Configuration);
                    ResultDate = this.GetMonthlyConfiguration(LastDate, Configuration);
                    break;
                case SchedulerFrecuency.Yearly:
                    ResultDate = this.LastResultDate.HasValue ? this.GetDailyConfiguration(LastDate, Configuration) :
                        this.GetDailyConfiguration(LastDate.AddYears(Configuration.Frecuency.Value), Configuration);
                    break;
            }
            return ResultDate;
        }

        private string GetDescription(bool Once, DateTime ResultDate, SchedulerConfiguration Configuration)
        {
            if (Once)
            {
                string OnceFrecuencyDescription = Configuration.Translations.GetOnceDescription();
                return this.GetDailyDescription(Configuration, OnceFrecuencyDescription, ResultDate);
            }
            if (Configuration.SchedulerFrecuency == SchedulerFrecuency.Weekly)
            {
                return this.GetWeeklyDescription(Configuration);
            }
            if (Configuration.SchedulerFrecuency == SchedulerFrecuency.Monthly)
            {
                return this.GetMonthlyDescription(Configuration);
            }
            return this.GetDailyDescription(Configuration, this.GetFrecuencyDescription(Configuration), ResultDate);
        }

        private DateTime GetDailyConfiguration(DateTime Date, SchedulerConfiguration Configuration)
        {
            if (Configuration.OccursOnceDaily)
            {
                return Date.TimeOfDay == Configuration.DailyHour ? Date :
                    Date.AddTicks(Configuration.DailyHour.Value.Ticks);
            }
            if (this.LastResultDate.HasValue == false)
            {
                return Date.AddTicks(Configuration.DailyStartHour.Value.Ticks);
            }
            Date = this.IncreaseTime(Date, Configuration);
            if (Date.TimeOfDay > Configuration.DailyEndHour)
            {
                return this.IncreaseSchedulerFrecuency(Date, Configuration);
            }
            return Date;
        }

        private DateTime IncreaseTime(DateTime Date, SchedulerConfiguration Configuration)
        {
            switch (Configuration.TimeFrecuency)
            {
                case TimeFrecuency.Hours:
                    return Date.AddHours(Configuration.DailyFrecuency.Value);                    
                case TimeFrecuency.Minutes:
                    return Date.AddMinutes(Configuration.DailyFrecuency.Value);                    
                case TimeFrecuency.Seconds:
                    return Date.AddSeconds(Configuration.DailyFrecuency.Value);
            }
            return Date;
        }

        private DateTime IncreaseSchedulerFrecuency(DateTime Date, SchedulerConfiguration Configuration)
        {
            switch (Configuration.SchedulerFrecuency)
            {
                case SchedulerFrecuency.Daily:
                    Date = Date.AddDays(Configuration.Frecuency.Value);
                    break;                
                case SchedulerFrecuency.Yearly:
                    Date = Date.AddYears(Configuration.Frecuency.Value);
                    break;
            }
            return new DateTime(Date.Year, Date.Month, Date.Day, Configuration.DailyStartHour.Value.Hours, Configuration.DailyStartHour.Value.Minutes, Configuration.DailyStartHour.Value.Seconds);
        }

        private string GetFrecuencyDescription(SchedulerConfiguration Configuration)
        {
            return Configuration.Translations.GetFrecuencyDescription(Configuration.SchedulerFrecuency);
        }

        private string GetDailyDescription(SchedulerConfiguration Configuration, string Frecuency, DateTime ResultDate)
        {
            string Description = string.Format(Configuration.Translations.GetDailyDescription(),Frecuency, ResultDate.ToString(), Configuration.StartDate.ToString());
            if (Configuration.EndDate.HasValue)
            {
                Description += string.Format(Configuration.Translations.GetEndDateDescription(),Configuration.EndDate.ToString());
            }
            return Description;
        }

        private string GetDailyDescription(SchedulerConfiguration Configuration)
        {
            StringBuilder Description = new StringBuilder();
            if (Configuration.OccursOnceDaily)
            {
                Description.Append(string.Format(Configuration.Translations.GetOccursOnceDailyDescription(),Configuration.DailyHour));
            }
            else
            {
                Description.Append(string.Format(Configuration.Translations.GetDailyRecurringConfigurationDescription(), Configuration.DailyFrecuency,
                    Configuration.Translations.GetTimeFrecuencyDescription(Configuration.TimeFrecuency),Configuration.DailyStartHour,Configuration.DailyEndHour, this.DateFormatted(Configuration.StartDate)));
            }
            return Description.ToString();
        }

        private string GetWeeklyDescription(SchedulerConfiguration Configuration)
        {
            StringBuilder WeekDays = new StringBuilder();
            StringBuilder Description = new StringBuilder();
            Array.ForEach(Configuration.WeekDays, D =>
            {
                WeekDays.Append(Configuration.Translations.GetWeekDayName(D) + ", ");
            });
            WeekDays = WeekDays.Remove(WeekDays.ToString().LastIndexOf(","), 1);
            int LastComma = WeekDays.ToString().LastIndexOf(',');
            string days = WeekDays.ToString().Trim();
            if (LastComma != -1)
            {
                days = WeekDays.ToString().Remove(LastComma, 1).Insert(LastComma, string.Format(" " + Configuration.Translations.GetMultipleWeekDaysConcat())).Trim();
            }
            Description.Append(string.Format(Configuration.Translations.GetWeeklyConfigurationDescription(),Configuration.WeekFrecuency,days));
            Description.Append(this.GetDailyDescription(Configuration));
            return Description.ToString();
        }

        private DateTime GetWeekDate(DateTime CurrentDate, SchedulerConfiguration Configuration)
        {
            DateTime Date = CurrentDate;
            bool RebootHour = false;
            if (Configuration.OccursOnceDaily ||
                (Configuration.OccursOnceDaily == false && this.LastResultDate.HasValue &&
                this.LastResultDate.Value.TimeOfDay == Configuration.DailyEndHour))
            {
                RebootHour = true;
                Date = this.GetNextWeekDate(Date, Configuration);
            }
            do
            {
                if (Configuration.WeekDays.Contains(Date.DayOfWeek) == false)
                {
                    Date = Date.AddDays(1);
                }
            } while (Configuration.WeekDays.Contains(Date.DayOfWeek) == false);
            if (RebootHour && Configuration.OccursOnceDaily == false)
            {
                return new DateTime(Date.Year, Date.Month, Date.Day, Configuration.DailyStartHour.Value.Hours, Configuration.DailyStartHour.Value.Minutes, Configuration.DailyStartHour.Value.Seconds);
            }
            return this.GetDailyConfiguration(Date, Configuration);
        }

        private DateTime GetNextWeekDate(DateTime CurrentDate, SchedulerConfiguration Configuration)
        {
            DateTime Date = CurrentDate;
            if (Date.DayOfWeek >= Configuration.WeekDays.Max() || Configuration.OccursOnceDaily)
            {
                if (Date.DayOfWeek != Configuration.CurrentDate.DayOfWeek)
                {
                    Date = Configuration.CurrentDate.AddDays(Configuration.WeekFrecuency.Value * 7);
                    Configuration.CurrentDate = Date;
                }
                else
                {
                    Date = CurrentDate.AddDays(Configuration.WeekFrecuency.Value * 7);
                }
            }
            else
            {
                Date = CurrentDate.AddDays(1);
                Date = new DateTime(Date.Year, Date.Month, Date.Day, Configuration.DailyStartHour.Value.Hours, Configuration.DailyStartHour.Value.Minutes, Configuration.DailyStartHour.Value.Seconds);
            }
            return Date; 
        }

        private string GetMonthlyDescription(SchedulerConfiguration Configuration)
        {
            StringBuilder Description = new StringBuilder();
            if (Configuration.MonthDayFrecuency)
            {
                string ordinal = string.Empty;
                if (Configuration.Language == Language.EnglishUK ||
                    Configuration.Language == Language.EnglishUS)
                {
                    switch (Configuration.DayOfMonth)
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
                }
                Description.Append(string.Format(
                    Configuration.Translations.GetMonthlyMonthDayConfigurationDescription(),Configuration.DayOfMonth,ordinal,Configuration.MonthFrecuency));
                return Description.Append(" " + this.GetDailyDescription(Configuration)).ToString();
            }
            Description.Append(string.Format(Configuration.Translations.
                GetMonthlyConfigurationDescription(),Configuration.Translations.GetMonthDayFrecuencyDescription(Configuration.MonthlyDayFrecuency),Configuration.Translations.GetMonthDayNameDescription(Configuration.MonthlyWeekDayFrecuency),Configuration.MonthFrecuency));
            return Description.Append(" " + this.GetDailyDescription(Configuration)).ToString();
        }

        private DateTime GetMonthlyConfiguration(DateTime Date, SchedulerConfiguration Configuration)
        {
            DateTime Result = Date;
            if (this.LastResultDate.HasValue == false)
            {
                return this.GetDailyConfiguration(Result, Configuration);
            }
            if (Configuration.OccursOnceDaily || this.LastResultDate.Value.TimeOfDay == Configuration.DailyEndHour)
            {
                return this.IncreaseMonth(Result, Configuration);
            }
            return this.GetDailyConfiguration(Result, Configuration);
        }

        private DateTime IncreaseMonth(DateTime Result, SchedulerConfiguration Configuration)
        {
            if (Configuration.OccursOnceDaily == false)
            {
                Result = new DateTime(Result.Year, Result.Month, Result.Day, Configuration.DailyStartHour.Value.Hours, Configuration.DailyStartHour.Value.Minutes, Configuration.DailyStartHour.Value.Seconds);
            }
            Result = Result.AddMonths(Configuration.MonthFrecuency);

            if (Configuration.MonthDayFrecuency)
            {
                if (DateTime.DaysInMonth(Result.Year, Result.Month) < Configuration.DayOfMonth)
                {
                    Result = new DateTime(Result.Year, Result.Month, DateTime.DaysInMonth(Result.Year, Result.Month), Result.Hour, Result.Minute, Result.Second);
                }
                else
                {
                    Result = new DateTime(Result.Year, Result.Month, Configuration.DayOfMonth.Value, Result.Hour, Result.Minute, Result.Second);
                }
            }
            else
            {
                Result = this.GetMonthDay(Result, Configuration.MonthlyDayFrecuency, Configuration.MonthlyWeekDayFrecuency);
            }
            return Result;
        }

        private DateTime GetMonthDay(DateTime Result, MonthlyDayFrecuency DayFrecuency, MonthlyWeekDayFrecuency WeekDayFrecuency)
        {
            bool LastOcurrence = DayFrecuency == MonthlyDayFrecuency.Last;
            int cont = LastOcurrence ? 5 : 1;
            List<DayOfWeek> Days = new List<DayOfWeek>();
            int ocurrence = this.GetDayPosition(DayFrecuency);
            Days.AddRange(this.GetWeekDays(WeekDayFrecuency));
            DateTime DayOfMonth = LastOcurrence ? new DateTime(Result.Year, Result.Month, DateTime.DaysInMonth(Result.Year, Result.Month),Result.Hour, Result.Minute, Result.Second) :
                new DateTime(Result.Year, Result.Month, 1, Result.Hour,Result.Minute,Result.Second);
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
                throw new SchedulerException(Configuration.Translations.GetCurrentDateMaxMinValueMessage());
            }
            if (Configuration.StartDate == DateTime.MinValue || Configuration.StartDate == DateTime.MaxValue)
            {
                throw new SchedulerException(Configuration.Translations.GetStartDateMaxMinValueMessage());
            }
            if (Configuration.EndDate == DateTime.MinValue || Configuration.EndDate == DateTime.MaxValue)
            {
                throw new SchedulerException(Configuration.Translations.GetEndDateMaxMinValueMessage());
            }
            if (Configuration.StartDate > Configuration.EndDate)
            {
                throw new SchedulerException(Configuration.Translations.GetStartDateGreaterThanEndDateDescription());
            }
            if ((Configuration.OccursOnceDaily && Configuration.DailyHour == null) ||
                (Configuration.OccursOnceDaily == false &&
                (Configuration.DailyFrecuency.HasValue == false || Configuration.DailyStartHour.HasValue == false || Configuration.DailyEndHour.HasValue == false)))
            {
                throw new SchedulerException(Configuration.Translations.GetIncorrectDailyConfigurationDescription());
            }
        }

        private void OnceValidations(SchedulerConfiguration Configuration)
        {
            if (Configuration.ConfigurationDate.HasValue == false)
            {
                throw new SchedulerException(Configuration.Translations.GetConfigurationDateWithoutValueDescription());
            }
            if (Configuration.ConfigurationDate.HasValue && (Configuration.ConfigurationDate == DateTime.MinValue || Configuration.ConfigurationDate == DateTime.MaxValue))
            {
                throw new SchedulerException(Configuration.Translations.GetIncorrectConfigurationDateDescription());
            }
        }

        private void RecurringValidation(SchedulerConfiguration Configuration)
        {
            if (Configuration.Frecuency.HasValue == false)
            {
                throw new SchedulerException(Configuration.Translations.GetFrecuencyWithoutValueDescription());
            }
            if (Configuration.Frecuency.HasValue && (Configuration.Frecuency <= 0 || Configuration.Frecuency < Int32.MinValue || Configuration.Frecuency > Int32.MaxValue))
            {
                throw new SchedulerException(Configuration.Translations.GetIncorrectFrecuencyDescription());
            }
        }

        private void WeeklyFrecuencyValidations(SchedulerConfiguration Configuration)
        {
            if (Configuration.SchedulerFrecuency == SchedulerFrecuency.Weekly &&
                    (Configuration.WeekFrecuency.HasValue == false || Configuration.WeekDays == null
                    || Configuration.WeekDays.Length == 0 || (Configuration.WeekFrecuency.HasValue && Configuration.WeekFrecuency <= 0)))
            {
                throw new SchedulerException(Configuration.Translations.GetIncorrectWeeklyFrecuencyDescription());
            }
        }

        private void MonthlyFrecuencyValidations(SchedulerConfiguration Configuration)
        {
            if (Configuration.MonthDayFrecuency && (Configuration.DayOfMonth.HasValue == false || Configuration.DayOfMonth <= 0))
            {
                throw new SchedulerException(Configuration.Translations.GetMonthDayNotSelectedDescription());
            }
            if (Configuration.MonthFrecuency <= 0)
            {
                throw new SchedulerException(Configuration.Translations.GetPositiveMonthFrecuencyDescription());
            }
        }

        private string DateFormatted(DateTime Date)
        {
            string Time = Date.TimeOfDay.ToString();
            if (Time[0] == '0' && Time[1] == '0')
            {
                Time = Time.Remove(0, 1);
            }
            return Date.ToShortDateString() + " " + Time;
        }
    }
}
