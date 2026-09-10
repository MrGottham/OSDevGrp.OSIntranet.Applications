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
public class GetContactAccountAsyncTests : ServiceGatewayTestBase
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
    public async Task GetContactAccountAsync_WhenCalled_AssertContactaccountsAsyncWasCalledOnWebApiClient()
    {
        IAccountingGateway sut = CreateSut();

        await sut.GetContactAccountAsync(_fixture.Create<int>(), _fixture.Create<string>(), DateTimeOffset.Now.AddDays(_random!.Next(0, 365) * -1));

        _webApiClientMock!.Verify(m => m.ContactaccountsAsync(
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<DateTimeOffset?>(),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetContactAccountAsync_WhenCalled_AssertContactaccountsAsyncWasCalledOnWebApiClientWithAccountingNumberEqualToAccountingNumberFromArguments()
    {
        IAccountingGateway sut = CreateSut();

        int accountingNumber = _fixture.Create<int>();
        await sut.GetContactAccountAsync(accountingNumber, _fixture.Create<string>(), DateTimeOffset.Now.AddDays(_random!.Next(0, 365) * -1));

        _webApiClientMock!.Verify(m => m.ContactaccountsAsync(
                It.Is<int>(value => value == accountingNumber),
                It.IsAny<string>(),
                It.IsAny<DateTimeOffset?>(),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetContactAccountAsync_WhenCalled_AssertContactaccountsAsyncWasCalledOnWebApiClientWithAccountNumberEqualToAccountNumberFromArguments()
    {
        IAccountingGateway sut = CreateSut();

        string accountNumber = _fixture.Create<string>();
        await sut.GetContactAccountAsync(_fixture.Create<int>(), accountNumber, DateTimeOffset.Now.AddDays(_random!.Next(0, 365) * -1));

        _webApiClientMock!.Verify(m => m.ContactaccountsAsync(
                It.IsAny<int>(),
                It.Is<string>(value => value == accountNumber),
                It.IsAny<DateTimeOffset?>(),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetContactAccountAsync_WhenCalled_AssertContactaccountsAsyncWasCalledOnWebApiClientWithStatusDateEqualToStatusDateFromArguments()
    {
        IAccountingGateway sut = CreateSut();

        DateTimeOffset statusDate = DateTimeOffset.Now.AddDays(_random!.Next(0, 365) * -1);
        await sut.GetContactAccountAsync(_fixture.Create<int>(), _fixture.Create<string>(), statusDate);

        _webApiClientMock!.Verify(m => m.ContactaccountsAsync(
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.Is<DateTimeOffset?>(value => value != null && value == statusDate),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetContactAccountAsync_WhenCalled_AssertContactaccountsAsyncWasCalledOnWebApiClientWithCancellationTokenEqualToCancellationTokenFromArguments()
    {
        IAccountingGateway sut = CreateSut();

        CancellationToken cancellationToken = CancellationToken.None;
        await sut.GetContactAccountAsync(_fixture.Create<int>(), _fixture.Create<string>(), DateTimeOffset.Now.AddDays(_random!.Next(0, 365) * -1), cancellationToken);

        _webApiClientMock!.Verify(m => m.ContactaccountsAsync(
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<DateTimeOffset?>(),
                It.Is<CancellationToken>(value => value == cancellationToken)), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetContactAccountAsync_WhenCalled_ReturnContactAccountModelFromWebApiClient()
    {
        ContactAccountModel contactAccountModel = CreateContactAccountModel();
        IAccountingGateway sut = CreateSut(contactAccountModel: contactAccountModel);

        ContactAccountModel result = await sut.GetContactAccountAsync(_fixture.Create<int>(), _fixture.Create<string>(), DateTimeOffset.Now.AddDays(_random!.Next(0, 365) * -1));

        Assert.That(result, Is.EqualTo(contactAccountModel));
    }

    [Test]
    [Category("UnitTest")]
    public void GetContactAccountAsync_WhenWebApiClientThrowsNonGenericWebApiClientException_ThrowsServiceGatewayExceptionBase()
    {
        WebApiClientException webApiClientException = _fixture!.CreateWebApiClientException((int) HttpStatusCode.BadRequest);
        IAccountingGateway sut = CreateSut(exception: webApiClientException);

        ServiceGatewayExceptionBase? result = Assert.ThrowsAsync<ServiceGatewayBadRequestException>(async () => await sut.GetContactAccountAsync(_fixture.Create<int>(), _fixture.Create<string>(), DateTimeOffset.Now.AddDays(_random!.Next(0, 365) * -1)));

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void GetContactAccountAsync_WhenWebApiClientThrowsGenericWebApiClientExceptionWithErrorModel_ThrowsServiceGatewayExceptionBase()
    {
        WebApiClientException<ErrorModel> webApiClientException = _fixture!.CreateWebApiClientException((int) HttpStatusCode.BadRequest, result: _fixture!.CreateErrorModel());
        IAccountingGateway sut = CreateSut(exception: webApiClientException);

        ServiceGatewayExceptionBase? result = Assert.ThrowsAsync<ServiceGatewayBadRequestException>(async () => await sut.GetContactAccountAsync(_fixture.Create<int>(), _fixture.Create<string>(), DateTimeOffset.Now.AddDays(_random!.Next(0, 365) * -1)));

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("IntegrationTest")]
    public async Task GetContactAccountAsync_WhenCalled_ExpectNoErrors()
    {
        await using ServiceGatewayCreator serviceGatewayCreator = new ServiceGatewayCreator(CreateTestConfiguration());

        AccountingTestOptions accountingTestOptions = serviceGatewayCreator.GetAccountingTestOptions();
        TimeProvider timeProvider = serviceGatewayCreator.GetTimeProvider();

        IAccountingGateway sut = serviceGatewayCreator.CreateAccountingGateway();
        try
        {
            await sut.GetContactAccountAsync(accountingTestOptions.ExistingAccountingNumber, accountingTestOptions.ExistingAccountNumberForContactAccount, timeProvider.GetLocalNow());
        }
        catch (Exception ex)
        {
            Assert.Fail(ex.Message);
        }
    }

    #region Private methods

    private IAccountingGateway CreateSut(ContactAccountModel? contactAccountModel = null, Exception? exception = null)
    {
        if (exception != null)
        {
            _webApiClientMock!.Setup(m => m.ContactaccountsAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTimeOffset?>(), It.IsAny<CancellationToken>()))
                .Throws(exception);
        }
        else
        {
            _webApiClientMock!.Setup(m => m.ContactaccountsAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTimeOffset?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(contactAccountModel ?? CreateContactAccountModel()));
        }

        return new ServiceGateways.AccountingGateway(_webApiClientMock!.Object);
    }

    private ContactAccountModel CreateContactAccountModel()
    {
        return _fixture!.Create<ContactAccountModel>();
    }

    #endregion
}