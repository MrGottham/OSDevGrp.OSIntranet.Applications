namespace OSDevGrp.OSIntranet.Domain.Interfaces.Security
{
    public interface IScopeBuilder
    {
        IScopeBuilder WithRelatedClaim(string claimType);

        IScope Build();
    }
}