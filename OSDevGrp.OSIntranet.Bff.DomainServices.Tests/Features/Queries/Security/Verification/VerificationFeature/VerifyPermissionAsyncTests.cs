using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Security.Verification;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.UserHelper;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries.Security.Verification.VerificationFeature;

[TestFixture]
public class VerifyPermissionAsyncTests
{
    #region Private variables

    private Mock<IPermissionChecker>? _permissionCheckerMock;
    private Mock<IVerificationCodeVerifier>? _verificationCodeVerifierMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _permissionCheckerMock = new Mock<IPermissionChecker>();
        _verificationCodeVerifierMock = new Mock<IVerificationCodeVerifier>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task VerifyPermissionAsync_WhenCalled_AssertUserWasCalledOnGivenSecurityContext(bool isAuthenticated)
    {
        IPermissionVerifiable<VerificationRequest> sut = CreateSut(isAuthenticated: isAuthenticated);

        Mock<ISecurityContext> securityContextMock = _fixture!.CreateSecurityContextMock();
        await sut.VerifyPermissionAsync(securityContextMock.Object, CreateVerificationRequest());

        securityContextMock.Verify(m => m.User, Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task VerifyPermissionAsync_WhenCalled_AssertIsAuthenticatedWasCalledOnPermissionCheckerWithUserFromGivenSecurityContext(bool isAuthenticated)
    {
        IPermissionVerifiable<VerificationRequest> sut = CreateSut(isAuthenticated: isAuthenticated);

        ClaimsPrincipal user = isAuthenticated ? _fixture!.CreateAuthenticatedClaimsPrincipal() : _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        ISecurityContext securityContext = _fixture!.CreateSecurityContext(user: user);
        await sut.VerifyPermissionAsync(securityContext, CreateVerificationRequest());

        _permissionCheckerMock!.Verify(m => m.IsAuthenticated(It.Is<ClaimsPrincipal>(value => value == user)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, true)]
    [TestCase(false, false)]
    public async Task VerifyPermissionAsync_WhenCalled_ReturnsExpectedValue(bool isAuthenticated, bool expectedValue)
    {
        IPermissionVerifiable<VerificationRequest> sut = CreateSut(isAuthenticated: isAuthenticated);

        bool result = await sut.VerifyPermissionAsync(_fixture!.CreateSecurityContext(), CreateVerificationRequest());

        Assert.That(result, Is.EqualTo(expectedValue));
    }

    private IPermissionVerifiable<VerificationRequest> CreateSut(bool isAuthenticated = true)
    {
        _permissionCheckerMock!.Setup(_fixture!, isAuthenticated: isAuthenticated);

        return new DomainServices.Features.Queries.Security.Verification.VerificationFeature(_permissionCheckerMock!.Object, _verificationCodeVerifierMock!.Object);
    }

    private VerificationRequest CreateVerificationRequest()
    {
        return new VerificationRequest(Guid.NewGuid(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture!.CreateSecurityContext());
    }
}