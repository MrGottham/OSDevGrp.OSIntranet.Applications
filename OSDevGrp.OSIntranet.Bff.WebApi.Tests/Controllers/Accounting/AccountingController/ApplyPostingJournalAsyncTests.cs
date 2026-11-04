using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Accounting;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Notifications;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Notification;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData;
using OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;
using OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security.SecurityContextProvider;
using OSDevGrp.OSIntranet.Bff.WebApi.Tests.Shared.Dtos;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Controllers.Accounting.AccountingController;

[TestFixture]
public class ApplyPostingJournalAsyncTests
{
    #region Private variables

    private Mock<TimeProvider>? _timeProviderMock;
    private Mock<ISecurityContextProvider>? _securityContextProviderMock;
    private Mock<ICommandFeature<ApplyPostingJournalRequest>>? _commandFeatureMock;
    private Mock<IQueryFeature<NotificationsRequest, NotificationsResponse>>? _queryFeatureMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _timeProviderMock = new Mock<TimeProvider>();
        _securityContextProviderMock = new Mock<ISecurityContextProvider>();
        _commandFeatureMock = new Mock<ICommandFeature<ApplyPostingJournalRequest>>();
        _queryFeatureMock = new Mock<IQueryFeature<NotificationsRequest, NotificationsResponse>>();
        _fixture = new Fixture();
        _random = new Random(_fixture.Create<int>());
    }

    #region Command Feature Invocation Tests

    [Test]
    [Category("UnitTest")]
    public async Task ApplyPostingJournalAsync_WhenCalled_AssertGetCurrentSecurityContextAsyncWasCalledOnSecurityContextProviderWithGivenCancellationToken()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.ApplyPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), cancellationToken);

        _securityContextProviderMock!.Verify(m => m.GetCurrentSecurityContextAsync(It.Is<CancellationToken>(value => value == cancellationToken)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ApplyPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnCommandFeatureWithApplyPostingJournalRequestWhereRequestIdIsNotEqualToGuidEmpty()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ApplyPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), cancellationTokenSource.Token);

        _commandFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<ApplyPostingJournalRequest>(value => value.RequestId != Guid.Empty),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ApplyPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnCommandFeatureWithApplyPostingJournalRequestWhereAccountingNumberIsEqualToGivenAccountingNumber()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        int accountingNumber = _fixture!.Create<int>();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ApplyPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, accountingNumber, cancellationTokenSource.Token);

        _commandFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<ApplyPostingJournalRequest>(value => value.AccountingNumber == accountingNumber),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ApplyPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnCommandFeatureWithApplyPostingJournalRequestWhereSecurityContextIsEqualToSecurityContextResolvedBySecurityContextProvider()
    {
        ISecurityContext securityContext = _fixture!.CreateSecurityContext();
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut(securityContext: securityContext);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ApplyPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), cancellationTokenSource.Token);

        _commandFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<ApplyPostingJournalRequest>(value => value.SecurityContext == securityContext),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ApplyPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnCommandFeatureWithApplyPostingJournalRequestWhereFormatProviderIsEqualToFormatProviderFromDependencies()
    {
        IFormatProvider formatProvider = _fixture!.Create<CultureInfo>();
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut(formatProvider: formatProvider);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ApplyPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), cancellationTokenSource.Token);

        _commandFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<ApplyPostingJournalRequest>(value => value.FormatProvider == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region Query Feature Invocation Tests

    [Test]
    [Category("UnitTest")]
    public async Task ApplyPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithNotificationsRequestWhereRequestIdIsNotEqualToGuidEmpty()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ApplyPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<NotificationsRequest>(value => value.RequestId != Guid.Empty),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ApplyPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithNotificationsRequestWhereFormatProviderIsEqualToFormatProviderFromDependencies()
    {
        IFormatProvider formatProvider = _fixture!.Create<CultureInfo>();
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut(formatProvider: formatProvider);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ApplyPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<NotificationsRequest>(value => value.FormatProvider == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ApplyPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithNotificationsRequestWhereSecurityContextIsEqualToSecurityContextResolvedBySecurityContextProvider()
    {
        ISecurityContext securityContext = _fixture!.CreateSecurityContext();
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut(securityContext: securityContext);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ApplyPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<NotificationsRequest>(value => value.SecurityContext == securityContext),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ApplyPostingJournalAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithGivenCancellationToken()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.ApplyPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), cancellationToken);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(It.IsAny<NotificationsRequest>(), It.Is<CancellationToken>(value => value == cancellationToken)), Times.Once);
    }

    #endregion

    #region Response Tests

    [Test]
    [Category("UnitTest")]
    public async Task ApplyPostingJournalAsync_WhenCalled_ReturnsOkObjectResult()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        IActionResult result = await sut.ApplyPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), cancellationTokenSource.Token);

        Assert.That(result, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task ApplyPostingJournalAsync_WhenCalled_AssertOkObjectResultValueIsApplyPostingJournalResponseDto()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        IActionResult result = await sut.ApplyPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), cancellationTokenSource.Token);

        Assert.That(((OkObjectResult)result).Value, Is.TypeOf<ApplyPostingJournalResponseDto>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task ApplyPostingJournalAsync_WhenCalled_AssertOkObjectResultValueNumberIsEqualToGivenAccountingNumber()
    {
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut();

        int accountingNumber = _fixture!.Create<int>();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        IActionResult result = await sut.ApplyPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, accountingNumber, cancellationTokenSource.Token);

        Assert.That(((ApplyPostingJournalResponseDto)((OkObjectResult)result).Value!).Number, Is.EqualTo(accountingNumber));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ApplyPostingJournalAsync_WhenCalled_AssertOkObjectResultValueNotificationsAreEqualToNotificationsFromQueryFeature()
    {
        IReadOnlyCollection<INotification> notifications = new List<INotification> { _fixture!.CreateNotification(), _fixture!.CreateNotification() };
        WebApi.Controllers.Accounting.AccountingController sut = CreateSut(notifications: notifications);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        IActionResult result = await sut.ApplyPostingJournalAsync(_commandFeatureMock!.Object, _queryFeatureMock!.Object, _fixture!.Create<int>(), cancellationTokenSource.Token);

        Assert.That(((ApplyPostingJournalResponseDto)((OkObjectResult)result).Value!).Notifications, Has.Count.EqualTo(notifications.Count));
    }

    #endregion

    #region Private methods

    private WebApi.Controllers.Accounting.AccountingController CreateSut(IFormatProvider? formatProvider = null, ISecurityContext? securityContext = null, IReadOnlyCollection<INotification>? notifications = null)
    {
        _securityContextProviderMock!.Setup(_fixture!, securityContext: securityContext);

        _commandFeatureMock!.Setup(m => m.ExecuteAsync(It.IsAny<ApplyPostingJournalRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _queryFeatureMock!.Setup(m => m.ExecuteAsync(It.IsAny<NotificationsRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(CreateNotificationsResponse(notifications)));

        return new WebApi.Controllers.Accounting.AccountingController(_timeProviderMock!.Object, formatProvider ?? CultureInfo.InvariantCulture, _securityContextProviderMock!.Object);
    }

    private NotificationsResponse CreateNotificationsResponse(IReadOnlyCollection<INotification>? notifications = null)
    {
        IReadOnlyDictionary<StaticTextKey, string> staticTexts = _fixture!.CreateStaticTexts(_random!);

        return new NotificationsResponse(notifications ?? new List<INotification> { _fixture!.CreateNotification() }, staticTexts);
    }

    #endregion
}