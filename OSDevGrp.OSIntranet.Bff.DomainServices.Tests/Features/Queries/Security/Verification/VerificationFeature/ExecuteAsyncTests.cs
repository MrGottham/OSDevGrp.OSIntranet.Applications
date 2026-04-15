using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Security.Verification;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.UserHelper;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.VerificationCodeVerifier;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries.Security.Verification.VerificationFeature;

[TestFixture]
public class ExecuteAsyncTests
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
    public async Task ExecuteAsync_WhenCalled_AssertVerifyAsyncWasCalledOnVerificationCodeVerifierWithVerificationKeyFromGivenVerificationRequest()
    {
        IQueryFeature<VerificationRequest, VerificationResponse> sut = CreateSut();

        string verificationKey = _fixture!.Create<string>();
        VerificationRequest verificationRequest = CreateVerificationRequest(verificationKey: verificationKey);
        await sut.ExecuteAsync(verificationRequest);

        _verificationCodeVerifierMock!.Verify(m => m.VerifyAsync(
                It.Is<string>(value => value == verificationKey),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertVerifyAsyncWasCalledOnVerificationCodeVerifierWithVerificationCodeFromGivenVerificationRequest()
    {
        IQueryFeature<VerificationRequest, VerificationResponse> sut = CreateSut();

        string verificationCode = _fixture!.Create<string>();
        VerificationRequest verificationRequest = CreateVerificationRequest(verificationCode: verificationCode);
        await sut.ExecuteAsync(verificationRequest);

        _verificationCodeVerifierMock!.Verify(m => m.VerifyAsync(
                It.IsAny<string>(),
                It.Is<string>(value => value == verificationCode),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertVerifyAsyncWasCalledOnVerificationCodeVerifierWithGivenCancellationToken()
    {
        IQueryFeature<VerificationRequest, VerificationResponse> sut = CreateSut();

        VerificationRequest verificationRequest = CreateVerificationRequest();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.ExecuteAsync(verificationRequest, cancellationToken);

        _verificationCodeVerifierMock!.Verify(m => m.VerifyAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task ExecuteAsync_WhenCalled_ReturnsVerificationResponseWhereVerifiedIsEqualToVerifiedResultFromVerificationCodeVerifier(bool verifyResult)
    {
        IQueryFeature<VerificationRequest, VerificationResponse> sut = CreateSut(verifyResult: verifyResult);

        VerificationRequest verificationRequest = CreateVerificationRequest();
        VerificationResponse result = await sut.ExecuteAsync(verificationRequest);

        Assert.That(result.Verified, Is.EqualTo(verifyResult));
    }

    private IQueryFeature<VerificationRequest, VerificationResponse> CreateSut(bool verifyResult = true)
    {
        _verificationCodeVerifierMock!.Setup(verifyResult: verifyResult);

        return new DomainServices.Features.Queries.Security.Verification.VerificationFeature(_permissionCheckerMock!.Object, _verificationCodeVerifierMock!.Object);
    }

    private VerificationRequest CreateVerificationRequest(string? verificationKey = null, string? verificationCode = null)
    {
        return new VerificationRequest(Guid.NewGuid(), verificationKey ?? _fixture!.Create<string>(), verificationCode ?? _fixture!.Create<string>(), _fixture!.CreateSecurityContext());
    }
}