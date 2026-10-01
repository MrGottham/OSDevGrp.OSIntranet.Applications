using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Commands.Accounting.AppendPostingLineToPostingJournal.AppendPostingLineToPostingJournalRequest;

[TestFixture]
public class ConstructorTests
{
    #region Private variables

    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    public void Constructor_WhenCalled_AssertAccountingNumberIsSet()
    {
        // Arrange
        int expectedAccountingNumber = _fixture!.Create<int>();
        ISecurityContext securityContext = CreateSecurityContext();

        // Act
        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest request = new DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest(
            _fixture.Create<Guid>(),
            expectedAccountingNumber,
            _fixture.Create<Guid>(),
            _fixture.Create<DateTimeOffset>(),
            _fixture.Create<string>(),
            "1000",
            "Test posting",
            _fixture.Create<string>(),
            100m,
            0m,
            _fixture.Create<string>(),
            securityContext);

        // Assert
        Assert.That(request.AccountingNumber, Is.EqualTo(expectedAccountingNumber));
    }

    [Test]
    [Category("UnitTest")]
    public void Constructor_WhenCalled_AssertIdentifierIsSet()
    {
        // Arrange
        Guid expectedIdentifier = _fixture!.Create<Guid>();
        ISecurityContext securityContext = CreateSecurityContext();

        // Act
        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest request = new DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest(
            _fixture.Create<Guid>(),
            _fixture.Create<int>(),
            expectedIdentifier,
            _fixture.Create<DateTimeOffset>(),
            _fixture.Create<string>(),
            "1000",
            "Test posting",
            _fixture.Create<string>(),
            100m,
            0m,
            _fixture.Create<string>(),
            securityContext);

        // Assert
        Assert.That(request.Identifier, Is.EqualTo(expectedIdentifier));
    }

    [Test]
    [Category("UnitTest")]
    public void Constructor_WhenCalled_AssertPostingDateIsSet()
    {
        // Arrange
        DateTimeOffset expectedPostingDate = _fixture!.Create<DateTimeOffset>();
        ISecurityContext securityContext = CreateSecurityContext();

        // Act
        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest request = new DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest(
            _fixture.Create<Guid>(),
            _fixture.Create<int>(),
            _fixture.Create<Guid>(),
            expectedPostingDate,
            _fixture.Create<string>(),
            "1000",
            "Test posting",
            _fixture.Create<string>(),
            100m,
            0m,
            _fixture.Create<string>(),
            securityContext);

        // Assert
        Assert.That(request.PostingDate, Is.EqualTo(expectedPostingDate));
    }

    [Test]
    [Category("UnitTest")]
    public void Constructor_WhenCalled_AssertPostingReferenceIsSet()
    {
        // Arrange
        string expectedPostingReference = "REF123";
        ISecurityContext securityContext = CreateSecurityContext();

        // Act
        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest request = new DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest(
            _fixture!.Create<Guid>(),
            _fixture.Create<int>(),
            _fixture.Create<Guid>(),
            _fixture.Create<DateTimeOffset>(),
            expectedPostingReference,
            "1000",
            "Test posting",
            _fixture.Create<string>(),
            100m,
            0m,
            _fixture.Create<string>(),
            securityContext);

        // Assert
        Assert.That(request.PostingReference, Is.EqualTo(expectedPostingReference));
    }

    [Test]
    [Category("UnitTest")]
    public void Constructor_WhenCalled_AssertAccountIsSet()
    {
        // Arrange
        string expectedAccount = "1000";
        ISecurityContext securityContext = CreateSecurityContext();

        // Act
        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest request = new DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest(
            _fixture!.Create<Guid>(),
            _fixture.Create<int>(),
            _fixture.Create<Guid>(),
            _fixture.Create<DateTimeOffset>(),
            _fixture.Create<string>(),
            expectedAccount,
            "Test posting",
            _fixture.Create<string>(),
            100m,
            0m,
            _fixture.Create<string>(),
            securityContext);

        // Assert
        Assert.That(request.Account, Is.EqualTo(expectedAccount));
    }

    [Test]
    [Category("UnitTest")]
    public void Constructor_WhenCalled_AssertPostingTextIsSet()
    {
        // Arrange
        string expectedPostingText = "Test posting";
        ISecurityContext securityContext = CreateSecurityContext();

        // Act
        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest request = new DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest(
            _fixture!.Create<Guid>(),
            _fixture.Create<int>(),
            _fixture.Create<Guid>(),
            _fixture.Create<DateTimeOffset>(),
            _fixture.Create<string>(),
            "1000",
            expectedPostingText,
            _fixture.Create<string>(),
            100m,
            0m,
            _fixture.Create<string>(),
            securityContext);

        // Assert
        Assert.That(request.PostingText, Is.EqualTo(expectedPostingText));
    }

    [Test]
    [Category("UnitTest")]
    public void Constructor_WhenCalled_AssertBudgetAccountIsSet()
    {
        // Arrange
        string expectedBudgetAccount = "2000";
        ISecurityContext securityContext = CreateSecurityContext();

        // Act
        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest request = new DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest(
            _fixture!.Create<Guid>(),
            _fixture.Create<int>(),
            _fixture.Create<Guid>(),
            _fixture.Create<DateTimeOffset>(),
            _fixture.Create<string>(),
            "1000",
            "Test posting",
            expectedBudgetAccount,
            100m,
            0m,
            _fixture.Create<string>(),
            securityContext);

        // Assert
        Assert.That(request.BudgetAccount, Is.EqualTo(expectedBudgetAccount));
    }

