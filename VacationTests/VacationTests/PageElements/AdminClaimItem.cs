using Kontur.Selone.Selectors.Context;
using VacationTests.Infrastructure;
using VacationTests.Infrastructure.PageElements;

namespace VacationTests.PageElements
{
    [InjectControlsAttribute]
    public class AdminClaimItem : ControlBase
    {
        public AdminClaimItem(IContextBy contextBy) : base(contextBy)
        {
        }

        public Button TitleLink { get; private set; }
        public Label PeriodLabel { get; private set; }
        public Label StatusLabel { get; private set; }
        public Button AcceptButton { get; private set; }
        public Button RejectButton { get; private set; }
    }
}