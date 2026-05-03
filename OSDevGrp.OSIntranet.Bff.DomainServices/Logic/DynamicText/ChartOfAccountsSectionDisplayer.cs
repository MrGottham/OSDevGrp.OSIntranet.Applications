using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class ChartOfAccountsSectionDisplayer : IChartOfAccountsSectionDisplayer
{
    #region Constructor

    private ChartOfAccountsSectionDisplayer(string identification, string description, IReadOnlyCollection<IChartOfAccountsLineDisplayer> lines)
    {
        Identification = identification;
        Description = description;
        Lines = lines;
    }

    #endregion

    #region Properties

    public string Identification { get; }

    public string Description { get; }

    public IReadOnlyCollection<IChartOfAccountsLineDisplayer> Lines { get; }

    #endregion

    #region Methods

    internal static IChartOfAccountsSectionDisplayer Create(AccountGroupModel accountGroup, IReadOnlyCollection<AccountModel> accounts, IFormatProvider formatProvider)
    {
        return new ChartOfAccountsSectionDisplayer(accountGroup.Number.ToString(formatProvider), accountGroup.Name, accounts.Select(account => ChartOfAccountsLineDisplayer.Create(account, formatProvider)).ToArray());
    }

    #endregion
}