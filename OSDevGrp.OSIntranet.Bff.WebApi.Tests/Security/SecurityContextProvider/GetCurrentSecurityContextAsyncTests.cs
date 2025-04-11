using AutoFixture;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.WebApi.Security;
using OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security.TokenStorage;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security.SecurityContextProvider;

[TestFixture]
public class GetCurrentSecurityContextAsyncTests
{
    #region Private variables

    private Mock<IHttpContextAccessor>? _httpContextAccessorMock;
    private Mock<ITokenStorage>? _tokenStorageMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _tokenStorageMock = new Mock<ITokenStorage>();
        _fixture = new Fixture();
        _random = new Random(_fixture.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetCurrentSecurityContextAsync_WhenCalled_AssertHttpContextWasCalledOnHttpContextAccessor()
    {
        ISecurityContextProvider sut = CreateSut();

        await sut.GetCurrentSecurityContextAsync();

        _httpContextAccessorMock!.Verify(m => m.HttpContext, Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public void GetCurrentSecurityContextAsync_WhenNoHttpContextWasResolvedFromHttpContextAccessor_ThrowsInvalidOperationException()
    {
        ISecurityContextProvider sut = CreateSut(hasHttpContext: false);

        InvalidOperationException? result = Assert.ThrowsAsync<InvalidOperationException>(async () => await sut.GetCurrentSecurityContextAsync());

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void GetCurrentSecurityContextAsync_WhenNoHttpContextWasResolvedFromHttpContextAccessor_ThrowsInvalidOperationExceptionWithSpecificMessage()
    {
        ISecurityContextProvider sut = CreateSut(hasHttpContext: false);

        InvalidOperationException? result = Assert.ThrowsAsync<InvalidOperationException>(async () => await sut.GetCurrentSecurityContextAsync());

        Assert.That(result!.Message, Is.EqualTo("No HTTP context was resolved from the HTTP context accessor."));
    }

    [Test]
    [Category("UnitTest")]
    public void GetCurrentSecurityContextAsync_WhenNoHttpContextWasResolvedFromHttpContextAccessor_ThrowsInvalidOperationExceptionWhereInnerExceptionIsNull()
    {
        ISecurityContextProvider sut = CreateSut(hasHttpContext: false);

        InvalidOperationException? result = Assert.ThrowsAsync<InvalidOperationException>(async () => await sut.GetCurrentSecurityContextAsync());

        Assert.That(result!.InnerException, Is.Null);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task GetCurrentSecurityContextAsync_WhenCalled_AssertGetTokenAsyncWasCalledOnTokenStorageWithUserFromResolvedHttpContext(bool isAuthenticated)
    {
        ClaimsPrincipal user = isAuthenticated 
            ? _fixture!.CreateAuthenticatedClaimsPrincipal() 
            : _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        HttpContext httpContext = CreateHttpContext(user: user);
        ISecurityContextProvider sut = CreateSut(httpContext: httpContext);

        await sut.GetCurrentSecurityContextAsync();

        _tokenStorageMock!.Verify(m => m.GetTokenAsync(
                It.Is<ClaimsPrincipal>(value => value == user),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetCurrentSecurityContextAsync_WhenCalled_AssertGetTokenAsyncWasCalledOnTokenStorageWithGivenCancellationToken()
    {
        ISecurityContextProvider sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.GetCurrentSecurityContextAsync(cancellationToken);

        _tokenStorageMock!.Verify(m => m.GetTokenAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetCurrentSecurityContextAsync_WhenCalled_ReturnsLocalSecurityContext()
    {
        ISecurityContextProvider sut = CreateSut();

        ISecurityContext result = await sut.GetCurrentSecurityContextAsync();

        Assert.That(result, Is.TypeOf<LocalSecurityContext>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task GetCurrentSecurityContextAsync_WhenCalled_ReturnsLocalSecurityContextWhereUserIsEqualToUserFromResolvedHttpContext(bool isAuthenticated)
    {
        ClaimsPrincipal user = isAuthenticated 
            ? _fixture!.CreateAuthenticatedClaimsPrincipal() 
            : _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        HttpContext httpContext = CreateHttpContext(user: user);
        ISecurityContextProvider sut = CreateSut(httpContext: httpContext);

        ISecurityContext result = await sut.GetCurrentSecurityContextAsync();

        Assert.That(result.User, Is.EqualTo(user));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task GetCurrentSecurityContextAsync_WhenCalled_ReturnsLocalSecurityContextWhereAccessTokenIsEqualToTokenFromTokenStorage(bool isAuthenticated)
    {
        IToken token = _fixture!.CreateToken(_random!);
        ISecurityContextProvider sut = CreateSut(token: token);

        ISecurityContext result = await sut.GetCurrentSecurityContextAsync();

        Assert.That(result.AccessToken, Is.EqualTo(token));
    }

    private ISecurityContextProvider CreateSut(bool hasHttpContext = true, HttpContext? httpContext = null, IToken? token = null)
    {
        _httpContextAccessorMock!.Setup(m => m.HttpContext)
            .Returns(hasHttpContext ? httpContext ?? CreateHttpContext() : null);

        _tokenStorageMock!.Setup(_fixture!, _random!, token);

        return new WebApi.Security.SecurityContextProvider(_httpContextAccessorMock!.Object, _tokenStorageMock!.Object);
    }

    private HttpContext CreateHttpContext(ClaimsPrincipal? user = null)
    {
        return new DefaultHttpContext
        {
            User = user ?? _fixture!.CreateAuthenticatedClaimsPrincipal()
        };
    }
}