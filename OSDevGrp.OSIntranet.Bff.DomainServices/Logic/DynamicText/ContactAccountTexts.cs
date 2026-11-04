using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class ContactAccountTexts : DynamicTextsBase<ContactAccountModel>, IContactAccountTexts
{
    #region Constructor

    public ContactAccountTexts(ContactAccountModel model, IValueDisplayer statusDate, IContactAccountValuesDisplayer valuesAtStatusDate, IContactAccountValuesDisplayer valuesAtEndOfLastMonthFromStatusDate, IContactAccountValuesDisplayer valuesAtEndOfLastYearFromStatusDate, IFormatProvider formatProvider) 
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

    public IContactAccountValuesDisplayer ValuesAtStatusDate { get; }

    public IContactAccountValuesDisplayer ValuesAtEndOfLastMonthFromStatusDate { get; }

    public IContactAccountValuesDisplayer ValuesAtEndOfLastYearFromStatusDate { get; }

    #endregion
}