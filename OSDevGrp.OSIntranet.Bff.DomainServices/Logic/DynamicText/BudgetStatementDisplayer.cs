using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class BudgetStatementDisplayer : IBudgetStatementDisplayer
{
    #region Constructor

    private BudgetStatementDisplayer(string header, IValueDisplayer budget, IValueDisplayer posted, IValueDisplayer available)
    {
        Header = header;
        Budget = budget;
        Posted = posted;
        Available = available;
    }

    #endregion

    #region Properties

    public string Header { get; }

    public IValueDisplayer Budget { get; }

    public IValueDisplayer Posted { get; }

    public IValueDisplayer Available { get; }

    #endregion

    #region Methods

    internal static async Task<IBudgetStatementDisplayer> CreateAsync<TModel>(StaticTextKey header, StaticTextKey budget, StaticTextKey posted, StaticTextKey available, IStaticTextProvider staticTextProvider, TModel model, Func<TModel, decimal> budgetCalculator, Func<TModel, decimal> postedCalculator, Func<TModel, decimal> availableCalculator, IFormatProvider formatProvider, CancellationToken cancellationToken = default) where TModel : class
    {
        string headerText = await staticTextProvider.GetStaticTextAsync(header, header.DefaultArguments(), formatProvider, cancellationToken);
        string budgetText = await staticTextProvider.GetStaticTextAsync(budget, budget.DefaultArguments(), formatProvider, cancellationToken);
        string postedText = await staticTextProvider.GetStaticTextAsync(posted, posted.DefaultArguments(), formatProvider, cancellationToken);
        string availableText = await staticTextProvider.GetStaticTextAsync(available, available.DefaultArguments(), formatProvider, cancellationToken);

        return new BudgetStatementDisplayer(
            headerText,
            new ValueDisplayer<decimal>(budgetText, budgetCalculator(model), formatProvider, (v, fp) => v.ToString("C", fp)),
            new ValueDisplayer<decimal>(postedText, postedCalculator(model), formatProvider, (v, fp) => v.ToString("C", fp)),
            new ValueDisplayer<decimal>(availableText, availableCalculator(model), formatProvider, (v, fp) => v.ToString("C", fp)));
    }

    #endregion
}