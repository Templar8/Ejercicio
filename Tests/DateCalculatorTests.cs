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
                DailyHour = new TimeSpan(15, 30, 0),
                Language = Language.EnglishUK
            };
            Configuration.Invoking(e => Calculator.GetNextExecutionDate(Configuration)).Should().Throw<SchedulerException>()
                .WithMessage("The current date can't be date min or max values");
            Configuration.Language = Language.Espa�ol;
            Configuration.Invoking(e => Calculator.GetNextExecutionDate(Configuration)).Should().Throw<SchedulerException>()
                .WithMessage("La fecha actual no puede ser el valor m�nimo o m�ximo de la fecha");
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
            Configuration.Language = Language.Espa�ol;
            Configuration.Invoking(e => Calculator.GetNextExecutionDate(Configuration)).Should().Throw<SchedulerException>()
                .WithMessage("La fecha de comienzo no puede ser el valor m�ximo o m�nimo de la fecha");
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
            Configuration.Language = Language.Espa�ol;
            Configuration.Invoking(e => Calculator.GetNextExecutionDate(Configuration)).Should().Throw<SchedulerException>()
                .WithMessage("La fecha de comienzo no puede ser mayor que la fecha fin");
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
                DailyHour = new TimeSpan(15, 30, 0),
                Language = Language.EnglishUK
            };
            Configuration.Invoking(e => Calculator.GetNextExecutionDate(Configuration)).Should().Throw<SchedulerException>()
                .WithMessage("The end date can't be date min or max values");
            Configuration.Language = Language.Espa�ol;
            Configuration.Invoking(e => Calculator.GetNextExecutionDate(Configuration)).Should().Throw<SchedulerException>()
                .WithMessage("La fecha fin no puede ser el valor m�ximo o m�nimo de la fecha");
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
            Configuration.Language = Language.Espa�ol;
            Configuration.Invoking(e => Calculator.GetNextExecutionDate(Configuration)).Should().Throw<SchedulerException>()
                .WithMessage(@"Debe establecerse una configuraci�n Diaria indicanto si ocurre una vez al d�a (especificando la hora en la que ocurre) o si ocurre repetidas veces (indicando el n�mero de horas, minutos o segundos entre cada ejecuci�n y la hora de inicio y fin)");
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
            Configuration.Language = Language.Espa�ol;
            Configuration.Invoking(e => Calculator.GetNextExecutionDate(Configuration)).Should().Throw<SchedulerException>()
                .WithMessage("Si el modo 'Una vez' se selecciona debe indicarse una fecha de configuraci�n para comenzar el proceso");
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
            Configuration.Language = Language.Espa�ol;
            Configuration.Invoking(e => Calculator.GetNextExecutionDate(Configuration)).Should().Throw<SchedulerException>()
                .WithMessage("La fecha de configuraci�n no puede ser el valor m�nimo o m�ximo de la fecha");
        }
        [TestMethod]
        public void Recurring_Type_Once_With_Configuration_Date_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration ConfigurationEN = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                ConfigurationDate = TestDate.AddDays(5),
                Type = RecurringType.Once,
                SchedulerFrecuency = SchedulerFrecuency.Daily,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = TestDate,
                Language = Language.EnglishUK
            };
            SchedulerConfiguration ConfigurationUS = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                ConfigurationDate = TestDate.AddDays(5),
                Type = RecurringType.Once,
                SchedulerFrecuency = SchedulerFrecuency.Daily,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = TestDate,
                Language = Language.EnglishUS
            };
            SchedulerConfiguration ConfigurationES = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                ConfigurationDate = TestDate.AddDays(5),
                Type = RecurringType.Once,
                SchedulerFrecuency = SchedulerFrecuency.Daily,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = TestDate,
                Language = Language.Espa�ol
            };
            DateResult Result = Calculator.GetNextExecutionDate(ConfigurationEN);
            DateTime ExpectedDateTime = new DateTime(2021, 10, 23, 15, 30, 0);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().Be($"Occurs once. Schedule will be used on {ExpectedDateTime} starting on {ConfigurationEN.StartDate}"
                    + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result = Calculator.GetNextExecutionDate(ConfigurationUS);
            Result.Description.Should().Be($"Occurs once. Schedule will be used on {ExpectedDateTime} starting on {ConfigurationEN.StartDate}"
                    + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result = Calculator.GetNextExecutionDate(ConfigurationES);
            Result.Description.Should().Be($"Ocurre una vez. El calendario se utilizar� el {ExpectedDateTime} empezando el {ConfigurationEN.StartDate}"
                    + (ConfigurationEN.EndDate.HasValue ? $" y terminando el {ConfigurationEN.EndDate}" : string.Empty));
        }
        [TestMethod]
        public void Recurring_Type_Once_With_Configuration_Date_And_End_Date_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration ConfigurationEN = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                ConfigurationDate = TestDate.AddDays(5),
                Type = RecurringType.Once,
                SchedulerFrecuency = SchedulerFrecuency.Daily,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = TestDate,
                EndDate = TestDate.AddDays(6),
                Language = Language.EnglishUK
            };
            SchedulerConfiguration ConfigurationUS = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                ConfigurationDate = TestDate.AddDays(5),
                Type = RecurringType.Once,
                SchedulerFrecuency = SchedulerFrecuency.Daily,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = TestDate,
                EndDate = TestDate.AddDays(6),
                Language = Language.EnglishUS
            };
            SchedulerConfiguration ConfigurationES = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                ConfigurationDate = TestDate.AddDays(5),
                Type = RecurringType.Once,
                SchedulerFrecuency = SchedulerFrecuency.Daily,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = TestDate,
                EndDate = TestDate.AddDays(6),
                Language = Language.Espa�ol
            };
            DateResult Result = Calculator.GetNextExecutionDate(ConfigurationEN);
            DateTime ExpectedDateTime = new DateTime(2021, 10, 23, 15, 30, 0);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().Be($"Occurs once. Schedule will be used on {ExpectedDateTime} starting on {ConfigurationEN.StartDate}"
                    + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result = Calculator.GetNextExecutionDate(ConfigurationUS);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().Be($"Occurs once. Schedule will be used on {ExpectedDateTime} starting on {ConfigurationEN.StartDate}"
                    + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result = Calculator.GetNextExecutionDate(ConfigurationES);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().Be($"Ocurre una vez. El calendario se utilizar� el {ExpectedDateTime} empezando el {ConfigurationEN.StartDate}"
                    + (ConfigurationEN.EndDate.HasValue ? $" y terminando el {ConfigurationEN.EndDate}" : string.Empty));
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
            SchedulerConfiguration ConfigurationEN = new SchedulerConfiguration()
            {
                CurrentDate = DateTime.Today,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Daily,
                StartDate = DateTime.Today,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                Language = Language.EnglishUK
            };
            SchedulerConfiguration ConfigurationES = new SchedulerConfiguration()
            {
                CurrentDate = DateTime.Today,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Daily,
                StartDate = DateTime.Today,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                Language = Language.Espa�ol
            };
            SchedulerException Ex = UT.Assert.ThrowsException<SchedulerException>(() => Calculator.GetNextExecutionDate(ConfigurationEN));
            Ex.Message.Should().Be("If 'Recurring' type is selected you must indicate a frecuency");
            Ex = UT.Assert.ThrowsException<SchedulerException>(() => Calculator.GetNextExecutionDate(ConfigurationES));
            Ex.Message.Should().Be("Si se selecciona el modo 'Recurrente' debe indicarse una frecuencia");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Null_Frecuency_Should_Throw_Exception()
        {
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration ConfigurationEN = new SchedulerConfiguration()
            {
                CurrentDate = DateTime.Today,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Daily,
                Frecuency = -5,
                StartDate = DateTime.Today,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                Language = Language.EnglishUK
            };
            SchedulerConfiguration ConfigurationES = new SchedulerConfiguration()
            {
                CurrentDate = DateTime.Today,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Daily,
                Frecuency = -5,
                StartDate = DateTime.Today,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                Language = Language.Espa�ol
            };
            ConfigurationEN.Invoking(e => Calculator.GetNextExecutionDate(ConfigurationEN)).Should().Throw<SchedulerException>()
                .WithMessage("Frecuency can neither be negative nor exceed integer max or min values");
            ConfigurationES.Invoking(e => Calculator.GetNextExecutionDate(ConfigurationES)).Should().Throw<SchedulerException>()
                .WithMessage("La frecuencia no puede ser negativa ni exceder los valores m�ximo o m�nimo de los enteros");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Daily_Configuration_Recurring_Without_Frecuencys_Should_Throw_Exception()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();

            DayOfWeek[] Days = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Friday };
            SchedulerConfiguration ConfigurationEN = new SchedulerConfiguration()
            {
                CurrentDate = DateTime.Today,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Daily,
                Frecuency = 5,
                StartDate = DateTime.Today,
                OccursOnceDaily = false,
                Language = Language.EnglishUK
            };
            SchedulerConfiguration ConfigurationES = new SchedulerConfiguration()
            {
                CurrentDate = DateTime.Today,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Daily,
                Frecuency = 5,
                StartDate = DateTime.Today,
                OccursOnceDaily = false,
                Language = Language.Espa�ol
            };
            ConfigurationEN.Invoking(e => Calculator.GetNextExecutionDate(ConfigurationEN)).Should().Throw<SchedulerException>()
                .WithMessage("You must set a Daily Configuration indicating if occurs once in the day (especifying the hour when it happens) or if it occurs multiple times (indicating how many hours, minutes or seconds between executions and the start and end time)");
            ConfigurationES.Invoking(e => Calculator.GetNextExecutionDate(ConfigurationES)).Should().Throw<SchedulerException>()
                .WithMessage("Debe establecerse una configuraci�n Diaria indicanto si ocurre una vez al d�a (especificando la hora en la que ocurre) o si ocurre repetidas veces (indicando el n�mero de horas, minutos o segundos entre cada ejecuci�n y la hora de inicio y fin)");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Daily_Configuration_Occurs_Once_Without_Hour_Should_Throw_Exception()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DayOfWeek[] Days = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Friday };
            SchedulerConfiguration ConfigurationEN = new SchedulerConfiguration()
            {
                CurrentDate = DateTime.Today,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Daily,
                Frecuency = 5,
                StartDate = DateTime.Today,
                OccursOnceDaily = true,
                Language = Language.EnglishUK
            };
            SchedulerConfiguration ConfigurationES = new SchedulerConfiguration()
            {
                CurrentDate = DateTime.Today,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Daily,
                Frecuency = 5,
                StartDate = DateTime.Today,
                OccursOnceDaily = true,
                Language = Language.Espa�ol
            };
            ConfigurationEN.Invoking(e => Calculator.GetNextExecutionDate(ConfigurationEN)).Should().Throw<SchedulerException>()
                .WithMessage("You must set a Daily Configuration indicating if occurs once in the day (especifying the hour when it happens) or if it occurs multiple times (indicating how many hours, minutes or seconds between executions and the start and end time)");
            ConfigurationES.Invoking(e => Calculator.GetNextExecutionDate(ConfigurationES)).Should().Throw<SchedulerException>()
                .WithMessage("Debe establecerse una configuraci�n Diaria indicanto si ocurre una vez al d�a (especificando la hora en la que ocurre) o si ocurre repetidas veces (indicando el n�mero de horas, minutos o segundos entre cada ejecuci�n y la hora de inicio y fin)");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Daily_Frecuency_Without_End_Date_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration ConfigurationEN = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Daily,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = TestDate,
                Language = Language.EnglishUK
            };
            SchedulerConfiguration ConfigurationUS = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Daily,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = TestDate,
                Language = Language.EnglishUS
            };
            SchedulerConfiguration ConfigurationES = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Daily,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = TestDate,
                Language = Language.Espa�ol
            };
            DateResult Result = Calculator.GetNextExecutionDate(ConfigurationEN);
            DateTime ExpectedDateTime = new DateTime(2021, 10, 23, 15, 30, 0);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {ExpectedDateTime} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result = Calculator.GetNextExecutionDate(ConfigurationUS);
            Result.Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {ExpectedDateTime} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result = Calculator.GetNextExecutionDate(ConfigurationES);
            Result.Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {ExpectedDateTime} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Daily_Frecuency_With_End_Date_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration ConfigurationEN = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Daily,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = TestDate,
                EndDate = TestDate.AddDays(15),
                Language = Language.EnglishUK
            };
            SchedulerConfiguration ConfigurationUS = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Daily,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = TestDate,
                EndDate = TestDate.AddDays(15),
                Language = Language.EnglishUS
            };
            SchedulerConfiguration ConfigurationES = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Daily,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = TestDate,
                EndDate = TestDate.AddDays(15),
                Language = Language.Espa�ol
            };
            DateResult Result = Calculator.GetNextExecutionDate(ConfigurationEN);
            DateTime ExpectedDateTime = new DateTime(2021, 10, 23, 15, 30, 0);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {ExpectedDateTime} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result = Calculator.GetNextExecutionDate(ConfigurationEN);
            Result.Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {ExpectedDateTime} starting on {ConfigurationUS.StartDate}"
                 + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result = Calculator.GetNextExecutionDate(ConfigurationES);
            Result.Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {ExpectedDateTime} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Daily_Frecuency_Hourly_With_End_Date_Repeated_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration ConfigurationEN = new SchedulerConfiguration()
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
                EndDate = TestDate.AddDays(6),
                Language = Language.EnglishUK
            };
            SchedulerConfiguration ConfigurationUS = new SchedulerConfiguration()
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
                EndDate = TestDate.AddDays(6),
                Language = Language.EnglishUS
            };
            SchedulerConfiguration ConfigurationES = new SchedulerConfiguration()
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
                EndDate = TestDate.AddDays(6),
                Language = Language.Espa�ol
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(ConfigurationEN, 6);
            DateTime ExpectedDateTime = new DateTime(2021, 10, 23, 6, 30, 0);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 0));
            Result[0].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 23, 7, 30, 0));
            Result[1].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 7, 30, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 23, 8, 30, 0));
            Result[2].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 8, 30, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[3].NextDate.Should().Be(new DateTime(2021, 10, 23, 9, 30, 0));
            Result[3].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 9, 30, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[4].NextDate.Should().Be(new DateTime(2021, 10, 23, 10, 30, 0));
            Result[4].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 10, 30, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[5].Should().BeNull();
            Calculator = new DateCalculator();
            Result = Calculator.GetNextExecutionDateRecurring(ConfigurationUS, 6);
            Result[0].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[1].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 7, 30, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[2].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 8, 30, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[3].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 9, 30, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[4].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 10, 30, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Calculator = new DateCalculator();
            Result = Calculator.GetNextExecutionDateRecurring(ConfigurationES, 6);
            Result[0].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 30, 0)} " +
                $"empezando el {ConfigurationES.StartDate}" + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[1].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 7, 30, 0)} " +
                $"empezando el {ConfigurationES.StartDate}" + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[2].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 8, 30, 0)} " +
                $"empezando el {ConfigurationES.StartDate}" + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[3].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 9, 30, 0)} " +
                $"empezando el {ConfigurationES.StartDate}" + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[4].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 10, 30, 0)} " +
                $"empezando el {ConfigurationES.StartDate}" + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Daily_Frecuency_Hourly_Without_End_Date_Repeated_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration ConfigurationEN = new SchedulerConfiguration()
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
                Language = Language.EnglishUK
            };
            SchedulerConfiguration ConfigurationUS = new SchedulerConfiguration()
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
                Language = Language.EnglishUS
            };
            SchedulerConfiguration ConfigurationES = new SchedulerConfiguration()
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
                Language = Language.Espa�ol
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(ConfigurationEN, 7);
            DateTime ExpectedDateTime = new DateTime(2021, 10, 23, 6, 30, 0);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 0));
            Result[0].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 23, 7, 30, 0));
            Result[1].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 7, 30, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 23, 8, 30, 0));
            Result[2].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 8, 30, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[3].NextDate.Should().Be(new DateTime(2021, 10, 23, 9, 30, 0));
            Result[3].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 9, 30, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[4].NextDate.Should().Be(new DateTime(2021, 10, 23, 10, 30, 0));
            Result[4].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 10, 30, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[5].NextDate.Should().Be(new DateTime(2021, 10, 28, 6, 30, 0));
            Result[5].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 28, 6, 30, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[6].NextDate.Should().Be(new DateTime(2021, 10, 28, 7, 30, 0));
            Result[6].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 28, 7, 30, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));

            Calculator = new DateCalculator();
            Result = Calculator.GetNextExecutionDateRecurring(ConfigurationUS, 7);
            Result[0].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[1].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 7, 30, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[2].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 8, 30, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[3].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 9, 30, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[4].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 10, 30, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[5].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 28, 6, 30, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[6].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 28, 7, 30, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));

            Calculator = new DateCalculator();
            Result = Calculator.GetNextExecutionDateRecurring(ConfigurationES, 7);
            Result[0].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 30, 0)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[1].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 7, 30, 0)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[2].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 8, 30, 0)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[3].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 9, 30, 0)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[4].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 10, 30, 0)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[5].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 28, 6, 30, 0)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[6].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 28, 7, 30, 0)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Daily_Frecuency_Minutes_With_End_Date_Repeated_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration ConfigurationEN = new SchedulerConfiguration()
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
                EndDate = TestDate.AddDays(6),
                Language = Language.EnglishUK
            };
            SchedulerConfiguration ConfigurationUS = new SchedulerConfiguration()
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
                EndDate = TestDate.AddDays(6),
                Language = Language.EnglishUS
            };
            SchedulerConfiguration ConfigurationES = new SchedulerConfiguration()
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
                EndDate = TestDate.AddDays(6),
                Language = Language.Espa�ol
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(ConfigurationEN, 12);
            DateTime ExpectedDateTime = new DateTime(2021, 10, 23, 6, 50, 0);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 50, 0));
            Result[0].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 50, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 51, 0));
            Result[1].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 51, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 52, 0));
            Result[2].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 52, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[3].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 53, 0));
            Result[3].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 53, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[4].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 54, 0));
            Result[4].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 54, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[5].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 55, 0));
            Result[5].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 55, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[6].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 56, 0));
            Result[6].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 56, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[7].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 57, 0));
            Result[7].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 57, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[8].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 58, 0));
            Result[8].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 58, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[9].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 59, 0));
            Result[9].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 59, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[10].NextDate.Should().Be(new DateTime(2021, 10, 23, 7, 0, 0));
            Result[10].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 7, 0, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[11].Should().BeNull();

            Calculator = new DateCalculator();
            Result = Calculator.GetNextExecutionDateRecurring(ConfigurationUS, 12);
            Result[0].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 50, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[1].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 51, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[2].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 52, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[3].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 53, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[4].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 54, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[5].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 55, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[6].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 56, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[7].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 57, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[8].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 58, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[9].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 59, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[10].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 7, 0, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));


            Calculator = new DateCalculator();
            Result = Calculator.GetNextExecutionDateRecurring(ConfigurationES, 12);
            Result[0].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 50, 0)} empezando el {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" y terminando el {ConfigurationEN.EndDate}" : string.Empty));
            Result[1].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 51, 0)} empezando el {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" y terminando el {ConfigurationEN.EndDate}" : string.Empty));
            Result[2].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 52, 0)} empezando el {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" y terminando el {ConfigurationEN.EndDate}" : string.Empty));
            Result[3].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 53, 0)} empezando el {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" y terminando el {ConfigurationEN.EndDate}" : string.Empty));
            Result[4].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 54, 0)} empezando el {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" y terminando el {ConfigurationEN.EndDate}" : string.Empty));
            Result[5].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 55, 0)} empezando el {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" y terminando el {ConfigurationEN.EndDate}" : string.Empty));
            Result[6].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 56, 0)} empezando el {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" y terminando el {ConfigurationEN.EndDate}" : string.Empty));
            Result[7].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 57, 0)} empezando el {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" y terminando el {ConfigurationEN.EndDate}" : string.Empty));
            Result[8].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 58, 0)} empezando el {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" y terminando el {ConfigurationEN.EndDate}" : string.Empty));
            Result[9].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 59, 0)} empezando el {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" y terminando el {ConfigurationEN.EndDate}" : string.Empty));
            Result[10].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 7, 0, 0)} empezando el {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" y terminando el {ConfigurationEN.EndDate}" : string.Empty));



        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Daily_Frecuency_Minutes_Without_End_Date_Repeated_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration ConfigurationEN = new SchedulerConfiguration()
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
                Language = Language.EnglishUK
            };
            SchedulerConfiguration ConfigurationUS = new SchedulerConfiguration()
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
                Language = Language.EnglishUS
            };
            SchedulerConfiguration ConfigurationES = new SchedulerConfiguration()
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
                Language = Language.Espa�ol
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(ConfigurationEN, 12);
            DateTime ExpectedDateTime = new DateTime(2021, 10, 23, 6, 50, 0);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 50, 0));
            Result[0].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 50, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 51, 0));
            Result[1].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 51, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 52, 0));
            Result[2].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 52, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[3].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 53, 0));
            Result[3].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 53, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[4].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 54, 0));
            Result[4].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 54, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[5].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 55, 0));
            Result[5].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 55, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[6].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 56, 0));
            Result[6].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 56, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[7].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 57, 0));
            Result[7].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 57, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[8].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 58, 0));
            Result[8].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 58, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[9].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 59, 0));
            Result[9].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 59, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[10].NextDate.Should().Be(new DateTime(2021, 10, 23, 7, 0, 0));
            Result[10].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 7, 0, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[11].NextDate.Should().Be(new DateTime(2021, 10, 28, 6, 50, 0));
            Result[11].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 28, 6, 50, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));

            Calculator = new DateCalculator();
            Result = Calculator.GetNextExecutionDateRecurring(ConfigurationUS, 12);
            Result[0].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 50, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[1].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 51, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[2].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 52, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[3].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 53, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[4].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 54, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[5].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 55, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[6].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 56, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[7].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 57, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[8].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 58, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[9].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 59, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[10].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 7, 0, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[11].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 28, 6, 50, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));

            Calculator = new DateCalculator();
            Result = Calculator.GetNextExecutionDateRecurring(ConfigurationES, 12);
            Result[0].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 50, 0)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[1].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 51, 0)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[2].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 52, 0)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[3].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 53, 0)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[4].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 54, 0)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[5].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 55, 0)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[6].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 56, 0)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[7].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 57, 0)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[8].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 58, 0)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[9].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 59, 0)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[10].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 7, 0, 0)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[11].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 28, 6, 50, 0)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Daily_Frecuency_Seconds_With_End_Date_Repeated_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration ConfigurationEN = new SchedulerConfiguration()
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
                EndDate = TestDate.AddDays(6),
                Language = Language.EnglishUK
            };
            SchedulerConfiguration ConfigurationUS = new SchedulerConfiguration()
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
                EndDate = TestDate.AddDays(6),
                Language = Language.EnglishUS
            };
            SchedulerConfiguration ConfigurationES = new SchedulerConfiguration()
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
                EndDate = TestDate.AddDays(6),
                Language = Language.Espa�ol
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(ConfigurationEN, 12);
            DateTime ExpectedDateTime = new DateTime(2021, 10, 23, 6, 50, 50);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 50));
            Result[0].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 50)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 51));
            Result[1].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 51)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 52));
            Result[2].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 52)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[3].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 53));
            Result[3].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 53)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[4].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 54));
            Result[4].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 54)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[5].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 55));
            Result[5].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 55)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[6].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 56));
            Result[6].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 56)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[7].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 57));
            Result[7].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 57)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[8].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 58));
            Result[8].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 58)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[9].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 59));
            Result[9].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 59)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[10].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 31, 0));
            Result[10].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 31, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[11].Should().BeNull();

            Calculator = new DateCalculator();
            Result = Calculator.GetNextExecutionDateRecurring(ConfigurationUS, 12);
            Result[0].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 50)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[1].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 51)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[2].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 52)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[3].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 53)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[4].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 54)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[5].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 55)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[6].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 56)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[7].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 57)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[8].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 58)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[9].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 59)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[10].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 31, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));

            Calculator = new DateCalculator();
            Result = Calculator.GetNextExecutionDateRecurring(ConfigurationES, 12);
            Result[0].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 30, 50)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[1].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 30, 51)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[2].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 30, 52)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[3].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 30, 53)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[4].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 30, 54)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[5].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 30, 55)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[6].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 30, 56)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[7].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 30, 57)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[8].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 30, 58)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[9].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 30, 59)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[10].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 31, 0)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Daily_Frecuency_Without_End_Date_Repeated_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration ConfigurationEN = new SchedulerConfiguration()
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
                Language = Language.EnglishUK
            };
            SchedulerConfiguration ConfigurationUS = new SchedulerConfiguration()
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
                Language = Language.EnglishUS
            };
            SchedulerConfiguration ConfigurationES = new SchedulerConfiguration()
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
                Language = Language.Espa�ol
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(ConfigurationEN, 12);
            DateTime ExpectedDateTime = new DateTime(2021, 10, 23, 6, 50, 50);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 50));
            Result[0].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 50)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 51));
            Result[1].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 51)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 52));
            Result[2].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 52)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[3].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 53));
            Result[3].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 53)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[4].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 54));
            Result[4].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 54)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[5].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 55));
            Result[5].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 55)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[6].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 56));
            Result[6].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 56)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[7].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 57));
            Result[7].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 57)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[8].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 58));
            Result[8].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 58)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[9].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 59));
            Result[9].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 59)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[10].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 31, 0));
            Result[10].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 31, 0)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[11].NextDate.Should().Be(new DateTime(2021, 10, 28, 6, 30, 50));
            Result[11].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 28, 6, 30, 50)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));

            Calculator = new DateCalculator();
            Result = Calculator.GetNextExecutionDateRecurring(ConfigurationUS, 12);
            Result[0].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 50)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[1].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 51)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[2].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 52)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[3].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 53)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[4].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 54)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[5].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 55)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[6].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 56)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[7].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 57)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[8].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 58)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[9].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 30, 59)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[10].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 23, 6, 31, 0)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));
            Result[11].Description.Should().Be($"Occurs {ConfigurationUS.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2021, 10, 28, 6, 30, 50)} starting on {ConfigurationUS.StartDate}"
                + (ConfigurationUS.EndDate.HasValue ? $" and ending on {ConfigurationUS.EndDate}" : string.Empty));


            Calculator = new DateCalculator();
            Result = Calculator.GetNextExecutionDateRecurring(ConfigurationES, 12);
            Result[0].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 30, 50)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[1].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 30, 51)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[2].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 30, 52)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[3].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 30, 53)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[4].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 30, 54)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[5].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 30, 55)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[6].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 30, 56)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[7].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 30, 57)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[8].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 30, 58)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[9].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 30, 59)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[10].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 23, 6, 31, 0)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[11].Description.Should().Be($"Ocurre Diariamente. El calendario se utilizar� el {new DateTime(2021, 10, 28, 6, 30, 50)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
        }


        #endregion

        #region Yearly

        [TestMethod]
        public void Recurring_Type_Recurring_With_Yearly_Frecuency_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration ConfigurationEN = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Yearly,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = TestDate,
                Language = Language.EnglishUK
            };
            SchedulerConfiguration ConfigurationES = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Yearly,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = TestDate,
                Language = Language.Espa�ol
            };
            DateResult Result = Calculator.GetNextExecutionDate(ConfigurationEN);
            DateTime ExpectedDateTime = new DateTime(2026, 10, 18, 15, 30, 0);
            Result.NextDate.Should().Be(ExpectedDateTime);
            Result.Description.Should().NotBeNullOrEmpty();
            Result.Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {ExpectedDateTime} starting on {ConfigurationEN.StartDate}"
               + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result = Calculator.GetNextExecutionDate(ConfigurationES);
            Result.Description.Should().Be($"Ocurre Anualmente. El calendario se utilizar� el {ExpectedDateTime} empezando el {ConfigurationES.StartDate}"
               + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Yearly_Frecuency_Repeated_Hours_With_End_Date_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration ConfigurationEN = new SchedulerConfiguration()
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
                EndDate = TestDate.AddYears(10),
                Language = Language.EnglishUK
            };
            SchedulerConfiguration ConfigurationES = new SchedulerConfiguration()
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
                EndDate = TestDate.AddYears(10),
                Language = Language.Espa�ol
            };

            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(ConfigurationEN, 5);

            Result[0].NextDate.Should().Be(new DateTime(2026, 10, 18, 8, 30, 30));
            Result[0].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2026, 10, 18, 8, 30, 30)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[1].NextDate.Should().Be(new DateTime(2026, 10, 18, 9, 30, 30));
            Result[1].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2026, 10, 18, 9, 30, 30)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[2].NextDate.Should().Be(new DateTime(2026, 10, 18, 10, 30, 30));
            Result[2].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2026, 10, 18, 10, 30, 30)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[3].Should().BeNull();
            Result[4].Should().BeNull();

            Calculator = new DateCalculator();
            Result = Calculator.GetNextExecutionDateRecurring(ConfigurationES, 5);
            Result[0].Description.Should().Be($"Ocurre Anualmente. El calendario se utilizar� el {new DateTime(2026, 10, 18, 8, 30, 30)} empezando el {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" y terminando el {ConfigurationEN.EndDate}" : string.Empty));
            Result[1].Description.Should().Be($"Ocurre Anualmente. El calendario se utilizar� el {new DateTime(2026, 10, 18, 9, 30, 30)} empezando el {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" y terminando el {ConfigurationEN.EndDate}" : string.Empty));
            Result[2].Description.Should().Be($"Ocurre Anualmente. El calendario se utilizar� el {new DateTime(2026, 10, 18, 10, 30, 30)} empezando el {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" y terminando el {ConfigurationEN.EndDate}" : string.Empty));
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Yearly_Frecuency_Repeated_Hours_Without_End_Date_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration ConfigurationEN = new SchedulerConfiguration()
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
                Language = Language.EnglishUK
            };
            SchedulerConfiguration ConfigurationES = new SchedulerConfiguration()
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
                Language = Language.Espa�ol
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(ConfigurationEN, 5);

            Result[0].NextDate.Should().Be(new DateTime(2026, 10, 18, 8, 30, 30));
            Result[0].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2026, 10, 18, 8, 30, 30)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[1].NextDate.Should().Be(new DateTime(2026, 10, 18, 9, 30, 30));
            Result[1].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2026, 10, 18, 9, 30, 30)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[2].NextDate.Should().Be(new DateTime(2026, 10, 18, 10, 30, 30));
            Result[2].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2026, 10, 18, 10, 30, 30)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[3].NextDate.Should().Be(new DateTime(2031, 10, 18, 8, 30, 30));
            Result[3].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2031, 10, 18, 8, 30, 30)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[4].NextDate.Should().Be(new DateTime(2031, 10, 18, 9, 30, 30));
            Result[4].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2031, 10, 18, 9, 30, 30)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));

            Calculator = new DateCalculator();
            Result = Calculator.GetNextExecutionDateRecurring(ConfigurationES, 5);
            Result[0].Description.Should().Be($"Ocurre Anualmente. El calendario se utilizar� el {new DateTime(2026, 10, 18, 8, 30, 30)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[1].Description.Should().Be($"Ocurre Anualmente. El calendario se utilizar� el {new DateTime(2026, 10, 18, 9, 30, 30)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[2].Description.Should().Be($"Ocurre Anualmente. El calendario se utilizar� el {new DateTime(2026, 10, 18, 10, 30, 30)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[3].Description.Should().Be($"Ocurre Anualmente. El calendario se utilizar� el {new DateTime(2031, 10, 18, 8, 30, 30)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[4].Description.Should().Be($"Ocurre Anualmente. El calendario se utilizar� el {new DateTime(2031, 10, 18, 9, 30, 30)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Yearly_Frecuency_Repeated_Minutes_With_End_Date_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration ConfigurationEN = new SchedulerConfiguration()
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
                EndDate = TestDate.AddYears(10),
                Language = Language.EnglishUK
            };
            SchedulerConfiguration ConfigurationES = new SchedulerConfiguration()
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
                EndDate = TestDate.AddYears(10),
                Language = Language.Espa�ol
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(ConfigurationEN, 5);

            Result[0].NextDate.Should().Be(new DateTime(2026, 10, 18, 8, 30, 30));
            Result[0].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2026, 10, 18, 8, 30, 30)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[1].NextDate.Should().Be(new DateTime(2026, 10, 18, 8, 31, 30));
            Result[1].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2026, 10, 18, 8, 31, 30)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[2].NextDate.Should().Be(new DateTime(2026, 10, 18, 8, 32, 30));
            Result[2].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2026, 10, 18, 8, 32, 30)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[3].Should().BeNull();
            Result[4].Should().BeNull();

            Calculator = new DateCalculator();
            Result = Calculator.GetNextExecutionDateRecurring(ConfigurationES, 5);
            Result[0].Description.Should().Be($"Ocurre Anualmente. El calendario se utilizar� el {new DateTime(2026, 10, 18, 8, 30, 30)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[1].Description.Should().Be($"Ocurre Anualmente. El calendario se utilizar� el {new DateTime(2026, 10, 18, 8, 31, 30)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[2].Description.Should().Be($"Ocurre Anualmente. El calendario se utilizar� el {new DateTime(2026, 10, 18, 8, 32, 30)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Yearly_Frecuency_Repeated_Minutes_Without_End_Date_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration ConfigurationEN = new SchedulerConfiguration()
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
                Language = Language.EnglishUK
            };
            SchedulerConfiguration ConfigurationES = new SchedulerConfiguration()
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
                Language = Language.Espa�ol
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(ConfigurationEN, 5);

            Result[0].NextDate.Should().Be(new DateTime(2026, 10, 18, 8, 30, 30));
            Result[0].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2026, 10, 18, 8, 30, 30)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[1].NextDate.Should().Be(new DateTime(2026, 10, 18, 8, 31, 30));
            Result[1].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2026, 10, 18, 8, 31, 30)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[2].NextDate.Should().Be(new DateTime(2026, 10, 18, 8, 32, 30));
            Result[2].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2026, 10, 18, 8, 32, 30)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[3].NextDate.Should().Be(new DateTime(2031, 10, 18, 8, 30, 30));
            Result[3].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2031, 10, 18, 8, 30, 30)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[4].NextDate.Should().Be(new DateTime(2031, 10, 18, 8, 31, 30));
            Result[4].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2031, 10, 18, 8, 31, 30)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));

            Calculator = new DateCalculator();
            Result = Calculator.GetNextExecutionDateRecurring(ConfigurationES, 5);
            Result[0].Description.Should().Be($"Ocurre Anualmente. El calendario se utilizar� el {new DateTime(2026, 10, 18, 8, 30, 30)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[1].Description.Should().Be($"Ocurre Anualmente. El calendario se utilizar� el {new DateTime(2026, 10, 18, 8, 31, 30)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[2].Description.Should().Be($"Ocurre Anualmente. El calendario se utilizar� el {new DateTime(2026, 10, 18, 8, 32, 30)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[3].Description.Should().Be($"Ocurre Anualmente. El calendario se utilizar� el {new DateTime(2031, 10, 18, 8, 30, 30)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[4].Description.Should().Be($"Ocurre Anualmente. El calendario se utilizar� el {new DateTime(2031, 10, 18, 8, 31, 30)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Yearly_Frecuency_Repeated_Seconds_With_End_Date_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration ConfigurationEN = new SchedulerConfiguration()
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
                EndDate = TestDate.AddYears(10),
                Language = Language.EnglishUK
            };
            SchedulerConfiguration ConfigurationES = new SchedulerConfiguration()
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
                EndDate = TestDate.AddYears(10),
                Language = Language.Espa�ol
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(ConfigurationEN, 5);

            Result[0].NextDate.Should().Be(new DateTime(2026, 10, 18, 8, 30, 30));
            Result[0].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2026, 10, 18, 8, 30, 30)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[1].NextDate.Should().Be(new DateTime(2026, 10, 18, 8, 30, 31));
            Result[1].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2026, 10, 18, 8, 30, 31)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[2].NextDate.Should().Be(new DateTime(2026, 10, 18, 8, 30, 32));
            Result[2].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2026, 10, 18, 8, 30, 32)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[3].Should().BeNull();
            Result[4].Should().BeNull();

            Calculator = new DateCalculator();
            Result = Calculator.GetNextExecutionDateRecurring(ConfigurationES, 5);
            Result[0].Description.Should().Be($"Ocurre Anualmente. El calendario se utilizar� el {new DateTime(2026, 10, 18, 8, 30, 30)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[1].Description.Should().Be($"Ocurre Anualmente. El calendario se utilizar� el {new DateTime(2026, 10, 18, 8, 30, 31)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[2].Description.Should().Be($"Ocurre Anualmente. El calendario se utilizar� el {new DateTime(2026, 10, 18, 8, 30, 32)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));

        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Yearly_Frecuency_Repeated_Seconds_Without_End_Date_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            SchedulerConfiguration ConfigurationEN = new SchedulerConfiguration()
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
                Language = Language.EnglishUK
            };
            SchedulerConfiguration ConfigurationES = new SchedulerConfiguration()
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
                Language = Language.Espa�ol
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(ConfigurationEN, 5);

            Result[0].NextDate.Should().Be(new DateTime(2026, 10, 18, 8, 30, 30));
            Result[0].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2026, 10, 18, 8, 30, 30)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[1].NextDate.Should().Be(new DateTime(2026, 10, 18, 8, 30, 31));
            Result[1].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2026, 10, 18, 8, 30, 31)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[2].NextDate.Should().Be(new DateTime(2026, 10, 18, 8, 30, 32));
            Result[2].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2026, 10, 18, 8, 30, 32)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[3].NextDate.Should().Be(new DateTime(2031, 10, 18, 8, 30, 30));
            Result[3].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2031, 10, 18, 8, 30, 30)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));
            Result[4].NextDate.Should().Be(new DateTime(2031, 10, 18, 8, 30, 31));
            Result[4].Description.Should().Be($"Occurs {ConfigurationEN.SchedulerFrecuency.ToString()}. Schedule will be used on {new DateTime(2031, 10, 18, 8, 30, 31)} starting on {ConfigurationEN.StartDate}"
                + (ConfigurationEN.EndDate.HasValue ? $" and ending on {ConfigurationEN.EndDate}" : string.Empty));

            Calculator = new DateCalculator();
            Result = Calculator.GetNextExecutionDateRecurring(ConfigurationES, 5);
            Result[0].Description.Should().Be($"Ocurre Anualmente. El calendario se utilizar� el {new DateTime(2026, 10, 18, 8, 30, 30)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[1].Description.Should().Be($"Ocurre Anualmente. El calendario se utilizar� el {new DateTime(2026, 10, 18, 8, 30, 31)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[2].Description.Should().Be($"Ocurre Anualmente. El calendario se utilizar� el {new DateTime(2026, 10, 18, 8, 30, 32)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[3].Description.Should().Be($"Ocurre Anualmente. El calendario se utilizar� el {new DateTime(2031, 10, 18, 8, 30, 30)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));
            Result[4].Description.Should().Be($"Ocurre Anualmente. El calendario se utilizar� el {new DateTime(2031, 10, 18, 8, 30, 31)} empezando el {ConfigurationES.StartDate}"
                + (ConfigurationES.EndDate.HasValue ? $" y terminando el {ConfigurationES.EndDate}" : string.Empty));

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
            Configuration.Language = Language.Espa�ol;
            Configuration.Invoking(e => Calculator.GetNextExecutionDate(Configuration)).Should().Throw<SchedulerException>()
                .WithMessage("Si se selecciona la frecuencia semanal debe indicarse una frecuencia y seleccionar al menos un d�a de la semana");
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
            Configuration.Language = Language.Espa�ol;
            Configuration.Invoking(e => Calculator.GetNextExecutionDate(Configuration)).Should().Throw<SchedulerException>()
                .WithMessage("La frecuencia no puede ser negativa ni exceder los valores m�ximo o m�nimo de los enteros");

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
            Configuration.Language = Language.Espa�ol;
            Configuration.Invoking(e => Calculator.GetNextExecutionDate(Configuration)).Should().Throw<SchedulerException>()
                .WithMessage("Si se selecciona la frecuencia semanal debe indicarse una frecuencia y seleccionar al menos un d�a de la semana");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Weekly_Frecuency_On_Mondays_Once_Daily_Without_End_Date_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DayOfWeek[] Days = new DayOfWeek[] { DayOfWeek.Monday };
            SchedulerConfiguration ConfigurationEN = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Weekly,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = DateTime.Today,
                WeekDays = Days,
                WeekFrecuency = 5,
                Language = Language.EnglishUK
            };
            SchedulerConfiguration ConfigurationES = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Weekly,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = DateTime.Today,
                WeekDays = Days,
                WeekFrecuency = 5,
                Language = Language.Espa�ol
            };

            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(ConfigurationEN, 5);

            Result[0].NextDate.Should().Be(new DateTime(2021, 11, 22, 15, 30, 0));
            Result[0].Description.Should().Be("Occurs every 5 weeks on Monday at 15:30:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 12, 27, 15, 30, 0));
            Result[2].NextDate.Should().Be(new DateTime(2022, 1, 31, 15, 30, 0));
            Result[3].NextDate.Should().Be(new DateTime(2022, 3, 7, 15, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2022, 4, 11, 15, 30, 0));

            Calculator = new DateCalculator();
            Result = Calculator.GetNextExecutionDateRecurring(ConfigurationES, 5);
            Result[0].Description.Should().Be("Ocurre cada 5 semanas los Lunes a las 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Weekly_Frecuency_On_Mondays_Once_Daily_With_End_Date_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DayOfWeek[] Days = new DayOfWeek[] { DayOfWeek.Monday };
            SchedulerConfiguration ConfigurationEN = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Weekly,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = DateTime.Today,
                EndDate = new DateTime(2022, 2, 1),
                WeekDays = Days,
                WeekFrecuency = 5,
                Language = Language.EnglishUK
            };
            SchedulerConfiguration ConfigurationES = new SchedulerConfiguration()
            {
                CurrentDate = TestDate,
                Type = RecurringType.Recurring,
                SchedulerFrecuency = SchedulerFrecuency.Weekly,
                Frecuency = 5,
                OccursOnceDaily = true,
                DailyHour = new TimeSpan(15, 30, 0),
                StartDate = DateTime.Today,
                EndDate = new DateTime(2022, 2, 1),
                WeekDays = Days,
                WeekFrecuency = 5,
                Language = Language.Espa�ol
            };
            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(ConfigurationEN, 5);

            Result[0].NextDate.Should().Be(new DateTime(2021, 11, 22, 15, 30, 0));
            Result[0].Description.Should().Be("Occurs every 5 weeks on Monday at 15:30:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 12, 27, 15, 30, 0));
            Result[2].NextDate.Should().Be(new DateTime(2022, 1, 31, 15, 30, 0));
            Result[3].Should().BeNull();
            Result[4].Should().BeNull();

            Calculator = new DateCalculator();
            Result = Calculator.GetNextExecutionDateRecurring(ConfigurationES, 5);
            Result[0].Description.Should().Be("Ocurre cada 5 semanas los Lunes a las 15:30:00");
        }
        [TestMethod]
        public void Recurring_Type_Recurring_With_Weekly_Frecuency_On_Mondays_Repeated_Hourly_Daily_Should_Return_Object()
        {
            DateTime TestDate = new DateTime(2021, 10, 18);
            DateCalculator Calculator = new DateCalculator();
            DayOfWeek[] Days = new DayOfWeek[] { DayOfWeek.Monday };
            SchedulerConfiguration ConfigurationEN = new SchedulerConfiguration()
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
                WeekFrecuency = 5,
                Language = Language.EnglishUK
            };
            SchedulerConfiguration ConfigurationUS = new SchedulerConfiguration()
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
                WeekFrecuency = 5,
                Language = Language.EnglishUS
            };
            SchedulerConfiguration ConfigurationES = new SchedulerConfiguration()
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
                WeekFrecuency = 5,
                Language = Language.Espa�ol
            };

            DateResult[] Result = Calculator.GetNextExecutionDateRecurring(ConfigurationEN, 10);

            Result[0].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 30, 0));
            Result[0].Description.Should().Be("Occurs every 5 weeks on Monday every 1 Hours between 06:30:00 and 08:30:00 starting on 18/10/2021 0:00:00");
            Result[1].NextDate.Should().Be(new DateTime(2021, 10, 18, 7, 30, 0));
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 11, 22, 6, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2021, 11, 22, 7, 30, 0));
            Result[5].NextDate.Should().Be(new DateTime(2021, 11, 22, 8, 30, 0));
            Result[6].NextDate.Should().Be(new DateTime(2021, 12, 27, 6, 30, 0));
            Result[7].NextDate.Should().Be(new DateTime(2021, 12, 27, 7, 30, 0));
            Result[8].NextDate.Should().Be(new DateTime(2021, 12, 27, 8, 30, 0));
            Result[9].NextDate.Should().Be(new DateTime(2022, 1, 31, 6, 30, 0));

            Calculator = new DateCalculator();
            Result = Calculator.GetNextExecutionDateRecurring(ConfigurationUS, 5);
            Result[0].Description.Should().Be("Occurs every 5 weeks on Monday every 1 Hours between 06:30:00 and 08:30:00 starting on 10/18/2021 0:00:00");

            Calculator = new DateCalculator();
            Result = Calculator.GetNextExecutionDateRecurring(ConfigurationES, 5);
            Result[0].Description.Should().Be("Ocurre cada 5 semanas los Lunes cada 1 Horas entre el 06:30:00 y el 08:30:00 comenzando el 18/10/2021 0:00:00");

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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 19, 6, 32, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 11, 23, 6, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2021, 11, 23, 6, 31, 0));
            Result[5].NextDate.Should().Be(new DateTime(2021, 11, 23, 6, 32, 0));
            Result[6].NextDate.Should().Be(new DateTime(2021, 12, 28, 6, 30, 0));
            Result[7].NextDate.Should().Be(new DateTime(2021, 12, 28, 6, 31, 0));
            Result[8].NextDate.Should().Be(new DateTime(2021, 12, 28, 6, 32, 0));
            Result[9].NextDate.Should().Be(new DateTime(2022, 2, 1, 6, 30, 0));

            Configuration.Language = Language.EnglishUS;
            Calculator = new DateCalculator();
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 5);
            Result[0].Description.Should().Be("Occurs every 5 weeks on Tuesday every 1 Minutes between 06:30:00 and 06:32:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Calculator = new DateCalculator();
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 5);
            Result[0].Description.Should().Be("Ocurre cada 5 semanas los Martes cada 1 Minutos entre el 06:30:00 y el 06:32:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 20, 6, 31, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 11, 24, 6, 30, 58));
            Result[4].NextDate.Should().Be(new DateTime(2021, 11, 24, 6, 30, 59));
            Result[5].NextDate.Should().Be(new DateTime(2021, 11, 24, 6, 31, 0));
            Result[6].NextDate.Should().Be(new DateTime(2021, 12, 29, 6, 30, 58));
            Result[7].NextDate.Should().Be(new DateTime(2021, 12, 29, 6, 30, 59));
            Result[8].NextDate.Should().Be(new DateTime(2021, 12, 29, 6, 31, 0));
            Result[9].NextDate.Should().Be(new DateTime(2022, 2, 2, 6, 30, 58));

            Configuration.Language = Language.EnglishUS;
            Calculator = new DateCalculator();
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 5);
            Result[0].Description.Should().Be("Occurs every 5 weeks on Wednesday every 1 Seconds between 06:30:58 and 06:31:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Calculator = new DateCalculator();
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 5);
            Result[0].Description.Should().Be("Ocurre cada 5 semanas los Mi�rcoles cada 1 Segundos entre el 06:30:58 y el 06:31:00 comenzando el 18/10/2021 0:00:00");
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

            Configuration.Language = Language.Espa�ol;
            Calculator = new DateCalculator();
            Result = Calculator.GetNextExecutionDate(Configuration);
            Result.Description.Should().Be("Ocurre cada 5 semanas los Lunes y Viernes a las 15:30:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 10, 22, 6, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2021, 10, 22, 7, 30, 0));
            Result[5].NextDate.Should().Be(new DateTime(2021, 10, 22, 8, 30, 0));
            Result[6].NextDate.Should().Be(new DateTime(2021, 11, 22, 6, 30, 0));
            Result[7].NextDate.Should().Be(new DateTime(2021, 11, 22, 7, 30, 0));
            Result[8].NextDate.Should().Be(new DateTime(2021, 11, 22, 8, 30, 0));
            Result[9].NextDate.Should().Be(new DateTime(2021, 11, 26, 6, 30, 0));
            Result[10].NextDate.Should().Be(new DateTime(2021, 11, 26, 7, 30, 0));
            Result[11].NextDate.Should().Be(new DateTime(2021, 11, 26, 8, 30, 0));
            Result[12].NextDate.Should().Be(new DateTime(2021, 12, 27, 6, 30, 0));
            Result[13].NextDate.Should().Be(new DateTime(2021, 12, 27, 7, 30, 0));
            Result[14].NextDate.Should().Be(new DateTime(2021, 12, 27, 8, 30, 0));

            Configuration.Language = Language.EnglishUS;
            Calculator = new DateCalculator();
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 5);
            Result[0].Description.Should().Be("Occurs every 5 weeks on Monday and Friday every 1 Hours between 06:30:00 and 08:30:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Calculator = new DateCalculator();
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 5);
            Result[0].Description.Should().Be("Ocurre cada 5 semanas los Lunes y Viernes cada 1 Horas entre el 06:30:00 y el 08:30:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 19, 6, 32, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 10, 21, 6, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2021, 10, 21, 6, 31, 0));
            Result[5].NextDate.Should().Be(new DateTime(2021, 10, 21, 6, 32, 0));
            Result[6].NextDate.Should().Be(new DateTime(2021, 11, 23, 6, 30, 0));
            Result[7].NextDate.Should().Be(new DateTime(2021, 11, 23, 6, 31, 0));
            Result[8].NextDate.Should().Be(new DateTime(2021, 11, 23, 6, 32, 0));
            Result[9].NextDate.Should().Be(new DateTime(2021, 11, 25, 6, 30, 0));
            Result[10].NextDate.Should().Be(new DateTime(2021, 11, 25, 6, 31, 0));
            Result[11].NextDate.Should().Be(new DateTime(2021, 11, 25, 6, 32, 0));
            Result[12].NextDate.Should().Be(new DateTime(2021, 12, 28, 6, 30, 0));
            Result[13].NextDate.Should().Be(new DateTime(2021, 12, 28, 6, 31, 0));
            Result[14].NextDate.Should().Be(new DateTime(2021, 12, 28, 6, 32, 0));

            Configuration.Language = Language.EnglishUS;
            Calculator = new DateCalculator();
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 5);
            Result[0].Description.Should().Be("Occurs every 5 weeks on Tuesday and Thursday every 1 Minutes between 06:30:00 and 06:32:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Calculator = new DateCalculator();
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 5);
            Result[0].Description.Should().Be("Ocurre cada 5 semanas los Martes y Jueves cada 1 Minutos entre el 06:30:00 y el 06:32:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 20, 6, 31, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 58));
            Result[4].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 30, 59));
            Result[5].NextDate.Should().Be(new DateTime(2021, 10, 23, 6, 31, 0));
            Result[6].NextDate.Should().Be(new DateTime(2021, 11, 24, 6, 30, 58));
            Result[7].NextDate.Should().Be(new DateTime(2021, 11, 24, 6, 30, 59));
            Result[8].NextDate.Should().Be(new DateTime(2021, 11, 24, 6, 31, 0));
            Result[9].NextDate.Should().Be(new DateTime(2021, 11, 27, 6, 30, 58));
            Result[10].NextDate.Should().Be(new DateTime(2021, 11, 27, 6, 30, 59));
            Result[11].NextDate.Should().Be(new DateTime(2021, 11, 27, 6, 31, 0));
            Result[12].NextDate.Should().Be(new DateTime(2021, 12, 29, 6, 30, 58));
            Result[13].NextDate.Should().Be(new DateTime(2021, 12, 29, 6, 30, 59));
            Result[14].NextDate.Should().Be(new DateTime(2021, 12, 29, 6, 31, 0));

            Configuration.Language = Language.EnglishUS;
            Calculator = new DateCalculator();
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 5);
            Result[0].Description.Should().Be("Occurs every 5 weeks on Wednesday and Saturday every 1 Seconds between 06:30:58 and 06:31:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Calculator = new DateCalculator();
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 5);
            Result[0].Description.Should().Be("Ocurre cada 5 semanas los Mi�rcoles y S�bado cada 1 Segundos entre el 06:30:58 y el 06:31:00 comenzando el 18/10/2021 0:00:00");
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
            Configuration.Language = Language.Espa�ol;
            Configuration.Invoking(e => Calculator.GetNextExecutionDate(Configuration)).Should().Throw<SchedulerException>()
                .WithMessage("Debe indicarse una frecuencia mensual positiva");
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
            Configuration.Language = Language.Espa�ol;
            Configuration.Invoking(e => Calculator.GetNextExecutionDate(Configuration)).Should().Throw<SchedulerException>()
                .WithMessage("Debe indicarse un d�a si se selecciona frecuencia mensual");
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
            Configuration.Language = Language.Espa�ol;
            Configuration.Invoking(e => Calculator.GetNextExecutionDate(Configuration)).Should().Throw<SchedulerException>()
                .WithMessage("Debe indicarse un d�a si se selecciona frecuencia mensual");
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
            Configuration.Language = Language.Espa�ol;
            Configuration.Invoking(e => Calculator.GetNextExecutionDate(Configuration)).Should().Throw<SchedulerException>()
                .WithMessage("Debe indicarse un d�a si se selecciona frecuencia mensual");
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
            Result[2].NextDate.Should().Be(new DateTime(2022, 8, 18, 15, 30, 0));
            Result[3].NextDate.Should().Be(new DateTime(2023, 1, 18, 15, 30, 0));

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el 18 de cada 5 meses a las 15:30:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 10, 6, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 10, 7, 30, 0));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 10, 8, 30, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 10, 6, 30, 0));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the 10th of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el 10 de cada 2 meses cada 1 Horas entre el 06:30:00 y el 08:30:00 comenzando el 18/10/2021 0:00:00");

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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 32, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 10, 6, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 10, 6, 31, 0));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 10, 6, 32, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 10, 6, 30, 0));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the 10th of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el 10 de cada 2 meses cada 1 Minutos entre el 06:30:00 y el 06:32:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 31, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 10, 8, 30, 58));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 10, 8, 30, 59));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 10, 8, 31, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 10, 8, 30, 58));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the 10th of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el 10 de cada 2 meses cada 1 Segundos entre el 08:30:58 y el 08:31:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2022, 2, 1, 15, 30, 0));

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el Primer Martes de cada 2 meses a las 15:30:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 7, 6, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 7, 7, 30, 0));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 7, 8, 30, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 1, 6, 30, 0));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the First Tuesday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el Primer Martes de cada 2 meses cada 1 Horas entre el 06:30:00 y el 08:30:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 32, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 7, 6, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 7, 6, 31, 0));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 7, 6, 32, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 1, 6, 30, 0));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the First Tuesday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el Primer Martes de cada 2 meses cada 1 Minutos entre el 06:30:00 y el 06:32:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 31, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 7, 8, 30, 58));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 7, 8, 30, 59));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 7, 8, 31, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 1, 8, 30, 58));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the First Tuesday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el Primer Martes de cada 2 meses cada 1 Segundos entre el 08:30:58 y el 08:31:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2022, 2, 26, 15, 30, 0));

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el �ltimo S�bado de cada 2 meses a las 15:30:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 25, 6, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 25, 7, 30, 0));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 25, 8, 30, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 26, 6, 30, 0));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the Last Saturday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el �ltimo S�bado de cada 2 meses cada 1 Horas entre el 06:30:00 y el 08:30:00 comenzando el 18/10/2021 0:00:00");

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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 32, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 25, 6, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 25, 6, 31, 0));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 25, 6, 32, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 26, 6, 30, 0));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the Last Saturday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el �ltimo S�bado de cada 2 meses cada 1 Minutos entre el 06:30:00 y el 06:32:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 31, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 25, 8, 30, 58));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 25, 8, 30, 59));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 25, 8, 31, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 26, 8, 30, 58));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the Last Saturday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el �ltimo S�bado de cada 2 meses cada 1 Segundos entre el 08:30:58 y el 08:31:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2022, 2, 2, 15, 30, 0));

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el Segundo D�a de la semana de cada 2 meses a las 15:30:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 2, 6, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 2, 7, 30, 0));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 2, 8, 30, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 2, 6, 30, 0));


            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the Second Weekday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el Segundo D�a de la semana de cada 2 meses cada 1 Horas entre el 06:30:00 y el 08:30:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 32, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 2, 6, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 2, 6, 31, 0));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 2, 6, 32, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 2, 6, 30, 0));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the Second Weekday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el Segundo D�a de la semana de cada 2 meses cada 1 Minutos entre el 06:30:00 y el 06:32:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 31, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 2, 8, 30, 58));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 2, 8, 30, 59));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 2, 8, 31, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 2, 8, 30, 58));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the Second Weekday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el Segundo D�a de la semana de cada 2 meses cada 1 Segundos entre el 08:30:58 y el 08:31:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2022, 2, 6, 15, 30, 0));

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el Segundo D�a del fin de semana de cada 2 meses a las 15:30:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 5, 6, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 5, 7, 30, 0));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 5, 8, 30, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 6, 6, 30, 0));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the Second Weekendday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el Segundo D�a del fin de semana de cada 2 meses cada 1 Horas entre el 06:30:00 y el 08:30:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 32, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 5, 6, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 5, 6, 31, 0));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 5, 6, 32, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 6, 6, 30, 0));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the Second Weekendday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el Segundo D�a del fin de semana de cada 2 meses cada 1 Minutos entre el 06:30:00 y el 06:32:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 31, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 5, 8, 30, 58));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 5, 8, 30, 59));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 5, 8, 31, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 6, 8, 30, 58));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the Second Weekendday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el Segundo D�a del fin de semana de cada 2 meses cada 1 Segundos entre el 08:30:58 y el 08:31:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2022, 2, 12, 15, 30, 0));

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el Tercer D�a del fin de semana de cada 2 meses a las 15:30:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 11, 6, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 11, 7, 30, 0));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 11, 8, 30, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 12, 6, 30, 0));
            
            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the Third Weekendday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el Tercer D�a del fin de semana de cada 2 meses cada 1 Horas entre el 06:30:00 y el 08:30:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 32, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 11, 6, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 11, 6, 31, 0));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 11, 6, 32, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 12, 6, 30, 0));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the Third Weekendday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el Tercer D�a del fin de semana de cada 2 meses cada 1 Minutos entre el 06:30:00 y el 06:32:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 31, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 11, 8, 30, 58));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 11, 8, 30, 59));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 11, 8, 31, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 12, 8, 30, 58));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the Third Weekendday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el Tercer D�a del fin de semana de cada 2 meses cada 1 Segundos entre el 08:30:58 y el 08:31:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2022, 2, 25, 15, 30, 0));

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el Cuarto Viernes de cada 2 meses a las 15:30:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 24, 6, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 24, 7, 30, 0));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 24, 8, 30, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 25, 6, 30, 0));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the Fourth Friday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el Cuarto Viernes de cada 2 meses cada 1 Horas entre el 06:30:00 y el 08:30:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 32, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 24, 6, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 24, 6, 31, 0));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 24, 6, 32, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 25, 6, 30, 0));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the Fourth Friday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el Cuarto Viernes de cada 2 meses cada 1 Minutos entre el 06:30:00 y el 06:32:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 31, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 24, 8, 30, 58));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 24, 8, 30, 59));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 24, 8, 31, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 25, 8, 30, 58));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the Fourth Friday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el Cuarto Viernes de cada 2 meses cada 1 Segundos entre el 08:30:58 y el 08:31:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2022, 2, 28, 15, 30, 0));

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el �ltimo Lunes de cada 2 meses a las 15:30:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 27, 6, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 27, 7, 30, 0));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 27, 8, 30, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 28, 6, 30, 0));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the Last Monday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el �ltimo Lunes de cada 2 meses cada 1 Horas entre el 06:30:00 y el 08:30:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 32, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 27, 6, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 27, 6, 31, 0));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 27, 6, 32, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 28, 6, 30, 0));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the Last Monday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el �ltimo Lunes de cada 2 meses cada 1 Minutos entre el 06:30:00 y el 06:32:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 31, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 27, 8, 30, 58));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 27, 8, 30, 59));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 27, 8, 31, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 28, 8, 30, 58));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the Last Monday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el �ltimo Lunes de cada 2 meses cada 1 Segundos entre el 08:30:58 y el 08:31:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2022, 2, 2, 15, 30, 0));

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el Primer Mi�rcoles de cada 2 meses a las 15:30:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 1, 6, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 1, 7, 30, 0));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 1, 8, 30, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 2, 6, 30, 0));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the First Wednesday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el Primer Mi�rcoles de cada 2 meses cada 1 Horas entre el 06:30:00 y el 08:30:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 32, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 1, 6, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 1, 6, 31, 0));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 1, 6, 32, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 2, 6, 30, 0));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the First Wednesday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el Primer Mi�rcoles de cada 2 meses cada 1 Minutos entre el 06:30:00 y el 06:32:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 31, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 1, 8, 30, 58));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 1, 8, 30, 59));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 1, 8, 31, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 2, 8, 30, 58));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the First Wednesday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el Primer Mi�rcoles de cada 2 meses cada 1 Segundos entre el 08:30:58 y el 08:31:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2022, 2, 10, 15, 30, 0));

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el Segundo Jueves de cada 2 meses a las 15:30:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 9, 6, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 9, 7, 30, 0));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 9, 8, 30, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 10, 6, 30, 0));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the Second Thursday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el Segundo Jueves de cada 2 meses cada 1 Horas entre el 06:30:00 y el 08:30:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 32, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 9, 6, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 9, 6, 31, 0));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 9, 6, 32, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 10, 6, 30, 0));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the Second Thursday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el Segundo Jueves de cada 2 meses cada 1 Minutos entre el 06:30:00 y el 06:32:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 31, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 9, 8, 30, 58));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 9, 8, 30, 59));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 9, 8, 31, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 10, 8, 30, 58));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the Second Thursday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el Segundo Jueves de cada 2 meses cada 1 Segundos entre el 08:30:58 y el 08:31:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2022, 2, 20, 15, 30, 0));

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el Tercer Domingo de cada 2 meses a las 15:30:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 19, 6, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 19, 7, 30, 0));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 19, 8, 30, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 20, 6, 30, 0));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the Third Sunday of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el Tercer Domingo de cada 2 meses cada 1 Horas entre el 06:30:00 y el 08:30:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 32, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 19, 6, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 19, 6, 31, 0));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 19, 6, 32, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 20, 6, 30, 0));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the Third Sunday of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el Tercer Domingo de cada 2 meses cada 1 Minutos entre el 06:30:00 y el 06:32:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 31, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 19, 8, 30, 58));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 19, 8, 30, 59));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 19, 8, 31, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 20, 8, 30, 58));


            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the Third Sunday of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el Tercer Domingo de cada 2 meses cada 1 Segundos entre el 08:30:58 y el 08:31:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2022, 2, 28, 15, 30, 0));

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el �ltimo D�a de cada 2 meses a las 15:30:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 31, 6, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 31, 7, 30, 0));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 31, 8, 30, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 28, 6, 30, 0));


            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the Last Day of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el �ltimo D�a de cada 2 meses cada 1 Horas entre el 06:30:00 y el 08:30:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 32, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 31, 6, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 31, 6, 31, 0));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 31, 6, 32, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 28, 6, 30, 0));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the Last Day of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el �ltimo D�a de cada 2 meses cada 1 Minutos entre el 06:30:00 y el 06:32:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 31, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 31, 8, 30, 58));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 31, 8, 30, 59));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 31, 8, 31, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 28, 8, 30, 58));


            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the Last Day of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el �ltimo D�a de cada 2 meses cada 1 Segundos entre el 08:30:58 y el 08:31:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2022, 2, 1, 15, 30, 0));

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el 1 de cada 2 meses a las 15:30:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 1, 6, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 1, 7, 30, 0));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 1, 8, 30, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 1, 6, 30, 0));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the 1st of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el 1 de cada 2 meses cada 1 Horas entre el 06:30:00 y el 08:30:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 32, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 1, 6, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 1, 6, 31, 0));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 1, 6, 32, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 1, 6, 30, 0));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the 1st of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el 1 de cada 2 meses cada 1 Minutos entre el 06:30:00 y el 06:32:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 31, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 1, 8, 30, 58));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 1, 8, 30, 59));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 1, 8, 31, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 1, 8, 30, 58));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the 1st of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el 1 de cada 2 meses cada 1 Segundos entre el 08:30:58 y el 08:31:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2022, 2, 2, 15, 30, 0));


            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el 2 de cada 2 meses a las 15:30:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 2, 6, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 2, 7, 30, 0));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 2, 8, 30, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 2, 6, 30, 0));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the 2nd of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el 2 de cada 2 meses cada 1 Horas entre el 06:30:00 y el 08:30:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 32, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 2, 6, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 2, 6, 31, 0));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 2, 6, 32, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 2, 6, 30, 0));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the 2nd of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el 2 de cada 2 meses cada 1 Minutos entre el 06:30:00 y el 06:32:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 31, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 2, 8, 30, 58));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 2, 8, 30, 59));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 2, 8, 31, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 2, 8, 30, 58));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the 2nd of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el 2 de cada 2 meses cada 1 Segundos entre el 08:30:58 y el 08:31:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2022, 2, 3, 15, 30, 0));

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el 3 de cada 2 meses a las 15:30:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 3, 6, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 3, 7, 30, 0));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 3, 8, 30, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 3, 6, 30, 0));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the 3rd of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el 3 de cada 2 meses cada 1 Horas entre el 06:30:00 y el 08:30:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 32, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 3, 6, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 3, 6, 31, 0));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 3, 6, 32, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 3, 6, 30, 0));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the 3rd of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el 3 de cada 2 meses cada 1 Minutos entre el 06:30:00 y el 06:32:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 31, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 3, 8, 30, 58));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 3, 8, 30, 59));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 3, 8, 31, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 3, 8, 30, 58));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the 3rd of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el 3 de cada 2 meses cada 1 Segundos entre el 08:30:58 y el 08:31:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2022, 2, 28, 15, 30, 0));

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el 31 de cada 2 meses a las 15:30:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 30, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 31, 6, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 31, 7, 30, 0));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 31, 8, 30, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 28, 6, 30, 0));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the 31th of every 2 months every 1 Hours between 06:30:00 and 08:30:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el 31 de cada 2 meses cada 1 Horas entre el 06:30:00 y el 08:30:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 6, 32, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 31, 6, 30, 0));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 31, 6, 31, 0));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 31, 6, 32, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 28, 6, 30, 0));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the 31th of every 2 months every 1 Minutes between 06:30:00 and 06:32:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el 31 de cada 2 meses cada 1 Minutos entre el 06:30:00 y el 06:32:00 comenzando el 18/10/2021 0:00:00");
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
            Result[2].NextDate.Should().Be(new DateTime(2021, 10, 18, 8, 31, 0));
            Result[3].NextDate.Should().Be(new DateTime(2021, 12, 31, 8, 30, 58));
            Result[4].NextDate.Should().Be(new DateTime(2021, 12, 31, 8, 30, 59));
            Result[5].NextDate.Should().Be(new DateTime(2021, 12, 31, 8, 31, 0));
            Result[6].NextDate.Should().Be(new DateTime(2022, 2, 28, 8, 30, 58));

            Configuration.Language = Language.EnglishUS;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Occurs the 31th of every 2 months every 1 Seconds between 08:30:58 and 08:31:00 starting on 10/18/2021 0:00:00");

            Configuration.Language = Language.Espa�ol;
            Result = Calculator.GetNextExecutionDateRecurring(Configuration, 4);
            Result[0].Description.Should().Be("Ocurre el 31 de cada 2 meses cada 1 Segundos entre el 08:30:58 y el 08:31:00 comenzando el 18/10/2021 0:00:00");
        }
        #endregion
    }
}
