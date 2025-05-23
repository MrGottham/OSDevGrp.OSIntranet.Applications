using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Home.Error;

internal class ErrorFeature : PageFeatureBase<ErrorRequest, ErrorResponse, object>
{
    #region Constructor

    public ErrorFeature(IStaticTextProvider staticTextProvider) 
        : base(staticTextProvider)
    {
    }

    #endregion

    #region Methods

    public override async Task<ErrorResponse> ExecuteAsync(ErrorRequest request, CancellationToken cancellationToken = default)
    {
        IReadOnlyDictionary<StaticTextKey, string> staticTexts = await GetStaticTextsAsync(request, new object(), cancellationToken);

        return new ErrorResponse(request.ErrorMessage, staticTexts);
    }

    protected override IReadOnlyDictionary<StaticTextKey, IEnumerable<object>> GetStaticTextSpecifications(ErrorRequest request, object argument)
    {
        object[] noArguments = Array.Empty<object>();

        return new Dictionary<StaticTextKey, IEnumerable<object>>
        {
            { StaticTextKey.SomethingWentWrong, noArguments }
        };
    }

    #endregion
}