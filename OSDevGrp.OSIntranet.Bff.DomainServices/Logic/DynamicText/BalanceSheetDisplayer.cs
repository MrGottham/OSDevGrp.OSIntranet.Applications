using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class BalanceSheetDisplayer : IBalanceSheetDisplayer
{
    #region Constructor

    private BalanceSheetDisplayer(string header, IValueDisplayer assets, IValueDisplayer liabilities)
    {
        Header = header;
        Assets = assets;
        Liabilities = liabilities;
    }

    #endregion

    #region Properties

    public string Header { get; }

    public IValueDisplayer Assets { get; }

    public IValueDisplayer Liabilities { get; }

    #endregion

    #region Methods

    internal static async Task<IBalanceSheetDisplayer> CreateAsync<TModel>(StaticTextKey header, StaticTextKey assets, StaticTextKey liabilities, IStaticTextProvider staticTextProvider, TModel model, Func<TModel, decimal> assetsCalculator, Func<TModel, decimal> liabilitiesCalculator, IFormatProvider formatProvider, CancellationToken cancellationToken = default) where TModel : class
    {
        string headerText = await staticTextProvider.GetStaticTextAsync(header, header.DefaultArguments(), formatProvider, cancellationToken);
        string assetsText = await staticTextProvider.GetStaticTextAsync(assets, assets.DefaultArguments(), formatProvider, cancellationToken);
        string liabilitiesText = await staticTextProvider.GetStaticTextAsync(liabilities, liabilities.DefaultArguments(), formatProvider, cancellationToken);

        return new BalanceSheetDisplayer(
            headerText,
            new ValueDisplayer<decimal>(assetsText, assetsCalculator(model), formatProvider, (v, fp) => v.ToString("C", fp)),
            new ValueDisplayer<decimal>(liabilitiesText, liabilitiesCalculator(model), formatProvider, (v, fp) => v.ToString("C", fp)));
    }

    #endregion
}