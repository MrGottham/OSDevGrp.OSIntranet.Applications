using System.Security.Claims;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.WebApi.Security;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security.TokenProvider;

[TestFixture]
public class ResolveAsyncTests
{
    #region Private variables

    private Mock<IServiceProvider>? _serviceProviderMock;
    private Mock<ISecurityGateway>? _securityGatewayMock;
    private Mock<TimeProvider>? _timeProviderMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _serviceProviderMock = new Mock<IServiceProvider>();
        _securityGatewayMock = new Mock<ISecurityGateway>();
        _timeProviderMock = new Mock<TimeProvider>();
        _fixture = new Fixture();
        _random = new Random(_fixture.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task ResolveAsync_WhenCalled_AssertGetServiceWasCalledOnServiceProviderWithInterfaceTypeForSecurityGateway(bool isAuthenticated)
    {
        ITokenProvider sut = CreateSut();

        ClaimsPrincipal claimsPrincipal = isAuthenticated 
            ? _fixture!.CreateAuthenticatedClaimsPrincipal()
            : _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        await sut.ResolveAsync(claimsPrincipal);

        _serviceProviderMock!.Verify(m => m.GetService(It.Is<Type>(value => value == typeof(ISecurityGateway))), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task ResolveAsync_WhenCalled_AssertGetServiceWasCalledOnServiceProviderWithTypeForTimeProvider(bool isAuthenticated)
    {
        ITokenProvider sut = CreateSut();

        ClaimsPrincipal claimsPrincipal = isAuthenticated 
            ? _fixture!.CreateAuthenticatedClaimsPrincipal()
            : _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        await sut.ResolveAsync(claimsPrincipal);

        _serviceProviderMock!.Verify(m => m.GetService(It.Is<Type>(value => value == typeof(TimeProvider))), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task ResolveAsync_WhenCalled_AssertAquireTokenAsyncWasCalledOnSecurityGatewayWithGivenCancellationToken(bool isAuthenticated)
    {
        ITokenProvider sut = CreateSut();

        ClaimsPrincipal claimsPrincipal = isAuthenticated 
            ? _fixture!.CreateAuthenticatedClaimsPrincipal()
            : _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.ResolveAsync(claimsPrincipal, cancellationToken);

        _securityGatewayMock!.Verify(m => m.AquireTokenAsync(It.Is<CancellationToken>(value => value == cancellationToken)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task ResolveAsync_WhenCalled_AsserGetUtcNowWasCalledOnTimeProvider(bool isAuthenticated)
    {
        ITokenProvider sut = CreateSut();

        ClaimsPrincipal claimsPrincipal = isAuthenticated 
            ? _fixture!.CreateAuthenticatedClaimsPrincipal()
            : _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        await sut.ResolveAsync(claimsPrincipal);

        _timeProviderMock!.Verify(m => m.GetUtcNow(), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task ResolveAsync_WhenCalled_ReturnsLocalToken(bool isAuthenticated)
    {
        ITokenProvider sut = CreateSut();

        ClaimsPrincipal claimsPrincipal = isAuthenticated 
            ? _fixture!.CreateAuthenticatedClaimsPrincipal()
            : _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        IToken result = await sut.ResolveAsync(claimsPrincipal);

        Assert.That(result, Is.TypeOf<WebApi.Security.LocalToken>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task ResolveAsync_WhenCalled_ReturnsLocalTokenWhereTokenTypeIsEqualToTokenTypeFromTokenAquiredFromSecurityGateway(bool isAuthenticated)
    {
        string tokenType = _fixture!.Create<string>();
        AccessTokenModel accessTokenModel = CreateAccessTokenModel(tokenType: tokenType);
        ITokenProvider sut = CreateSut(accessTokenModel: accessTokenModel);

        ClaimsPrincipal claimsPrincipal = isAuthenticated 
            ? _fixture!.CreateAuthenticatedClaimsPrincipal()
            : _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        IToken result = await sut.ResolveAsync(claimsPrincipal);

        Assert.That(result.TokenType, Is.EqualTo(tokenType));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task ResolveAsync_WhenCalled_ReturnsLocalTokenWhereTokenIsEqualToAccessTokenFromTokenAquiredFromSecurityGateway(bool isAuthenticated)
    {
        string accessToken = _fixture!.Create<string>();
        AccessTokenModel accessTokenModel = CreateAccessTokenModel(accessToken: accessToken);
        ITokenProvider sut = CreateSut(accessTokenModel: accessTokenModel);

        ClaimsPrincipal claimsPrincipal = isAuthenticated 
            ? _fixture!.CreateAuthenticatedClaimsPrincipal()
            : _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        IToken result = await sut.ResolveAsync(claimsPrincipal);

        Assert.That(result.Token, Is.EqualTo(accessToken));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task ResolveAsync_WhenCalled_ReturnsLocalTokenWhereExpiresIsCalculatedBasedOnExpiresInFromTokenAquiredFromSecurityGateway(bool isAuthenticated)
    {
        int expiresIn = _random!.Next(300, 3600);
        AccessTokenModel accessTokenModel = CreateAccessTokenModel(expiresIn: expiresIn);
        DateTimeOffset utcNow = DateTimeOffset.UtcNow;
        ITokenProvider sut = CreateSut(accessTokenModel: accessTokenModel, utcNow: utcNow);

        ClaimsPrincipal claimsPrincipal = isAuthenticated 
            ? _fixture!.CreateAuthenticatedClaimsPrincipal()
            : _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        IToken result = await sut.ResolveAsync(claimsPrincipal);

        Assert.That(result.Expires, Is.EqualTo(utcNow.AddSeconds(expiresIn)));
    }

    private ITokenProvider CreateSut(AccessTokenModel? accessTokenModel = null, DateTimeOffset? utcNow = null)
    {
        _serviceProviderMock!.Setup(m => m.GetService(It.Is<Type>(value => value == typeof(ISecurityGateway))))
            .Returns(_securityGatewayMock!.Object);
        _serviceProviderMock!.Setup(m => m.GetService(It.Is<Type>(value => value == typeof(TimeProvider))))
            .Returns(_timeProviderMock!.Object);

        _timeProviderMock!.Setup(m => m.GetUtcNow())
            .Returns(utcNow ?? DateTimeOffset.UtcNow);

        _securityGatewayMock!.Setup(m => m.AquireTokenAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(accessTokenModel ?? CreateAccessTokenModel()));

        return new WebApi.Security.TokenProvider(_serviceProviderMock!.Object);
    }

    private AccessTokenModel CreateAccessTokenModel(string? tokenType = null, string? accessToken = null, int? expiresIn = null)
    {
        return new AccessTokenModel(accessToken ?? _fixture.Create<string>(), expiresIn ?? _random!.Next(300, 3600), null, tokenType ?? _fixture.Create<string>());
    }
}