using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Accounting;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.PostingJournal;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData;
using OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;
using OSDevGrp.OSIntranet.Bff.WebApi.Tests.Controllers.Accounting.Dtos;
using OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security.SecurityContextProvider;
using OSDevGrp.OSIntranet.Bff.WebApi.Tests.Shared.Dtos;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Controllers.Accounting.AccountingController;

[TestFixture]
public class AppendPostingLineToPostingJournalAsyncTests
{
    #region Private variables

    private Mock<TimeProvider>? _timeProviderMock;
    private Mock<ISecurityContextProvider>? _securityContextProviderMock;
    private Mock<ICommandFeature<AppendPostingLineToPostingJournalRequest>>? _commandFeatureMock;
    private Mock<IQueryFeature<PostingJournalRequest, PostingJournalResponse>>? _queryFeatureMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _timeProviderMock = new Mock<TimeProvider>();
        _securityContextProviderMock = new Mock<ISecurityContextProvider>();
        _commandFeatureMock = new Mock<ICommandFeature<AppendPostingLineToPostingJournalRequest>>();
        _queryFeatureMock = new Mock<IQueryFeature<PostingJournalRequest, PostingJournalResponse>>();
        _fixture = new Fixture();
        _random = new Random(_fixture.Create<int>());
    }

    #region Command Feature Invocation Tests

    [Test]
    [Category("UnitTest")]
    public async Task AppendPostingLineToPostingJournalAsync_WhenCalled_AssertGetCurrentSecurityContextAsyncWasCalledOnSecurityContextProviderWithGivenCancellationToken()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();
        AppendPostingLineToPostingJournalDto dto = CreateAppendPostingLineToPostingJournalDto();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.AppendPostingLineToPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), dto, cancellationToken);

        _securityContextProviderMock!.Verify(m => m.GetCurrentSecurityContextAsync(It.Is<CancellationToken>(value => value == cancellationToken)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AppendPostingLineToPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnCommandFeatureWithAppendPostingLineToPostingJournalRequestWhereRequestIdIsNotEqualToGuidEmpty()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();
        AppendPostingLineToPostingJournalDto dto = CreateAppendPostingLineToPostingJournalDto();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AppendPostingLineToPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), dto, cancellationTokenSource.Token);

        _commandFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<AppendPostingLineToPostingJournalRequest>(value => value.RequestId != Guid.Empty),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AppendPostingLineToPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnCommandFeatureWithAppendPostingLineToPostingJournalRequestWhereAccountingNumberIsEqualToGivenAccountingNumber()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();
        AppendPostingLineToPostingJournalDto dto = CreateAppendPostingLineToPostingJournalDto();

        int accountingNumber = _fixture!.Create<int>();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AppendPostingLineToPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, accountingNumber, dto, cancellationTokenSource.Token);

        _commandFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<AppendPostingLineToPostingJournalRequest>(value => value.AccountingNumber == accountingNumber),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AppendPostingLineToPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnCommandFeatureWithAppendPostingLineToPostingJournalRequestWhereIdentifierIsEqualToDtoIdentifier()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();
        Guid expectedIdentifier = _fixture!.Create<Guid>();
        AppendPostingLineToPostingJournalDto dto = CreateAppendPostingLineToPostingJournalDto(identifier: expectedIdentifier);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AppendPostingLineToPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), dto, cancellationTokenSource.Token);

        _commandFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<AppendPostingLineToPostingJournalRequest>(value => value.Identifier == expectedIdentifier),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AppendPostingLineToPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnCommandFeatureWithAppendPostingLineToPostingJournalRequestWherePostingDateIsEqualToDtoPostingDate()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();
        DateTimeOffset expectedPostingDate = DateTimeOffset.Now;
        AppendPostingLineToPostingJournalDto dto = CreateAppendPostingLineToPostingJournalDto(postingDate: expectedPostingDate);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AppendPostingLineToPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), dto, cancellationTokenSource.Token);

        _commandFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<AppendPostingLineToPostingJournalRequest>(value => value.PostingDate == expectedPostingDate),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AppendPostingLineToPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnCommandFeatureWithAppendPostingLineToPostingJournalRequestWherePostingReferenceIsEqualToDtoPostingReference()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();
        string expectedPostingReference = _fixture!.Create<string>();
        AppendPostingLineToPostingJournalDto dto = CreateAppendPostingLineToPostingJournalDto(postingReference: expectedPostingReference);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AppendPostingLineToPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), dto, cancellationTokenSource.Token);

        _commandFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<AppendPostingLineToPostingJournalRequest>(value => value.PostingReference == expectedPostingReference),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AppendPostingLineToPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnCommandFeatureWithAppendPostingLineToPostingJournalRequestWhereAccountIsEqualToDtoAccount()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();
        string expectedAccount = "2000";
        AppendPostingLineToPostingJournalDto dto = CreateAppendPostingLineToPostingJournalDto(account: expectedAccount);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AppendPostingLineToPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), dto, cancellationTokenSource.Token);

        _commandFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<AppendPostingLineToPostingJournalRequest>(value => value.Account == expectedAccount),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AppendPostingLineToPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnCommandFeatureWithAppendPostingLineToPostingJournalRequestWherePostingTextIsEqualToDtoPostingText()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();
        string expectedPostingText = "Invoice payment";
        AppendPostingLineToPostingJournalDto dto = CreateAppendPostingLineToPostingJournalDto(postingText: expectedPostingText);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AppendPostingLineToPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), dto, cancellationTokenSource.Token);

        _commandFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<AppendPostingLineToPostingJournalRequest>(value => value.PostingText == expectedPostingText),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AppendPostingLineToPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnCommandFeatureWithAppendPostingLineToPostingJournalRequestWhereBudgetAccountIsEqualToDtoBudgetAccount()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();
        string expectedBudgetAccount = "3000";
        AppendPostingLineToPostingJournalDto dto = CreateAppendPostingLineToPostingJournalDto(budgetAccount: expectedBudgetAccount);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AppendPostingLineToPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), dto, cancellationTokenSource.Token);

        _commandFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<AppendPostingLineToPostingJournalRequest>(value => value.BudgetAccount == expectedBudgetAccount),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AppendPostingLineToPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnCommandFeatureWithAppendPostingLineToPostingJournalRequestWhereDebitIsEqualToDtoDebit()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();
        decimal expectedDebit = 500.50m;
        AppendPostingLineToPostingJournalDto dto = CreateAppendPostingLineToPostingJournalDto(debit: expectedDebit);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AppendPostingLineToPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), dto, cancellationTokenSource.Token);

        _commandFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<AppendPostingLineToPostingJournalRequest>(value => value.Debit == expectedDebit),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AppendPostingLineToPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnCommandFeatureWithAppendPostingLineToPostingJournalRequestWhereCreditIsEqualToDtoCredit()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();
        decimal expectedCredit = 250.75m;
        AppendPostingLineToPostingJournalDto dto = CreateAppendPostingLineToPostingJournalDto(credit: expectedCredit);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AppendPostingLineToPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), dto, cancellationTokenSource.Token);

        _commandFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<AppendPostingLineToPostingJournalRequest>(value => value.Credit == expectedCredit),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AppendPostingLineToPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnCommandFeatureWithAppendPostingLineToPostingJournalRequestWhereContactAccountIsEqualToDtoContactAccount()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();
        string expectedContactAccount = "4000";
        AppendPostingLineToPostingJournalDto dto = CreateAppendPostingLineToPostingJournalDto(contactAccount: expectedContactAccount);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AppendPostingLineToPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), dto, cancellationTokenSource.Token);

        _commandFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<AppendPostingLineToPostingJournalRequest>(value => value.ContactAccount == expectedContactAccount),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AppendPostingLineToPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnCommandFeatureWithAppendPostingLineToPostingJournalRequestWhereSecurityContextIsEqualToSecurityContextResolvedBySecurityContextProvider()
    {
        ISecurityContext securityContext = _fixture!.CreateSecurityContext();
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut(securityContext: securityContext);
        AppendPostingLineToPostingJournalDto dto = CreateAppendPostingLineToPostingJournalDto();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AppendPostingLineToPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), dto, cancellationTokenSource.Token);

        _commandFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<AppendPostingLineToPostingJournalRequest>(value => value.SecurityContext == securityContext),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region Query Feature Invocation Tests

    [Test]
    [Category("UnitTest")]
    public async Task AppendPostingLineToPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithPostingJournalRequestWhereRequestIdIsNotEqualToGuidEmpty()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();
        AppendPostingLineToPostingJournalDto dto = CreateAppendPostingLineToPostingJournalDto();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AppendPostingLineToPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), dto, cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<PostingJournalRequest>(value => value.RequestId != Guid.Empty),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AppendPostingLineToPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithPostingJournalRequestWhereAccountingNumberIsEqualToGivenAccountingNumber()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();
        AppendPostingLineToPostingJournalDto dto = CreateAppendPostingLineToPostingJournalDto();

        int accountingNumber = _fixture!.Create<int>();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AppendPostingLineToPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, accountingNumber, dto, cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<PostingJournalRequest>(value => value.AccountingNumber == accountingNumber),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AppendPostingLineToPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithPostingJournalRequestWhereStatusDateIsEqualToLocalNowResolvedByTimeProvider()
    {
        DateTimeOffset localNow = DateTimeOffset.Now;
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut(localNow: localNow);
        AppendPostingLineToPostingJournalDto dto = CreateAppendPostingLineToPostingJournalDto();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AppendPostingLineToPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), dto, cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<PostingJournalRequest>(value => value.StatusDate == localNow.Date),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AppendPostingLineToPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithPostingJournalRequestWhereFormatProviderIsEqualToFormatProviderFromDependencies()
    {
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut(formatProvider: formatProvider);
        AppendPostingLineToPostingJournalDto dto = CreateAppendPostingLineToPostingJournalDto();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AppendPostingLineToPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), dto, cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<PostingJournalRequest>(value => value.FormatProvider == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AppendPostingLineToPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithPostingJournalRequestWhereSecurityContextIsEqualToSecurityContextResolvedBySecurityContextProvider()
    {
        ISecurityContext securityContext = _fixture!.CreateSecurityContext();
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut(securityContext: securityContext);
        AppendPostingLineToPostingJournalDto dto = CreateAppendPostingLineToPostingJournalDto();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AppendPostingLineToPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), dto, cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<PostingJournalRequest>(value => value.SecurityContext == securityContext),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AppendPostingLineToPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithGivenCancellationToken()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();
        AppendPostingLineToPostingJournalDto dto = CreateAppendPostingLineToPostingJournalDto();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.AppendPostingLineToPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), dto, cancellationToken);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.IsAny<PostingJournalRequest>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    #endregion

    #region Response Tests

    [Test]
    [Category("UnitTest")]
    public async Task AppendPostingLineToPostingJournalAsync_WhenCalled_ReturnsOkObjectResult()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();
        AppendPostingLineToPostingJournalDto dto = CreateAppendPostingLineToPostingJournalDto();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        IActionResult result = await sut.AppendPostingLineToPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), dto, cancellationTokenSource.Token);

        Assert.That(result, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task AppendPostingLineToPostingJournalAsync_WhenCalled_AssertOkObjectResultValueIsPostingJournalResponseDto()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();
        AppendPostingLineToPostingJournalDto dto = CreateAppendPostingLineToPostingJournalDto();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        IActionResult result = await sut.AppendPostingLineToPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), dto, cancellationTokenSource.Token);

        Assert.That(((OkObjectResult)result).Value, Is.TypeOf<PostingJournalResponseDto>());
    }

    #endregion

    #region Private methods

    private WebApi.Controllers.Accounting.AccountingController CreateSut(DateTimeOffset? localNow = null, IFormatProvider? formatProvider = null, PostingJournalResponse? postingJournalResponse = null, ISecurityContext? securityContext = null)
    {
        _securityContextProviderMock!.Setup(_fixture!, securityContext: securityContext);

        _timeProviderMock!.Setup(m => m.GetUtcNow())
            .Returns((localNow ?? DateTimeOffset.Now).ToUniversalTime());
        _timeProviderMock!.Setup(m => m.LocalTimeZone)
            .Returns(TimeZoneInfo.Local);

        _commandFeatureMock!.Setup(m => m.ExecuteAsync(It.IsAny<AppendPostingLineToPostingJournalRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _queryFeatureMock!.Setup(m => m.ExecuteAsync(It.IsAny<PostingJournalRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(postingJournalResponse ?? CreatePostingJournalResponse()));

        return new WebApi.Controllers.Accounting.AccountingController(_timeProviderMock!.Object, formatProvider ?? CultureInfo.InvariantCulture, _securityContextProviderMock!.Object);
    }

    private AppendPostingLineToPostingJournalDto CreateAppendPostingLineToPostingJournalDto(Guid? identifier = null, DateTimeOffset? postingDate = null, string? postingReference = null, string? account = null, string? postingText = null, string? budgetAccount = null, decimal? debit = null, decimal? credit = null, string? contactAccount = null)
    {
        return new AppendPostingLineToPostingJournalDto
        {
            Identifier = identifier ?? _fixture!.Create<Guid>(),
            PostingDate = postingDate ?? DateTimeOffset.Now,
            PostingReference = postingReference,
            Account = account ?? "1000",
            PostingText = postingText ?? "Test posting",
            BudgetAccount = budgetAccount,
            Debit = debit,
            Credit = credit,
            ContactAccount = contactAccount
        };
    }

    private PostingJournalResponse CreatePostingJournalResponse()
    {
        IReadOnlyDictionary<StaticTextKey, string> staticTexts = _fixture!.CreateStaticTexts(_random!);
        IReadOnlyCollection<IValidationRule> validationRuleSet = _fixture!.CreateValidationRuleSet();

        return new PostingJournalResponse(
            Tuple.Create(_fixture!.CreateApplyPostingJournalModel(_random!), (Predicate<int>)(_ => true)),
            _fixture!.CreatePostingJournalTexts(_random!),
            staticTexts,
            validationRuleSet);
    }

    #endregion
}