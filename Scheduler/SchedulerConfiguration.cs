using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler
{
    public class SchedulerConfiguration
    {
        private DateTime currentDate;
        private DateTime? configurationDate;
        private RecurringType type;
        private SchedulerFrecuency schedulerFrecuency;
        private int? frecuency;
        DateTime startDate;
        DateTime? endDate;

        public SchedulerConfiguration()
        { }

        public SchedulerConfiguration(DateTime CurrentDate, DateTime? ConfigurationDate, RecurringType Type, SchedulerFrecuency SchedulerFrecuency, int? Frecuency, DateTime StartDate, DateTime? EndDate)
        {
            this.currentDate = CurrentDate;
            this.configurationDate = ConfigurationDate;
            this.type = Type;
            this.schedulerFrecuency = SchedulerFrecuency;
            this.frecuency = Frecuency;
            this.startDate = StartDate;
            this.endDate = EndDate;
        }

        public DateTime CurrentDate
        {
            get
            {
                return this.currentDate;
            }
        }

        public DateTime? ConfigurationDate
        {
            get
            {
                return this.configurationDate;
            }
        }

        public RecurringType Type
        {
            get
            {
                return this.type;
            }
        }
         
        public SchedulerFrecuency SchedulerFrecuency
        {
            get
            {
                return this.schedulerFrecuency;
            }
        }

        public int? Frecuency
        {
            get
            {
                return this.frecuency;
            }
        }

        public DateTime StartDate
        {
            get
            {
                return this.startDate;
            }
        }

        public DateTime? EndDate
        {
            get
            {
                return this.endDate;
            }
        }

        public int? WeeksFrecuency { }
    }
}
