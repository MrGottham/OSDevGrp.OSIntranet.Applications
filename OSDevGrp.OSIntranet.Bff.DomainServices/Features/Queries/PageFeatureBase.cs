using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using System.Collections.Concurrent;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries;

internal abstract class PageFeatureBase<TPageRequest, TPageResponse, TStaticTextArgument> : IQueryFeature<TPageRequest, TPageResponse> where TPageRequest : PageRequestBase where TPageResponse : PageResponseBase
{
    #region Private variables

    private readonly IStaticTextProvider _staticTextProvider;

    #endregion

    #region Constructor

    protected PageFeatureBase(IStaticTextProvider staticTextProvider)
    {
        _staticTextProvider = staticTextProvider;
    }

    #endregion

    #region Methods

    public abstract Task<TPageResponse> ExecuteAsync(TPageRequest request, CancellationToken cancellationToken = default);

    protected abstract IReadOnlyDictionary<StaticTextKey, IEnumerable<object>> GetStaticTextSpecifications(TPageRequest request, TStaticTextArgument argument);

    protected async Task<IReadOnlyDictionary<StaticTextKey, string>> GetStaticTextsAsync(TPageRequest request, TStaticTextArgument argument, CancellationToken cancellationToken = default)
    {
        IFormatProvider formatProvider = request.FormatProvider;

        IReadOnlyDictionary<StaticTextKey, IEnumerable<object>> staticTextSpecifications = GetStaticTextSpecifications(request, argument);
        KeyValuePair<StaticTextKey, string>[] staticTexts = await Task.WhenAll(staticTextSpecifications.Select(staticTextSpecification => GetStaticTextAsync(staticTextSpecification.Key, staticTextSpecification.Value, formatProvider, cancellationToken)));

        return new ConcurrentDictionary<StaticTextKey, string>(staticTexts).AsReadOnly();
    }

    private async Task<KeyValuePair<StaticTextKey, string>> GetStaticTextAsync(StaticTextKey staticTextKey, IEnumerable<object> arguments, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        return new KeyValuePair<StaticTextKey, string>(staticTextKey, await _staticTextProvider.GetStaticTextAsync(staticTextKey, arguments, formatProvider, cancellationToken));
    }

    #endregion
}