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
public class GetBudgetAccountGroupsAsyncTests : ServiceGatewayTestBase
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
    public async Task GetBudgetAccountGroupsAsync_WhenCalled_AssertBudgetaccountgroupsAsyncWasCalledOnWebApiClient()
    {
        IAccountingGateway sut = CreateSut();

        await sut.GetBudgetAccountGroupsAsync();

        _webApiClientMock!.Verify(m => m.BudgetaccountgroupsAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetBudgetAccountGroupsAsync_WhenCalled_AssertBudgetaccountgroupsAsyncWasCalledOnWebApiClientWithCancellationTokenEqualToCancellationTokenFromArguments()
    {
        IAccountingGateway sut = CreateSut();

        CancellationToken cancellationToken = CancellationToken.None;
        await sut.GetBudgetAccountGroupsAsync(cancellationToken);

        _webApiClientMock!.Verify(m => m.BudgetaccountgroupsAsync(It.Is<CancellationToken>(value => value == cancellationToken)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetBudgetAccountGroupsAsync_WhenCalled_ReturnBudgetAccountGroupModelsFromWebApiClient()
    {
        ICollection<BudgetAccountGroupModel> budgetAccountGroupModels = CreateBudgetAccountGroupModels();
        IAccountingGateway sut = CreateSut(budgetAccountGroupModels: budgetAccountGroupModels);

        IEnumerable<BudgetAccountGroupModel> result = await sut.GetBudgetAccountGroupsAsync();

        Assert.That(result, Is.EqualTo(budgetAccountGroupModels));
    }

    [Test]
    [Category("UnitTest")]
    public void GetBudgetAccountGroupsAsync_WhenWebApiClientThrowsNonGenericWebApiClientException_ThrowsServiceGatewayExceptionBase()
    {
        WebApiClientException webApiClientException = _fixture!.CreateWebApiClientException((int) HttpStatusCode.BadRequest);
        IAccountingGateway sut = CreateSut(exception: webApiClientException);

        ServiceGatewayExceptionBase? result = Assert.ThrowsAsync<ServiceGatewayBadRequestException>(async () => await sut.GetBudgetAccountGroupsAsync());

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void GetBudgetAccountGroupsAsync_WhenWebApiClientThrowsGenericWebApiClientExceptionWithErrorModel_ThrowsServiceGatewayExceptionBase()
    {
        WebApiClientException<ErrorModel> webApiClientException = _fixture!.CreateWebApiClientException((int) HttpStatusCode.BadRequest, result: _fixture!.CreateErrorModel());
        IAccountingGateway sut = CreateSut(exception: webApiClientException);

        ServiceGatewayExceptionBase? result = Assert.ThrowsAsync<ServiceGatewayBadRequestException>(async () => await sut.GetBudgetAccountGroupsAsync());

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("IntegrationTest")]
    public async Task GetBudgetAccountGroupsAsync_WhenCalled_ExpectNoErrors()
    {
        await using ServiceGatewayCreator serviceGatewayCreator = new ServiceGatewayCreator(CreateTestConfiguration());

        IAccountingGateway sut = serviceGatewayCreator.CreateAccountingGateway();
        try
        {
            await sut.GetBudgetAccountGroupsAsync();
        }
        catch (Exception ex)
        {
            Assert.Fail(ex.Message);
        }
    }

    private IAccountingGateway CreateSut(ICollection<BudgetAccountGroupModel>? budgetAccountGroupModels = null, Exception? exception = null)
    {
        if (exception != null)
        {
            _webApiClientMock!.Setup(m => m.BudgetaccountgroupsAsync(It.IsAny<CancellationToken>()))
                .Throws(exception);
        }
        else
        {
            _webApiClientMock!.Setup(m => m.BudgetaccountgroupsAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(budgetAccountGroupModels ?? CreateBudgetAccountGroupModels()));
        }

        return new ServiceGateways.AccountingGateway(_webApiClientMock!.Object);
    }

    private ICollection<BudgetAccountGroupModel> CreateBudgetAccountGroupModels()
    {
        return _fixture!.CreateMany<BudgetAccountGroupModel>(_random!.Next(5, 10)).ToArray();
    }
}