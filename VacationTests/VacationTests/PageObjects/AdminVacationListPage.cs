using Kontur.Selone.Extensions;
using OpenQA.Selenium;
using VacationTests.Infrastructure;
using VacationTests.Infrastructure.PageElements;
using VacationTests.PageElements;

namespace VacationTests.PageObjects
{
    [InjectControlsAttribute]
    public class AdminVacationListPage : PageBase
    {
        public AdminVacationListPage(IWebDriver webDriver) : base(webDriver)
        {
        }

        public AdminClaimList ClaimList { get; private set; }
        public Label TitleLabel { get; private set; }
        public Link ClaimsTab { get; private set; }
        public Button DownloadButton { get; private set; }
        public PageFooter Footer { get; private set; }
        public bool IsAdminPage
        {
            get
            {
                var employeeVacationListPage = new ControlFactory().CreatePage<EmployeeVacationListPage>(WrappedDriver);
                return !(employeeVacationListPage.SalaryCalculatorTab.Visible.Get()
                       && employeeVacationListPage.CreateButton.Visible.Get());
            }
        }
    }
}