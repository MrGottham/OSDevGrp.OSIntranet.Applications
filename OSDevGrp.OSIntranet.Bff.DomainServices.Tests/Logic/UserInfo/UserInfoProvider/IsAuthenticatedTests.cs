using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.UserInfo;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.UserInfo.UserInfoProvider;

[TestFixture]
public class IsAuthenticatedTests
{
    #region Private variables

    private Mock<IAccountingGateway>? _accountingGatewayMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _accountingGatewayMock = new Mock<IAccountingGateway>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    public void IsAuthenticated_WhenIdentityOnClaimsPrincipalIsNull_ReturnsFalse()
    {
        IUserInfoProvider sut = CreateSut();

        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
        bool result = sut.IsAuthenticated(claimsPrincipal);

        Assert.That(result, Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public void IsAuthenticated_WhenIdentityOnClaimsPrincipalIsNonAuthenticatedClaimsIdentity_ReturnsFalse()
    {
        IUserInfoProvider sut = CreateSut();

        ClaimsIdentity claimsIdentity = _fixture!.CreateNonAuthenticatedClaimsIdentity();
        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        bool result = sut.IsAuthenticated(claimsPrincipal);

        Assert.That(result, Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public void IsAuthenticated_WhenIdentityOnClaimsPrincipalIsAuthenticatedClaimsIdentity_ReturnsTrue()
    {
        IUserInfoProvider sut = CreateSut();

        ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity();
        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        bool result = sut.IsAuthenticated(claimsPrincipal);

        Assert.That(result, Is.True);
    }

    private IUserInfoProvider CreateSut()
    {
        return new DomainServices.Logic.UserInfo.UserInfoProvider(_accountingGatewayMock!.Object);
    }
}