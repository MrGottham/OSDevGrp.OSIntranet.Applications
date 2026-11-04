using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class ContactAccountValuesDisplayer : IContactAccountValuesDisplayer
{
    #region Constructor

    private ContactAccountValuesDisplayer(string header, IValueDisplayer balance)
    {
        Header = header;
        Balance = balance;
    }

    #endregion

    #region Properties

    public string Header { get; }

    public IValueDisplayer Balance { get; }

    #endregion

    #region Methods

    internal static async Task<IContactAccountValuesDisplayer> CreateAsync(StaticTextKey headerKey, BalanceInfoValuesModel values, IStaticTextProvider staticTextProvider, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(values);
        ArgumentNullException.ThrowIfNull(staticTextProvider);
        ArgumentNullException.ThrowIfNull(formatProvider);

        string headerText = await staticTextProvider.GetStaticTextAsync(headerKey, headerKey.DefaultArguments(), formatProvider, cancellationToken);
        string balanceText = await staticTextProvider.GetStaticTextAsync(StaticTextKey.Balance, StaticTextKey.Balance.DefaultArguments(), formatProvider, cancellationToken);

        return new ContactAccountValuesDisplayer(
            headerText,
            new ValueDisplayer<decimal>(balanceText, (decimal)values.Balance, formatProvider, (v, fp) => v.ToString("C", fp)));
    }

    #endregion
}