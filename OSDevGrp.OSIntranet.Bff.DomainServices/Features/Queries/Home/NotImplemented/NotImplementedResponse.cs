using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Home.NotImplemented;

public class NotImplementedResponse : PageResponseBase
{
    #region Constructor

    public NotImplementedResponse(IReadOnlyDictionary<StaticTextKey, string> staticTexts) 
        : base(staticTexts)
    {
    }

    #endregion
}