using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scheduler;
using System;
using Xunit;
using FluentAssertions;
using Scheduler.Exceptions;
using System.Text;

namespace Tests
{
    [TestClass]
    public class DateCalculatorTests
    {
        [TestMethod]
        public void Recurring_Type_Once_Without_Configuration_Date_Should_Throw_Exception()
        {
            DateCalculator Calculator = new DateCalculator();
            DailyConfiguration DailyConfiguration = new DailyConfiguration() {
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)                
            };
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (DateTime.Today, null, RecurringType.Once, SchedulerFrecuency.Daily, null, null, DailyConfiguration, DateTime.Today, null);
            Action NextDate = () => Calculator.GetNextExecutionDate(Configuration);
            NextDate.Should().Throw<SchedulerException>();
            try
            {
                NextDate.Invoke();
            }
            catch (SchedulerException e)
            {
                e.Message.Should().Be("If 'Once' type is selected you must indicate a Configuration DateTime in order to start the process");
            }
        }
        [TestMethod]
        public void Start_Date_Greater_Than_End_Date_Should_Throw_Exception()
        {
            DateCalculator Calculator = new DateCalculator();
            DailyConfiguration DailyConfiguration = new DailyConfiguration()
            {
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (DateTime.Today, null, RecurringType.Once, SchedulerFrecuency.Daily, null, null, DailyConfiguration, DateTime.Today, DateTime.Today.AddDays(-5));
            Action NextDate = () => Calculator.GetNextExecutionDate(Configuration);
            NextDate.Should().Throw<SchedulerException>();
            try
            {
                NextDate.Invoke();
            }
            catch (SchedulerException e)
            {
                e.Message.Should().Be("Start date cannot be greater than end date");
            }
        }
        [TestMethod]
        public void Null_Daily_configuration_Should_Throw_exception()
        {
            DateCalculator Calculator = new DateCalculator();            
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (DateTime.Today, null, RecurringType.Once, SchedulerFrecuency.Daily, null, null, null, DateTime.Today, null);
            Action NextDate = () => Calculator.GetNextExecutionDate(Configuration);
            NextDate.Should().Throw<SchedulerException>();
            try
            {
                NextDate.Invoke();
            }
            catch (SchedulerException e)
            {
                e.Message.Should().Be("You must set a Daily Configuration indicating if occurs once in the day (especifying the hour when it happens) " +
                    "or if it occurs multiple times (indicating how many hours, minutes or seconds between executions and the start and end time)");
            }
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Negative_Frecuency_Should_Throw_Exception()
        {
            DateCalculator Calculator = new DateCalculator();
            DailyConfiguration DailyConfiguration = new DailyConfiguration()
            {
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (DateTime.Today, null, RecurringType.Recurring, SchedulerFrecuency.Daily, -5, null, DailyConfiguration, DateTime.Today, null);
            Action NextDate = () => Calculator.GetNextExecutionDate(Configuration);
            NextDate.Should().Throw<SchedulerException>();
            try
            {
                NextDate.Invoke();
            }
            catch (SchedulerException e)
            {
                e.Message.Should().Be("Frecuency can neither be negative nor exceed integer max or min values");
            }
        }
        [TestMethod]
        public void Recurring_Type_Recurring_Weekly_Without_Frecuency_Should_Throw_exception()
        {
            DateCalculator Calculator = new DateCalculator();
            DailyConfiguration DailyConfiguration = new DailyConfiguration()
            {
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };
            DayOfWeek[] Days = new DayOfWeek[] { DayOfWeek.Monday };
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (DateTime.Today, null, RecurringType.Recurring, SchedulerFrecuency.Weekly, null, Days, DailyConfiguration, DateTime.Today, null);

            Action NextDate = () => Calculator.GetNextExecutionDate(Configuration);
            NextDate.Should().Throw<SchedulerException>();
            try
            {
                NextDate.Invoke();
            }
            catch (SchedulerException e)
            {
                e.Message.Should().Be("If weekly frecuency is selected you must set a week frecuency and select at least one day of the week");
            }

        }
        [TestMethod]
        public void Recurring_Type_Recurring_Weekly_With_Negative_Frecuency_Should_Throw_exception()
        {
            DateCalculator Calculator = new DateCalculator();
            DailyConfiguration DailyConfiguration = new DailyConfiguration()
            {
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };
            DayOfWeek[] Days = new DayOfWeek[] { DayOfWeek.Monday };                
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (DateTime.Today, null, RecurringType.Recurring, SchedulerFrecuency.Weekly, -5, Days, DailyConfiguration, DateTime.Today, null);

            Action NextDate = () => Calculator.GetNextExecutionDate(Configuration);
            NextDate.Should().Throw<SchedulerException>();
            try
            {
                NextDate.Invoke();
            }
            catch (SchedulerException e)
            {
                e.Message.Should().Be("Frecuency can neither be negative nor exceed integer max or min values");
            }

        }
        [TestMethod]
        public void Recurring_Type_Recurring_Weekly_With_Null_Days_Should_Throw_exception()
        {
            DateCalculator Calculator = new DateCalculator();
            DailyConfiguration DailyConfiguration = new DailyConfiguration()
            {
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };            
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (DateTime.Today, null, RecurringType.Recurring, SchedulerFrecuency.Weekly, 2, null, DailyConfiguration, DateTime.Today, null);

            Action NextDate = () => Calculator.GetNextExecutionDate(Configuration);
            NextDate.Should().Throw<SchedulerException>();
            try
            {
                NextDate.Invoke();
            }
            catch (SchedulerException e)
            {
                e.Message.Should().Be("If weekly frecuency is selected you must set a week frecuency and select at least one day of the week");
            }

        }
        [TestMethod]
        public void Recurring_Type_Once_With_Configuration_Date_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DailyConfiguration DailyConfiguration = new DailyConfiguration()
            {
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (TestDate, TestDate.AddDays(5), RecurringType.Once, SchedulerFrecuency.Daily, null, null, DailyConfiguration, TestDate, null);
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = new DateTime(2021,10,23,15,30,0);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().NotBeNullOrEmpty();
            Result.Description.Should().Be($"Occurs once. Schedule will be used on {ExpectedDateTime} starting on {Configuration.StartDate}"
                    + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Daily_Frecuency_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DailyConfiguration DailyConfiguration = new DailyConfiguration()
            {
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (DateTime.Today, null, RecurringType.Recurring, SchedulerFrecuency.Daily, 5, null, DailyConfiguration, TestDate, null);
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = new DateTime(2021,10,23,15,30,0);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().NotBeNullOrEmpty();
            Result.Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {ExpectedDateTime} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DailyConfiguration DailyConfiguration = new DailyConfiguration()
            {
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (TestDate, null, RecurringType.Recurring, SchedulerFrecuency.Monthly, 5, null, DailyConfiguration, TestDate, null);
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = new DateTime(2022,3,18,15,30,0);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().NotBeNullOrEmpty();
            Result.Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {ExpectedDateTime} starting on {Configuration.StartDate}"
               + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Yearly_Frecuency_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DailyConfiguration DailyConfiguration = new DailyConfiguration()
            {
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (TestDate, null, RecurringType.Recurring, SchedulerFrecuency.Yearly, 5, null, DailyConfiguration, TestDate, null);
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = new DateTime(2026,10,18,15,30,0);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().NotBeNullOrEmpty();
            Result.Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {ExpectedDateTime} starting on {Configuration.StartDate}"
               + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Weekly_Frecuency_On_Mondays_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021,10,18);
            DateCalculator Calculator = new DateCalculator();
            DailyConfiguration DailyConfiguration = new DailyConfiguration()
            {
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };
            DayOfWeek[] Days = new DayOfWeek[] { DayOfWeek.Monday };
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (TestDate, null, RecurringType.Recurring, SchedulerFrecuency.Weekly, 5, Days, DailyConfiguration, DateTime.Today, null);
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = TestDate.AddDays(Configuration.WeekConfiguration.WeekFrecuency.Value * 7).AddTicks(Configuration.DailyConfiguration.DailyHour.Value.Ticks);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().NotBeNullOrEmpty();
            Result.Description.Should().Be("Occurs every 5 weeks on Monday at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Weekly_Frecuency_On_Fridays_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DailyConfiguration DailyConfiguration = new DailyConfiguration()
            {
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };
            DayOfWeek[] Days = new DayOfWeek[] { DayOfWeek.Friday };
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (TestDate, null, RecurringType.Recurring, SchedulerFrecuency.Weekly, 5, Days, DailyConfiguration, DateTime.Today, null);
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = TestDate.AddDays(Configuration.WeekConfiguration.WeekFrecuency.Value * 7 + 4).AddTicks(Configuration.DailyConfiguration.DailyHour.Value.Ticks);
            Result.NextDate.Should().Be(TestDate.AddDays(Configuration.WeekConfiguration.WeekFrecuency.Value * 7 + 4).AddTicks(Configuration.DailyConfiguration.DailyHour.Value.Ticks));
            Result.Description.Should().NotBeNullOrEmpty();
            Result.Description.Should().Be("Occurs every 5 weeks on Friday at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Weekly_Frecuency_On_Mondays_And_Fridays_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DailyConfiguration DailyConfiguration = new DailyConfiguration()
            {
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };
            DayOfWeek[] Days = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Friday };
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (TestDate, null, RecurringType.Recurring, SchedulerFrecuency.Weekly, 5, Days, DailyConfiguration, DateTime.Today, null);
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = TestDate.AddDays(Configuration.WeekConfiguration.WeekFrecuency.Value * 7).AddTicks(Configuration.DailyConfiguration.DailyHour.Value.Ticks);
            Result.NextDate.Should().Be(TestDate.AddDays(Configuration.WeekConfiguration.WeekFrecuency.Value * 7).AddTicks(Configuration.DailyConfiguration.DailyHour.Value.Ticks));
            Result.Description.Should().NotBeNullOrEmpty();
            Result.Description.Should().Be("Occurs every 5 weeks on Monday and Friday at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Weekly_Frecuency_On_Mondays_And_Fridays_With_Recurring_daily_Frecuency_Hourly_Frecuency_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DailyConfiguration DailyConfiguration = new DailyConfiguration()
            {
                OccursOnceDaily = false,                
                DailyStartHour = new TimeSpan(6,30,0),
                DailyEndHour = new TimeSpan (10,30,0),
                TimeFrecuency = TimeFrecuency.Hours,
                DailyFrecuency = 1
            };
            DayOfWeek[] Days = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Friday };
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (TestDate, null, RecurringType.Recurring, SchedulerFrecuency.Weekly, 5, Days, DailyConfiguration, DateTime.Today, null);
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = new DateTime(2021, 11, 22, 7, 30, 0);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().NotBeNullOrEmpty();
            Result.Description.Should().Be($"Occurs every 5 weeks on Monday and Friday every 1 Hours between 06:30:00 and 10:30:00 starting on {DateTime.Today}");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Weekly_Frecuency_On_Mondays_And_Fridays_With_Recurring_daily_Frecuency_Minute_Frecuency_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DailyConfiguration DailyConfiguration = new DailyConfiguration()
            {
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(10, 30, 0),
                TimeFrecuency = TimeFrecuency.Minutes,
                DailyFrecuency = 1
            };
            DayOfWeek[] Days = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Friday };
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (TestDate, null, RecurringType.Recurring, SchedulerFrecuency.Weekly, 5, Days, DailyConfiguration, DateTime.Today, null);
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = new DateTime(2021, 11, 22, 6, 31, 0);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().NotBeNullOrEmpty();
            Result.Description.Should().Be($"Occurs every 5 weeks on Monday and Friday every 1 Minutes between 06:30:00 and 10:30:00 starting on {DateTime.Today}");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Weekly_Frecuency_On_Mondays_And_Fridays_With_Recurring_daily_Frecuency_Second_Frecuency_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DailyConfiguration DailyConfiguration = new DailyConfiguration()
            {
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(10, 30, 0),
                TimeFrecuency = TimeFrecuency.Seconds,
                DailyFrecuency = 1
            };
            DayOfWeek[] Days = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Friday };
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (TestDate, null, RecurringType.Recurring, SchedulerFrecuency.Weekly, 5, Days, DailyConfiguration, DateTime.Today, null);
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = new DateTime(2021, 11, 22, 6, 30, 1);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().NotBeNullOrEmpty();
            Result.Description.Should().Be($"Occurs every 5 weeks on Monday and Friday every 1 Seconds between 06:30:00 and 10:30:00 starting on {DateTime.Today}");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Daily_Configuration_Occurs_Recurring_Without_Frecuencys_Should_Throw_Exception()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DailyConfiguration DailyConfiguration = new DailyConfiguration()
            {
                OccursOnceDaily = false,               
            };
            DayOfWeek[] Days = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Friday };
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (TestDate, null, RecurringType.Recurring, SchedulerFrecuency.Weekly, 5, Days, DailyConfiguration, DateTime.Today, null);            
            Action NextDate = () => Calculator.GetNextExecutionDate(Configuration);
            NextDate.Should().Throw<SchedulerException>();
            try
            {
                NextDate.Invoke();
            }
            catch (SchedulerException e)
            {
                e.Message.Should().Be("You must set a Daily Configuration indicating if occurs once in the day (especifying the hour when it happens) or if it occurs multiple times " +
                    "(indicating how many hours, minutes or seconds between executions and the start and end time)");
            }
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Daily_Configuration_Occurs_Once_Without_Hour_Should_Throw_Exception()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DailyConfiguration DailyConfiguration = new DailyConfiguration()
            {
                OccursOnceDaily = true,
            };
            DayOfWeek[] Days = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Friday };
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (TestDate, null, RecurringType.Recurring, SchedulerFrecuency.Weekly, 5, Days, DailyConfiguration, DateTime.Today, null);
            Action NextDate = () => Calculator.GetNextExecutionDate(Configuration);
            NextDate.Should().Throw<SchedulerException>();
            try
            {
                NextDate.Invoke();
            }
            catch (SchedulerException e)
            {
                e.Message.Should().Be("You must set a Daily Configuration indicating if occurs once in the day (especifying the hour when it happens) or if it occurs multiple times " +
                    "(indicating how many hours, minutes or seconds between executions and the start and end time)");
            }
        }
    }
}
