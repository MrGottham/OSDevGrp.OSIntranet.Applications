using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.Accountings;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.UserHelper;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries.Accounting.Accountings.AccountingsFeature;

[TestFixture]
public class VerifyPermissionAsyncTests : AccountingsFeatureTestBase
{
    #region Private variables

    private Mock<IPermissionChecker>? _permissionCheckerMock;
    private Mock<IAccountingGateway>? _accountingGatewayMock;
    private Mock<IStaticTextProvider>? _staticTextProviderMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _permissionCheckerMock = new Mock<IPermissionChecker>();
        _accountingGatewayMock = new Mock<IAccountingGateway>();
        _staticTextProviderMock = new Mock<IStaticTextProvider>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, true)]
    [TestCase(true, false)]
    [TestCase(false, true)]
    [TestCase(false, false)]
    public async Task VerifyPermissionAsync_WhenCalled_AssertUserWasCalledOnGivenSecurityContext(bool isAuthenticated, bool hasAccountingAccess)
    {
        IPermissionVerifiable<AccountingsRequest> sut = CreateSut(isAuthenticated: isAuthenticated, hasAccountingAccess: hasAccountingAccess);

        Mock<ISecurityContext> securityContextMock = _fixture!.CreateSecurityContextMock();
        await sut.VerifyPermissionAsync(securityContextMock.Object, CreateAccountingsRequest(_fixture!));

        securityContextMock.Verify(m => m.User, Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, true)]
    [TestCase(true, false)]
    [TestCase(false, true)]
    [TestCase(false, false)]
    public async Task VerifyPermissionAsync_WhenCalled_AssertIsAuthenticatedWasCalledOnPermissionCheckerWithUserFromGivenSecurityContext(bool isAuthenticated, bool hasAccountingAccess)
    {
        IPermissionVerifiable<AccountingsRequest> sut = CreateSut(isAuthenticated: isAuthenticated, hasAccountingAccess: hasAccountingAccess);

        ClaimsPrincipal user = isAuthenticated ? _fixture!.CreateAuthenticatedClaimsPrincipal() : _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        ISecurityContext securityContext = _fixture!.CreateSecurityContext(user: user);
        await sut.VerifyPermissionAsync(securityContext, CreateAccountingsRequest(_fixture!));

        _permissionCheckerMock!.Verify(m => m.IsAuthenticated(It.Is<ClaimsPrincipal>(value => value == user)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task VerifyPermissionAsync_WhenUserIsAuthenticated_AssertHasAccountingAccessWasCalledOnPermissionCheckerWithUserFromGivenSecurityContext(bool hasAccountingAccess)
    {
        IPermissionVerifiable<AccountingsRequest> sut = CreateSut(isAuthenticated: true, hasAccountingAccess: hasAccountingAccess);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        ISecurityContext securityContext = _fixture!.CreateSecurityContext(user: user);
        await sut.VerifyPermissionAsync(securityContext, CreateAccountingsRequest(_fixture!));

        _permissionCheckerMock!.Verify(m => m.HasAccountingAccess(It.Is<ClaimsPrincipal>(value => value == user)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task VerifyPermissionAsync_WhenUserIsNotAuthenticated_AssertHasAccountingAccessWasNotCalledOnPermissionChecker(bool hasAccountingAccess)
    {
        IPermissionVerifiable<AccountingsRequest> sut = CreateSut(isAuthenticated: false, hasAccountingAccess: hasAccountingAccess);

        await sut.VerifyPermissionAsync(_fixture!.CreateSecurityContext(), CreateAccountingsRequest(_fixture!));

        _permissionCheckerMock!.Verify(m => m.HasAccountingAccess(It.IsAny<ClaimsPrincipal>()), Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, true, true)]
    [TestCase(true, false, false)]
    [TestCase(false, true, false)]
    [TestCase(false, false, false)]
    public async Task VerifyPermissionAsync_WhenCalled_ReturnsExpectedValue(bool isAuthenticated, bool hasAccountingAccess, bool expectedValue)
    {
        IPermissionVerifiable<AccountingsRequest> sut = CreateSut(isAuthenticated: isAuthenticated, hasAccountingAccess: hasAccountingAccess);

        bool result = await sut.VerifyPermissionAsync(_fixture!.CreateSecurityContext(), CreateAccountingsRequest(_fixture!));

        Assert.That(result, Is.EqualTo(expectedValue));
    }

    private IPermissionVerifiable<AccountingsRequest> CreateSut(bool isAuthenticated = true, bool hasAccountingAccess = true)
    {
        _permissionCheckerMock!.Setup(_fixture!, isAuthenticated: isAuthenticated, hasAccountingAccess: hasAccountingAccess);

        return new DomainServices.Features.Queries.Accounting.Accountings.AccountingsFeature(_permissionCheckerMock!.Object, _accountingGatewayMock!.Object, _staticTextProviderMock!.Object);
    }
}