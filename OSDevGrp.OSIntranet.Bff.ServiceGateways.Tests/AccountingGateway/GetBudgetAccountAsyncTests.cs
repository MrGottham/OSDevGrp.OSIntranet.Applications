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
public class GetBudgetAccountAsyncTests : ServiceGatewayTestBase
{
    #region Private variables

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
    public async Task GetBudgetAccountAsync_WhenCalled_AssertBudgetaccountsAsyncWasCalledOnWebApiClient()
    {
        IAccountingGateway sut = CreateSut();

        await sut.GetBudgetAccountAsync(_fixture.Create<int>(), _fixture.Create<string>(), DateTimeOffset.Now.AddDays(_random!.Next(0, 365) * -1));

        _webApiClientMock!.Verify(m => m.BudgetaccountsAsync(
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<DateTimeOffset?>(),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetBudgetAccountAsync_WhenCalled_AssertBudgetaccountsAsyncWasCalledOnWebApiClientWithAccountingNumberEqualToAccountingNumberFromArguments()
    {
        IAccountingGateway sut = CreateSut();

        int accountingNumber = _fixture.Create<int>();
        await sut.GetBudgetAccountAsync(accountingNumber, _fixture.Create<string>(), DateTimeOffset.Now.AddDays(_random!.Next(0, 365) * -1));

        _webApiClientMock!.Verify(m => m.BudgetaccountsAsync(
                It.Is<int>(value => value == accountingNumber),
                It.IsAny<string>(),
                It.IsAny<DateTimeOffset?>(),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetBudgetAccountAsync_WhenCalled_AssertBudgetaccountsAsyncWasCalledOnWebApiClientWithAccountNumberEqualToAccountNumberFromArguments()
    {
        IAccountingGateway sut = CreateSut();

        string accountNumber = _fixture.Create<string>();
        await sut.GetBudgetAccountAsync(_fixture.Create<int>(), accountNumber, DateTimeOffset.Now.AddDays(_random!.Next(0, 365) * -1));

        _webApiClientMock!.Verify(m => m.BudgetaccountsAsync(
                It.IsAny<int>(),
                It.Is<string>(value => value == accountNumber),
                It.IsAny<DateTimeOffset?>(),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetBudgetAccountAsync_WhenCalled_AssertBudgetaccountsAsyncWasCalledOnWebApiClientWithStatusDateEqualToStatusDateFromArguments()
    {
        IAccountingGateway sut = CreateSut();

        DateTimeOffset statusDate = DateTimeOffset.Now.AddDays(_random!.Next(0, 365) * -1);
        await sut.GetBudgetAccountAsync(_fixture.Create<int>(), _fixture.Create<string>(), statusDate);

        _webApiClientMock!.Verify(m => m.BudgetaccountsAsync(
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.Is<DateTimeOffset?>(value => value != null && value == statusDate),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetBudgetAccountAsync_WhenCalled_AssertBudgetaccountsAsyncWasCalledOnWebApiClientWithCancellationTokenEqualToCancellationTokenFromArguments()
    {
        IAccountingGateway sut = CreateSut();

        CancellationToken cancellationToken = CancellationToken.None;
        await sut.GetBudgetAccountAsync(_fixture.Create<int>(), _fixture.Create<string>(), DateTimeOffset.Now.AddDays(_random!.Next(0, 365) * -1), cancellationToken);

        _webApiClientMock!.Verify(m => m.BudgetaccountsAsync(
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<DateTimeOffset?>(),
                It.Is<CancellationToken>(value => value == cancellationToken)), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetBudgetAccountAsync_WhenCalled_ReturnBudgetAccountModelFromWebApiClient()
    {
        BudgetAccountModel budgetAccountModel = CreateBudgetAccountModel();
        IAccountingGateway sut = CreateSut(budgetAccountModel: budgetAccountModel);

        BudgetAccountModel result = await sut.GetBudgetAccountAsync(_fixture.Create<int>(), _fixture.Create<string>(), DateTimeOffset.Now.AddDays(_random!.Next(0, 365) * -1));

        Assert.That(result, Is.EqualTo(budgetAccountModel));
    }

    [Test]
    [Category("UnitTest")]
    public void GetBudgetAccountAsync_WhenWebApiClientThrowsNonGenericWebApiClientException_ThrowsServiceGatewayExceptionBase()
    {
        WebApiClientException webApiClientException = _fixture!.CreateWebApiClientException((int) HttpStatusCode.BadRequest);
        IAccountingGateway sut = CreateSut(exception: webApiClientException);

        ServiceGatewayExceptionBase? result = Assert.ThrowsAsync<ServiceGatewayBadRequestException>(async () => await sut.GetBudgetAccountAsync(_fixture.Create<int>(), _fixture.Create<string>(), DateTimeOffset.Now.AddDays(_random!.Next(0, 365) * -1)));

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void GetBudgetAccountAsync_WhenWebApiClientThrowsGenericWebApiClientExceptionWithErrorModel_ThrowsServiceGatewayExceptionBase()
    {
        WebApiClientException<ErrorModel> webApiClientException = _fixture!.CreateWebApiClientException((int) HttpStatusCode.BadRequest, result: _fixture!.CreateErrorModel());
        IAccountingGateway sut = CreateSut(exception: webApiClientException);

        ServiceGatewayExceptionBase? result = Assert.ThrowsAsync<ServiceGatewayBadRequestException>(async () => await sut.GetBudgetAccountAsync(_fixture.Create<int>(), _fixture.Create<string>(), DateTimeOffset.Now.AddDays(_random!.Next(0, 365) * -1)));

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("IntegrationTest")]
    public async Task GetBudgetAccountAsync_WhenCalled_ExpectNoErrors()
    {
        await using ServiceGatewayCreator serviceGatewayCreator = new ServiceGatewayCreator(CreateTestConfiguration());

        AccountingTestOptions accountingTestOptions = serviceGatewayCreator.GetAccountingTestOptions();
        TimeProvider timeProvider = serviceGatewayCreator.GetTimeProvider();

        IAccountingGateway sut = serviceGatewayCreator.CreateAccountingGateway();
        try
        {
            await sut.GetBudgetAccountAsync(accountingTestOptions.ExistingAccountingNumber, accountingTestOptions.ExistingAccountNumberForBudgetAccount, timeProvider.GetLocalNow());
        }
        catch (Exception ex)
        {
            Assert.Fail(ex.Message);
        }
    }

    #region Private methods

    private IAccountingGateway CreateSut(BudgetAccountModel? budgetAccountModel = null, Exception? exception = null)
    {
        if (exception != null)
        {
            _webApiClientMock!.Setup(m => m.BudgetaccountsAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTimeOffset?>(), It.IsAny<CancellationToken>()))
                .Throws(exception);
        }
        else
        {
            _webApiClientMock!.Setup(m => m.BudgetaccountsAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTimeOffset?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(budgetAccountModel ?? CreateBudgetAccountModel()));
        }

        return new ServiceGateways.AccountingGateway(_webApiClientMock!.Object);
    }

    private BudgetAccountModel CreateBudgetAccountModel()
    {
        return _fixture!.Create<BudgetAccountModel>();
    }

    #endregion
}