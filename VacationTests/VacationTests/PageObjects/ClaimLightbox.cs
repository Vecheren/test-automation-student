using Kontur.Selone.Extensions;
using OpenQA.Selenium;
using VacationTests.Infrastructure;
using VacationTests.Infrastructure.PageElements;
using VacationTests.PageElements;

namespace VacationTests.PageObjects
{
    [InjectControlsAttribute]
    public class ClaimLightbox : PageBase
    {
        public ClaimLightbox(IWebDriver webDriver) : base(webDriver)
        {
        }

        public Portal ClaimModal { get; private set; }

        // Вариант работы с элементами, для которых нужен ленивый поиск
        public Label ModalHeader { get; private set; }
        
        public Label StatusLabel { get; private set; }

        public Label ClaimTypeLabel { get; private set; }

        public Label ChildAgeLabel { get; private set; }

        public Label PeriodLabel { get; private set; }

        public Label AvailableDaysMessageLabel { get; private set; }

        public Label AvailableDaysLabel { get; private set; }

        public Checkbox PayNowCheckbox { get; private set; }

        public Label DirectorFioLabel { get; private set; }

        [ByTid("ModalFooter")] public ClaimLightboxFooter Footer { get; private set; }
        
        private IWebElement GetModalContext()
        {
            return ClaimModal.GetPortalElement();
        }
    }
}