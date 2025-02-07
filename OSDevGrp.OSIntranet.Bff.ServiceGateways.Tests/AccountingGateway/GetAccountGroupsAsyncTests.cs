using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests.Extensions;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using System.Net;

namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests.AccountingGateway;

[TestFixture]
public class GetAccountGroupsAsyncTests : ServiceGatewayTestBase
{
    #region Prviate variables

    private Mock<IWebApiClient>? _webApiClientMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _webApiClientMock = new Mock<IWebApiClient>();
        _fixture = new Fixture();
        _random = new Random(_fixture.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetAccountGroupsAsync_WhenCalled_AssertAccountgroupsAsyncWasCalledOnWebApiClient()
    {
        IAccountingGateway sut = CreateSut();

        await sut.GetAccountGroupsAsync();

        _webApiClientMock!.Verify(m => m.AccountgroupsAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetAccountGroupsAsync_WhenCalled_AssertAccountgroupsAsyncWasCalledOnWebApiClientWithCancellationTokenEqualToCancellationTokenFromArguments()
    {
        IAccountingGateway sut = CreateSut();

        CancellationToken cancellationToken = CancellationToken.None;
        await sut.GetAccountGroupsAsync(cancellationToken);

        _webApiClientMock!.Verify(m => m.AccountgroupsAsync(It.Is<CancellationToken>(value => value == cancellationToken)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetAccountGroupsAsync_WhenCalled_ReturnAccountGroupModelsFromWebApiClient()
    {
        ICollection<AccountGroupModel> accountGroupModels = CreateAccountGroupModels();
        IAccountingGateway sut = CreateSut(accountGroupModels: accountGroupModels);

        IEnumerable<AccountGroupModel> result = await sut.GetAccountGroupsAsync();

        Assert.That(result, Is.EqualTo(accountGroupModels));
    }

    [Test]
    [Category("UnitTest")]
    public void GetAccountGroupsAsync_WhenWebApiClientThrowsNonGenericWebApiClientException_ThrowsServiceGatewayExceptionBase()
    {
        WebApiClientException webApiClientException = _fixture!.CreateWebApiClientException((int) HttpStatusCode.BadRequest);
        IAccountingGateway sut = CreateSut(exception: webApiClientException);

        ServiceGatewayExceptionBase? result = Assert.ThrowsAsync<ServiceGatewayBadRequestException>(async () => await sut.GetAccountGroupsAsync());

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void GetAccountGroupsAsync_WhenWebApiClientThrowsGenericWebApiClientExceptionWithErrorModel_ThrowsServiceGatewayExceptionBase()
    {
        WebApiClientException<ErrorModel> webApiClientException = _fixture!.CreateWebApiClientException((int) HttpStatusCode.BadRequest, result: _fixture!.CreateErrorModel());
        IAccountingGateway sut = CreateSut(exception: webApiClientException);

        ServiceGatewayExceptionBase? result = Assert.ThrowsAsync<ServiceGatewayBadRequestException>(async () => await sut.GetAccountGroupsAsync());

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("IntegrationTest")]
    public async Task GetAccountGroupsAsync_WhenCalled_ExpectNoErrors()
    {
        await using ServiceGatewayCreator serviceGatewayCreator = new ServiceGatewayCreator(CreateTestConfiguration());

        IAccountingGateway sut = serviceGatewayCreator.CreateAccountingGateway();
        try
        {
            await sut.GetAccountGroupsAsync();
        }
        catch (Exception ex)
        {
            Assert.Fail(ex.Message);
        }
    }

    private IAccountingGateway CreateSut(ICollection<AccountGroupModel>? accountGroupModels = null, Exception? exception = null)
    {
        if (exception != null)
        {
            _webApiClientMock!.Setup(m => m.AccountgroupsAsync(It.IsAny<CancellationToken>()))
                .Throws(exception);
        }
        else
        {
            _webApiClientMock!.Setup(m => m.AccountgroupsAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(accountGroupModels ?? CreateAccountGroupModels()));
        }

        return new ServiceGateways.AccountingGateway(_webApiClientMock!.Object);
    }

    private ICollection<AccountGroupModel> CreateAccountGroupModels()
    {
        return _fixture!.CreateMany<AccountGroupModel>(_random!.Next(5, 10)).ToArray();
    }
}