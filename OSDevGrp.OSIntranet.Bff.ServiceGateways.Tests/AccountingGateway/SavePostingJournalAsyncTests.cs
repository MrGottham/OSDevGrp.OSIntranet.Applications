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
public class SavePostingJournalAsyncTests : ServiceGatewayTestBase
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
    public async Task SavePostingJournalAsync_WhenCalled_AssertPostingjournalPOSTAsyncWasCalledOnWebApiClient()
    {
        IAccountingGateway sut = CreateSut();

        await sut.SavePostingJournalAsync(_fixture!.Create<int>(), CreateApplyPostingJournalModel());

        _webApiClientMock!.Verify(m => m.PostingjournalPOSTAsync(
                It.IsAny<int>(),
                It.IsAny<ApplyPostingJournalModel>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task SavePostingJournalAsync_WhenCalled_AssertPostingjournalPOSTAsyncWasCalledOnWebApiClientWithAccountingNumberEqualToAccountingNumberFromArguments()
    {
        IAccountingGateway sut = CreateSut();

        int accountingNumber = _fixture!.Create<int>();
        await sut.SavePostingJournalAsync(accountingNumber, CreateApplyPostingJournalModel());

        _webApiClientMock!.Verify(m => m.PostingjournalPOSTAsync(
                It.Is<int>(value => value == accountingNumber),
                It.IsAny<ApplyPostingJournalModel>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task SavePostingJournalAsync_WhenCalled_AssertPostingjournalPOSTAsyncWasCalledOnWebApiClientWithPostingJournalEqualToPostingJournalFromArguments()
    {
        IAccountingGateway sut = CreateSut();

        ApplyPostingJournalModel postingJournal = CreateApplyPostingJournalModel();
        await sut.SavePostingJournalAsync(_fixture!.Create<int>(), postingJournal);

        _webApiClientMock!.Verify(m => m.PostingjournalPOSTAsync(
                It.IsAny<int>(),
                It.Is<ApplyPostingJournalModel>(value => value == postingJournal),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task SavePostingJournalAsync_WhenCalled_AssertPostingjournalPOSTAsyncWasCalledOnWebApiClientWithCancellationTokenEqualToCancellationTokenFromArguments()
    {
        IAccountingGateway sut = CreateSut();

        CancellationToken cancellationToken = CancellationToken.None;
        await sut.SavePostingJournalAsync(_fixture!.Create<int>(), CreateApplyPostingJournalModel(), cancellationToken);

        _webApiClientMock!.Verify(m => m.PostingjournalPOSTAsync(
                It.IsAny<int>(),
                It.IsAny<ApplyPostingJournalModel>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task SavePostingJournalAsync_WhenCalled_ReturnApplyPostingJournalModelFromWebApiClient()
    {
        ApplyPostingJournalModel postingJournalModel = CreateApplyPostingJournalModel();
        IAccountingGateway sut = CreateSut(postingJournalModel: postingJournalModel);

        ApplyPostingJournalModel result = await sut.SavePostingJournalAsync(_fixture!.Create<int>(), CreateApplyPostingJournalModel());

        Assert.That(result, Is.EqualTo(postingJournalModel));
    }

    [Test]
    [Category("UnitTest")]
    public void SavePostingJournalAsync_WhenWebApiClientThrowsNonGenericWebApiClientException_ThrowsServiceGatewayExceptionBase()
    {
        WebApiClientException webApiClientException = _fixture!.CreateWebApiClientException((int) HttpStatusCode.BadRequest);
        IAccountingGateway sut = CreateSut(exception: webApiClientException);

        ServiceGatewayExceptionBase? result = Assert.ThrowsAsync<ServiceGatewayBadRequestException>(async () => await sut.SavePostingJournalAsync(_fixture.Create<int>(), CreateApplyPostingJournalModel()));

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void SavePostingJournalAsync_WhenWebApiClientThrowsGenericWebApiClientExceptionWithErrorModel_ThrowsServiceGatewayExceptionBase()
    {
        WebApiClientException<ErrorModel> webApiClientException = _fixture!.CreateWebApiClientException((int) HttpStatusCode.BadRequest, result: _fixture!.CreateErrorModel());
        IAccountingGateway sut = CreateSut(exception: webApiClientException);

        ServiceGatewayExceptionBase? result = Assert.ThrowsAsync<ServiceGatewayBadRequestException>(async () => await sut.SavePostingJournalAsync(_fixture.Create<int>(), CreateApplyPostingJournalModel()));

        Assert.That(result, Is.Not.Null);
    }

    private IAccountingGateway CreateSut(ApplyPostingJournalModel? postingJournalModel = null, Exception? exception = null)
    {
        if (exception != null)
        {
            _webApiClientMock!.Setup(m => m.PostingjournalPOSTAsync(It.IsAny<int>(), It.IsAny<ApplyPostingJournalModel>(), It.IsAny<CancellationToken>()))
                .Throws(exception);
        }
        else
        {
            _webApiClientMock!.Setup(m => m.PostingjournalPOSTAsync(It.IsAny<int>(), It.IsAny<ApplyPostingJournalModel>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(postingJournalModel ?? CreateApplyPostingJournalModel()));
        }

        return new ServiceGateways.AccountingGateway(_webApiClientMock!.Object);
    }

    private ApplyPostingJournalModel CreateApplyPostingJournalModel()
    {
        return _fixture!.Create<ApplyPostingJournalModel>();
    }
}
