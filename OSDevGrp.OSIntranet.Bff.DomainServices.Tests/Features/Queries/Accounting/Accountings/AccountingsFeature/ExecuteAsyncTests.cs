using System.Security.Claims;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.Accountings;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.UserHelper;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries.Accounting.Accountings.AccountingsFeature;

[TestFixture]
public class ExecuteAsyncTests : AccountingsFeatureTestBase
{
    #region Private variables

    private Mock<IPermissionChecker>? _permissionCheckerMock;
    private Mock<IAccountingGateway>? _accountingGatewayMock;
    private Mock<IStaticTextProvider>? _staticTextProviderMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _permissionCheckerMock = new Mock<IPermissionChecker>();
        _accountingGatewayMock = new Mock<IAccountingGateway>();
        _staticTextProviderMock = new Mock<IStaticTextProvider>();
        _fixture = new Fixture();
        _random = new Random(_fixture!.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task ExecuteAsync_WhenCalled_AssertUserWasCalledOnSecurityContextFromGivenAccountingsRequest(bool isAccountingCreator = true)
    {
        IQueryFeature<AccountingsRequest, AccountingsResponse> sut = CreateSut(isAccountingCreator: isAccountingCreator);

        Mock<ISecurityContext> securityContextMock = _fixture!.CreateSecurityContextMock();
        AccountingsRequest accountingsRequest = CreateAccountingsRequest(_fixture!, securityContext: securityContextMock.Object);
        await sut.ExecuteAsync(accountingsRequest);

        securityContextMock.Verify(m => m.User, Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task ExecuteAsync_WhenCalled_AssertIsAccountingCreatorWasCalledOnPermissionCheckerWithUserGivenBySecurityContextFromGivenAccountingsRequest(bool isAccountingCreator = true)
    {
        IQueryFeature<AccountingsRequest, AccountingsResponse> sut = CreateSut(isAccountingCreator: isAccountingCreator);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        ISecurityContext securityContext = _fixture!.CreateSecurityContext(user: user);
        AccountingsRequest accountingsRequest = CreateAccountingsRequest(_fixture!, securityContext: securityContext);
        await sut.ExecuteAsync(accountingsRequest);

        _permissionCheckerMock!.Verify(m => m.IsAccountingCreator(It.Is<ClaimsPrincipal>(value => value == user)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertGetAccountingsAsyncWasCalledOnAccountingGatewayWithGivenCancellationToken()
    {
        IQueryFeature<AccountingsRequest, AccountingsResponse> sut = CreateSut();

        AccountingsRequest accountingsRequest = CreateAccountingsRequest(_fixture!);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.ExecuteAsync(accountingsRequest, cancellationToken);

        _accountingGatewayMock!.Verify(m => m.GetAccountingsAsync(It.Is<CancellationToken>(value => value == cancellationToken)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task ExecuteAsync_WhenCalled_ReturnsAccountingsResponseWhereCreationAllowedIsEqulaToValueGivenByIsAccountingCreatorOnPermissionChecker(bool isAccountingCreator = true)
    {
        IQueryFeature<AccountingsRequest, AccountingsResponse> sut = CreateSut(isAccountingCreator: isAccountingCreator);

        AccountingsRequest accountingsRequest = CreateAccountingsRequest(_fixture!);
        AccountingsResponse result = await sut.ExecuteAsync(accountingsRequest);

        Assert.That(result.CreationAllowed, Is.EqualTo(isAccountingCreator));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_ReturnsAccountingsResponseWhereAccountingsContainsAccountingsFromAccountingGateway()
    {
        AccountingModel[] accountingModels = CreateAccountingModels(_fixture!, _random!);
        IQueryFeature<AccountingsRequest, AccountingsResponse> sut = CreateSut(accountingModels: accountingModels);

        AccountingsRequest accountingsRequest = CreateAccountingsRequest(_fixture!);
        AccountingsResponse result = await sut.ExecuteAsync(accountingsRequest);

        Assert.That(accountingModels.All(accountingModel => result.Accountings.Contains(accountingModel)), Is.True);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.Accountings)]
    [TestCase(StaticTextKey.CreateNewAccounting)]
    public async Task ExecuteAsync_WhenCalled_ReturnsAccountingsResponseWhereStaticTextsContainsExpectedStaticTextKey(StaticTextKey staticTextKey)
    {
        IQueryFeature<AccountingsRequest, AccountingsResponse> sut = CreateSut();

        AccountingsRequest accountingsRequest = CreateAccountingsRequest(_fixture!);
        AccountingsResponse result = await sut.ExecuteAsync(accountingsRequest);

        Assert.That(result.StaticTexts.ContainsKey(staticTextKey), Is.True);
    }

    private IQueryFeature<AccountingsRequest, AccountingsResponse> CreateSut(bool isAccountingCreator = true, IEnumerable<AccountingModel>? accountingModels = null)
    {
        _permissionCheckerMock!.Setup(_fixture!, isAccountingCreator: isAccountingCreator);

        _accountingGatewayMock!.Setup(m => m.GetAccountingsAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(accountingModels ?? CreateAccountingModels(_fixture!, _random!)));

        _staticTextProviderMock!.Setup(_fixture!);

        return new DomainServices.Features.Queries.Accounting.Accountings.AccountingsFeature(_permissionCheckerMock!.Object, _accountingGatewayMock!.Object, _staticTextProviderMock!.Object);
    }
}