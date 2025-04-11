using AutoFixture;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.WebApi.Security;
using OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security.TokenKeyProvider;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security.TokenStorage;

[TestFixture]
public class DeleteTokenAsyncTests : TokenStorageTestBase
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
    public async Task DeleteTokenAsync_WhenCalled_AssertResolveAsyncWasCalledOnTokenKeyProviderWithGivenClaimsPrincipal(bool authenticatedUser)
    {
        ITokenStorage sut = CreateSut();

        ClaimsPrincipal user = authenticatedUser ? _fixture!.CreateAuthenticatedClaimsPrincipal() : _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        await sut.DeleteTokenAsync(user);

        _tokenKeyProviderMock!.Verify(m => m.ResolveAsync(
                It.Is<ClaimsPrincipal>(value => value == user),
                It.IsAny<CancellationToken>()),
            Times.Once());
    }

    [Test]
    [Category("UnitTest")]
    public async Task DeleteTokenAsync_WhenCalled_AssertResolveAsyncWasCalledOnTokenKeyProviderWithGivenCancellationToken()
    {
        ITokenStorage sut = CreateSut();

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.DeleteTokenAsync(user, cancellationToken);

        _tokenKeyProviderMock!.Verify(m => m.ResolveAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once());
    }

    [Test]
    [Category("UnitTest")]
    public async Task DeleteTokenAsync_WhenCalled_AssertRemoveWasCalledOnMemoryCacheWithTokenKeyResolvedByTokenKeyProvider()
    {
        string tokenKey = _fixture!.Create<string>();
        ITokenStorage sut = CreateSut(tokenKey: tokenKey);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        await sut.DeleteTokenAsync(user);

        _memoryCacheMock!.Verify(m => m.Remove(It.Is<object>(value => value.Equals(tokenKey))), Times.Once());
    }

    private ITokenStorage CreateSut(string? tokenKey = null)
    {
        _tokenKeyProviderMock!.Setup(_fixture!, tokenKey: tokenKey);
        _tokenProviderMock!.Setup(_fixture!, _random!);

        return new WebApi.Security.TokenStorage(_memoryCacheMock!.Object, _tokenKeyProviderMock!.Object, _tokenProviderMock!.Object);
    }
}