using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scheduler;
using System;
using Xunit;
using FluentAssertions;
using Scheduler.Exceptions;
using System.Text;
using System.Collections.Generic;
using UT = Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class DateCalculatorTests
    {
        [TestMethod]
        public void Recurring_Type_Once_Without_Configuration_Date_Should_Throw_Exception()
        {
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = DateTime.Today,
                Type = RecurringType.Once,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(5),
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };
            SchedulerException Ex = UT.Assert.ThrowsException<SchedulerException>(() => Calculator.GetNextExecutionDate(Configuration));
            Ex.Message.Should().Be("If 'Once' type is selected you must indicate a Configuration DateTime in order to start the process");
        }
        [TestMethod]
        public void Recurring_Type_Once_Configuration_Date_Max_Value_Should_Throw_Exception()
        {
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = DateTime.Today,
                ConfigurationDate = DateTime.MaxValue,
                StartDate = DateTime.Today,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };
            SchedulerException Ex = UT.Assert.ThrowsException<SchedulerException>(() => Calculator.GetNextExecutionDate(Configuration));
            Ex.Message.Should().Be("The configuration date can't be date min or max values");
        }
        [TestMethod]
        public void Start_Date_Greater_Than_End_Date_Should_Throw_Exception()
        {
            DateCalculator Calculator = new DateCalculator();

            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = DateTime.Today,
                Type = RecurringType.Once,
                SchedulerFrecuency = SchedulerFrecuency.Daily,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(-5),
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };
            SchedulerException Ex = UT.Assert.ThrowsException<SchedulerException>(() => Calculator.GetNextExecutionDate(Configuration));
            Ex.Message.Should().Be("Start date cannot be greater than end date");
        }
        [TestMethod]
        public void Current_Date_Max_Value_Should_Throw_Exception()
        {
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = DateTime.MaxValue,
                Type = RecurringType.Once,
                SchedulerFrecuency = SchedulerFrecuency.Daily,
                StartDate = DateTime.Today,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };
            SchedulerException Ex = UT.Assert.ThrowsException<SchedulerException>(() => Calculator.GetNextExecutionDate(Configuration));
            Ex.Message.Should().Be("The current date can't be date min or max values");
        }
        [TestMethod]
        public void Start_Date_Max_Value_Should_Throw_Exception()
        {
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = DateTime.Today,
                Type = RecurringType.Once,
                SchedulerFrecuency = SchedulerFrecuency.Daily,
                StartDate = DateTime.MaxValue,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };
            SchedulerException Ex = UT.Assert.ThrowsException<SchedulerException>(() => Calculator.GetNextExecutionDate(Configuration));
            Ex.Message.Should().Be("The start date can't be date min or max values");
        }
        [TestMethod]
        public void End_Date_Min_Value_Should_Throw_Exception()
        {
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = DateTime.Today,
                Type = RecurringType.Once,
                SchedulerFrecuency = SchedulerFrecuency.Daily,
                StartDate = DateTime.Today,
                EndDate = DateTime.MaxValue,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };
            SchedulerException Ex = UT.Assert.ThrowsException<SchedulerException>(() => Calculator.GetNextExecutionDate(Configuration));
            Ex.Message.Should().Be("The end date can't be date min or max values");
        }
        [TestMethod]
        public void Null_Daily_configuration_Should_Throw_exception()
        {
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = DateTime.Today,
                Type = RecurringType.Once,
                SchedulerFrecuency = SchedulerFrecuency.Daily,
                StartDate = DateTime.Today
            };
            SchedulerException Ex = UT.Assert.ThrowsException<SchedulerException>(() => Calculator.GetNextExecutionDate(Configuration));
            Ex.Message.Should().Be(@"You must set a Daily Configuration indicating if occurs once in the day (especifying the hour when it happens) or if it occurs multiple times (indicating how many hours, minutes or seconds between executions and the start and end time)");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Negative_Frecuency_Should_Throw_Exception()
        {
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = DateTime.Today,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Daily,                
                StartDate = DateTime.Today,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };
            SchedulerException Ex = UT.Assert.ThrowsException<SchedulerException>(() => Calculator.GetNextExecutionDate(Configuration));
            Ex.Message.Should().Be("If 'Recurring' type is selected you must indicate a frecuency");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Null_Frecuency_Should_Throw_Exception()
        {
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = DateTime.Today,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Daily,
                Frecuency = -5,
                StartDate = DateTime.Today,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0)
            };
            SchedulerException Ex = UT.Assert.ThrowsException<SchedulerException>(() => Calculator.GetNextExecutionDate(Configuration));
            Ex.Message.Should().Be("Frecuency can neither be negative nor exceed integer max or min values");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Daily_Configuration_Recurring_Without_Frecuencys_Should_Throw_Exception()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();

            DayOfWeek[] Days = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Friday };
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = DateTime.Today,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Daily,
                Frecuency = 5,
                StartDate = DateTime.Today,
                OccursOnceDaily = false
            };
            SchedulerException Ex = UT.Assert.ThrowsException<SchedulerException>(() => Calculator.GetNextExecutionDate(Configuration));
            Ex.Message.Should().Be("You must set a Daily Configuration indicating if occurs once in the day (especifying the hour when it happens) or if it occurs multiple times (indicating how many hours, minutes or seconds between executions and the start and end time)");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Daily_Configuration_Occurs_Once_Without_Hour_Should_Throw_Exception()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DayOfWeek[] Days = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Friday };
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = DateTime.Today,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Daily,
                Frecuency = 5,
                StartDate = DateTime.Today,
                OccursOnceDaily = true
            };
            SchedulerException Ex = UT.Assert.ThrowsException<SchedulerException>(() => Calculator.GetNextExecutionDate(Configuration));
            Ex.Message.Should().Be("You must set a Daily Configuration indicating if occurs once in the day (especifying the hour when it happens) or if it occurs multiple times (indicating how many hours, minutes or seconds between executions and the start and end time)");
        }
        [TestMethod]
        public void Recurring_Type_Once_With_Configuration_Date_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                ConfigurationDate = TestDate.AddDays(5),
                Type = RecurringType.Once,
                SchedulerFrecuency = SchedulerFrecuency.Daily,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = TestDate
            };
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
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Daily,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = TestDate
            };
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
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                MonthDayFrecuency = true,
                DayOfMonth = 18,
                MonthFrecuency = 5,
                StartDate = TestDate
            };
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
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Yearly,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = TestDate
            };
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = new DateTime(2026, 10, 18, 15, 30, 0);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().NotBeNullOrEmpty();
            Result.Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {ExpectedDateTime} starting on {Configuration.StartDate}"
               + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
        }
        [TestMethod]
        public void Recurring_Type_Recurring_Weekly_Without_Frecuency_Should_Throw_exception()
        {
            DateCalculator Calculator = new DateCalculator();
            DayOfWeek[] Days = new DayOfWeek[] { DayOfWeek.Monday };
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = DateTime.Today,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Weekly,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = DateTime.Today,
                WeekDays = Days
            };
            SchedulerException Ex = UT.Assert.ThrowsException<SchedulerException>(() => Calculator.GetNextExecutionDate(Configuration));
            Ex.Message.Should().Be("If weekly frecuency is selected you must set a week frecuency and select at least one day of the week");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_Weekly_With_Negative_Frecuency_Should_Throw_exception()
        {
            DateCalculator Calculator = new DateCalculator();
            DayOfWeek[] Days = new DayOfWeek[] { DayOfWeek.Monday };
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = DateTime.Today,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Weekly,
                Frecuency = -5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = DateTime.Today,
                WeekDays = Days,
            };
            SchedulerException Ex = UT.Assert.ThrowsException<SchedulerException>(() => Calculator.GetNextExecutionDate(Configuration));
            Ex.Message.Should().Be("Frecuency can neither be negative nor exceed integer max or min values");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_Weekly_With_Null_Days_Should_Throw_exception()
        {
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = DateTime.Today,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Weekly,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = DateTime.Today,
            };
            SchedulerException Ex = UT.Assert.ThrowsException<SchedulerException>(() => Calculator.GetNextExecutionDate(Configuration));
            Ex.Message.Should().Be("If weekly frecuency is selected you must set a week frecuency and select at least one day of the week");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Weekly_Frecuency_On_Mondays_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DayOfWeek[] Days = new DayOfWeek[] { DayOfWeek.Monday };
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Weekly,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = DateTime.Today,
                WeekDays = Days,
                WeekFrecuency = 5
            };
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = TestDate.AddDays(Configuration.WeekFrecuency.Value * 7).AddTicks(Configuration.DailyHour.Value.Ticks);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().NotBeNullOrEmpty();
            Result.Description.Should().Be("Occurs every 5 weeks on Monday at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Weekly_Frecuency_On_Fridays_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DayOfWeek[] Days = new DayOfWeek[] { DayOfWeek.Friday };
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Weekly,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = DateTime.Today,
                WeekDays = Days,
                WeekFrecuency = 5
            };
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = TestDate.AddDays(Configuration.WeekFrecuency.Value * 7 + 4).AddTicks(Configuration.DailyHour.Value.Ticks);
            Result.NextDate.Should().Be(TestDate.AddDays(Configuration.WeekFrecuency.Value * 7 + 4).AddTicks(Configuration.DailyHour.Value.Ticks));
            Result.Description.Should().NotBeNullOrEmpty();
            Result.Description.Should().Be("Occurs every 5 weeks on Friday at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Weekly_Frecuency_On_Mondays_And_Fridays_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DayOfWeek[] Days = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Friday };
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Weekly,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = DateTime.Today,
                WeekDays = Days,
                WeekFrecuency = 5
            };
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = TestDate.AddDays(Configuration.WeekFrecuency.Value * 7).AddTicks(Configuration.DailyHour.Value.Ticks);
            Result.NextDate.Should().Be(TestDate.AddDays(Configuration.WeekFrecuency.Value * 7).AddTicks(Configuration.DailyHour.Value.Ticks));
            Result.Description.Should().NotBeNullOrEmpty();
            Result.Description.Should().Be("Occurs every 5 weeks on Monday and Friday at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Weekly_Frecuency_On_Mondays_And_Fridays_With_Recurring_daily_Frecuency_Hourly_Frecuency_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DayOfWeek[] Days = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Friday };
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Weekly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(10, 30, 0),
                TimeFrecuency = TimeFrecuency.Hours,
                DailyFrecuency = 1,
                StartDate = DateTime.Today,
                WeekDays = Days,
                WeekFrecuency = 5
            };
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
            DayOfWeek[] Days = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Friday };
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Weekly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(10, 30, 0),
                TimeFrecuency = TimeFrecuency.Minutes,
                DailyFrecuency = 1,
                StartDate = DateTime.Today,
                WeekDays = Days,
                WeekFrecuency = 5
            };
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
            DayOfWeek[] Days = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Friday };
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Weekly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(10, 30, 0),
                TimeFrecuency = TimeFrecuency.Seconds,
                DailyFrecuency = 1,
                StartDate = DateTime.Today,
                WeekDays = Days,
                WeekFrecuency = 5
            };
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
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = DateTime.Today,
                MonthDayFrecuency = true,
                DayOfMonth = 10,
                MonthFrecuency = 2
            };
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
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = DateTime.Today,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.First,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Tuesday,
                MonthFrecuency = 2
            };
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
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = DateTime.Today,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Last,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Saturday,
                MonthFrecuency = 2
            };
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
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = DateTime.Today,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Second,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Weekday,
                MonthFrecuency = 2
            };
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
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = DateTime.Today,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Second,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Weekendday,
                MonthFrecuency = 2
            };
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
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = DateTime.Today,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Third,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Weekendday,
                MonthFrecuency = 2
            };
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = new DateTime(2021, 12, 11, 15, 30, 0);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().NotBeNullOrEmpty();
            Result.Description.Should().Be($"Occurs the Third Weekendday of every 2 months at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_Fourth_Friday_Every_2_Months_With_Daily_Frecuency_Once_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = DateTime.Today,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Fourth,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Friday,
                MonthFrecuency = 2
            };
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = new DateTime(2021, 12, 24, 15, 30, 0);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().NotBeNullOrEmpty();
            Result.Description.Should().Be($"Occurs the Fourth Friday of every 2 months at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_Last_Monday_Every_2_Months_With_Daily_Frecuency_Once_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = DateTime.Today,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Last,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Monday,
                MonthFrecuency = 2
            };
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = new DateTime(2021, 12, 27, 15, 30, 0);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().NotBeNullOrEmpty();
            Result.Description.Should().Be($"Occurs the Last Monday of every 2 months at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_First_Wednesday_Every_2_Months_With_Daily_Frecuency_Once_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = DateTime.Today,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.First,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Wednesday,
                MonthFrecuency = 2
            };
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = new DateTime(2021, 12, 1, 15, 30, 0);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().NotBeNullOrEmpty();
            Result.Description.Should().Be($"Occurs the First Wednesday of every 2 months at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_Second_Thursday_Every_2_Months_With_Daily_Frecuency_Once_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = DateTime.Today,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Second,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Thursday,
                MonthFrecuency = 2
            };
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = new DateTime(2021, 12, 9, 15, 30, 0);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().NotBeNullOrEmpty();
            Result.Description.Should().Be($"Occurs the Second Thursday of every 2 months at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_Third_Sunday_Every_2_Months_With_Daily_Frecuency_Once_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = DateTime.Today,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Third,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Sunday,
                MonthFrecuency = 2
            };
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = new DateTime(2021, 12, 19, 15, 30, 0);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().NotBeNullOrEmpty();
            Result.Description.Should().Be($"Occurs the Third Sunday of every 2 months at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_Last_Day_Every_2_Months_With_Daily_Frecuency_Once_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = DateTime.Today,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Last,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Day,
                MonthFrecuency = 2
            };
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = new DateTime(2021, 12, 31, 15, 30, 0);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().NotBeNullOrEmpty();
            Result.Description.Should().Be($"Occurs the Last Day of every 2 months at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_1st_Every_2_Months_With_Daily_Frecuency_Once_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 9, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = DateTime.Today,
                MonthDayFrecuency = true,
                DayOfMonth = 1,
                MonthFrecuency = 2
            };
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = new DateTime(2021, 11, 1, 15, 30, 0);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().NotBeNullOrEmpty();
            Result.Description.Should().Be($"Occurs the 1st of every 2 months at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_2nd_Every_2_Months_With_Daily_Frecuency_Once_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 9, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = DateTime.Today,
                MonthDayFrecuency = true,
                DayOfMonth = 2,
                MonthFrecuency = 2
            };
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = new DateTime(2021, 11, 2, 15, 30, 0);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().NotBeNullOrEmpty();
            Result.Description.Should().Be($"Occurs the 2nd of every 2 months at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_3rd_Every_2_Months_With_Daily_Frecuency_Once_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 9, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = DateTime.Today,
                MonthDayFrecuency = true,
                DayOfMonth = 3,
                MonthFrecuency = 2
            };
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = new DateTime(2021, 11, 3, 15, 30, 0);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().NotBeNullOrEmpty();
            Result.Description.Should().Be($"Occurs the 3rd of every 2 months at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_31th_Every_2_Months_With_Daily_Frecuency_Once_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 9, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = DateTime.Today,
                MonthDayFrecuency = true,
                DayOfMonth = 31,
                MonthFrecuency = 2
            };
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = new DateTime(2021, 11, 30, 15, 30, 0);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().NotBeNullOrEmpty();
            Result.Description.Should().Be($"Occurs the 31th of every 2 months at 15:30:00");
        }

        [ExpectedException(typeof(SchedulerException),
           @"If Monthly frecuency is selected you must set a monthly configuration")]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_And_Null_Month_Configuration_Should_Throw_Exception()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = DateTime.Today,
            };
            SchedulerException Ex = UT.Assert.ThrowsException<SchedulerException>(() => Calculator.GetNextExecutionDate(Configuration));
            Ex.Message.Should().Be("If weekly frecuency is selected you must set a week frecuency and select at least one day of the week");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_Negative_Should_Throw_Exception()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = DateTime.Today,
                MonthDayFrecuency = false,
                MonthFrecuency = -7
            };
            SchedulerException Ex = UT.Assert.ThrowsException<SchedulerException>(() => Calculator.GetNextExecutionDate(Configuration));
            Ex.Message.Should().Be("You must set a positive month frecuency");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_Month_Day_Frecuency_With_Negative_Day_Should_Throw_Exception()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = DateTime.Today,
                MonthDayFrecuency = true,
                DayOfMonth = -7
            };
            SchedulerException Ex = UT.Assert.ThrowsException<SchedulerException>(() => Calculator.GetNextExecutionDate(Configuration));
            Ex.Message.Should().Be("You must indicate a day if monthly day frecuency is selected");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_Month_Day_Frecuency_With_Null_Day_Should_Throw_Exception()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = DateTime.Today,
                MonthDayFrecuency = true
            };
            SchedulerException Ex = UT.Assert.ThrowsException<SchedulerException>(() => Calculator.GetNextExecutionDate(Configuration));
            Ex.Message.Should().Be("You must indicate a day if monthly day frecuency is selected");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_Month_Day_Frecuency_With_Null_Day_Should_Throw_Exceptions()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = DateTime.Today,
                MonthDayFrecuency = true
            };
            SchedulerException Ex = UT.Assert.ThrowsException<SchedulerException>(() => Calculator.GetNextExecutionDate(Configuration));
            Ex.Message.Should().Be("You must indicate a day if monthly day frecuency is selected");
        }
    }
}
