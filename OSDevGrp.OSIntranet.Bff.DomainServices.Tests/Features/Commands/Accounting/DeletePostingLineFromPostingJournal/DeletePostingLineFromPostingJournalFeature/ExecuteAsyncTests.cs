using System.Globalization;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using DeletePostingLineFeature = OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Accounting.DeletePostingLineFromPostingJournalFeature;
using DeletePostingLineRequest = OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Accounting.DeletePostingLineFromPostingJournalRequest;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Commands.Accounting.DeletePostingLineFromPostingJournal.DeletePostingLineFromPostingJournalFeature;

[TestFixture]
public class ExecuteAsyncTests
{
    #region Private variables

    private Mock<IPermissionChecker>? _permissionCheckerMock;
    private Mock<IAccountingGateway>? _accountingGatewayMock;
    private Mock<IStaticTextProvider>? _staticTextProviderMock;
    private Fixture? _fixture;

    #endregion

    #region Setup

    [SetUp]
    public void SetUp()
    {
        _permissionCheckerMock = new Mock<IPermissionChecker>();
        _accountingGatewayMock = new Mock<IAccountingGateway>();
        _staticTextProviderMock = new Mock<IStaticTextProvider>();
        _fixture = new Fixture();
    }

    #endregion

    #region Tests

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenPostingLineExists_RemovesOnlyMatchingLine()
    {
        // Arrange
        Guid targetIdentifier = _fixture!.Create<Guid>();
        ApplyPostingLineModel targetLine = CreatePostingLine(targetIdentifier);
        ApplyPostingLineModel remainingLine1 = CreatePostingLine(postingDate: new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero));
        ApplyPostingLineModel remainingLine2 = CreatePostingLine(postingDate: new DateTimeOffset(2025, 1, 2, 0, 0, 0, TimeSpan.Zero));
        ApplyPostingJournalModel originalJournal = CreatePostingJournal(targetLine, remainingLine1, remainingLine2);
        DeletePostingLineRequest request = CreateRequest(targetIdentifier);
        DeletePostingLineFeature sut = CreateSut(originalJournal);
        ApplyPostingJournalModel? savedJournal = null;

        _accountingGatewayMock!
            .Setup(mock => mock.SavePostingJournalAsync(It.IsAny<int>(), It.IsAny<ApplyPostingJournalModel>(), It.IsAny<CancellationToken>()))
            .Callback((int _, ApplyPostingJournalModel journal, CancellationToken _) => savedJournal = journal)
            .ReturnsAsync(originalJournal);

        // Act
        await sut.ExecuteAsync(request);

