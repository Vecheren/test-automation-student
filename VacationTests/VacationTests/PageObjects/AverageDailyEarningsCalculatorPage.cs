using OpenQA.Selenium;
using VacationTests.Infrastructure;
using VacationTests.Infrastructure.PageElements;
using VacationTests.PageElements;

namespace VacationTests.PageObjects
{
    [InjectControlsAttribute]
    public class AverageDailyEarningsCalculatorPage : PageBase
    {
        public AverageDailyEarningsCalculatorPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        
        [ByTid("first")] public AverageSalaryRow AverageSalaryRowFirst { get; private set; }
        [ByTid("second")] public AverageSalaryRow AverageSalaryRowSecond { get; private set; }
        public CurrencyLabel AverageDailyEarningsCurrencyLabel { get; private set; }
        public Input CountOfExcludeDaysInput { get; private set; }
        public CurrencyLabel TotalEarningsCurrencyLabel { get; private set; }
        public Label TotalDaysForCalcLabel { get; private set; }
        public Label DaysInTwoYearsLabel { get; private set; }
    }
}