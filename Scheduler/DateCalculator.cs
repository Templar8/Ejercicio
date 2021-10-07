using System;
using System.Collections.Generic;

namespace Scheduler
{
    public class DateCalculator
    {
        public DateResult GetNextExecutionDate(DateTime CurrentDate, DateTime? ConfigurationDate, RecurringType Type, SchedulerFrecuency SchedulerFrecuency, int? Frecuency, DateTime StartDate, DateTime? EndDate)
        {
            this.ParameterValidation(CurrentDate, ConfigurationDate, Frecuency, StartDate, EndDate);
            if (Type == RecurringType.Once)
            {
                if (ConfigurationDate.HasValue == false)
                {
                    throw new Exception("If 'Once' type is selected you must indicate a Configuration DateTime in order to start the process");
                }
                return new DateResult() {
                    NextDate = ConfigurationDate.Value,
                    Description = $"Occurs once. Schedule will be used on {ConfigurationDate.Value} starting on {StartDate}"
                    + (EndDate.HasValue ? $" and ending on {EndDate}" : string.Empty)
                };                
            }
            DateTime ResultDate = new DateTime();
            if (Frecuency.HasValue == false)
            {
                throw new Exception("If 'Recurring' type is selected you must indicate a frecuency");
            }            
            switch (SchedulerFrecuency)
            {
                case SchedulerFrecuency.Daily:
                    ResultDate = CurrentDate.AddDays(Frecuency.Value);
                    break;
                case SchedulerFrecuency.Monthly:
                    ResultDate = CurrentDate.AddMonths(Frecuency.Value);
                    break;
                case SchedulerFrecuency.Yearly:
                    ResultDate = CurrentDate.AddYears(Frecuency.Value);
                    break;
            }            
            return new DateResult() { 
                NextDate = ResultDate,
                Description = $"Occurs {SchedulerFrecuency.ToString()}. Schedule wll be used on {ResultDate} starting on {StartDate}"
                + (EndDate.HasValue ? $" and ending on {EndDate}" : string.Empty)
            };
        }        

        private void ParameterValidation(DateTime CurrentDate, DateTime? ConfigurationDate, int? Frecuency, DateTime StartDate, DateTime? EndDate)
        {
            if (CurrentDate < DateTime.MinValue || CurrentDate > DateTime.MaxValue)
            {
                throw new Exception("The current date exceeds the supported date min and max values");
            }
            if (StartDate < DateTime.MinValue || StartDate > DateTime.MaxValue)
            {
                throw new Exception("The start date exceeds the supported date min and max values");
            }
            if (EndDate < DateTime.MinValue || EndDate > DateTime.MaxValue)
            {
                throw new Exception("The end date exceeds the supported date min and max values");
            }
            if (ConfigurationDate.HasValue && (ConfigurationDate < DateTime.MinValue || ConfigurationDate > DateTime.MaxValue))
            {
                throw new Exception("The configuration date exceeds the supported date min and max values");
            }
            if (StartDate > EndDate)
            {
                throw new Exception("Start date cannot be greater than end date");
            }
            if (Frecuency.HasValue && (Frecuency <= 0 || Frecuency < Int32.MinValue || Frecuency > Int32.MaxValue))
            {
                throw new Exception("Frecuency can neither be negative nor exceed integer max or min values");
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
        Monthly = 1,
        Yearly = 2
    }

    public enum RecurringType
    {
        Once = 0,
        Recurring = 1
    }
}
