using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting;

public abstract class AccountingIdentificationResponseBase<TModel, TDynamicTexts> : PageResponseBase where TModel : class where TDynamicTexts : IDynamicTexts
{
    #region Constructor

    protected AccountingIdentificationResponseBase(TModel model, TDynamicTexts dynamicTexts, IReadOnlyDictionary<StaticTextKey, string> staticTexts)
        : base(staticTexts)
    {
        Model = model;
        DynamicTexts = dynamicTexts;
    }

    #endregion

    #region Properties

    public TModel Model { get; set; }

    public TDynamicTexts DynamicTexts { get; set; }

    #endregion
}