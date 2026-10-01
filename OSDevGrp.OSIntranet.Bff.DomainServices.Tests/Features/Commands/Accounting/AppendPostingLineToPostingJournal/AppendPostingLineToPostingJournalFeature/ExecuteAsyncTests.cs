using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Exceptions;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Commands.Accounting.AppendPostingLineToPostingJournal.AppendPostingLineToPostingJournalFeature;

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

    #region Duplicate Identifier Detection Tests

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenIdentifierAlreadyExists_ThrowsIdentifierAlreadyExistsException()
    {
        // Arrange
        Guid duplicateIdentifier = _fixture!.Create<Guid>();
        ApplyPostingJournalModel journalWithExistingLine = CreatePostingJournal(
            existingLineIdentifiers: new[] { duplicateIdentifier });
        
        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest request = CreateRequest(identifier: duplicateIdentifier);
        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalFeature sut = CreateSut(postingJournalModel: journalWithExistingLine);

        // Act & Assert
        Assert.ThrowsAsync<IdentifierAlreadyExistsException>(
            async () => await sut.ExecuteAsync(request));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenIdentifierAlreadyExists_IncludesIdentifierInException()
    {
        // Arrange
        Guid duplicateIdentifier = _fixture!.Create<Guid>();
        ApplyPostingJournalModel journalWithExistingLine = CreatePostingJournal(
            existingLineIdentifiers: new[] { duplicateIdentifier });
        
        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest request = CreateRequest(identifier: duplicateIdentifier);
        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalFeature sut = CreateSut(postingJournalModel: journalWithExistingLine);

        // Act & Assert
        IdentifierAlreadyExistsException? exception = Assert.ThrowsAsync<IdentifierAlreadyExistsException>(
            async () => await sut.ExecuteAsync(request));
        
        Assert.That(exception!.Identifier, Is.EqualTo(duplicateIdentifier));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenIdentifierAlreadyExists_FetchesStaticTextFromProvider()
    {
        // Arrange
        Guid duplicateIdentifier = _fixture!.Create<Guid>();
        ApplyPostingJournalModel journalWithExistingLine = CreatePostingJournal(
            existingLineIdentifiers: new[] { duplicateIdentifier });
        
        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest request = CreateRequest(identifier: duplicateIdentifier);
        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalFeature sut = CreateSut(postingJournalModel: journalWithExistingLine);

        _staticTextProviderMock!
            .Setup(m => m.GetStaticTextAsync(
                It.IsAny<StaticTextKey>(),
                It.IsAny<IEnumerable<object>>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync("Duplicate identifier");

        // Act & Assert
        Assert.ThrowsAsync<IdentifierAlreadyExistsException>(
            async () => await sut.ExecuteAsync(request));

        _staticTextProviderMock.Verify(
            m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(key => key == StaticTextKey.IdentifierAlreadyExists),
                It.IsAny<IEnumerable<object>>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenUniqueIdentifier_DoesNotThrow()
    {
        // Arrange
        Guid uniqueIdentifier = _fixture!.Create<Guid>();
        Guid existingIdentifier = _fixture!.Create<Guid>();
        
        ApplyPostingJournalModel journalWithExistingLine = CreatePostingJournal(
            existingLineIdentifiers: new[] { existingIdentifier });
        
        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest request = CreateRequest(identifier: uniqueIdentifier);
        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalFeature sut = CreateSut(postingJournalModel: journalWithExistingLine);

        // Act & Assert
        Assert.DoesNotThrowAsync(async () => await sut.ExecuteAsync(request));
    }

    #endregion

    #region Line Creation and Property Mapping Tests

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_CreatesLineWithCorrectIdentifier()
    {
        // Arrange
        Guid expectedIdentifier = _fixture!.Create<Guid>();
        ApplyPostingJournalModel emptyJournal = CreatePostingJournal();
        
        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest request = CreateRequest(identifier: expectedIdentifier);
        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalFeature sut = CreateSut(postingJournalModel: emptyJournal);

        ApplyPostingJournalModel? savedJournal = null;
        _accountingGatewayMock!
            .Setup(m => m.SavePostingJournalAsync(It.IsAny<int>(), It.IsAny<ApplyPostingJournalModel>(), It.IsAny<CancellationToken>()))
            .Callback((int _, ApplyPostingJournalModel model, CancellationToken _) => savedJournal = model)
            .ReturnsAsync(emptyJournal);

        // Act
        await sut.ExecuteAsync(request);

        // Assert
        Assert.That(savedJournal, Is.Not.Null);
        Assert.That(savedJournal!.ApplyPostingLines, Has.Count.EqualTo(1));
        var lines = savedJournal.ApplyPostingLines!.ToList();
        Assert.That(lines[0].Identifier, Is.EqualTo(expectedIdentifier));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_MapsAllPropertiesFromRequest()
    {
        // Arrange
        DateTimeOffset expectedPostingDate = DateTimeOffset.Now;
        string expectedReference = "REF123";
        string expectedAccount = "1000";
        string expectedPostingText = "Test posting";
        string expectedBudgetAccount = "2000";
        decimal expectedDebit = 100m;
        decimal expectedCredit = 0m;
        string expectedContactAccount = "3000";

        ApplyPostingJournalModel emptyJournal = CreatePostingJournal();

        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest request = CreateRequest(
            postingDate: expectedPostingDate,
            postingReference: expectedReference,
            account: expectedAccount,
            postingText: expectedPostingText,
            budgetAccount: expectedBudgetAccount,
            debit: expectedDebit,
            credit: expectedCredit,
            contactAccount: expectedContactAccount);

        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalFeature sut = CreateSut(postingJournalModel: emptyJournal);

        ApplyPostingJournalModel? savedJournal = null;
        _accountingGatewayMock!
            .Setup(m => m.SavePostingJournalAsync(It.IsAny<int>(), It.IsAny<ApplyPostingJournalModel>(), It.IsAny<CancellationToken>()))
            .Callback((int _, ApplyPostingJournalModel model, CancellationToken _) => savedJournal = model)
            .ReturnsAsync(emptyJournal);

        // Act
        await sut.ExecuteAsync(request);

        // Assert
        Assert.That(savedJournal, Is.Not.Null);
        var lines = savedJournal!.ApplyPostingLines!.ToList();
        ApplyPostingLineModel createdLine = lines[0];
        
        Assert.That(createdLine.PostingDate, Is.EqualTo(expectedPostingDate));
        Assert.That(createdLine.Reference, Is.EqualTo(expectedReference));
        Assert.That(createdLine.AccountNumber, Is.EqualTo(expectedAccount));
        Assert.That(createdLine.Details, Is.EqualTo(expectedPostingText));
        Assert.That(createdLine.BudgetAccountNumber, Is.EqualTo(expectedBudgetAccount));
        Assert.That(createdLine.Debit, Is.EqualTo(expectedDebit));
        Assert.That(createdLine.Credit, Is.EqualTo(expectedCredit));
        Assert.That(createdLine.ContactAccountNumber, Is.EqualTo(expectedContactAccount));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenRequestPropertiesAreNull_CreatesLineWithNullProperties()
    {
        // Arrange
        ApplyPostingJournalModel emptyJournal = CreatePostingJournal();

        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest request = CreateRequest(
            postingReference: null,
            budgetAccount: null,
            debit: null,
            credit: null,
            contactAccount: null);

        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalFeature sut = CreateSut(postingJournalModel: emptyJournal);

        ApplyPostingJournalModel? savedJournal = null;
        _accountingGatewayMock!
            .Setup(m => m.SavePostingJournalAsync(It.IsAny<int>(), It.IsAny<ApplyPostingJournalModel>(), It.IsAny<CancellationToken>()))
            .Callback((int _, ApplyPostingJournalModel model, CancellationToken _) => savedJournal = model)
            .ReturnsAsync(emptyJournal);

        // Act
        await sut.ExecuteAsync(request);

        // Assert
        Assert.That(savedJournal, Is.Not.Null);
        var lines = savedJournal!.ApplyPostingLines!.ToList();
        ApplyPostingLineModel createdLine = lines[0];
        
        Assert.That(createdLine.Reference, Is.Null);
        Assert.That(createdLine.BudgetAccountNumber, Is.Null);
        Assert.That(createdLine.Debit, Is.Null);
        Assert.That(createdLine.Credit, Is.Null);
        Assert.That(createdLine.ContactAccountNumber, Is.Null);
    }

    #endregion

    #region Sort Order Calculation Tests

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenJournalIsEmpty_SetsSortOrderToOne()
    {
        // Arrange
        ApplyPostingJournalModel emptyJournal = CreatePostingJournal();
        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest request = CreateRequest(
            postingReference: "REF",
            budgetAccount: "BUD",
            contactAccount: "CON",
            debit: 100m,
            credit: 0m);
        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalFeature sut = CreateSut(postingJournalModel: emptyJournal);

        ApplyPostingJournalModel? savedJournal = null;
        _accountingGatewayMock!
            .Setup(m => m.SavePostingJournalAsync(It.IsAny<int>(), It.IsAny<ApplyPostingJournalModel>(), It.IsAny<CancellationToken>()))
            .Callback((int _, ApplyPostingJournalModel model, CancellationToken _) => savedJournal = model)
            .ReturnsAsync(emptyJournal);

        // Act
        await sut.ExecuteAsync(request);

        // Assert
        var lines = savedJournal!.ApplyPostingLines!.ToList();
        Assert.That(lines[0].SortOrder, Is.EqualTo(1));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenJournalHasExistingLines_SetsSortOrderToMaxPlusOne()
    {
        // Arrange
        ApplyPostingJournalModel journalWithLines = CreatePostingJournal(lineCount: 3, sortOrders: new int[] { 1, 3, 5 });

        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest request = CreateRequest(
            postingReference: "REF",
            budgetAccount: "BUD",
            contactAccount: "CON",
            debit: 100m,
            credit: 0m);
        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalFeature sut = CreateSut(postingJournalModel: journalWithLines);

        ApplyPostingJournalModel? savedJournal = null;
        _accountingGatewayMock!
            .Setup(m => m.SavePostingJournalAsync(It.IsAny<int>(), It.IsAny<ApplyPostingJournalModel>(), It.IsAny<CancellationToken>()))
            .Callback((int _, ApplyPostingJournalModel model, CancellationToken _) => savedJournal = model)
            .ReturnsAsync(journalWithLines);

        // Act
        await sut.ExecuteAsync(request);

        // Assert
        // New line should have SortOrder = 6 (max 5 + 1)
        Assert.That(savedJournal!.ApplyPostingLines, Has.Count.EqualTo(4));
        var newLine = savedJournal.ApplyPostingLines.FirstOrDefault(l => l.Identifier == request.Identifier);
        Assert.That(newLine, Is.Not.Null);
        Assert.That(newLine!.SortOrder, Is.EqualTo(6));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenExistingLinesHaveGaps_CalculatesMaxCorrectly()
    {
        // Arrange
        ApplyPostingJournalModel journalWithLines = CreatePostingJournal(lineCount: 3, sortOrders: new int[] { 1, 2, 10 });

        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest request = CreateRequest(
            postingReference: "REF",
            budgetAccount: "BUD",
            contactAccount: "CON",
            debit: 100m,
            credit: 0m);
        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalFeature sut = CreateSut(postingJournalModel: journalWithLines);

        ApplyPostingJournalModel? savedJournal = null;
        _accountingGatewayMock!
            .Setup(m => m.SavePostingJournalAsync(It.IsAny<int>(), It.IsAny<ApplyPostingJournalModel>(), It.IsAny<CancellationToken>()))
            .Callback((int _, ApplyPostingJournalModel model, CancellationToken _) => savedJournal = model)
            .ReturnsAsync(journalWithLines);

        // Act
        await sut.ExecuteAsync(request);

        // Assert
        // New line should have SortOrder = 11 (max 10 + 1)
        var newLine = savedJournal!.ApplyPostingLines.FirstOrDefault(l => l.Identifier == request.Identifier);
        Assert.That(newLine!.SortOrder, Is.EqualTo(11));
    }

    #endregion

    #region Line Collection Management Tests

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AddsNewLineToExistingCollection()
    {
        // Arrange
        ApplyPostingJournalModel journalWithLines = CreatePostingJournal(lineCount: 2);
        int initialLineCount = journalWithLines.ApplyPostingLines!.Count;

        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest request = CreateRequest();
        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalFeature sut = CreateSut(postingJournalModel: journalWithLines);

        ApplyPostingJournalModel? savedJournal = null;
        _accountingGatewayMock!
            .Setup(m => m.SavePostingJournalAsync(It.IsAny<int>(), It.IsAny<ApplyPostingJournalModel>(), It.IsAny<CancellationToken>()))
            .Callback((int _, ApplyPostingJournalModel model, CancellationToken _) => savedJournal = model)
            .ReturnsAsync(journalWithLines);

        // Act
        await sut.ExecuteAsync(request);

        // Assert
        Assert.That(savedJournal!.ApplyPostingLines, Has.Count.EqualTo(initialLineCount + 1));
        Assert.That(savedJournal.ApplyPostingLines.Any(l => l.Identifier == request.Identifier), Is.True);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenJournalApplyPostingLinesIsEmpty_AddsLineToCollection()
    {
        // Arrange
        ApplyPostingJournalModel journalWithEmptyLines = new ApplyPostingJournalModel(
            _fixture!.Create<int>(),
            new List<ApplyPostingLineModel>());

        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest request = CreateRequest(
            postingReference: "REF",
            budgetAccount: "BUD",
            contactAccount: "CON",
            debit: 100m,
            credit: 0m);
        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalFeature sut = CreateSut(postingJournalModel: journalWithEmptyLines);

        ApplyPostingJournalModel? savedJournal = null;
        _accountingGatewayMock!
            .Setup(m => m.SavePostingJournalAsync(It.IsAny<int>(), It.IsAny<ApplyPostingJournalModel>(), It.IsAny<CancellationToken>()))
            .Callback((int _, ApplyPostingJournalModel model, CancellationToken _) => savedJournal = model)
            .ReturnsAsync(journalWithEmptyLines);

        // Act & Assert
        Assert.DoesNotThrowAsync(async () => await sut.ExecuteAsync(request));
        Assert.That(savedJournal!.ApplyPostingLines, Has.Count.EqualTo(1));
        Assert.That(savedJournal.ApplyPostingLines.First().Identifier, Is.EqualTo(request.Identifier));
    }

    #endregion

    #region Cloning and Sorting Tests

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_ReturnsNewJournalInstance()
    {
        // Arrange
        ApplyPostingJournalModel originalJournal = CreatePostingJournal();
        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest request = CreateRequest();
        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalFeature sut = CreateSut(postingJournalModel: originalJournal);

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
        ApplyPostingJournalModel originalJournal = CreatePostingJournal(accountingNumber: expectedAccountingNumber);
        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest request = CreateRequest();
        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalFeature sut = CreateSut(postingJournalModel: originalJournal);

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
        var line1 = CreatePostingLine(postingDate: new DateTimeOffset(2024, 1, 15, 0, 0, 0, TimeSpan.Zero), sortOrder: 1);
        var line2 = CreatePostingLine(postingDate: new DateTimeOffset(2024, 1, 10, 0, 0, 0, TimeSpan.Zero), sortOrder: 2);
        var newLineDate = new DateTimeOffset(2024, 1, 01, 0, 0, 0, TimeSpan.Zero);

        ApplyPostingJournalModel journalWithLines = new ApplyPostingJournalModel(
            _fixture!.Create<int>(),
            new List<ApplyPostingLineModel> { line1, line2 });

        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest request = CreateRequest(
            postingDate: newLineDate,
            postingReference: "REF",
            budgetAccount: "BUD",
            contactAccount: "CON",
            debit: 100m,
            credit: 0m);
        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalFeature sut = CreateSut(postingJournalModel: journalWithLines);

        ApplyPostingJournalModel? savedJournal = null;
        _accountingGatewayMock!
            .Setup(m => m.SavePostingJournalAsync(It.IsAny<int>(), It.IsAny<ApplyPostingJournalModel>(), It.IsAny<CancellationToken>()))
            .Callback((int _, ApplyPostingJournalModel model, CancellationToken _) => savedJournal = model)
            .ReturnsAsync(journalWithLines);

        // Act
        await sut.ExecuteAsync(request);

        // Assert
        Assert.That(savedJournal!.ApplyPostingLines, Has.Count.EqualTo(3));
        // Lines should be sorted by posting date descending: 2024-01-15, 2024-01-10, 2024-01-01
        var lines = savedJournal.ApplyPostingLines!.ToList();
        Assert.That(lines[0].PostingDate.Date, Is.EqualTo(new DateTimeOffset(2024, 1, 15, 0, 0, 0, TimeSpan.Zero).Date));
        Assert.That(lines[1].PostingDate.Date, Is.EqualTo(new DateTimeOffset(2024, 1, 10, 0, 0, 0, TimeSpan.Zero).Date));
        Assert.That(lines[2].PostingDate.Date, Is.EqualTo(newLineDate.Date));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_SortsLinesByPostingDateThenSortOrderDescending()
    {
        // Arrange
        var line1 = CreatePostingLine(postingDate: new DateTimeOffset(2024, 1, 10, 0, 0, 0, TimeSpan.Zero), sortOrder: 1);
        var line2 = CreatePostingLine(postingDate: new DateTimeOffset(2024, 1, 10, 0, 0, 0, TimeSpan.Zero), sortOrder: 3);
        var newLineDate = new DateTimeOffset(2024, 1, 10, 0, 0, 0, TimeSpan.Zero);

        ApplyPostingJournalModel journalWithLines = new ApplyPostingJournalModel(
            _fixture!.Create<int>(),
            new List<ApplyPostingLineModel> { line1, line2 });

        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest request = CreateRequest(
            postingDate: newLineDate,
            postingReference: "REF",
            budgetAccount: "BUD",
            contactAccount: "CON",
            debit: 100m,
            credit: 0m);
        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalFeature sut = CreateSut(postingJournalModel: journalWithLines);

        ApplyPostingJournalModel? savedJournal = null;
        _accountingGatewayMock!
            .Setup(m => m.SavePostingJournalAsync(It.IsAny<int>(), It.IsAny<ApplyPostingJournalModel>(), It.IsAny<CancellationToken>()))
            .Callback((int _, ApplyPostingJournalModel model, CancellationToken _) => savedJournal = model)
            .ReturnsAsync(journalWithLines);

        // Act
        await sut.ExecuteAsync(request);

        // Assert
        Assert.That(savedJournal!.ApplyPostingLines, Has.Count.EqualTo(3));
        // All have same posting date, so sorted by SortOrder descending: 4 (new), 3 (line2), 1 (line1)
        var lines = savedJournal.ApplyPostingLines!.ToList();
        Assert.That(lines[0].SortOrder, Is.EqualTo(4));
        Assert.That(lines[1].SortOrder, Is.EqualTo(3));
        Assert.That(lines[2].SortOrder, Is.EqualTo(1));
    }

    #endregion

    #region Test Helpers

    private DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalFeature CreateSut(ApplyPostingJournalModel? postingJournalModel = null)
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
            .ReturnsAsync("The identifier already exists");

        return new DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalFeature(
            _permissionCheckerMock!.Object,
            _accountingGatewayMock.Object,
            _staticTextProviderMock.Object);
    }

    private DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest CreateRequest(
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
        ISecurityContext? securityContext = null)
    {
        return new DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest(
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