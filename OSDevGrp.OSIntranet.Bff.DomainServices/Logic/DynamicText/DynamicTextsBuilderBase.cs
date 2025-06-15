using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal abstract class DynamicTextsBuilderBase<TModel, TDynamicTexts> : IDynamicTextsBuilder<TModel, TDynamicTexts> where TModel : class where TDynamicTexts : IDynamicTexts
{
    #region Constructor

    protected DynamicTextsBuilderBase(IStaticTextProvider staticTextProvider)
    {
        StaticTextProvider = staticTextProvider;
    }

    #endregion

    #region Properties

    protected IStaticTextProvider StaticTextProvider { get; }

    #endregion

    #region Methods

    public abstract Task<TDynamicTexts> BuildAsync(TModel model, IFormatProvider formatProvider, CancellationToken cancellationToken = default);

    public async Task<IReadOnlyCollection<TDynamicTexts>> BuildAsync(IEnumerable<TModel> models, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await Task.WhenAll(models.Select(model => BuildAsync(model, formatProvider, cancellationToken)));
    }

    protected Task<IValueDisplayer> GetValueDisplayerAsync<TValue>(StaticTextKey staticTextKey, TValue value, IFormatProvider formatProvider, Func<TValue, IFormatProvider, string?> valueFormatter, CancellationToken cancellationToken = default)
    {
        return GetValueDisplayerAsync(staticTextKey, Array.Empty<object>(), value, formatProvider, valueFormatter, cancellationToken);
    }

    protected async Task<IValueDisplayer> GetValueDisplayerAsync<TValue>(StaticTextKey staticTextKey, IEnumerable<object> arguments, TValue value, IFormatProvider formatProvider, Func<TValue, IFormatProvider, string?> valueFormatter, CancellationToken cancellationToken = default)
    {
        string staticText = await StaticTextProvider.GetStaticTextAsync(staticTextKey, arguments, formatProvider, cancellationToken);

        return new ValueDisplayer<TValue>(staticText, value, formatProvider, valueFormatter);
    }

    protected static string Resolve<TEnum>(TEnum value, IDictionary<TEnum, string> staticTexts) where TEnum : struct, Enum
    {
        if (staticTexts.TryGetValue(value, out string? staticText) == false)
        {
            throw new ArgumentOutOfRangeException(nameof(value), value, $"The value '{value}' is not defined in the static texts.");
        }

        return staticText;
    }

    #endregion
}