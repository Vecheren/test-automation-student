using Kontur.Selone.Elements;
using Kontur.Selone.Selectors.Context;
using Kontur.Selone.Selectors.Css;
using Kontur.Selone.Selectors.XPath;
using VacationTests.Infrastructure;
using VacationTests.Infrastructure.PageElements;

namespace VacationTests.PageElements
{
    // Класс списка наследуем от ControlBase, поскольку это тоже контрол и могут понадобиться базовые методы и пропсы
    public class EmployeeClaimList : ControlBase
    {
        public EmployeeClaimList(IContextBy contextBy) : base(contextBy)
        {
        }

        [ByTid("ClaimItem")] public ElementsCollection<EmployeeClaimItem> Items { get; private set; }
    }
}