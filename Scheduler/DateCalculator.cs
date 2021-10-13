using Scheduler.Exceptions;
using System;
using System.Collections.Generic;

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
                    NextDate = Configuration.ConfigurationDate.Value,
                    Description = $"Occurs once. Schedule will be used on {Configuration.ConfigurationDate.Value} starting on {Configuration.StartDate}"
                    + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty)
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
                    ResultDate = Configuration.CurrentDate.AddDays(Configuration.Frecuency.Value);
                    break;
                case SchedulerFrecuency.Monthly:
                    ResultDate = Configuration.CurrentDate.AddMonths(Configuration.Frecuency.Value);
                    break;
                case SchedulerFrecuency.Yearly:
                    ResultDate = Configuration.CurrentDate.AddYears(Configuration.Frecuency.Value);
                    break;
            }            
            return new DateResult() { 
                NextDate = ResultDate,
                Description = $"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule wll be used on {ResultDate} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty)
            };
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
