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
public class GetPostingLinesAsyncTests : ServiceGatewayTestBase
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
    public async Task GetPostingLinesAsync_WhenCalled_AssertPostinglinesAllAsyncWasCalledOnWebApiClient()
    {
        IAccountingGateway sut = CreateSut();

        await sut.GetPostingLinesAsync(_fixture.Create<int>(), DateTimeOffset.Now.AddDays(_random!.Next(0, 365) * -1), _fixture.Create<int>(), CreateFilter());

        _webApiClientMock!.Verify(m => m.PostinglinesAllAsync(
                It.IsAny<int>(),
                It.IsAny<DateTimeOffset?>(),
                It.IsAny<int?>(),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetPostingLinesAsync_WhenCalled_AssertPostinglinesAllAsyncWasCalledOnWebApiClientWithAccountingNumberEqualToAccountingNumberFromArguments()
    {
        IAccountingGateway sut = CreateSut();

        int accountingNumber = _fixture.Create<int>();
        await sut.GetPostingLinesAsync(accountingNumber, DateTimeOffset.Now.AddDays(_random!.Next(0, 365) * -1), _fixture.Create<int>(), CreateFilter());

        _webApiClientMock!.Verify(m => m.PostinglinesAllAsync(
                It.Is<int>(value => value == accountingNumber),
                It.IsAny<DateTimeOffset?>(),
                It.IsAny<int?>(),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetPostingLinesAsync_WhenCalled_AssertPostinglinesAllAsyncWasCalledOnWebApiClientWithStatusDateEqualToStatusDateFromArguments()
    {
        IAccountingGateway sut = CreateSut();

        DateTimeOffset statusDate = DateTimeOffset.Now.AddDays(_random!.Next(0, 365) * -1);
        await sut.GetPostingLinesAsync(_fixture.Create<int>(), statusDate, _fixture.Create<int>(), CreateFilter());

        _webApiClientMock!.Verify(m => m.PostinglinesAllAsync(
                It.IsAny<int>(),
                It.Is<DateTimeOffset?>(value => value != null && value == statusDate),
                It.IsAny<int?>(),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetPostingLinesAsync_WhenCalled_AssertPostinglinesAllAsyncWasCalledOnWebApiClientWithNumberOfPostingLinesEqualToNumberOfPostingLinesFromArguments()
    {
        IAccountingGateway sut = CreateSut();

        int numberOfPostingLines = _fixture.Create<int>();
        await sut.GetPostingLinesAsync(_fixture.Create<int>(), DateTimeOffset.Now.AddDays(_random!.Next(0, 365) * -1), numberOfPostingLines, CreateFilter());

        _webApiClientMock!.Verify(m => m.PostinglinesAllAsync(
                It.IsAny<int>(),
                It.IsAny<DateTimeOffset?>(),
                It.Is<int?>(value => value != null && value == numberOfPostingLines),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetPostingLinesAsync_WhenCalled_AssertPostinglinesAllAsyncWasCalledOnWebApiClientWithCancellationTokenEqualToCancellationTokenFromArguments()
    {
        IAccountingGateway sut = CreateSut();

        CancellationToken cancellationToken = CancellationToken.None;
        await sut.GetPostingLinesAsync(_fixture.Create<int>(), DateTimeOffset.Now.AddDays(_random!.Next(0, 365) * -1), _fixture.Create<int>(), CreateFilter(), cancellationToken);

        _webApiClientMock!.Verify(m => m.PostinglinesAllAsync(
                It.IsAny<int>(),
                It.IsAny<DateTimeOffset?>(),
                It.IsAny<int?>(),
                It.Is<CancellationToken>(value => value == cancellationToken)), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetPostingLinesAsync_WhenCalled_ReturnPostingLineModelsFromWebApiClient()
    {
        ICollection<PostingLineModel> postingLineModels = CreatePostingLineModels();
        IAccountingGateway sut = CreateSut(postingLineModels: postingLineModels);

        IEnumerable<PostingLineModel> result = await sut.GetPostingLinesAsync(_fixture.Create<int>(), DateTimeOffset.Now.AddDays(_random!.Next(0, 365) * -1), _fixture.Create<int>(), CreateFilter());

        Assert.That(result, Is.EqualTo(postingLineModels));
    }

    [Test]
    [Category("UnitTest")]
    public void GetPostingLinesAsync_WhenWebApiClientThrowsNonGenericWebApiClientException_ThrowsServiceGatewayExceptionBase()
    {
        WebApiClientException webApiClientException = _fixture!.CreateWebApiClientException((int) HttpStatusCode.BadRequest);
        IAccountingGateway sut = CreateSut(exception: webApiClientException);

        ServiceGatewayExceptionBase? result = Assert.ThrowsAsync<ServiceGatewayBadRequestException>(async () => await sut.GetPostingLinesAsync(_fixture.Create<int>(), DateTimeOffset.Now.AddDays(_random!.Next(0, 365) * -1), _fixture.Create<int>(), CreateFilter()));

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void GetPostingLinesAsync_WhenWebApiClientThrowsGenericWebApiClientExceptionWithErrorModel_ThrowsServiceGatewayExceptionBase()
    {
        WebApiClientException<ErrorModel> webApiClientException = _fixture!.CreateWebApiClientException((int) HttpStatusCode.BadRequest, result: _fixture!.CreateErrorModel());
        IAccountingGateway sut = CreateSut(exception: webApiClientException);

        ServiceGatewayExceptionBase? result = Assert.ThrowsAsync<ServiceGatewayBadRequestException>(async () => await sut.GetPostingLinesAsync(_fixture.Create<int>(), DateTimeOffset.Now.AddDays(_random!.Next(0, 365) * -1), _fixture.Create<int>(), CreateFilter()));

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("IntegrationTest")]
    public async Task GetPostingLinesAsync_WhenCalled_ExpectNoErrors()
    {
        await using ServiceGatewayCreator serviceGatewayCreator = new ServiceGatewayCreator(CreateTestConfiguration());

        AccountingTestOptions accountingTestOptions = serviceGatewayCreator.GetAccountingTestOptions();
        TimeProvider timeProvider = serviceGatewayCreator.GetTimeProvider();

        IAccountingGateway sut = serviceGatewayCreator.CreateAccountingGateway();
        try
        {
            await sut.GetPostingLinesAsync(accountingTestOptions.ExistingAccountingNumber, timeProvider.GetLocalNow(), 256, CreateFilter());
        }
        catch (Exception ex)
        {
            Assert.Fail(ex.Message);
        }
    }

    private IAccountingGateway CreateSut(ICollection<PostingLineModel>? postingLineModels = null, Exception? exception = null)
    {
        if (exception != null)
        {
            _webApiClientMock!.Setup(m => m.PostinglinesAllAsync(It.IsAny<int>(), It.IsAny<DateTimeOffset?>(), It.IsAny<int?>(), It.IsAny<CancellationToken>()))
                .Throws(exception);
        }
        else
        {
            _webApiClientMock!.Setup(m => m.PostinglinesAllAsync(It.IsAny<int>(), It.IsAny<DateTimeOffset?>(), It.IsAny<int?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(postingLineModels ?? CreatePostingLineModels()));
        }

        return new ServiceGateways.AccountingGateway(_webApiClientMock!.Object);
    }

    private ICollection<PostingLineModel> CreatePostingLineModels()
    {
        return _fixture!.CreateMany<PostingLineModel>(_random!.Next(10, 25)).ToArray();
    }

    private static Predicate<PostingLineModel> CreateFilter() => _ => true;
}