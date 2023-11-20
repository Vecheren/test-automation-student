using System;
using Kontur.Selone.Extensions;
using OpenQA.Selenium;
using VacationTests.Infrastructure;
using VacationTests.Infrastructure.PageElements;

namespace VacationTests.PageObjects
{
    [InjectControlsAttribute]
    public class InfoSidePage : PageBase
    {
        public InfoSidePage(IWebDriver webDriver) : base(webDriver)
        {
        }

        public Button AgreeButton { get; private set; }
        [ByTid("SidePageBody")] public Label BodyLabel { get; private set; }
        public Button CloseButton { get; private set; }

        [ByTid("SidePage__close")] public Button CrossButton { get; private set; }
        [ByTid("SidePageHeader")] public Label HeaderLabel { get; private set; }
        public Button NotAgreeButton { get; private set; }
        [ByTid("InfoSidePage")] public Portal InfoSidePageModel { get; private set; }
        

        private IWebElement GetModalContext()
        {
            return InfoSidePageModel.GetPortalElement();
        }
    }
}