    [Test]
    [Category("UnitTest")]
    public void Constructor_WhenCalled_AssertDebitIsSet()
    {
        // Arrange
        decimal expectedDebit = 500m;
        ISecurityContext securityContext = CreateSecurityContext();

        // Act
        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest request = new DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest(
            _fixture!.Create<Guid>(),
            _fixture.Create<int>(),
            _fixture.Create<Guid>(),
            _fixture.Create<DateTimeOffset>(),
            _fixture.Create<string>(),
            "1000",
            "Test posting",
            _fixture.Create<string>(),
            expectedDebit,
            0m,
            _fixture.Create<string>(),
            securityContext);

        // Assert
        Assert.That(request.Debit, Is.EqualTo(expectedDebit));
    }

    [Test]
    [Category("UnitTest")]
    public void Constructor_WhenCalled_AssertCreditIsSet()
    {
        // Arrange
        decimal expectedCredit = 250m;
        ISecurityContext securityContext = CreateSecurityContext();

        // Act
        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest request = new DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest(
            _fixture!.Create<Guid>(),
            _fixture.Create<int>(),
            _fixture.Create<Guid>(),
            _fixture.Create<DateTimeOffset>(),
            _fixture.Create<string>(),
            "1000",
            "Test posting",
            _fixture.Create<string>(),
            0m,
            expectedCredit,
            _fixture.Create<string>(),
            securityContext);

        // Assert
        Assert.That(request.Credit, Is.EqualTo(expectedCredit));
    }

    [Test]
    [Category("UnitTest")]
    public void Constructor_WhenCalled_AssertContactAccountIsSet()
    {
        // Arrange
        string expectedContactAccount = "3000";
        ISecurityContext securityContext = CreateSecurityContext();

        // Act
        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest request = new DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest(
            _fixture!.Create<Guid>(),
            _fixture.Create<int>(),
            _fixture.Create<Guid>(),
            _fixture.Create<DateTimeOffset>(),
            _fixture.Create<string>(),
            "1000",
            "Test posting",
            _fixture.Create<string>(),
            100m,
            0m,
            expectedContactAccount,
            securityContext);

        // Assert
        Assert.That(request.ContactAccount, Is.EqualTo(expectedContactAccount));
    }

    [Test]
    [Category("UnitTest")]
    public void Constructor_WhenCalled_AssertSecurityContextIsSet()
    {
        // Arrange
        ISecurityContext expectedSecurityContext = CreateSecurityContext();

        // Act
        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest request = new DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest(
            _fixture!.Create<Guid>(),
            _fixture.Create<int>(),
            _fixture.Create<Guid>(),
            _fixture.Create<DateTimeOffset>(),
            _fixture.Create<string>(),
            "1000",
            "Test posting",
            _fixture.Create<string>(),
            100m,
            0m,
            _fixture.Create<string>(),
            expectedSecurityContext);

        // Assert
        Assert.That(request.SecurityContext, Is.SameAs(expectedSecurityContext));
    }

    [Test]
    [Category("UnitTest")]
    public void Constructor_WhenCalled_AssertRequestIdIsSet()
    {
        // Arrange
        Guid expectedRequestId = _fixture!.Create<Guid>();
        ISecurityContext securityContext = CreateSecurityContext();

        // Act
        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest request = new DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest(
            expectedRequestId,
            _fixture.Create<int>(),
            _fixture.Create<Guid>(),
            _fixture.Create<DateTimeOffset>(),
            _fixture.Create<string>(),
            "1000",
            "Test posting",
            _fixture.Create<string>(),
            100m,
            0m,
            _fixture.Create<string>(),
            securityContext);

        // Assert
        Assert.That(request.RequestId, Is.EqualTo(expectedRequestId));
    }

    [Test]
    [Category("UnitTest")]
    public void Constructor_WhenCalledWithNullOptionalProperties_PreservesNullValues()
    {
        // Arrange
        ISecurityContext securityContext = CreateSecurityContext();

        // Act
        DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest request = new DomainServices.Features.Commands.Accounting.AppendPostingLineToPostingJournalRequest(
            _fixture!.Create<Guid>(),
            _fixture.Create<int>(),
            _fixture.Create<Guid>(),
            _fixture.Create<DateTimeOffset>(),
            null,
            "1000",
            "Test posting",
            null,
            null,
            null,
            null,
            securityContext);

        // Assert
        Assert.That(request.PostingReference, Is.Null);
        Assert.That(request.BudgetAccount, Is.Null);
        Assert.That(request.Debit, Is.Null);
        Assert.That(request.Credit, Is.Null);
        Assert.That(request.ContactAccount, Is.Null);
    }

    #region Test Helpers

    private ISecurityContext CreateSecurityContext()
    {
        Mock<ISecurityContext> securityContextMock = new Mock<ISecurityContext>();
        return securityContextMock.Object;
    }

    #endregion
}