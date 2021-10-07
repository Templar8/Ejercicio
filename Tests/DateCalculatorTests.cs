using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scheduler;
using System;
using Xunit;
using FluentAssertions;
namespace Tests
{
    [TestClass]
    public class DateCalculatorTests
    {
        [TestMethod]
        public void Recurring_Type_Once_Without_Configuration_Date_Should_Throw_Exception()
        {
            DateCalculator Calculator = new DateCalculator();         
            Action NextDate = () => Calculator.GetNextExecutionDate(DateTime.Today, null, RecurringType.Once, SchedulerFrecuency.Daily, null, DateTime.Today, null);
            NextDate.Should().Throw<Exception>();
        }
        [TestMethod]
        public void Start_Date_Greater_Than_End_Date_Should_Throw_Exception()
        {
            DateCalculator Calculator = new DateCalculator();
            Action NextDate = () => Calculator.GetNextExecutionDate(DateTime.Today,null, RecurringType.Once, SchedulerFrecuency.Daily, null, DateTime.Today, DateTime.Today.AddDays(-5));
            NextDate.Should().Throw<Exception>();
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Negative_Frecuency_Should_Throw_Exception()
        {
            DateCalculator Calculator = new DateCalculator();
            Action NextDate = () => Calculator.GetNextExecutionDate(DateTime.Today,null, RecurringType.Recurring, SchedulerFrecuency.Daily, -5, DateTime.Today, null);
            NextDate.Should().Throw<Exception>();
        }
        [TestMethod]
        public void Recurring_Type_Once_With_Configuration_Date_Should_Return_Object()
        {
            DateCalculator Calculator = new DateCalculator();
            DateResult Result = Calculator.GetNextExecutionDate(DateTime.Today,DateTime.Today.AddDays(5),RecurringType.Once,SchedulerFrecuency.Daily,null,DateTime.Today, null);
            Result.NextDate.Should().Be(DateTime.Today.AddDays(5));
            Result.Description.Should().NotBeNullOrEmpty();
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Daily_Frecuency_Should_Return_Object()
        {
            DateCalculator Calculator = new DateCalculator();
            DateResult Result = Calculator.GetNextExecutionDate(DateTime.Today, null,RecurringType.Recurring, SchedulerFrecuency.Daily, 5, DateTime.Today, null);
            Result.NextDate.Should().Be(DateTime.Today.AddDays(5));
            Result.Description.Should().NotBeNullOrEmpty();
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Monthly_Frecuency_Should_Return_Object()
        {
            DateCalculator Calculator = new DateCalculator();
            DateResult Result = Calculator.GetNextExecutionDate(DateTime.Today, null, RecurringType.Recurring, SchedulerFrecuency.Monthly, 5, DateTime.Today, null);
            Result.NextDate.Should().Be(DateTime.Today.AddMonths(5));
            Result.Description.Should().NotBeNullOrEmpty();
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Yearly_Frecuency_Should_Return_Object()
        {
            DateCalculator Calculator = new DateCalculator();
            DateResult Result = Calculator.GetNextExecutionDate(DateTime.Today, null, RecurringType.Recurring, SchedulerFrecuency.Yearly, 5, DateTime.Today, null);
            Result.NextDate.Should().Be(DateTime.Today.AddYears(5));
            Result.Description.Should().NotBeNullOrEmpty();
        }
    }
}