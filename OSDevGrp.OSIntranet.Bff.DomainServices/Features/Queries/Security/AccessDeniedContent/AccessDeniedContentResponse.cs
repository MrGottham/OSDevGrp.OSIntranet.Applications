using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Security.AccessDeniedContent;

public class AccessDeniedContentResponse : PageResponseBase
{
    #region Constructor

    public AccessDeniedContentResponse(IReadOnlyDictionary<StaticTextKey, string> staticTexts) 
        : base(staticTexts)
    {
    }

    #endregion
}