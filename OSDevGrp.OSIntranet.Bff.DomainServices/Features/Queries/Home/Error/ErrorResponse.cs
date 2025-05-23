using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Home.Error;

public class ErrorResponse : PageResponseBase
{
    #region Constructor

    public ErrorResponse(string errorMessage, IReadOnlyDictionary<StaticTextKey, string> staticTexts)
        : base(staticTexts)
    {
        ErrorMessage = errorMessage;
    }

    #endregion

    #region Properties

    public string ErrorMessage { get; }

    #endregion
}