using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class BudgetAccountTexts : DynamicTextsBase<BudgetAccountModel>, IBudgetAccountTexts
{
    #region Constructor

    public BudgetAccountTexts(BudgetAccountModel model, IValueDisplayer statusDate, IBudgetAccountValuesDisplayer valuesForMonthOfStatusDate, IBudgetAccountValuesDisplayer valuesForLastMonthOfStatusDate, IBudgetAccountValuesDisplayer valuesForYearToDateOfStatusDate, IBudgetAccountValuesDisplayer valuesForLastYearOfStatusDate, IFormatProvider formatProvider) 
        : base(model, formatProvider)
    {
        StatusDate = statusDate;
        ValuesForMonthOfStatusDate = valuesForMonthOfStatusDate;
        ValuesForLastMonthOfStatusDate = valuesForLastMonthOfStatusDate;
        ValuesForYearToDateOfStatusDate = valuesForYearToDateOfStatusDate;
        ValuesForLastYearOfStatusDate = valuesForLastYearOfStatusDate;
    }

    #endregion

    #region Properties

    public IValueDisplayer StatusDate { get; }

    public IBudgetAccountValuesDisplayer ValuesForMonthOfStatusDate { get; }

    public IBudgetAccountValuesDisplayer ValuesForLastMonthOfStatusDate { get; }

    public IBudgetAccountValuesDisplayer ValuesForYearToDateOfStatusDate { get; }

    public IBudgetAccountValuesDisplayer ValuesForLastYearOfStatusDate { get; }

    #endregion
}