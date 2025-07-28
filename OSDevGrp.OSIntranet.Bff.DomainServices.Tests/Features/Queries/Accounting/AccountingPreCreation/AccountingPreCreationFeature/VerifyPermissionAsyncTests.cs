using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.AccountingPreCreation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.AccountingRuleSetBuilder;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.UserHelper;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries.Accounting.AccountingPreCreation.AccountingPreCreationFeature;

[TestFixture]
public class VerifyPermissionAsyncTests : AccountingPreCreationFeatureTestBase
{
    #region Private variables

    private Mock<IPermissionChecker>? _permissionCheckerMock;
    private Mock<ICommonGateway>? _commonGatewayMock;
    private Mock<IStaticTextProvider>? _staticTextProviderMock;
    private Mock<IAccountingRuleSetBuilder>? _accountingRuleSetBuilderMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _permissionCheckerMock = new Mock<IPermissionChecker>();
        _commonGatewayMock = new Mock<ICommonGateway>();
        _staticTextProviderMock = new Mock<IStaticTextProvider>();
        _accountingRuleSetBuilderMock = new Mock<IAccountingRuleSetBuilder>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, true, true, true)]
    [TestCase(true, true, true, false)]
    [TestCase(true, true, false, true)]
    [TestCase(true, true, false, false)]
    [TestCase(true, false, true, true)]
    [TestCase(true, false, true, false)]
    [TestCase(true, false, false, true)]
    [TestCase(true, false, false, false)]
    [TestCase(false, true, true, true)]
    [TestCase(false, true, true, false)]
    [TestCase(false, true, false, true)]
    [TestCase(false, true, false, false)]
    [TestCase(false, false, true, true)]
    [TestCase(false, false, true, false)]
    [TestCase(false, false, false, true)]
    [TestCase(false, false, false, false)]
    public async Task VerifyPermissionAsync_WhenCalled_AssertUserWasCalledOnGivenSecurityContext(bool isAuthenticated, bool hasAccountingAccess, bool isAccountingCreator, bool hasCommonDataAccess)
    {
        IPermissionVerifiable<AccountingPreCreationRequest> sut = CreateSut(isAuthenticated: isAuthenticated, hasAccountingAccess: hasAccountingAccess, isAccountingCreator: isAccountingCreator, hasCommonDataAccess: hasCommonDataAccess);

        Mock<ISecurityContext> securityContextMock = _fixture!.CreateSecurityContextMock();
        await sut.VerifyPermissionAsync(securityContextMock.Object, CreateAccountingPreCreationRequestRequest(_fixture!));

        securityContextMock.Verify(m => m.User, Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, true, true, true)]
    [TestCase(true, true, true, false)]
    [TestCase(true, true, false, true)]
    [TestCase(true, true, false, false)]
    [TestCase(true, false, true, true)]
    [TestCase(true, false, true, false)]
    [TestCase(true, false, false, true)]
    [TestCase(true, false, false, false)]
    [TestCase(false, true, true, true)]
    [TestCase(false, true, true, false)]
    [TestCase(false, true, false, true)]
    [TestCase(false, true, false, false)]
    [TestCase(false, false, true, true)]
    [TestCase(false, false, true, false)]
    [TestCase(false, false, false, true)]
    [TestCase(false, false, false, false)]
    public async Task VerifyPermissionAsync_WhenCalled_AssertIsAuthenticatedWasCalledOnPermissionCheckerWithUserFromGivenSecurityContext(bool isAuthenticated, bool hasAccountingAccess, bool isAccountingCreator, bool hasCommonDataAccess)
    {
        IPermissionVerifiable<AccountingPreCreationRequest> sut = CreateSut(isAuthenticated: isAuthenticated, hasAccountingAccess: hasAccountingAccess, isAccountingCreator: isAccountingCreator, hasCommonDataAccess: hasCommonDataAccess);

        ClaimsPrincipal user = isAuthenticated ? _fixture!.CreateAuthenticatedClaimsPrincipal() : _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        ISecurityContext securityContext = _fixture!.CreateSecurityContext(user: user);
        await sut.VerifyPermissionAsync(securityContext, CreateAccountingPreCreationRequestRequest(_fixture!));

        _permissionCheckerMock!.Verify(m => m.IsAuthenticated(It.Is<ClaimsPrincipal>(value => value == user)), Times.Once);
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
    public async Task VerifyPermissionAsync_WhenUserIsAuthenticated_AssertHasAccountingAccessWasCalledOnPermissionCheckerWithUserFromGivenSecurityContext(bool hasAccountingAccess, bool isAccountingCreator, bool hasCommonDataAccess)
    {
        IPermissionVerifiable<AccountingPreCreationRequest> sut = CreateSut(isAuthenticated: true, hasAccountingAccess: hasAccountingAccess, isAccountingCreator: isAccountingCreator, hasCommonDataAccess: hasCommonDataAccess);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        ISecurityContext securityContext = _fixture!.CreateSecurityContext(user: user);
        await sut.VerifyPermissionAsync(securityContext, CreateAccountingPreCreationRequestRequest(_fixture!));

        _permissionCheckerMock!.Verify(m => m.HasAccountingAccess(It.Is<ClaimsPrincipal>(value => value == user)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, true)]
    [TestCase(true, false)]
    [TestCase(false, true)]
    [TestCase(false, false)]
    public async Task VerifyPermissionAsync_WhenUserIsAuthenticatedAndHasAccountingAccess_AssertIsAccountingCreatorWasCalledOnPermissionCheckerWithUserFromGivenSecurityContext(bool isAccountingCreator, bool hasCommonDataAccess)
    {
        IPermissionVerifiable<AccountingPreCreationRequest> sut = CreateSut(isAuthenticated: true, hasAccountingAccess: true, isAccountingCreator: isAccountingCreator, hasCommonDataAccess: hasCommonDataAccess);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        ISecurityContext securityContext = _fixture!.CreateSecurityContext(user: user);
        await sut.VerifyPermissionAsync(securityContext, CreateAccountingPreCreationRequestRequest(_fixture!));

        _permissionCheckerMock!.Verify(m => m.IsAccountingCreator(It.Is<ClaimsPrincipal>(value => value == user)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task VerifyPermissionAsync_WhenUserIsAuthenticatedHasAccountingAccessAndIsAccountingCreator_AssertHasCommonDataAccessWasCalledOnPermissionCheckerWithUserFromGivenSecurityContext(bool hasCommonDataAccess)
    {
        IPermissionVerifiable<AccountingPreCreationRequest> sut = CreateSut(isAuthenticated: true, hasAccountingAccess: true, isAccountingCreator: true, hasCommonDataAccess: hasCommonDataAccess);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        ISecurityContext securityContext = _fixture!.CreateSecurityContext(user: user);
        await sut.VerifyPermissionAsync(securityContext, CreateAccountingPreCreationRequestRequest(_fixture!));

        _permissionCheckerMock!.Verify(m => m.HasCommonDataAccess(It.Is<ClaimsPrincipal>(value => value == user)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task VerifyPermissionAsync_WhenUserIsAuthenticatedHasAccountingAccessButIsNotAccountingCreator_AssertHasCommonDataAccessWasNotCalledOnPermissionChecker(bool hasCommonDataAccess)
    {
        IPermissionVerifiable<AccountingPreCreationRequest> sut = CreateSut(isAuthenticated: true, hasAccountingAccess: true, isAccountingCreator: false, hasCommonDataAccess: hasCommonDataAccess);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        ISecurityContext securityContext = _fixture!.CreateSecurityContext(user: user);
        await sut.VerifyPermissionAsync(securityContext, CreateAccountingPreCreationRequestRequest(_fixture!));

        _permissionCheckerMock!.Verify(m => m.HasCommonDataAccess(It.IsAny<ClaimsPrincipal>()), Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, true)]
    [TestCase(true, false)]
    [TestCase(false, true)]
    [TestCase(false, false)]
    public async Task VerifyPermissionAsync_WhenUserIsAuthenticatedButDoesNotHaveAccountingAccess_AssertIsAccountingCreatorWasCalledOnPermissionChecker(bool isAccountingCreator, bool hasCommonDataAccess)
    {
        IPermissionVerifiable<AccountingPreCreationRequest> sut = CreateSut(isAuthenticated: true, hasAccountingAccess: false, isAccountingCreator: isAccountingCreator, hasCommonDataAccess: hasCommonDataAccess);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        ISecurityContext securityContext = _fixture!.CreateSecurityContext(user: user);
        await sut.VerifyPermissionAsync(securityContext, CreateAccountingPreCreationRequestRequest(_fixture!));

        _permissionCheckerMock!.Verify(m => m.IsAccountingCreator(It.IsAny<ClaimsPrincipal>()), Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, true)]
    [TestCase(true, false)]
    [TestCase(false, true)]
    [TestCase(false, false)]
    public async Task VerifyPermissionAsync_WhenUserIsAuthenticatedButDoesNotHaveAccountingAccess_AssertHasCommonDataAccessWasCalledOnPermissionChecker(bool isAccountingCreator, bool hasCommonDataAccess)
    {
        IPermissionVerifiable<AccountingPreCreationRequest> sut = CreateSut(isAuthenticated: true, hasAccountingAccess: false, isAccountingCreator: isAccountingCreator, hasCommonDataAccess: hasCommonDataAccess);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        ISecurityContext securityContext = _fixture!.CreateSecurityContext(user: user);
        await sut.VerifyPermissionAsync(securityContext, CreateAccountingPreCreationRequestRequest(_fixture!));

        _permissionCheckerMock!.Verify(m => m.HasCommonDataAccess(It.IsAny<ClaimsPrincipal>()), Times.Never);
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
    public async Task VerifyPermissionAsync_WhenUserIsNotAuthenticated_AssertHasAccountingAccessWasNotCalledOnPermissionChecker(bool hasAccountingAccess, bool isAccountingCreator, bool hasCommonDataAccess)
    {
        IPermissionVerifiable<AccountingPreCreationRequest> sut = CreateSut(isAuthenticated: false, hasAccountingAccess: hasAccountingAccess, isAccountingCreator: isAccountingCreator, hasCommonDataAccess: hasCommonDataAccess);

        ClaimsPrincipal user = _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        ISecurityContext securityContext = _fixture!.CreateSecurityContext(user: user);
        await sut.VerifyPermissionAsync(securityContext, CreateAccountingPreCreationRequestRequest(_fixture!));

        _permissionCheckerMock!.Verify(m => m.HasAccountingAccess(It.IsAny<ClaimsPrincipal>()), Times.Never);
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
    public async Task VerifyPermissionAsync_WhenUserIsNotAuthenticated_AssertIsAccountingCreatorWasNotCalledOnPermissionChecker(bool hasAccountingAccess, bool isAccountingCreator, bool hasCommonDataAccess)
    {
        IPermissionVerifiable<AccountingPreCreationRequest> sut = CreateSut(isAuthenticated: false, hasAccountingAccess: hasAccountingAccess, isAccountingCreator: isAccountingCreator, hasCommonDataAccess: hasCommonDataAccess);

        ClaimsPrincipal user = _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        ISecurityContext securityContext = _fixture!.CreateSecurityContext(user: user);
        await sut.VerifyPermissionAsync(securityContext, CreateAccountingPreCreationRequestRequest(_fixture!));

        _permissionCheckerMock!.Verify(m => m.IsAccountingCreator(It.IsAny<ClaimsPrincipal>()), Times.Never);
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
    public async Task VerifyPermissionAsync_WhenUserIsNotAuthenticated_AssertHasCommonDataAccessWasNotCalledOnPermissionChecker(bool hasAccountingAccess, bool isAccountingCreator, bool hasCommonDataAccess)
    {
        IPermissionVerifiable<AccountingPreCreationRequest> sut = CreateSut(isAuthenticated: false, hasAccountingAccess: hasAccountingAccess, isAccountingCreator: isAccountingCreator, hasCommonDataAccess: hasCommonDataAccess);

        ClaimsPrincipal user = _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        ISecurityContext securityContext = _fixture!.CreateSecurityContext(user: user);
        await sut.VerifyPermissionAsync(securityContext, CreateAccountingPreCreationRequestRequest(_fixture!));

        _permissionCheckerMock!.Verify(m => m.HasCommonDataAccess(It.IsAny<ClaimsPrincipal>()), Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, true, true, true, true)]
    [TestCase(true, true, true, false, false)]
    [TestCase(true, true, false, true, false)]
    [TestCase(true, true, false, false, false)]
    [TestCase(true, false, true, true, false)]
    [TestCase(true, false, true, false, false)]
    [TestCase(true, false, false, true, false)]
    [TestCase(true, false, false, false, false)]
    [TestCase(false, true, true, true, false)]
    [TestCase(false, true, true, false, false)]
    [TestCase(false, true, false, true, false)]
    [TestCase(false, true, false, false, false)]
    [TestCase(false, false, true, true, false)]
    [TestCase(false, false, true, false, false)]
    [TestCase(false, false, false, true, false)]
    [TestCase(false, false, false, false, false)]
    public async Task VerifyPermissionAsync_WhenCalled_ReturnsExpectedValue(bool isAuthenticated, bool hasAccountingAccess, bool isAccountingCreator, bool hasCommonDataAccess, bool expectedValue)
    {
        IPermissionVerifiable<AccountingPreCreationRequest> sut = CreateSut(isAuthenticated: isAuthenticated, hasAccountingAccess: hasAccountingAccess, isAccountingCreator: isAccountingCreator, hasCommonDataAccess: hasCommonDataAccess);

        Mock<ISecurityContext> securityContextMock = _fixture!.CreateSecurityContextMock();
        bool result = await sut.VerifyPermissionAsync(securityContextMock.Object, CreateAccountingPreCreationRequestRequest(_fixture!));

        Assert.That(result, Is.EqualTo(expectedValue));
    }

    private IPermissionVerifiable<AccountingPreCreationRequest> CreateSut(bool isAuthenticated = true, bool hasAccountingAccess = true, bool isAccountingCreator = true, bool hasCommonDataAccess = true)
    {
        _permissionCheckerMock!.Setup(_fixture!, isAuthenticated: isAuthenticated, hasAccountingAccess: hasAccountingAccess, isAccountingCreator: isAccountingCreator, hasCommonDataAccess: hasCommonDataAccess);
        _staticTextProviderMock!.Setup(_fixture!);
        _accountingRuleSetBuilderMock!.Setup(_fixture!);

        return new DomainServices.Features.Queries.Accounting.AccountingPreCreation.AccountingPreCreationFeature(_permissionCheckerMock!.Object, _commonGatewayMock!.Object, _staticTextProviderMock!.Object, _accountingRuleSetBuilderMock!.Object);
    }
}