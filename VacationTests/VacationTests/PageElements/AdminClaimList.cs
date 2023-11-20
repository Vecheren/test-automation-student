using Kontur.Selone.Elements;
using Kontur.Selone.Selectors.Context;
using Kontur.Selone.Selectors.XPath;
using VacationTests.Infrastructure;
using VacationTests.Infrastructure.PageElements;

namespace VacationTests.PageElements
{
    public class AdminClaimList : ControlBase
    {
        [ByTid("ClaimItem")] public ElementsCollection<AdminClaimItem> ClaimItems { get; private set; }

        public AdminClaimList(IContextBy contextBy) : base(contextBy)
        {
        }
    }
}