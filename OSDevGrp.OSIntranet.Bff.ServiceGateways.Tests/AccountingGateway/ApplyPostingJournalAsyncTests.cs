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
public class ApplyPostingJournalAsyncTests : ServiceGatewayTestBase
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
    public async Task ApplyPostingJournalAsync_WhenCalled_AssertPostinglinesAsyncWasCalledOnWebApiClient()
    {
        IAccountingGateway sut = CreateSut();

        await sut.ApplyPostingJournalAsync(_fixture!.Create<int>(), CreateApplyPostingJournalModel());

        _webApiClientMock!.Verify(m => m.PostinglinesAsync(
                It.IsAny<int>(),
                It.Is<string>(value => value == ApiVersions.Version1_0),
                It.IsAny<IEnumerable<ApplyPostingLineModel>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ApplyPostingJournalAsync_WhenCalled_AssertPostinglinesAsyncWasCalledOnWebApiClientWithAccountingNumberEqualToAccountingNumberFromArguments()
    {
        IAccountingGateway sut = CreateSut();

        int accountingNumber = _fixture!.Create<int>();
        await sut.ApplyPostingJournalAsync(accountingNumber, CreateApplyPostingJournalModel());

        _webApiClientMock!.Verify(m => m.PostinglinesAsync(
                It.Is<int>(value => value == accountingNumber),
                It.Is<string>(value => value == ApiVersions.Version1_0),
                It.IsAny<IEnumerable<ApplyPostingLineModel>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ApplyPostingJournalAsync_WhenCalled_AssertPostinglinesAsyncWasCalledOnWebApiClientWithApplyPostingLinesEqualToApplyPostingLinesFromPostingJournalArguments()
    {
        IAccountingGateway sut = CreateSut();

        ApplyPostingJournalModel postingJournal = CreateApplyPostingJournalModel();
        await sut.ApplyPostingJournalAsync(_fixture!.Create<int>(), postingJournal);

        _webApiClientMock!.Verify(m => m.PostinglinesAsync(
                It.IsAny<int>(),
                It.Is<string>(value => value == ApiVersions.Version1_0),
                It.Is<IEnumerable<ApplyPostingLineModel>>(value => value == postingJournal.ApplyPostingLines),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ApplyPostingJournalAsync_WhenCalled_AssertPostinglinesAsyncWasCalledOnWebApiClientWithCancellationTokenEqualToCancellationTokenFromArguments()
    {
        IAccountingGateway sut = CreateSut();

        CancellationToken cancellationToken = CancellationToken.None;
        await sut.ApplyPostingJournalAsync(_fixture!.Create<int>(), CreateApplyPostingJournalModel(), cancellationToken);

        _webApiClientMock!.Verify(m => m.PostinglinesAsync(
                It.IsAny<int>(),
                It.Is<string>(value => value == ApiVersions.Version1_0),
                It.IsAny<IEnumerable<ApplyPostingLineModel>>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ApplyPostingJournalAsync_WhenCalled_ReturnApplyPostingJournalResultModelFromWebApiClient()
    {
        ApplyPostingJournalResultModel applyPostingJournalResultModel = _fixture!.Create<ApplyPostingJournalResultModel>();
        IAccountingGateway sut = CreateSut(applyPostingJournalResultModel: applyPostingJournalResultModel);

        ApplyPostingJournalResultModel result = await sut.ApplyPostingJournalAsync(_fixture!.Create<int>(), CreateApplyPostingJournalModel());

        Assert.That(result, Is.EqualTo(applyPostingJournalResultModel));
    }

    [Test]
    [Category("UnitTest")]
    public async System.Threading.Tasks.Task ApplyPostingJournalAsync_WhenWebApiClientThrowsNonGenericWebApiClientException_ThrowsServiceGatewayExceptionBase()
    {
        WebApiClientException webApiClientException = _fixture!.CreateWebApiClientException((int) HttpStatusCode.BadRequest);
        IAccountingGateway sut = CreateSut(exception: webApiClientException);

        ServiceGatewayExceptionBase? result = (await Assert.ThrowsAsync<ServiceGatewayBadRequestException>(async () => await sut.ApplyPostingJournalAsync(_fixture.Create<int>(), CreateApplyPostingJournalModel())))!;

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async System.Threading.Tasks.Task ApplyPostingJournalAsync_WhenWebApiClientThrowsGenericWebApiClientExceptionWithErrorModel_ThrowsServiceGatewayExceptionBase()
    {
        WebApiClientException<ErrorModel> webApiClientException = _fixture!.CreateWebApiClientException((int) HttpStatusCode.BadRequest, result: _fixture!.CreateErrorModel());
        IAccountingGateway sut = CreateSut(exception: webApiClientException);

        ServiceGatewayExceptionBase? result = (await Assert.ThrowsAsync<ServiceGatewayBadRequestException>(async () => await sut.ApplyPostingJournalAsync(_fixture.Create<int>(), CreateApplyPostingJournalModel())))!;

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("IntegrationTest")]
    [Ignore("Integration test verified working; ignored because it requires live MySQL and OAuth/Graph services.")]
    public async Task ApplyPostingJournalAsync_WhenCalledWithDebitAndEqualCredit_ExpectNoErrors()
    {
        await using ServiceGatewayCreator serviceGatewayCreator = new ServiceGatewayCreator(CreateTestConfiguration());

        AccountingTestOptions accountingTestOptions = serviceGatewayCreator.GetAccountingTestOptions();

        IAccountingGateway sut = serviceGatewayCreator.CreateAccountingGateway();
        try
        {
            DateTimeOffset postingDate = DateTimeOffset.Now.Date;
            ApplyPostingJournalModel postingJournal = new ApplyPostingJournalModel(
                accountingTestOptions.ExistingAccountingNumber,
                new[]
                {
                    new ApplyPostingLineModel(accountingTestOptions.ExistingAccountNumberForAccount, null, null, null, 1000D, "Test", null, postingDate, null, 1),
                    new ApplyPostingLineModel(accountingTestOptions.ExistingAccountNumberForAccount, null, null, 1000D, null, "Test", null, postingDate, null, 2)
                });

            await sut.ApplyPostingJournalAsync(accountingTestOptions.ExistingAccountingNumber, postingJournal);
        }
        catch (Exception ex)
        {
            Assert.Fail(ex.Message);
        }
    }

    private IAccountingGateway CreateSut(ApplyPostingJournalResultModel? applyPostingJournalResultModel = null, Exception? exception = null)
    {
        if (exception != null)
        {
            _webApiClientMock!.Setup(m => m.PostinglinesAsync(It.IsAny<int>(), It.Is<string>(value => value == ApiVersions.Version1_0), It.IsAny<IEnumerable<ApplyPostingLineModel>>(), It.IsAny<CancellationToken>()))
                .Throws(exception);
        }
        else
        {
            _webApiClientMock!.Setup(m => m.PostinglinesAsync(It.IsAny<int>(), It.Is<string>(value => value == ApiVersions.Version1_0), It.IsAny<IEnumerable<ApplyPostingLineModel>>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(applyPostingJournalResultModel ?? _fixture!.Create<ApplyPostingJournalResultModel>()));
        }

        return new ServiceGateways.AccountingGateway(_webApiClientMock!.Object);
    }

    private ApplyPostingJournalModel CreateApplyPostingJournalModel()
    {
        return _fixture!.Create<ApplyPostingJournalModel>();
    }
}