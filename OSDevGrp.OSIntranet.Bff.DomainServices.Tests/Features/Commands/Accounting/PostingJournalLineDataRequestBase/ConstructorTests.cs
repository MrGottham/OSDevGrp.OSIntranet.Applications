using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Commands.Accounting.PostingJournalLineDataRequestBase;

[TestFixture]
public class ConstructorTests
{
    #region Private variables

    private Fixture _fixture = null!;

    #endregion

    #region Setup

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
    }

    #endregion

    #region Tests

    [Test]
    [Category("UnitTest")]
    public void Constructor_WhenCalled_AssertPostingDateIsSet()
    {
        // Arrange
        Guid requestId = _fixture.Create<Guid>();
        int accountingNumber = _fixture.Create<int>();
        Guid identifier = _fixture.Create<Guid>();
        DateTimeOffset postingDate = _fixture.Create<DateTimeOffset>();
        string? postingReference = _fixture.Create<string?>();
        string account = _fixture.Create<string>();
        string postingText = _fixture.Create<string>();
        string? budgetAccount = _fixture.Create<string?>();
        decimal? debit = _fixture.Create<decimal?>();
        decimal? credit = _fixture.Create<decimal?>();
        string? contactAccount = _fixture.Create<string?>();
        ISecurityContext securityContext = _fixture.CreateSecurityContext();

        // Act
        TestPostingJournalLineDataRequest sut = new TestPostingJournalLineDataRequest(
            requestId,
            accountingNumber,
            identifier,
            postingDate,
            postingReference,
            account,
            postingText,
            budgetAccount,
            debit,
            credit,
            contactAccount,
            securityContext);

        // Assert
        Assert.That(sut.PostingDate, Is.EqualTo(postingDate));
    }

    [Test]
    [Category("UnitTest")]
    public void Constructor_WhenCalled_AssertPostingReferenceIsSet()
    {
        // Arrange
        Guid requestId = _fixture.Create<Guid>();
        int accountingNumber = _fixture.Create<int>();
        Guid identifier = _fixture.Create<Guid>();
        DateTimeOffset postingDate = _fixture.Create<DateTimeOffset>();
        string? postingReference = _fixture.Create<string?>();
        string account = _fixture.Create<string>();
        string postingText = _fixture.Create<string>();
        string? budgetAccount = _fixture.Create<string?>();
        decimal? debit = _fixture.Create<decimal?>();
        decimal? credit = _fixture.Create<decimal?>();
        string? contactAccount = _fixture.Create<string?>();
        ISecurityContext securityContext = _fixture.CreateSecurityContext();

        // Act
        TestPostingJournalLineDataRequest sut = new TestPostingJournalLineDataRequest(
            requestId,
            accountingNumber,
            identifier,
            postingDate,
            postingReference,
            account,
            postingText,
            budgetAccount,
            debit,
            credit,
            contactAccount,
            securityContext);

        // Assert
        Assert.That(sut.PostingReference, Is.EqualTo(postingReference));
    }

    [Test]
    [Category("UnitTest")]
    public void Constructor_WhenCalled_AssertAccountIsSet()
    {
        // Arrange
        Guid requestId = _fixture.Create<Guid>();
        int accountingNumber = _fixture.Create<int>();
        Guid identifier = _fixture.Create<Guid>();
        DateTimeOffset postingDate = _fixture.Create<DateTimeOffset>();
        string? postingReference = _fixture.Create<string?>();
        string account = _fixture.Create<string>();
        string postingText = _fixture.Create<string>();
        string? budgetAccount = _fixture.Create<string?>();
        decimal? debit = _fixture.Create<decimal?>();
        decimal? credit = _fixture.Create<decimal?>();
        string? contactAccount = _fixture.Create<string?>();
        ISecurityContext securityContext = _fixture.CreateSecurityContext();

        // Act
        TestPostingJournalLineDataRequest sut = new TestPostingJournalLineDataRequest(
            requestId,
            accountingNumber,
            identifier,
            postingDate,
            postingReference,
            account,
            postingText,
            budgetAccount,
            debit,
            credit,
            contactAccount,
            securityContext);

        // Assert
        Assert.That(sut.Account, Is.EqualTo(account));
    }

    [Test]
    [Category("UnitTest")]
    public void Constructor_WhenCalled_AssertPostingTextIsSet()
    {
        // Arrange
        Guid requestId = _fixture.Create<Guid>();
        int accountingNumber = _fixture.Create<int>();
        Guid identifier = _fixture.Create<Guid>();
        DateTimeOffset postingDate = _fixture.Create<DateTimeOffset>();
        string? postingReference = _fixture.Create<string?>();
        string account = _fixture.Create<string>();
        string postingText = _fixture.Create<string>();
        string? budgetAccount = _fixture.Create<string?>();
        decimal? debit = _fixture.Create<decimal?>();
        decimal? credit = _fixture.Create<decimal?>();
        string? contactAccount = _fixture.Create<string?>();
        ISecurityContext securityContext = _fixture.CreateSecurityContext();

        // Act
        TestPostingJournalLineDataRequest sut = new TestPostingJournalLineDataRequest(
            requestId,
            accountingNumber,
            identifier,
            postingDate,
            postingReference,
            account,
            postingText,
            budgetAccount,
            debit,
            credit,
            contactAccount,
            securityContext);

        // Assert
        Assert.That(sut.PostingText, Is.EqualTo(postingText));
    }

    [Test]
    [Category("UnitTest")]
    public void Constructor_WhenCalled_AssertBudgetAccountIsSet()
    {
        // Arrange
        Guid requestId = _fixture.Create<Guid>();
        int accountingNumber = _fixture.Create<int>();
        Guid identifier = _fixture.Create<Guid>();
        DateTimeOffset postingDate = _fixture.Create<DateTimeOffset>();
        string? postingReference = _fixture.Create<string?>();
        string account = _fixture.Create<string>();
        string postingText = _fixture.Create<string>();
        string? budgetAccount = _fixture.Create<string?>();
        decimal? debit = _fixture.Create<decimal?>();
        decimal? credit = _fixture.Create<decimal?>();
        string? contactAccount = _fixture.Create<string?>();
        ISecurityContext securityContext = _fixture.CreateSecurityContext();

        // Act
        TestPostingJournalLineDataRequest sut = new TestPostingJournalLineDataRequest(
            requestId,
            accountingNumber,
            identifier,
            postingDate,
            postingReference,
            account,
            postingText,
            budgetAccount,
            debit,
            credit,
            contactAccount,
            securityContext);

        // Assert
        Assert.That(sut.BudgetAccount, Is.EqualTo(budgetAccount));
    }

    [Test]
    [Category("UnitTest")]
    public void Constructor_WhenCalled_AssertDebitIsSet()
    {
        // Arrange
        Guid requestId = _fixture.Create<Guid>();
        int accountingNumber = _fixture.Create<int>();
        Guid identifier = _fixture.Create<Guid>();
        DateTimeOffset postingDate = _fixture.Create<DateTimeOffset>();
        string? postingReference = _fixture.Create<string?>();
        string account = _fixture.Create<string>();
        string postingText = _fixture.Create<string>();
        string? budgetAccount = _fixture.Create<string?>();
        decimal? debit = _fixture.Create<decimal?>();
        decimal? credit = _fixture.Create<decimal?>();
        string? contactAccount = _fixture.Create<string?>();
        ISecurityContext securityContext = _fixture.CreateSecurityContext();

        // Act
        TestPostingJournalLineDataRequest sut = new TestPostingJournalLineDataRequest(
            requestId,
            accountingNumber,
            identifier,
            postingDate,
            postingReference,
            account,
            postingText,
            budgetAccount,
            debit,
            credit,
            contactAccount,
            securityContext);

        // Assert
        Assert.That(sut.Debit, Is.EqualTo(debit));
    }

    [Test]
    [Category("UnitTest")]
    public void Constructor_WhenCalled_AssertCreditIsSet()
    {
        // Arrange
        Guid requestId = _fixture.Create<Guid>();
        int accountingNumber = _fixture.Create<int>();
        Guid identifier = _fixture.Create<Guid>();
        DateTimeOffset postingDate = _fixture.Create<DateTimeOffset>();
        string? postingReference = _fixture.Create<string?>();
        string account = _fixture.Create<string>();
        string postingText = _fixture.Create<string>();
        string? budgetAccount = _fixture.Create<string?>();
        decimal? debit = _fixture.Create<decimal?>();
        decimal? credit = _fixture.Create<decimal?>();
        string? contactAccount = _fixture.Create<string?>();
        ISecurityContext securityContext = _fixture.CreateSecurityContext();

        // Act
        TestPostingJournalLineDataRequest sut = new TestPostingJournalLineDataRequest(
            requestId,
            accountingNumber,
            identifier,
            postingDate,
            postingReference,
            account,
            postingText,
            budgetAccount,
            debit,
            credit,
            contactAccount,
            securityContext);

        // Assert
        Assert.That(sut.Credit, Is.EqualTo(credit));
    }

    [Test]
    [Category("UnitTest")]
    public void Constructor_WhenCalled_AssertContactAccountIsSet()
    {
        // Arrange
        Guid requestId = _fixture.Create<Guid>();
        int accountingNumber = _fixture.Create<int>();
        Guid identifier = _fixture.Create<Guid>();
        DateTimeOffset postingDate = _fixture.Create<DateTimeOffset>();
        string? postingReference = _fixture.Create<string?>();
        string account = _fixture.Create<string>();
        string postingText = _fixture.Create<string>();
        string? budgetAccount = _fixture.Create<string?>();
        decimal? debit = _fixture.Create<decimal?>();
        decimal? credit = _fixture.Create<decimal?>();
        string? contactAccount = _fixture.Create<string?>();
        ISecurityContext securityContext = _fixture.CreateSecurityContext();

        // Act
        TestPostingJournalLineDataRequest sut = new TestPostingJournalLineDataRequest(
            requestId,
            accountingNumber,
            identifier,
            postingDate,
            postingReference,
            account,
            postingText,
            budgetAccount,
            debit,
            credit,
            contactAccount,
            securityContext);

        // Assert
        Assert.That(sut.ContactAccount, Is.EqualTo(contactAccount));
    }

    [Test]
    [Category("UnitTest")]
    public void Constructor_IsPostingJournalLineIdentificationRequestBase()
    {
        // Arrange
        Guid requestId = _fixture.Create<Guid>();
        int accountingNumber = _fixture.Create<int>();
        Guid identifier = _fixture.Create<Guid>();
        DateTimeOffset postingDate = _fixture.Create<DateTimeOffset>();
        string? postingReference = _fixture.Create<string?>();
        string account = _fixture.Create<string>();
        string postingText = _fixture.Create<string>();
        string? budgetAccount = _fixture.Create<string?>();
        decimal? debit = _fixture.Create<decimal?>();
        decimal? credit = _fixture.Create<decimal?>();
        string? contactAccount = _fixture.Create<string?>();
        ISecurityContext securityContext = _fixture.CreateSecurityContext();

        // Act
        TestPostingJournalLineDataRequest sut = new TestPostingJournalLineDataRequest(
            requestId,
            accountingNumber,
            identifier,
            postingDate,
            postingReference,
            account,
            postingText,
            budgetAccount,
            debit,
            credit,
            contactAccount,
            securityContext);

        // Assert
        Assert.That(sut, Is.InstanceOf<DomainServices.Features.Commands.Accounting.PostingJournalLineIdentificationRequestBase>());
    }

    #endregion

    #region Nested classes

    private class TestPostingJournalLineDataRequest : DomainServices.Features.Commands.Accounting.PostingJournalLineDataRequestBase
    {
        public TestPostingJournalLineDataRequest(
            Guid requestId,
            int accountingNumber,
            Guid identifier,
            DateTimeOffset postingDate,
            string? postingReference,
            string account,
            string postingText,
            string? budgetAccount,
            decimal? debit,
            decimal? credit,
            string? contactAccount,
            ISecurityContext securityContext)
            : base(
                requestId,
                accountingNumber,
                identifier,
                postingDate,
                postingReference,
                account,
                postingText,
                budgetAccount,
                debit,
                credit,
                contactAccount,
                securityContext)
        {
        }
    }

    #endregion
}