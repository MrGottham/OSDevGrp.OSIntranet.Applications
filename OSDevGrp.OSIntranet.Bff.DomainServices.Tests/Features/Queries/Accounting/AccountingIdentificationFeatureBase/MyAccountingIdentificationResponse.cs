using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries.Accounting.AccountingIdentificationFeatureBase;

public class MyAccountingIdentificationResponse : AccountingIdentificationResponseBase<object, IDynamicTexts>
{
    #region Constructor

    public MyAccountingIdentificationResponse(object model, IDynamicTexts dynamicTexts, IReadOnlyDictionary<StaticTextKey, string> staticTexts)
        : base(model, dynamicTexts, staticTexts)
    {
    }

    #endregion
}