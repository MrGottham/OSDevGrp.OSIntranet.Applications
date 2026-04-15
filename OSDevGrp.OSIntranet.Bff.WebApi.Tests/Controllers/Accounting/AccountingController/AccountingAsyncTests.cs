using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.Accounting;
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
public class AccountingAsyncTests
{
    #region Private variables

    private Mock<TimeProvider>? _timeProviderMock;
    private Mock<ISecurityContextProvider>? _securityContextProviderMock;
    private Mock<IQueryFeature<AccountingRequest, AccountingResponse>>? _queryFeatureMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _timeProviderMock = new Mock<TimeProvider>();
        _securityContextProviderMock = new Mock<ISecurityContextProvider>();
        _queryFeatureMock = new Mock<IQueryFeature<AccountingRequest, AccountingResponse>>();
        _fixture = new Fixture();
        _random = new Random(_fixture.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task AccountingAsync_WhenCalled_AssertGetCurrentSecurityContextAsyncWasCalledOnSecurityContextProviderWithGivenCancellationToken(bool withStatusDate)
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.AccountingAsync(_queryFeatureMock!.Object, _fixture!.Create<int>(), cancellationToken, withStatusDate ? DateTimeOffset.Now.AddDays(_random!.Next(1, 7) * -1) : null);

        _securityContextProviderMock!.Verify(m => m.GetCurrentSecurityContextAsync(It.Is<CancellationToken>(value => value == cancellationToken)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task AccountingAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithAccountingRequestWhereRequestIdIsNotEqualToGuidEmpty(bool withStatusDate)
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AccountingAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), cancellationTokenSource.Token, withStatusDate ? DateTimeOffset.Now.AddDays(_random!.Next(1, 7) * -1) : null);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<AccountingRequest>(value => value.RequestId != Guid.Empty),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task AccountingAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithAccountingRequestWhereAccountingNumberIsEqualToGivenAccountingNumber(bool withStatusDate)
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        int accountingNumber = _fixture!.Create<int>();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AccountingAsync(_queryFeatureMock!.Object, accountingNumber, cancellationTokenSource.Token, withStatusDate ? DateTimeOffset.Now.AddDays(_random!.Next(1, 7) * -1) : null);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<AccountingRequest>(value => value.AccountingNumber == accountingNumber),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AccountingAsync_WhenStatusDateIsGiven_AssertGetUtcNowWasNotCalledOnTimeProvider()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        DateTimeOffset statusDate = DateTimeOffset.Now.AddDays(_random!.Next(1, 7) * -1);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AccountingAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), cancellationTokenSource.Token, statusDate);

        _timeProviderMock!.Verify(m => m.GetUtcNow(), Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AccountingAsync_WhenStatusDateIsGiven_AssertLocalTimeZoneWasNotCalledOnTimeProvider()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        DateTimeOffset statusDate = DateTimeOffset.Now.AddDays(_random!.Next(1, 7) * -1);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AccountingAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), cancellationTokenSource.Token, statusDate);

        _timeProviderMock!.Verify(m => m.LocalTimeZone, Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AccountingAsync_WhenStatusDateIsGiven_AssertExecuteAsyncWasCalledOnQueryFeatureWithAccountingRequestWhereStatusDateIsEqualToGivenStatusDate()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        DateTimeOffset statusDate = DateTimeOffset.Now.AddDays(_random!.Next(1, 7) * -1);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AccountingAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), cancellationTokenSource.Token, statusDate);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<AccountingRequest>(value => value.StatusDate == statusDate.Date),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AccountingAsync_WhenStatusDateHasNotBeenGiven_AssertGetUtcNowWasCalledOnTimeProvider()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AccountingAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), cancellationTokenSource.Token);

        _timeProviderMock!.Verify(m => m.GetUtcNow(), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AccountingAsync_WhenStatusDateHasNotBeenGiven_AssertLocalTimeZoneWasCalledOnTimeProvider()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AccountingAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), cancellationTokenSource.Token);

        _timeProviderMock!.Verify(m => m.LocalTimeZone, Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task AccountingAsync_WhenStatusDateHasNotBeenGiven_AssertExecuteAsyncWasCalledOnQueryFeatureWithAccountingRequestWhereStatusDateIsEqualToLocalNowResolvedByTimeProvider()
    {
        DateTimeOffset localNow = DateTimeOffset.Now;
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut(localNow: localNow);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AccountingAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<AccountingRequest>(value => value.StatusDate == localNow.Date),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task AccountingAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithAccountingRequestWhereFormatProviderIsEqualToFormatProviderFromDependencies(bool withStatusDate)
    {
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut(formatProvider: formatProvider);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AccountingAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), cancellationTokenSource.Token, withStatusDate ? DateTimeOffset.Now.AddDays(_random!.Next(1, 7) * -1) : null);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<AccountingRequest>(value => value.FormatProvider == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task AccountingAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithAccountingRequestWhereSecurityContextIsEqualToSecurityResolvedBySecurityContextProvider(bool withStatusDate)
    {
        ISecurityContext securityContext = _fixture!.CreateSecurityContext();
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut(securityContext: securityContext);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.AccountingAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), cancellationTokenSource.Token, withStatusDate ? DateTimeOffset.Now.AddDays(_random!.Next(1, 7) * -1) : null);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<AccountingRequest>(value => value.SecurityContext == securityContext),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task AccountingAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithGivenCancellationToken(bool withStatusDate)
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.AccountingAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), cancellationToken, withStatusDate ? DateTimeOffset.Now.AddDays(_random!.Next(1, 7) * -1) : null);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.IsAny<AccountingRequest>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task AccountingAsync_WhenCalled_ReturnsOkObjectResult(bool withStatusDate)
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        IActionResult result = await sut.AccountingAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), cancellationTokenSource.Token, withStatusDate ? DateTimeOffset.Now.AddDays(_random!.Next(1, 7) * -1) : null);

        Assert.That(result, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task AccountingAsync_WhenCalled_ReturnsOkObjectResultWhereValueIsAccountingResponseDto(bool withStatusDate)
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        OkObjectResult result = (OkObjectResult)await sut.AccountingAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), cancellationTokenSource.Token, withStatusDate ? DateTimeOffset.Now.AddDays(_random!.Next(1, 7) * -1) : null);

        Assert.That(result.Value, Is.TypeOf<AccountingResponseDto>());
    }

    private WebApi.Controllers.Accounting.AccountingController CreateSut(DateTimeOffset? localNow = null, IFormatProvider? formatProvider = null, AccountingResponse? accountingResponse = null, ISecurityContext? securityContext = null)
    {
        _securityContextProviderMock!.Setup(_fixture!, securityContext: securityContext);

        _timeProviderMock!.Setup(m => m.GetUtcNow())
            .Returns((localNow ?? DateTimeOffset.Now).ToUniversalTime);
        _timeProviderMock!.Setup(m => m.LocalTimeZone)
            .Returns(TimeZoneInfo.Local);

        _queryFeatureMock!.Setup(m => m.ExecuteAsync(It.IsAny<AccountingRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(accountingResponse ?? CreateAccountingResponse()));

        return new WebApi.Controllers.Accounting.AccountingController(_timeProviderMock!.Object, formatProvider ?? CultureInfo.InvariantCulture, _securityContextProviderMock!.Object);
    }

    private AccountingResponse CreateAccountingResponse(AccountingModel? accountingModel = null, IAccountingTexts? accountingTexts = null, IReadOnlyCollection<LetterHeadIdentificationModel>? letterHeads = null)
    {
        IReadOnlyDictionary<StaticTextKey, string> staticTexts = _fixture!.CreateStaticTexts(_random!);
        IReadOnlyCollection<IValidationRule> validationRuleSet = _fixture!.CreateValidationRuleSet();

        return new AccountingResponse(accountingModel ?? _fixture!.CreateAccountingModel(_random!), accountingTexts ?? _fixture!.CreateAccountingTexts(_random!), letterHeads ?? _fixture!.CreateLetterHeadIdentificationModels(_random!), staticTexts, validationRuleSet);
    }
}