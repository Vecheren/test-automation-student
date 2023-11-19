using Kontur.Selone.Extensions;
using OpenQA.Selenium;
using VacationTests.Infrastructure;
using VacationTests.Infrastructure.PageElements;
using VacationTests.PageElements;

namespace VacationTests.PageObjects
{
    [InjectControlsAttribute]
    public class AverageDailyEarningsCalculatorPage : PageBase
    {
        public AverageDailyEarningsCalculatorPage(IWebDriver webDriver, ControlFactory controlFactory) : base(webDriver)
        {
            AverageDailyEarningsCurrencyLabel =
                controlFactory.CreateControl<CurrencyLabel>(
                    webDriver.Search(x => x.WithTid(nameof(AverageDailyEarningsCurrencyLabel))));
            CountOfExcludeDaysInput =
                controlFactory.CreateControl<Input>(webDriver.Search(x => x.WithTid(nameof(CountOfExcludeDaysInput))));
            TotalEarningsCurrencyLabel =
                controlFactory.CreateControl<CurrencyLabel>(webDriver.Search(x =>
                    x.WithTid(nameof(TotalEarningsCurrencyLabel))));
            AverageSalaryRowFirst =
                controlFactory.CreateControl<AverageSalaryRow>(webDriver.Search(x => x.WithTid("first")));
            AverageSalaryRowSecond =
                controlFactory.CreateControl<AverageSalaryRow>(webDriver.Search(x => x.WithTid("second")));
            TotalDaysForCalcLabel =
                controlFactory.CreateControl<Label>(webDriver.Search(x =>
                    x.WithTid(nameof(TotalDaysForCalcLabel))));
            DaysInTwoYearsLabel =
                controlFactory.CreateControl<Label>(webDriver.Search(x => x.WithTid(nameof(DaysInTwoYearsLabel))));
        }

        public AverageSalaryRow AverageSalaryRowFirst { get; }
        public AverageSalaryRow AverageSalaryRowSecond { get; }
        public CurrencyLabel AverageDailyEarningsCurrencyLabel { get; }
        public Input CountOfExcludeDaysInput { get; }
        public CurrencyLabel TotalEarningsCurrencyLabel { get; }
        public Label TotalDaysForCalcLabel { get; }
        public Label DaysInTwoYearsLabel { get; }
    }
}