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
        WeekConfiguration weekConfiguration;
        DailyConfiguration dailyConfiguration;

        public SchedulerConfiguration()
        { }

        public SchedulerConfiguration(DateTime CurrentDate, DateTime? ConfigurationDate, RecurringType Type, SchedulerFrecuency SchedulerFrecuency,
            int? Frecuency, DayOfWeek[] DaysOfWeek, DailyConfiguration DailyConfiguration,  DateTime StartDate, DateTime? EndDate)
        {
            this.currentDate = CurrentDate;
            this.configurationDate = ConfigurationDate;
            this.type = Type;
            this.schedulerFrecuency = SchedulerFrecuency;
            this.frecuency = Frecuency;
            this.weekConfiguration = new WeekConfiguration(DaysOfWeek,Frecuency);
            this.dailyConfiguration = DailyConfiguration;
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

        public WeekConfiguration WeekConfiguration
        {
            get
            {
                return this.weekConfiguration;
            }
        }

        public DailyConfiguration DailyConfiguration
        {
            get
            {
                return this.dailyConfiguration;
            }
        }
    }

    public class WeekConfiguration
    {
        int? weekFrecuency;
        DayOfWeek[] days;

        public WeekConfiguration(DayOfWeek[] Days, int? Frecuency)
        {
            this.weekFrecuency = Frecuency;
            days = Days;
        }

        public int? WeekFrecuency
        {
            get
            {
                return this.weekFrecuency;
            }           
        }

        public DayOfWeek[] Days
        {
            get
            {
                return this.days;
            }            
        }
    }

    public class DailyConfiguration
    {
        bool occursOnceDaily;
        TimeSpan? dailyHour;
        int? dailyFrecuency;
        TimeFrecuency timeFrecuency;
        TimeSpan? dailyStartHour;
        TimeSpan? dailyEndHour;

        public bool OccursOnceDaily
        {
            get
            {
                return this.occursOnceDaily;
            }
            set
            {
                this.occursOnceDaily = value;
            }
        }

        public TimeSpan? DailyHour
        {
            get
            {
                return this.dailyHour;
            }
            set
            {
                this.dailyHour = value.Value;
            }
        }

        public int? DailyFrecuency
        {
            get
            {
                return this.dailyFrecuency;
            }
            set
            {
                this.dailyFrecuency = value;
            }
        }

        public TimeFrecuency TimeFrecuency
        {
            get
            {
                return this.timeFrecuency;
            }
            set
            {
                this.timeFrecuency = value;
            }
        }

        public TimeSpan? DailyStartHour
        {
            get
            {
                return this.dailyStartHour;
            }
            set
            {
                this.dailyStartHour = value.Value;
            }
        }

        public TimeSpan? DailyEndHour
        {
            get
            {
                return this.dailyEndHour;
            }
            set
            {
                this.dailyEndHour = value.Value;
            }
        }

    }
}
