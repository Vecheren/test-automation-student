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
        private ControlFactory controlFactory;
        public AdminVacationListPage(IWebDriver webDriver, ControlFactory controlFactory) : base(webDriver)
        {
            this.controlFactory = controlFactory;
            // TitleLabel = controlFactory.CreateControl<Label>(webDriver.Search(x => x.WithTid("TitleLabel")));
            // ClaimsTab = controlFactory.CreateControl<Link>(webDriver.Search(x => x.WithTid("ClaimsTab")));
            // DownloadButton = controlFactory.CreateControl<Button>(webDriver.Search(x => x.WithTid("DownloadButton")));
            // Footer = controlFactory.CreateControl<PageFooter>(webDriver.Search(x => x.WithTid("Footer")));
            // ClaimList = controlFactory.CreateControl<AdminClaimList>(webDriver.Search(x => x.WithTid("ClaimList")));
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