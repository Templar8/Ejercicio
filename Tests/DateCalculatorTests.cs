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
        #region Common Validations Tests

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
            Configuration.Invoking(e => Calculator.GetNextExecutionDate(Configuration)).Should().Throw<SchedulerException>()
                .WithMessage("The current date can't be date min or max values");            
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
            Configuration.Invoking(e => Calculator.GetNextExecutionDate(Configuration)).Should().Throw<SchedulerException>()
                .WithMessage("The start date can't be date min or max values");            
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
            Configuration.Invoking(e => Calculator.GetNextExecutionDate(Configuration)).Should().Throw<SchedulerException>()
                .WithMessage("Start date cannot be greater than end date");            
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
            Configuration.Invoking(e => Calculator.GetNextExecutionDate(Configuration)).Should().Throw<SchedulerException>()
                .WithMessage("The end date can't be date min or max values");
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
            Configuration.Invoking(e => Calculator.GetNextExecutionDate(Configuration)).Should().Throw<SchedulerException>()
                .WithMessage(@"You must set a Daily Configuration indicating if occurs once in the day (especifying the hour when it happens) or if it occurs multiple times (indicating how many hours, minutes or seconds between executions and the start and end time)");
        }

        #endregion

        #region Recurring Once Tests

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
            Configuration.Invoking(e => Calculator.GetNextExecutionDate(Configuration)).Should().Throw<SchedulerException>()
                .WithMessage("If 'Once' type is selected you must indicate a Configuration DateTime in order to start the process");
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
            Configuration.Invoking(e => Calculator.GetNextExecutionDate(Configuration)).Should().Throw<SchedulerException>()
                .WithMessage("The configuration date can't be date min or max values");
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
            Result.Description.Should().Be($"Occurs once. Schedule will be used on {ExpectedDateTime} starting on {Configuration.StartDate}"
                    + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
        }
        [TestMethod]
        public void Recurring_Type_Once_With_Configuration_Date_And_End_Date_Should_Return_Object()
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
                StartDate = TestDate,
                EndDate = TestDate.AddDays(6)
            };
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = new DateTime(2021, 10, 23, 15, 30, 0);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().Be($"Occurs once. Schedule will be used on {ExpectedDateTime} starting on {Configuration.StartDate}"
                    + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
        }
        [TestMethod]
        public void Recurring_Type_Once_With_Configuration_Date_And_End_Date_Lesser_Than_Result_Should_Return_Null()
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
                StartDate = TestDate,
                EndDate = TestDate.AddDays(2)
            };
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);            
            Result.Should().BeNull();            
        }

        #endregion

        #region Daily Configuration

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
            Configuration.Invoking(e => Calculator.GetNextExecutionDate(Configuration)).Should().Throw<SchedulerException>()
                .WithMessage("Frecuency can neither be negative nor exceed integer max or min values");
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
            Configuration.Invoking(e => Calculator.GetNextExecutionDate(Configuration)).Should().Throw<SchedulerException>()
                .WithMessage("You must set a Daily Configuration indicating if occurs once in the day (especifying the hour when it happens) or if it occurs multiple times (indicating how many hours, minutes or seconds between executions and the start and end time)");
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
            Configuration.Invoking(e => Calculator.GetNextExecutionDate(Configuration)).Should().Throw<SchedulerException>()
                .WithMessage("You must set a Daily Configuration indicating if occurs once in the day (especifying the hour when it happens) or if it occurs multiple times (indicating how many hours, minutes or seconds between executions and the start and end time)");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Daily_Frecuency_Without_End_Date_Should_Return_Object()
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
            Result.Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {ExpectedDateTime} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Daily_Frecuency_With_End_Date_Should_Return_Object()
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
                StartDate = TestDate,
                EndDate = TestDate.AddDays(15)
            };
            DateResult Result = Calculator.GetNextExecutionDate(Configuration);
            DateTime ExpectedDateTime = new DateTime(2021, 10, 23, 15, 30, 0);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {ExpectedDateTime} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Daily_Frecuency_Hourly_With_End_Date_Repeated_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Daily,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(10, 30, 0),
                TimeFrecuency = TimeFrecuency.Hours,
                DailyFrecuency = 1,
                StartDate = TestDate,
                EndDate = TestDate.AddDays(6)
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 6);
            DateTime ExpectedDateTime = new DateTime(2021, 10, 23, 6, 30, 0);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 0));
            Result[0].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 23, 7, 30, 0));
            Result[1].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 7, 30, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 23, 8, 30, 0));
            Result[2].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 8, 30, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[3].NextDate.Should().Be(new DateTime(2021, 10, 23, 9, 30, 0));
            Result[3].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 9, 30, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[4].NextDate.Should().Be(new DateTime(2021, 10, 23, 10, 30, 0));
            Result[4].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 10, 30, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[5].Should().BeNull();
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Daily_Frecuency_Hourly_Without_End_Date_Repeated_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Daily,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(10, 30, 0),
                TimeFrecuency = TimeFrecuency.Hours,
                DailyFrecuency = 1,
                StartDate = TestDate
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);
            DateTime ExpectedDateTime = new DateTime(2021, 10, 23, 6, 30, 0);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 0));
            Result[0].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 23, 7, 30, 0));
            Result[1].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 7, 30, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 23, 8, 30, 0));
            Result[2].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 8, 30, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[3].NextDate.Should().Be(new DateTime(2021, 10, 23, 9, 30, 0));
            Result[3].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 9, 30, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[4].NextDate.Should().Be(new DateTime(2021, 10, 23, 10, 30, 0));
            Result[4].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 10, 30, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[5].NextDate.Should().Be(new DateTime(2021, 10, 28, 6, 30, 0));
            Result[5].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 28, 6, 30, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[6].NextDate.Should().Be(new DateTime(2021, 10, 28, 7, 30, 0));
            Result[6].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 28, 7, 30, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Daily_Frecuency_Minutes_With_End_Date_Repeated_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Daily,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 50, 0),
                DailyEndHour = new TimeSpan(7, 0, 0),
                TimeFrecuency = TimeFrecuency.Minutes,
                DailyFrecuency = 1,
                StartDate = TestDate,
                EndDate = TestDate.AddDays(6)
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 12);
            DateTime ExpectedDateTime = new DateTime(2021, 10, 23, 6, 50, 0);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 50, 0));
            Result[0].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 50, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 51, 0));
            Result[1].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 51, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 52, 0));
            Result[2].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 52, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[3].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 53, 0));
            Result[3].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 53, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[4].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 54, 0));
            Result[4].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 54, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[5].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 55, 0));
            Result[5].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 55, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[6].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 56, 0));
            Result[6].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 56, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[7].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 57, 0));
            Result[7].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 57, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[8].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 58, 0));
            Result[8].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 58, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[9].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 59, 0));
            Result[9].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 59, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[10].NextDate.Should().Be(new DateTime(2021, 10, 23, 7, 0, 0));
            Result[10].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 7, 0, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[11].Should().BeNull();
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Daily_Frecuency_Minutes_Without_End_Date_Repeated_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Daily,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 50, 0),
                DailyEndHour = new TimeSpan(7, 0, 0),
                TimeFrecuency = TimeFrecuency.Minutes,
                DailyFrecuency = 1,
                StartDate = TestDate
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 12);
            DateTime ExpectedDateTime = new DateTime(2021, 10, 23, 6, 50, 0);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 50, 0));
            Result[0].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 50, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 51, 0));
            Result[1].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 51, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 52, 0));
            Result[2].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 52, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[3].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 53, 0));
            Result[3].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 53, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[4].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 54, 0));
            Result[4].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 54, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[5].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 55, 0));
            Result[5].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 55, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[6].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 56, 0));
            Result[6].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 56, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[7].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 57, 0));
            Result[7].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 57, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[8].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 58, 0));
            Result[8].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 58, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[9].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 59, 0));
            Result[9].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 59, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[10].NextDate.Should().Be(new DateTime(2021, 10, 23, 7, 0, 0));
            Result[10].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 7, 0, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[11].NextDate.Should().Be(new DateTime(2021, 10, 28, 6, 50, 0));
            Result[11].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 28, 6, 50, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Daily_Frecuency_Seconds_With_End_Date_Repeated_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Daily,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 50),
                DailyEndHour = new TimeSpan(6, 31, 0),
                TimeFrecuency = TimeFrecuency.Seconds,
                DailyFrecuency = 1,
                StartDate = TestDate,
                EndDate = TestDate.AddDays(6)
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 12);
            DateTime ExpectedDateTime = new DateTime(2021, 10, 23, 6, 50, 50);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 50));
            Result[0].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 50)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 51));
            Result[1].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 51)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 52));
            Result[2].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 52)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[3].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 53));
            Result[3].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 53)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[4].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 54));
            Result[4].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 54)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[5].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 55));
            Result[5].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 55)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[6].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 56));
            Result[6].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 56)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[7].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 57));
            Result[7].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 57)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[8].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 58));
            Result[8].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 58)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[9].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 59));
            Result[9].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 59)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[10].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 31, 0));
            Result[10].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 31, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[11].Should().BeNull();
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Daily_Frecuency_Without_End_Date_Repeated_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Daily,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 50),
                DailyEndHour = new TimeSpan(6, 31, 0),
                TimeFrecuency = TimeFrecuency.Seconds,
                DailyFrecuency = 1,
                StartDate = TestDate                
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 12);
            DateTime ExpectedDateTime = new DateTime(2021, 10, 23, 6, 50, 50);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 50));
            Result[0].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 50)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 51));
            Result[1].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 51)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 52));
            Result[2].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 52)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[3].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 53));
            Result[3].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 53)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[4].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 54));
            Result[4].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 54)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[5].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 55));
            Result[5].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 55)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[6].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 56));
            Result[6].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 56)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[7].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 57));
            Result[7].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 57)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[8].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 58));
            Result[8].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 58)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[9].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 59));
            Result[9].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 59)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[10].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 31, 0));
            Result[10].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 31, 0)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[11].NextDate.Should().Be(new DateTime(2021, 10, 28, 6, 30, 50));
            Result[11].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 28, 6, 30, 50)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
        }


        #endregion

        #region Yearly

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
        public void Recurring_Type_Recurring_With_Yearly_Frecuency_Repeated_Hours_With_End_Date_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Yearly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(8, 30, 30),
                DailyEndHour = new TimeSpan(10, 30, 30),
                DailyFrecuency = 1,
                TimeFrecuency = TimeFrecuency.Hours,
                StartDate = TestDate,
                EndDate = TestDate.AddYears(10)
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 5);

            Result[0].NextDate.Should().Be(new DateTime(2026, 10, 18, 8, 30, 30));
            Result[0].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2026, 10, 18, 8, 30, 30)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[1].NextDate.Should().Be(new DateTime(2026, 10, 18, 9, 30, 30));
            Result[1].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2026, 10, 18, 9, 30, 30)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[2].NextDate.Should().Be(new DateTime(2026, 10, 18, 10, 30, 30));
            Result[2].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2026, 10, 18, 10, 30, 30)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[3].Should().BeNull();
            Result[4].Should().BeNull();
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Yearly_Frecuency_Repeated_Hours_Without_End_Date_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Yearly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(8, 30, 30),
                DailyEndHour = new TimeSpan(10, 30, 30),
                DailyFrecuency = 1,
                TimeFrecuency = TimeFrecuency.Hours,
                StartDate = TestDate
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 5);

            Result[0].NextDate.Should().Be(new DateTime(2026, 10, 18, 8, 30, 30));
            Result[0].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2026, 10, 18, 8, 30, 30)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[1].NextDate.Should().Be(new DateTime(2026, 10, 18, 9, 30, 30));
            Result[1].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2026, 10, 18, 9, 30, 30)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[2].NextDate.Should().Be(new DateTime(2026, 10, 18, 10, 30, 30));
            Result[2].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2026, 10, 18, 10, 30, 30)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[3].NextDate.Should().Be(new DateTime(2031, 10, 18, 8, 30, 30));
            Result[3].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2031, 10, 18, 8, 30, 30)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[4].NextDate.Should().Be(new DateTime(2031, 10, 18, 9, 30, 30));
            Result[4].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2031, 10, 18, 9, 30, 30)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Yearly_Frecuency_Repeated_Minutes_With_End_Date_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Yearly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(8, 30, 30),
                DailyEndHour = new TimeSpan(8, 32, 30),
                DailyFrecuency = 1,
                TimeFrecuency = TimeFrecuency.Minutes,
                StartDate = TestDate,
                EndDate = TestDate.AddYears(10)
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 5);

            Result[0].NextDate.Should().Be(new DateTime(2026, 10, 18, 8, 30, 30));
            Result[0].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2026, 10, 18, 8, 30, 30)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[1].NextDate.Should().Be(new DateTime(2026, 10, 18, 8, 31, 30));
            Result[1].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2026, 10, 18, 8, 31, 30)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[2].NextDate.Should().Be(new DateTime(2026, 10, 18, 8, 32, 30));
            Result[2].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2026, 10, 18, 8, 32, 30)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[3].Should().BeNull();            
            Result[4].Should().BeNull();
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Yearly_Frecuency_Repeated_Minutes_Without_End_Date_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Yearly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(8, 30, 30),
                DailyEndHour = new TimeSpan(8, 32, 30),
                DailyFrecuency = 1,
                TimeFrecuency = TimeFrecuency.Minutes,
                StartDate = TestDate
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 5);

            Result[0].NextDate.Should().Be(new DateTime(2026, 10, 18, 8, 30, 30));
            Result[0].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2026, 10, 18, 8, 30, 30)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[1].NextDate.Should().Be(new DateTime(2026, 10, 18, 8, 31, 30));
            Result[1].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2026, 10, 18, 8, 31, 30)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[2].NextDate.Should().Be(new DateTime(2026, 10, 18, 8, 32, 30));
            Result[2].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2026, 10, 18, 8, 32, 30)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[3].NextDate.Should().Be(new DateTime(2031, 10, 18, 8, 30, 30));
            Result[3].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2031, 10, 18, 8, 30, 30)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[4].NextDate.Should().Be(new DateTime(2031, 10, 18, 8, 31, 30));
            Result[4].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2031, 10, 18, 8, 31, 30)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Yearly_Frecuency_Repeated_Seconds_With_End_Date_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Yearly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(8, 30, 30),
                DailyEndHour = new TimeSpan(8, 30, 32),
                DailyFrecuency = 1,
                TimeFrecuency = TimeFrecuency.Seconds,
                StartDate = TestDate,
                EndDate = TestDate.AddYears(10)
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 5);

            Result[0].NextDate.Should().Be(new DateTime(2026, 10, 18, 8, 30, 30));
            Result[0].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2026, 10, 18, 8, 30, 30)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[1].NextDate.Should().Be(new DateTime(2026, 10, 18, 8, 30, 31));
            Result[1].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2026, 10, 18, 8, 30, 31)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[2].NextDate.Should().Be(new DateTime(2026, 10, 18, 8, 30, 32));
            Result[2].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2026, 10, 18, 8, 30, 32)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[3].Should().BeNull();
            Result[4].Should().BeNull();
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Yearly_Frecuency_Repeated_Seconds_Without_End_Date_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Yearly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(8, 30, 30),
                DailyEndHour = new TimeSpan(8, 30, 32),
                DailyFrecuency = 1,
                TimeFrecuency = TimeFrecuency.Seconds,
                StartDate = TestDate
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 5);

            Result[0].NextDate.Should().Be(new DateTime(2026, 10, 18, 8, 30, 30));
            Result[0].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2026, 10, 18, 8, 30, 30)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[1].NextDate.Should().Be(new DateTime(2026, 10, 18, 8, 30, 31));
            Result[1].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2026, 10, 18, 8, 30, 31)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[2].NextDate.Should().Be(new DateTime(2026, 10, 18, 8, 30, 32));
            Result[2].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2026, 10, 18, 8, 30, 32)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[3].NextDate.Should().Be(new DateTime(2031, 10, 18, 8, 30, 30));
            Result[3].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2031, 10, 18, 8, 30, 30)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
            Result[4].NextDate.Should().Be(new DateTime(2031, 10, 18, 8, 30, 31));
            Result[4].Description.Should().Be($"Occurs {Configuration.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2031, 10, 18, 8, 30, 31)} starting on {Configuration.StartDate}"
                + (Configuration.EndDate.HasValue ? $" and ending on {Configuration.EndDate}" : string.Empty));
        }

        #endregion

        #region Weekly Configuration
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
            Configuration.Invoking(e => Calculator.GetNextExecutionDate(Configuration)).Should().Throw<SchedulerException>()
                .WithMessage("If weekly frecuency is selected you must set a week frecuency and select at least one day of the week");            
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
            Configuration.Invoking(e => Calculator.GetNextExecutionDate(Configuration)).Should().Throw<SchedulerException>()
                .WithMessage("Frecuency can neither be negative nor exceed integer max or min values");            
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
            Configuration.Invoking(e => Calculator.GetNextExecutionDate(Configuration)).Should().Throw<SchedulerException>()
                .WithMessage("If weekly frecuency is selected you must set a week frecuency and select at least one day of the week");            
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Weekly_Frecuency_On_Mondays_Once_Daily_Without_End_Date_Should_Return_Object()
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

            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 5);

            Result[0].NextDate.Should().Be(new DateTime(2021, 11, 22, 15, 30, 0));
            Result[0].Description.Should().Be("Occurs every 5 weeks on Monday at 15:30:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 12, 27, 15, 30, 0));
            Result[1].Description.Should().Be("Occurs every 5 weeks on Monday at 15:30:00");
            Result[2].NextDate.Should().Be(new DateTime(2022, 1, 31, 15, 30, 0));
            Result[2].Description.Should().Be("Occurs every 5 weeks on Monday at 15:30:00");
            Result[3].NextDate.Should().Be(new DateTime(2022, 3, 7, 15, 30, 0));
            Result[3].Description.Should().Be("Occurs every 5 weeks on Monday at 15:30:00");
            Result[4].NextDate.Should().Be(new DateTime(2022, 4, 11, 15, 30, 0));
            Result[4].Description.Should().Be("Occurs every 5 weeks on Monday at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Weekly_Frecuency_On_Mondays_Once_Daily_With_End_Date_Should_Return_Object()
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
                EndDate = new DateTime(2022,2,1),
                WeekDays = Days,
                WeekFrecuency = 5
            };

            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 5);

            Result[0].NextDate.Should().Be(new DateTime(2021, 11, 22, 15, 30, 0));
            Result[0].Description.Should().Be("Occurs every 5 weeks on Monday at 15:30:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 12, 27, 15, 30, 0));
            Result[1].Description.Should().Be("Occurs every 5 weeks on Monday at 15:30:00");
            Result[2].NextDate.Should().Be(new DateTime(2022, 1, 31, 15, 30, 0));
            Result[2].Description.Should().Be("Occurs every 5 weeks on Monday at 15:30:00");
            Result[3].Should().BeNull();
            Result[4].Should().BeNull();
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Weekly_Frecuency_On_Mondays_Repeated_Hourly_Daily_Should_Return_Object()
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
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(8, 30, 0),
                TimeFrecuency = TimeFrecuency.Hours,
                DailyFrecuency = 1,
                StartDate = TestDate,
                WeekDays = Days,
                WeekFrecuency = 5
            };

            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 10);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs every 5 weeks on Monday every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 7, 30, 0));
            Result[1].Description.Should().Be("Occurs every 5 weeks on Monday every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 0));
            Result[2].Description.Should().Be("Occurs every 5 weeks on Monday every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 11, 22, 6, 30, 0));
            Result[3].Description.Should().Be("Occurs every 5 weeks on Monday every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 11, 22, 7, 30, 0));
            Result[4].Description.Should().Be("Occurs every 5 weeks on Monday every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 11, 22, 8, 30, 0));
            Result[5].Description.Should().Be("Occurs every 5 weeks on Monday every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2021, 12, 27, 6, 30, 0));
            Result[6].Description.Should().Be("Occurs every 5 weeks on Monday every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[7].NextDate.Should().Be(new DateTime(2021, 12, 27, 7, 30, 0));
            Result[7].Description.Should().Be("Occurs every 5 weeks on Monday every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[8].NextDate.Should().Be(new DateTime(2021, 12, 27, 8, 30, 0));
            Result[8].Description.Should().Be("Occurs every 5 weeks on Monday every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[9].NextDate.Should().Be(new DateTime(2022, 1, 31, 6, 30, 0));
            Result[9].Description.Should().Be("Occurs every 5 weeks on Monday every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Weekly_Frecuency_On_Tuesdays_Repeated_Minutes_Daily_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DayOfWeek[] Days = new DayOfWeek[] { DayOfWeek.Tuesday };
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Weekly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(6, 32, 0),
                TimeFrecuency = TimeFrecuency.Minutes,
                DailyFrecuency = 1,
                StartDate = TestDate,
                WeekDays = Days,
                WeekFrecuency = 5
            };

            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 10);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 19, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs every 5 weeks on Tuesday every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 19, 6, 31, 0));
            Result[1].Description.Should().Be("Occurs every 5 weeks on Tuesday every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 19, 6, 32, 0));
            Result[2].Description.Should().Be("Occurs every 5 weeks on Tuesday every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 11, 23, 6, 30, 0));
            Result[3].Description.Should().Be("Occurs every 5 weeks on Tuesday every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 11, 23, 6, 31, 0));
            Result[4].Description.Should().Be("Occurs every 5 weeks on Tuesday every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 11, 23, 6, 32, 0));
            Result[5].Description.Should().Be("Occurs every 5 weeks on Tuesday every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2021, 12, 28, 6, 30, 0));
            Result[6].Description.Should().Be("Occurs every 5 weeks on Tuesday every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[7].NextDate.Should().Be(new DateTime(2021, 12, 28, 6, 31, 0));
            Result[7].Description.Should().Be("Occurs every 5 weeks on Tuesday every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[8].NextDate.Should().Be(new DateTime(2021, 12, 28, 6, 32, 0));
            Result[8].Description.Should().Be("Occurs every 5 weeks on Tuesday every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[9].NextDate.Should().Be(new DateTime(2022, 2, 1, 6, 30, 0));
            Result[9].Description.Should().Be("Occurs every 5 weeks on Tuesday every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Weekly_Frecuency_On_Wednesdays_Repeated_Seconds_Daily_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DayOfWeek[] Days = new DayOfWeek[] { DayOfWeek.Wednesday };
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Weekly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 58),
                DailyEndHour = new TimeSpan(6, 31, 0),
                TimeFrecuency = TimeFrecuency.Seconds,
                DailyFrecuency = 1,
                StartDate = TestDate,
                WeekDays = Days,
                WeekFrecuency = 5
            };

            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 10);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 20, 6, 30, 58));
            Result[0].Description.Should().Be("Occurs every 5 weeks on Wednesday every 1 Seconds between 06:30:58 and 06:31:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 20, 6, 30, 59));
            Result[1].Description.Should().Be("Occurs every 5 weeks on Wednesday every 1 Seconds between 06:30:58 and 06:31:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 20, 6, 31, 0));
            Result[2].Description.Should().Be("Occurs every 5 weeks on Wednesday every 1 Seconds between 06:30:58 and 06:31:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 11, 24, 6, 30, 58));
            Result[3].Description.Should().Be("Occurs every 5 weeks on Wednesday every 1 Seconds between 06:30:58 and 06:31:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 11, 24, 6, 30, 59));
            Result[4].Description.Should().Be("Occurs every 5 weeks on Wednesday every 1 Seconds between 06:30:58 and 06:31:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 11, 24, 6, 31, 0));
            Result[5].Description.Should().Be("Occurs every 5 weeks on Wednesday every 1 Seconds between 06:30:58 and 06:31:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2021, 12, 29, 6, 30, 58));
            Result[6].Description.Should().Be("Occurs every 5 weeks on Wednesday every 1 Seconds between 06:30:58 and 06:31:00 starting on 18/10/2021 0:00:00");
            Result[7].NextDate.Should().Be(new DateTime(2021, 12, 29, 6, 30, 59));
            Result[7].Description.Should().Be("Occurs every 5 weeks on Wednesday every 1 Seconds between 06:30:58 and 06:31:00 starting on 18/10/2021 0:00:00");
            Result[8].NextDate.Should().Be(new DateTime(2021, 12, 29, 6, 31, 0));
            Result[8].Description.Should().Be("Occurs every 5 weeks on Wednesday every 1 Seconds between 06:30:58 and 06:31:00 starting on 18/10/2021 0:00:00");
            Result[9].NextDate.Should().Be(new DateTime(2022, 2, 2, 6, 30, 58));
            Result[9].Description.Should().Be("Occurs every 5 weeks on Wednesday every 1 Seconds between 06:30:58 and 06:31:00 starting on 18/10/2021 0:00:00");
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
        public void Recurring_Type_Recurring_With_Weekly_Frecuency_On_Mondays_And_Fridays_With_Recurring_Daily_Frecuency_Hourly_Frecuency_Should_Return_Object()
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
                DailyEndHour = new TimeSpan(8, 30, 0),
                TimeFrecuency = TimeFrecuency.Hours,
                DailyFrecuency = 1,
                StartDate = TestDate,
                WeekDays = Days,
                WeekFrecuency = 5
            };

            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 15);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs every 5 weeks on Monday and Friday every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 7, 30, 0));
            Result[1].Description.Should().Be("Occurs every 5 weeks on Monday and Friday every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 0));
            Result[2].Description.Should().Be("Occurs every 5 weeks on Monday and Friday every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 10, 22, 6, 30, 0));
            Result[3].Description.Should().Be("Occurs every 5 weeks on Monday and Friday every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 10, 22, 7, 30, 0));
            Result[4].Description.Should().Be("Occurs every 5 weeks on Monday and Friday every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 10, 22, 8, 30, 0));
            Result[5].Description.Should().Be("Occurs every 5 weeks on Monday and Friday every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2021, 11, 22, 6, 30, 0));
            Result[6].Description.Should().Be("Occurs every 5 weeks on Monday and Friday every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[7].NextDate.Should().Be(new DateTime(2021, 11, 22, 7, 30, 0));
            Result[7].Description.Should().Be("Occurs every 5 weeks on Monday and Friday every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[8].NextDate.Should().Be(new DateTime(2021, 11, 22, 8, 30, 0));
            Result[8].Description.Should().Be("Occurs every 5 weeks on Monday and Friday every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[9].NextDate.Should().Be(new DateTime(2021, 11, 26, 6, 30, 0));
            Result[9].Description.Should().Be("Occurs every 5 weeks on Monday and Friday every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[10].NextDate.Should().Be(new DateTime(2021, 11, 26, 7, 30, 0));
            Result[10].Description.Should().Be("Occurs every 5 weeks on Monday and Friday every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[11].NextDate.Should().Be(new DateTime(2021, 11, 26, 8, 30, 0));
            Result[11].Description.Should().Be("Occurs every 5 weeks on Monday and Friday every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[12].NextDate.Should().Be(new DateTime(2021, 12, 27, 6, 30, 0));
            Result[12].Description.Should().Be("Occurs every 5 weeks on Monday and Friday every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[13].NextDate.Should().Be(new DateTime(2021, 12, 27, 7, 30, 0));
            Result[13].Description.Should().Be("Occurs every 5 weeks on Monday and Friday every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[14].NextDate.Should().Be(new DateTime(2021, 12, 27, 8, 30, 0));
            Result[14].Description.Should().Be("Occurs every 5 weeks on Monday and Friday every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Weekly_Frecuency_On_Tuesdays_And_Thursdays_With_Recurring_Daily_Frecuency_Minute_Frecuency_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DayOfWeek[] Days = new DayOfWeek[] { DayOfWeek.Tuesday, DayOfWeek.Thursday };
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Weekly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(6, 32, 0),
                TimeFrecuency = TimeFrecuency.Minutes,
                DailyFrecuency = 1,
                StartDate = TestDate,
                WeekDays = Days,
                WeekFrecuency = 5
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 15);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 19, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs every 5 weeks on Tuesday and Thursday every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 19, 6, 31, 0));
            Result[1].Description.Should().Be("Occurs every 5 weeks on Tuesday and Thursday every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 19, 6, 32, 0));
            Result[2].Description.Should().Be("Occurs every 5 weeks on Tuesday and Thursday every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 10, 21, 6, 30, 0));
            Result[3].Description.Should().Be("Occurs every 5 weeks on Tuesday and Thursday every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 10, 21, 6, 31, 0));
            Result[4].Description.Should().Be("Occurs every 5 weeks on Tuesday and Thursday every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 10, 21, 6, 32, 0));
            Result[5].Description.Should().Be("Occurs every 5 weeks on Tuesday and Thursday every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2021, 11, 23, 6, 30, 0));
            Result[6].Description.Should().Be("Occurs every 5 weeks on Tuesday and Thursday every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[7].NextDate.Should().Be(new DateTime(2021, 11, 23, 6, 31, 0));
            Result[7].Description.Should().Be("Occurs every 5 weeks on Tuesday and Thursday every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[8].NextDate.Should().Be(new DateTime(2021, 11, 23, 6, 32, 0));
            Result[8].Description.Should().Be("Occurs every 5 weeks on Tuesday and Thursday every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[9].NextDate.Should().Be(new DateTime(2021, 11, 25, 6, 30, 0));
            Result[9].Description.Should().Be("Occurs every 5 weeks on Tuesday and Thursday every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[10].NextDate.Should().Be(new DateTime(2021, 11, 25, 6, 31, 0));
            Result[10].Description.Should().Be("Occurs every 5 weeks on Tuesday and Thursday every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[11].NextDate.Should().Be(new DateTime(2021, 11, 25, 6, 32, 0));
            Result[11].Description.Should().Be("Occurs every 5 weeks on Tuesday and Thursday every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[12].NextDate.Should().Be(new DateTime(2021, 12, 28, 6, 30, 0));
            Result[12].Description.Should().Be("Occurs every 5 weeks on Tuesday and Thursday every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[13].NextDate.Should().Be(new DateTime(2021, 12, 28, 6, 31, 0));
            Result[13].Description.Should().Be("Occurs every 5 weeks on Tuesday and Thursday every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[14].NextDate.Should().Be(new DateTime(2021, 12, 28, 6, 32, 0));
            Result[14].Description.Should().Be("Occurs every 5 weeks on Tuesday and Thursday every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Weekly_Frecuency_On_Wednesdays_And_Saturdays_With_Recurring_daily_Frecuency_Second_Frecuency_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DayOfWeek[] Days = new DayOfWeek[] { DayOfWeek.Wednesday, DayOfWeek.Saturday };
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Weekly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 58),
                DailyEndHour = new TimeSpan(6, 31, 0),
                TimeFrecuency = TimeFrecuency.Seconds,
                DailyFrecuency = 1,
                StartDate = TestDate,
                WeekDays = Days,
                WeekFrecuency = 5
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 15);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 20, 6, 30, 58));
            Result[0].Description.Should().Be("Occurs every 5 weeks on Wednesday and Saturday every 1 Seconds between 06:30:58 and 06:31:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 20, 6, 30, 59));
            Result[1].Description.Should().Be("Occurs every 5 weeks on Wednesday and Saturday every 1 Seconds between 06:30:58 and 06:31:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 20, 6, 31, 0));
            Result[2].Description.Should().Be("Occurs every 5 weeks on Wednesday and Saturday every 1 Seconds between 06:30:58 and 06:31:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 58));
            Result[3].Description.Should().Be("Occurs every 5 weeks on Wednesday and Saturday every 1 Seconds between 06:30:58 and 06:31:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 59));
            Result[4].Description.Should().Be("Occurs every 5 weeks on Wednesday and Saturday every 1 Seconds between 06:30:58 and 06:31:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 31, 0));
            Result[5].Description.Should().Be("Occurs every 5 weeks on Wednesday and Saturday every 1 Seconds between 06:30:58 and 06:31:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2021, 11, 24, 6, 30, 58));
            Result[6].Description.Should().Be("Occurs every 5 weeks on Wednesday and Saturday every 1 Seconds between 06:30:58 and 06:31:00 starting on 18/10/2021 0:00:00");
            Result[7].NextDate.Should().Be(new DateTime(2021, 11, 24, 6, 30, 59));
            Result[7].Description.Should().Be("Occurs every 5 weeks on Wednesday and Saturday every 1 Seconds between 06:30:58 and 06:31:00 starting on 18/10/2021 0:00:00");
            Result[8].NextDate.Should().Be(new DateTime(2021, 11, 24, 6, 31, 0));
            Result[8].Description.Should().Be("Occurs every 5 weeks on Wednesday and Saturday every 1 Seconds between 06:30:58 and 06:31:00 starting on 18/10/2021 0:00:00");
            Result[9].NextDate.Should().Be(new DateTime(2021, 11, 27, 6, 30, 58));
            Result[9].Description.Should().Be("Occurs every 5 weeks on Wednesday and Saturday every 1 Seconds between 06:30:58 and 06:31:00 starting on 18/10/2021 0:00:00");
            Result[10].NextDate.Should().Be(new DateTime(2021, 11, 27, 6, 30, 59));
            Result[10].Description.Should().Be("Occurs every 5 weeks on Wednesday and Saturday every 1 Seconds between 06:30:58 and 06:31:00 starting on 18/10/2021 0:00:00");
            Result[11].NextDate.Should().Be(new DateTime(2021, 11, 27, 6, 31, 0));
            Result[11].Description.Should().Be("Occurs every 5 weeks on Wednesday and Saturday every 1 Seconds between 06:30:58 and 06:31:00 starting on 18/10/2021 0:00:00");
            Result[12].NextDate.Should().Be(new DateTime(2021, 12, 29, 6, 30, 58));
            Result[12].Description.Should().Be("Occurs every 5 weeks on Wednesday and Saturday every 1 Seconds between 06:30:58 and 06:31:00 starting on 18/10/2021 0:00:00");
            Result[13].NextDate.Should().Be(new DateTime(2021, 12, 29, 6, 30, 59));
            Result[13].Description.Should().Be("Occurs every 5 weeks on Wednesday and Saturday every 1 Seconds between 06:30:58 and 06:31:00 starting on 18/10/2021 0:00:00");
            Result[14].NextDate.Should().Be(new DateTime(2021, 12, 29, 6, 31, 0));
            Result[14].Description.Should().Be("Occurs every 5 weeks on Wednesday and Saturday every 1 Seconds between 06:30:58 and 06:31:00 starting on 18/10/2021 0:00:00");
        }
        #endregion
        #region Monthly Configuration
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
            Configuration.Invoking(e => Calculator.GetNextExecutionDate(Configuration)).Should().Throw<SchedulerException>()
                .WithMessage("You must set a positive month frecuency");            
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
            Configuration.Invoking(e => Calculator.GetNextExecutionDate(Configuration)).Should().Throw<SchedulerException>()
                .WithMessage("You must indicate a day if monthly day frecuency is selected");
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
            Configuration.Invoking(e => Calculator.GetNextExecutionDate(Configuration)).Should().Throw<SchedulerException>()
                .WithMessage("You must indicate a day if monthly day frecuency is selected");            
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
            Configuration.Invoking(e => Calculator.GetNextExecutionDate(Configuration)).Should().Throw<SchedulerException>()
                .WithMessage("You must indicate a day if monthly day frecuency is selected");
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
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 15, 30, 0));
            Result[0].Description.Should().Be("Occurs the 18th of every 5 months at 15:30:00");
            Result[1].NextDate.Should().Be(new DateTime(2022, 3, 18, 15, 30, 0));
            Result[1].Description.Should().Be("Occurs the 18th of every 5 months at 15:30:00");
            Result[2].NextDate.Should().Be(new DateTime(2022, 8, 18, 15, 30, 0));
            Result[2].Description.Should().Be("Occurs the 18th of every 5 months at 15:30:00");
            Result[3].NextDate.Should().Be(new DateTime(2023, 1, 18, 15, 30, 0));
            Result[3].Description.Should().Be("Occurs the 18th of every 5 months at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_10th_Every_2_Months_With_Daily_Frecuency_Repeated_Hourly_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(8, 30, 0),
                TimeFrecuency = TimeFrecuency.Hours,
                DailyFrecuency = 1,
                StartDate = TestDate,
                MonthDayFrecuency = true,
                DayOfMonth = 10,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs the 10th of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 7, 30, 0));
            Result[1].Description.Should().Be("Occurs the 10th of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 0));
            Result[2].Description.Should().Be("Occurs the 10th of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 10, 6, 30, 0));
            Result[3].Description.Should().Be("Occurs the 10th of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 10, 7, 30, 0));
            Result[4].Description.Should().Be("Occurs the 10th of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 10, 8, 30, 0));
            Result[5].Description.Should().Be("Occurs the 10th of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 10, 6, 30, 0));
            Result[6].Description.Should().Be("Occurs the 10th of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_10th_Every_2_Months_With_Daily_Frecuency_Repeated_Minute_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(6, 32, 0),
                TimeFrecuency = TimeFrecuency.Minutes,
                DailyFrecuency = 1,
                StartDate = TestDate,
                MonthDayFrecuency = true,
                DayOfMonth = 10,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs the 10th of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 31, 0));
            Result[1].Description.Should().Be("Occurs the 10th of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 32, 0));
            Result[2].Description.Should().Be("Occurs the 10th of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 10, 6, 30, 0));
            Result[3].Description.Should().Be("Occurs the 10th of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 10, 6, 31, 0));
            Result[4].Description.Should().Be("Occurs the 10th of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 10, 6, 32, 0));
            Result[5].Description.Should().Be("Occurs the 10th of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 10, 6, 30, 0));
            Result[6].Description.Should().Be("Occurs the 10th of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_10th_Every_2_Months_With_Daily_Frecuency_Repeated_Seconds_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(8, 30, 58),
                DailyEndHour = new TimeSpan(8, 31, 0),
                TimeFrecuency = TimeFrecuency.Seconds,
                DailyFrecuency = 1,
                StartDate = TestDate,
                MonthDayFrecuency = true,
                DayOfMonth = 10,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 58));
            Result[0].Description.Should().Be("Occurs the 10th of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 59));
            Result[1].Description.Should().Be("Occurs the 10th of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 31, 0));
            Result[2].Description.Should().Be("Occurs the 10th of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 10, 8, 30, 58));
            Result[3].Description.Should().Be("Occurs the 10th of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 10, 8, 30, 59));
            Result[4].Description.Should().Be("Occurs the 10th of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 10, 8, 31, 0));
            Result[5].Description.Should().Be("Occurs the 10th of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 10, 8, 30, 58));
            Result[6].Description.Should().Be("Occurs the 10th of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
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
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 15, 30, 0));
            Result[0].Description.Should().Be("Occurs the First Tuesday of every 2 months at 15:30:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 12, 7, 15, 30, 0));
            Result[1].Description.Should().Be("Occurs the First Tuesday of every 2 months at 15:30:00");
            Result[2].NextDate.Should().Be(new DateTime(2022, 2, 1, 15, 30, 0));
            Result[2].Description.Should().Be("Occurs the First Tuesday of every 2 months at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_First_Tuesday_Every_2_Months_With_Daily_Frecuency_Repeated_Hours_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(8, 30, 0),
                TimeFrecuency = TimeFrecuency.Hours,
                DailyFrecuency = 1,
                StartDate = TestDate,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.First,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Tuesday,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs the First Tuesday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 7, 30, 0));
            Result[1].Description.Should().Be("Occurs the First Tuesday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 0));
            Result[2].Description.Should().Be("Occurs the First Tuesday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 7, 6, 30, 0));
            Result[3].Description.Should().Be("Occurs the First Tuesday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 7, 7, 30, 0));
            Result[4].Description.Should().Be("Occurs the First Tuesday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 7, 8, 30, 0));
            Result[5].Description.Should().Be("Occurs the First Tuesday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 1, 6, 30, 0));
            Result[6].Description.Should().Be("Occurs the First Tuesday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_First_Tuesday_Every_2_Months_With_Daily_Frecuency_Repeated_Minutes_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(6, 32, 0),
                StartDate = TestDate,
                TimeFrecuency = TimeFrecuency.Minutes,
                DailyFrecuency = 1,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.First,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Tuesday,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs the First Tuesday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 31, 0));
            Result[1].Description.Should().Be("Occurs the First Tuesday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 32, 0));
            Result[2].Description.Should().Be("Occurs the First Tuesday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 7, 6, 30, 0));
            Result[3].Description.Should().Be("Occurs the First Tuesday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 7, 6, 31, 0));
            Result[4].Description.Should().Be("Occurs the First Tuesday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 7, 6, 32, 0));
            Result[5].Description.Should().Be("Occurs the First Tuesday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 1, 6, 30, 0));
            Result[6].Description.Should().Be("Occurs the First Tuesday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_First_Tuesday_Every_2_Months_With_Daily_Frecuency_Repeated_Seconds_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(8, 30, 58),
                DailyEndHour = new TimeSpan(8, 31, 0),
                TimeFrecuency = TimeFrecuency.Seconds,
                DailyFrecuency = 1,
                StartDate = TestDate,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.First,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Tuesday,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 58));
            Result[0].Description.Should().Be("Occurs the First Tuesday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 59));
            Result[1].Description.Should().Be("Occurs the First Tuesday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 31, 0));
            Result[2].Description.Should().Be("Occurs the First Tuesday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 7, 8, 30, 58));
            Result[3].Description.Should().Be("Occurs the First Tuesday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 7, 8, 30, 59));
            Result[4].Description.Should().Be("Occurs the First Tuesday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 7, 8, 31, 0));
            Result[5].Description.Should().Be("Occurs the First Tuesday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 1, 8, 30, 58));
            Result[6].Description.Should().Be("Occurs the First Tuesday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
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
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 15, 30, 0));
            Result[0].Description.Should().Be("Occurs the Last Saturday of every 2 months at 15:30:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 12, 25, 15, 30, 0));
            Result[1].Description.Should().Be("Occurs the Last Saturday of every 2 months at 15:30:00");
            Result[2].NextDate.Should().Be(new DateTime(2022, 2, 26, 15, 30, 0));
            Result[2].Description.Should().Be("Occurs the Last Saturday of every 2 months at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_Last_Saturday_Every_2_Months_With_Daily_Frecuency_Repeated_Hours_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(8, 30, 0),
                TimeFrecuency = TimeFrecuency.Hours,
                DailyFrecuency = 1,
                StartDate = TestDate,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Last,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Saturday,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs the Last Saturday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 7, 30, 0));
            Result[1].Description.Should().Be("Occurs the Last Saturday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 0));
            Result[2].Description.Should().Be("Occurs the Last Saturday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 25, 6, 30, 0));
            Result[3].Description.Should().Be("Occurs the Last Saturday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 25, 7, 30, 0));
            Result[4].Description.Should().Be("Occurs the Last Saturday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 25, 8, 30, 0));
            Result[5].Description.Should().Be("Occurs the Last Saturday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 26, 6, 30, 0));
            Result[6].Description.Should().Be("Occurs the Last Saturday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_Last_Saturday_Every_2_Months_With_Daily_Frecuency_Repeated_Minutes_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(6, 32, 0),
                StartDate = TestDate,
                TimeFrecuency = TimeFrecuency.Minutes,
                DailyFrecuency = 1,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Last,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Saturday,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs the Last Saturday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 31, 0));
            Result[1].Description.Should().Be("Occurs the Last Saturday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 32, 0));
            Result[2].Description.Should().Be("Occurs the Last Saturday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 25, 6, 30, 0));
            Result[3].Description.Should().Be("Occurs the Last Saturday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 25, 6, 31, 0));
            Result[4].Description.Should().Be("Occurs the Last Saturday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 25, 6, 32, 0));
            Result[5].Description.Should().Be("Occurs the Last Saturday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 26, 6, 30, 0));
            Result[6].Description.Should().Be("Occurs the Last Saturday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_Last_Saturday_Every_2_Months_With_Daily_Frecuency_Repeated_Seconds_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(8, 30, 58),
                DailyEndHour = new TimeSpan(8, 31, 0),
                TimeFrecuency = TimeFrecuency.Seconds,
                DailyFrecuency = 1,
                StartDate = TestDate,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Last,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Saturday,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 58));
            Result[0].Description.Should().Be("Occurs the Last Saturday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 59));
            Result[1].Description.Should().Be("Occurs the Last Saturday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 31, 0));
            Result[2].Description.Should().Be("Occurs the Last Saturday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 25, 8, 30, 58));
            Result[3].Description.Should().Be("Occurs the Last Saturday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 25, 8, 30, 59));
            Result[4].Description.Should().Be("Occurs the Last Saturday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 25, 8, 31, 0));
            Result[5].Description.Should().Be("Occurs the Last Saturday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 26, 8, 30, 58));
            Result[6].Description.Should().Be("Occurs the Last Saturday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
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
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 15, 30, 0));
            Result[0].Description.Should().Be("Occurs the Second Weekday of every 2 months at 15:30:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 12, 2, 15, 30, 0));
            Result[1].Description.Should().Be("Occurs the Second Weekday of every 2 months at 15:30:00");
            Result[2].NextDate.Should().Be(new DateTime(2022, 2, 2, 15, 30, 0));
            Result[2].Description.Should().Be("Occurs the Second Weekday of every 2 months at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_Second_Weekday_Every_2_Months_With_Daily_Frecuency_Repeated_Hours_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(8, 30, 0),
                TimeFrecuency = TimeFrecuency.Hours,
                DailyFrecuency = 1,
                StartDate = TestDate,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Second,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Weekday,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs the Second Weekday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 7, 30, 0));
            Result[1].Description.Should().Be("Occurs the Second Weekday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 0));
            Result[2].Description.Should().Be("Occurs the Second Weekday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 2, 6, 30, 0));
            Result[3].Description.Should().Be("Occurs the Second Weekday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 2, 7, 30, 0));
            Result[4].Description.Should().Be("Occurs the Second Weekday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 2, 8, 30, 0));
            Result[5].Description.Should().Be("Occurs the Second Weekday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 2, 6, 30, 0));
            Result[6].Description.Should().Be("Occurs the Second Weekday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_Second_Weekday_Every_2_Months_With_Daily_Frecuency_Repeated_Minutes_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(6, 32, 0),
                StartDate = TestDate,
                TimeFrecuency = TimeFrecuency.Minutes,
                DailyFrecuency = 1,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Second,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Weekday,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs the Second Weekday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 31, 0));
            Result[1].Description.Should().Be("Occurs the Second Weekday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 32, 0));
            Result[2].Description.Should().Be("Occurs the Second Weekday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 2, 6, 30, 0));
            Result[3].Description.Should().Be("Occurs the Second Weekday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 2, 6, 31, 0));
            Result[4].Description.Should().Be("Occurs the Second Weekday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 2, 6, 32, 0));
            Result[5].Description.Should().Be("Occurs the Second Weekday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 2, 6, 30, 0));
            Result[6].Description.Should().Be("Occurs the Second Weekday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_Second_Weekday_Every_2_Months_With_Daily_Frecuency_Repeated_Seconds_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(8, 30, 58),
                DailyEndHour = new TimeSpan(8, 31, 0),
                TimeFrecuency = TimeFrecuency.Seconds,
                DailyFrecuency = 1,
                StartDate = TestDate,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Second,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Weekday,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 58));
            Result[0].Description.Should().Be("Occurs the Second Weekday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 59));
            Result[1].Description.Should().Be("Occurs the Second Weekday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 31, 0));
            Result[2].Description.Should().Be("Occurs the Second Weekday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 2, 8, 30, 58));
            Result[3].Description.Should().Be("Occurs the Second Weekday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 2, 8, 30, 59));
            Result[4].Description.Should().Be("Occurs the Second Weekday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 2, 8, 31, 0));
            Result[5].Description.Should().Be("Occurs the Second Weekday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 2, 8, 30, 58));
            Result[6].Description.Should().Be("Occurs the Second Weekday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
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
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 15, 30, 0));
            Result[0].Description.Should().Be("Occurs the Second Weekendday of every 2 months at 15:30:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 12, 5, 15, 30, 0));
            Result[1].Description.Should().Be("Occurs the Second Weekendday of every 2 months at 15:30:00");
            Result[2].NextDate.Should().Be(new DateTime(2022, 2, 6, 15, 30, 0));
            Result[2].Description.Should().Be("Occurs the Second Weekendday of every 2 months at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_Second_WeekendDay_Every_2_Months_With_Daily_Frecuency_Repeated_Hours_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(8, 30, 0),
                TimeFrecuency = TimeFrecuency.Hours,
                DailyFrecuency = 1,
                StartDate = TestDate,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Second,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Weekendday,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs the Second Weekendday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 7, 30, 0));
            Result[1].Description.Should().Be("Occurs the Second Weekendday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 0));
            Result[2].Description.Should().Be("Occurs the Second Weekendday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 5, 6, 30, 0));
            Result[3].Description.Should().Be("Occurs the Second Weekendday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 5, 7, 30, 0));
            Result[4].Description.Should().Be("Occurs the Second Weekendday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 5, 8, 30, 0));
            Result[5].Description.Should().Be("Occurs the Second Weekendday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 6, 6, 30, 0));
            Result[6].Description.Should().Be("Occurs the Second Weekendday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_Second_WeekendDay_Every_2_Months_With_Daily_Frecuency_Repeated_Minutes_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(6, 32, 0),
                StartDate = TestDate,
                TimeFrecuency = TimeFrecuency.Minutes,
                DailyFrecuency = 1,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Second,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Weekendday,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs the Second Weekendday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 31, 0));
            Result[1].Description.Should().Be("Occurs the Second Weekendday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 32, 0));
            Result[2].Description.Should().Be("Occurs the Second Weekendday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 5, 6, 30, 0));
            Result[3].Description.Should().Be("Occurs the Second Weekendday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 5, 6, 31, 0));
            Result[4].Description.Should().Be("Occurs the Second Weekendday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 5, 6, 32, 0));
            Result[5].Description.Should().Be("Occurs the Second Weekendday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 6, 6, 30, 0));
            Result[6].Description.Should().Be("Occurs the Second Weekendday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_Second_WeekendDay_Every_2_Months_With_Daily_Frecuency_Repeated_Seconds_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(8, 30, 58),
                DailyEndHour = new TimeSpan(8, 31, 0),
                TimeFrecuency = TimeFrecuency.Seconds,
                DailyFrecuency = 1,
                StartDate = TestDate,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Second,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Weekendday,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 58));
            Result[0].Description.Should().Be("Occurs the Second Weekendday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 59));
            Result[1].Description.Should().Be("Occurs the Second Weekendday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 31, 0));
            Result[2].Description.Should().Be("Occurs the Second Weekendday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 5, 8, 30, 58));
            Result[3].Description.Should().Be("Occurs the Second Weekendday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 5, 8, 30, 59));
            Result[4].Description.Should().Be("Occurs the Second Weekendday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 5, 8, 31, 0));
            Result[5].Description.Should().Be("Occurs the Second Weekendday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 6, 8, 30, 58));
            Result[6].Description.Should().Be("Occurs the Second Weekendday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
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
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 15, 30, 0));
            Result[0].Description.Should().Be("Occurs the Third Weekendday of every 2 months at 15:30:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 12, 11, 15, 30, 0));
            Result[1].Description.Should().Be("Occurs the Third Weekendday of every 2 months at 15:30:00");
            Result[2].NextDate.Should().Be(new DateTime(2022, 2, 12, 15, 30, 0));
            Result[2].Description.Should().Be("Occurs the Third Weekendday of every 2 months at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_Third_WeekendDay_Every_2_Months_With_Daily_Frecuency_Repeated_Hours_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(8, 30, 0),
                TimeFrecuency = TimeFrecuency.Hours,
                DailyFrecuency = 1,
                StartDate = TestDate,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Third,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Weekendday,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs the Third Weekendday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 7, 30, 0));
            Result[1].Description.Should().Be("Occurs the Third Weekendday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 0));
            Result[2].Description.Should().Be("Occurs the Third Weekendday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 11, 6, 30, 0));
            Result[3].Description.Should().Be("Occurs the Third Weekendday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 11, 7, 30, 0));
            Result[4].Description.Should().Be("Occurs the Third Weekendday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 11, 8, 30, 0));
            Result[5].Description.Should().Be("Occurs the Third Weekendday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 12, 6, 30, 0));
            Result[6].Description.Should().Be("Occurs the Third Weekendday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_Third_WeekendDay_Every_2_Months_With_Daily_Frecuency_Repeated_Minutes_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(6, 32, 0),
                StartDate = TestDate,
                TimeFrecuency = TimeFrecuency.Minutes,
                DailyFrecuency = 1,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Third,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Weekendday,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs the Third Weekendday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 31, 0));
            Result[1].Description.Should().Be("Occurs the Third Weekendday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 32, 0));
            Result[2].Description.Should().Be("Occurs the Third Weekendday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 11, 6, 30, 0));
            Result[3].Description.Should().Be("Occurs the Third Weekendday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 11, 6, 31, 0));
            Result[4].Description.Should().Be("Occurs the Third Weekendday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 11, 6, 32, 0));
            Result[5].Description.Should().Be("Occurs the Third Weekendday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 12, 6, 30, 0));
            Result[6].Description.Should().Be("Occurs the Third Weekendday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_Third_WeekendDay_Every_2_Months_With_Daily_Frecuency_Repeated_Seconds_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(8, 30, 58),
                DailyEndHour = new TimeSpan(8, 31, 0),
                TimeFrecuency = TimeFrecuency.Seconds,
                DailyFrecuency = 1,
                StartDate = TestDate,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Third,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Weekendday,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 58));
            Result[0].Description.Should().Be("Occurs the Third Weekendday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 59));
            Result[1].Description.Should().Be("Occurs the Third Weekendday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 31, 0));
            Result[2].Description.Should().Be("Occurs the Third Weekendday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 11, 8, 30, 58));
            Result[3].Description.Should().Be("Occurs the Third Weekendday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 11, 8, 30, 59));
            Result[4].Description.Should().Be("Occurs the Third Weekendday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 11, 8, 31, 0));
            Result[5].Description.Should().Be("Occurs the Third Weekendday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 12, 8, 30, 58));
            Result[6].Description.Should().Be("Occurs the Third Weekendday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
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
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 15, 30, 0));
            Result[0].Description.Should().Be("Occurs the Fourth Friday of every 2 months at 15:30:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 12, 24, 15, 30, 0));
            Result[1].Description.Should().Be("Occurs the Fourth Friday of every 2 months at 15:30:00");
            Result[2].NextDate.Should().Be(new DateTime(2022, 2, 25, 15, 30, 0));
            Result[2].Description.Should().Be("Occurs the Fourth Friday of every 2 months at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_Fourth_Friday_Every_2_Months_With_Daily_Frecuency_Repeated_Hours_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(8, 30, 0),
                TimeFrecuency = TimeFrecuency.Hours,
                DailyFrecuency = 1,
                StartDate = TestDate,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Fourth,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Friday,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs the Fourth Friday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 7, 30, 0));
            Result[1].Description.Should().Be("Occurs the Fourth Friday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 0));
            Result[2].Description.Should().Be("Occurs the Fourth Friday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 24, 6, 30, 0));
            Result[3].Description.Should().Be("Occurs the Fourth Friday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 24, 7, 30, 0));
            Result[4].Description.Should().Be("Occurs the Fourth Friday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 24, 8, 30, 0));
            Result[5].Description.Should().Be("Occurs the Fourth Friday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 25, 6, 30, 0));
            Result[6].Description.Should().Be("Occurs the Fourth Friday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_Fourth_Friday_Every_2_Months_With_Daily_Frecuency_Repeated_Minutes_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(6, 32, 0),
                StartDate = TestDate,
                TimeFrecuency = TimeFrecuency.Minutes,
                DailyFrecuency = 1,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Fourth,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Friday,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs the Fourth Friday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 31, 0));
            Result[1].Description.Should().Be("Occurs the Fourth Friday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 32, 0));
            Result[2].Description.Should().Be("Occurs the Fourth Friday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 24, 6, 30, 0));
            Result[3].Description.Should().Be("Occurs the Fourth Friday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 24, 6, 31, 0));
            Result[4].Description.Should().Be("Occurs the Fourth Friday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 24, 6, 32, 0));
            Result[5].Description.Should().Be("Occurs the Fourth Friday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 25, 6, 30, 0));
            Result[6].Description.Should().Be("Occurs the Fourth Friday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_Fourth_Friday_Every_2_Months_With_Daily_Frecuency_Repeated_Seconds_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(8, 30, 58),
                DailyEndHour = new TimeSpan(8, 31, 0),
                TimeFrecuency = TimeFrecuency.Seconds,
                DailyFrecuency = 1,
                StartDate = TestDate,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Fourth,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Friday,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 58));
            Result[0].Description.Should().Be("Occurs the Fourth Friday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 59));
            Result[1].Description.Should().Be("Occurs the Fourth Friday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 31, 0));
            Result[2].Description.Should().Be("Occurs the Fourth Friday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 24, 8, 30, 58));
            Result[3].Description.Should().Be("Occurs the Fourth Friday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 24, 8, 30, 59));
            Result[4].Description.Should().Be("Occurs the Fourth Friday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 24, 8, 31, 0));
            Result[5].Description.Should().Be("Occurs the Fourth Friday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 25, 8, 30, 58));
            Result[6].Description.Should().Be("Occurs the Fourth Friday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
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
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 15, 30, 0));
            Result[0].Description.Should().Be("Occurs the Last Monday of every 2 months at 15:30:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 12, 27, 15, 30, 0));
            Result[1].Description.Should().Be("Occurs the Last Monday of every 2 months at 15:30:00");
            Result[2].NextDate.Should().Be(new DateTime(2022, 2, 28, 15, 30, 0));
            Result[2].Description.Should().Be("Occurs the Last Monday of every 2 months at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_Last_Monday_Every_2_Months_With_Daily_Frecuency_Repeated_Hours_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(8, 30, 0),
                TimeFrecuency = TimeFrecuency.Hours,
                DailyFrecuency = 1,
                StartDate = TestDate,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Last,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Monday,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs the Last Monday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 7, 30, 0));
            Result[1].Description.Should().Be("Occurs the Last Monday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 0));
            Result[2].Description.Should().Be("Occurs the Last Monday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 27, 6, 30, 0));
            Result[3].Description.Should().Be("Occurs the Last Monday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 27, 7, 30, 0));
            Result[4].Description.Should().Be("Occurs the Last Monday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 27, 8, 30, 0));
            Result[5].Description.Should().Be("Occurs the Last Monday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 28, 6, 30, 0));
            Result[6].Description.Should().Be("Occurs the Last Monday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_Last_Monday_Every_2_Months_With_Daily_Frecuency_Repeated_Minutes_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(6, 32, 0),
                StartDate = TestDate,
                TimeFrecuency = TimeFrecuency.Minutes,
                DailyFrecuency = 1,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Last,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Monday,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs the Last Monday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 31, 0));
            Result[1].Description.Should().Be("Occurs the Last Monday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 32, 0));
            Result[2].Description.Should().Be("Occurs the Last Monday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 27, 6, 30, 0));
            Result[3].Description.Should().Be("Occurs the Last Monday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 27, 6, 31, 0));
            Result[4].Description.Should().Be("Occurs the Last Monday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 27, 6, 32, 0));
            Result[5].Description.Should().Be("Occurs the Last Monday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 28, 6, 30, 0));
            Result[6].Description.Should().Be("Occurs the Last Monday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_Last_Monday_Every_2_Months_With_Daily_Frecuency_Repeated_Seconds_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(8, 30, 58),
                DailyEndHour = new TimeSpan(8, 31, 0),
                TimeFrecuency = TimeFrecuency.Seconds,
                DailyFrecuency = 1,
                StartDate = TestDate,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Last,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Monday,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 58));
            Result[0].Description.Should().Be("Occurs the Last Monday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 59));
            Result[1].Description.Should().Be("Occurs the Last Monday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 31, 0));
            Result[2].Description.Should().Be("Occurs the Last Monday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 27, 8, 30, 58));
            Result[3].Description.Should().Be("Occurs the Last Monday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 27, 8, 30, 59));
            Result[4].Description.Should().Be("Occurs the Last Monday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 27, 8, 31, 0));
            Result[5].Description.Should().Be("Occurs the Last Monday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 28, 8, 30, 58));
            Result[6].Description.Should().Be("Occurs the Last Monday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
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
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 15, 30, 0));
            Result[0].Description.Should().Be("Occurs the First Wednesday of every 2 months at 15:30:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 12, 1, 15, 30, 0));
            Result[1].Description.Should().Be("Occurs the First Wednesday of every 2 months at 15:30:00");
            Result[2].NextDate.Should().Be(new DateTime(2022, 2, 2, 15, 30, 0));
            Result[2].Description.Should().Be("Occurs the First Wednesday of every 2 months at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_First_Wednesday_Every_2_Months_With_Daily_Frecuency_Repated_Hours_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(8, 30, 0),
                TimeFrecuency = TimeFrecuency.Hours,
                DailyFrecuency = 1,
                StartDate = TestDate,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.First,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Wednesday,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs the First Wednesday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 7, 30, 0));
            Result[1].Description.Should().Be("Occurs the First Wednesday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 0));
            Result[2].Description.Should().Be("Occurs the First Wednesday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 1, 6, 30, 0));
            Result[3].Description.Should().Be("Occurs the First Wednesday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 1, 7, 30, 0));
            Result[4].Description.Should().Be("Occurs the First Wednesday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 1, 8, 30, 0));
            Result[5].Description.Should().Be("Occurs the First Wednesday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 2, 6, 30, 0));
            Result[6].Description.Should().Be("Occurs the First Wednesday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_First_Wednesday_Every_2_Months_With_Daily_Frecuency_Repeated_Minutes_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(6, 32, 0),
                StartDate = TestDate,
                TimeFrecuency = TimeFrecuency.Minutes,
                DailyFrecuency = 1,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.First,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Wednesday,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs the First Wednesday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 31, 0));
            Result[1].Description.Should().Be("Occurs the First Wednesday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 32, 0));
            Result[2].Description.Should().Be("Occurs the First Wednesday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 1, 6, 30, 0));
            Result[3].Description.Should().Be("Occurs the First Wednesday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 1, 6, 31, 0));
            Result[4].Description.Should().Be("Occurs the First Wednesday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 1, 6, 32, 0));
            Result[5].Description.Should().Be("Occurs the First Wednesday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 2, 6, 30, 0));
            Result[6].Description.Should().Be("Occurs the First Wednesday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_First_Wednesday_Every_2_Months_With_Daily_Frecuency_Repeated_Seconds_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(8, 30, 58),
                DailyEndHour = new TimeSpan(8, 31, 0),
                TimeFrecuency = TimeFrecuency.Seconds,
                DailyFrecuency = 1,
                StartDate = TestDate,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.First,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Wednesday,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 58));
            Result[0].Description.Should().Be("Occurs the First Wednesday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 59));
            Result[1].Description.Should().Be("Occurs the First Wednesday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 31, 0));
            Result[2].Description.Should().Be("Occurs the First Wednesday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 1, 8, 30, 58));
            Result[3].Description.Should().Be("Occurs the First Wednesday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 1, 8, 30, 59));
            Result[4].Description.Should().Be("Occurs the First Wednesday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 1, 8, 31, 0));
            Result[5].Description.Should().Be("Occurs the First Wednesday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 2, 8, 30, 58));
            Result[6].Description.Should().Be("Occurs the First Wednesday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
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
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 15, 30, 0));
            Result[0].Description.Should().Be("Occurs the Second Thursday of every 2 months at 15:30:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 12, 9, 15, 30, 0));
            Result[1].Description.Should().Be("Occurs the Second Thursday of every 2 months at 15:30:00");
            Result[2].NextDate.Should().Be(new DateTime(2022, 2, 10, 15, 30, 0));
            Result[2].Description.Should().Be("Occurs the Second Thursday of every 2 months at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_Second_Thursday_Every_2_Months_With_Daily_Frecuency_Repeated_Hours_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(8, 30, 0),
                TimeFrecuency = TimeFrecuency.Hours,
                DailyFrecuency = 1,
                StartDate = TestDate,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Second,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Thursday,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs the Second Thursday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 7, 30, 0));
            Result[1].Description.Should().Be("Occurs the Second Thursday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 0));
            Result[2].Description.Should().Be("Occurs the Second Thursday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 9, 6, 30, 0));
            Result[3].Description.Should().Be("Occurs the Second Thursday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 9, 7, 30, 0));
            Result[4].Description.Should().Be("Occurs the Second Thursday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 9, 8, 30, 0));
            Result[5].Description.Should().Be("Occurs the Second Thursday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 10, 6, 30, 0));
            Result[6].Description.Should().Be("Occurs the Second Thursday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_Second_Thursday_Every_2_Months_With_Daily_Frecuency_Repeated_Minutes_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(6, 32, 0),
                StartDate = TestDate,
                TimeFrecuency = TimeFrecuency.Minutes,
                DailyFrecuency = 1,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Second,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Thursday,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs the Second Thursday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 31, 0));
            Result[1].Description.Should().Be("Occurs the Second Thursday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 32, 0));
            Result[2].Description.Should().Be("Occurs the Second Thursday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 9, 6, 30, 0));
            Result[3].Description.Should().Be("Occurs the Second Thursday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 9, 6, 31, 0));
            Result[4].Description.Should().Be("Occurs the Second Thursday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 9, 6, 32, 0));
            Result[5].Description.Should().Be("Occurs the Second Thursday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 10, 6, 30, 0));
            Result[6].Description.Should().Be("Occurs the Second Thursday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_Second_Thursday_Every_2_Months_With_Daily_Frecuency_Repeated_Seconds_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(8, 30, 58),
                DailyEndHour = new TimeSpan(8, 31, 0),
                TimeFrecuency = TimeFrecuency.Seconds,
                DailyFrecuency = 1,
                StartDate = TestDate,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Second,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Thursday,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 58));
            Result[0].Description.Should().Be("Occurs the Second Thursday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 59));
            Result[1].Description.Should().Be("Occurs the Second Thursday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 31, 0));
            Result[2].Description.Should().Be("Occurs the Second Thursday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 9, 8, 30, 58));
            Result[3].Description.Should().Be("Occurs the Second Thursday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 9, 8, 30, 59));
            Result[4].Description.Should().Be("Occurs the Second Thursday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 9, 8, 31, 0));
            Result[5].Description.Should().Be("Occurs the Second Thursday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 10, 8, 30, 58));
            Result[6].Description.Should().Be("Occurs the Second Thursday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
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
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 15, 30, 0));
            Result[0].Description.Should().Be("Occurs the Third Sunday of every 2 months at 15:30:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 12, 19, 15, 30, 0));
            Result[1].Description.Should().Be("Occurs the Third Sunday of every 2 months at 15:30:00");
            Result[2].NextDate.Should().Be(new DateTime(2022, 2, 20, 15, 30, 0));
            Result[2].Description.Should().Be("Occurs the Third Sunday of every 2 months at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_Third_Sunday_Every_2_Months_With_Daily_Frecuency_Repeated_Hours_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(8, 30, 0),
                TimeFrecuency = TimeFrecuency.Hours,
                DailyFrecuency = 1,
                StartDate = TestDate,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Third,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Sunday,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs the Third Sunday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 7, 30, 0));
            Result[1].Description.Should().Be("Occurs the Third Sunday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 0));
            Result[2].Description.Should().Be("Occurs the Third Sunday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 19, 6, 30, 0));
            Result[3].Description.Should().Be("Occurs the Third Sunday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 19, 7, 30, 0));
            Result[4].Description.Should().Be("Occurs the Third Sunday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 19, 8, 30, 0));
            Result[5].Description.Should().Be("Occurs the Third Sunday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 20, 6, 30, 0));
            Result[6].Description.Should().Be("Occurs the Third Sunday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_Third_Sunday_Every_2_Months_With_Daily_Frecuency_Repeated_Minutes_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(6, 32, 0),
                StartDate = TestDate,
                TimeFrecuency = TimeFrecuency.Minutes,
                DailyFrecuency = 1,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Third,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Sunday,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs the Third Sunday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 31, 0));
            Result[1].Description.Should().Be("Occurs the Third Sunday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 32, 0));
            Result[2].Description.Should().Be("Occurs the Third Sunday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 19, 6, 30, 0));
            Result[3].Description.Should().Be("Occurs the Third Sunday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 19, 6, 31, 0));
            Result[4].Description.Should().Be("Occurs the Third Sunday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 19, 6, 32, 0));
            Result[5].Description.Should().Be("Occurs the Third Sunday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 20, 6, 30, 0));
            Result[6].Description.Should().Be("Occurs the Third Sunday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_Third_Sunday_Every_2_Months_With_Daily_Frecuency_Repeated_Seconds_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(8, 30, 58),
                DailyEndHour = new TimeSpan(8, 31, 0),
                TimeFrecuency = TimeFrecuency.Seconds,
                DailyFrecuency = 1,
                StartDate = TestDate,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Third,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Sunday,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 58));
            Result[0].Description.Should().Be("Occurs the Third Sunday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 59));
            Result[1].Description.Should().Be("Occurs the Third Sunday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 31, 0));
            Result[2].Description.Should().Be("Occurs the Third Sunday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 19, 8, 30, 58));
            Result[3].Description.Should().Be("Occurs the Third Sunday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 19, 8, 30, 59));
            Result[4].Description.Should().Be("Occurs the Third Sunday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 19, 8, 31, 0));
            Result[5].Description.Should().Be("Occurs the Third Sunday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 20, 8, 30, 58));
            Result[6].Description.Should().Be("Occurs the Third Sunday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
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
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 15, 30, 0));
            Result[0].Description.Should().Be("Occurs the Last Day of every 2 months at 15:30:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 12, 31, 15, 30, 0));
            Result[1].Description.Should().Be("Occurs the Last Day of every 2 months at 15:30:00");
            Result[2].NextDate.Should().Be(new DateTime(2022, 2, 28, 15, 30, 0));
            Result[2].Description.Should().Be("Occurs the Last Day of every 2 months at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_Last_Day_Every_2_Months_With_Daily_Frecuency_Repeated_Hours_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(8, 30, 0),
                TimeFrecuency = TimeFrecuency.Hours,
                DailyFrecuency = 1,
                StartDate = TestDate,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Last,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Day,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs the Last Day of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 7, 30, 0));
            Result[1].Description.Should().Be("Occurs the Last Day of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 0));
            Result[2].Description.Should().Be("Occurs the Last Day of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 31, 6, 30, 0));
            Result[3].Description.Should().Be("Occurs the Last Day of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 31, 7, 30, 0));
            Result[4].Description.Should().Be("Occurs the Last Day of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 31, 8, 30, 0));
            Result[5].Description.Should().Be("Occurs the Last Day of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 28, 6, 30, 0));
            Result[6].Description.Should().Be("Occurs the Last Day of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_Last_Day_Every_2_Months_With_Daily_Frecuency_Repeated_Minutes_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(6, 32, 0),
                StartDate = TestDate,
                TimeFrecuency = TimeFrecuency.Minutes,
                DailyFrecuency = 1,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Last,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Day,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs the Last Day of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 31, 0));
            Result[1].Description.Should().Be("Occurs the Last Day of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 32, 0));
            Result[2].Description.Should().Be("Occurs the Last Day of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 31, 6, 30, 0));
            Result[3].Description.Should().Be("Occurs the Last Day of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 31, 6, 31, 0));
            Result[4].Description.Should().Be("Occurs the Last Day of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 31, 6, 32, 0));
            Result[5].Description.Should().Be("Occurs the Last Day of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 28, 6, 30, 0));
            Result[6].Description.Should().Be("Occurs the Last Day of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_Last_Day_Every_2_Months_With_Daily_Frecuency_Repeated_Seconds_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(8, 30, 58),
                DailyEndHour = new TimeSpan(8, 31, 0),
                TimeFrecuency = TimeFrecuency.Seconds,
                DailyFrecuency = 1,
                StartDate = TestDate,
                MonthDayFrecuency = false,
                MonthlyDayFrecuency = MonthlyDayFrecuency.Last,
                MonthlyWeekDayFrecuency = MonthlyWeekDayFrecuency.Day,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 58));
            Result[0].Description.Should().Be("Occurs the Last Day of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 59));
            Result[1].Description.Should().Be("Occurs the Last Day of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 31, 0));
            Result[2].Description.Should().Be("Occurs the Last Day of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 31, 8, 30, 58));
            Result[3].Description.Should().Be("Occurs the Last Day of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 31, 8, 30, 59));
            Result[4].Description.Should().Be("Occurs the Last Day of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 31, 8, 31, 0));
            Result[5].Description.Should().Be("Occurs the Last Day of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 28, 8, 30, 58));
            Result[6].Description.Should().Be("Occurs the Last Day of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_1st_Every_2_Months_With_Daily_Frecuency_Once_Should_Return_Object()
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
                DayOfMonth = 1,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 15, 30, 0));
            Result[0].Description.Should().Be("Occurs the 1st of every 2 months at 15:30:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 12, 1, 15, 30, 0));
            Result[1].Description.Should().Be("Occurs the 1st of every 2 months at 15:30:00");
            Result[2].NextDate.Should().Be(new DateTime(2022, 2, 1, 15, 30, 0));
            Result[2].Description.Should().Be("Occurs the 1st of every 2 months at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_1st_Every_2_Months_With_Daily_Frecuency_Repeated_Hours_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(8, 30, 0),
                TimeFrecuency = TimeFrecuency.Hours,
                DailyFrecuency = 1,
                StartDate = TestDate,
                MonthDayFrecuency = true,
                DayOfMonth = 1,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs the 1st of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 7, 30, 0));
            Result[1].Description.Should().Be("Occurs the 1st of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 0));
            Result[2].Description.Should().Be("Occurs the 1st of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 1, 6, 30, 0));
            Result[3].Description.Should().Be("Occurs the 1st of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 1, 7, 30, 0));
            Result[4].Description.Should().Be("Occurs the 1st of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 1, 8, 30, 0));
            Result[5].Description.Should().Be("Occurs the 1st of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 1, 6, 30, 0));
            Result[6].Description.Should().Be("Occurs the 1st of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_1st_Every_2_Months_With_Daily_Frecuency_Repeated_Minutes_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(6, 32, 0),
                StartDate = TestDate,
                TimeFrecuency = TimeFrecuency.Minutes,
                DailyFrecuency = 1,
                MonthDayFrecuency = true,
                DayOfMonth = 1,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs the 1st of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 31, 0));
            Result[1].Description.Should().Be("Occurs the 1st of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 32, 0));
            Result[2].Description.Should().Be("Occurs the 1st of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 1, 6, 30, 0));
            Result[3].Description.Should().Be("Occurs the 1st of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 1, 6, 31, 0));
            Result[4].Description.Should().Be("Occurs the 1st of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 1, 6, 32, 0));
            Result[5].Description.Should().Be("Occurs the 1st of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 1, 6, 30, 0));
            Result[6].Description.Should().Be("Occurs the 1st of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_1st_Every_2_Months_With_Daily_Frecuency_Repeated_Seconds_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(8, 30, 58),
                DailyEndHour = new TimeSpan(8, 31, 0),
                TimeFrecuency = TimeFrecuency.Seconds,
                DailyFrecuency = 1,
                StartDate = TestDate,
                MonthDayFrecuency = true,
                DayOfMonth = 1,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 58));
            Result[0].Description.Should().Be("Occurs the 1st of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 59));
            Result[1].Description.Should().Be("Occurs the 1st of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 31, 0));
            Result[2].Description.Should().Be("Occurs the 1st of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 1, 8, 30, 58));
            Result[3].Description.Should().Be("Occurs the 1st of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 1, 8, 30, 59));
            Result[4].Description.Should().Be("Occurs the 1st of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 1, 8, 31, 0));
            Result[5].Description.Should().Be("Occurs the 1st of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 1, 8, 30, 58));
            Result[6].Description.Should().Be("Occurs the 1st of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_2nd_Every_2_Months_With_Daily_Frecuency_Once_Should_Return_Object()
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
                DayOfMonth = 2,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 15, 30, 0));
            Result[0].Description.Should().Be("Occurs the 2nd of every 2 months at 15:30:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 12, 2, 15, 30, 0));
            Result[1].Description.Should().Be("Occurs the 2nd of every 2 months at 15:30:00");
            Result[2].NextDate.Should().Be(new DateTime(2022, 2, 2, 15, 30, 0));
            Result[2].Description.Should().Be("Occurs the 2nd of every 2 months at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_2nd_Every_2_Months_With_Daily_Frecuency_Repeated_Hours_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(8, 30, 0),
                TimeFrecuency = TimeFrecuency.Hours,
                DailyFrecuency = 1,
                StartDate = TestDate,
                MonthDayFrecuency = true,
                DayOfMonth = 2,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs the 2nd of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 7, 30, 0));
            Result[1].Description.Should().Be("Occurs the 2nd of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 0));
            Result[2].Description.Should().Be("Occurs the 2nd of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 2, 6, 30, 0));
            Result[3].Description.Should().Be("Occurs the 2nd of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 2, 7, 30, 0));
            Result[4].Description.Should().Be("Occurs the 2nd of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 2, 8, 30, 0));
            Result[5].Description.Should().Be("Occurs the 2nd of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 2, 6, 30, 0));
            Result[6].Description.Should().Be("Occurs the 2nd of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_2nd_Every_2_Months_With_Daily_Frecuency_Repeated_Minutes_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(6, 32, 0),
                StartDate = TestDate,
                TimeFrecuency = TimeFrecuency.Minutes,
                DailyFrecuency = 1,
                MonthDayFrecuency = true,
                DayOfMonth = 2,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs the 2nd of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 31, 0));
            Result[1].Description.Should().Be("Occurs the 2nd of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 32, 0));
            Result[2].Description.Should().Be("Occurs the 2nd of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 2, 6, 30, 0));
            Result[3].Description.Should().Be("Occurs the 2nd of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 2, 6, 31, 0));
            Result[4].Description.Should().Be("Occurs the 2nd of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 2, 6, 32, 0));
            Result[5].Description.Should().Be("Occurs the 2nd of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 2, 6, 30, 0));
            Result[6].Description.Should().Be("Occurs the 2nd of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_2nd_Every_2_Months_With_Daily_Frecuency_Repeated_Seconds_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(8, 30, 58),
                DailyEndHour = new TimeSpan(8, 31, 0),
                TimeFrecuency = TimeFrecuency.Seconds,
                DailyFrecuency = 1,
                StartDate = TestDate,
                MonthDayFrecuency = true,
                DayOfMonth = 2,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 58));
            Result[0].Description.Should().Be("Occurs the 2nd of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 59));
            Result[1].Description.Should().Be("Occurs the 2nd of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 31, 0));
            Result[2].Description.Should().Be("Occurs the 2nd of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 2, 8, 30, 58));
            Result[3].Description.Should().Be("Occurs the 2nd of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 2, 8, 30, 59));
            Result[4].Description.Should().Be("Occurs the 2nd of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 2, 8, 31, 0));
            Result[5].Description.Should().Be("Occurs the 2nd of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 2, 8, 30, 58));
            Result[6].Description.Should().Be("Occurs the 2nd of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_3rd_Every_2_Months_With_Daily_Frecuency_Once_Should_Return_Object()
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
                DayOfMonth = 3,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 15, 30, 0));
            Result[0].Description.Should().Be("Occurs the 3rd of every 2 months at 15:30:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 12, 3, 15, 30, 0));
            Result[1].Description.Should().Be("Occurs the 3rd of every 2 months at 15:30:00");
            Result[2].NextDate.Should().Be(new DateTime(2022, 2, 3, 15, 30, 0));
            Result[2].Description.Should().Be("Occurs the 3rd of every 2 months at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_3rd_Every_2_Months_With_Daily_Frecuency_Repeated_Hours_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(8, 30, 0),
                TimeFrecuency = TimeFrecuency.Hours,
                DailyFrecuency = 1,
                StartDate = TestDate,
                MonthDayFrecuency = true,
                DayOfMonth = 3,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs the 3rd of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 7, 30, 0));
            Result[1].Description.Should().Be("Occurs the 3rd of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 0));
            Result[2].Description.Should().Be("Occurs the 3rd of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 3, 6, 30, 0));
            Result[3].Description.Should().Be("Occurs the 3rd of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 3, 7, 30, 0));
            Result[4].Description.Should().Be("Occurs the 3rd of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 3, 8, 30, 0));
            Result[5].Description.Should().Be("Occurs the 3rd of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 3, 6, 30, 0));
            Result[6].Description.Should().Be("Occurs the 3rd of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_3rd_Every_2_Months_With_Daily_Frecuency_Repeated_Minutes_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(6, 32, 0),
                StartDate = TestDate,
                TimeFrecuency = TimeFrecuency.Minutes,
                DailyFrecuency = 1,
                MonthDayFrecuency = true,
                DayOfMonth = 3,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs the 3rd of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 31, 0));
            Result[1].Description.Should().Be("Occurs the 3rd of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 32, 0));
            Result[2].Description.Should().Be("Occurs the 3rd of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 3, 6, 30, 0));
            Result[3].Description.Should().Be("Occurs the 3rd of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 3, 6, 31, 0));
            Result[4].Description.Should().Be("Occurs the 3rd of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 3, 6, 32, 0));
            Result[5].Description.Should().Be("Occurs the 3rd of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 3, 6, 30, 0));
            Result[6].Description.Should().Be("Occurs the 3rd of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_3rd_Every_2_Months_With_Daily_Frecuency_Repeated_Seconds_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(8, 30, 58),
                DailyEndHour = new TimeSpan(8, 31, 0),
                TimeFrecuency = TimeFrecuency.Seconds,
                DailyFrecuency = 1,
                StartDate = TestDate,
                MonthDayFrecuency = true,
                DayOfMonth = 3,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 58));
            Result[0].Description.Should().Be("Occurs the 3rd of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 59));
            Result[1].Description.Should().Be("Occurs the 3rd of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 31, 0));
            Result[2].Description.Should().Be("Occurs the 3rd of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 3, 8, 30, 58));
            Result[3].Description.Should().Be("Occurs the 3rd of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 3, 8, 30, 59));
            Result[4].Description.Should().Be("Occurs the 3rd of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 3, 8, 31, 0));
            Result[5].Description.Should().Be("Occurs the 3rd of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 3, 8, 30, 58));
            Result[6].Description.Should().Be("Occurs the 3rd of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_31th_Every_2_Months_With_Daily_Frecuency_Once_Should_Return_Object()
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
                DayOfMonth = 31,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 15, 30, 0));
            Result[0].Description.Should().Be("Occurs the 31th of every 2 months at 15:30:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 12, 31, 15, 30, 0));
            Result[1].Description.Should().Be("Occurs the 31th of every 2 months at 15:30:00");
            Result[2].NextDate.Should().Be(new DateTime(2022, 2, 28, 15, 30, 0));
            Result[2].Description.Should().Be("Occurs the 31th of every 2 months at 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_31th_Every_2_Months_With_Daily_Frecuency_Repeated_Hours_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(8, 30, 0),
                TimeFrecuency = TimeFrecuency.Hours,
                DailyFrecuency = 1,
                StartDate = TestDate,
                MonthDayFrecuency = true,
                DayOfMonth = 31,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs the 31th of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 7, 30, 0));
            Result[1].Description.Should().Be("Occurs the 31th of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 0));
            Result[2].Description.Should().Be("Occurs the 31th of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 31, 6, 30, 0));
            Result[3].Description.Should().Be("Occurs the 31th of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 31, 7, 30, 0));
            Result[4].Description.Should().Be("Occurs the 31th of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 31, 8, 30, 0));
            Result[5].Description.Should().Be("Occurs the 31th of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 28, 6, 30, 0));
            Result[6].Description.Should().Be("Occurs the 31th of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_31th_Every_2_Months_With_Daily_Frecuency_Repeated_Minutes_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(6, 30, 0),
                DailyEndHour = new TimeSpan(6, 32, 0),
                StartDate = TestDate,
                TimeFrecuency = TimeFrecuency.Minutes,
                DailyFrecuency = 1,
                MonthDayFrecuency = true,
                DayOfMonth = 31,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs the 31th of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 31, 0));
            Result[1].Description.Should().Be("Occurs the 31th of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 32, 0));
            Result[2].Description.Should().Be("Occurs the 31th of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 31, 6, 30, 0));
            Result[3].Description.Should().Be("Occurs the 31th of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 31, 6, 31, 0));
            Result[4].Description.Should().Be("Occurs the 31th of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 31, 6, 32, 0));
            Result[5].Description.Should().Be("Occurs the 31th of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 28, 6, 30, 0));
            Result[6].Description.Should().Be("Occurs the 31th of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 18/10/2021 0:00:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_On_31th_Every_2_Months_With_Daily_Frecuency_Repeated_Seconds_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration Configuration = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Monthly,
                Frecuency = 5,
                OccursOnceDaily = false,
                DailyStartHour = new TimeSpan(8, 30, 58),
                DailyEndHour = new TimeSpan(8, 31, 0),
                TimeFrecuency = TimeFrecuency.Seconds,
                DailyFrecuency = 1,
                StartDate = TestDate,
                MonthDayFrecuency = true,
                DayOfMonth = 31,
                MonthFrecuency = 2
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(Configuration, 7);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 58));
            Result[0].Description.Should().Be("Occurs the 31th of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 59));
            Result[1].Description.Should().Be("Occurs the 31th of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 31, 0));
            Result[2].Description.Should().Be("Occurs the 31th of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 31, 8, 30, 58));
            Result[3].Description.Should().Be("Occurs the 31th of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 31, 8, 30, 59));
            Result[4].Description.Should().Be("Occurs the 31th of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 31, 8, 31, 0));
            Result[5].Description.Should().Be("Occurs the 31th of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 28, 8, 30, 58));
            Result[6].Description.Should().Be("Occurs the 31th of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 18/10/2021 0:00:00");
        }
        #endregion
    }
}
