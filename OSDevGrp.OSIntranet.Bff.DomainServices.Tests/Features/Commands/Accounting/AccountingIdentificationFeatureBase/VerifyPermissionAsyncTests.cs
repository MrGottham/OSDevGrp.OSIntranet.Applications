using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Commands.Accounting.AccountingIdentificationFeatureBase;

[TestFixture]
public class VerifyPermissionAsyncTests : AccountingIdentificationFeatureTestBase
{
    #region Private variables

    private Mock<IPermissionChecker>? _permissionCheckerMock;
    private Mock<IAccountingGateway>? _accountingGatewayMock;
    private Fixture? _fixture;

    #endregion

    #region Setup

    [SetUp]
    public void SetUp()
    {
        _permissionCheckerMock = new Mock<IPermissionChecker>();
        _accountingGatewayMock = new Mock<IAccountingGateway>();
        _fixture = new Fixture();
    }

    #endregion

    #region Tests

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
    public async Task VerifyPermissionAsync_WhenCalled_AssertUserWasCalledOnGivenSecurityContext(bool isAuthenticated, bool hasAccountingAccess, bool isAccountingModifier)
    {
        Mock<ISecurityContext> securityContextMock = _fixture!.CreateSecurityContextMock();
        IPermissionVerifiable<TestAccountingIdentificationRequest> sut = CreateSut<TestAccountingIdentificationRequest>(
            _fixture!,
            _permissionCheckerMock!,
            _accountingGatewayMock!,
            isAuthenticated: isAuthenticated,
            hasAccountingAccess: hasAccountingAccess,
            isAccountingModifier: isAccountingModifier);

        TestAccountingIdentificationRequest request = CreateAccountingIdentificationRequest<TestAccountingIdentificationRequest>(_fixture!);
        await sut.VerifyPermissionAsync(securityContextMock.Object, request);

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
    public async Task VerifyPermissionAsync_WhenCalled_AssertIsAuthenticatedWasCalledOnPermissionCheckerWithUserFromGivenSecurityContext(bool isAuthenticated, bool hasAccountingAccess, bool isAccountingModifier)
    {
        Mock<ISecurityContext> securityContextMock = _fixture!.CreateSecurityContextMock();
        var user = securityContextMock.Object.User;
        IPermissionVerifiable<TestAccountingIdentificationRequest> sut = CreateSut<TestAccountingIdentificationRequest>(
            _fixture!,
            _permissionCheckerMock!,
            _accountingGatewayMock!,
            isAuthenticated: isAuthenticated,
            hasAccountingAccess: hasAccountingAccess,
            isAccountingModifier: isAccountingModifier);

        TestAccountingIdentificationRequest request = CreateAccountingIdentificationRequest<TestAccountingIdentificationRequest>(_fixture!);
        await sut.VerifyPermissionAsync(securityContextMock.Object, request);

        _permissionCheckerMock!.Verify(m => m.IsAuthenticated(It.Is<System.Security.Claims.ClaimsPrincipal>(u => u == user)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, true, true)]
    [TestCase(true, true, false)]
    [TestCase(true, false, true)]
    [TestCase(true, false, false)]
    public async Task VerifyPermissionAsync_WhenUserIsAuthenticated_AssertHasAccountingAccessWasCalledOnPermissionCheckerWithUserFromGivenSecurityContext(bool isAuthenticated, bool hasAccountingAccess, bool isAccountingModifier)
    {
        Mock<ISecurityContext> securityContextMock = _fixture!.CreateSecurityContextMock();
        var user = securityContextMock.Object.User;
        IPermissionVerifiable<TestAccountingIdentificationRequest> sut = CreateSut<TestAccountingIdentificationRequest>(
            _fixture!,
            _permissionCheckerMock!,
            _accountingGatewayMock!,
            isAuthenticated: isAuthenticated,
            hasAccountingAccess: hasAccountingAccess,
            isAccountingModifier: isAccountingModifier);

        TestAccountingIdentificationRequest request = CreateAccountingIdentificationRequest<TestAccountingIdentificationRequest>(_fixture!);
        await sut.VerifyPermissionAsync(securityContextMock.Object, request);

        _permissionCheckerMock!.Verify(m => m.HasAccountingAccess(It.Is<System.Security.Claims.ClaimsPrincipal>(u => u == user)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(false, true, true)]
    [TestCase(false, true, false)]
    [TestCase(false, false, true)]
    [TestCase(false, false, false)]
    public async Task VerifyPermissionAsync_WhenUserIsNotAuthenticated_AssertHasAccountingAccessWasNotCalledOnPermissionChecker(bool isAuthenticated, bool hasAccountingAccess, bool isAccountingModifier)
    {
        Mock<ISecurityContext> securityContextMock = _fixture!.CreateSecurityContextMock();
        IPermissionVerifiable<TestAccountingIdentificationRequest> sut = CreateSut<TestAccountingIdentificationRequest>(
            _fixture!,
            _permissionCheckerMock!,
            _accountingGatewayMock!,
            isAuthenticated: isAuthenticated,
            hasAccountingAccess: hasAccountingAccess,
            isAccountingModifier: isAccountingModifier);

        TestAccountingIdentificationRequest request = CreateAccountingIdentificationRequest<TestAccountingIdentificationRequest>(_fixture!);
        await sut.VerifyPermissionAsync(securityContextMock.Object, request);

        _permissionCheckerMock!.Verify(m => m.HasAccountingAccess(It.IsAny<System.Security.Claims.ClaimsPrincipal>()), Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, true, true)]
    [TestCase(true, true, false)]
    public async Task VerifyPermissionAsync_WhenUserIsAuthenticatedAndHasAccountingAccess_AssertIsAccountingModifierWasCalledOnPermissionChecker(bool isAuthenticated, bool hasAccountingAccess, bool isAccountingModifier)
    {
        Mock<ISecurityContext> securityContextMock = _fixture!.CreateSecurityContextMock();
        var user = securityContextMock.Object.User;
        IPermissionVerifiable<TestAccountingIdentificationRequest> sut = CreateSut<TestAccountingIdentificationRequest>(
            _fixture!,
            _permissionCheckerMock!,
            _accountingGatewayMock!,
            isAuthenticated: isAuthenticated,
            hasAccountingAccess: hasAccountingAccess,
            isAccountingModifier: isAccountingModifier);

        TestAccountingIdentificationRequest request = CreateAccountingIdentificationRequest<TestAccountingIdentificationRequest>(_fixture!);
        await sut.VerifyPermissionAsync(securityContextMock.Object, request);

        _permissionCheckerMock!.Verify(m => m.IsAccountingModifier(It.Is<System.Security.Claims.ClaimsPrincipal>(u => u == user), It.Is<int?>(an => an == request.AccountingNumber)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, false, true)]
    [TestCase(true, false, false)]
    public async Task VerifyPermissionAsync_WhenUserIsAuthenticatedButDoesNotHaveAccountingAccess_AssertIsAccountingModifierWasNotCalledOnPermissionChecker(bool isAuthenticated, bool hasAccountingAccess, bool isAccountingModifier)
    {
        Mock<ISecurityContext> securityContextMock = _fixture!.CreateSecurityContextMock();
        IPermissionVerifiable<TestAccountingIdentificationRequest> sut = CreateSut<TestAccountingIdentificationRequest>(
            _fixture!,
            _permissionCheckerMock!,
            _accountingGatewayMock!,
            isAuthenticated: isAuthenticated,
            hasAccountingAccess: hasAccountingAccess,
            isAccountingModifier: isAccountingModifier);

        TestAccountingIdentificationRequest request = CreateAccountingIdentificationRequest<TestAccountingIdentificationRequest>(_fixture!);
        await sut.VerifyPermissionAsync(securityContextMock.Object, request);

        _permissionCheckerMock!.Verify(m => m.IsAccountingModifier(It.IsAny<System.Security.Claims.ClaimsPrincipal>(), It.IsAny<int?>()), Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(false, true, true)]
    [TestCase(false, true, false)]
    [TestCase(false, false, true)]
    [TestCase(false, false, false)]
    public async Task VerifyPermissionAsync_WhenUserIsNotAuthenticated_AssertIsAccountingModifierWasNotCalledOnPermissionChecker(bool isAuthenticated, bool hasAccountingAccess, bool isAccountingModifier)
    {
        Mock<ISecurityContext> securityContextMock = _fixture!.CreateSecurityContextMock();
        IPermissionVerifiable<TestAccountingIdentificationRequest> sut = CreateSut<TestAccountingIdentificationRequest>(
            _fixture!,
            _permissionCheckerMock!,
            _accountingGatewayMock!,
            isAuthenticated: isAuthenticated,
            hasAccountingAccess: hasAccountingAccess,
            isAccountingModifier: isAccountingModifier);

        TestAccountingIdentificationRequest request = CreateAccountingIdentificationRequest<TestAccountingIdentificationRequest>(_fixture!);
        await sut.VerifyPermissionAsync(securityContextMock.Object, request);

        _permissionCheckerMock!.Verify(m => m.IsAccountingModifier(It.IsAny<System.Security.Claims.ClaimsPrincipal>(), It.IsAny<int?>()), Times.Never);
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
    public async Task VerifyPermissionAsync_WhenCalled_ReturnsExpectedValue(bool isAuthenticated, bool hasAccountingAccess, bool isAccountingModifier, bool expectedValue)
    {
        IPermissionVerifiable<TestAccountingIdentificationRequest> sut = CreateSut<TestAccountingIdentificationRequest>(
            _fixture!,
            _permissionCheckerMock!,
            _accountingGatewayMock!,
            isAuthenticated: isAuthenticated,
            hasAccountingAccess: hasAccountingAccess,
            isAccountingModifier: isAccountingModifier);

        Mock<ISecurityContext> securityContextMock = _fixture!.CreateSecurityContextMock();
        TestAccountingIdentificationRequest request = CreateAccountingIdentificationRequest<TestAccountingIdentificationRequest>(_fixture!);

        bool result = await sut.VerifyPermissionAsync(securityContextMock.Object, request);

        Assert.That(result, Is.EqualTo(expectedValue));
    }

    [Test]
    [Category("UnitTest")]
    public async Task VerifyPermissionAsync_WhenCalled_AssertIsAuthenticatedWasCalledBeforeHasAccountingAccess()
    {
        var callSequence = new MockSequence();
        _permissionCheckerMock!.InSequence(callSequence).Setup(m => m.IsAuthenticated(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns(true);
        _permissionCheckerMock!.InSequence(callSequence).Setup(m => m.HasAccountingAccess(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns(true);
        _permissionCheckerMock!.InSequence(callSequence).Setup(m => m.IsAccountingModifier(It.IsAny<System.Security.Claims.ClaimsPrincipal>(), It.IsAny<int?>())).Returns(true);

        IPermissionVerifiable<TestAccountingIdentificationRequest> sut = CreateSut<TestAccountingIdentificationRequest>(
            _fixture!,
            _permissionCheckerMock!,
            _accountingGatewayMock!,
            isAuthenticated: true,
            hasAccountingAccess: true,
            isAccountingModifier: true);

        Mock<ISecurityContext> securityContextMock = _fixture!.CreateSecurityContextMock();
        TestAccountingIdentificationRequest request = CreateAccountingIdentificationRequest<TestAccountingIdentificationRequest>(_fixture!);

        await sut.VerifyPermissionAsync(securityContextMock.Object, request);

        _permissionCheckerMock!.Verify();
    }

    [Test]
    [Category("UnitTest")]
    public async Task VerifyPermissionAsync_WhenCalled_AssertHasAccountingAccessWasCalledBeforeIsAccountingModifier()
    {
        var callSequence = new MockSequence();
        _permissionCheckerMock!.InSequence(callSequence).Setup(m => m.IsAuthenticated(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns(true);
        _permissionCheckerMock!.InSequence(callSequence).Setup(m => m.HasAccountingAccess(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns(true);
        _permissionCheckerMock!.InSequence(callSequence).Setup(m => m.IsAccountingModifier(It.IsAny<System.Security.Claims.ClaimsPrincipal>(), It.IsAny<int?>())).Returns(true);

        IPermissionVerifiable<TestAccountingIdentificationRequest> sut = CreateSut<TestAccountingIdentificationRequest>(
            _fixture!,
            _permissionCheckerMock!,
            _accountingGatewayMock!,
            isAuthenticated: true,
            hasAccountingAccess: true,
            isAccountingModifier: true);

        Mock<ISecurityContext> securityContextMock = _fixture!.CreateSecurityContextMock();
        TestAccountingIdentificationRequest request = CreateAccountingIdentificationRequest<TestAccountingIdentificationRequest>(_fixture!);

        await sut.VerifyPermissionAsync(securityContextMock.Object, request);

        _permissionCheckerMock!.Verify();
    }

    #endregion

    #region Nested classes

    private class TestAccountingIdentificationRequest : OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Accounting.AccountingIdentificationRequestBase
    {
        public TestAccountingIdentificationRequest(Guid requestId, int accountingNumber, ISecurityContext securityContext)
            : base(requestId, accountingNumber, securityContext)
        {
        }
    }

    #endregion
}