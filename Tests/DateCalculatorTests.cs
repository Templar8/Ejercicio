using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scheduler;
using System;
using Xunit;
using FluentAssertions;
using Scheduler.Exceptions;
using System.Text;
using System.Collections.Generic;

namespace Tests
{
    [TestClass]
    public class DateCalculatorTests
    {
        [TestMethod]
        [ExpectedException(typeof(SchedulerException),
            "If 'Once' type is selected you must indicate a Configuration DateTime in order to start the process")]
        public void Recurring_Type_Once_Without_Configuration_Date_Should_Throw_Exception()
        {
            DateCalculator Calculator = new DateCalculator();
            DailyConfiguration DailyConfiguration = new DailyConfiguration()
            {
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (DateTime.Today, null, RecurringType.Once, SchedulerFrecuency.Daily, null, null, DailyConfiguration, null, DateTime.Today, null);
            Calculator.GetNextExecutionDate(Configuration);
        }
        [TestMethod]
        [ExpectedException(typeof(SchedulerException),
            "Start date cannot be greater than end date")]
        public void Start_Date_Greater_Than_End_Date_Should_Throw_Exception()
        {
            DateCalculator Calculator = new DateCalculator();
            DailyConfiguration DailyConfiguration = new DailyConfiguration()
            {
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (DateTime.Today, null, RecurringType.Once, SchedulerFrecuency.Daily, null, null, DailyConfiguration, null, DateTime.Today, DateTime.Today.AddDays(-5));
            Calculator.GetNextExecutionDate(Configuration);
        }
        [TestMethod]
        [ExpectedException(typeof(SchedulerException),
            @"You must set a Daily Configuration indicating if occurs once in the day (especifying the hour when it happens)  
             or if it occurs multiple times (indicating how many hours, minutes or seconds between executions and the start and end time)")]
        public void Null_Daily_configuration_Should_Throw_exception()
        {
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (DateTime.Today, null, RecurringType.Once, SchedulerFrecuency.Daily, null, null, null, null, DateTime.Today, null);
            Calculator.GetNextExecutionDate(Configuration);
        }
        [TestMethod]
        [ExpectedException(typeof(SchedulerException),
            "Frecuency can neither be negative nor exceed integer max or min values")]
        public void Recurring_Type_Recurring_With_Negative_Frecuency_Should_Throw_Exception()
        {
            DateCalculator Calculator = new DateCalculator();
            DailyConfiguration DailyConfiguration = new DailyConfiguration()
            {
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (DateTime.Today, null, RecurringType.Recurring, SchedulerFrecuency.Daily, -5, null, DailyConfiguration, null, DateTime.Today, null);
            Calculator.GetNextExecutionDate(Configuration);
        }
       
        [TestMethod]
        [ExpectedException(typeof(SchedulerException),
            @"You must set a Daily Configuration indicating if occurs once in the day (especifying the hour when it happens) or if it occurs multiple times 
             (indicating how many hours, minutes or seconds between executions and the start and end time)")]
        public void Recurring_Type_Recurring_With_Daily_Configuration_Recurring_Without_Frecuencys_Should_Throw_Exception()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DailyConfiguration DailyConfiguration = new DailyConfiguration()
            {
                OccursOnceDaily = false,
            };
            DayOfWeek[] Days = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Friday };
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (TestDate, null, RecurringType.Recurring, SchedulerFrecuency.Weekly, 5, Days, DailyConfiguration, null, DateTime.Today, null);
            Calculator.GetNextExecutionDate(Configuration);
        }
        [TestMethod]
        [ExpectedException(typeof(SchedulerException),
           @"You must set a Daily Configuration indicating if occurs once in the day (especifying the hour when it happens) or if it occurs multiple times
            (indicating how many hours, minutes or seconds between executions and the start and end time)")]
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
                (TestDate, null, RecurringType.Recurring, SchedulerFrecuency.Weekly, 5, Days, DailyConfiguration, null, DateTime.Today, null);
            Calculator.GetNextExecutionDate(Configuration);
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
                (TestDate, TestDate.AddDays(5), RecurringType.Once, SchedulerFrecuency.Daily, null, null, DailyConfiguration, null, TestDate, null);
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = new DateTime(2021, 10, 23, 15, 30, 0);
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
                (TestDate, null, RecurringType.Recurring, SchedulerFrecuency.Daily, 5, null, DailyConfiguration, null, TestDate, null);
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = new DateTime(2021, 10, 23, 15, 30, 0);
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
            MonthlyConfiguration MonthlyConfiguration = new MonthlyConfiguration()
            {
                DayFrecuency = true,
                DayOfMonth = 18,
                MonthFrecuency = 5
            };
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (TestDate, null, RecurringType.Recurring, SchedulerFrecuency.Monthly, 5, null, DailyConfiguration, MonthlyConfiguration, TestDate, null);
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = new DateTime(2022, 3, 18, 15, 30, 0);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().NotBeNullOrEmpty();
            Result.Description.Should().Be($"Occurs the 18th of every 5 months at 15:30:00");
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
                (TestDate, null, RecurringType.Recurring, SchedulerFrecuency.Yearly, 5, null, DailyConfiguration, null, TestDate, null);
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = new DateTime(2026, 10, 18, 15, 30, 0);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().NotBeNullOrEmpty();
            Result.Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {ExpectedDateTime} starting on {Configuration.StartDate}"
               + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
        }
        [TestMethod]
        [ExpectedException(typeof(SchedulerException),
           "If weekly frecuency is selected you must set a week frecuency and select at least one day of the week")]
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
                (DateTime.Today, null, RecurringType.Recurring, SchedulerFrecuency.Weekly, null, Days, DailyConfiguration, null, DateTime.Today, null);
            Calculator.GetNextExecutionDate(Configuration);
        }
        [TestMethod]
        [ExpectedException(typeof(SchedulerException),
           "Frecuency can neither be negative nor exceed integer max or min values")]
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
                (DateTime.Today, null, RecurringType.Recurring, SchedulerFrecuency.Weekly, -5, Days, DailyConfiguration, null, DateTime.Today, null);

            Calculator.GetNextExecutionDate(Configuration);
        }
        [TestMethod]
        [ExpectedException(typeof(SchedulerException),
            "If weekly frecuency is selected you must set a week frecuency and select at least one day of the week")]
        public void Recurring_Type_Recurring_Weekly_With_Null_Days_Should_Throw_exception()
        {
            DateCalculator Calculator = new DateCalculator();
            DailyConfiguration DailyConfiguration = new DailyConfiguration()
            {
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (DateTime.Today, null, RecurringType.Recurring, SchedulerFrecuency.Weekly, 2, null, DailyConfiguration, null, DateTime.Today, null);

            Calculator.GetNextExecutionDate(Configuration);

        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Weekly_Frecuency_On_Mondays_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DailyConfiguration DailyConfiguration = new DailyConfiguration()
            {
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };
            DayOfWeek[] Days = new DayOfWeek[] { DayOfWeek.Monday };
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (TestDate, null, RecurringType.Recurring, SchedulerFrecuency.Weekly, 5, Days, DailyConfiguration, null, DateTime.Today, null);
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
                (TestDate, null, RecurringType.Recurring, SchedulerFrecuency.Weekly, 5, Days, DailyConfiguration, null, DateTime.Today, null);
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
                (TestDate, null, RecurringType.Recurring, SchedulerFrecuency.Weekly, 5, Days, DailyConfiguration, null, DateTime.Today, null);
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
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(10, 30, 0),
                TimeFrecuency = TimeFrecuency.Hours,
                DailyFrecuency = 1
            };
            DayOfWeek[] Days = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Friday };
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (TestDate, null, RecurringType.Recurring, SchedulerFrecuency.Weekly, 5, Days, DailyConfiguration, null, DateTime.Today, null);
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
                (TestDate, null, RecurringType.Recurring, SchedulerFrecuency.Weekly, 5, Days, DailyConfiguration, null, DateTime.Today, null);
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
                (TestDate, null, RecurringType.Recurring, SchedulerFrecuency.Weekly, 5, Days, DailyConfiguration, null, DateTime.Today, null);
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = new DateTime(2021, 11, 22, 6, 30, 1);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().NotBeNullOrEmpty();
            Result.Description.Should().Be($"Occurs every 5 weeks on Monday and Friday every 1 Seconds between 06:30:00 and 10:30:00 starting on {DateTime.Today}");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_10th_Every_2_Months_With_Daily_Frecuency_Once_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DailyConfiguration DailyConfiguration = new DailyConfiguration()
            {
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };
            MonthlyConfiguration MonthlyConfiguration = new MonthlyConfiguration()
            {
                DayFrecuency = true,
                DayOfMonth = 10,
                MonthFrecuency = 2
            };
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (TestDate, null, RecurringType.Recurring, SchedulerFrecuency.Monthly, 5, null, DailyConfiguration, MonthlyConfiguration, DateTime.Today, null);
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = new DateTime(2021, 12, 10, 15, 30, 0);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().NotBeNullOrEmpty();
            Result.Description.Should().Be($"Occurs the 10th of every 2 months at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_First_Tuesday_Every_2_Months_With_Daily_Frecuency_Once_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DailyConfiguration DailyConfiguration = new DailyConfiguration()
            {
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };
            MonthlyConfiguration MonthlyConfiguration = new MonthlyConfiguration()
            {
                DayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.First,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Tuesday,
                MonthFrecuency = 2
            };
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (TestDate, null, RecurringType.Recurring, SchedulerFrecuency.Monthly, 5, null, DailyConfiguration, MonthlyConfiguration, DateTime.Today, null);
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = new DateTime(2021, 12, 7, 15, 30, 0);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().NotBeNullOrEmpty();
            Result.Description.Should().Be($"Occurs the First Tuesday of every 2 months at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_Last_Saturday_Every_2_Months_With_Daily_Frecuency_Once_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DailyConfiguration DailyConfiguration = new DailyConfiguration()
            {
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };
            MonthlyConfiguration MonthlyConfiguration = new MonthlyConfiguration()
            {
                DayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Last,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Saturday,
                MonthFrecuency = 2
            };
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (TestDate, null, RecurringType.Recurring, SchedulerFrecuency.Monthly, 5, null, DailyConfiguration, MonthlyConfiguration, DateTime.Today, null);
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = new DateTime(2021, 12, 25, 15, 30, 0);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().NotBeNullOrEmpty();
            Result.Description.Should().Be($"Occurs the Last Saturday of every 2 months at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_Second_Weekday_Every_2_Months_With_Daily_Frecuency_Once_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DailyConfiguration DailyConfiguration = new DailyConfiguration()
            {
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };
            MonthlyConfiguration MonthlyConfiguration = new MonthlyConfiguration()
            {
                DayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Second,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Weekday,
                MonthFrecuency = 2
            };
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (TestDate, null, RecurringType.Recurring, SchedulerFrecuency.Monthly, 5, null, DailyConfiguration, MonthlyConfiguration, DateTime.Today, null);
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = new DateTime(2021, 12, 2, 15, 30, 0);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().NotBeNullOrEmpty();
            Result.Description.Should().Be($"Occurs the Second Weekday of every 2 months at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_Second_WeekendDay_Every_2_Months_With_Daily_Frecuency_Once_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DailyConfiguration DailyConfiguration = new DailyConfiguration()
            {
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };
            MonthlyConfiguration MonthlyConfiguration = new MonthlyConfiguration()
            {
                DayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Second,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Weekendday,
                MonthFrecuency = 2
            };
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (TestDate, null, RecurringType.Recurring, SchedulerFrecuency.Monthly, 5, null, DailyConfiguration, MonthlyConfiguration, DateTime.Today, null);
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = new DateTime(2021, 12, 5, 15, 30, 0);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().NotBeNullOrEmpty();
            Result.Description.Should().Be($"Occurs the Second Weekendday of every 2 months at 15:30:00");
        }
        [TestMethod]        
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_Third_WeekendDay_Every_2_Months_With_Daily_Frecuency_Once_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DailyConfiguration DailyConfiguration = new DailyConfiguration()
            {
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };
            MonthlyConfiguration MonthlyConfiguration = new MonthlyConfiguration()
            {
                DayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Third,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Weekendday,
                MonthFrecuency = 2
            };
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (TestDate, null, RecurringType.Recurring, SchedulerFrecuency.Monthly, 5, null, DailyConfiguration, MonthlyConfiguration, DateTime.Today, null);
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = new DateTime(2021, 12, 11, 15, 30, 0);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().NotBeNullOrEmpty();
            Result.Description.Should().Be($"Occurs the Third Weekendday of every 2 months at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_31th_Every_2_Months_With_Daily_Frecuency_Once_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 9, 18);
            DateCalculator Calculator = new DateCalculator();
            DailyConfiguration DailyConfiguration = new DailyConfiguration()
            {
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };
            MonthlyConfiguration MonthlyConfiguration = new MonthlyConfiguration()
            {
                DayFrecuency = true,
                DayOfMonth = 31,
                MonthFrecuency = 2
            };
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (TestDate, null, RecurringType.Recurring, SchedulerFrecuency.Monthly, 5, null, DailyConfiguration, MonthlyConfiguration, DateTime.Today, null);
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = new DateTime(2021, 11, 30, 15, 30, 0);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().NotBeNullOrEmpty();
            Result.Description.Should().Be($"Occurs the 31th of every 2 months at 15:30:00");
        }
        [TestMethod]
        [ExpectedException(typeof(SchedulerException),
           @"If Monthly frecuency is selected you must set a monthly configuration")]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_And_Null_Month_Configuration_Should_Throw_Exception()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DailyConfiguration DailyConfiguration = new DailyConfiguration()
            {
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (TestDate, null, RecurringType.Recurring, SchedulerFrecuency.Monthly, 5, null, DailyConfiguration, null, DateTime.Today, null);
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);                        
        }
        [TestMethod]
        [ExpectedException(typeof(SchedulerException),
           @"You must set a positive month frecuency")]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_Negative_Should_Throw_Exception()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DailyConfiguration DailyConfiguration = new DailyConfiguration()
            {
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };
            MonthlyConfiguration MonthlyConfiguration = new MonthlyConfiguration()
            {
                DayFrecuency = false,
                MonthFrecuency = -7
            };
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (TestDate, null, RecurringType.Recurring, SchedulerFrecuency.Monthly, 5, null, DailyConfiguration, MonthlyConfiguration, DateTime.Today, null);
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);            
        }
        [TestMethod]
        [ExpectedException(typeof(SchedulerException),
           @"You must indicate a day if monthly day frecuency is selected")]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_Month_Day_Frecuency_With_Negative_Day_Should_Throw_Exception()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DailyConfiguration DailyConfiguration = new DailyConfiguration()
            {
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };
            MonthlyConfiguration MonthlyConfiguration = new MonthlyConfiguration()
            {
                DayFrecuency = true,
                DayOfMonth = -7
            };
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (TestDate, null, RecurringType.Recurring, SchedulerFrecuency.Monthly, 5, null, DailyConfiguration, MonthlyConfiguration, DateTime.Today, null);
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);            
        }
        [TestMethod]
        [ExpectedException(typeof(SchedulerException),
           @"You must indicate a day if monthly day frecuency is selected")]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_Month_Day_Frecuency_With_Null_Day_Should_Throw_Exception()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DailyConfiguration DailyConfiguration = new DailyConfiguration()
            {
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };
            MonthlyConfiguration MonthlyConfiguration = new MonthlyConfiguration()
            {
                DayFrecuency = true
            };
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (TestDate, null, RecurringType.Recurring, SchedulerFrecuency.Monthly, 5, null, DailyConfiguration, MonthlyConfiguration, DateTime.Today, null);
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);            
        }
        [TestMethod]
        [ExpectedException(typeof(SchedulerException),
           @"You must indicate a day if monthly day frecuency is selected")]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_Month_Day_Frecuency_With_Null_Day_Should_Throw_Exceptions()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DailyConfiguration DailyConfiguration = new DailyConfiguration()
            {
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };
            MonthlyConfiguration MonthlyConfiguration = new MonthlyConfiguration()
            {
                DayFrecuency = true
            };
            SchedulerConfiguration Configuration = new SchedulerConfiguration
                (TestDate, null, RecurringType.Recurring, SchedulerFrecuency.Monthly, 5, null, DailyConfiguration, MonthlyConfiguration, DateTime.Today, null);
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
        }
    }
}
