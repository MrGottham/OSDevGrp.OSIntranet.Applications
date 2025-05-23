using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries;

public abstract class PageFeatureTestBase
{
    #region Methods

    protected static ISecurityContext CreateSecurityContext(Fixture fixture, bool withAuthenticatedUser = true)
    {
        return CreateSecurityContextMock(fixture, withAuthenticatedUser).Object;
    }

    protected static ISecurityContext CreateSecurityContext(Fixture fixture, ClaimsPrincipal user)
    {
        return CreateSecurityContextMock(fixture, user).Object;
    }

    protected static Mock<ISecurityContext> CreateSecurityContextMock(Fixture fixture, bool withAuthenticatedUser = true)
    {
        return CreateSecurityContextMock(fixture, user: withAuthenticatedUser ? CreateAuthenticatedUser(fixture) : CreateNonAuthenticated(fixture));
    }

    protected static Mock<ISecurityContext> CreateSecurityContextMock(Fixture fixture, ClaimsPrincipal user)
    {
        return fixture.CreateSecurityContextMock(user: user);
    }

    protected static ClaimsPrincipal CreateAuthenticatedUser(Fixture fixture)
    {
        return fixture.CreateAuthenticatedClaimsPrincipal();
    }

    protected static ClaimsPrincipal CreateNonAuthenticated(Fixture fixture)
    {
        return fixture.CreateNonAuthenticatedClaimsPrincipal();
    }

    #endregion
}