using Kontur.Selone.Extensions;
using Kontur.Selone.Selectors.Context;
using VacationTests.Infrastructure;
using VacationTests.Infrastructure.PageElements;

namespace VacationTests.PageElements
{
    [InjectControlsAttribute]
    public class AverageSalaryRow : ControlBase
    {
        public AverageSalaryRow(IContextBy contextBy, ControlFactory controlFactory) : base(contextBy)
        {
            YearSelect =
                controlFactory.CreateControl<Select>(
                    Container.Search(x => x.WithTid("YearSelect")));
            SalaryCurrencyInput =
                controlFactory.CreateControl<CurrencyInput>(Container.Search(x =>
                    x.WithTid("SalaryCurrencyInput")));
            CountBaseCurrencyLabel =
                controlFactory.CreateControl<CurrencyLabel>(Container.Search(x =>
                    x.WithTid("CountBaseCurrencyLabel")));
        }

        public Select YearSelect { get; }
        public CurrencyInput SalaryCurrencyInput { get; }
        public CurrencyLabel CountBaseCurrencyLabel { get; }
    }
}