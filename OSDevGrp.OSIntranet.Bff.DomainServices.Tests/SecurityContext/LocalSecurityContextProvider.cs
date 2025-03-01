using AutoFixture;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.SecurityContext;

internal class LocalSecurityContextProvider : ISecurityContextProvider
{
    #region Private variables

    private readonly ISecurityContext _securityContext;

    #endregion

    #region Constructor

    internal LocalSecurityContextProvider()
    {
        Fixture fixture = new Fixture();

        _securityContext = fixture.CreateSecurityContext();
    }

    #endregion

    #region Methods

    public Task<ISecurityContext> GetCurrentSecurityContextAsync(CancellationToken cancellationToken = default) => Task.FromResult(_securityContext);

    #endregion
}