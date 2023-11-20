using Kontur.Selone.Elements;
using Kontur.Selone.Selectors.Context;
using Kontur.Selone.Selectors.XPath;
using VacationTests.Infrastructure;
using VacationTests.Infrastructure.PageElements;

namespace VacationTests.PageElements
{
    public class AdminClaimList : ControlBase
    {
        public ElementsCollection<AdminClaimItem> ClaimItems { get; private set; }

        public AdminClaimList(IContextBy contextBy, ControlFactory controlFactory) : base(contextBy)
        {
            ClaimItems = controlFactory.CreateElementsCollection<AdminClaimItem>(Container,
                x => x.WithTid("ClaimItem").FixedByIndex());
        }
    }
}