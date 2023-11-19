using Kontur.Selone.Selectors.Context;
using VacationTests.Infrastructure;
using VacationTests.Infrastructure.PageElements;

namespace VacationTests.PageElements
{
    public class PageFooter : ControlBase
    {
        public PageFooter(IContextBy contextBy) : base(contextBy)
        {
        }
    
        [ByTid("KnowEnvironmentLink")] public Link KnowEnvironmentLink { get; private set; }
        [ByTid("OurFooterLink")] public Link OurFooterLink { get; private set; }
    }
}