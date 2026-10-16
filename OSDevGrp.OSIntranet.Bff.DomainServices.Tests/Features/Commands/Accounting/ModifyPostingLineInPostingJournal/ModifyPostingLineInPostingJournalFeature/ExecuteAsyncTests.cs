using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Exceptions;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Commands.Accounting.ModifyPostingLineInPostingJournal.ModifyPostingLineInPostingJournalFeature;

[TestFixture]
public class ExecuteAsyncTests
{
    #region Private variables

    private Mock<IPermissionChecker>? _permissionCheckerMock;
    private Mock<IAccountingGateway>? _accountingGatewayMock;
    private Mock<IStaticTextProvider>? _staticTextProviderMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _permissionCheckerMock = new Mock<IPermissionChecker>();
        _accountingGatewayMock = new Mock<IAccountingGateway>();
        _staticTextProviderMock = new Mock<IStaticTextProvider>();
        _fixture = new Fixture();
        _random = new Random();
    }

    #region Line Found and Replacement Tests

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenPostingLineExists_ReplaceLineWithNewData()
    {
        // Arrange
        Guid targetIdentifier = _fixture!.Create<Guid>();
        Guid otherIdentifier = _fixture!.Create<Guid>();
        ApplyPostingJournalModel journalWithLines = CreatePostingJournal(
            existingLineIdentifiers: new[] { targetIdentifier, otherIdentifier });

        DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalRequest request = CreateRequest(
            identifier: targetIdentifier,
            account: "2000",
            postingText: "Updated posting");
        DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalFeature sut = CreateSut(postingJournalModel: journalWithLines);

        ApplyPostingJournalModel? savedJournal = null;
        _accountingGatewayMock!
            .Setup(m => m.SavePostingJournalAsync(It.IsAny<int>(), It.IsAny<ApplyPostingJournalModel>(), It.IsAny<CancellationToken>()))
            .Callback((int _, ApplyPostingJournalModel model, CancellationToken _) => savedJournal = model)
            .ReturnsAsync(journalWithLines);

        // Act
        await sut.ExecuteAsync(request);

        // Assert
        Assert.That(savedJournal, Is.Not.Null);
        var modifiedLine = savedJournal!.ApplyPostingLines!.FirstOrDefault(l => l.Identifier == targetIdentifier);
        Assert.That(modifiedLine, Is.Not.Null);
        Assert.That(modifiedLine!.AccountNumber, Is.EqualTo("2000"));
        Assert.That(modifiedLine.Details, Is.EqualTo("Updated posting"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenPostingLineExists_PreservesLineIdentifier()
    {
        // Arrange
        Guid targetIdentifier = _fixture!.Create<Guid>();
        ApplyPostingJournalModel journalWithLines = CreatePostingJournal(
            existingLineIdentifiers: new[] { targetIdentifier });

        DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalRequest request = CreateRequest(
            identifier: targetIdentifier,
            account: "2000");
        DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalFeature sut = CreateSut(postingJournalModel: journalWithLines);

        ApplyPostingJournalModel? savedJournal = null;
        _accountingGatewayMock!
            .Setup(m => m.SavePostingJournalAsync(It.IsAny<int>(), It.IsAny<ApplyPostingJournalModel>(), It.IsAny<CancellationToken>()))
            .Callback((int _, ApplyPostingJournalModel model, CancellationToken _) => savedJournal = model)
            .ReturnsAsync(journalWithLines);

        // Act
        await sut.ExecuteAsync(request);

        // Assert
        var modifiedLine = savedJournal!.ApplyPostingLines!.FirstOrDefault(l => l.Identifier == targetIdentifier);
        Assert.That(modifiedLine!.Identifier, Is.EqualTo(targetIdentifier), 
            "Identifier should be preserved after modification");
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenPostingLineExists_PreservesLineSortOrder()
    {
        // Arrange
        Guid targetIdentifier = _fixture!.Create<Guid>();
        int[] sortOrders = new[] { 5 };
        ApplyPostingJournalModel journalWithLines = CreatePostingJournal(
            existingLineIdentifiers: new[] { targetIdentifier },
            sortOrders: sortOrders);

        DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalRequest request = CreateRequest(
            identifier: targetIdentifier,
            account: "2000");
        DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalFeature sut = CreateSut(postingJournalModel: journalWithLines);

        ApplyPostingJournalModel? savedJournal = null;
        _accountingGatewayMock!
            .Setup(m => m.SavePostingJournalAsync(It.IsAny<int>(), It.IsAny<ApplyPostingJournalModel>(), It.IsAny<CancellationToken>()))
            .Callback((int _, ApplyPostingJournalModel model, CancellationToken _) => savedJournal = model)
            .ReturnsAsync(journalWithLines);

        // Act
        await sut.ExecuteAsync(request);

        // Assert
        var modifiedLine = savedJournal!.ApplyPostingLines!.FirstOrDefault(l => l.Identifier == targetIdentifier);
        Assert.That(modifiedLine!.SortOrder, Is.EqualTo(5),
            "SortOrder should be preserved after modification");
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenPostingLineExists_OtherLinesUnchanged()
    {
        // Arrange
        Guid targetIdentifier = _fixture!.Create<Guid>();
        Guid otherIdentifier1 = _fixture!.Create<Guid>();
        Guid otherIdentifier2 = _fixture!.Create<Guid>();
        ApplyPostingJournalModel journalWithLines = CreatePostingJournal(
            existingLineIdentifiers: new[] { targetIdentifier, otherIdentifier1, otherIdentifier2 });

        DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalRequest request = CreateRequest(
            identifier: targetIdentifier,
            account: "2000");
        DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalFeature sut = CreateSut(postingJournalModel: journalWithLines);

        ApplyPostingJournalModel? savedJournal = null;
        _accountingGatewayMock!
            .Setup(m => m.SavePostingJournalAsync(It.IsAny<int>(), It.IsAny<ApplyPostingJournalModel>(), It.IsAny<CancellationToken>()))
            .Callback((int _, ApplyPostingJournalModel model, CancellationToken _) => savedJournal = model)
            .ReturnsAsync(journalWithLines);

        // Act
        await sut.ExecuteAsync(request);

        // Assert
        Assert.That(savedJournal!.ApplyPostingLines, Has.Count.EqualTo(3));
        Assert.That(savedJournal.ApplyPostingLines.Any(l => l.Identifier == otherIdentifier1), Is.True,
            "Other line 1 should remain unchanged");
        Assert.That(savedJournal.ApplyPostingLines.Any(l => l.Identifier == otherIdentifier2), Is.True,
            "Other line 2 should remain unchanged");
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenPostingLineExists_ModifiesAllEightDataFields()
    {
        // Arrange
        Guid targetIdentifier = _fixture!.Create<Guid>();
        DateTimeOffset newPostingDate = DateTimeOffset.Now.AddDays(-5);
        string newPostingReference = "NEW_REF";
        string newAccount = "3000";
        string newPostingText = "New posting text";
        string newBudgetAccount = "4000";
        decimal newDebit = 250m;
        decimal newCredit = 0m;
        string newContactAccount = "5000";

        ApplyPostingJournalModel journalWithLines = CreatePostingJournal(
            existingLineIdentifiers: new[] { targetIdentifier });

        DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalRequest request = CreateRequest(
            identifier: targetIdentifier,
            postingDate: newPostingDate,
            postingReference: newPostingReference,
            account: newAccount,
            postingText: newPostingText,
            budgetAccount: newBudgetAccount,
            debit: newDebit,
            credit: newCredit,
            contactAccount: newContactAccount);

        DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalFeature sut = CreateSut(postingJournalModel: journalWithLines);

        ApplyPostingJournalModel? savedJournal = null;
        _accountingGatewayMock!
            .Setup(m => m.SavePostingJournalAsync(It.IsAny<int>(), It.IsAny<ApplyPostingJournalModel>(), It.IsAny<CancellationToken>()))
            .Callback((int _, ApplyPostingJournalModel model, CancellationToken _) => savedJournal = model)
            .ReturnsAsync(journalWithLines);

        // Act
        await sut.ExecuteAsync(request);

        // Assert
        var modifiedLine = savedJournal!.ApplyPostingLines!.FirstOrDefault(l => l.Identifier == targetIdentifier);
        Assert.That(modifiedLine!.PostingDate, Is.EqualTo(newPostingDate));
        Assert.That(modifiedLine.Reference, Is.EqualTo(newPostingReference));
        Assert.That(modifiedLine.AccountNumber, Is.EqualTo(newAccount));
        Assert.That(modifiedLine.Details, Is.EqualTo(newPostingText));
        Assert.That(modifiedLine.BudgetAccountNumber, Is.EqualTo(newBudgetAccount));
        Assert.That(modifiedLine.Debit, Is.EqualTo(newDebit));
        Assert.That(modifiedLine.Credit, Is.EqualTo(newCredit));
        Assert.That(modifiedLine.ContactAccountNumber, Is.EqualTo(newContactAccount));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenRequestPropertiesAreNull_ReplacesWithNullProperties()
    {
        // Arrange
        Guid targetIdentifier = _fixture!.Create<Guid>();
        ApplyPostingJournalModel journalWithLines = CreatePostingJournal(
            existingLineIdentifiers: new[] { targetIdentifier });

        DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalRequest request = CreateRequest(
            identifier: targetIdentifier,
            postingReference: null,
            budgetAccount: null,
            debit: null,
            credit: null,
            contactAccount: null);

        DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalFeature sut = CreateSut(postingJournalModel: journalWithLines);

        ApplyPostingJournalModel? savedJournal = null;
        _accountingGatewayMock!
            .Setup(m => m.SavePostingJournalAsync(It.IsAny<int>(), It.IsAny<ApplyPostingJournalModel>(), It.IsAny<CancellationToken>()))
            .Callback((int _, ApplyPostingJournalModel model, CancellationToken _) => savedJournal = model)
            .ReturnsAsync(journalWithLines);

        // Act
        await sut.ExecuteAsync(request);

        // Assert
        var modifiedLine = savedJournal!.ApplyPostingLines!.FirstOrDefault(l => l.Identifier == targetIdentifier);
        Assert.That(modifiedLine!.Reference, Is.Null);
        Assert.That(modifiedLine.BudgetAccountNumber, Is.Null);
        Assert.That(modifiedLine.Debit, Is.Null);
        Assert.That(modifiedLine.Credit, Is.Null);
        Assert.That(modifiedLine.ContactAccountNumber, Is.Null);
    }

    #endregion

    #region Unknown Identifier Exception Tests

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenPostingLineNotFound_ThrowsUnknownIdentifierException()
    {
        // Arrange
        Guid missingIdentifier = _fixture!.Create<Guid>();
        Guid existingIdentifier = _fixture!.Create<Guid>();
        ApplyPostingJournalModel journalWithLines = CreatePostingJournal(
            existingLineIdentifiers: new[] { existingIdentifier });

        DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalRequest request = CreateRequest(
            identifier: missingIdentifier);
        DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalFeature sut = CreateSut(postingJournalModel: journalWithLines);

        // Act & Assert
        Assert.ThrowsAsync<UnknownIdentifierException>(
            async () => await sut.ExecuteAsync(request));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenPostingLineNotFound_IncludesRequestedIdentifierInException()
    {
        // Arrange
        Guid missingIdentifier = _fixture!.Create<Guid>();
        Guid existingIdentifier = _fixture!.Create<Guid>();
        ApplyPostingJournalModel journalWithLines = CreatePostingJournal(
            existingLineIdentifiers: new[] { existingIdentifier });

        DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalRequest request = CreateRequest(
            identifier: missingIdentifier);
        DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalFeature sut = CreateSut(postingJournalModel: journalWithLines);

        // Act & Assert
        UnknownIdentifierException? exception = Assert.ThrowsAsync<UnknownIdentifierException>(
            async () => await sut.ExecuteAsync(request));

        Assert.That(exception!.Identifier, Is.EqualTo(missingIdentifier));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenPostingLineNotFound_FetchesStaticTextFromProvider()
    {
        // Arrange
        Guid missingIdentifier = _fixture!.Create<Guid>();
        Guid existingIdentifier = _fixture!.Create<Guid>();
        ApplyPostingJournalModel journalWithLines = CreatePostingJournal(
            existingLineIdentifiers: new[] { existingIdentifier });

        DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalRequest request = CreateRequest(
            identifier: missingIdentifier);
        DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalFeature sut = CreateSut(postingJournalModel: journalWithLines);

        _staticTextProviderMock!
            .Setup(m => m.GetStaticTextAsync(
                It.IsAny<StaticTextKey>(),
                It.IsAny<IEnumerable<object>>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync("Unknown identifier");

        // Act & Assert
        Assert.ThrowsAsync<UnknownIdentifierException>(
            async () => await sut.ExecuteAsync(request));

        _staticTextProviderMock.Verify(
            m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(key => key == StaticTextKey.UnknownIdentifier),
                It.IsAny<IEnumerable<object>>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenJournalIsEmpty_ThrowsUnknownIdentifierException()
    {
        // Arrange
        Guid missingIdentifier = _fixture!.Create<Guid>();
        ApplyPostingJournalModel emptyJournal = CreatePostingJournal();

        DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalRequest request = CreateRequest(
            identifier: missingIdentifier);
        DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalFeature sut = CreateSut(postingJournalModel: emptyJournal);

        // Act & Assert
        Assert.ThrowsAsync<UnknownIdentifierException>(
            async () => await sut.ExecuteAsync(request));
    }

    #endregion

    #region Cloning and Sorting Tests

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_ReturnsNewJournalInstance()
    {
        // Arrange
        Guid targetIdentifier = _fixture!.Create<Guid>();
        ApplyPostingJournalModel originalJournal = CreatePostingJournal(
            existingLineIdentifiers: new[] { targetIdentifier });
        DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalRequest request = CreateRequest(
            identifier: targetIdentifier);
        DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalFeature sut = CreateSut(postingJournalModel: originalJournal);

        ApplyPostingJournalModel? savedJournal = null;
        _accountingGatewayMock!
            .Setup(m => m.SavePostingJournalAsync(It.IsAny<int>(), It.IsAny<ApplyPostingJournalModel>(), It.IsAny<CancellationToken>()))
            .Callback((int _, ApplyPostingJournalModel model, CancellationToken _) => savedJournal = model)
            .ReturnsAsync(originalJournal);

        // Act
        await sut.ExecuteAsync(request);

        // Assert
        Assert.That(ReferenceEquals(savedJournal, originalJournal), Is.False,
            "ProcessPostingJournalAsync should return a new cloned instance, not the original");
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_PreservesAccountingNumber()
    {
        // Arrange
        int expectedAccountingNumber = _fixture!.Create<int>();
        Guid targetIdentifier = _fixture!.Create<Guid>();
        ApplyPostingJournalModel originalJournal = CreatePostingJournal(
            accountingNumber: expectedAccountingNumber,
            existingLineIdentifiers: new[] { targetIdentifier });
        DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalRequest request = CreateRequest(
            identifier: targetIdentifier);
        DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalFeature sut = CreateSut(postingJournalModel: originalJournal);

        ApplyPostingJournalModel? savedJournal = null;
        _accountingGatewayMock!
            .Setup(m => m.SavePostingJournalAsync(It.IsAny<int>(), It.IsAny<ApplyPostingJournalModel>(), It.IsAny<CancellationToken>()))
            .Callback((int _, ApplyPostingJournalModel model, CancellationToken _) => savedJournal = model)
            .ReturnsAsync(originalJournal);

        // Act
        await sut.ExecuteAsync(request);

        // Assert
        Assert.That(savedJournal!.AccountingNumber, Is.EqualTo(expectedAccountingNumber));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_SortsLinesByPostingDateDescending()
    {
        // Arrange
        Guid targetIdentifier = _fixture!.Create<Guid>();
        var line1 = CreatePostingLine(
            postingDate: new DateTimeOffset(2024, 1, 15, 0, 0, 0, TimeSpan.Zero),
            sortOrder: 1);
        var line2 = CreatePostingLine(
            postingDate: new DateTimeOffset(2024, 1, 10, 0, 0, 0, TimeSpan.Zero),
            sortOrder: 2);
        var line3 = CreatePostingLine(
            identifier: targetIdentifier,
            postingDate: new DateTimeOffset(2024, 1, 05, 0, 0, 0, TimeSpan.Zero),
            sortOrder: 3);

        ApplyPostingJournalModel journalWithLines = new ApplyPostingJournalModel(
            _fixture!.Create<int>(),
            new List<ApplyPostingLineModel> { line1, line2, line3 });

        DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalRequest request = CreateRequest(
            identifier: targetIdentifier,
            postingDate: new DateTimeOffset(2024, 1, 20, 0, 0, 0, TimeSpan.Zero));  // Change posting date to most recent
        DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalFeature sut = CreateSut(postingJournalModel: journalWithLines);

        ApplyPostingJournalModel? savedJournal = null;
        _accountingGatewayMock!
            .Setup(m => m.SavePostingJournalAsync(It.IsAny<int>(), It.IsAny<ApplyPostingJournalModel>(), It.IsAny<CancellationToken>()))
            .Callback((int _, ApplyPostingJournalModel model, CancellationToken _) => savedJournal = model)
            .ReturnsAsync(journalWithLines);

        // Act
        await sut.ExecuteAsync(request);

        // Assert
        Assert.That(savedJournal!.ApplyPostingLines, Has.Count.EqualTo(3));
        // Lines should be sorted by posting date descending: 2024-01-20 (modified), 2024-01-15, 2024-01-10
        var lines = savedJournal.ApplyPostingLines!.ToList();
        Assert.That(lines[0].PostingDate.Date, Is.EqualTo(new DateTimeOffset(2024, 1, 20, 0, 0, 0, TimeSpan.Zero).Date));
        Assert.That(lines[1].PostingDate.Date, Is.EqualTo(new DateTimeOffset(2024, 1, 15, 0, 0, 0, TimeSpan.Zero).Date));
        Assert.That(lines[2].PostingDate.Date, Is.EqualTo(new DateTimeOffset(2024, 1, 10, 0, 0, 0, TimeSpan.Zero).Date));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_SortsLinesByPostingDateThenSortOrderDescending()
    {
        // Arrange
        Guid targetIdentifier = _fixture!.Create<Guid>();
        var line1 = CreatePostingLine(
            postingDate: new DateTimeOffset(2024, 1, 10, 0, 0, 0, TimeSpan.Zero),
            sortOrder: 1);
        var line2 = CreatePostingLine(
            postingDate: new DateTimeOffset(2024, 1, 10, 0, 0, 0, TimeSpan.Zero),
            sortOrder: 3);
        var line3 = CreatePostingLine(
            identifier: targetIdentifier,
            postingDate: new DateTimeOffset(2024, 1, 10, 0, 0, 0, TimeSpan.Zero),
            sortOrder: 2);

        ApplyPostingJournalModel journalWithLines = new ApplyPostingJournalModel(
            _fixture!.Create<int>(),
            new List<ApplyPostingLineModel> { line1, line2, line3 });

        DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalRequest request = CreateRequest(
            identifier: targetIdentifier,
            postingDate: new DateTimeOffset(2024, 1, 10, 0, 0, 0, TimeSpan.Zero));  // Same date
        DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalFeature sut = CreateSut(postingJournalModel: journalWithLines);

        ApplyPostingJournalModel? savedJournal = null;
        _accountingGatewayMock!
            .Setup(m => m.SavePostingJournalAsync(It.IsAny<int>(), It.IsAny<ApplyPostingJournalModel>(), It.IsAny<CancellationToken>()))
            .Callback((int _, ApplyPostingJournalModel model, CancellationToken _) => savedJournal = model)
            .ReturnsAsync(journalWithLines);

        // Act
        await sut.ExecuteAsync(request);

        // Assert
        Assert.That(savedJournal!.ApplyPostingLines, Has.Count.EqualTo(3));
        // All have same posting date, so sorted by SortOrder descending: 3, 2, 1
        var lines = savedJournal.ApplyPostingLines!.ToList();
        Assert.That(lines[0].SortOrder, Is.EqualTo(3));
        Assert.That(lines[1].SortOrder, Is.EqualTo(2));
        Assert.That(lines[2].SortOrder, Is.EqualTo(1));
    }

    #endregion

    #region Gateway Interaction Tests

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_InvokesAccountingGatewayGetPostingJournalAsync()
    {
        // Arrange
        Guid targetIdentifier = _fixture!.Create<Guid>();
        int expectedAccountingNumber = _fixture!.Create<int>();
        ApplyPostingJournalModel journalWithLines = CreatePostingJournal(
            accountingNumber: expectedAccountingNumber,
            existingLineIdentifiers: new[] { targetIdentifier });

        DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalRequest request = CreateRequest(
            accountingNumber: expectedAccountingNumber,
            identifier: targetIdentifier);
        DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalFeature sut = CreateSut(postingJournalModel: journalWithLines);

        // Act
        await sut.ExecuteAsync(request);

        // Assert
        _accountingGatewayMock!.Verify(
            m => m.GetPostingJournalAsync(
                It.Is<int>(num => num == expectedAccountingNumber),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_InvokesAccountingGatewaySavePostingJournalAsync()
    {
        // Arrange
        Guid targetIdentifier = _fixture!.Create<Guid>();
        int expectedAccountingNumber = _fixture!.Create<int>();
        ApplyPostingJournalModel journalWithLines = CreatePostingJournal(
            accountingNumber: expectedAccountingNumber,
            existingLineIdentifiers: new[] { targetIdentifier });

        DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalRequest request = CreateRequest(
            accountingNumber: expectedAccountingNumber,
            identifier: targetIdentifier);
        DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalFeature sut = CreateSut(postingJournalModel: journalWithLines);

        // Act
        await sut.ExecuteAsync(request);

        // Assert
        _accountingGatewayMock!.Verify(
            m => m.SavePostingJournalAsync(
                It.Is<int>(num => num == expectedAccountingNumber),
                It.IsAny<ApplyPostingJournalModel>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_PassesCancellationTokenToGatewayMethods()
    {
        // Arrange
        Guid targetIdentifier = _fixture!.Create<Guid>();
        ApplyPostingJournalModel journalWithLines = CreatePostingJournal(
            existingLineIdentifiers: new[] { targetIdentifier });

        DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalRequest request = CreateRequest(
            identifier: targetIdentifier);
        DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalFeature sut = CreateSut(postingJournalModel: journalWithLines);

        CancellationToken capturedToken = default;
        _accountingGatewayMock!
            .Setup(m => m.GetPostingJournalAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Callback((int _, CancellationToken token) => capturedToken = token)
            .ReturnsAsync(journalWithLines);

        CancellationTokenSource cts = new CancellationTokenSource();

        // Act
        await sut.ExecuteAsync(request, cts.Token);

        // Assert
        Assert.That(capturedToken, Is.EqualTo(cts.Token));
    }

    #endregion

    #region Test Helpers

    private DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalFeature CreateSut(ApplyPostingJournalModel? postingJournalModel = null)
    {
        ApplyPostingJournalModel modelToUse = postingJournalModel ?? CreatePostingJournal();

        _accountingGatewayMock!
            .Setup(m => m.GetPostingJournalAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(modelToUse);

        _accountingGatewayMock!
            .Setup(m => m.SavePostingJournalAsync(It.IsAny<int>(), It.IsAny<ApplyPostingJournalModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(modelToUse);

        _staticTextProviderMock!
            .Setup(m => m.GetStaticTextAsync(
                It.IsAny<StaticTextKey>(),
                It.IsAny<IEnumerable<object>>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync("Unknown identifier");

        return new DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalFeature(
            _permissionCheckerMock!.Object,
            _accountingGatewayMock.Object,
            _staticTextProviderMock.Object);
    }

    private DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalRequest CreateRequest(
        Guid? requestId = null,
        int? accountingNumber = null,
        Guid? identifier = null,
        DateTimeOffset? postingDate = null,
        string? postingReference = null,
        string? account = null,
        string? postingText = null,
        string? budgetAccount = null,
        decimal? debit = null,
        decimal? credit = null,
        string? contactAccount = null,
        IFormatProvider? formatProvider = null,
        ISecurityContext? securityContext = null)
    {
        return new DomainServices.Features.Commands.Accounting.ModifyPostingLineInPostingJournalRequest(
            requestId ?? _fixture!.Create<Guid>(),
            accountingNumber ?? _fixture!.Create<int>(),
            identifier ?? _fixture!.Create<Guid>(),
            postingDate ?? DateTimeOffset.Now,
            postingReference,  // Keep null if not provided
            account ?? "1000",
            postingText ?? "Test posting",
            budgetAccount,  // Keep null if not provided
            debit,  // Keep null if not provided
            credit,  // Keep null if not provided
            contactAccount,  // Keep null if not provided
            formatProvider ?? _fixture!.Create<CultureInfo>(),
            securityContext ?? CreateSecurityContext());
    }

    private ApplyPostingJournalModel CreatePostingJournal(
        int? accountingNumber = null,
        int lineCount = 0,
        Guid[]? existingLineIdentifiers = null,
        int[]? sortOrders = null)
    {
        var lines = new List<ApplyPostingLineModel>();

        if (existingLineIdentifiers != null)
        {
            int index = 0;
            foreach (var identifier in existingLineIdentifiers)
            {
                int? sortOrder = sortOrders != null && index < sortOrders.Length ? sortOrders[index] : (int?)(index + 1);
                lines.Add(CreatePostingLine(identifier: identifier, sortOrder: sortOrder));
                index++;
            }
        }
        else if (lineCount > 0)
        {
            for (int i = 0; i < lineCount; i++)
            {
                int? sortOrder = sortOrders != null && i < sortOrders.Length ? sortOrders[i] : (int?)(i + 1);
                lines.Add(CreatePostingLine(sortOrder: sortOrder));
            }
        }

        return new ApplyPostingJournalModel(
            accountingNumber ?? _fixture!.Create<int>(),
            (System.Collections.Generic.ICollection<ApplyPostingLineModel>)lines);
    }

    private ApplyPostingLineModel CreatePostingLine(
        Guid? identifier = null,
        DateTimeOffset? postingDate = null,
        int? sortOrder = null)
    {
        return new ApplyPostingLineModel(
            "1000",
            _fixture!.Create<string>(),
            _fixture!.Create<string>(),
            0d,
            (double)_random!.Next(0, 1000),
            _fixture!.Create<string>(),
            identifier ?? _fixture!.Create<Guid>(),
            postingDate ?? DateTimeOffset.Now,
            _fixture!.Create<string>(),
            sortOrder ?? _fixture!.Create<int>());
    }

    private ISecurityContext CreateSecurityContext()
    {
        Mock<ISecurityContext> securityContextMock = new Mock<ISecurityContext>();
        return securityContextMock.Object;
    }

    #endregion
}