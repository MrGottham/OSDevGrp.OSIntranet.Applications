using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests.Extensions;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using System.Net;

namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests.SecurityGateway;

[TestFixture]
public class AquireTokenAsyncTests : ServiceGatewayTestBase
{
    #region Prviate variables

    private Mock<IWebApiClient>? _webApiClientMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _webApiClientMock = new Mock<IWebApiClient>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    public async Task AquireTokenAsync_WhenCalled_AssertTokenAsyncWasCalledOnWebApiClient()
    {
        ISecurityGateway sut = CreateSut();

        await sut.AquireTokenAsync();

        _webApiClientMock!.Verify(m => m.TokenAsync(
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

        _webApiClientMock!.Verify(m => m.TokenAsync(
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

        _webApiClientMock!.Verify(m => m.TokenAsync(
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

        _webApiClientMock!.Verify(m => m.TokenAsync(
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

        _webApiClientMock!.Verify(m => m.TokenAsync(
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

        _webApiClientMock!.Verify(m => m.TokenAsync(
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

        _webApiClientMock!.Verify(m => m.TokenAsync(
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
    [Category("UnitTest")]
    public void AquireTokenAsync_WhenWebApiClientThrowsNonGenericWebApiClientException_ThrowsServiceGatewayExceptionBase()
    {
        WebApiClientException webApiClientException = _fixture!.CreateWebApiClientException((int) HttpStatusCode.BadRequest);
        ISecurityGateway sut = CreateSut(exception: webApiClientException);

        ServiceGatewayExceptionBase? result = Assert.ThrowsAsync<ServiceGatewayBadRequestException>(async () => await sut.AquireTokenAsync());

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void AquireTokenAsync_WhenWebApiClientThrowsGenericWebApiClientExceptionWithErrorResponseModel_ThrowsServiceGatewayExceptionBase()
    {
        WebApiClientException<ErrorResponseModel> webApiClientException = _fixture!.CreateWebApiClientException((int) HttpStatusCode.BadRequest, result: _fixture!.CreateErrorResponseModel());
        ISecurityGateway sut = CreateSut(exception: webApiClientException);

        ServiceGatewayExceptionBase? result = Assert.ThrowsAsync<ServiceGatewayBadRequestException>(async () => await sut.AquireTokenAsync());

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("IntegrationTest")]
    public async Task AquireTokenAsync_WhenCalled_ExpectNoErrors()
    {
        await using ServiceGatewayCreator serviceGatewayCreator = new ServiceGatewayCreator(CreateTestConfiguration());

        ISecurityGateway sut = serviceGatewayCreator.CreateSecurityGateway();
        try
        {
            await sut.AquireTokenAsync();
        }
        catch (Exception ex)
        {
            Assert.Fail(ex.Message);
        }
    }

    private ISecurityGateway CreateSut(AccessTokenModel? accessTokenModel = null, Exception? exception = null)
    {
        if (exception != null)
        {
            _webApiClientMock!.Setup(m => m.TokenAsync(It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
                .Throws(exception);
        }
        else
        {
            _webApiClientMock!.Setup(m => m.TokenAsync(It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(accessTokenModel ?? CreateAccessTokenModel()));
        }

        return new ServiceGateways.SecurityGateway(_webApiClientMock.Object);
    }

    private AccessTokenModel CreateAccessTokenModel()
    {
        return _fixture.Create<AccessTokenModel>();
    }
}