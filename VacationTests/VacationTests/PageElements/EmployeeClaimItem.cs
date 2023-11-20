using Kontur.Selone.Extensions;
using Kontur.Selone.Selectors.Context;
using VacationTests.Infrastructure;
using VacationTests.Infrastructure.PageElements;

namespace VacationTests.PageElements
{
    // Класс элемента списка отпусков EmployeeClaimList наследуем от ControlBase,
    // поскольку это тоже контрол и могут понадобиться базовые методы и пропсы
    [InjectControlsAttribute]
    public class EmployeeClaimItem : ControlBase
    {
        public EmployeeClaimItem(IContextBy contextBy) : base(contextBy)
        {
        }

        // При обращении из теста к любому элементу списка отпусков будут доступны три свойства
        public Link TitleLink { get; private set; }
        public Label PeriodLabel { get; private set; }
        public Label StatusLabel { get; private set; }

        // Можно вот так реализовать метод для наведения мыши на конкретный элемент,
        // такой метод может понадобиться, если только при наведении на элемент списка показывается какой-то контрол
        public void MouseOver()
        {
            Container.Mouseover();
        }
    }
}