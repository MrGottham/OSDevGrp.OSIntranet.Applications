using System;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Security
{
    public interface IAuthorizationStateBuilder
    {
        IAuthorizationStateBuilder WithClientSecret(string clientSecret);

        IAuthorizationStateBuilder WithExternalState(string externalState);

        IAuthorizationStateBuilder WithNonce(string nonce);

        IAuthorizationStateBuilder WithAuthorizationCode(IAuthorizationCode authorizationCode);

        IAuthorizationStateBuilder WithAuthorizationCode(string value, DateTimeOffset expires);

        IAuthorizationState Build();
    }
}