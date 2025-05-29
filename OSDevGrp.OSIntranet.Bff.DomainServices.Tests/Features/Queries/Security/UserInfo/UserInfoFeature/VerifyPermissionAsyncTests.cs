using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Security.UserInfo;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.UserInfo;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.UserHelper;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries.Security.UserInfo.UserInfoFeature;

[TestFixture]
public class VerifyPermissionAsyncTests : UserInfoFeatureTestBase
{
    #region Private variables

    private Mock<IPermissionChecker>? _permissionCheckerMock;
    private Mock<IUserInfoProvider>? _userInfoProviderMock;
    private Mock<IStaticTextProvider>? _staticTextProviderMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _permissionCheckerMock = new Mock<IPermissionChecker>();
        _userInfoProviderMock = new Mock<IUserInfoProvider>();
        _staticTextProviderMock = new Mock<IStaticTextProvider>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    public async Task VerifyPermissionAsync_WhenCalled_AssertUserWasNotCalledOnGivenSecurityContext()
    {
        IPermissionVerifiable<UserInfoRequest> sut = CreateSut();

        Mock<ISecurityContext> securityContextMock = CreateSecurityContextMock(_fixture!);
        await sut.VerifyPermissionAsync(securityContextMock.Object, CreateUserInfoRequest(_fixture!));

        securityContextMock.Verify(m => m.User, Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task VerifyPermissionAsync_WhenCalled_AssertUserWasCalledOnSecurityContextFromGivenUserInfoRequest()
    {
        IPermissionVerifiable<UserInfoRequest> sut = CreateSut();

        Mock<ISecurityContext> securityContextMock = CreateSecurityContextMock(_fixture!);
        UserInfoRequest request = CreateUserInfoRequest(_fixture!, securityContextMock.Object);
        await sut.VerifyPermissionAsync(CreateSecurityContext(_fixture!), request);

        securityContextMock.Verify(m => m.User, Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task VerifyPermissionAsync_WhenCalled_AssertIsAuthenticatedWasCalledOnPermissionCheckerWithUserFromGivenUserInfoRequest()
    {
        IPermissionVerifiable<UserInfoRequest> sut = CreateSut();

        ClaimsPrincipal user = CreateAuthenticatedUser(_fixture!); 
        ISecurityContext securityContext = CreateSecurityContext(_fixture!, user: user);
        UserInfoRequest request = CreateUserInfoRequest(_fixture!, securityContext);
        await sut.VerifyPermissionAsync(CreateSecurityContext(_fixture!), request);

        _permissionCheckerMock!.Verify(m => m.IsAuthenticated(It.Is<ClaimsPrincipal>(value => value == user)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task VerifyPermissionAsync_WhenCalled_ReturnsIsAuthenticatedFromPermissionChecker(bool isAuthenticated)
    {
        IPermissionVerifiable<UserInfoRequest> sut = CreateSut(isAuthenticated: isAuthenticated);

        ClaimsPrincipal user = CreateAuthenticatedUser(_fixture!);
        ISecurityContext securityContext = CreateSecurityContext(_fixture!, user: user);
        UserInfoRequest request = CreateUserInfoRequest(_fixture!, securityContext);
        bool result = await sut.VerifyPermissionAsync(CreateSecurityContext(_fixture!), request);

        Assert.That(result, Is.EqualTo(isAuthenticated));
    }

    private IPermissionVerifiable<UserInfoRequest> CreateSut(bool isAuthenticated = true)
    {
        _permissionCheckerMock!.Setup(_fixture!, isAuthenticated: isAuthenticated);

        return new DomainServices.Features.Queries.Security.UserInfo.UserInfoFeature(_permissionCheckerMock!.Object, _userInfoProviderMock!.Object, _staticTextProviderMock!.Object);
    }
}