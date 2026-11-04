using NUnit.Framework;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using JournalCloner = OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Accounting.PostingJournalCloner;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Accounting.PostingJournalCloner;

[TestFixture]
public class SortAndClonePostingJournalTests
{
    #region Tests

    [Test]
    [Category("UnitTest")]
    public void SortAndClonePostingJournal_WhenCalled_KeepsAccountingNumber()
    {
        // Arrange
        ApplyPostingJournalModel existingJournal = CreateJournal(42);

        // Act
        ApplyPostingJournalModel result = SortAndClone(existingJournal, []);

        // Assert
        Assert.That(result.AccountingNumber, Is.EqualTo(42));
    }

    [Test]
    [Category("UnitTest")]
    public void SortAndClonePostingJournal_WhenCalled_ReturnsNewJournalInstance()
    {
        // Arrange
        ApplyPostingJournalModel existingJournal = CreateJournal(42);

        // Act
        ApplyPostingJournalModel result = SortAndClone(existingJournal, []);

        // Assert
        Assert.That(result, Is.Not.SameAs(existingJournal));
    }

    [Test]
    [Category("UnitTest")]
    public void SortAndClonePostingJournal_WhenCalledWithNoLines_ReturnsJournalWithoutLines()
    {
        // Arrange
        ApplyPostingJournalModel existingJournal = CreateJournal(42);

        // Act
        ApplyPostingJournalModel result = SortAndClone(existingJournal, []);

        // Assert
        Assert.That(result.ApplyPostingLines, Is.Empty);
    }

    [Test]
    [Category("UnitTest")]
    public void SortAndClonePostingJournal_WhenPostingDatesDiffer_SortsLinesByPostingDateDescending()
    {
        // Arrange
        ApplyPostingLineModel oldest = CreateLine(new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero), 1);
        ApplyPostingLineModel newest = CreateLine(new DateTimeOffset(2026, 3, 1, 0, 0, 0, TimeSpan.Zero), 1);
        ApplyPostingLineModel middle = CreateLine(new DateTimeOffset(2026, 2, 1, 0, 0, 0, TimeSpan.Zero), 1);
        ApplyPostingJournalModel existingJournal = CreateJournal(42);

        // Act
        ApplyPostingJournalModel result = SortAndClone(existingJournal, [oldest, newest, middle]);

        // Assert
        Assert.That(result.ApplyPostingLines, Is.EqualTo(new[] { newest, middle, oldest }));
    }

    [Test]
    [Category("UnitTest")]
    public void SortAndClonePostingJournal_WhenPostingDatesAreEqual_SortsLinesBySortOrderDescending()
    {
        // Arrange
        DateTimeOffset postingDate = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        ApplyPostingLineModel first = CreateLine(postingDate, 1);
        ApplyPostingLineModel last = CreateLine(postingDate, 3);
        ApplyPostingLineModel middle = CreateLine(postingDate, 2);
        ApplyPostingJournalModel existingJournal = CreateJournal(42);

        // Act
        ApplyPostingJournalModel result = SortAndClone(existingJournal, [first, last, middle]);

        // Assert
        Assert.That(result.ApplyPostingLines, Is.EqualTo(new[] { last, middle, first }));
    }

    [Test]
    [Category("UnitTest")]
    public void SortAndClonePostingJournal_WhenSortOrderIsNull_TreatsItAsZero()
    {
        // Arrange
        DateTimeOffset postingDate = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        ApplyPostingLineModel withoutSortOrder = CreateLine(postingDate, null);
        ApplyPostingLineModel withSortOrder = CreateLine(postingDate, 1);
        ApplyPostingJournalModel existingJournal = CreateJournal(42);

        // Act
        ApplyPostingJournalModel result = SortAndClone(existingJournal, [withoutSortOrder, withSortOrder]);

        // Assert
        Assert.That(result.ApplyPostingLines, Is.EqualTo(new[] { withSortOrder, withoutSortOrder }));
    }

    [Test]
    [Category("UnitTest")]
    public void SortAndClonePostingJournal_WhenExistingJournalIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        ApplyPostingJournalModel? existingJournal = null;

        // Act & Assert
        Assert.That(
            () => SortAndClone(existingJournal!, []),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("existingJournal"));
    }

    [Test]
    [Category("UnitTest")]
    public void SortAndClonePostingJournal_WhenUpdatedLinesIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        ApplyPostingJournalModel existingJournal = CreateJournal(42);

        // Act & Assert
        Assert.That(
            () => SortAndClone(existingJournal, null!),
            Throws.ArgumentNullException.With.Property("ParamName").EqualTo("updatedLines"));
    }

    #endregion

    #region Test Helpers

    private static ApplyPostingJournalModel SortAndClone(ApplyPostingJournalModel existingJournal, IEnumerable<ApplyPostingLineModel> updatedLines)
    {
        return JournalCloner.SortAndClonePostingJournal(existingJournal, updatedLines);
    }

    private static ApplyPostingJournalModel CreateJournal(int accountingNumber)
    {
        return new ApplyPostingJournalModel(accountingNumber, []);
    }

    private static ApplyPostingLineModel CreateLine(DateTimeOffset postingDate, int? sortOrder)
    {
        return new ApplyPostingLineModel(
            "1000",
            null,
            null,
            null,
            100,
            "Details",
            null,
            postingDate,
            null,
            sortOrder);
    }

    #endregion
}