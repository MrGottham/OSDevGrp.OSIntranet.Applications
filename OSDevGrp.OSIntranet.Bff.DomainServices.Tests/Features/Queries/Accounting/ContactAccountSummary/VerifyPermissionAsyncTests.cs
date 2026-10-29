using System.Security.Claims;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries.Accounting.AccountIdentificationFeatureBase;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries.Accounting.ContactAccountSummary;

[TestFixture]
public class VerifyPermissionAsyncTests : AccountIdentificationFeatureTestBase
{
    #region Private variables

    private Mock<IPermissionChecker>? _permissionCheckerMock;
    private Mock<IAccountingGateway>? _accountingGatewayMock;
    private Mock<IStaticTextProvider>? _staticTextProviderMock;
    private Mock<IDynamicTextsBuilder<object, IDynamicTexts>>? _dynamicTextsBuilderMock;
    private Mock<IValidationRuleSetBuilder>? _validationRuleSetBuilderMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _permissionCheckerMock = new Mock<IPermissionChecker>();
        _accountingGatewayMock = new Mock<IAccountingGateway>();
        _staticTextProviderMock = new Mock<IStaticTextProvider>();
        _dynamicTextsBuilderMock = new Mock<IDynamicTextsBuilder<object, IDynamicTexts>>();
        _validationRuleSetBuilderMock = new Mock<IValidationRuleSetBuilder>();
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
        IPermissionVerifiable<MyAccountIdentificationRequest> sut = CreateSut(_fixture!, _permissionCheckerMock!, _accountingGatewayMock!, _staticTextProviderMock!, _dynamicTextsBuilderMock!, _validationRuleSetBuilderMock!, isAuthenticated: isAuthenticated, hasAccountingAccess: hasAccountingAccess, isAccountingViewer: isAccountingViewer);

        Mock<ISecurityContext> securityContextMock = _fixture!.CreateSecurityContextMock();
        await sut.VerifyPermissionAsync(securityContextMock.Object, CreateAccountIdentificationRequest(_fixture!));

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
        IPermissionVerifiable<MyAccountIdentificationRequest> sut = CreateSut(_fixture!, _permissionCheckerMock!, _accountingGatewayMock!, _staticTextProviderMock!, _dynamicTextsBuilderMock!, _validationRuleSetBuilderMock!, isAuthenticated: isAuthenticated, hasAccountingAccess: hasAccountingAccess, isAccountingViewer: isAccountingViewer);

        ClaimsPrincipal user = isAuthenticated ? _fixture!.CreateAuthenticatedClaimsPrincipal() : _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        ISecurityContext securityContext = _fixture!.CreateSecurityContext(user: user);
        await sut.VerifyPermissionAsync(securityContext, CreateAccountIdentificationRequest(_fixture!));

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
        IPermissionVerifiable<MyAccountIdentificationRequest> sut = CreateSut(_fixture!, _permissionCheckerMock!, _accountingGatewayMock!, _staticTextProviderMock!, _dynamicTextsBuilderMock!, _validationRuleSetBuilderMock!, hasAccountingAccess: hasAccountingAccess, isAccountingViewer: isAccountingViewer);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        ISecurityContext securityContext = _fixture!.CreateSecurityContext(user: user);
        await sut.VerifyPermissionAsync(securityContext, CreateAccountIdentificationRequest(_fixture!));

        _permissionCheckerMock!.Verify(m => m.HasAccountingAccess(It.Is<ClaimsPrincipal>(value => value == user)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task VerifyPermissionAsync_WhenUserIsAuthenticatedAndHasAccountingAccess_AssertIsAccountingViewerWasCalledOnPermissionCheckerWithUserFromGivenSecurityContextAndAccountingNumberFromAccountIdentificationRequest(bool isAccountingViewer)
    {
        IPermissionVerifiable<MyAccountIdentificationRequest> sut = CreateSut(_fixture!, _permissionCheckerMock!, _accountingGatewayMock!, _staticTextProviderMock!, _dynamicTextsBuilderMock!, _validationRuleSetBuilderMock!, isAccountingViewer: isAccountingViewer);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        ISecurityContext securityContext = _fixture!.CreateSecurityContext(user: user);
        MyAccountIdentificationRequest request = CreateAccountIdentificationRequest(_fixture!);
        await sut.VerifyPermissionAsync(securityContext, request);

        _permissionCheckerMock!.Verify(m => m.IsAccountingViewer(It.Is<ClaimsPrincipal>(value => value == user), It.Is<int>(value => value == request.AccountingNumber)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task VerifyPermissionAsync_WhenUserIsNotAuthenticated_AssertResultIsFalse()
    {
        IPermissionVerifiable<MyAccountIdentificationRequest> sut = CreateSut(_fixture!, _permissionCheckerMock!, _accountingGatewayMock!, _staticTextProviderMock!, _dynamicTextsBuilderMock!, _validationRuleSetBuilderMock!, isAuthenticated: false);

        ISecurityContext securityContext = _fixture!.CreateSecurityContext(user: _fixture!.CreateNonAuthenticatedClaimsPrincipal());
        bool result = await sut.VerifyPermissionAsync(securityContext, CreateAccountIdentificationRequest(_fixture!));

        Assert.That(result, Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public async Task VerifyPermissionAsync_WhenUserIsAuthenticatedButDoesNotHaveAccountingAccess_AssertResultIsFalse()
    {
        IPermissionVerifiable<MyAccountIdentificationRequest> sut = CreateSut(_fixture!, _permissionCheckerMock!, _accountingGatewayMock!, _staticTextProviderMock!, _dynamicTextsBuilderMock!, _validationRuleSetBuilderMock!, hasAccountingAccess: false);

        ISecurityContext securityContext = _fixture!.CreateSecurityContext(user: _fixture!.CreateAuthenticatedClaimsPrincipal());
        bool result = await sut.VerifyPermissionAsync(securityContext, CreateAccountIdentificationRequest(_fixture!));

        Assert.That(result, Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public async Task VerifyPermissionAsync_WhenUserIsAuthenticatedAndHasAccountingAccessButIsNotAccountingViewer_AssertResultIsFalse()
    {
        IPermissionVerifiable<MyAccountIdentificationRequest> sut = CreateSut(_fixture!, _permissionCheckerMock!, _accountingGatewayMock!, _staticTextProviderMock!, _dynamicTextsBuilderMock!, _validationRuleSetBuilderMock!, isAccountingViewer: false);

        ISecurityContext securityContext = _fixture!.CreateSecurityContext(user: _fixture!.CreateAuthenticatedClaimsPrincipal());
        bool result = await sut.VerifyPermissionAsync(securityContext, CreateAccountIdentificationRequest(_fixture!));

        Assert.That(result, Is.False);
    }
}