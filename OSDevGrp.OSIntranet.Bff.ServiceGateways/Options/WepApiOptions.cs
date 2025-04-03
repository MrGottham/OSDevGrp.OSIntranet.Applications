namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.Options;

public class WebApiOptions
{
    public string? EndpointAddress { get; set; }

    public string? ClientId { get; set; }

    public string? ClientSecret { get; set; }

    public Uri AsEndpointAddressUrl()
    {
        if (Uri.TryCreate(EndpointAddress, UriKind.Absolute, out Uri? endpointAddressUrl) && endpointAddressUrl.IsWellFormedOriginalString()) 
        {
            return endpointAddressUrl;
        }

        throw new InvalidOperationException($"The endpoint address '{EndpointAddress}' is not a valid absolute URI."); 
    }
}