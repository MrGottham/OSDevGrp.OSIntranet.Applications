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
using OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData;
using System.Text;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Commands.Security.GenerateVerification.GenerateVerificationFeature;

[TestFixture]
public class ExecuteAsyncTests
{
    #region Private variables

    private Mock<IPermissionChecker>? _permissionCheckerMock;
    private Mock<IHashGenerator>? _hashGeneratorMock;
    private Mock<IVerificationCodeGenerator>? _verificationCodeGeneratorMock;
    private Mock<IVerificationCodeStorage>? _verificationCodeStorageMock;
    private Mock<ICaptchaGenerator>? _captchaGeneratorMock;
    private Mock<TimeProvider>? _timeProviderMock;
    private Fixture? _fixture;
    private Random? _random;

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
        _random = new Random(_fixture!.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertGenerateAsyncWasCalledOnHashGeneratorWithByteArrayForRequestIdFromGivenGenerateVerificationRequest()
    {
        ICommandFeature<GenerateVerificationRequest> sut = CreateSut();

        Guid requestId = Guid.NewGuid();
        GenerateVerificationRequest generateVerificationRequest = CreateGenerateVerificationRequest(requestId: requestId);
        await sut.ExecuteAsync(generateVerificationRequest);

        _hashGeneratorMock!.Verify(m => m.GenerateAsync(
                It.Is<byte[]>(value => Encoding.UTF8.GetString(value) == requestId.ToString("D")),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertGenerateAsyncWasCalledOnHashGeneratorWithGivenCancellationToken()
    {
        ICommandFeature<GenerateVerificationRequest> sut = CreateSut();

        GenerateVerificationRequest generateVerificationRequest = CreateGenerateVerificationRequest();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.ExecuteAsync(generateVerificationRequest, cancellationToken);

        _hashGeneratorMock!.Verify(m => m.GenerateAsync(
                It.IsAny<byte[]>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertGenerateAsyncWasCalledOnVerificationCodeGeneratorWithGivenCancellationToken()
    {
        ICommandFeature<GenerateVerificationRequest> sut = CreateSut();

        GenerateVerificationRequest generateVerificationRequest = CreateGenerateVerificationRequest();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.ExecuteAsync(generateVerificationRequest, cancellationToken);

        _verificationCodeGeneratorMock!.Verify(m => m.GenerateAsync(It.Is<CancellationToken>(value => value == cancellationToken)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertGenerateAsyncWasCalledOnCaptchaGeneratorWithGivenVerificationCodeGeneratedByVerificationCodeGenerator()
    {
        string verificationCode = _fixture!.Create<string>();
        ICommandFeature<GenerateVerificationRequest> sut = CreateSut(verificationCode: verificationCode);

        GenerateVerificationRequest generateVerificationRequest = CreateGenerateVerificationRequest();
        await sut.ExecuteAsync(generateVerificationRequest);

        _captchaGeneratorMock!.Verify(m => m.GenerateAsync(
                It.Is<string>(value => value == verificationCode),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertGenerateAsyncWasCalledOnCaptchaGeneratorWithGivenCancellationToken()
    {
        ICommandFeature<GenerateVerificationRequest> sut = CreateSut();

        GenerateVerificationRequest generateVerificationRequest = CreateGenerateVerificationRequest();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.ExecuteAsync(generateVerificationRequest, cancellationToken);

        _captchaGeneratorMock!.Verify(m => m.GenerateAsync(
                It.IsAny<string>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertGetUtcNowWasCalledOnTimeProvider()
    {
        ICommandFeature<GenerateVerificationRequest> sut = CreateSut();

        GenerateVerificationRequest generateVerificationRequest = CreateGenerateVerificationRequest();
        await sut.ExecuteAsync(generateVerificationRequest);

        _timeProviderMock!.Verify(m => m.GetUtcNow(), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertStoreVerificationCodeAsyncCalledOnVerificationCodeStorageWithHashGeneratedByHashGenerator()
    {
        string hash = _fixture!.Create<string>();
        ICommandFeature<GenerateVerificationRequest> sut = CreateSut(hash: hash);

        GenerateVerificationRequest generateVerificationRequest = CreateGenerateVerificationRequest();
        await sut.ExecuteAsync(generateVerificationRequest);

        _verificationCodeStorageMock!.Verify(m => m.StoreVerificationCodeAsync(
                It.Is<string>(value => value == hash),
                It.IsAny<string>(),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertStoreVerificationCodeAsyncCalledOnVerificationCodeStorageWithVerificationCodeGeneratedByVerificationCodeGenerator()
    {
        string verificationCode = _fixture!.Create<string>();
        ICommandFeature<GenerateVerificationRequest> sut = CreateSut(verificationCode: verificationCode);

        GenerateVerificationRequest generateVerificationRequest = CreateGenerateVerificationRequest();
        await sut.ExecuteAsync(generateVerificationRequest);

        _verificationCodeStorageMock!.Verify(m => m.StoreVerificationCodeAsync(
                It.IsAny<string>(),
                It.Is<string>(value => value == verificationCode),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertStoreVerificationCodeAsyncCalledOnVerificationCodeStorageWithExpiresBasesOnUtcNowFromTimeProvider()
    {
        DateTimeOffset utcNow = DateTimeOffset.UtcNow;
        ICommandFeature<GenerateVerificationRequest> sut = CreateSut(utcNow: utcNow);

        GenerateVerificationRequest generateVerificationRequest = CreateGenerateVerificationRequest();
        await sut.ExecuteAsync(generateVerificationRequest);

        _verificationCodeStorageMock!.Verify(m => m.StoreVerificationCodeAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.Is<DateTimeOffset>(value => value == utcNow.AddSeconds(150)),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertStoreVerificationCodeAsyncCalledOnVerificationCodeStorageWithGivenCancellationToken()
    {
        ICommandFeature<GenerateVerificationRequest> sut = CreateSut();

        GenerateVerificationRequest generateVerificationRequest = CreateGenerateVerificationRequest();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.ExecuteAsync(generateVerificationRequest, cancellationToken);

        _verificationCodeStorageMock!.Verify(m => m.StoreVerificationCodeAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<DateTimeOffset>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertOnVerificationCreatedWasCalledOnGenerateVerificationRequestWithHashGeneratedByHashGenerator()
    {
        string hash = _fixture!.Create<string>();
        ICommandFeature<GenerateVerificationRequest> sut = CreateSut(hash: hash);

        string? onVerificationCreatedCalledWith = null;
        Action<string, IReadOnlyCollection<byte>, DateTimeOffset> onVerificationCreated = (vk, _, _) => onVerificationCreatedCalledWith = vk;
        GenerateVerificationRequest generateVerificationRequest = CreateGenerateVerificationRequest(onVerificationCreated: onVerificationCreated);
        await sut.ExecuteAsync(generateVerificationRequest);
        
        Assert.That(onVerificationCreatedCalledWith, Is.EqualTo(hash));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertOnVerificationCreatedWasCalledOnGenerateVerificationRequestWithCaptchaImageGeneratedByCaptchaGenerator()
    {
        byte[] captchaImage = _fixture!.CreateMany<byte>(_random!.Next(1024, 4096)).ToArray();
        ICommandFeature<GenerateVerificationRequest> sut = CreateSut(captchaImage: captchaImage);

        IReadOnlyCollection<byte>? onVerificationCreatedCalledWith = null;
        Action<string, IReadOnlyCollection<byte>, DateTimeOffset> onVerificationCreated = (_, ci, _) => onVerificationCreatedCalledWith = ci;
        GenerateVerificationRequest generateVerificationRequest = CreateGenerateVerificationRequest(onVerificationCreated: onVerificationCreated);
        await sut.ExecuteAsync(generateVerificationRequest);
        
        Assert.That(onVerificationCreatedCalledWith, Is.EqualTo(captchaImage));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertOnVerificationCreatedWasCalledOnGenerateVerificationRequestWithExpiresBasesOnUtcNowFromTimeProvider()
    {
        DateTimeOffset utcNow = DateTimeOffset.UtcNow;
        ICommandFeature<GenerateVerificationRequest> sut = CreateSut(utcNow: utcNow);

        DateTimeOffset? onVerificationCreatedCalledWith = null;
        Action<string, IReadOnlyCollection<byte>, DateTimeOffset> onVerificationCreated = (_, _, e) => onVerificationCreatedCalledWith = e;
        GenerateVerificationRequest generateVerificationRequest = CreateGenerateVerificationRequest(onVerificationCreated: onVerificationCreated);
        await sut.ExecuteAsync(generateVerificationRequest);
        
        Assert.That(onVerificationCreatedCalledWith, Is.EqualTo(utcNow.AddSeconds(150)));
    }

    private ICommandFeature<GenerateVerificationRequest> CreateSut(string? hash = null, string? verificationCode = null, byte[]? captchaImage = null, DateTimeOffset? utcNow = null)
    {
        _hashGeneratorMock!.Setup(_fixture!, hash: hash);
        _verificationCodeGeneratorMock!.Setup(_fixture!, verificationCode: verificationCode);
        _verificationCodeStorageMock!.Setup(_fixture!);
        _captchaGeneratorMock!.Setup(_fixture!, _random!, captchaImage: captchaImage);

        _timeProviderMock!.Setup(m => m.GetUtcNow())
            .Returns(utcNow ?? DateTimeOffset.UtcNow);

        return new DomainServices.Features.Commands.Security.GenerateVerification.GenerateVerificationFeature(_permissionCheckerMock!.Object, _hashGeneratorMock!.Object, _verificationCodeGeneratorMock!.Object, _verificationCodeStorageMock!.Object, _captchaGeneratorMock!.Object, _timeProviderMock!.Object);
    }

    private GenerateVerificationRequest CreateGenerateVerificationRequest(Guid? requestId = null, Action<string, IReadOnlyCollection<byte>, DateTimeOffset>? onVerificationCreated = null)
    {
        return new GenerateVerificationRequest(requestId ?? Guid.NewGuid(), onVerificationCreated ?? ((_, _, _) => { }), _fixture!.CreateSecurityContext());
    }
}