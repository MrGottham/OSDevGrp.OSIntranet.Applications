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
public class DeletePostingLineFromPostingJournalAsyncTests
{
    #region Private variables

    private Mock<TimeProvider>? _timeProviderMock;
    private Mock<ISecurityContextProvider>? _securityContextProviderMock;
    private Mock<ICommandFeature<DeletePostingLineFromPostingJournalRequest>>? _commandFeatureMock;
    private Mock<IQueryFeature<PostingJournalRequest, PostingJournalResponse>>? _queryFeatureMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _timeProviderMock = new Mock<TimeProvider>();
        _securityContextProviderMock = new Mock<ISecurityContextProvider>();
        _commandFeatureMock = new Mock<ICommandFeature<DeletePostingLineFromPostingJournalRequest>>();
        _queryFeatureMock = new Mock<IQueryFeature<PostingJournalRequest, PostingJournalResponse>>();
        _fixture = new Fixture();
        _random = new Random(_fixture.Create<int>());
    }

    #region Command Feature Invocation Tests

    [Test]
    [Category("UnitTest")]
    public async Task DeletePostingLineFromPostingJournalAsync_WhenCalled_ResolvesSecurityContextOnceWithGivenCancellationToken()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        await sut.DeletePostingLineFromPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), _fixture!.Create<Guid>(), _fixture!.Create<string>(), _fixture!.Create<string>(), cancellationToken);

        _securityContextProviderMock!.Verify(m => m.GetCurrentSecurityContextAsync(It.Is<CancellationToken>(value => value == cancellationToken)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task DeletePostingLineFromPostingJournalAsync_WhenCalled_ExecutesCommandWithExpectedRequestAndCancellationToken()
    {
        int accountingNumber = _fixture!.Create<int>();
        Guid identifier = _fixture!.Create<Guid>();
        string verificationKey = _fixture!.Create<string>();
        string verificationCode = _fixture!.Create<string>();
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        ISecurityContext securityContext = _fixture!.CreateSecurityContext();
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut(formatProvider: formatProvider, securityContext: securityContext);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        await sut.DeletePostingLineFromPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, accountingNumber, identifier, verificationKey, verificationCode, cancellationToken);

        _commandFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<DeletePostingLineFromPostingJournalRequest>(value =>
                    value.RequestId != Guid.Empty &&
                    value.AccountingNumber == accountingNumber &&
                    value.Identifier == identifier &&
                    value.VerificationKey == verificationKey &&
                    value.VerificationCode == verificationCode &&
                    value.FormatProvider == formatProvider &&
                    value.SecurityContext == securityContext),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    #endregion

    #region Query Feature Invocation Tests

    [Test]
    [Category("UnitTest")]
    public async Task DeletePostingLineFromPostingJournalAsync_WhenCalled_QueriesRefreshedJournalWithExpectedRequestAndCancellationToken()
    {
        DateTimeOffset localNow = DateTimeOffset.Now;
        int accountingNumber = _fixture!.Create<int>();
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        ISecurityContext securityContext = _fixture!.CreateSecurityContext();
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut(localNow: localNow, formatProvider: formatProvider, securityContext: securityContext);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        await sut.DeletePostingLineFromPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, accountingNumber, _fixture!.Create<Guid>(), _fixture!.Create<string>(), _fixture!.Create<string>(), cancellationToken);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<PostingJournalRequest>(value =>
                    value.RequestId != Guid.Empty &&
                    value.AccountingNumber == accountingNumber &&
                    value.StatusDate == localNow.Date &&
                    value.FormatProvider == formatProvider &&
                    value.SecurityContext == securityContext),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    #endregion

    #region Response Tests

    [Test]
    [Category("UnitTest")]
    public async Task DeletePostingLineFromPostingJournalAsync_WhenCalled_ReturnsOkObjectResultContainingPostingJournalResponseDto()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        IActionResult result = await sut.DeletePostingLineFromPostingJournalAsync(
            _commandFeatureMock!.Object,
            _queryFeatureMock!.Object,
            _fixture!.Create<int>(),
            _fixture!.Create<Guid>(),
            _fixture!.Create<string>(),
            _fixture!.Create<string>(),
            CancellationToken.None);

        Assert.That(result, Is.TypeOf<OkObjectResult>());
        Assert.That(((OkObjectResult)result).Value, Is.TypeOf<PostingJournalResponseDto>());
    }

    #endregion

    #region Private methods

    private WebApi.Controllers.Accounting.AccountingController CreateSut(DateTimeOffset? localNow = null, IFormatProvider? formatProvider = null, ISecurityContext? securityContext = null)
    {
        _securityContextProviderMock!.Setup(_fixture!, securityContext: securityContext);

        _timeProviderMock!.Setup(m => m.GetUtcNow())
            .Returns((localNow ?? DateTimeOffset.Now).ToUniversalTime());
        _timeProviderMock!.Setup(m => m.LocalTimeZone)
            .Returns(TimeZoneInfo.Local);

        _commandFeatureMock!.Setup(m => m.ExecuteAsync(It.IsAny<DeletePostingLineFromPostingJournalRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _queryFeatureMock!.Setup(m => m.ExecuteAsync(It.IsAny<PostingJournalRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(CreatePostingJournalResponse()));

        return new WebApi.Controllers.Accounting.AccountingController(_timeProviderMock!.Object, formatProvider ?? CultureInfo.InvariantCulture, _securityContextProviderMock!.Object);
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