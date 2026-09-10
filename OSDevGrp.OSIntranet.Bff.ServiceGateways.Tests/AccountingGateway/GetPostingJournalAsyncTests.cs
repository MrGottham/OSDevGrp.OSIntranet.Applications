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
public class GetPostingJournalAsyncTests : ServiceGatewayTestBase
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
    public async Task GetPostingJournalAsync_WhenCalled_AssertPostingjournalGETAsyncWasCalledOnWebApiClient()
    {
        IAccountingGateway sut = CreateSut();

        await sut.GetPostingJournalAsync(_fixture!.Create<int>());

        _webApiClientMock!.Verify(m => m.PostingjournalGETAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetPostingJournalAsync_WhenCalled_AssertPostingjournalGETAsyncWasCalledOnWebApiClientWithAccountingNumberEqualToAccountingNumberFromArguments()
    {
        IAccountingGateway sut = CreateSut();

        int accountingNumber = _fixture!.Create<int>();
        await sut.GetPostingJournalAsync(accountingNumber);

        _webApiClientMock!.Verify(m => m.PostingjournalGETAsync(
                It.Is<int>(value => value == accountingNumber),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetPostingJournalAsync_WhenCalled_AssertPostingjournalGETAsyncWasCalledOnWebApiClientWithCancellationTokenEqualToCancellationTokenFromArguments()
    {
        IAccountingGateway sut = CreateSut();

        CancellationToken cancellationToken = CancellationToken.None;
        await sut.GetPostingJournalAsync(_fixture!.Create<int>(), cancellationToken);

        _webApiClientMock!.Verify(m => m.PostingjournalGETAsync(
                It.IsAny<int>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetPostingJournalAsync_WhenCalled_ReturnApplyPostingJournalModelFromWebApiClient()
    {
        ApplyPostingJournalModel postingJournalModel = CreateApplyPostingJournalModel();
        IAccountingGateway sut = CreateSut(postingJournalModel: postingJournalModel);

        ApplyPostingJournalModel result = await sut.GetPostingJournalAsync(_fixture!.Create<int>());

        Assert.That(result, Is.EqualTo(postingJournalModel));
    }

    [Test]
    [Category("UnitTest")]
    public void GetPostingJournalAsync_WhenWebApiClientThrowsNonGenericWebApiClientException_ThrowsServiceGatewayExceptionBase()
    {
        WebApiClientException webApiClientException = _fixture!.CreateWebApiClientException((int) HttpStatusCode.BadRequest);
        IAccountingGateway sut = CreateSut(exception: webApiClientException);

        ServiceGatewayExceptionBase? result = Assert.ThrowsAsync<ServiceGatewayBadRequestException>(async () => await sut.GetPostingJournalAsync(_fixture.Create<int>()));

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void GetPostingJournalAsync_WhenWebApiClientThrowsGenericWebApiClientExceptionWithErrorModel_ThrowsServiceGatewayExceptionBase()
    {
        WebApiClientException<ErrorModel> webApiClientException = _fixture!.CreateWebApiClientException((int) HttpStatusCode.BadRequest, result: _fixture!.CreateErrorModel());
        IAccountingGateway sut = CreateSut(exception: webApiClientException);

        ServiceGatewayExceptionBase? result = Assert.ThrowsAsync<ServiceGatewayBadRequestException>(async () => await sut.GetPostingJournalAsync(_fixture.Create<int>()));

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("IntegrationTest")]
    public async Task GetPostingJournalAsync_WhenCalled_ExpectNoErrors()
    {
        await using ServiceGatewayCreator serviceGatewayCreator = new ServiceGatewayCreator(CreateTestConfiguration());

        AccountingTestOptions accountingTestOptions = serviceGatewayCreator.GetAccountingTestOptions();

        IAccountingGateway sut = serviceGatewayCreator.CreateAccountingGateway();
        try
        {
            await sut.GetPostingJournalAsync(accountingTestOptions.ExistingAccountingNumber);
        }
        catch (Exception ex)
        {
            Assert.Fail(ex.Message);
        }
    }

    private IAccountingGateway CreateSut(ApplyPostingJournalModel? postingJournalModel = null, Exception? exception = null)
    {
        if (exception != null)
        {
            _webApiClientMock!.Setup(m => m.PostingjournalGETAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Throws(exception);
        }
        else
        {
            _webApiClientMock!.Setup(m => m.PostingjournalGETAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(postingJournalModel ?? CreateApplyPostingJournalModel()));
        }

        return new ServiceGateways.AccountingGateway(_webApiClientMock!.Object);
    }

    private ApplyPostingJournalModel CreateApplyPostingJournalModel()
    {
        return _fixture!.Create<ApplyPostingJournalModel>();
    }
}
