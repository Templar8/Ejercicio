using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler
{
    public class SchedulerConfiguration
    {
        public DateTime CurrentDate { get; set; }

        public DateTime? ConfigurationDate { get; set; }

        public RecurringType Type { get; set; }

        public SchedulerFrecuency SchedulerFrecuency { get; set; }

        public int? Frecuency { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        #region Daily Configuration

        public bool OccursOnceDaily { get; set; }

        public TimeSpan? DailyHour { get; set; }

        public int? DailyFrecuency { get; set; }

        public TimeFrecuency TimeFrecuency { get; set; }

        public TimeSpan? DailyStartHour { get; set; }

        public TimeSpan? DailyEndHour { get; set; }

        #endregion

        #region Weekly Configuration        

        public int? WeekFrecuency { get; set; }

        public DayOfWeek[] WeekDays { get; set; }

        #endregion

        #region Monthly Configuration
        public bool MonthDayFrecuency { get; set; }
        public int? DayOfMonth { get; set; }
        public int MonthFrecuency { get; set; }
        public MonthlyDayFrecuency MonthlyDayFrecuency { get; set; }
        public MonthlyWeekDayFrecuency MonthlyWeekDayFrecuency { get; set; }

        #endregion        
    }
}
