using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class AccountingTexts : DynamicTextsBase<AccountingModel>, IAccountingTexts
{
    #region Constructor

    public AccountingTexts(AccountingModel model, IValueDisplayer balanceBelowZero, IValueDisplayer backDating, IFormatProvider formatProvider)
        : base(model, formatProvider)
    {
        BalanceBelowZero = balanceBelowZero;
        BackDating = backDating;
    }

    #endregion

    #region Properties

    public IValueDisplayer BalanceBelowZero { get; }

    public IValueDisplayer BackDating { get; }

    #endregion
}