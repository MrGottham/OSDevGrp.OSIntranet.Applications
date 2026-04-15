using AutoFixture;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.VerificationCodeStorage;

[TestFixture]
public class StoreVerificationCodeAsyncTests
{
    #region Private variables

    private Mock<IMemoryCache>? _memoryCacheMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _memoryCacheMock = new Mock<IMemoryCache>();
        _fixture = new Fixture();
        _random = new Random(_fixture!.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task StoreVerificationCodeAsync_WhenCalled_AssertCreateEntryWasCalledOnMemoryCacheWithGivenVerificationKey()
    {
        IVerificationCodeStorage sut = CreateSut();

        string verificationKey = _fixture!.Create<string>();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.StoreVerificationCodeAsync(verificationKey, _fixture!.Create<string>(), DateTimeOffset.Now.AddSeconds(_random!.Next(60, 300)), cancellationTokenSource.Token);

        _memoryCacheMock!.Verify(m => m.CreateEntry(It.Is<string>(value => value == verificationKey)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task StoreVerificationCodeAsync_WhenCalled_AssertValueSetterWasCalledOnCacheEntryWithGivenVerificationCode()
    {
        Mock<ICacheEntry> createdCacheEntryMock = CreateCacheEntryMock();
        IVerificationCodeStorage sut = CreateSut(createdCacheEntry: createdCacheEntryMock.Object);

        string verificationCode = _fixture!.Create<string>();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.StoreVerificationCodeAsync(_fixture!.Create<string>(), verificationCode, DateTimeOffset.Now.AddSeconds(_random!.Next(60, 300)), cancellationTokenSource.Token);

        createdCacheEntryMock.VerifySet(m => m.Value = It.Is<string>(value => value == verificationCode), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task StoreVerificationCodeAsync_WhenCalled_AssertAbsoluteExpirationSetterWasCalledOnCacheEntryWithGivenExpires()
    {
        Mock<ICacheEntry> createdCacheEntryMock = CreateCacheEntryMock();
        IVerificationCodeStorage sut = CreateSut(createdCacheEntry: createdCacheEntryMock.Object);

        DateTimeOffset expires = DateTimeOffset.Now.AddSeconds(_random!.Next(60, 300));
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.StoreVerificationCodeAsync(_fixture!.Create<string>(), _fixture!.Create<string>(), expires, cancellationTokenSource.Token);

        createdCacheEntryMock.VerifySet(m => m.AbsoluteExpiration = It.Is<DateTimeOffset>(value => value == expires), Times.Once);
    }

    private IVerificationCodeStorage CreateSut(ICacheEntry? createdCacheEntry = null)
    {
        _memoryCacheMock!.Setup(m => m.CreateEntry(It.IsAny<object>()))
            .Returns(createdCacheEntry ?? CreateCacheEntry());
        return new DomainServices.Security.VerificationCodeStorage(_memoryCacheMock!.Object);
    }

    private ICacheEntry CreateCacheEntry()
    {
        return CreateCacheEntryMock().Object;
    }

    private Mock<ICacheEntry> CreateCacheEntryMock()
    {
        Mock<ICacheEntry> cacheEntryMock = new Mock<ICacheEntry>();
        cacheEntryMock.Setup(m => m.Key)
            .Returns(_fixture!.Create<object>());
        return cacheEntryMock;
    }
}