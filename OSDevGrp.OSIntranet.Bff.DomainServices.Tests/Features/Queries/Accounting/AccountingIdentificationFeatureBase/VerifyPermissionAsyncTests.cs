using System.Security.Claims;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries.Accounting.AccountingIdentificationFeatureBase;

[TestFixture]
public class VerifyPermissionAsyncTests : AccountingIdentificationFeatureTestBase
{
    #region Private variables

    private Mock<IPermissionChecker>? _permissionCheckerMock;
    private Mock<IAccountingGateway>? _accountingGatewayMock;
    private Mock<IStaticTextProvider>? _staticTextProviderMock;
    private Mock<IDynamicTextsBuilder<object, IDynamicTexts>>? _dynamicTextsBuilderMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _permissionCheckerMock = new Mock<IPermissionChecker>();
        _accountingGatewayMock = new Mock<IAccountingGateway>();
        _staticTextProviderMock = new Mock<IStaticTextProvider>();
        _dynamicTextsBuilderMock = new Mock<IDynamicTextsBuilder<object, IDynamicTexts>>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, true, true)]
    [TestCase(true, true, false)]
    [TestCase(true, false, true)]
    [TestCase(true, false, false)]
    [TestCase(false, true, true)]
    [TestCase(false, true, false)]
    [TestCase(false, false, true)]
    [TestCase(false, false, false)]
    public async Task VerifyPermissionAsync_WhenCalled_AssertUserWasCalledOnGivenSecurityContext(bool isAuthenticated, bool hasAccountingAccess, bool isAccountingViewer)
    {
        IPermissionVerifiable<MyAccountingIdentificationRequest> sut = CreateSut(isAuthenticated: isAuthenticated, hasAccountingAccess: hasAccountingAccess, isAccountingViewer: isAccountingViewer);

        Mock<ISecurityContext> securityContextMock = _fixture!.CreateSecurityContextMock();
        await sut.VerifyPermissionAsync(securityContextMock.Object, CreateAccountingIdentificationRequest(_fixture!));

        securityContextMock.Verify(m => m.User, Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, true, true)]
    [TestCase(true, true, false)]
    [TestCase(true, false, true)]
    [TestCase(true, false, false)]
    [TestCase(false, true, true)]
    [TestCase(false, true, false)]
    [TestCase(false, false, true)]
    [TestCase(false, false, false)]
    public async Task VerifyPermissionAsync_WhenCalled_AssertIsAuthenticatedWasCalledOnPermissionCheckerWithUserFromGivenSecurityContext(bool isAuthenticated, bool hasAccountingAccess, bool isAccountingViewer)
    {
        IPermissionVerifiable<MyAccountingIdentificationRequest> sut = CreateSut(isAuthenticated: isAuthenticated, hasAccountingAccess: hasAccountingAccess, isAccountingViewer: isAccountingViewer);

        ClaimsPrincipal user = isAuthenticated ? _fixture!.CreateAuthenticatedClaimsPrincipal() : _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        ISecurityContext securityContext = _fixture!.CreateSecurityContext(user: user);
        await sut.VerifyPermissionAsync(securityContext, CreateAccountingIdentificationRequest(_fixture!));

        _permissionCheckerMock!.Verify(m => m.IsAuthenticated(It.Is<ClaimsPrincipal>(value => value == user)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, true)]
    [TestCase(true, false)]
    [TestCase(false, true)]
    [TestCase(false, false)]
    public async Task VerifyPermissionAsync_WhenUserIsAuthenticated_AssertHasAccountingAccessWasCalledOnPermissionCheckerWithUserFromGivenSecurityContext(bool hasAccountingAccess, bool isAccountingViewer)
    {
        IPermissionVerifiable<MyAccountingIdentificationRequest> sut = CreateSut(hasAccountingAccess: hasAccountingAccess, isAccountingViewer: isAccountingViewer);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        ISecurityContext securityContext = _fixture!.CreateSecurityContext(user: user);
        await sut.VerifyPermissionAsync(securityContext, CreateAccountingIdentificationRequest(_fixture!));

        _permissionCheckerMock!.Verify(m => m.HasAccountingAccess(It.Is<ClaimsPrincipal>(value => value == user)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task VerifyPermissionAsync_WhenUserIsAuthenticatedAndHasAccountingAccess_AssertIsAccountingViewerWasCalledOnPermissionCheckerWithUserFromGivenSecurityContextAndAccountingNumberFromGivenAccountingIdentificationRequest(bool isAccountingViewer)
    {
        IPermissionVerifiable<MyAccountingIdentificationRequest> sut = CreateSut(isAccountingViewer: isAccountingViewer);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        ISecurityContext securityContext = _fixture!.CreateSecurityContext(user: user);
        int accountingNumber = _fixture!.Create<int>();
        await sut.VerifyPermissionAsync(securityContext, CreateAccountingIdentificationRequest(_fixture!, accountingNumber: accountingNumber));

        _permissionCheckerMock!.Verify(m => m.IsAccountingViewer(
                It.Is<ClaimsPrincipal>(value => value == user),
                It.Is<int?>(value => value == accountingNumber)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task VerifyPermissionAsync_WhenUserIsAuthenticatedButDoesNotHaveAccountingAccess_AssertIsAccountingViewerWasNotCalledOnPermissionChecker(bool isAccountingViewer)
    {
        IPermissionVerifiable<MyAccountingIdentificationRequest> sut = CreateSut(hasAccountingAccess: false, isAccountingViewer: isAccountingViewer);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        ISecurityContext securityContext = _fixture!.CreateSecurityContext(user: user);
        await sut.VerifyPermissionAsync(securityContext, CreateAccountingIdentificationRequest(_fixture!));

        _permissionCheckerMock!.Verify(m => m.IsAccountingViewer(It.IsAny<ClaimsPrincipal>(), It.IsAny<int?>()), Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, true)]
    [TestCase(true, false)]
    [TestCase(false, true)]
    [TestCase(false, false)]
    public async Task VerifyPermissionAsync_WhenUserIsNotAuthenticated_AssertHasAccountingAccessWasNotCalledOnPermissionChecker(bool hasAccountingAccess, bool isAccountingViewer)
    {
        IPermissionVerifiable<MyAccountingIdentificationRequest> sut = CreateSut(isAuthenticated: false, hasAccountingAccess: hasAccountingAccess, isAccountingViewer: isAccountingViewer);

        await sut.VerifyPermissionAsync(_fixture!.CreateSecurityContext(), CreateAccountingIdentificationRequest(_fixture!));

        _permissionCheckerMock!.Verify(m => m.HasAccountingAccess(It.IsAny<ClaimsPrincipal>()), Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, true)]
    [TestCase(true, false)]
    [TestCase(false, true)]
    [TestCase(false, false)]
    public async Task VerifyPermissionAsync_WhenUserIsNotAuthenticated_AssertIsAccountingViewerWasNotCalledOnPermissionChecker(bool hasAccountingAccess, bool isAccountingViewer)
    {
        IPermissionVerifiable<MyAccountingIdentificationRequest> sut = CreateSut(isAuthenticated: false, hasAccountingAccess: hasAccountingAccess, isAccountingViewer: isAccountingViewer);

        await sut.VerifyPermissionAsync(_fixture!.CreateSecurityContext(), CreateAccountingIdentificationRequest(_fixture!));

        _permissionCheckerMock!.Verify(m => m.IsAccountingViewer(It.IsAny<ClaimsPrincipal>(), It.IsAny<int?>()), Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, true, true, true)]
    [TestCase(true, true, false, false)]
    [TestCase(true, false, true, false)]
    [TestCase(true, false, false, false)]
    [TestCase(false, true, true, false)]
    [TestCase(false, true, false, false)]
    [TestCase(false, false, true, false)]
    [TestCase(false, false, false, false)]
    public async Task VerifyPermissionAsync_WhenCalled_ReturnsExpectedValue(bool isAuthenticated, bool hasAccountingAccess, bool isAccountingViewer, bool expectedValue)
    {
        IPermissionVerifiable<MyAccountingIdentificationRequest> sut = CreateSut(isAuthenticated: isAuthenticated, hasAccountingAccess: hasAccountingAccess, isAccountingViewer: isAccountingViewer);

        Mock<ISecurityContext> securityContextMock = _fixture!.CreateSecurityContextMock();
        bool result = await sut.VerifyPermissionAsync(securityContextMock.Object, CreateAccountingIdentificationRequest(_fixture!));

        Assert.That(result, Is.EqualTo(expectedValue));
    }

    private IPermissionVerifiable<MyAccountingIdentificationRequest> CreateSut(bool isAuthenticated = true, bool hasAccountingAccess = true, bool isAccountingViewer = true)
    {
        return (IPermissionVerifiable<MyAccountingIdentificationRequest>) CreateSut(_fixture!, _permissionCheckerMock!, _accountingGatewayMock!, _staticTextProviderMock!, _dynamicTextsBuilderMock!, isAuthenticated: isAuthenticated, hasAccountingAccess: hasAccountingAccess, isAccountingViewer: isAccountingViewer);
    }
}