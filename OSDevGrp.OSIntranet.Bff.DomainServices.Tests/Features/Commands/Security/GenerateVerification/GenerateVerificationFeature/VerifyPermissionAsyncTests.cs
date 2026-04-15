using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Security.GenerateVerification;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.CaptchaGenerator;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.HashGenerator;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.UserHelper;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.VerificationCodeGenerator;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.VerificationCodeStorage;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Commands.Security.GenerateVerification.GenerateVerificationFeature;

[TestFixture]
public class VerifyPermissionAsyncTests
{
    #region Private variables

    private Mock<IPermissionChecker>? _permissionCheckerMock;
    private Mock<IHashGenerator>? _hashGeneratorMock;
    private Mock<IVerificationCodeGenerator>? _verificationCodeGeneratorMock;
    private Mock<IVerificationCodeStorage>? _verificationCodeStorageMock;
    private Mock<ICaptchaGenerator>? _captchaGeneratorMock;
    private Mock<TimeProvider>? _timeProviderMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _permissionCheckerMock = new Mock<IPermissionChecker>();
        _hashGeneratorMock = new Mock<IHashGenerator>();
        _verificationCodeGeneratorMock = new Mock<IVerificationCodeGenerator>();
        _verificationCodeStorageMock = new Mock<IVerificationCodeStorage>();
        _captchaGeneratorMock = new Mock<ICaptchaGenerator>();
        _timeProviderMock = new Mock<TimeProvider>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task VerifyPermissionAsync_WhenCalled_AssertUserWasCalledOnGivenSecurityContext(bool isAuthenticated)
    {
        IPermissionVerifiable<GenerateVerificationRequest> sut = CreateSut(isAuthenticated: isAuthenticated);

        Mock<ISecurityContext> securityContextMock = _fixture!.CreateSecurityContextMock();
        await sut.VerifyPermissionAsync(securityContextMock.Object, CreateGenerateVerificationRequest());

        securityContextMock.Verify(m => m.User, Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task VerifyPermissionAsync_WhenCalled_AssertIsAuthenticatedWasCalledOnPermissionCheckerWithUserFromGivenSecurityContext(bool isAuthenticated)
    {
        IPermissionVerifiable<GenerateVerificationRequest> sut = CreateSut(isAuthenticated: isAuthenticated);

        ClaimsPrincipal user = isAuthenticated ? _fixture!.CreateAuthenticatedClaimsPrincipal() : _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        ISecurityContext securityContext = _fixture!.CreateSecurityContext(user: user);
        await sut.VerifyPermissionAsync(securityContext, CreateGenerateVerificationRequest());

        _permissionCheckerMock!.Verify(m => m.IsAuthenticated(It.Is<ClaimsPrincipal>(value => value == user)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, true)]
    [TestCase(false, false)]
    public async Task VerifyPermissionAsync_WhenCalled_ReturnsExpectedValue(bool isAuthenticated, bool expectedValue)
    {
        IPermissionVerifiable<GenerateVerificationRequest> sut = CreateSut(isAuthenticated: isAuthenticated);

        bool result = await sut.VerifyPermissionAsync(_fixture!.CreateSecurityContext(), CreateGenerateVerificationRequest());

        Assert.That(result, Is.EqualTo(expectedValue));
    }

    private IPermissionVerifiable<GenerateVerificationRequest> CreateSut(bool isAuthenticated = true)
    {
        _permissionCheckerMock!.Setup(_fixture!, isAuthenticated: isAuthenticated);

        return new DomainServices.Features.Commands.Security.GenerateVerification.GenerateVerificationFeature(_permissionCheckerMock!.Object, _hashGeneratorMock!.Object, _verificationCodeGeneratorMock!.Object, _verificationCodeStorageMock!.Object, _captchaGeneratorMock!.Object, _timeProviderMock!.Object);
    }

    private GenerateVerificationRequest CreateGenerateVerificationRequest()
    {
        return new GenerateVerificationRequest(Guid.NewGuid(), (_, _, _) => { }, _fixture!.CreateSecurityContext());
    }
}