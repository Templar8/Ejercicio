using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Translations
{
    public class Translations
    {
        const string LANGUAGE_NO_VALID = "There is no translation for this text, please select a valid language";
        public Language Language { get; set; }


        public string GetFrecuencyDescription(SchedulerFrecuency Frecuency)
        {
            switch (this.Language)
            {
                case Language.EnglishUK:
                case Language.EnglishUS:
                    return Frecuency.ToString();
                case Language.Español:
                    switch (Frecuency)
                    {
                        case SchedulerFrecuency.Daily:
                            return "Diariamente";
                        case SchedulerFrecuency.Weekly:
                            return "Semanalmente";
                        case SchedulerFrecuency.Monthly:
                            return "Mensualmente";
                        case SchedulerFrecuency.Yearly:
                            return "Anualmente";
                        default:
                            return "Frecuencia no existente";
                    }
                default:
                    return LANGUAGE_NO_VALID;
            }
        }
        public string GetDailyDescription()
        {
            switch (this.Language)
            {
                case Language.EnglishUK:
                case Language.EnglishUS:
                    return "Occurs {0}. Schedule will be used on {1} starting on {2}";
                case Language.Español:
                    return "Ocurre {0}. El calendario se utilizará el {1} empezando el {2}";
                default:
                    return LANGUAGE_NO_VALID;
            }
        }
        public string GetEndDateDescription()
        {
            switch (this.Language)
            {
                case Language.EnglishUK:
                case Language.EnglishUS:
                    return " and ending on {0}";
                case Language.Español:
                    return " y terminando el {0}";
                default:
                    return LANGUAGE_NO_VALID;
            }
        }
        public string GetOccursOnceDailyDescription()
        {
            switch (this.Language)
            {
                case Language.EnglishUK:
                case Language.EnglishUS:
                    return "at {0}";
                case Language.Español:
                    return "a las {0}";
                default:
                    return LANGUAGE_NO_VALID;
            }
        }
        public string GetDailyRecurringConfigurationDescription()
        {
            switch (this.Language)
            {
                case Language.EnglishUK:
                case Language.EnglishUS:
                    return "every {0} {1} between {2} and {3} starting on {4}";
                case Language.Español:
                    return "cada {0} {1} entre el {2} y el {3} comenzando el {4}";
                default:
                    return LANGUAGE_NO_VALID;
            }
        }
        public string GetWeekDayName(DayOfWeek Day)
        {
            switch (this.Language)
            {
                case Language.EnglishUK:
                case Language.EnglishUS:
                    return Day.ToString();
                case Language.Español:
                    switch (Day)
                    {
                        case DayOfWeek.Sunday:
                            return "Domingo";
                        case DayOfWeek.Monday:
                            return "Lunes";
                        case DayOfWeek.Tuesday:
                            return "Martes";
                        case DayOfWeek.Wednesday:
                            return "Miércoles";
                        case DayOfWeek.Thursday:
                            return "Jueves";
                        case DayOfWeek.Friday:
                            return "Viernes";
                        case DayOfWeek.Saturday:
                            return "Sábado";
                        default:
                            return "Día no existente";
                    }
                default:
                    return LANGUAGE_NO_VALID;
            }
        }
        public string GetTimeFrecuencyDescription(TimeFrecuency Time)
        {
            switch (this.Language)
            {
                case Language.EnglishUK:                    
                case Language.EnglishUS:
                    return Time.ToString();
                case Language.Español:
                    switch (Time)
                    {
                        case TimeFrecuency.Hours:
                            return "Horas";
                        case TimeFrecuency.Minutes:
                            return "Minutos";
                        case TimeFrecuency.Seconds:
                            return "Segundos";
                        default:
                            return "Tiempo no existente";
                    }
                default:
                    return LANGUAGE_NO_VALID;
            }

        }
        public string GetMultipleWeekDaysConcat()
        {
            switch (this.Language)
            {
                case Language.EnglishUK:
                case Language.EnglishUS:
                    return "and";
                case Language.Español:
                    return "y";
                default:
                    return LANGUAGE_NO_VALID;
            }
        }
        public string GetWeeklyConfigurationDescription()
        {
            switch (this.Language)
            {
                case Language.EnglishUK:
                case Language.EnglishUS:
                    return "Occurs every {0} weeks on {1} ";
                case Language.Español:
                    return "Ocurre cada {0} semanas los {1} ";
                default:
                    return LANGUAGE_NO_VALID;
            }
        }
        public string GetMonthlyMonthDayConfigurationDescription()
        {
            switch (this.Language)
            {
                case Language.EnglishUK:
                case Language.EnglishUS:
                    return "Occurs the {0}{1} of every {2} months";
                case Language.Español:
                    return "Ocurre el {0}{1} de cada {2} meses";
                default:
                    return LANGUAGE_NO_VALID;
            }
        }
        public string GetMonthlyConfigurationDescription()
        {
            switch (this.Language)
            {
                case Language.EnglishUK:
                case Language.EnglishUS:
                    return "Occurs the {0} {1} of every {2} months";
                case Language.Español:
                    return "Ocurre el {0} {1} de cada {2} meses";
                default:
                    return LANGUAGE_NO_VALID;
            }
        }
        public string GetCurrentDateMaxMinValueMessage()
        {
            switch (this.Language)
            {
                case Language.EnglishUK:
                case Language.EnglishUS:
                    return "The current date can't be date min or max values";
                case Language.Español:
                    return "La fecha actual no puede ser el valor mínimo o máximo de la fecha";
                default:
                    return LANGUAGE_NO_VALID;
            }
        }
        public string GetStartDateMaxMinValueMessage()
        {
            switch (this.Language)
            {
                case Language.EnglishUK:
                case Language.EnglishUS:
                    return "The start date can't be date min or max values";
                case Language.Español:
                    return "La fecha de comienzo no puede ser el valor máximo o mínimo de la fecha";
                default:
                    return LANGUAGE_NO_VALID;
            }
        }
        public string GetEndDateMaxMinValueMessage()
        {
            switch (this.Language)
            {
                case Language.EnglishUK:
                case Language.EnglishUS:
                    return "The end date can't be date min or max values";
                case Language.Español:
                    return "La fecha fin no puede ser el valor máximo o mínimo de la fecha";
                default:
                    return LANGUAGE_NO_VALID;
            }
        }
        public string GetStartDateGreaterThanEndDateDescription()
        {
            switch (this.Language)
            {
                case Language.EnglishUK:
                case Language.EnglishUS:
                    return "Start date cannot be greater than end date";
                case Language.Español:
                    return "La fecha de comienzo no puede ser mayor que la fecha fin";
                default:
                    return LANGUAGE_NO_VALID;
            }
        }
        public string GetIncorrectDailyConfigurationDescription()
        {
            switch (this.Language)
            {
                case Language.EnglishUK:
                case Language.EnglishUS:
                    return @"You must set a Daily Configuration indicating if occurs once in the day (especifying the hour when it happens) or if it occurs multiple times (indicating how many hours, minutes or seconds between executions and the start and end time)";
                case Language.Español:
                    return @"Debe establecerse una configuración Diaria indicanto si ocurre una vez al día (especificando la hora en la que ocurre) o si ocurre repetidas veces (indicando el número de horas, minutos o segundos entre cada ejecución y la hora de inicio y fin)";
                default:
                    return LANGUAGE_NO_VALID;
            }
        }
        public string GetOnceDescription()
        {
            switch (this.Language)
            {
                case Language.EnglishUK:                    
                case Language.EnglishUS:
                    return "once";
                case Language.Español:
                    return "una vez";
                default:
                    return LANGUAGE_NO_VALID;
            }
        }
        public string GetConfigurationDateWithoutValueDescription()
        {
            switch (this.Language)
            {
                case Language.EnglishUK:                    
                case Language.EnglishUS:
                    return "If 'Once' type is selected you must indicate a Configuration DateTime in order to start the process";
                case Language.Español:
                    return "Si el modo 'Una vez' se selecciona debe indicarse una fecha de configuración para comenzar el proceso";
                default:
                    return LANGUAGE_NO_VALID;
            }
        }
        public string GetIncorrectConfigurationDateDescription()
        {
            switch (this.Language)
            {
                case Language.EnglishUK:                    
                case Language.EnglishUS:
                    return "The configuration date can't be date min or max values";
                case Language.Español:
                    return "La fecha de configuración no puede ser el valor mínimo o máximo de la fecha";
                default:
                    return LANGUAGE_NO_VALID;
            }
        }
        public string GetFrecuencyWithoutValueDescription()
        {
            switch (this.Language)
            {
                case Language.EnglishUK:                    
                case Language.EnglishUS:
                    return "If 'Recurring' type is selected you must indicate a frecuency";
                case Language.Español:
                    return "Si se selecciona el modo 'Recurrente' debe indicarse una frecuencia";
                default:
                    return LANGUAGE_NO_VALID;
            }
        }
        public string GetIncorrectFrecuencyDescription()
        {
            switch (this.Language)
            {
                case Language.EnglishUK:                    
                case Language.EnglishUS:
                    return "Frecuency can neither be negative nor exceed integer max or min values";
                case Language.Español:
                    return "La frecuencia no puede ser negativa ni exceder los valores máximo o mínimo de los enteros";
                default:
                    return LANGUAGE_NO_VALID;
            }
        }
        public string GetIncorrectWeeklyFrecuencyDescription()
        {
            switch (this.Language)
            {
                case Language.EnglishUK:                    
                case Language.EnglishUS:
                    return "If weekly frecuency is selected you must set a week frecuency and select at least one day of the week";
                case Language.Español:
                    return "Si se selecciona la frecuencia semanal debe indicarse una frecuencia y seleccionar al menos un día de la semana";
                default:
                    return LANGUAGE_NO_VALID;
            }
        }
        public string GetMonthDayNotSelectedDescription()
        {
            switch (this.Language)
            {
                case Language.EnglishUK:                    
                case Language.EnglishUS:
                    return "You must indicate a day if monthly day frecuency is selected";
                case Language.Español:
                    return "Debe indicarse un día si se selecciona frecuencia mensual";
                default:
                    return LANGUAGE_NO_VALID;
            }
        }
        public string GetPositiveMonthFrecuencyDescription()
        {
            switch (this.Language)
            {
                case Language.EnglishUK:             
                case Language.EnglishUS:
                    return "You must set a positive month frecuency";
                case Language.Español:
                    return "Debe indicarse una frecuencia mensual positiva";
                default:
                    return LANGUAGE_NO_VALID;
            }
        }
        public string GetMonthDayFrecuencyDescription(MonthlyDayFrecuency Frecuency)
        {
            switch (this.Language)
            {
                case Language.EnglishUK:
                case Language.EnglishUS:
                    return Frecuency.ToString();
                case Language.Español:
                    switch (Frecuency)
                    {
                        case MonthlyDayFrecuency.First:
                            return "Primer";
                        case MonthlyDayFrecuency.Second:
                            return "Segundo";
                        case MonthlyDayFrecuency.Third:
                            return "Tercer";
                        case MonthlyDayFrecuency.Fourth:
                            return "Cuarto";
                        case MonthlyDayFrecuency.Last:
                            return "Último";
                        default:
                            return "Frecuencia no existente";
                    }
                default:
                    return LANGUAGE_NO_VALID;
            }
        }
        public string GetMonthDayNameDescription(MonthlyWeekDayFrecuency WeekFrecuency)
        {
            switch (this.Language)
            {
                case Language.EnglishUK:                  
                case Language.EnglishUS:
                    return WeekFrecuency.ToString();
                case Language.Español:
                    switch (WeekFrecuency)
                    {
                        case MonthlyWeekDayFrecuency.Monday:
                            return "Lunes";
                        case MonthlyWeekDayFrecuency.Tuesday:
                            return "Martes";
                        case MonthlyWeekDayFrecuency.Wednesday:
                            return "Miércoles";
                        case MonthlyWeekDayFrecuency.Thursday:
                            return "Jueves";
                        case MonthlyWeekDayFrecuency.Friday:
                            return "Viernes";
                        case MonthlyWeekDayFrecuency.Saturday:
                            return "Sábado";
                        case MonthlyWeekDayFrecuency.Sunday:
                            return "Domingo";
                        case MonthlyWeekDayFrecuency.Day:
                            return "Día";
                        case MonthlyWeekDayFrecuency.Weekday:
                            return "Día de la semana";
                        case MonthlyWeekDayFrecuency.Weekendday:
                            return "Día del fin de semana";
                        default:
                            return "frecuencia semanal no existente";
                    }
                default:
                    return LANGUAGE_NO_VALID;
            }
        }
    }
}

