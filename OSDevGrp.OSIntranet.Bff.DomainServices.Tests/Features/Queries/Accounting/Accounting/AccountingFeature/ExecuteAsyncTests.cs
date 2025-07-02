using System.Globalization;
using System.Security.Claims;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.Accounting;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.UserHelper;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries.Accounting.Accounting.AccountingFeature;

[TestFixture]
public class ExecuteAsyncTests : AccountingPageFeatureTestBase
{
    #region Private variables

    private Mock<IPermissionChecker>? _permissionCheckerMock;
    private Mock<IAccountingGateway>? _accountingGatewayMock;
    private Mock<ICommonGateway>? _commonGatewayMock;
    private Mock<IStaticTextProvider>? _staticTextProviderMock;
    private Mock<IAccountingTextsBuilder>? _accountingTextsBuilderMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _permissionCheckerMock = new Mock<IPermissionChecker>();
        _accountingGatewayMock = new Mock<IAccountingGateway>();
        _commonGatewayMock = new Mock<ICommonGateway>();
        _staticTextProviderMock = new Mock<IStaticTextProvider>();
        _accountingTextsBuilderMock = new Mock<IAccountingTextsBuilder>();
        _fixture = new Fixture();
        _random = new Random(_fixture!.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertGetAccountingAsyncWasCalledOnAccountingGatewayWithAccountingNumberFromAccountingRequest()
    {
        IQueryFeature<AccountingRequest, AccountingResponse> sut = CreateSut();

        int accountingNumber = _fixture!.Create<int>();
        AccountingRequest accountingRequest = CreateAccountingRequest(_fixture!, accountingNumber: accountingNumber);
        await sut.ExecuteAsync(accountingRequest);

        _accountingGatewayMock!.Verify(m => m.GetAccountingAsync(
                It.Is<int>(value => value == accountingNumber),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertGetAccountingAsyncWasCalledOnAccountingGatewayWithStatusDateFromAccountingRequest()
    {
        IQueryFeature<AccountingRequest, AccountingResponse> sut = CreateSut();

        DateTimeOffset statusDate = DateTimeOffset.Now.AddDays(_random!.Next(0, 7) * -1).Date;
        AccountingRequest accountingRequest = CreateAccountingRequest(_fixture!, statusDate: statusDate);
        await sut.ExecuteAsync(accountingRequest);

        _accountingGatewayMock!.Verify(m => m.GetAccountingAsync(
                It.IsAny<int>(),
                It.Is<DateTimeOffset>(value => value == statusDate),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertGetAccountingAsyncWasCalledOnAccountingGatewayWithGivenCancellationToken()
    {
        IQueryFeature<AccountingRequest, AccountingResponse> sut = CreateSut();

        AccountingRequest accountingRequest = CreateAccountingRequest(_fixture!);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.ExecuteAsync(accountingRequest, cancellationToken);

        _accountingGatewayMock!.Verify(m => m.GetAccountingAsync(
                It.IsAny<int>(),
                It.IsAny<DateTimeOffset>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task ExecuteAsync_WhenCalled_AssertUserWasCalledOnSecurityContextFromAccountingRequest(bool hasCommonDataAccess)
    {
        IQueryFeature<AccountingRequest, AccountingResponse> sut = CreateSut(hasCommonDataAccess: hasCommonDataAccess);

        Mock<ISecurityContext> securityContextMock = _fixture!.CreateSecurityContextMock();
        AccountingRequest accountingRequest = CreateAccountingRequest(_fixture!, securityContext: securityContextMock.Object);
        await sut.ExecuteAsync(accountingRequest);

        securityContextMock.Verify(m => m.User, Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task ExecuteAsync_WhenCalled_AssertHasCommonDataAccessWasCalledOnPermissionCheckerWithUserFromSecurityContextInAccountingRequest(bool hasCommonDataAccess)
    {
        IQueryFeature<AccountingRequest, AccountingResponse> sut = CreateSut(hasCommonDataAccess: hasCommonDataAccess);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        ISecurityContext securityContext = _fixture!.CreateSecurityContext(user: user);
        AccountingRequest accountingRequest = CreateAccountingRequest(_fixture!, securityContext: securityContext);
        await sut.ExecuteAsync(accountingRequest);

        _permissionCheckerMock!.Verify(m => m.HasCommonDataAccess(It.Is<ClaimsPrincipal>(value => value == user)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenUserHasCommonDataAccess_AssertGetLetterHeadsAsyncWasCalledOnCommonGatewayWithGivenCancellationToken()
    {
        IQueryFeature<AccountingRequest, AccountingResponse> sut = CreateSut(hasCommonDataAccess: true);

        AccountingRequest accountingRequest = CreateAccountingRequest(_fixture!);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.ExecuteAsync(accountingRequest, cancellationToken);

        _commonGatewayMock!.Verify(m => m.GetLetterHeadsAsync(It.Is<CancellationToken>(value => value == cancellationToken)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenUserDoesNotHaveCommonDataAccess_AssertGetLetterHeadsAsyncWasNotCalledOnCommonGateway()
    {
        IQueryFeature<AccountingRequest, AccountingResponse> sut = CreateSut(hasCommonDataAccess: false);

        AccountingRequest accountingRequest = CreateAccountingRequest(_fixture!);
        await sut.ExecuteAsync(accountingRequest);

        _commonGatewayMock!.Verify(m => m.GetLetterHeadsAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_ReturnsAccountingResponseWhereModelIsEqualToAccountingModelResolvedByAccountingGateway()
    {
        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IQueryFeature<AccountingRequest, AccountingResponse> sut = CreateSut(accountingModel: accountingModel);

        AccountingRequest accountingRequest = CreateAccountingRequest(_fixture!);
        AccountingResponse result = await sut.ExecuteAsync(accountingRequest);

        Assert.That(result.Model, Is.EqualTo(accountingModel));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenUserHasCommonDataAccess_ReturnsAccountingResponseWhereLetterHeadsContainsLetterHeadsResovledByCommonGateway()
    {
        LetterHeadModel[] letterHeadModels = _fixture!.CreateLetterHeadModels(_random!);
        IQueryFeature<AccountingRequest, AccountingResponse> sut = CreateSut(hasCommonDataAccess: true, letterHeadModels: letterHeadModels);

        AccountingRequest accountingRequest = CreateAccountingRequest(_fixture!);
        AccountingResponse result = await sut.ExecuteAsync(accountingRequest);

        Assert.That(letterHeadModels.All(letterHeadModel => result.LetterHeads.Any(m => m.Number == letterHeadModel.Number && m.Name == letterHeadModel.Name)), Is.True);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenUserDoesNotHaveCommonDataAccess_ReturnsAccountingResponseWhereLetterHeadsOnlyContainsLetterHeadFromAccountingModelResolvedByAccountingGateway()
    {
        LetterHeadIdentificationModel letterHeadIdentificationModel = _fixture!.CreateLetterHeadIdentificationModel(_random!);
        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!, letterHeadIdentificationModel: letterHeadIdentificationModel);
        IQueryFeature<AccountingRequest, AccountingResponse> sut = CreateSut(hasCommonDataAccess: false, accountingModel: accountingModel);

        AccountingRequest accountingRequest = CreateAccountingRequest(_fixture!);
        AccountingResponse result = await sut.ExecuteAsync(accountingRequest);

        Assert.Multiple(() =>
        {
            Assert.That(result.LetterHeads.Count, Is.EqualTo(1));
            Assert.That(result.LetterHeads.ElementAt(0).Number, Is.EqualTo(letterHeadIdentificationModel.Number));
            Assert.That(result.LetterHeads.ElementAt(0).Name, Is.EqualTo(letterHeadIdentificationModel.Name));
        });
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_ReturnsAccountingResponseWhereDynamicTextsIsEqualToAccountingTextsResolvedByAccountingTextsBuilder()
    {
        IAccountingTexts accountingTexts = new Mock<IAccountingTexts>().Object;
        IQueryFeature<AccountingRequest, AccountingResponse> sut = CreateSut(accountingTexts: accountingTexts);

        AccountingRequest accountingRequest = CreateAccountingRequest(_fixture!);
        AccountingResponse result = await sut.ExecuteAsync(accountingRequest);

        Assert.That(result.DynamicTexts, Is.EqualTo(accountingTexts));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.UpdateAccounting)]
    [TestCase(StaticTextKey.DeleteAccounting)]
    [TestCase(StaticTextKey.MasterData)]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    [TestCase(StaticTextKey.LetterHead)]
    [TestCase(StaticTextKey.BalanceBelowZero)]
    [TestCase(StaticTextKey.Debtors)]
    [TestCase(StaticTextKey.Creditors)]
    [TestCase(StaticTextKey.BackDating)]
    public async Task ExecuteAsync_WhenCalled_ReturnsAccountingResponseWhereStaticTextsContainsExpectedStaticTextKey(StaticTextKey staticTextKey)
    {
        IAccountingTexts accountingTexts = new Mock<IAccountingTexts>().Object;
        IQueryFeature<AccountingRequest, AccountingResponse> sut = CreateSut(accountingTexts: accountingTexts);

        AccountingRequest accountingRequest = CreateAccountingRequest(_fixture!);
        AccountingResponse result = await sut.ExecuteAsync(accountingRequest);

        Assert.That(result.StaticTexts.ContainsKey(staticTextKey), Is.True);
    }

    private IQueryFeature<AccountingRequest, AccountingResponse> CreateSut(AccountingModel? accountingModel = null, IAccountingTexts? accountingTexts = null, bool hasCommonDataAccess = true, IEnumerable<LetterHeadModel>? letterHeadModels = null)
    {
        _permissionCheckerMock!.Setup(_fixture!, hasCommonDataAccess: hasCommonDataAccess);
        _staticTextProviderMock!.Setup(_fixture!);
        _accountingTextsBuilderMock!.Setup(accountingTexts: accountingTexts);

        _accountingGatewayMock!.Setup(m => m.GetAccountingAsync(It.IsAny<int>(), It.IsAny<DateTimeOffset>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(accountingModel ?? _fixture!.CreateAccountingModel(_random!)));
        _commonGatewayMock!.Setup(m => m.GetLetterHeadsAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(letterHeadModels ?? _fixture!.CreateLetterHeadModels(_random!)));

        return new DomainServices.Features.Queries.Accounting.Accounting.AccountingFeature(_permissionCheckerMock!.Object, _accountingGatewayMock!.Object, _commonGatewayMock!.Object, _staticTextProviderMock!.Object, _accountingTextsBuilderMock!.Object);
    }

    private static AccountingRequest CreateAccountingRequest(Fixture fixture, int? accountingNumber = null, DateTimeOffset? statusDate = null, ISecurityContext? securityContext = null)
    {
        return new AccountingRequest(Guid.NewGuid(), accountingNumber ?? fixture.Create<int>(), statusDate ?? DateTimeOffset.Now.Date, CultureInfo.InvariantCulture, securityContext ?? CreateSecurityContext(fixture));
    }
}