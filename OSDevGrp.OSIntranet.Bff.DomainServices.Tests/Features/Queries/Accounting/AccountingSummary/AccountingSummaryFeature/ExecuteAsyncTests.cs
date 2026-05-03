using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.AccountingSummary;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText.AccountingTextsBuilder;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText.PostingLineCollectionTextsBuilder;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.AccountingRuleSetBuilder;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MaxLengthRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MinLengthRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.RequiredValueRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.UserHelper;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries.Accounting.AccountingSummary.AccountingSummaryFeature;

[TestFixture]
public class ExecuteAsyncTests : AccountingPageFeatureTestBase
{
    #region Private variables

    private Mock<IPermissionChecker>? _permissionCheckerMock;
    private Mock<IAccountingGateway>? _accountingGatewayMock;
    private Mock<IStaticTextProvider>? _staticTextProviderMock;
    private Mock<IAccountingTextsBuilder>? _accountingTextsBuilderMock;
    private Mock<IEmptyRuleSetBuilder>? _emptyRuleSetBuilderMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _permissionCheckerMock = new Mock<IPermissionChecker>();
        _accountingGatewayMock = new Mock<IAccountingGateway>();
        _staticTextProviderMock = new Mock<IStaticTextProvider>();
        _accountingTextsBuilderMock = new Mock<IAccountingTextsBuilder>();
        _emptyRuleSetBuilderMock = new Mock<IEmptyRuleSetBuilder>();
        _fixture = new Fixture();
        _random = new Random(_fixture!.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertGetAccountingAsyncWasCalledOnAccountingGatewayWithAccountingNumberFromAccountingSummaryRequest()
    {
        IQueryFeature<AccountingSummaryRequest, AccountingSummaryResponse> sut = CreateSut();

        int accountingNumber = _fixture!.Create<int>();
        AccountingSummaryRequest accountingSummaryRequest = CreateAccountingSummaryRequest(_fixture!, accountingNumber: accountingNumber);
        await sut.ExecuteAsync(accountingSummaryRequest);

        _accountingGatewayMock!.Verify(m => m.GetAccountingAsync(
                It.Is<int>(value => value == accountingNumber),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertGetAccountingAsyncWasCalledOnAccountingGatewayWithStatusDateFromAccountingSummaryRequest()
    {
        IQueryFeature<AccountingSummaryRequest, AccountingSummaryResponse> sut = CreateSut();

        DateTimeOffset statusDate = DateTimeOffset.Now.AddDays(_random!.Next(0, 7) * -1).Date;
        AccountingSummaryRequest accountingSummaryRequest = CreateAccountingSummaryRequest(_fixture!, statusDate: statusDate);
        await sut.ExecuteAsync(accountingSummaryRequest);

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
        IQueryFeature<AccountingSummaryRequest, AccountingSummaryResponse> sut = CreateSut();

        AccountingSummaryRequest accountingSummaryRequest = CreateAccountingSummaryRequest(_fixture!);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.ExecuteAsync(accountingSummaryRequest, cancellationToken);

        _accountingGatewayMock!.Verify(m => m.GetAccountingAsync(
                It.IsAny<int>(),
                It.IsAny<DateTimeOffset>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(1)]
    [TestCase(16)]
    [TestCase(32)]
    public async Task ExecuteAsync_WhenNumberOfPostingLinesInAccountingSummaryRequestIsGreaterThanZero_AssertGetPostingLinesAsyncWasCalledOnAccountingGatewayWithAccountingNumberFromAccountingSummaryRequest(int numberOfPostingLines)
    {
        IQueryFeature<AccountingSummaryRequest, AccountingSummaryResponse> sut = CreateSut();

        int accountingNumber = _fixture!.Create<int>();
        AccountingSummaryRequest accountingSummaryRequest = CreateAccountingSummaryRequest(_fixture!, accountingNumber: accountingNumber, numberOfPostingLines: numberOfPostingLines);
        await sut.ExecuteAsync(accountingSummaryRequest);

        _accountingGatewayMock!.Verify(m => m.GetPostingLinesAsync(
                It.Is<int>(value => value == accountingNumber),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<int>(),
                It.IsAny<Predicate<PostingLineModel>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(1)]
    [TestCase(16)]
    [TestCase(32)]
    public async Task ExecuteAsync_WhenNumberOfPostingLinesInAccountingSummaryRequestIsGreaterThanZero_AssertGetPostingLinesAsyncWasCalledOnAccountingGatewayWithStatusDateFromAccountingSummaryRequest(int numberOfPostingLines)
    {
        IQueryFeature<AccountingSummaryRequest, AccountingSummaryResponse> sut = CreateSut();

        DateTimeOffset statusDate = DateTimeOffset.Now.AddDays(_random!.Next(0, 7) * -1).Date;
        AccountingSummaryRequest accountingSummaryRequest = CreateAccountingSummaryRequest(_fixture!, statusDate: statusDate, numberOfPostingLines: numberOfPostingLines);
        await sut.ExecuteAsync(accountingSummaryRequest);

        _accountingGatewayMock!.Verify(m => m.GetPostingLinesAsync(
                It.IsAny<int>(),
                It.Is<DateTimeOffset>(value => value == statusDate),
                It.IsAny<int>(),
                It.IsAny<Predicate<PostingLineModel>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(1)]
    [TestCase(16)]
    [TestCase(32)]
    public async Task ExecuteAsync_WhenNumberOfPostingLinesInAccountingSummaryRequestIsGreaterThanZero_AssertGetPostingLinesAsyncWasCalledOnAccountingGatewayWithNumberOfPostingLinesFromAccountingSummaryRequest(int numberOfPostingLines)
    {
        IQueryFeature<AccountingSummaryRequest, AccountingSummaryResponse> sut = CreateSut();

        AccountingSummaryRequest accountingSummaryRequest = CreateAccountingSummaryRequest(_fixture!, numberOfPostingLines: numberOfPostingLines);
        await sut.ExecuteAsync(accountingSummaryRequest);

        _accountingGatewayMock!.Verify(m => m.GetPostingLinesAsync(
                It.IsAny<int>(),
                It.IsAny<DateTimeOffset>(),
                It.Is<int>(value => value == numberOfPostingLines),
                It.IsAny<Predicate<PostingLineModel>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(1)]
    [TestCase(16)]
    [TestCase(32)]
    public async Task ExecuteAsync_WhenNumberOfPostingLinesInAccountingSummaryRequestIsGreaterThanZero_AssertGetPostingLinesAsyncWasCalledOnAccountingGatewayWithGivenCancellationToken(int numberOfPostingLines)
    {
        IQueryFeature<AccountingSummaryRequest, AccountingSummaryResponse> sut = CreateSut();

        AccountingSummaryRequest accountingSummaryRequest = CreateAccountingSummaryRequest(_fixture!, numberOfPostingLines: numberOfPostingLines);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.ExecuteAsync(accountingSummaryRequest, cancellationToken);

        _accountingGatewayMock!.Verify(m => m.GetPostingLinesAsync(
                It.IsAny<int>(),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<int>(),
                It.IsAny<Predicate<PostingLineModel>>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(0)]
    [TestCase(-16)]
    [TestCase(-32)]
    public async Task ExecuteAsync_WhenNumberOfPostingLinesInAccountingSummaryRequestIsLessThanOrEqualToZero_AssertGetPostingLinesAsyncWasNotCalledOnAccountingGateway(int numberOfPostingLines)
    {
        IQueryFeature<AccountingSummaryRequest, AccountingSummaryResponse> sut = CreateSut();

        AccountingSummaryRequest accountingSummaryRequest = CreateAccountingSummaryRequest(_fixture!, numberOfPostingLines: numberOfPostingLines);
        await sut.ExecuteAsync(accountingSummaryRequest);

        _accountingGatewayMock!.Verify(m => m.GetPostingLinesAsync(
                It.IsAny<int>(),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<int>(),
                It.IsAny<Predicate<PostingLineModel>>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_ReturnsAccountingSummaryResponseWhereAccountingIsEqualToAccountingModelResolvedByAccountingGateway()
    {
        AccountingModel accountingModel = _fixture!.CreateAccountingModel(_random!);
        IQueryFeature<AccountingSummaryRequest, AccountingSummaryResponse> sut = CreateSut(accountingModel: accountingModel);

        AccountingSummaryRequest accountingSummaryRequest = CreateAccountingSummaryRequest(_fixture!);
        AccountingSummaryResponse result = await sut.ExecuteAsync(accountingSummaryRequest);

        Assert.That(result.Accounting, Is.EqualTo(accountingModel));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(1)]
    [TestCase(16)]
    [TestCase(32)]
    public async Task ExecuteAsync_WhenNumberOfPostingLinesInAccountingSummaryRequestIsGreaterThanZero_ReturnsAccountingSummaryResponseWherePostingLinesIsEqualToPostingLineModelsResolvedByAccountingGateway(int numberOfPostingLines)
    {
        PostingLineModel[] postingLineModels = _fixture!.CreatePostingLineModels(_random!);
        IQueryFeature<AccountingSummaryRequest, AccountingSummaryResponse> sut = CreateSut(postingLineModels: postingLineModels);

        AccountingSummaryRequest accountingSummaryRequest = CreateAccountingSummaryRequest(_fixture!, numberOfPostingLines: numberOfPostingLines);
        AccountingSummaryResponse result = await sut.ExecuteAsync(accountingSummaryRequest);

        Assert.That(result.PostingLines, Is.EqualTo(postingLineModels));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(0)]
    [TestCase(-16)]
    [TestCase(-32)]
    public async Task ExecuteAsync_WhenNumberOfPostingLinesInAccountingSummaryRequestIsLessThanOrEqualToZero_ReturnsAccountingSummaryResponseWherePostingLinesIsEmpty(int numberOfPostingLines)
    {
        IQueryFeature<AccountingSummaryRequest, AccountingSummaryResponse> sut = CreateSut();

        AccountingSummaryRequest accountingSummaryRequest = CreateAccountingSummaryRequest(_fixture!, numberOfPostingLines: numberOfPostingLines);
        AccountingSummaryResponse result = await sut.ExecuteAsync(accountingSummaryRequest);

        Assert.That(result.PostingLines, Is.Empty);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_ReturnsAccountingSummaryResponseWhereDynamicTextsIsEqualToAccountingTextsResolvedByAccountingTextsBuilder()
    {
        IAccountingTexts accountingTexts = new Mock<IAccountingTexts>().Object;
        IQueryFeature<AccountingSummaryRequest, AccountingSummaryResponse> sut = CreateSut(accountingTexts: accountingTexts);

        AccountingSummaryRequest accountingSummaryRequest = CreateAccountingSummaryRequest(_fixture!);
        AccountingSummaryResponse result = await sut.ExecuteAsync(accountingSummaryRequest);

        Assert.That(result.DynamicTexts, Is.EqualTo(accountingTexts));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task ExecuteAsync_WhenCalled_ReturnsAccountingSummaryResponseWhereStaticTextsContainsExpectedStaticTextKey(StaticTextKey staticTextKey)
    {
        IQueryFeature<AccountingSummaryRequest, AccountingSummaryResponse> sut = CreateSut();

        AccountingSummaryRequest accountingSummaryRequest = CreateAccountingSummaryRequest(_fixture!);
        AccountingSummaryResponse result = await sut.ExecuteAsync(accountingSummaryRequest);

        Assert.That(result.StaticTexts.ContainsKey(staticTextKey), Is.True);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_ReturnsAccountingSummaryResponseWhereValidationRuleSetIsEmpty()
    {
        IQueryFeature<AccountingSummaryRequest, AccountingSummaryResponse> sut = CreateSut();

        AccountingSummaryRequest accountingSummaryRequest = CreateAccountingSummaryRequest(_fixture!);
        AccountingSummaryResponse result = await sut.ExecuteAsync(accountingSummaryRequest);

        Assert.That(result.ValidationRuleSet, Is.Empty);
    }

    private IQueryFeature<AccountingSummaryRequest, AccountingSummaryResponse> CreateSut(AccountingModel? accountingModel = null, IEnumerable<PostingLineModel>? postingLineModels = null, IAccountingTexts? accountingTexts = null)
    {
        _permissionCheckerMock!.Setup(_fixture!);
        _staticTextProviderMock!.Setup(_fixture!);
        _accountingTextsBuilderMock!.Setup(accountingTexts: accountingTexts);
        _emptyRuleSetBuilderMock!.Setup();

        _accountingGatewayMock!.Setup(m => m.GetAccountingAsync(It.IsAny<int>(), It.IsAny<DateTimeOffset>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(accountingModel ?? _fixture!.CreateAccountingModel(_random!)));
        _accountingGatewayMock!.Setup(m => m.GetPostingLinesAsync(It.IsAny<int>(), It.IsAny<DateTimeOffset>(), It.IsAny<int>(), It.IsAny<Predicate<PostingLineModel>>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(postingLineModels ?? _fixture!.CreatePostingLineModels(_random!)));

        return new DomainServices.Features.Queries.Accounting.AccountingSummary.AccountingSummaryFeature(_permissionCheckerMock!.Object, _accountingGatewayMock!.Object, _staticTextProviderMock!.Object, _accountingTextsBuilderMock!.Object, _emptyRuleSetBuilderMock!.Object);
    }

    private static AccountingSummaryRequest CreateAccountingSummaryRequest(Fixture fixture, int? accountingNumber = null, DateTimeOffset? statusDate = null, int? numberOfPostingLines = null, ISecurityContext? securityContext = null)
    {
        return new AccountingSummaryRequest(Guid.NewGuid(), accountingNumber ?? fixture.Create<int>(), statusDate ?? DateTimeOffset.Now.Date, numberOfPostingLines ?? fixture.Create<int>(), CultureInfo.InvariantCulture, securityContext ?? CreateSecurityContext(fixture));
    }
}