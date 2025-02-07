using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests.Extensions;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests.Options;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using System.Net;

namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests.AccountingGateway;

[TestFixture]
public class GetAccountingAsyncTests : ServiceGatewayTestBase
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
    public async Task GetAccountingAsync_WhenCalled_AssertAccountingAsyncWasCalledOnWebApiClient()
    {
        IAccountingGateway sut = CreateSut();

        await sut.GetAccountingAsync(_fixture.Create<int>(), DateTimeOffset.Now.AddDays(_random!.Next(0, 365) * -1));

        _webApiClientMock!.Verify(m => m.AccountingAsync(
                It.IsAny<int>(),
                It.IsAny<DateTimeOffset?>(),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetAccountingAsync_WhenCalled_AssertAccountingAsyncWasCalledOnWebApiClientWithAccountingNumberEqualToAccountingNumberFromArguments()
    {
        IAccountingGateway sut = CreateSut();

        int accountingNumber = _fixture.Create<int>();
        await sut.GetAccountingAsync(accountingNumber, DateTimeOffset.Now.AddDays(_random!.Next(0, 365) * -1));

        _webApiClientMock!.Verify(m => m.AccountingAsync(
                It.Is<int>(value => value == accountingNumber),
                It.IsAny<DateTimeOffset?>(),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetAccountingAsync_WhenCalled_AssertAccountingAsyncWasCalledOnWebApiClientWithStatusDateEqualToStatusDateFromArguments()
    {
        IAccountingGateway sut = CreateSut();

        DateTimeOffset statusDate = DateTimeOffset.Now.AddDays(_random!.Next(0, 365) * -1);
        await sut.GetAccountingAsync(_fixture.Create<int>(), statusDate);

        _webApiClientMock!.Verify(m => m.AccountingAsync(
                It.IsAny<int>(),
                It.Is<DateTimeOffset?>(value => value != null && value == statusDate),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetAccountingAsync_WhenCalled_AssertAccountingAsyncWasCalledOnWebApiClientWithCancellationTokenEqualToCancellationTokenFromArguments()
    {
        IAccountingGateway sut = CreateSut();

        CancellationToken cancellationToken = CancellationToken.None;
        await sut.GetAccountingAsync(_fixture.Create<int>(), DateTimeOffset.Now.AddDays(_random!.Next(0, 365) * -1), cancellationToken);

        _webApiClientMock!.Verify(m => m.AccountingAsync(
                It.IsAny<int>(),
                It.IsAny<DateTimeOffset?>(),
                It.Is<CancellationToken>(value => value == cancellationToken)), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetAccountingAsync_WhenCalled_ReturnAccountingModelsFromWebApiClient()
    {
        AccountingModel accountingModel = CreateAccountingModel();
        IAccountingGateway sut = CreateSut(accountingModel: accountingModel);

        AccountingModel result = await sut.GetAccountingAsync(_fixture.Create<int>(), DateTimeOffset.Now.AddDays(_random!.Next(0, 365) * -1));

        Assert.That(result, Is.EqualTo(accountingModel));
    }

    [Test]
    [Category("UnitTest")]
    public void GetAccountingAsync_WhenWebApiClientThrowsNonGenericWebApiClientException_ThrowsServiceGatewayExceptionBase()
    {
        WebApiClientException webApiClientException = _fixture!.CreateWebApiClientException((int) HttpStatusCode.BadRequest);
        IAccountingGateway sut = CreateSut(exception: webApiClientException);

        ServiceGatewayExceptionBase? result = Assert.ThrowsAsync<ServiceGatewayBadRequestException>(async () => await sut.GetAccountingAsync(_fixture.Create<int>(), DateTimeOffset.Now.AddDays(_random!.Next(0, 365) * -1)));

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void GetAccountingAsync_WhenWebApiClientThrowsGenericWebApiClientExceptionWithErrorModel_ThrowsServiceGatewayExceptionBase()
    {
        WebApiClientException<ErrorModel> webApiClientException = _fixture!.CreateWebApiClientException((int) HttpStatusCode.BadRequest, result: _fixture!.CreateErrorModel());
        IAccountingGateway sut = CreateSut(exception: webApiClientException);

        ServiceGatewayExceptionBase? result = Assert.ThrowsAsync<ServiceGatewayBadRequestException>(async () => await sut.GetAccountingAsync(_fixture.Create<int>(), DateTimeOffset.Now.AddDays(_random!.Next(0, 365) * -1)));

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("IntegrationTest")]
    public async Task GetAccountingAsync_WhenCalled_ExpectNoErrors()
    {
        await using ServiceGatewayCreator serviceGatewayCreator = new ServiceGatewayCreator(CreateTestConfiguration());

        AccountingTestOptions accountingTestOptions = serviceGatewayCreator.GetAccountingTestOptions();
        TimeProvider timeProvider = serviceGatewayCreator.GetTimeProvider();

        IAccountingGateway sut = serviceGatewayCreator.CreateAccountingGateway();
        try
        {
            await sut.GetAccountingAsync(accountingTestOptions.ExistingAccountingNumber, timeProvider.GetLocalNow());
        }
        catch (Exception ex)
        {
            Assert.Fail(ex.Message);
        }
    }

    private IAccountingGateway CreateSut(AccountingModel? accountingModel = null, Exception? exception = null)
    {
        if (exception != null)
        {
            _webApiClientMock!.Setup(m => m.AccountingAsync(It.IsAny<int>(), It.IsAny<DateTimeOffset?>(), It.IsAny<CancellationToken>()))
                .Throws(exception);
        }
        else
        {
            _webApiClientMock!.Setup(m => m.AccountingAsync(It.IsAny<int>(), It.IsAny<DateTimeOffset?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(accountingModel ?? CreateAccountingModel()));
        }

        return new ServiceGateways.AccountingGateway(_webApiClientMock!.Object);
    }

    private AccountingModel CreateAccountingModel()
    {
        return _fixture!.Create<AccountingModel>();
    }
}