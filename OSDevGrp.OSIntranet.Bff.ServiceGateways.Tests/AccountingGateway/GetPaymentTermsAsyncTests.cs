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
public class GetPaymentTermsAsyncTests : ServiceGatewayTestBase
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
    public async Task GetPaymentTermsAsync_WhenCalled_AssertPaymenttermsAsyncWasCalledOnWebApiClient()
    {
        IAccountingGateway sut = CreateSut();

        await sut.GetPaymentTermsAsync();

        _webApiClientMock!.Verify(m => m.PaymenttermsAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetPaymentTermsAsync_WhenCalled_AssertPaymenttermsAsyncWasCalledOnWebApiClientWithCancellationTokenEqualToCancellationTokenFromArguments()
    {
        IAccountingGateway sut = CreateSut();

        CancellationToken cancellationToken = CancellationToken.None;
        await sut.GetPaymentTermsAsync(cancellationToken);

        _webApiClientMock!.Verify(m => m.PaymenttermsAsync(It.Is<CancellationToken>(value => value == cancellationToken)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetPaymentTermsAsync_WhenCalled_ReturnPaymentTermModelsFromWebApiClient()
    {
        ICollection<PaymentTermModel> paymentTermModels = CreatePaymentTermModels();
        IAccountingGateway sut = CreateSut(paymentTermModels: paymentTermModels);

        IEnumerable<PaymentTermModel> result = await sut.GetPaymentTermsAsync();

        Assert.That(result, Is.EqualTo(paymentTermModels));
    }

    [Test]
    [Category("UnitTest")]
    public void GetPaymentTermsAsync_WhenWebApiClientThrowsNonGenericWebApiClientException_ThrowsServiceGatewayExceptionBase()
    {
        WebApiClientException webApiClientException = _fixture!.CreateWebApiClientException((int) HttpStatusCode.BadRequest);
        IAccountingGateway sut = CreateSut(exception: webApiClientException);

        ServiceGatewayExceptionBase? result = Assert.ThrowsAsync<ServiceGatewayBadRequestException>(async () => await sut.GetPaymentTermsAsync());

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void GetPaymentTermsAsync_WhenWebApiClientThrowsGenericWebApiClientExceptionWithErrorModel_ThrowsServiceGatewayExceptionBase()
    {
        WebApiClientException<ErrorModel> webApiClientException = _fixture!.CreateWebApiClientException((int) HttpStatusCode.BadRequest, result: _fixture!.CreateErrorModel());
        IAccountingGateway sut = CreateSut(exception: webApiClientException);

        ServiceGatewayExceptionBase? result = Assert.ThrowsAsync<ServiceGatewayBadRequestException>(async () => await sut.GetPaymentTermsAsync());

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("IntegrationTest")]
    public async Task GetPaymentTermsAsync_WhenCalled_ExpectNoErrors()
    {
        await using ServiceGatewayCreator serviceGatewayCreator = new ServiceGatewayCreator(CreateTestConfiguration());

        IAccountingGateway sut = serviceGatewayCreator.CreateAccountingGateway();
        try
        {
            await sut.GetPaymentTermsAsync();
        }
        catch (Exception ex)
        {
            Assert.Fail(ex.Message);
        }
    }

    private IAccountingGateway CreateSut(ICollection<PaymentTermModel>? paymentTermModels = null, Exception? exception = null)
    {
        if (exception != null)
        {
            _webApiClientMock!.Setup(m => m.PaymenttermsAsync(It.IsAny<CancellationToken>()))
                .Throws(exception);
        }
        else
        {
            _webApiClientMock!.Setup(m => m.PaymenttermsAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(paymentTermModels ?? CreatePaymentTermModels()));
        }

        return new ServiceGateways.AccountingGateway(_webApiClientMock!.Object);
    }

    private ICollection<PaymentTermModel> CreatePaymentTermModels()
    {
        return _fixture!.CreateMany<PaymentTermModel>(_random!.Next(5, 10)).ToArray();
    }
}