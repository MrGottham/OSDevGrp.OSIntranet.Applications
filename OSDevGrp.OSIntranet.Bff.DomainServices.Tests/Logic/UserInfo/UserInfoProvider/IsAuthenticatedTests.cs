using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.UserInfo;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.UserHelper;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.UserInfo.UserInfoProvider;

[TestFixture]
public class IsAuthenticatedTests
{
    #region Private variables

    private Mock<IUserHelper>? _userHelperMock;
    private Mock<IAccountingGateway>? _accountingGatewayMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _userHelperMock = new Mock<IUserHelper>();
        _accountingGatewayMock = new Mock<IAccountingGateway>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    public void IsAuthenticated_WhenCalled_AssertIsAuthenticatedWasCalledOnUserHelperWithGivenClaimsPrincipal()
    {
        IUserInfoProvider sut = CreateSut();

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        sut.IsAuthenticated(user);

        _userHelperMock!.Verify(m => m.IsAuthenticated(It.Is<ClaimsPrincipal>(value => value == user)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public void IsAuthenticated_WhenCalled_ReturnsIsAuthenticatedFromUserHelper(bool isAuthenticated)
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: isAuthenticated);

        ClaimsPrincipal user = isAuthenticated
            ? _fixture!.CreateAuthenticatedClaimsPrincipal()
            : _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        bool result = sut.IsAuthenticated(user);

        Assert.That(result, Is.EqualTo(isAuthenticated));
    }

    private IUserInfoProvider CreateSut(bool isAuthenticated = true)
    {
        _userHelperMock!.Setup(_fixture!, isAuthenticated: isAuthenticated);

        return new DomainServices.Logic.UserInfo.UserInfoProvider(_userHelperMock!.Object, _accountingGatewayMock!.Object);
    }
}