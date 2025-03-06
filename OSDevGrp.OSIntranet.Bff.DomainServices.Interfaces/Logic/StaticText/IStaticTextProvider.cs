namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;

public interface IStaticTextProvider
{
    Task<string> GetStaticTextAsync(StaticTextKey staticTextKey, IEnumerable<object> arguments, IFormatProvider formatProvider, CancellationToken cancellationToken = default);
}