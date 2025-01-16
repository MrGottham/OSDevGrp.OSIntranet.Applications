using AutoFixture;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Options;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests.SecurityGateway;

[TestFixture]
public class AquireTokenAsyncTests : ServiceGatewayTestBase
{
    #region Prviate variables

    private Mock<IWebApiClient> _webApiClientMock;
    private Mock<IOptions<WebApiOptions>> _webApiOptionsMock;
    private Fixture _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _webApiClientMock = new Mock<IWebApiClient>();
        _webApiOptionsMock = new Mock<IOptions<WebApiOptions>>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    public async Task AquireTokenAsync_WhenCalled_AssertValueWasCalledOnWebApiOptions()
    {
        ISecurityGateway sut = CreateSut();

        await sut.AquireTokenAsync();

        _webApiOptionsMock.Verify(m => m.Value, Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AquireTokenAsync_WhenCalled_AssertTokenAsyncWasCalledOnWebApiClient()
    {
        ISecurityGateway sut = CreateSut();

        await sut.AquireTokenAsync();

        _webApiClientMock.Verify(m => m.TokenAsync(
                It.IsAny<string?>(), 
                It.IsAny<string?>(), 
                It.IsAny<string?>(), 
                It.IsAny<string?>(), 
                It.IsAny<string?>(), 
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AquireTokenAsync_WhenCalled_AssertTokenAsyncWasCalledOnWebApiClientWithGrantTypeEqualToClientCredentials()
    {
        ISecurityGateway sut = CreateSut();

        await sut.AquireTokenAsync();

        _webApiClientMock.Verify(m => m.TokenAsync(
                It.Is<string?>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "client_credentials") == 0), 
                It.IsAny<string?>(), 
                It.IsAny<string?>(), 
                It.IsAny<string?>(), 
                It.IsAny<string?>(), 
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AquireTokenAsync_WhenCalled_AssertTokenAsyncWasCalledOnWebApiClientWithCodeEqualToEmptyString()
    {
        ISecurityGateway sut = CreateSut();

        await sut.AquireTokenAsync();

        _webApiClientMock.Verify(m => m.TokenAsync(
                It.IsAny<string?>(), 
                It.Is<string?>(value => value != null && string.Compare(value, string.Empty) == 0), 
                It.IsAny<string?>(), 
                It.IsAny<string?>(), 
                It.IsAny<string?>(), 
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AquireTokenAsync_WhenCalled_AssertTokenAsyncWasCalledOnWebApiClientWithClientIdEqualToEmptyString()
    {
        ISecurityGateway sut = CreateSut();

        await sut.AquireTokenAsync();

        _webApiClientMock.Verify(m => m.TokenAsync(
                It.IsAny<string?>(),
                It.IsAny<string?>(), 
                It.Is<string?>(value => value != null && string.Compare(value, string.Empty) == 0), 
                It.IsAny<string?>(), 
                It.IsAny<string?>(), 
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AquireTokenAsync_WhenCalled_AssertTokenAsyncWasCalledOnWebApiClientWithClientSecretEqualToEmptyString()
    {
        ISecurityGateway sut = CreateSut();

        await sut.AquireTokenAsync();

        _webApiClientMock.Verify(m => m.TokenAsync(
                It.IsAny<string?>(),
                It.IsAny<string?>(),  
                It.IsAny<string?>(), 
                It.Is<string?>(value => value != null && string.Compare(value, string.Empty) == 0),
                It.IsAny<string?>(), 
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AquireTokenAsync_WhenCalled_AssertTokenAsyncWasCalledOnWebApiClientWithRedirectUriEqualToEmptyString()
    {
        ISecurityGateway sut = CreateSut();

        await sut.AquireTokenAsync();

        _webApiClientMock.Verify(m => m.TokenAsync(
                It.IsAny<string?>(), 
                It.IsAny<string?>(), 
                It.IsAny<string?>(), 
                It.IsAny<string?>(), 
                It.Is<string?>(value => value != null && string.Compare(value, string.Empty) == 0), 
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AquireTokenAsync_WhenCalled_AssertTokenAsyncWasCalledOnWebApiClientWithCancellationTokenEqualToCancellationTokenFromArguments()
    {
        ISecurityGateway sut = CreateSut();

        CancellationToken cancellationToken = CancellationToken.None;
        await sut.AquireTokenAsync(cancellationToken);

        _webApiClientMock.Verify(m => m.TokenAsync(
                It.IsAny<string?>(), 
                It.IsAny<string?>(), 
                It.IsAny<string?>(), 
                It.IsAny<string?>(), 
                It.IsAny<string?>(), 
                It.Is<CancellationToken>(value => value == cancellationToken)), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AquireTokenAsync_WhenCalled_ReturnsAccessTokenModelFromWebApiClient()
    {
        AccessTokenModel accessTokenModel = CreateAccessTokenModel();
        ISecurityGateway sut = CreateSut(accessTokenModel: accessTokenModel);

        AccessTokenModel result = await sut.AquireTokenAsync();

        Assert.That(result, Is.EqualTo(accessTokenModel));
    }

    [Test]
    [Category("IntegrationTest")]
    public async Task AquireTokenAsync_WhenCalled_ExpectNoErrors()
    {
        await using ServiceGatewayCreator serviceGatewayCreator = new ServiceGatewayCreator(CreateTestConfiguration());

        ISecurityGateway sut = serviceGatewayCreator.CreateSecurityGateway();
        await sut.AquireTokenAsync();
    }

    private ISecurityGateway CreateSut(AccessTokenModel? accessTokenModel = null, WebApiOptions? webApiOptions = null)
    {
        _webApiClientMock.Setup(m => m.TokenAsync(It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(accessTokenModel ?? CreateAccessTokenModel()));

        _webApiOptionsMock.Setup(m => m.Value)
            .Returns(webApiOptions ?? CreateWebApiOptions());

        return new ServiceGateways.SecurityGateway(_webApiClientMock.Object, _webApiOptionsMock.Object);
    }

    private WebApiOptions CreateWebApiOptions(string? clientId = null, string? clientSecret = null)
    {
        return new WebApiOptions
        {
            ClientId =  clientId ?? _fixture.Create<string>(),
            ClientSecret = clientSecret ?? _fixture.Create<string>()
        };
    }

    private AccessTokenModel CreateAccessTokenModel()
    {
        return _fixture.Create<AccessTokenModel>();
    }
}