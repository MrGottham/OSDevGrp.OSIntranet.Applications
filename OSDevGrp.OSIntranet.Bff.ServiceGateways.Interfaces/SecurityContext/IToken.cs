namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

public interface IToken
{
    string TokenType { get; }

    string Token { get; }

    DateTimeOffset Expires { get; }

    bool Expired { get; }
}