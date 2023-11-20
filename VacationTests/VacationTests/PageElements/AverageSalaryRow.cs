using Kontur.Selone.Selectors.Context;
using VacationTests.Infrastructure;
using VacationTests.Infrastructure.PageElements;

namespace VacationTests.PageElements
{
    [InjectControlsAttribute]
    public class AverageSalaryRow : ControlBase
    {
        public AverageSalaryRow(IContextBy contextBy) : base(contextBy)
        {
        }

        public Select YearSelect { get; private set; }
        public CurrencyInput SalaryCurrencyInput { get; private set; }
        public CurrencyLabel CountBaseCurrencyLabel { get; private set; }
    }
}