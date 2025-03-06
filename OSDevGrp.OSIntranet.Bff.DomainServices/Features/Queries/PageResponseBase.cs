using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries;

public abstract class PageResponseBase : ResponseBase
{
    #region Constructor

    protected PageResponseBase(IReadOnlyDictionary<StaticTextKey, string> staticTexts)
        : base()
    {
        StaticTexts = staticTexts ?? throw new ArgumentNullException(nameof(staticTexts));
    }

    #endregion

    #region Properties

    public IReadOnlyDictionary<StaticTextKey, string> StaticTexts { get;  }

    #endregion
}