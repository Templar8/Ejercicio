using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler
{
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

    public enum MonthlyDayFrecuency
    { 
        First = 0,
        Second = 1,
        Third = 2,
        Fourth = 3,
        Last = 4
    }

    public enum MonthlyWeekDayFrecuency
    { 
        Monday = 0,
        Tuesday = 1,
        Wednesday = 2,
        Thursday = 3,
        Friday = 4,
        Saturday = 5,
        Sunday = 6,
        Day = 7,
        Weekday = 8,
        Weekendday = 9
    }

    public enum Language
    { 
        EnglishUK = 0,
        EnglishUS = 1,
        Español = 2
    }
}
