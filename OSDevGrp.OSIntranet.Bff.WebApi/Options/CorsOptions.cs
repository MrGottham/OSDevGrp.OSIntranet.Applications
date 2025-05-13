namespace OSDevGrp.OSIntranet.Bff.WebApi.Options;

public class CorsOptions
{
    public string? OriginCollection { get; set; }

    internal IEnumerable<string> AsOrigins() => OriginCollection?.Split(';').Where(origin => string.IsNullOrWhiteSpace(origin) == false).ToArray() ?? Array.Empty<string>();
}