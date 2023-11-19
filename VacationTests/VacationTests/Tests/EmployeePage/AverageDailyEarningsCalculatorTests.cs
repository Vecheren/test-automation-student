using System;
using System.Linq;
using System.Reflection;
using Kontur.Selone.Extensions;
using NUnit.Framework;
using VacationTests.Infrastructure;
using VacationTests.Infrastructure.PageElements;

namespace VacationTests.Tests.EmployeePage
{
    public class AverageDailyEarningsCalculatorTests : VacationTestBase
    {
        [Test]
        public void SmokyTest()
        {
            var calculatorTabPage = Navigation.OpenAverageDailyEarningsCalculatorPage();
            calculatorTabPage.AverageSalaryRowFirst.YearSelect.WaitVisible();
            calculatorTabPage.AverageSalaryRowFirst.SalaryCurrencyInput.WaitVisible();
            calculatorTabPage.AverageSalaryRowSecond.YearSelect.WaitVisible();
            calculatorTabPage.AverageSalaryRowSecond.SalaryCurrencyInput.WaitVisible();
            calculatorTabPage.AverageDailyEarningsCurrencyLabel.WaitVisible();
            calculatorTabPage.AverageDailyEarningsCurrencyLabel.Sum.Wait().EqualTo(370.85m);
        }

        [Test]
        public void FillAllFields_ShouldCalculateRight()
        {
            var calculatorTabPage = Navigation.OpenAverageDailyEarningsCalculatorPage();
            calculatorTabPage.AverageSalaryRowFirst.YearSelect.SelectValueByText("2020");
            calculatorTabPage.AverageSalaryRowSecond.YearSelect.SelectValueByText("2021");
            calculatorTabPage.AverageSalaryRowFirst.SalaryCurrencyInput.ClearAndInputCurrency(100000);
            calculatorTabPage.AverageSalaryRowSecond.SalaryCurrencyInput.ClearAndInputCurrency(200000);
            calculatorTabPage.CountOfExcludeDaysInput.ClearAndInputText("100");
            calculatorTabPage.AverageDailyEarningsCurrencyLabel.Sum.Wait().EqualTo(475.44m);
        }

        [Test]
        public void SalaryMoreThanBase_ShouldUseBaseForCalculations()
        {
            var calculatorTabPage = Navigation.OpenAverageDailyEarningsCalculatorPage();
            calculatorTabPage.AverageSalaryRowFirst.YearSelect.SelectValueByText("2020");
            calculatorTabPage.AverageSalaryRowFirst.SalaryCurrencyInput.ClearAndInputCurrency(2000000.00m);
            calculatorTabPage.AverageSalaryRowFirst.CountBaseCurrencyLabel.Sum.Wait().EqualTo(912000);
            calculatorTabPage.AverageSalaryRowFirst.YearSelect.SelectValueByText("2021");
            calculatorTabPage.AverageSalaryRowFirst.CountBaseCurrencyLabel.Sum.Wait().EqualTo(966000);
        }

        [Test]
        public void SalaryLessThatBase_ShouldUseSalaryForCalculations()
        {
            var calculatorTabPage = Navigation.OpenAverageDailyEarningsCalculatorPage();
            var firstSum = 100000.1m;
            var secondSum = 200000.2m;
            calculatorTabPage.AverageSalaryRowFirst.SalaryCurrencyInput.ClearAndInputCurrency(firstSum);
            calculatorTabPage.AverageSalaryRowSecond.SalaryCurrencyInput.ClearAndInputCurrency(secondSum);
            calculatorTabPage.AverageSalaryRowFirst.CountBaseCurrencyLabel.Sum.Wait().EqualTo(firstSum);
            calculatorTabPage.AverageSalaryRowSecond.CountBaseCurrencyLabel.Sum.Wait().EqualTo(secondSum);
            calculatorTabPage.TotalEarningsCurrencyLabel.Sum.Wait().EqualTo(firstSum + secondSum);
        }

        [Test]
        public void LeapYear_ShouldAddExtraDay()
        {
            var calculatorTabPage = Navigation.OpenAverageDailyEarningsCalculatorPage();
            calculatorTabPage.AverageSalaryRowFirst.YearSelect.SelectValueByText("2020");
            calculatorTabPage.AverageSalaryRowSecond.YearSelect.SelectValueByText("2021");
            calculatorTabPage.TotalDaysForCalcLabel.Text.Wait().EqualTo("731");
            calculatorTabPage.AverageSalaryRowFirst.YearSelect.SelectValueByText("2019");
            calculatorTabPage.TotalDaysForCalcLabel.Text.Wait().EqualTo("730");
        }

        [Test]
        public void ExcludeDays_ShouldExcludeFromCalculations()
        {
            var calculatorTabPage = Navigation.OpenAverageDailyEarningsCalculatorPage();
            calculatorTabPage.AverageSalaryRowFirst.YearSelect.SelectValueByText("2020");
            calculatorTabPage.AverageSalaryRowSecond.YearSelect.SelectValueByText("2021");
            calculatorTabPage.DaysInTwoYearsLabel.Text.Wait().EqualTo("731");
            calculatorTabPage.CountOfExcludeDaysInput.ClearAndInputText("100");
            calculatorTabPage.TotalDaysForCalcLabel.Text.Wait().EqualTo("631");
        }
    }
}