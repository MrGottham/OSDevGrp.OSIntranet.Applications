using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Home.NotImplemented;

internal class NotImplementedFeature : PageFeatureBase<NotImplementedRequest, NotImplementedResponse, object>
{
    #region Constructor

    public NotImplementedFeature(IStaticTextProvider staticTextProvider)
        : base(staticTextProvider)
    {
    }

    #endregion

    #region Methods

    public override async Task<NotImplementedResponse> ExecuteAsync(NotImplementedRequest request, CancellationToken cancellationToken = default)
    {
        IReadOnlyDictionary<StaticTextKey, string> staticTexts = await GetStaticTextsAsync(request, new object(), cancellationToken);

        return new NotImplementedResponse(staticTexts);
    }

    protected override IReadOnlyDictionary<StaticTextKey, IEnumerable<object>> GetStaticTextSpecifications(NotImplementedRequest request, object argument)
    {
        object[] noArguments = Array.Empty<object>();

        return new Dictionary<StaticTextKey, IEnumerable<object>>
        {
            { StaticTextKey.FunctionalityNotImplmented, noArguments },
            { StaticTextKey.FunctionalityNotImplmentedDetails, noArguments },
        };
    }

    #endregion
}