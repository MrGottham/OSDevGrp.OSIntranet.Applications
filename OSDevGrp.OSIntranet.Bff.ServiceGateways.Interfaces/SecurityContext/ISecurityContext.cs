namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

public interface ISecurityContext
{
    IToken AccessToken { get; }
}