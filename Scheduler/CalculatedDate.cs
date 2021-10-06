using System;

namespace Scheduler
{
    public class CalculatedDate
    {
        private DateTime currentDate;
        private DateTime? configurationDate;
        private RecurringType type;
        private Frecuency schedulerFrecuency;
        private int frecuency;
        private DateTime startDate;
        private DateTime? endDate;
        public CalculatedDate(DateTime CurrentDate, DateTime? ConfigurationDate, RecurringType Type, Frecuency SchedulerFrecuency, int Frecuency, DateTime StartDate, DateTime? EndDate)
        {
            this.currentDate = CurrentDate;
            this.configurationDate = ConfigurationDate;
            this.type = Type;
            this.schedulerFrecuency = SchedulerFrecuency;
            this.frecuency = Frecuency;
            this.startDate = StartDate;
            this.endDate = EndDate;
        }


        public DateTime GetNextExecutionDate()
        {
            if (this.type == RecurringType.Once)
            {
                if (this.configurationDate.HasValue == false)
                {
                    throw new Exception("If 'Once' type is selected you must indicate a Configuration DateTime in order to start the process");
                }
                return this.configurationDate.Value;
            }
            switch (this.schedulerFrecuency)
            {
                case Frecuency.Daily:
                    this.currentDate = this.currentDate.AddDays(this.frecuency);
                    break;
                case Frecuency.Monthly:
                    this.currentDate = this.currentDate.AddMonths(this.frecuency);
                    break;
                case Frecuency.Yearly:
                    this.currentDate = this.currentDate.AddYears(this.frecuency);
                    break;
            }
            return this.currentDate;
        }


        public string GetDescription()
        {
            if (this.type == RecurringType.Once)
            {
                if (this.configurationDate.HasValue == false)
                {
                    throw new Exception("If 'Once' type is selected you must indicate a Configuration DateTime in order to start the process");
                }
                return $"Occurs once. Schedule will be used on {this.configurationDate.Value} starting on {this.startDate}"
                    + (this.endDate.HasValue ? $" and ending on {this.endDate}" : string.Empty);
            }
            return $"Occurs {this.schedulerFrecuency.ToString()}. Schedule wll be used on {this.currentDate} starting on {this.startDate}"
                + (this.endDate.HasValue ? $" and ending on {this.endDate}" : string.Empty);
        }
    }

    public enum Frecuency
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
