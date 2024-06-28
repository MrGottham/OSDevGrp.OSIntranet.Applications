namespace OSDevGrp.OSIntranet.Domain.Interfaces.Security
{
    public interface IAuthorizationStateBuilder
    {
        IAuthorizationStateBuilder WithExternalState(string externalState);

        IAuthorizationState Build();
    }
}