using Kontur.Selone.Selectors.Context;
using VacationTests.Infrastructure;
using VacationTests.Infrastructure.PageElements;

namespace VacationTests.PageElements
{
    [InjectControlsAttribute]
    public class PageFooter : ControlBase
    {
        public PageFooter(IContextBy contextBy) : base(contextBy)
        {
        }
    
        public Link KnowEnvironmentLink { get; private set; }
        public Link OurFooterLink { get; private set; }
    }
}