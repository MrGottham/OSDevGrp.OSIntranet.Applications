using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.VerificationCodeStorage;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.VerificationCodeVerifier;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Cqs.PipelineExtensions.FeatureHumanVerifier.CommandFeatureHumanVerifier;

[TestFixture]
public class ExecuteAsyncTests
{
    #region Private variables

    private Mock<ICommandFeature<IRequest>>? _innerFeatureMock;
    private Mock<IVerificationCodeVerifier>? _verificationCodeVerifierMock;
    private Mock<IVerificationCodeStorage>? _verificationCodeStorageMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _innerFeatureMock = new Mock<ICommandFeature<IRequest>>();
        _verificationCodeVerifierMock = new Mock<IVerificationCodeVerifier>();
        _verificationCodeStorageMock = new Mock<IVerificationCodeStorage>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenRequestIsNotHumanVerifiable_AssertVerifyAsyncWasNotCalledOnVerificationCodeVerifier()
    {
        ICommandFeature<IRequest> sut = CreateSut();

        IRequest request = CreateRequest(isHumanVerifiable: false);
        await sut.ExecuteAsync(request);

        _verificationCodeVerifierMock!.Verify(m => m.VerifyAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenRequestIsNotHumanVerifiable_AssertRemoveVerificationCodeAsyncWasNotCalledOnVerificationCodeStorage()
    {
        ICommandFeature<IRequest> sut = CreateSut();

        IRequest request = CreateRequest(isHumanVerifiable: false);
        await sut.ExecuteAsync(request);

        _verificationCodeStorageMock!.Verify(m => m.RemoveVerificationCodeAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenRequestIsNotHumanVerifiable_AssertExecuteAsyncWasCalledOnInnerFeatureWithGivenRequest()
    {
        ICommandFeature<IRequest> sut = CreateSut();

        IRequest request = CreateRequest(isHumanVerifiable: false);
        await sut.ExecuteAsync(request);

        _innerFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<IRequest>(value => value == request),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenRequestIsNotHumanVerifiable_AssertExecuteAsyncWasCalledOnInnerFeatureWithGivenCancellationToken()
    {
        ICommandFeature<IRequest> sut = CreateSut();

        IRequest request = CreateRequest(isHumanVerifiable: false);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token; 
        await sut.ExecuteAsync(request, cancellationToken);

        _innerFeatureMock!.Verify(m => m.ExecuteAsync(
                It.IsAny<IRequest>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenRequestIsHumanVerifiable_AssertVerificationKeyWasCalledOnHumanVerifiableRequest()
    {
        ICommandFeature<IRequest> sut = CreateSut();

        Mock<IRequest> requestMock = CreateRequestMock(isHumanVerifiable: true);
        await sut.ExecuteAsync(requestMock.Object);

        requestMock.As<IHumanVerifiableRequest>().Verify(m => m.VerificationKey, Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenRequestIsHumanVerifiable_AssertVerificationCodeWasCalledOnHumanVerifiableRequest()
    {
        ICommandFeature<IRequest> sut = CreateSut();

        Mock<IRequest> requestMock = CreateRequestMock(isHumanVerifiable: true);
        await sut.ExecuteAsync(requestMock.Object);

        requestMock.As<IHumanVerifiableRequest>().Verify(m => m.VerificationCode, Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenRequestIsHumanVerifiable_AssertVerifyAsyncWasCalledOnVerificationCodeVerifierWithVerificationKeyFromHumanVerifiableRequest()
    {
        ICommandFeature<IRequest> sut = CreateSut();

        string verificationKey = _fixture!.Create<string>();
        IRequest request = CreateRequest(isHumanVerifiable: true, verificationKey: verificationKey);
        await sut.ExecuteAsync(request);

        _verificationCodeVerifierMock!.Verify(m => m.VerifyAsync(
                It.Is<string>(value => value == verificationKey),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenRequestIsHumanVerifiable_AssertVerifyAsyncWasCalledOnVerificationCodeVerifierWithVerificationCodeFromHumanVerifiableRequest()
    {
        ICommandFeature<IRequest> sut = CreateSut();

        string verificationCode = _fixture!.Create<string>();
        IRequest request = CreateRequest(isHumanVerifiable: true, verificationCode: verificationCode);
        await sut.ExecuteAsync(request);

        _verificationCodeVerifierMock!.Verify(m => m.VerifyAsync(
                It.IsAny<string>(),
                It.Is<string>(value => value == verificationCode),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenRequestIsHumanVerifiable_AssertVerifyAsyncWasCalledOnVerificationCodeVerifierWithGivenCancellationToken()
    {
        ICommandFeature<IRequest> sut = CreateSut();

        IRequest request = CreateRequest(isHumanVerifiable: true);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token; 
        await sut.ExecuteAsync(request, cancellationToken);

        _verificationCodeVerifierMock!.Verify(m => m.VerifyAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public void ExecuteAsync_WhenVerificationCodeNotVerifiedByVerificationCodeVerifier_AssertExecuteAsyncWasNotCalledOnInnerFeature()
    {
        ICommandFeature<IRequest> sut = CreateSut(verifyResult: false);

        IRequest request = CreateRequest(isHumanVerifiable: true);
        Assert.ThrowsAsync<VerificationFailedException>(async () => await sut.ExecuteAsync(request));

        _innerFeatureMock!.Verify(m => m.ExecuteAsync(
                It.IsAny<IRequest>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public void ExecuteAsync_WhenVerificationCodeNotVerifiedByVerificationCodeVerifier_AssertRemoveVerificationCodeAsyncWasCalledOnVerificationCodeStorageWithVerificationKeyFromHumanVerifiableRequest()
    {
        ICommandFeature<IRequest> sut = CreateSut(verifyResult: false);

        string verificationKey = _fixture!.Create<string>();
        IRequest request = CreateRequest(isHumanVerifiable: true, verificationKey: verificationKey);
        Assert.ThrowsAsync<VerificationFailedException>(async () => await sut.ExecuteAsync(request));

        _verificationCodeStorageMock!.Verify(m => m.RemoveVerificationCodeAsync(
                It.Is<string>(value => value == verificationKey),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public void ExecuteAsync_WhenVerificationCodeNotVerifiedByVerificationCodeVerifier_AssertRemoveVerificationCodeAsyncWasCalledOnVerificationCodeStorageWithGivenCancellationToken()
    {
        ICommandFeature<IRequest> sut = CreateSut(verifyResult: false);

        IRequest request = CreateRequest(isHumanVerifiable: true);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token; 
        Assert.ThrowsAsync<VerificationFailedException>(async () => await sut.ExecuteAsync(request, cancellationToken));

        _verificationCodeStorageMock!.Verify(m => m.RemoveVerificationCodeAsync(
                It.IsAny<string>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public void ExecuteAsync_WhenVerificationCodeNotVerifiedByVerificationCodeVerifier_ThrowsVerificationFailedException()
    {
        ICommandFeature<IRequest> sut = CreateSut(verifyResult: false);

        IRequest request = CreateRequest(isHumanVerifiable: true);
        VerificationFailedException? result = Assert.ThrowsAsync<VerificationFailedException>(async () => await sut.ExecuteAsync(request));

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void ExecuteAsync_WhenVerificationCodeNotVerifiedByVerificationCodeVerifier_ThrowsVerificationFailedExceptionWithSpecificMessage()
    {
        ICommandFeature<IRequest> sut = CreateSut(verifyResult: false);

        IRequest request = CreateRequest(isHumanVerifiable: true);
        VerificationFailedException? result = Assert.ThrowsAsync<VerificationFailedException>(async () => await sut.ExecuteAsync(request));

        Assert.That(result!.Message, Is.EqualTo("Unable to verify the given verification code."));
    }

    [Test]
    [Category("UnitTest")]
    public void ExecuteAsync_WhenVerificationCodeNotVerifiedByVerificationCodeVerifier_ThrowsVerificationFailedExceptionWithInnerExceptionEqualToNull()
    {
        ICommandFeature<IRequest> sut = CreateSut(verifyResult: false);

        IRequest request = CreateRequest(isHumanVerifiable: true);
        VerificationFailedException? result = Assert.ThrowsAsync<VerificationFailedException>(async () => await sut.ExecuteAsync(request));

        Assert.That(result!.InnerException, Is.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenVerificationCodeVerifiedByVerificationCodeVerifier_AssertExecuteAsyncWasCalledOnInnerFeatureWithGivenRequest()
    {
        ICommandFeature<IRequest> sut = CreateSut(verifyResult: true);

        IRequest request = CreateRequest(isHumanVerifiable: true);
        await sut.ExecuteAsync(request);

        _innerFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<IRequest>(value => value == request),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenVerificationCodeVerifiedByVerificationCodeVerifier_AssertExecuteAsyncWasCalledOnInnerFeatureWithGivenCancellationToken()
    {
        ICommandFeature<IRequest> sut = CreateSut(verifyResult: true);

        IRequest request = CreateRequest(isHumanVerifiable: true);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token; 
        await sut.ExecuteAsync(request, cancellationToken);

        _innerFeatureMock!.Verify(m => m.ExecuteAsync(
                It.IsAny<IRequest>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenVerificationCodeVerifiedByVerificationCodeVerifier_AssertRemoveVerificationCodeAsyncWasCalledOnVerificationCodeStorageWithVerificationKeyFromHumanVerifiableRequest()
    {
        ICommandFeature<IRequest> sut = CreateSut(verifyResult: true);

        string verificationKey = _fixture!.Create<string>();
        IRequest request = CreateRequest(isHumanVerifiable: true, verificationKey: verificationKey);
        await sut.ExecuteAsync(request);

        _verificationCodeStorageMock!.Verify(m => m.RemoveVerificationCodeAsync(
                It.Is<string>(value => value == verificationKey),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenVerificationCodeVerifiedByVerificationCodeVerifier_AssertRemoveVerificationCodeAsyncWasCalledOnVerificationCodeStorageWithGivenCancellationToken()
    {
        ICommandFeature<IRequest> sut = CreateSut(verifyResult: true);

        IRequest request = CreateRequest(isHumanVerifiable: true);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token; 
        await sut.ExecuteAsync(request, cancellationToken);

        _verificationCodeStorageMock!.Verify(m => m.RemoveVerificationCodeAsync(
                It.IsAny<string>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    private ICommandFeature<IRequest> CreateSut(bool verifyResult = true)
    {
        _verificationCodeVerifierMock!.Setup(verifyResult: verifyResult);
        _verificationCodeStorageMock!.Setup(_fixture!);

        _innerFeatureMock!.Setup(m => m.ExecuteAsync(It.IsAny<IRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        return new DomainServices.Cqs.PipelineExtensions.FeatureHumanVerifier.CommandFeatureHumanVerifier<IRequest>(_innerFeatureMock!.Object, _verificationCodeVerifierMock!.Object, _verificationCodeStorageMock!.Object);
    }

    private IRequest CreateRequest(bool isHumanVerifiable = true, string? verificationKey = null, string? verificationCode = null)
    {
        return CreateRequestMock(isHumanVerifiable, verificationKey, verificationCode).Object;
    }

    private Mock<IRequest> CreateRequestMock(bool isHumanVerifiable = true, string? verificationKey = null, string? verificationCode = null)
    {
        Mock<IRequest> requestMock = new Mock<IRequest>();
        if (isHumanVerifiable == false)
        {
            return requestMock;
        }

        Mock<IHumanVerifiableRequest> humanVerifiableRequestMock = requestMock.As<IHumanVerifiableRequest>();
        humanVerifiableRequestMock.Setup(m => m.VerificationKey)
            .Returns(verificationKey ?? _fixture!.Create<string>());
        humanVerifiableRequestMock.Setup(m => m.VerificationCode)
            .Returns(verificationCode ?? _fixture!.Create<string>());

        return requestMock;
    }
}