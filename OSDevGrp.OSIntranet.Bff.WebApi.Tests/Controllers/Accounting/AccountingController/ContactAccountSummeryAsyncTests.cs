using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.ContactAccountSummary;
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
public class ContactAccountSummeryAsyncTests
{
    #region Private variables

    private Mock<TimeProvider>? _timeProviderMock;
    private Mock<ISecurityContextProvider>? _securityContextProviderMock;
    private Mock<IQueryFeature<ContactAccountSummaryRequest, ContactAccountSummaryResponse>>? _queryFeatureMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _timeProviderMock = new Mock<TimeProvider>();
        _securityContextProviderMock = new Mock<ISecurityContextProvider>();
        _queryFeatureMock = new Mock<IQueryFeature<ContactAccountSummaryRequest, ContactAccountSummaryResponse>>();
        _fixture = new Fixture();
        _random = new Random(_fixture.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task ContactAccountSummeryAsync_WhenCalled_AssertGetCurrentSecurityContextAsyncWasCalledOnSecurityContextProviderWithGivenCancellationToken(bool withStatusDate)
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.ContactAccountSummeryAsync(_queryFeatureMock!.Object, _fixture!.Create<int>(), _fixture!.Create<string>(), cancellationToken, withStatusDate ? DateTimeOffset.Now.AddDays(_random!.Next(1, 7) * -1) : null);

        _securityContextProviderMock!.Verify(m => m.GetCurrentSecurityContextAsync(It.Is<CancellationToken>(value => value == cancellationToken)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task ContactAccountSummeryAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithContactAccountSummaryRequestWhereRequestIdIsNotEqualToGuidEmpty(bool withStatusDate)
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ContactAccountSummeryAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), _fixture.Create<string>(), cancellationTokenSource.Token, withStatusDate ? DateTimeOffset.Now.AddDays(_random!.Next(1, 7) * -1) : null);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<ContactAccountSummaryRequest>(value => value.RequestId != Guid.Empty),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task ContactAccountSummeryAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithContactAccountSummaryRequestWhereAccountingNumberIsEqualToGivenAccountingNumber(bool withStatusDate)
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        int accountingNumber = _fixture!.Create<int>();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ContactAccountSummeryAsync(_queryFeatureMock!.Object, accountingNumber, _fixture.Create<string>(), cancellationTokenSource.Token, withStatusDate ? DateTimeOffset.Now.AddDays(_random!.Next(1, 7) * -1) : null);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<ContactAccountSummaryRequest>(value => value.AccountingNumber == accountingNumber),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task ContactAccountSummeryAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithContactAccountSummaryRequestWhereAccountNumberIsEqualToGivenAccountNumber(bool withStatusDate)
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        string accountNumber = _fixture!.Create<string>();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ContactAccountSummeryAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), accountNumber, cancellationTokenSource.Token, withStatusDate ? DateTimeOffset.Now.AddDays(_random!.Next(1, 7) * -1) : null);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<ContactAccountSummaryRequest>(value => value.AccountNumber == accountNumber),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ContactAccountSummeryAsync_WhenStatusDateIsGiven_AssertGetUtcNowWasNotCalledOnTimeProvider()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        DateTimeOffset statusDate = DateTimeOffset.Now.AddDays(_random!.Next(1, 7) * -1);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ContactAccountSummeryAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), _fixture.Create<string>(), cancellationTokenSource.Token, statusDate);

        _timeProviderMock!.Verify(m => m.GetUtcNow(), Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ContactAccountSummeryAsync_WhenStatusDateIsGiven_AssertLocalTimeZoneWasNotCalledOnTimeProvider()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        DateTimeOffset statusDate = DateTimeOffset.Now.AddDays(_random!.Next(1, 7) * -1);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ContactAccountSummeryAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), _fixture.Create<string>(), cancellationTokenSource.Token, statusDate);

        _timeProviderMock!.Verify(m => m.LocalTimeZone, Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ContactAccountSummeryAsync_WhenStatusDateIsGiven_AssertExecuteAsyncWasCalledOnQueryFeatureWithContactAccountSummaryRequestWhereStatusDateIsEqualToGivenStatusDate()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        DateTimeOffset statusDate = DateTimeOffset.Now.AddDays(_random!.Next(1, 7) * -1);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ContactAccountSummeryAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), _fixture.Create<string>(), cancellationTokenSource.Token, statusDate);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<ContactAccountSummaryRequest>(value => value.StatusDate == statusDate.Date),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ContactAccountSummeryAsync_WhenStatusDateHasNotBeenGiven_AssertGetUtcNowWasCalledOnTimeProvider()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ContactAccountSummeryAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), _fixture.Create<string>(), cancellationTokenSource.Token);

        _timeProviderMock!.Verify(m => m.GetUtcNow(), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ContactAccountSummeryAsync_WhenStatusDateHasNotBeenGiven_AssertLocalTimeZoneWasCalledOnTimeProvider()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ContactAccountSummeryAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), _fixture.Create<string>(), cancellationTokenSource.Token);

        _timeProviderMock!.Verify(m => m.LocalTimeZone, Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ContactAccountSummeryAsync_WhenStatusDateHasNotBeenGiven_AssertExecuteAsyncWasCalledOnQueryFeatureWithContactAccountSummaryRequestWhereStatusDateIsEqualToLocalNowResolvedByTimeProvider()
    {
        DateTimeOffset localNow = DateTimeOffset.Now;
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut(localNow: localNow);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ContactAccountSummeryAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), _fixture.Create<string>(), cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<ContactAccountSummaryRequest>(value => value.StatusDate == localNow.Date),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task ContactAccountSummeryAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithContactAccountSummaryRequestWhereFormatProviderIsEqualToFormatProviderFromDependencies(bool withStatusDate)
    {
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut(formatProvider: formatProvider);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ContactAccountSummeryAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), _fixture.Create<string>(), cancellationTokenSource.Token, withStatusDate ? DateTimeOffset.Now.AddDays(_random!.Next(1, 7) * -1) : null);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<ContactAccountSummaryRequest>(value => value.FormatProvider == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task ContactAccountSummeryAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithContactAccountSummaryRequestWhereSecurityContextIsEqualToSecurityResolvedBySecurityContextProvider(bool withStatusDate)
    {
        ISecurityContext securityContext = _fixture!.CreateSecurityContext();
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut(securityContext: securityContext);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ContactAccountSummeryAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), _fixture.Create<string>(), cancellationTokenSource.Token, withStatusDate ? DateTimeOffset.Now.AddDays(_random!.Next(1, 7) * -1) : null);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<ContactAccountSummaryRequest>(value => value.SecurityContext == securityContext),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task ContactAccountSummeryAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithGivenCancellationToken(bool withStatusDate)
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.ContactAccountSummeryAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), _fixture.Create<string>(), cancellationToken, withStatusDate ? DateTimeOffset.Now.AddDays(_random!.Next(1, 7) * -1) : null);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.IsAny<ContactAccountSummaryRequest>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task ContactAccountSummeryAsync_WhenCalled_ReturnsOkObjectResult(bool withStatusDate)
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        IActionResult result = await sut.ContactAccountSummeryAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), _fixture.Create<string>(), cancellationTokenSource.Token, withStatusDate ? DateTimeOffset.Now.AddDays(_random!.Next(1, 7) * -1) : null);

        Assert.That(result, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task ContactAccountSummeryAsync_WhenCalled_ReturnsOkObjectResultWhereValueIsContactAccountSummaryResponseDto(bool withStatusDate)
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        OkObjectResult result = (OkObjectResult)await sut.ContactAccountSummeryAsync(_queryFeatureMock!.Object, _fixture.Create<int>(), _fixture.Create<string>(), cancellationTokenSource.Token, withStatusDate ? DateTimeOffset.Now.AddDays(_random!.Next(1, 7) * -1) : null);

        Assert.That(result.Value, Is.TypeOf<ContactAccountSummaryResponseDto>());
    }

    private WebApi.Controllers.Accounting.AccountingController CreateSut(DateTimeOffset? localNow = null, IFormatProvider? formatProvider = null, ContactAccountSummaryResponse? contactAccountSummaryResponse = null, ISecurityContext? securityContext = null)
    {
        _securityContextProviderMock!.Setup(_fixture!, securityContext: securityContext);

        _timeProviderMock!.Setup(m => m.GetUtcNow())
            .Returns((localNow ?? DateTimeOffset.Now).ToUniversalTime);
        _timeProviderMock!.Setup(m => m.LocalTimeZone)
            .Returns(TimeZoneInfo.Local);

        _queryFeatureMock!.Setup(m => m.ExecuteAsync(It.IsAny<ContactAccountSummaryRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(contactAccountSummaryResponse ?? CreateContactAccountSummaryResponse()));

        return new WebApi.Controllers.Accounting.AccountingController(_timeProviderMock!.Object, formatProvider ?? CultureInfo.InvariantCulture, _securityContextProviderMock!.Object);
    }

    private ContactAccountSummaryResponse CreateContactAccountSummaryResponse(ContactAccountModel? contactAccountModel = null, IContactAccountTexts? contactAccountTexts = null)
    {
        IReadOnlyDictionary<StaticTextKey, string> staticTexts = _fixture!.CreateStaticTexts(_random!);
        IReadOnlyCollection<IValidationRule> validationRuleSet = _fixture!.CreateEmptyValidationRuleSet();

        return new ContactAccountSummaryResponse(
            contactAccountModel ?? _fixture!.CreateContactAccountModel(_random!),
            contactAccountTexts ?? _fixture!.CreateContactAccountTexts(_random!),
            staticTexts,
            validationRuleSet);
    }
}