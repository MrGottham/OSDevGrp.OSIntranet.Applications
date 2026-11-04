using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class AccountTexts : DynamicTextsBase<AccountModel>, IAccountTexts
{
    #region Constructor

    public AccountTexts(AccountModel model, IValueDisplayer statusDate, IAccountValuesDisplayer valuesAtStatusDate, IAccountValuesDisplayer valuesAtEndOfLastMonthFromStatusDate, IAccountValuesDisplayer valuesAtEndOfLastYearFromStatusDate, IFormatProvider formatProvider) 
        : base(model, formatProvider)
    {
        StatusDate = statusDate;
        ValuesAtStatusDate = valuesAtStatusDate;
        ValuesAtEndOfLastMonthFromStatusDate = valuesAtEndOfLastMonthFromStatusDate;
        ValuesAtEndOfLastYearFromStatusDate = valuesAtEndOfLastYearFromStatusDate;
    }

    #endregion

    #region Properties

    public IValueDisplayer StatusDate { get; }

    public IAccountValuesDisplayer ValuesAtStatusDate { get; }

    public IAccountValuesDisplayer ValuesAtEndOfLastMonthFromStatusDate { get; }

    public IAccountValuesDisplayer ValuesAtEndOfLastYearFromStatusDate { get; }

    #endregion
}