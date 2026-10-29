using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.AccountSummary;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData;
using OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;
using OSDevGrp.OSIntranet.Bff.WebApi.Tests.Controllers.Accounting.Dtos;
using OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security.SecurityContextProvider;
using OSDevGrp.OSIntranet.Bff.WebApi.Tests.Shared.Dtos;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Controllers.Accounting.AccountingController;

[TestFixture]
public class AccountSummeryAsyncTests
{
    #region Private variables

    private Mock<TimeProvider>? _timeProviderMock;
    private Mock<ISecurityContextProvider>? _securityContextProviderMock;
    private Mock<IQueryFeature<AccountSummaryRequest, AccountSummaryResponse>>? _queryFeatureMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _timeProviderMock = new Mock<TimeProvider>();
        _securityContextProviderMock = new Mock<ISecurityContextProvider>();
        _queryFeatureMock = new Mock<IQueryFeature<AccountSummaryRequest, AccountSummaryResponse>>();
        _fixture = new Fixture();
        _random = new Random(_fixture.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task AccountSummeryAsync_WhenCalled_AssertGetCurrentSecurityContextAsyncWasCalledOnSecurityContextProviderWithGivenCancellationToken(bool withStatusDate)
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.AccountSummeryAsync(_queryFeatureMock!.Object, _fixture!.Create<int>(), _fixture!.Create<string>(), cancellationToken, withStatusDate ? DateTimeOffset.Now.AddDays(_random!.Next(1, 7) * -1) : null);

        _securityContextProviderMock!.Verify(m => m.GetCurrentSecurityContextAsync(It.Is<CancellationToken>(value => value == cancellationToken)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task AccountSummeryAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithAccountSummaryRequestWhereRequestIdIsNotEqualToGuidEmpty(bool withStatusDate)
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AccountSummeryAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), _fixture.Create<string>(), cancellationTokenSource.Token, withStatusDate ? DateTimeOffset.Now.AddDays(_random!.Next(1, 7) * -1) : null);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<AccountSummaryRequest>(value => value.RequestId != Guid.Empty),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task AccountSummeryAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithAccountSummaryRequestWhereAccountingNumberIsEqualToGivenAccountingNumber(bool withStatusDate)
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        int accountingNumber = _fixture!.Create<int>();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AccountSummeryAsync(_queryFeatureMock!.Object, accountingNumber, _fixture.Create<string>(), cancellationTokenSource.Token, withStatusDate ? DateTimeOffset.Now.AddDays(_random!.Next(1, 7) * -1) : null);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<AccountSummaryRequest>(value => value.AccountingNumber == accountingNumber),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task AccountSummeryAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithAccountSummaryRequestWhereAccountNumberIsEqualToGivenAccountNumber(bool withStatusDate)
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        string accountNumber = _fixture!.Create<string>();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AccountSummeryAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), accountNumber, cancellationTokenSource.Token, withStatusDate ? DateTimeOffset.Now.AddDays(_random!.Next(1, 7) * -1) : null);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<AccountSummaryRequest>(value => value.AccountNumber == accountNumber),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AccountSummeryAsync_WhenStatusDateIsGiven_AssertGetUtcNowWasNotCalledOnTimeProvider()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        DateTimeOffset statusDate = DateTimeOffset.Now.AddDays(_random!.Next(1, 7) * -1);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AccountSummeryAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), _fixture.Create<string>(), cancellationTokenSource.Token, statusDate);

        _timeProviderMock!.Verify(m => m.GetUtcNow(), Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AccountSummeryAsync_WhenStatusDateIsGiven_AssertLocalTimeZoneWasNotCalledOnTimeProvider()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        DateTimeOffset statusDate = DateTimeOffset.Now.AddDays(_random!.Next(1, 7) * -1);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AccountSummeryAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), _fixture.Create<string>(), cancellationTokenSource.Token, statusDate);

        _timeProviderMock!.Verify(m => m.LocalTimeZone, Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AccountSummeryAsync_WhenStatusDateIsGiven_AssertExecuteAsyncWasCalledOnQueryFeatureWithAccountSummaryRequestWhereStatusDateIsEqualToGivenStatusDate()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        DateTimeOffset statusDate = DateTimeOffset.Now.AddDays(_random!.Next(1, 7) * -1);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AccountSummeryAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), _fixture.Create<string>(), cancellationTokenSource.Token, statusDate);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<AccountSummaryRequest>(value => value.StatusDate == statusDate.Date),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AccountSummeryAsync_WhenStatusDateHasNotBeenGiven_AssertGetUtcNowWasCalledOnTimeProvider()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AccountSummeryAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), _fixture.Create<string>(), cancellationTokenSource.Token);

        _timeProviderMock!.Verify(m => m.GetUtcNow(), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AccountSummeryAsync_WhenStatusDateHasNotBeenGiven_AssertLocalTimeZoneWasCalledOnTimeProvider()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AccountSummeryAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), _fixture.Create<string>(), cancellationTokenSource.Token);

        _timeProviderMock!.Verify(m => m.LocalTimeZone, Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AccountSummeryAsync_WhenStatusDateHasNotBeenGiven_AssertExecuteAsyncWasCalledOnQueryFeatureWithAccountSummaryRequestWhereStatusDateIsEqualToLocalNowResolvedByTimeProvider()
    {
        DateTimeOffset localNow = DateTimeOffset.Now;
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut(localNow: localNow);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AccountSummeryAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), _fixture.Create<string>(), cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<AccountSummaryRequest>(value => value.StatusDate == localNow.Date),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task AccountSummeryAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithAccountSummaryRequestWhereFormatProviderIsEqualToFormatProviderFromDependencies(bool withStatusDate)
    {
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut(formatProvider: formatProvider);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AccountSummeryAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), _fixture.Create<string>(), cancellationTokenSource.Token, withStatusDate ? DateTimeOffset.Now.AddDays(_random!.Next(1, 7) * -1) : null);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<AccountSummaryRequest>(value => value.FormatProvider == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task AccountSummeryAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithAccountSummaryRequestWhereSecurityContextIsEqualToSecurityResolvedBySecurityContextProvider(bool withStatusDate)
    {
        ISecurityContext securityContext = _fixture!.CreateSecurityContext();
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut(securityContext: securityContext);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AccountSummeryAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), _fixture.Create<string>(), cancellationTokenSource.Token, withStatusDate ? DateTimeOffset.Now.AddDays(_random!.Next(1, 7) * -1) : null);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<AccountSummaryRequest>(value => value.SecurityContext == securityContext),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task AccountSummeryAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithGivenCancellationToken(bool withStatusDate)
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.AccountSummeryAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), _fixture.Create<string>(), cancellationToken, withStatusDate ? DateTimeOffset.Now.AddDays(_random!.Next(1, 7) * -1) : null);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.IsAny<AccountSummaryRequest>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task AccountSummeryAsync_WhenCalled_ReturnsOkObjectResult(bool withStatusDate)
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        IActionResult result = await sut.AccountSummeryAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), _fixture.Create<string>(), cancellationTokenSource.Token, withStatusDate ? DateTimeOffset.Now.AddDays(_random!.Next(1, 7) * -1) : null);

        Assert.That(result, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task AccountSummeryAsync_WhenCalled_ReturnsOkObjectResultWhereValueIsAccountSummaryResponseDto(bool withStatusDate)
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        OkObjectResult result = (OkObjectResult)await sut.AccountSummeryAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), _fixture.Create<string>(), cancellationTokenSource.Token, withStatusDate ? DateTimeOffset.Now.AddDays(_random!.Next(1, 7) * -1) : null);

        Assert.That(result.Value, Is.TypeOf<AccountSummaryResponseDto>());
    }

    private WebApi.Controllers.Accounting.AccountingController CreateSut(DateTimeOffset? localNow = null, IFormatProvider? formatProvider = null, AccountSummaryResponse? accountSummaryResponse = null, ISecurityContext? securityContext = null)
    {
        _securityContextProviderMock!.Setup(_fixture!, securityContext: securityContext);

        _timeProviderMock!.Setup(m => m.GetUtcNow())
            .Returns((localNow ?? DateTimeOffset.Now).ToUniversalTime);
        _timeProviderMock!.Setup(m => m.LocalTimeZone)
            .Returns(TimeZoneInfo.Local);

        _queryFeatureMock!.Setup(m => m.ExecuteAsync(It.IsAny<AccountSummaryRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(accountSummaryResponse ?? CreateAccountSummaryResponse()));

        return new WebApi.Controllers.Accounting.AccountingController(_timeProviderMock!.Object, formatProvider ?? CultureInfo.InvariantCulture, _securityContextProviderMock!.Object);
    }

    private AccountSummaryResponse CreateAccountSummaryResponse(AccountModel? accountModel = null, IAccountTexts? accountTexts = null)
    {
        IReadOnlyDictionary<StaticTextKey, string> staticTexts = _fixture!.CreateStaticTexts(_random!);
        IReadOnlyCollection<IValidationRule> validationRuleSet = _fixture!.CreateEmptyValidationRuleSet();

        return new AccountSummaryResponse(
            accountModel ?? _fixture!.CreateAccountModel(_random!),
            accountTexts ?? _fixture!.CreateAccountTexts(_random!),
            staticTexts,
            validationRuleSet);
    }
}