using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class BudgetAccountValuesDisplayer : IBudgetAccountValuesDisplayer
{
    #region Constructor

    private BudgetAccountValuesDisplayer(string header, IValueDisplayer budget, IValueDisplayer posted, IValueDisplayer available)
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

    internal static async Task<IBudgetAccountValuesDisplayer> CreateAsync(StaticTextKey headerKey, BudgetInfoValuesModel values, IStaticTextProvider staticTextProvider, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(values);
        ArgumentNullException.ThrowIfNull(staticTextProvider);
        ArgumentNullException.ThrowIfNull(formatProvider);

        string headerText = await staticTextProvider.GetStaticTextAsync(headerKey, headerKey.DefaultArguments(), formatProvider, cancellationToken);
        string budgetText = await staticTextProvider.GetStaticTextAsync(StaticTextKey.Budget, StaticTextKey.Budget.DefaultArguments(), formatProvider, cancellationToken);
        string postedText = await staticTextProvider.GetStaticTextAsync(StaticTextKey.Posted, StaticTextKey.Posted.DefaultArguments(), formatProvider, cancellationToken);
        string availableText = await staticTextProvider.GetStaticTextAsync(StaticTextKey.Available, StaticTextKey.Available.DefaultArguments(), formatProvider, cancellationToken);

        return new BudgetAccountValuesDisplayer(
            headerText,
            new ValueDisplayer<decimal>(budgetText, (decimal)values.Budget, formatProvider, (v, fp) => v.ToString("C", fp)),
            new ValueDisplayer<decimal>(postedText, (decimal)values.Posted, formatProvider, (v, fp) => v.ToString("C", fp)),
            new ValueDisplayer<decimal>(availableText, (decimal)values.Available, formatProvider, (v, fp) => v.ToString("C", fp)));
    }

    #endregion
}