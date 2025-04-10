namespace OSDevGrp.OSIntranet.Bff.WebApi.Options;

public class OpenIdConnectOptions
{
    public string? Authority { get; set; }

    public string? ClientId { get; set; }

    public string? ClientSecret { get; set; }

    internal Uri AsAuthorityUrl()
    {
        if (Uri.TryCreate(Authority, UriKind.Absolute, out Uri? authorityUrl) && authorityUrl.IsWellFormedOriginalString()) 
        {
            return authorityUrl;
        }

        throw new InvalidOperationException($"The authority '{Authority}' is not a valid absolute URI."); 
    }
}