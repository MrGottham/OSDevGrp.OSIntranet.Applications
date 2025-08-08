using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.VerificationCodeStorage;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.VerificationCodeVerifier;

[TestFixture]
public class VerifyAsyncTests
{
    #region Private variables

    private Mock<IVerificationCodeStorage>? _verificationCodeStorageMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _verificationCodeStorageMock = new Mock<IVerificationCodeStorage>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    public async Task VerifyAsync_WhenCalled_AssertGetVerificationCodeAsyncWasCalledOnVerificationCodeStorageWithGivenVerificationKey()
    {
        IVerificationCodeVerifier sut = CreateSut();

        string verificationKey = _fixture!.Create<string>();
        await sut.VerifyAsync(verificationKey, _fixture!.Create<string>());

        _verificationCodeStorageMock!.Verify(m => m.GetVerificationCodeAsync(
                It.Is<string>(value => value == verificationKey),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task VerifyAsync_WhenCalled_AssertGetVerificationCodeAsyncWasCalledOnVerificationCodeStorageWithGivenCancellationToken()
    {
        IVerificationCodeVerifier sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.VerifyAsync(_fixture!.Create<string>(), _fixture!.Create<string>(), cancellationToken);

        _verificationCodeStorageMock!.Verify(m => m.GetVerificationCodeAsync(
                It.IsAny<string>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task VerifyAsync_WhenNoVerificationCodeWasReturnedFromVerificationStorage_ReturnsFalse()
    {
        IVerificationCodeVerifier sut = CreateSut(hasVerificationCode: false);

        bool result = await sut.VerifyAsync(_fixture!.Create<string>(), _fixture!.Create<string>());

        Assert.That(result, Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public async Task VerifyAsync_WhenNonMatchingVerificationCodeWasReturnedFromVerificationStorage_ReturnsFalse()
    {
        string verificationCode = _fixture!.Create<string>();
        IVerificationCodeVerifier sut = CreateSut(hasVerificationCode: true, verificationCode: verificationCode);

        bool result = await sut.VerifyAsync(_fixture!.Create<string>(), _fixture!.Create<string>());

        Assert.That(result, Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public async Task VerifyAsync_WhenMatchingVerificationCodeWasReturnedFromVerificationStorage_ReturnsTrue()
    {
        string verificationCode = _fixture!.Create<string>();
        IVerificationCodeVerifier sut = CreateSut(hasVerificationCode: true, verificationCode: verificationCode);

        bool result = await sut.VerifyAsync(_fixture!.Create<string>(), verificationCode);

        Assert.That(result, Is.True);
    }

    private IVerificationCodeVerifier CreateSut(bool hasVerificationCode = true, string? verificationCode = null)
    {
        _verificationCodeStorageMock!.Setup(_fixture!, hasVerificationCode: hasVerificationCode, verificationCode: verificationCode);

        return new DomainServices.Security.VerificationCodeVerifier(_verificationCodeStorageMock!.Object);
    }
}