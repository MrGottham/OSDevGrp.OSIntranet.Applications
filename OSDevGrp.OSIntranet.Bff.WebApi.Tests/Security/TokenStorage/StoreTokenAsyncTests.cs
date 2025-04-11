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
public class StoreTokenAsyncTests : TokenStorageTestBase
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
    public async Task StoreTokenAsync_WhenCalled_AssertResolveAsyncWasCalledOnTokenKeyProviderWithGivenClaimsPrincipal(bool authenticatedUser)
    {
        ITokenStorage sut = CreateSut();

        ClaimsPrincipal user = authenticatedUser ? _fixture!.CreateAuthenticatedClaimsPrincipal() : _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        IToken token = _fixture!.CreateToken(_random!);
        await sut.StoreTokenAsync(user, token);

        _tokenKeyProviderMock!.Verify(m => m.ResolveAsync(
                It.Is<ClaimsPrincipal>(value => value == user),
                It.IsAny<CancellationToken>()),
            Times.Once());
    }

    [Test]
    [Category("UnitTest")]
    public async Task StoreTokenAsync_WhenCalled_AssertResolveAsyncWasCalledOnTokenKeyProviderWithGivenCancellationToken()
    {
        ITokenStorage sut = CreateSut();

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        IToken token = _fixture!.CreateToken(_random!);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.StoreTokenAsync(user, token, cancellationToken);

        _tokenKeyProviderMock!.Verify(m => m.ResolveAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once());
    }

    [Test]
    [Category("UnitTest")]
    public async Task StoreTokenAsync_WhenCalled_AssertExpiresWasCalledOnGivenToken()
    {
        ITokenStorage sut = CreateSut();

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        Mock<IToken> tokenMock = _fixture!.CreateTokenMock(_random!);
        await sut.StoreTokenAsync(user, tokenMock.Object);

        tokenMock.Verify(m => m.Expires, Times.Once());
    }

    [Test]
    [Category("UnitTest")]
    public async Task StoreTokenAsync_WhenCalled_AssertCreateEntryWasCalledOnMemoryCacheWithTokenKeyResolvedByTokenKeyProvider()
    {
        string tokenKey = _fixture!.Create<string>();
        ITokenStorage sut = CreateSut(tokenKey: tokenKey);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        IToken token = _fixture!.CreateToken(_random!);
        await sut.StoreTokenAsync(user, token);

        _memoryCacheMock!.Verify(m => m.CreateEntry(It.Is<object>(value => value.Equals(tokenKey))), Times.Once());
    }

    [Test]
    [Category("UnitTest")]
    public async Task StoreTokenAsync_WhenCalled_AssertValueSetterWasCalledOnCacheEntryCreatedByMemoryCacheWithGivenToken()
    {
        Mock<ICacheEntry> cacheEntryMock = CreateCacheEntryMock(_fixture!, _random!);
        ITokenStorage sut = CreateSut(cacheEntry: cacheEntryMock.Object);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        IToken token = _fixture!.CreateToken(_random!);
        await sut.StoreTokenAsync(user, token);

        cacheEntryMock.VerifySet(m => m.Value = It.Is<IToken>(value => value == token), Times.Once());
    }

    [Test]
    [Category("UnitTest")]
    public async Task StoreTokenAsync_WhenCalled_AssertAbsoluteExpirationSetterWasCalledOnCacheEntryCreatedByMemoryCacheWithExpiresFromGivenToken()
    {
        Mock<ICacheEntry> cacheEntryMock = CreateCacheEntryMock(_fixture!, _random!);
        ITokenStorage sut = CreateSut(cacheEntry: cacheEntryMock.Object);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        DateTimeOffset expires = DateTimeOffset.UtcNow.AddMinutes(_random!.Next(5, 60));
        IToken token = _fixture!.CreateToken(_random!, expires: expires);
        await sut.StoreTokenAsync(user, token);

        cacheEntryMock.VerifySet(m => m.AbsoluteExpiration = It.Is<DateTimeOffset>(value => value == expires), Times.Once());
    }

    private ITokenStorage CreateSut(ICacheEntry? cacheEntry = null, string? tokenKey = null)
    {
        _memoryCacheMock!.Setup(m => m.CreateEntry(It.IsAny<object>()))
            .Returns(cacheEntry ?? CreateCacheEntry(_fixture!, _random!));

        _tokenKeyProviderMock!.Setup(_fixture!, tokenKey: tokenKey);
        _tokenProviderMock!.Setup(_fixture!, _random!);

        return new WebApi.Security.TokenStorage(_memoryCacheMock!.Object, _tokenKeyProviderMock!.Object, _tokenProviderMock!.Object);
    }
}