using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests.Extensions;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using System.Net;

namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests.CommonGateway;

[TestFixture]
public class GetLetterHeadsAsyncTests : ServiceGatewayTestBase
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
    public async Task GetLetterHeadsAsync_WhenCalled_AssertLetterheadsAsyncWasCalledOnWebApiClient()
    {
        ICommonGateway sut = CreateSut();

        await sut.GetLetterHeadsAsync();

        _webApiClientMock!.Verify(m => m.LetterheadsAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetLetterHeadsAsync_WhenCalled_AssertLetterheadsAsyncWasCalledOnWebApiClientWithCancellationTokenEqualToCancellationTokenFromArguments()
    {
        ICommonGateway sut = CreateSut();

        CancellationToken cancellationToken = CancellationToken.None;
        await sut.GetLetterHeadsAsync(cancellationToken);

        _webApiClientMock!.Verify(m => m.LetterheadsAsync(It.Is<CancellationToken>(value => value == cancellationToken)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetLetterHeadsAsync_WhenCalled_ReturnLetterHeadModelsFromWebApiClient()
    {
        ICollection<LetterHeadModel> letterHeadModels = CreateLetterHeadModels();
        ICommonGateway sut = CreateSut(letterHeadModels: letterHeadModels);

        IEnumerable<LetterHeadModel> result = await sut.GetLetterHeadsAsync();

        Assert.That(result, Is.EqualTo(letterHeadModels));
    }

    [Test]
    [Category("UnitTest")]
    public void GetLetterHeadsAsync_WhenWebApiClientThrowsNonGenericWebApiClientException_ThrowsServiceGatewayExceptionBase()
    {
        WebApiClientException webApiClientException = _fixture!.CreateWebApiClientException((int) HttpStatusCode.BadRequest);
        ICommonGateway sut = CreateSut(exception: webApiClientException);

        ServiceGatewayExceptionBase? result = Assert.ThrowsAsync<ServiceGatewayBadRequestException>(async () => await sut.GetLetterHeadsAsync());

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void GetLetterHeadsAsync_WhenWebApiClientThrowsGenericWebApiClientExceptionWithErrorModel_ThrowsServiceGatewayExceptionBase()
    {
        WebApiClientException<ErrorModel> webApiClientException = _fixture!.CreateWebApiClientException((int) HttpStatusCode.BadRequest, result: _fixture!.CreateErrorModel());
        ICommonGateway sut = CreateSut(exception: webApiClientException);

        ServiceGatewayExceptionBase? result = Assert.ThrowsAsync<ServiceGatewayBadRequestException>(async () => await sut.GetLetterHeadsAsync());

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("IntegrationTest")]
    public async Task GetLetterHeadsAsync_WhenCalled_ExpectNoErrors()
    {
        await using ServiceGatewayCreator serviceGatewayCreator = new ServiceGatewayCreator(CreateTestConfiguration());

        ICommonGateway sut = serviceGatewayCreator.CreateCommonGateway();
        try
        {
            await sut.GetLetterHeadsAsync();
        }
        catch (Exception ex)
        {
            Assert.Fail(ex.Message);
        }
    }

    private ICommonGateway CreateSut(ICollection<LetterHeadModel>? letterHeadModels = null, Exception? exception = null)
    {
        if (exception != null)
        {
            _webApiClientMock!.Setup(m => m.LetterheadsAsync(It.IsAny<CancellationToken>()))
                .Throws(exception);
        }
        else
        {
            _webApiClientMock!.Setup(m => m.LetterheadsAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(letterHeadModels ?? CreateLetterHeadModels()));
        }

        return new ServiceGateways.CommonGateway(_webApiClientMock!.Object);
    }

    private ICollection<LetterHeadModel> CreateLetterHeadModels()
    {
        return _fixture!.CreateMany<LetterHeadModel>(_random!.Next(5, 10)).ToArray();
    }
}