        // Assert
        Assert.That(savedJournal, Is.Not.Null);
        Assert.That(savedJournal!.ApplyPostingLines, Has.Count.EqualTo(2));
        Assert.That(savedJournal.ApplyPostingLines!.Select(line => line.Identifier), Is.EquivalentTo(new[] { remainingLine1.Identifier, remainingLine2.Identifier }));
        Assert.That(originalJournal.ApplyPostingLines, Does.Contain(targetLine));
        Assert.That(savedJournal.ApplyPostingLines, Does.Contain(remainingLine1));
        Assert.That(savedJournal.ApplyPostingLines, Does.Contain(remainingLine2));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenPostingLineDoesNotExist_ThrowsLocalizedUnknownIdentifierException()
    {
        // Arrange
        Guid missingIdentifier = _fixture!.Create<Guid>();
        IFormatProvider formatProvider = CultureInfo.GetCultureInfo("da-DK");
        ApplyPostingJournalModel journal = CreatePostingJournal(CreatePostingLine());
        DeletePostingLineRequest request = CreateRequest(missingIdentifier, formatProvider: formatProvider);
        DeletePostingLineFeature sut = CreateSut(journal);
        CancellationToken cancellationToken = _fixture.Create<CancellationToken>();
        const string expectedMessage = "Unknown posting-line identifier.";

        _staticTextProviderMock!
            .Setup(mock => mock.GetStaticTextAsync(
                It.IsAny<StaticTextKey>(),
                It.IsAny<IEnumerable<object>>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedMessage);

        // Act
        UnknownIdentifierException exception = (await Assert.ThrowsAsync<UnknownIdentifierException>(
            () => sut.ExecuteAsync(request, cancellationToken)))!;

        // Assert
        Assert.That(exception.Identifier, Is.EqualTo(missingIdentifier));
        Assert.That(exception.Message, Is.EqualTo(expectedMessage));
        _staticTextProviderMock.Verify(
            mock => mock.GetStaticTextAsync(
                It.Is<StaticTextKey>(key => key == StaticTextKey.UnknownIdentifier),
                It.IsAny<IEnumerable<object>>(),
                It.Is<IFormatProvider>(provider => ReferenceEquals(provider, formatProvider)),
                It.Is<CancellationToken>(token => token == cancellationToken)),
            Times.Once);
        _accountingGatewayMock!.Verify(
            mock => mock.SavePostingJournalAsync(It.IsAny<int>(), It.IsAny<ApplyPostingJournalModel>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_SortsAndClonesJournalAndSavesWithRequestAccountingNumberAndCancellationToken()
    {
        // Arrange
        Guid targetIdentifier = _fixture!.Create<Guid>();
        int accountingNumber = _fixture.Create<int>();
        ApplyPostingLineModel olderLine = CreatePostingLine(postingDate: new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero), sortOrder: 2);
        ApplyPostingLineModel targetLine = CreatePostingLine(targetIdentifier);
        ApplyPostingLineModel newerLine = CreatePostingLine(postingDate: new DateTimeOffset(2025, 1, 3, 0, 0, 0, TimeSpan.Zero), sortOrder: 1);
        ApplyPostingJournalModel originalJournal = CreatePostingJournal(targetLine, olderLine, newerLine);
        DeletePostingLineRequest request = CreateRequest(targetIdentifier, accountingNumber);
        DeletePostingLineFeature sut = CreateSut(originalJournal);
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        ApplyPostingJournalModel? savedJournal = null;

        _accountingGatewayMock!
            .Setup(mock => mock.SavePostingJournalAsync(It.IsAny<int>(), It.IsAny<ApplyPostingJournalModel>(), It.IsAny<CancellationToken>()))
            .Callback((int _, ApplyPostingJournalModel journal, CancellationToken _) => savedJournal = journal)
            .ReturnsAsync(originalJournal);

        // Act
        await sut.ExecuteAsync(request, cancellationToken);

        // Assert
        Assert.That(savedJournal, Is.Not.Null);
        Assert.That(ReferenceEquals(savedJournal, originalJournal), Is.False);
        Assert.That(ReferenceEquals(savedJournal!.ApplyPostingLines, originalJournal.ApplyPostingLines), Is.False);
        Assert.That(savedJournal.ApplyPostingLines!.Select(line => line.Identifier), Is.EqualTo(new[] { newerLine.Identifier, olderLine.Identifier }));
        _accountingGatewayMock!.Verify(
            mock => mock.GetPostingJournalAsync(
                It.Is<int>(value => value == accountingNumber),
                It.Is<CancellationToken>(token => token == cancellationToken)),
            Times.Once);
        _accountingGatewayMock.Verify(
            mock => mock.SavePostingJournalAsync(
                It.Is<int>(value => value == accountingNumber),
                It.IsAny<ApplyPostingJournalModel>(),
                It.Is<CancellationToken>(token => token == cancellationToken)),
            Times.Once);
    }

    #endregion

    #region Test Helpers

    private DeletePostingLineFeature CreateSut(ApplyPostingJournalModel journal)
    {
        _accountingGatewayMock!
            .Setup(mock => mock.GetPostingJournalAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(journal);
        _accountingGatewayMock
            .Setup(mock => mock.SavePostingJournalAsync(It.IsAny<int>(), It.IsAny<ApplyPostingJournalModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(journal);
        _staticTextProviderMock!
            .Setup(mock => mock.GetStaticTextAsync(
                It.IsAny<StaticTextKey>(),
                It.IsAny<IEnumerable<object>>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync("Unknown identifier");

        return new DeletePostingLineFeature(
            _permissionCheckerMock!.Object,
            _accountingGatewayMock.Object,
            _staticTextProviderMock.Object);
    }

    private DeletePostingLineRequest CreateRequest(
        Guid identifier,
        int? accountingNumber = null,
        IFormatProvider? formatProvider = null)
    {
        return new DeletePostingLineRequest(
            _fixture!.Create<Guid>(),
            accountingNumber ?? _fixture.Create<int>(),
            identifier,
            _fixture.Create<string>(),
            _fixture.Create<string>(),
            formatProvider ?? CultureInfo.GetCultureInfo("en-US"),
            CreateSecurityContext());
    }

    private ApplyPostingJournalModel CreatePostingJournal(params ApplyPostingLineModel[] lines)
    {
        return new ApplyPostingJournalModel(_fixture!.Create<int>(), lines.ToList());
    }

    private ApplyPostingLineModel CreatePostingLine(
        Guid? identifier = null,
        DateTimeOffset? postingDate = null,
        int? sortOrder = null)
    {
        return new ApplyPostingLineModel(
            "1000",
            "Budget",
            "Contact",
            0d,
            10d,
            "Posting",
            identifier ?? _fixture!.Create<Guid>(),
            postingDate ?? new DateTimeOffset(2025, 1, 2, 0, 0, 0, TimeSpan.Zero),
            "Reference",
            sortOrder ?? 1);
    }

    private ISecurityContext CreateSecurityContext()
    {
        return new Mock<ISecurityContext>().Object;
    }

    #endregion
}