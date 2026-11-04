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
public class ModifyPostingLineInPostingJournalAsyncTests
{
    #region Private variables

    private Mock<TimeProvider>? _timeProviderMock;
    private Mock<ISecurityContextProvider>? _securityContextProviderMock;
    private Mock<ICommandFeature<ModifyPostingLineInPostingJournalRequest>>? _commandFeatureMock;
    private Mock<IQueryFeature<PostingJournalRequest, PostingJournalResponse>>? _queryFeatureMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _timeProviderMock = new Mock<TimeProvider>();
        _securityContextProviderMock = new Mock<ISecurityContextProvider>();
        _commandFeatureMock = new Mock<ICommandFeature<ModifyPostingLineInPostingJournalRequest>>();
        _queryFeatureMock = new Mock<IQueryFeature<PostingJournalRequest, PostingJournalResponse>>();
        _fixture = new Fixture();
        _random = new Random(_fixture.Create<int>());
    }

    #region Command Feature Invocation Tests

    [Test]
    [Category("UnitTest")]
    public async Task ModifyPostingLineInPostingJournalAsync_WhenCalled_AssertGetCurrentSecurityContextAsyncWasCalledOnSecurityContextProviderWithGivenCancellationToken()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();
        ModifyPostingLineInPostingJournalDto dto = CreateModifyPostingLineInPostingJournalDto();
        Guid identifier = _fixture!.Create<Guid>();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.ModifyPostingLineInPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), identifier, dto, cancellationToken);

        _securityContextProviderMock!.Verify(m => m.GetCurrentSecurityContextAsync(It.Is<CancellationToken>(value => value == cancellationToken)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ModifyPostingLineInPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnCommandFeatureWithModifyPostingLineInPostingJournalRequestWhereRequestIdIsNotEqualToGuidEmpty()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();
        ModifyPostingLineInPostingJournalDto dto = CreateModifyPostingLineInPostingJournalDto();
        Guid identifier = _fixture!.Create<Guid>();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ModifyPostingLineInPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), identifier, dto, cancellationTokenSource.Token);

        _commandFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<ModifyPostingLineInPostingJournalRequest>(value => value.RequestId != Guid.Empty),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ModifyPostingLineInPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnCommandFeatureWithModifyPostingLineInPostingJournalRequestWhereAccountingNumberIsEqualToGivenAccountingNumber()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();
        ModifyPostingLineInPostingJournalDto dto = CreateModifyPostingLineInPostingJournalDto();
        Guid identifier = _fixture!.Create<Guid>();

        int accountingNumber = _fixture!.Create<int>();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ModifyPostingLineInPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, accountingNumber, identifier, dto, cancellationTokenSource.Token);

        _commandFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<ModifyPostingLineInPostingJournalRequest>(value => value.AccountingNumber == accountingNumber),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ModifyPostingLineInPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnCommandFeatureWithModifyPostingLineInPostingJournalRequestWhereIdentifierIsEqualToRouteIdentifier()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();
        ModifyPostingLineInPostingJournalDto dto = CreateModifyPostingLineInPostingJournalDto();
        Guid expectedIdentifier = _fixture!.Create<Guid>();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ModifyPostingLineInPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), expectedIdentifier, dto, cancellationTokenSource.Token);

        _commandFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<ModifyPostingLineInPostingJournalRequest>(value => value.Identifier == expectedIdentifier),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ModifyPostingLineInPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnCommandFeatureWithModifyPostingLineInPostingJournalRequestWherePostingDateIsEqualToDtoPostingDate()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();
        DateTimeOffset expectedPostingDate = DateTimeOffset.Now;
        ModifyPostingLineInPostingJournalDto dto = CreateModifyPostingLineInPostingJournalDto(postingDate: expectedPostingDate);
        Guid identifier = _fixture!.Create<Guid>();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ModifyPostingLineInPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), identifier, dto, cancellationTokenSource.Token);

        _commandFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<ModifyPostingLineInPostingJournalRequest>(value => value.PostingDate == expectedPostingDate),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ModifyPostingLineInPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnCommandFeatureWithModifyPostingLineInPostingJournalRequestWherePostingReferenceIsEqualToDtoPostingReference()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();
        string expectedPostingReference = _fixture!.Create<string>();
        ModifyPostingLineInPostingJournalDto dto = CreateModifyPostingLineInPostingJournalDto(postingReference: expectedPostingReference);
        Guid identifier = _fixture!.Create<Guid>();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ModifyPostingLineInPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), identifier, dto, cancellationTokenSource.Token);

        _commandFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<ModifyPostingLineInPostingJournalRequest>(value => value.PostingReference == expectedPostingReference),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ModifyPostingLineInPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnCommandFeatureWithModifyPostingLineInPostingJournalRequestWhereAccountIsEqualToDtoAccount()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();
        string expectedAccount = "2000";
        ModifyPostingLineInPostingJournalDto dto = CreateModifyPostingLineInPostingJournalDto(account: expectedAccount);
        Guid identifier = _fixture!.Create<Guid>();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ModifyPostingLineInPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), identifier, dto, cancellationTokenSource.Token);

        _commandFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<ModifyPostingLineInPostingJournalRequest>(value => value.Account == expectedAccount),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ModifyPostingLineInPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnCommandFeatureWithModifyPostingLineInPostingJournalRequestWherePostingTextIsEqualToDtoPostingText()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();
        string expectedPostingText = "Invoice payment";
        ModifyPostingLineInPostingJournalDto dto = CreateModifyPostingLineInPostingJournalDto(postingText: expectedPostingText);
        Guid identifier = _fixture!.Create<Guid>();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ModifyPostingLineInPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), identifier, dto, cancellationTokenSource.Token);

        _commandFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<ModifyPostingLineInPostingJournalRequest>(value => value.PostingText == expectedPostingText),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ModifyPostingLineInPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnCommandFeatureWithModifyPostingLineInPostingJournalRequestWhereBudgetAccountIsEqualToDtoBudgetAccount()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();
        string expectedBudgetAccount = "3000";
        ModifyPostingLineInPostingJournalDto dto = CreateModifyPostingLineInPostingJournalDto(budgetAccount: expectedBudgetAccount);
        Guid identifier = _fixture!.Create<Guid>();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ModifyPostingLineInPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), identifier, dto, cancellationTokenSource.Token);

        _commandFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<ModifyPostingLineInPostingJournalRequest>(value => value.BudgetAccount == expectedBudgetAccount),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ModifyPostingLineInPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnCommandFeatureWithModifyPostingLineInPostingJournalRequestWhereDebitIsEqualToDtoDebit()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();
        decimal expectedDebit = 500.50m;
        ModifyPostingLineInPostingJournalDto dto = CreateModifyPostingLineInPostingJournalDto(debit: expectedDebit);
        Guid identifier = _fixture!.Create<Guid>();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ModifyPostingLineInPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), identifier, dto, cancellationTokenSource.Token);

        _commandFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<ModifyPostingLineInPostingJournalRequest>(value => value.Debit == expectedDebit),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ModifyPostingLineInPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnCommandFeatureWithModifyPostingLineInPostingJournalRequestWhereCreditIsEqualToDtoCredit()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();
        decimal expectedCredit = 250.75m;
        ModifyPostingLineInPostingJournalDto dto = CreateModifyPostingLineInPostingJournalDto(credit: expectedCredit);
        Guid identifier = _fixture!.Create<Guid>();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ModifyPostingLineInPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), identifier, dto, cancellationTokenSource.Token);

        _commandFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<ModifyPostingLineInPostingJournalRequest>(value => value.Credit == expectedCredit),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ModifyPostingLineInPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnCommandFeatureWithModifyPostingLineInPostingJournalRequestWhereContactAccountIsEqualToDtoContactAccount()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();
        string expectedContactAccount = "4000";
        ModifyPostingLineInPostingJournalDto dto = CreateModifyPostingLineInPostingJournalDto(contactAccount: expectedContactAccount);
        Guid identifier = _fixture!.Create<Guid>();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ModifyPostingLineInPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), identifier, dto, cancellationTokenSource.Token);

        _commandFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<ModifyPostingLineInPostingJournalRequest>(value => value.ContactAccount == expectedContactAccount),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ModifyPostingLineInPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnCommandFeatureWithModifyPostingLineInPostingJournalRequestWhereSecurityContextIsEqualToSecurityContextResolvedBySecurityContextProvider()
    {
        ISecurityContext securityContext = _fixture!.CreateSecurityContext();
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut(securityContext: securityContext);
        ModifyPostingLineInPostingJournalDto dto = CreateModifyPostingLineInPostingJournalDto();
        Guid identifier = _fixture!.Create<Guid>();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ModifyPostingLineInPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), identifier, dto, cancellationTokenSource.Token);

        _commandFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<ModifyPostingLineInPostingJournalRequest>(value => value.SecurityContext == securityContext),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ModifyPostingLineInPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnCommandFeatureWithModifyPostingLineInPostingJournalRequestWhereFormatProviderIsEqualToFormatProviderFromDependencies()
    {
        IFormatProvider formatProvider = _fixture!.Create<CultureInfo>();
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut(formatProvider: formatProvider);
        ModifyPostingLineInPostingJournalDto dto = CreateModifyPostingLineInPostingJournalDto();
        Guid identifier = _fixture!.Create<Guid>();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ModifyPostingLineInPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), identifier, dto, cancellationTokenSource.Token);

        _commandFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<ModifyPostingLineInPostingJournalRequest>(value => value.FormatProvider == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region Query Feature Invocation Tests

    [Test]
    [Category("UnitTest")]
    public async Task ModifyPostingLineInPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithPostingJournalRequestWhereRequestIdIsNotEqualToGuidEmpty()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();
        ModifyPostingLineInPostingJournalDto dto = CreateModifyPostingLineInPostingJournalDto();
        Guid identifier = _fixture!.Create<Guid>();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ModifyPostingLineInPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), identifier, dto, cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<PostingJournalRequest>(value => value.RequestId != Guid.Empty),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ModifyPostingLineInPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithPostingJournalRequestWhereAccountingNumberIsEqualToGivenAccountingNumber()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();
        ModifyPostingLineInPostingJournalDto dto = CreateModifyPostingLineInPostingJournalDto();
        Guid identifier = _fixture!.Create<Guid>();

        int accountingNumber = _fixture!.Create<int>();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ModifyPostingLineInPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, accountingNumber, identifier, dto, cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<PostingJournalRequest>(value => value.AccountingNumber == accountingNumber),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ModifyPostingLineInPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithPostingJournalRequestWhereStatusDateIsEqualToLocalNowResolvedByTimeProvider()
    {
        DateTimeOffset localNow = DateTimeOffset.Now;
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut(localNow: localNow);
        ModifyPostingLineInPostingJournalDto dto = CreateModifyPostingLineInPostingJournalDto();
        Guid identifier = _fixture!.Create<Guid>();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ModifyPostingLineInPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), identifier, dto, cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<PostingJournalRequest>(value => value.StatusDate == localNow.Date),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ModifyPostingLineInPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithPostingJournalRequestWhereFormatProviderIsEqualToFormatProviderFromDependencies()
    {
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut(formatProvider: formatProvider);
        ModifyPostingLineInPostingJournalDto dto = CreateModifyPostingLineInPostingJournalDto();
        Guid identifier = _fixture!.Create<Guid>();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ModifyPostingLineInPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), identifier, dto, cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<PostingJournalRequest>(value => value.FormatProvider == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ModifyPostingLineInPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithPostingJournalRequestWhereSecurityContextIsEqualToSecurityContextResolvedBySecurityContextProvider()
    {
        ISecurityContext securityContext = _fixture!.CreateSecurityContext();
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut(securityContext: securityContext);
        ModifyPostingLineInPostingJournalDto dto = CreateModifyPostingLineInPostingJournalDto();
        Guid identifier = _fixture!.Create<Guid>();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ModifyPostingLineInPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), identifier, dto, cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<PostingJournalRequest>(value => value.SecurityContext == securityContext),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ModifyPostingLineInPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithGivenCancellationToken()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();
        ModifyPostingLineInPostingJournalDto dto = CreateModifyPostingLineInPostingJournalDto();
        Guid identifier = _fixture!.Create<Guid>();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.ModifyPostingLineInPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), identifier, dto, cancellationToken);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.IsAny<PostingJournalRequest>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    #endregion

    #region Response Tests

    [Test]
    [Category("UnitTest")]
    public async Task ModifyPostingLineInPostingJournalAsync_WhenCalled_ReturnsOkObjectResult()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();
        ModifyPostingLineInPostingJournalDto dto = CreateModifyPostingLineInPostingJournalDto();
        Guid identifier = _fixture!.Create<Guid>();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        IActionResult result = await sut.ModifyPostingLineInPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), identifier, dto, cancellationTokenSource.Token);

        Assert.That(result, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task ModifyPostingLineInPostingJournalAsync_WhenCalled_AssertOkObjectResultValueIsPostingJournalResponseDto()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();
        ModifyPostingLineInPostingJournalDto dto = CreateModifyPostingLineInPostingJournalDto();
        Guid identifier = _fixture!.Create<Guid>();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        IActionResult result = await sut.ModifyPostingLineInPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), identifier, dto, cancellationTokenSource.Token);

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

        _commandFeatureMock!.Setup(m => m.ExecuteAsync(It.IsAny<ModifyPostingLineInPostingJournalRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _queryFeatureMock!.Setup(m => m.ExecuteAsync(It.IsAny<PostingJournalRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(postingJournalResponse ?? CreatePostingJournalResponse()));

        return new WebApi.Controllers.Accounting.AccountingController(_timeProviderMock!.Object, formatProvider ?? CultureInfo.InvariantCulture, _securityContextProviderMock!.Object);
    }

    private ModifyPostingLineInPostingJournalDto CreateModifyPostingLineInPostingJournalDto(DateTimeOffset? postingDate = null, string? postingReference = null, string? account = null, string? postingText = null, string? budgetAccount = null, decimal? debit = null, decimal? credit = null, string? contactAccount = null)
    {
        return new ModifyPostingLineInPostingJournalDto
        {
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