using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Accounting;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Commands.Accounting.PostingJournalLineIdentificationRequestBase;

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
    public void Constructor_WhenCalled_AssertIdentifierIsSet()
    {
        // Arrange
        Guid requestId = _fixture.Create<Guid>();
        int accountingNumber = _fixture.Create<int>();
        Guid identifier = _fixture.Create<Guid>();
        ISecurityContext securityContext = _fixture.CreateSecurityContext();

        // Act
        TestPostingJournalLineIdentificationRequest sut = new TestPostingJournalLineIdentificationRequest(
            requestId,
            accountingNumber,
            identifier,
            securityContext);

        // Assert
        Assert.That(sut.Identifier, Is.EqualTo(identifier));
    }

    [Test]
    [Category("UnitTest")]
    public void Constructor_WhenCalled_AssertAccountingNumberIsInherited()
    {
        // Arrange
        Guid requestId = _fixture.Create<Guid>();
        int accountingNumber = _fixture.Create<int>();
        Guid identifier = _fixture.Create<Guid>();
        ISecurityContext securityContext = _fixture.CreateSecurityContext();

        // Act
        TestPostingJournalLineIdentificationRequest sut = new TestPostingJournalLineIdentificationRequest(
            requestId,
            accountingNumber,
            identifier,
            securityContext);

        // Assert
        Assert.That(sut.AccountingNumber, Is.EqualTo(accountingNumber));
    }

    [Test]
    [Category("UnitTest")]
    public void Constructor_WhenCalled_AssertRequestIdIsInherited()
    {
        // Arrange
        Guid requestId = _fixture.Create<Guid>();
        int accountingNumber = _fixture.Create<int>();
        Guid identifier = _fixture.Create<Guid>();
        ISecurityContext securityContext = _fixture.CreateSecurityContext();

        // Act
        TestPostingJournalLineIdentificationRequest sut = new TestPostingJournalLineIdentificationRequest(
            requestId,
            accountingNumber,
            identifier,
            securityContext);

        // Assert
        Assert.That(sut.RequestId, Is.EqualTo(requestId));
    }

    [Test]
    [Category("UnitTest")]
    public void Constructor_IsAccountingIdentificationRequestBase()
    {
        // Arrange
        Guid requestId = _fixture.Create<Guid>();
        int accountingNumber = _fixture.Create<int>();
        Guid identifier = _fixture.Create<Guid>();
        ISecurityContext securityContext = _fixture.CreateSecurityContext();

        // Act
        TestPostingJournalLineIdentificationRequest sut = new TestPostingJournalLineIdentificationRequest(
            requestId,
            accountingNumber,
            identifier,
            securityContext);

        // Assert
        Assert.That(sut, Is.InstanceOf<AccountingIdentificationRequestBase>());
    }

    #endregion

    #region Nested classes

    private class TestPostingJournalLineIdentificationRequest : DomainServices.Features.Commands.Accounting.PostingJournalLineIdentificationRequestBase
    {
        public TestPostingJournalLineIdentificationRequest(
            Guid requestId,
            int accountingNumber,
            Guid identifier,
            ISecurityContext securityContext)
            : base(requestId, accountingNumber, identifier, securityContext)
        {
        }
    }

    #endregion
}