namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

public interface IDynamicTextsBuilder<TModel, TDynamicTexts> where TModel : class where TDynamicTexts : IDynamicTexts
{
    Task<TDynamicTexts> BuildAsync(TModel model, IFormatProvider formatProvider, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<TDynamicTexts>> BuildAsync(IEnumerable<TModel> models, IFormatProvider formatProvider, CancellationToken cancellationToken = default);
}