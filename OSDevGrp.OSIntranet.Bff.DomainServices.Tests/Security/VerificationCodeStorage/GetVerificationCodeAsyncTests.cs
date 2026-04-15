using AutoFixture;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.VerificationCodeStorage;

[TestFixture]
public class GetVerificationCodeAsyncTests
{
    #region Private variables

    private Mock<IMemoryCache>? _memoryCacheMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _memoryCacheMock = new Mock<IMemoryCache>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetVerificationCodeAsync_WhenCalled_AssertTryGetValueWasCalledOnMemoryCacheWithGivenVerificationKey()
    {
        IVerificationCodeStorage sut = CreateSut();

        string verificationKey = _fixture!.Create<string>();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.GetVerificationCodeAsync(verificationKey, cancellationTokenSource.Token);

        _memoryCacheMock!.Verify(m => m.TryGetValue(
                It.Is<string>(value => value == verificationKey),
                out It.Ref<object?>.IsAny),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetVerificationCodeAsync_WhenVerificationCodeHasBeenCachedForVerificationKey_ReturnsNotNull()
    {
        IVerificationCodeStorage sut = CreateSut(hasCachedVerificationKey: true);

        string verificationKey = _fixture!.Create<string>();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        string? result = await sut.GetVerificationCodeAsync(verificationKey, cancellationTokenSource.Token);

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetVerificationCodeAsync_WhenVerificationCodeHasBeenCachedForVerificationKey_ReturnsCachedVerificationCode()
    {
        string cachedVerificationCode = _fixture!.Create<string>();
        IVerificationCodeStorage sut = CreateSut(hasCachedVerificationKey: true, cachedVerificationCode: cachedVerificationCode);

        string verificationKey = _fixture!.Create<string>();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        string? result = await sut.GetVerificationCodeAsync(verificationKey, cancellationTokenSource.Token);

        Assert.That(result, Is.EqualTo(cachedVerificationCode));
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetVerificationCodeAsync_WhenVerificationCodeHasNotBeenCachedForVerificationKey_ReturnsNull()
    {
        IVerificationCodeStorage sut = CreateSut(hasCachedVerificationKey: false);

        string verificationKey = _fixture!.Create<string>();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        string? result = await sut.GetVerificationCodeAsync(verificationKey, cancellationTokenSource.Token);

        Assert.That(result, Is.Null);
    }

    private IVerificationCodeStorage CreateSut(bool hasCachedVerificationKey = true, object? cachedVerificationCode = null)
    {
        object? cachedObject = hasCachedVerificationKey ? cachedVerificationCode ?? _fixture!.Create<string>() : null;
        _memoryCacheMock!.Setup(m => m.TryGetValue(It.IsAny<object>(), out cachedObject))
            .Returns(cachedObject != null);

        return new DomainServices.Security.VerificationCodeStorage(_memoryCacheMock!.Object);
    }
}