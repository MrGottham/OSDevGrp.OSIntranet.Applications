using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class BudgetAccountTexts : DynamicTextsBase<BudgetAccountModel>, IBudgetAccountTexts
{
    #region Constructor

    public BudgetAccountTexts(BudgetAccountModel model, IFormatProvider formatProvider) 
        : base(model, formatProvider)
    {
    }

    #endregion
}