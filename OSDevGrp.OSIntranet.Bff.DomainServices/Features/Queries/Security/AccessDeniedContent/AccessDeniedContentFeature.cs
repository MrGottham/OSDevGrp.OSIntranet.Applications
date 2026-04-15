using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Security.AccessDeniedContent;

internal class AccessDeniedContentFeature : PageFeatureBase<AccessDeniedContentRequest, AccessDeniedContentResponse, object>
{
    #region Constructor

    public AccessDeniedContentFeature(IStaticTextProvider staticTextProvider) 
        : base(staticTextProvider)
    {
    }

    #endregion

    #region Methods

    public override async Task<AccessDeniedContentResponse> ExecuteAsync(AccessDeniedContentRequest request, CancellationToken cancellationToken = default)
    {
        IReadOnlyDictionary<StaticTextKey, string> staticTexts = await GetStaticTextsAsync(request, new object(), cancellationToken);

        return new AccessDeniedContentResponse(staticTexts);
    }

    protected override IReadOnlyDictionary<StaticTextKey, IEnumerable<object>> GetStaticTextSpecifications(AccessDeniedContentRequest request, object argument)
    {
        return new Dictionary<StaticTextKey, IEnumerable<object>>
        {
            { StaticTextKey.AccessDenied, StaticTextKey.AccessDenied.DefaultArguments() },
            { StaticTextKey.MissingPermissionToPage, StaticTextKey.MissingPermissionToPage.DefaultArguments() },
            { StaticTextKey.CheckYourCredentials, StaticTextKey.CheckYourCredentials.DefaultArguments() }
        };
    }

    #endregion
}