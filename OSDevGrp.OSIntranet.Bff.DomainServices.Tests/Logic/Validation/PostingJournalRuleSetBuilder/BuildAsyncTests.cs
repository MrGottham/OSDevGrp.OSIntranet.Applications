using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MaxLengthRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MaxValueRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MinLengthRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MinValueRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.PatternRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.RequiredValueRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.ExtendedValidationRuleSetBuilder;
using System.Globalization;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.AccountingNumberRuleSetBuilder;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.PostingJournalLineIdentifierRuleSetBuilder;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.PostingDateRuleSetBuilder;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.PostingReferenceRuleSetBuilder;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.AccountNumberRuleSetBuilder;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.PostingTextRuleSetBuilder;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.BudgetAccountNumberRuleSetBuilder;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.DebitRuleSetBuilder;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.CreditRuleSetBuilder;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.ContactAccountNumberRuleSetBuilder;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.PostingJournalRuleSetBuilder;

[TestFixture]
public class BuildAsyncTests
{
    #region Private variables

    private Mock<IExtendedValidationRuleSetBuilder>? _extendedValidationRuleSetBuilderMock;
    private Mock<IAccountingNumberRuleSetBuilder>? _accountingNumberRuleSetBuilderMock;
    private Mock<IPostingJournalLineIdentifierRuleSetBuilder>? _postingJournalLineIdentifierRuleSetBuilderMock;
    private Mock<IPostingDateRuleSetBuilder>? _postingDateRuleSetBuilderMock;
    private Mock<IPostingReferenceRuleSetBuilder>? _postingReferenceRuleSetBuilderMock;
    private Mock<IAccountNumberRuleSetBuilder>? _accountNumberRuleSetBuilderMock;
    private Mock<IPostingTextRuleSetBuilder>? _postingTextRuleSetBuilderMock;
    private Mock<IBudgetAccountNumberRuleSetBuilder>? _budgetAccountNumberRuleSetBuilderMock;
    private Mock<IDebitRuleSetBuilder>? _debitRuleSetBuilderMock;
    private Mock<ICreditRuleSetBuilder>? _creditRuleSetBuilderMock;
    private Mock<IContactAccountNumberRuleSetBuilder>? _contactAccountNumberRuleSetBuilderMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _extendedValidationRuleSetBuilderMock = new Mock<IExtendedValidationRuleSetBuilder>();
        _accountingNumberRuleSetBuilderMock = new Mock<IAccountingNumberRuleSetBuilder>();
        _postingJournalLineIdentifierRuleSetBuilderMock = new Mock<IPostingJournalLineIdentifierRuleSetBuilder>();
        _postingDateRuleSetBuilderMock = new Mock<IPostingDateRuleSetBuilder>();
        _postingReferenceRuleSetBuilderMock = new Mock<IPostingReferenceRuleSetBuilder>();
        _accountNumberRuleSetBuilderMock = new Mock<IAccountNumberRuleSetBuilder>();
        _postingTextRuleSetBuilderMock = new Mock<IPostingTextRuleSetBuilder>();
        _budgetAccountNumberRuleSetBuilderMock = new Mock<IBudgetAccountNumberRuleSetBuilder>();
        _debitRuleSetBuilderMock = new Mock<IDebitRuleSetBuilder>();
        _creditRuleSetBuilderMock = new Mock<ICreditRuleSetBuilder>();
        _contactAccountNumberRuleSetBuilderMock = new Mock<IContactAccountNumberRuleSetBuilder>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnAccountingNumberRuleSetBuilderWithGivenFormatProvider()
    {
        IPostingJournalRuleSetBuilder sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.BuildAsync(formatProvider);

        _accountingNumberRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnAccountingNumberRuleSetBuilderWithGivenCancellationToken()
    {
        IPostingJournalRuleSetBuilder sut = CreateSut();

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.BuildAsync(CultureInfo.InvariantCulture, cancellationToken);

        _accountingNumberRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnPostingJournalLineIdentifierRuleSetBuilderWithGivenFormatProvider()
    {
        IPostingJournalRuleSetBuilder sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.BuildAsync(formatProvider);

        _postingJournalLineIdentifierRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnPostingJournalLineIdentifierRuleSetBuilderWithGivenCancellationToken()
    {
        IPostingJournalRuleSetBuilder sut = CreateSut();

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.BuildAsync(CultureInfo.InvariantCulture, cancellationToken);

        _postingJournalLineIdentifierRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnPostingDateRuleSetBuilderWithGivenFormatProvider()
    {
        IPostingJournalRuleSetBuilder sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.BuildAsync(formatProvider);

        _postingDateRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnPostingDateRuleSetBuilderWithGivenCancellationToken()
    {
        IPostingJournalRuleSetBuilder sut = CreateSut();

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.BuildAsync(CultureInfo.InvariantCulture, cancellationToken);

        _postingDateRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnPostingReferenceRuleSetBuilderWithGivenFormatProvider()
    {
        IPostingJournalRuleSetBuilder sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.BuildAsync(formatProvider);

        _postingReferenceRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnPostingReferenceRuleSetBuilderWithGivenCancellationToken()
    {
        IPostingJournalRuleSetBuilder sut = CreateSut();

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.BuildAsync(CultureInfo.InvariantCulture, cancellationToken);

        _postingReferenceRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnAccountNumberRuleSetBuilderWithGivenFormatProvider()
    {
        IPostingJournalRuleSetBuilder sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.BuildAsync(formatProvider);

        _accountNumberRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnAccountNumberRuleSetBuilderWithGivenCancellationToken()
    {
        IPostingJournalRuleSetBuilder sut = CreateSut();

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.BuildAsync(CultureInfo.InvariantCulture, cancellationToken);

        _accountNumberRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnPostingTextRuleSetBuilderWithGivenFormatProvider()
    {
        IPostingJournalRuleSetBuilder sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.BuildAsync(formatProvider);

        _postingTextRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnPostingTextRuleSetBuilderWithGivenCancellationToken()
    {
        IPostingJournalRuleSetBuilder sut = CreateSut();

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.BuildAsync(CultureInfo.InvariantCulture, cancellationToken);

        _postingTextRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnBudgetAccountNumberRuleSetBuilderWithGivenFormatProvider()
    {
        IPostingJournalRuleSetBuilder sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.BuildAsync(formatProvider);

        _budgetAccountNumberRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnBudgetAccountNumberRuleSetBuilderWithGivenCancellationToken()
    {
        IPostingJournalRuleSetBuilder sut = CreateSut();

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.BuildAsync(CultureInfo.InvariantCulture, cancellationToken);

        _budgetAccountNumberRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnDebitRuleSetBuilderWithGivenFormatProvider()
    {
        IPostingJournalRuleSetBuilder sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.BuildAsync(formatProvider);

        _debitRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnDebitRuleSetBuilderWithGivenCancellationToken()
    {
        IPostingJournalRuleSetBuilder sut = CreateSut();

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.BuildAsync(CultureInfo.InvariantCulture, cancellationToken);

        _debitRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnCreditRuleSetBuilderWithGivenFormatProvider()
    {
        IPostingJournalRuleSetBuilder sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.BuildAsync(formatProvider);

        _creditRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnCreditRuleSetBuilderWithGivenCancellationToken()
    {
        IPostingJournalRuleSetBuilder sut = CreateSut();

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.BuildAsync(CultureInfo.InvariantCulture, cancellationToken);

        _creditRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnContactAccountNumberRuleSetBuilderWithGivenFormatProvider()
    {
        IPostingJournalRuleSetBuilder sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.BuildAsync(formatProvider);

        _contactAccountNumberRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_AssertBuildAsyncWasCalledOnContactAccountNumberRuleSetBuilderWithGivenCancellationToken()
    {
        IPostingJournalRuleSetBuilder sut = CreateSut();

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.BuildAsync(CultureInfo.InvariantCulture, cancellationToken);

        _contactAccountNumberRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsNonEmptyValidationRuleSet()
    {
        IPostingJournalRuleSetBuilder sut = CreateSut();

        IReadOnlyCollection<IValidationRule> result = await sut.BuildAsync(CultureInfo.InvariantCulture);

        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsDistinctValidationRules()
    {
        IPostingJournalRuleSetBuilder sut = CreateSut();

        IReadOnlyCollection<IValidationRule> result = await sut.BuildAsync(CultureInfo.InvariantCulture);

        int validationRuleCount = result.Count;
        int distinctValidationRuleCount = result.DistinctBy(validationRule => validationRule.Name).Count();
        Assert.That(validationRuleCount, Is.EqualTo(distinctValidationRuleCount));
    }

    private IPostingJournalRuleSetBuilder CreateSut()
    {
        _extendedValidationRuleSetBuilderMock!.Setup(_fixture!);
        _accountingNumberRuleSetBuilderMock!.Setup(_fixture!);
        _postingJournalLineIdentifierRuleSetBuilderMock!.Setup(_fixture!);
        _postingDateRuleSetBuilderMock!.Setup(_fixture!);
        _postingReferenceRuleSetBuilderMock!.Setup(_fixture!);
        _accountNumberRuleSetBuilderMock!.Setup(_fixture!);
        _postingTextRuleSetBuilderMock!.Setup(_fixture!);
        _budgetAccountNumberRuleSetBuilderMock!.Setup(_fixture!);
        _debitRuleSetBuilderMock!.Setup(_fixture!);
        _creditRuleSetBuilderMock!.Setup(_fixture!);
        _contactAccountNumberRuleSetBuilderMock!.Setup(_fixture!);

        return new DomainServices.Logic.Validation.PostingJournalRuleSetBuilder(
            _extendedValidationRuleSetBuilderMock!.Object,
            _accountingNumberRuleSetBuilderMock!.Object,
            _postingJournalLineIdentifierRuleSetBuilderMock!.Object,
            _postingDateRuleSetBuilderMock!.Object,
            _postingReferenceRuleSetBuilderMock!.Object,
            _accountNumberRuleSetBuilderMock!.Object,
            _postingTextRuleSetBuilderMock!.Object,
            _budgetAccountNumberRuleSetBuilderMock!.Object,
            _debitRuleSetBuilderMock!.Object,
            _creditRuleSetBuilderMock!.Object,
            _contactAccountNumberRuleSetBuilderMock!.Object);
    }
}