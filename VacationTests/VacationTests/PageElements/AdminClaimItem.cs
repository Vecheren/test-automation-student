using Kontur.Selone.Extensions;
using Kontur.Selone.Selectors.Context;
using OpenQA.Selenium;
using VacationTests.Infrastructure;
using VacationTests.Infrastructure.PageElements;

namespace VacationTests.PageElements
{
    [InjectControlsAttribute]
    public class AdminClaimItem : ControlBase
    {
        public AdminClaimItem(IContextBy contextBy, ControlFactory controlFactory) : base(contextBy)
        {
            TitleLink = controlFactory.CreateControl<Button>(Container.Search(x => x.WithTid(nameof(TitleLink))));
            PeriodLabel = controlFactory.CreateControl<Label>(Container.Search(x => x.WithTid(nameof(PeriodLabel))));
            StatusLabel = controlFactory.CreateControl<Label>(Container.Search(x => x.WithTid(nameof(StatusLabel))));
            AcceptButton = controlFactory.CreateControl<Button>(Container.Search(x => x.WithTid(nameof(AcceptButton))));
            RejectButton = controlFactory.CreateControl<Button>(Container.Search(x => x.WithTid(nameof(RejectButton))));
        }
            
        public Button TitleLink;
        public Label PeriodLabel;
        public Label StatusLabel;
        public Button AcceptButton;
        public Button RejectButton;

    }

}