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
        MonthlyConfiguration monthlyConfiguration;
        WeekConfiguration weekConfiguration;
        DailyConfiguration dailyConfiguration;

        public SchedulerConfiguration(DateTime CurrentDate, DateTime? ConfigurationDate, RecurringType Type, SchedulerFrecuency SchedulerFrecuency,
            int? Frecuency, DayOfWeek[] DaysOfWeek, DailyConfiguration DailyConfiguration, MonthlyConfiguration MonthlyConfiguration, DateTime StartDate, DateTime? EndDate)
        {
            this.currentDate = CurrentDate;
            this.configurationDate = ConfigurationDate;
            this.type = Type;
            this.schedulerFrecuency = SchedulerFrecuency;
            this.frecuency = Frecuency;
            this.weekConfiguration = new WeekConfiguration(DaysOfWeek, Frecuency);
            this.monthlyConfiguration = MonthlyConfiguration;
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

        public MonthlyConfiguration MonthlyConfiguration
        {
            get
            {
                return this.monthlyConfiguration;
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

    public class MonthlyConfiguration
    {
        public bool DayFrecuency { get; set; }
        public int? DayOfMonth { get; set; }
        public int MonthFrecuency { get; set; }
        public MonthlyDayFrecuency MonthlyDayFrecuency { get; set; }
        public MonthlyWeekDayFrecuency MonthlyWeekDayFrecuency { get; set; }
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
        public bool OccursOnceDaily { get; set; }

        public TimeSpan? DailyHour { get; set; }        

        public int? DailyFrecuency { get; set; }        

        public TimeFrecuency TimeFrecuency { get; set; }        

        public TimeSpan? DailyStartHour { get; set; }        

        public TimeSpan? DailyEndHour { get; set; }
    }
}
