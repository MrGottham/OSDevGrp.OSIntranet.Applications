using AutoFixture;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.WebApi.Security;
using OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security.TokenKeyProvider;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security.TokenStorage;

[TestFixture]
public class GetTokenAsyncTests : TokenStorageTestBase
{
    #region Private variables

    private Mock<IMemoryCache>? _memoryCacheMock;
    private Mock<ITokenKeyProvider>? _tokenKeyProviderMock;
    private Mock<ITokenProvider>? _tokenProviderMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _memoryCacheMock = new Mock<IMemoryCache>();
        _tokenKeyProviderMock = new Mock<ITokenKeyProvider>();
        _tokenProviderMock = new Mock<ITokenProvider>();
        _fixture = new Fixture();
        _random = new Random(_fixture.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task GetTokenAsync_WhenCalled_AssertResolveAsyncWasCalledOnTokenKeyProviderWithGivenClaimsPrincipal(bool authenticatedUser)
    {
        ITokenStorage sut = CreateSut();

        ClaimsPrincipal user = authenticatedUser ? _fixture!.CreateAuthenticatedClaimsPrincipal() : _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        await sut.GetTokenAsync(user);

        _tokenKeyProviderMock!.Verify(m => m.ResolveAsync(
                It.Is<ClaimsPrincipal>(value => value == user),
                It.IsAny<CancellationToken>()),
            Times.Once());
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetTokenAsync_WhenCalled_AssertResolveAsyncWasCalledOnTokenKeyProviderWithGivenCancellationToken()
    {
        ITokenStorage sut = CreateSut();

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.GetTokenAsync(user, cancellationToken);

        _tokenKeyProviderMock!.Verify(m => m.ResolveAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once());
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetTokenAsync_WhenCalled_AssertTryGetValueWasCalledOnMemoryCacheWithTokenKeyResolvedByTokenKeyProvider()
    {
        string tokenKey = _fixture!.Create<string>();
        ITokenStorage sut = CreateSut(tokenKey: tokenKey);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        await sut.GetTokenAsync(user);

        _memoryCacheMock!.Verify(m => m.TryGetValue(It.Is<object>(value => value.Equals(tokenKey)), out It.Ref<object?>.IsAny), Times.Once());
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetTokenAsync_WhenTokenWasResolvedByMemoryCache_AssertExpiresWasCalledOnTokenResolvedByMemoryCache()
    {
        Mock<IToken> cachedTokenMock = _fixture!.CreateTokenMock(_random!);
        ITokenStorage sut = CreateSut(hasCachedToken: true, cachedToken: cachedTokenMock.Object);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        await sut.GetTokenAsync(user);

        cachedTokenMock.Verify(m => m.Expired, Times.Once());
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetTokenAsync_WhenNonExpiredTokenWasResolvedByMemoryCache_AssertResolveAsyncWasNotCalledOnTokenProvider()
    {
        IToken cachedToken = _fixture!.CreateToken(_random!, expired: false);
        ITokenStorage sut = CreateSut(hasCachedToken: true, cachedToken: cachedToken);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        await sut.GetTokenAsync(user);

        _tokenProviderMock!.Verify(m => m.ResolveAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<CancellationToken>()), 
            Times.Never());
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetTokenAsync_WhenNonExpiredTokenWasResolvedByMemoryCache_AssertCreateEntryWasNotCalledOnMemoryCache()
    {
        IToken cachedToken = _fixture!.CreateToken(_random!, expired: false);
        ITokenStorage sut = CreateSut(hasCachedToken: true, cachedToken: cachedToken);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        await sut.GetTokenAsync(user);

        _memoryCacheMock!.Verify(m => m.CreateEntry(It.IsAny<object>()), Times.Never());
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetTokenAsync_WhenNonExpiredTokenWasResolvedByMemoryCache_ReturnsTokenResolvedByMemoryCache()
    {
        IToken cachedToken = _fixture!.CreateToken(_random!, expired: false);
        ITokenStorage sut = CreateSut(hasCachedToken: true, cachedToken: cachedToken);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        IToken result = await sut.GetTokenAsync(user);

        Assert.That(result, Is.EqualTo(cachedToken));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task GetTokenAsync_WhenExpiredTokenWasResolvedByMemoryCache_AssertResolveAsyncWasCalledOnTokenProviderWithGivenClaimsPrincipal(bool authenticatedUser)
    {
        IToken cachedToken = _fixture!.CreateToken(_random!, expired: true);
        ITokenStorage sut = CreateSut(hasCachedToken: true, cachedToken: cachedToken);

        ClaimsPrincipal user = authenticatedUser ? _fixture!.CreateAuthenticatedClaimsPrincipal() : _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        await sut.GetTokenAsync(user);

        _tokenProviderMock!.Verify(m => m.ResolveAsync(
                It.Is<ClaimsPrincipal>(value => value == user),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetTokenAsync_WhenExpiredTokenWasResolvedByMemoryCache_AssertResolveAsyncWasCalledOnTokenProviderWithGivenCancellationToken()
    {
        IToken cachedToken = _fixture!.CreateToken(_random!, expired: true);
        ITokenStorage sut = CreateSut(hasCachedToken: true, cachedToken: cachedToken);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.GetTokenAsync(user, cancellationToken);

        _tokenProviderMock!.Verify(m => m.ResolveAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.Is<CancellationToken>(value => value == cancellationToken)), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetTokenAsync_WhenExpiredTokenWasResolvedByMemoryCache_AssertExpiresWasCalledOnTokenResolvedByTokenProvider()
    {
        IToken cachedToken = _fixture!.CreateToken(_random!, expired: true);
        Mock<IToken> tokenResolvedByTokenProviderMock = _fixture!.CreateTokenMock(_random!);
        ITokenStorage sut = CreateSut(hasCachedToken: true, cachedToken: cachedToken, tokenResolvedByTokenProvider:tokenResolvedByTokenProviderMock.Object);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        await sut.GetTokenAsync(user);

        tokenResolvedByTokenProviderMock.Verify(m => m.Expires, Times.Once());
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetTokenAsync_WhenExpiredTokenWasResolvedByMemoryCache_AssertCreateEntryWasCalledOnMemoryCacheWithTokenKeyResolvedByTokenKeyProvider()
    {
        string tokenKey = _fixture!.Create<string>();
        IToken cachedToken = _fixture!.CreateToken(_random!, expired: true);
        ITokenStorage sut = CreateSut(hasCachedToken: true, cachedToken: cachedToken, tokenKey: tokenKey);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        await sut.GetTokenAsync(user);

        _memoryCacheMock!.Verify(m => m.CreateEntry(It.Is<object>(value => value.Equals(tokenKey))), Times.Once());
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetTokenAsync_WhenExpiredTokenWasResolvedByMemoryCache_AssertValueSetterWasCalledOnCacheEntryCreatedByMemoryCacheWithTokenResolvedByTokenProvider()
    {
        IToken cachedToken = _fixture!.CreateToken(_random!, expired: true);
        IToken tokenResolvedByTokenProvider = _fixture!.CreateToken(_random!);
        Mock<ICacheEntry> createdCacheEntryMock = CreateCacheEntryMock(_fixture!, _random!);
        ITokenStorage sut = CreateSut(hasCachedToken: true, cachedToken: cachedToken, tokenResolvedByTokenProvider: tokenResolvedByTokenProvider, createdCacheEntry: createdCacheEntryMock.Object);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        await sut.GetTokenAsync(user);

        createdCacheEntryMock.VerifySet(m => m.Value = It.Is<IToken>(value => value == tokenResolvedByTokenProvider), Times.Once());
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetTokenAsync_WhenExpiredTokenWasResolvedByMemoryCache_AssertAbsoluteExpirationSetterWasCalledOnCacheEntryCreatedByMemoryCacheWithExpiresFromTokenResolvedByTokenProvider()
    {
        IToken cachedToken = _fixture!.CreateToken(_random!, expired: true);
        DateTimeOffset expires = DateTimeOffset.UtcNow.AddMinutes(_random!.Next(5, 60));
        IToken tokenResolvedByTokenProvider = _fixture!.CreateToken(_random!, expires: expires);
        Mock<ICacheEntry> createdCacheEntryMock = CreateCacheEntryMock(_fixture!, _random!);
        ITokenStorage sut = CreateSut(hasCachedToken: true, cachedToken: cachedToken, tokenResolvedByTokenProvider: tokenResolvedByTokenProvider, createdCacheEntry: createdCacheEntryMock.Object);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        await sut.GetTokenAsync(user);

        createdCacheEntryMock.VerifySet(m => m.AbsoluteExpiration = It.Is<DateTimeOffset>(value => value == expires), Times.Once());
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetTokenAsync_WhenExpiredTokenWasResolvedByMemoryCache_ReturnsTokenResolvedByTokenProvider()
    {
        IToken cachedToken = _fixture!.CreateToken(_random!, expired: true);
        IToken tokenResolvedByTokenProvider = _fixture!.CreateToken(_random!);
        ITokenStorage sut = CreateSut(hasCachedToken: true, cachedToken: cachedToken, tokenResolvedByTokenProvider: tokenResolvedByTokenProvider);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        IToken result = await sut.GetTokenAsync(user);

        Assert.That(result, Is.EqualTo(tokenResolvedByTokenProvider));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task GetTokenAsync_WhenNoTokenWasResolvedByMemoryCache_AssertResolveAsyncWasCalledOnTokenProviderWithGivenClaimsPrincipal(bool authenticatedUser)
    {
        ITokenStorage sut = CreateSut(hasCachedToken: false);

        ClaimsPrincipal user = authenticatedUser ? _fixture!.CreateAuthenticatedClaimsPrincipal() : _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        await sut.GetTokenAsync(user);

        _tokenProviderMock!.Verify(m => m.ResolveAsync(
                It.Is<ClaimsPrincipal>(value => value == user),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetTokenAsync_WhenNoTokenWasResolvedByMemoryCache_AssertResolveAsyncWasCalledOnTokenProviderWithGivenCancellationToken()
    {
        ITokenStorage sut = CreateSut(hasCachedToken: false);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.GetTokenAsync(user, cancellationToken);

        _tokenProviderMock!.Verify(m => m.ResolveAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.Is<CancellationToken>(value => value == cancellationToken)), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetTokenAsync_WhenNoTokenWasResolvedByMemoryCache_AssertExpiresWasCalledOnTokenResolvedByTokenProvider()
    {
        Mock<IToken> tokenResolvedByTokenProviderMock = _fixture!.CreateTokenMock(_random!);
        ITokenStorage sut = CreateSut(hasCachedToken: false, tokenResolvedByTokenProvider:tokenResolvedByTokenProviderMock.Object);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        await sut.GetTokenAsync(user);

        tokenResolvedByTokenProviderMock.Verify(m => m.Expires, Times.Once());
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetTokenAsync_WhenNoTokenWasResolvedByMemoryCache_AssertCreateEntryWasCalledOnMemoryCacheWithTokenKeyResolvedByTokenKeyProvider()
    {
        string tokenKey = _fixture!.Create<string>();
        ITokenStorage sut = CreateSut(hasCachedToken: false, tokenKey: tokenKey);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        await sut.GetTokenAsync(user);

        _memoryCacheMock!.Verify(m => m.CreateEntry(It.Is<object>(value => value.Equals(tokenKey))), Times.Once());
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetTokenAsync_WhenNoTokenWasResolvedByMemoryCache_AssertValueSetterWasCalledOnCacheEntryCreatedByMemoryCacheWithTokenResolvedByTokenProvider()
    {
        IToken tokenResolvedByTokenProvider = _fixture!.CreateToken(_random!);
        Mock<ICacheEntry> createdCacheEntryMock = CreateCacheEntryMock(_fixture!, _random!);
        ITokenStorage sut = CreateSut(hasCachedToken: false, tokenResolvedByTokenProvider: tokenResolvedByTokenProvider, createdCacheEntry: createdCacheEntryMock.Object);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        await sut.GetTokenAsync(user);

        createdCacheEntryMock.VerifySet(m => m.Value = It.Is<IToken>(value => value == tokenResolvedByTokenProvider), Times.Once());
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetTokenAsync_WhenNoTokenWasResolvedByMemoryCache_AssertAbsoluteExpirationSetterWasCalledOnCacheEntryCreatedByMemoryCacheWithExpiresFromTokenResolvedByTokenProvider()
    {
        DateTimeOffset expires = DateTimeOffset.UtcNow.AddMinutes(_random!.Next(5, 60));
        IToken tokenResolvedByTokenProvider = _fixture!.CreateToken(_random!, expires: expires);
        Mock<ICacheEntry> createdCacheEntryMock = CreateCacheEntryMock(_fixture!, _random!);
        ITokenStorage sut = CreateSut(hasCachedToken: false, tokenResolvedByTokenProvider: tokenResolvedByTokenProvider, createdCacheEntry: createdCacheEntryMock.Object);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        await sut.GetTokenAsync(user);

        createdCacheEntryMock.VerifySet(m => m.AbsoluteExpiration = It.Is<DateTimeOffset>(value => value == expires), Times.Once());
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetTokenAsync_WhenNoTokenWasResolvedByMemoryCache_ReturnsTokenResolvedByTokenProvider()
    {
        IToken tokenResolvedByTokenProvider = _fixture!.CreateToken(_random!);
        ITokenStorage sut = CreateSut(hasCachedToken: false, tokenResolvedByTokenProvider: tokenResolvedByTokenProvider);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        IToken result = await sut.GetTokenAsync(user);

        Assert.That(result, Is.EqualTo(tokenResolvedByTokenProvider));
    }

    private ITokenStorage CreateSut(string? tokenKey = null, bool hasCachedToken = true, IToken? cachedToken = null, IToken? tokenResolvedByTokenProvider = null, ICacheEntry? createdCacheEntry = null)
    {
        object? cachedObject = hasCachedToken ? cachedToken ?? _fixture!.CreateToken(_random!) : null;
        _memoryCacheMock!.Setup(m => m.TryGetValue(It.IsAny<object>(), out cachedObject))
            .Returns(cachedObject != null);
        _memoryCacheMock!.Setup(m => m.CreateEntry(It.IsAny<object>()))
            .Returns(createdCacheEntry ?? CreateCacheEntry(_fixture!, _random!));

        _tokenKeyProviderMock!.Setup(_fixture!, tokenKey: tokenKey);
        _tokenProviderMock!.Setup(_fixture!, _random!, token: tokenResolvedByTokenProvider);

        return new WebApi.Security.TokenStorage(_memoryCacheMock!.Object, _tokenKeyProviderMock!.Object, _tokenProviderMock!.Object);
    }
}