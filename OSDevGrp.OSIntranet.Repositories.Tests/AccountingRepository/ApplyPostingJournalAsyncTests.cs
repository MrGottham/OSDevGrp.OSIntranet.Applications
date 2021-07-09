using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    [TestFixture]
    public class ApplyPostingJournalAsyncTests : AccountingRepositoryTestBase
    {
        #region Private variables

        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyPostingJournalAsync_WhenPostingJournalIsNull_ThrowsArgumentNullException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ApplyPostingJournalAsync(null, _fixture.BuildPostingWarningCalculatorMock().Object));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("postingJournal"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyPostingJournalAsync_WhenPostingWarningCalculatorIsNull_ThrowsArgumentNullException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ApplyPostingJournalAsync(_fixture.BuildPostingJournalMock().Object, null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("postingWarningCalculator"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("IntegrationTest")]
        [Ignore("Test which applies a posting journal and should only be run once")]
        public async Task ApplyPostingJournalAsync_WhenCalled_ReturnsNonEmptyPostingJournalResult()
        {
            IAccountingRepository sut = CreateSut();

            IPostingJournal postingJournal = await BuildPostingJournalAsync(sut);
            IPostingWarningCalculator postingWarningCalculator = await BuildPostingWarningCalculator();
            IPostingJournalResult result = await sut.ApplyPostingJournalAsync(postingJournal, postingWarningCalculator);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.PostingLineCollection, Is.Not.Null);
            Assert.That(result.PostingLineCollection, Is.Not.Empty);
            Assert.That(result.PostingLineCollection.Count(), Is.EqualTo(postingJournal.PostingLineCollection.Count()));
            Assert.That(result.PostingWarningCollection, Is.Not.Null);
            Assert.That(result.PostingWarningCollection, Is.Empty);
        }

        private async Task<IPostingJournal> BuildPostingJournalAsync(IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            IAccounting accounting = await accountingRepository.GetAccountingAsync(WithExistingAccountingNumber(), DateTime.Today);
            IAccount primaryAccount = accounting.AccountCollection.Single(account => string.CompareOrdinal(account.AccountNumber, WithExistingAccountNumberForAccount()) == 0);
            IBudgetAccount primaryBudgetAccount = accounting.BudgetAccountCollection.Single(budgetAccount => string.CompareOrdinal(budgetAccount.AccountNumber, WithExistingAccountNumberForBudgetAccount()) == 0);

            IPostingLineCollection postingLineCollection = new PostingLineCollection
            {
                new PostingLine(Guid.NewGuid(), DateTime.Today, null, primaryAccount, "Testing apply posting journal", primaryBudgetAccount, 25000M, 0M, null, 0),
                new PostingLine(Guid.NewGuid(), DateTime.Today, null, primaryAccount, "Testing apply posting journal", primaryBudgetAccount, 0M, 25000M, null, 0)
            };

            return new PostingJournal(postingLineCollection);
        }

        private Task<IPostingWarningCalculator> BuildPostingWarningCalculator()
        {
            return Task.FromResult<IPostingWarningCalculator>(new PostingWarningCalculator());
        }
    }
}