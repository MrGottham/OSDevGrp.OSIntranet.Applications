using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class AccountValuesDisplayer : IAccountValuesDisplayer
{
    #region Constructor

    private AccountValuesDisplayer(string header, IValueDisplayer credit, IValueDisplayer balance, IValueDisplayer available)
    {
        Header = header;
        Credit = credit;
        Balance = balance;
        Available = available;
    }

    #endregion

    #region Properties

    public string Header { get; }

    public IValueDisplayer Credit { get; }

    public IValueDisplayer Balance { get; }

    public IValueDisplayer Available { get; }

    #endregion

    #region Methods

    internal static async Task<IAccountValuesDisplayer> CreateAsync(StaticTextKey headerKey, CreditInfoValuesModel values, IStaticTextProvider staticTextProvider, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(values);
        ArgumentNullException.ThrowIfNull(staticTextProvider);
        ArgumentNullException.ThrowIfNull(formatProvider);

        string headerText = await staticTextProvider.GetStaticTextAsync(headerKey, headerKey.DefaultArguments(), formatProvider, cancellationToken);
        string creditText = await staticTextProvider.GetStaticTextAsync(StaticTextKey.Credit, StaticTextKey.Credit.DefaultArguments(), formatProvider, cancellationToken);
        string balanceText = await staticTextProvider.GetStaticTextAsync(StaticTextKey.Balance, StaticTextKey.Balance.DefaultArguments(), formatProvider, cancellationToken);
        string availableText = await staticTextProvider.GetStaticTextAsync(StaticTextKey.Available, StaticTextKey.Available.DefaultArguments(), formatProvider, cancellationToken);

        return new AccountValuesDisplayer(
            headerText,
            new ValueDisplayer<decimal>(creditText, (decimal)values.Credit, formatProvider, (v, fp) => v.ToString("C", fp)),
            new ValueDisplayer<decimal>(balanceText, (decimal)values.Balance, formatProvider, (v, fp) => v.ToString("C", fp)),
            new ValueDisplayer<decimal>(availableText, (decimal)values.Available, formatProvider, (v, fp) => v.ToString("C", fp)));
    }

    #endregion
}