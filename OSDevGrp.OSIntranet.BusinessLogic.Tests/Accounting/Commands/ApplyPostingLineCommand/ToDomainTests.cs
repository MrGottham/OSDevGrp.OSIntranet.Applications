using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Commands.ApplyPostingLineCommand
{
    [TestFixture]
    public class ToDomainTests
    {
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenAccountingIsNull_ThrowsArgumentNullException()
        {
            // ReSharper disable UnusedVariable
            IApplyPostingLineCommand sut = CreateSut(out Mock<IAccounting> accountingMock);
            // ReSharper restore UnusedVariable

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ToDomain(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("accounting"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertAccountCollectionWasCalledOnAccounting()
        {
            IApplyPostingLineCommand sut = CreateSut(out Mock<IAccounting> accountingMock);

            sut.ToDomain(accountingMock.Object);

            accountingMock.Verify(m => m.AccountCollection, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertBudgetAccountCollectionWasCalledOnAccounting()
        {
            IApplyPostingLineCommand sut = CreateSut(out Mock<IAccounting> accountingMock);

            sut.ToDomain(accountingMock.Object);

            accountingMock.Verify(m => m.BudgetAccountCollection, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertContactAccountCollectionWasCalledOnAccounting()
        {
            IApplyPostingLineCommand sut = CreateSut(out Mock<IAccounting> accountingMock);

            sut.ToDomain(accountingMock.Object);

            accountingMock.Verify(m => m.ContactAccountCollection, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsNotNull()
        {
            IApplyPostingLineCommand sut = CreateSut(out Mock<IAccounting> accountingMock);

            IPostingLine result = sut.ToDomain(accountingMock.Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsPostingLine()
        {
            IApplyPostingLineCommand sut = CreateSut(out Mock<IAccounting> accountingMock);

            IPostingLine result = sut.ToDomain(accountingMock.Object);

            Assert.That(result, Is.TypeOf<PostingLine>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenApplyPostingLineCommandHasNoIdentifier_ReturnsPostingLineWhereIdentifierNotEqualToGuidEmpty()
        {
            IApplyPostingLineCommand sut = CreateSut(out Mock<IAccounting> accountingMock);

            IPostingLine result = sut.ToDomain(accountingMock.Object);

            Assert.That(result.Identifier, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenApplyPostingLineCommandHasIdentifier_ReturnsPostingLineWhereIdentifierEqualToIdentifierFromApplyPostingLineCommand()
        {
            Guid identifier = Guid.NewGuid();
            IApplyPostingLineCommand sut = CreateSut(out Mock<IAccounting> accountingMock, true, identifier);

            IPostingLine result = sut.ToDomain(accountingMock.Object);

            Assert.That(result.Identifier, Is.EqualTo(identifier));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsPostingLineWherePostingDateEqualToPostingDateFromApplyPostingLineCommand()
        {
            DateTime postingDate = DateTime.Today.AddDays(_random.Next(0, 30) * -1).AddMinutes(_random.Next(8 * 60, 16 * 60));
            IApplyPostingLineCommand sut = CreateSut(out Mock<IAccounting> accountingMock, postingDate: postingDate);

            IPostingLine result = sut.ToDomain(accountingMock.Object);

            Assert.That(result.PostingDate, Is.EqualTo(postingDate.Date));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenApplyPostingLineCommandHasNoReference_ReturnsPostingLineWhereReferenceEqualToNull()
        {
            IApplyPostingLineCommand sut = CreateSut(out Mock<IAccounting> accountingMock, hasReference: false);

            IPostingLine result = sut.ToDomain(accountingMock.Object);

            Assert.That(result.Reference, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenApplyPostingLineCommandHasReference_ReturnsPostingLineWhereReferenceNotEqualToNull()
        {
            IApplyPostingLineCommand sut = CreateSut(out Mock<IAccounting> accountingMock);

            IPostingLine result = sut.ToDomain(accountingMock.Object);

            Assert.That(result.Reference, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenApplyPostingLineCommandHasReference_ReturnsPostingLineWhereReferenceEqualToReferenceFromApplyPostingLineCommand()
        {
            string reference = _fixture.Create<string>();
            IApplyPostingLineCommand sut = CreateSut(out Mock<IAccounting> accountingMock, reference: reference);

            IPostingLine result = sut.ToDomain(accountingMock.Object);

            Assert.That(result.Reference, Is.EqualTo(reference));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsPostingLineWhereAccountNotEqualToNull()
        {
            IApplyPostingLineCommand sut = CreateSut(out Mock<IAccounting> accountingMock);

            IPostingLine result = sut.ToDomain(accountingMock.Object);

            Assert.That(result.Account, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsPostingLineWhereAccountMatchesAccountNumberFromApplyPostingLineCommand()
        {
            string accountNumber = _fixture.Create<string>();
            IApplyPostingLineCommand sut = CreateSut(out Mock<IAccounting> accountingMock, accountNumber: accountNumber);

            IPostingLine result = sut.ToDomain(accountingMock.Object);

            Assert.That(result.Account.AccountNumber, Is.EqualTo(accountNumber.ToUpper()));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsPostingLineWhereDetailsNotEqualToNull()
        {
            IApplyPostingLineCommand sut = CreateSut(out Mock<IAccounting> accountingMock);

            IPostingLine result = sut.ToDomain(accountingMock.Object);

            Assert.That(result.Details, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsPostingLineWhereDetailsEqualToDetailsFromApplyPostingLineCommand()
        {
            string details = _fixture.Create<string>();
            IApplyPostingLineCommand sut = CreateSut(out Mock<IAccounting> accountingMock, details: details);

            IPostingLine result = sut.ToDomain(accountingMock.Object);

            Assert.That(result.Details, Is.EqualTo(details));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalledApplyPostingLineCommandHasNoBudgetAccountNumber_ReturnsPostingLineWhereBudgetAccountEqualToNull()
        {
            IApplyPostingLineCommand sut = CreateSut(out Mock<IAccounting> accountingMock, hasBudgetAccountNumber: false);

            IPostingLine result = sut.ToDomain(accountingMock.Object);

            Assert.That(result.BudgetAccount, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalledApplyPostingLineCommandHasBudgetAccountNumber_ReturnsPostingLineWhereBudgetAccountNotEqualToNull()
        {
            IApplyPostingLineCommand sut = CreateSut(out Mock<IAccounting> accountingMock);

            IPostingLine result = sut.ToDomain(accountingMock.Object);

            Assert.That(result.BudgetAccount, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalledApplyPostingLineCommandHasBudgetAccountNumber_ReturnsPostingLineWhereBudgetAccountMatchesBudgetAccountNumberFromApplyPostingLineCommand()
        {
            string budgetAccountNumber = _fixture.Create<string>();
            IApplyPostingLineCommand sut = CreateSut(out Mock<IAccounting> accountingMock, budgetAccountNumber: budgetAccountNumber);

            IPostingLine result = sut.ToDomain(accountingMock.Object);

            Assert.That(result.BudgetAccount.AccountNumber, Is.EqualTo(budgetAccountNumber.ToUpper()));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalledApplyPostingLineCommandHasNoDebit_ReturnsPostingLineWhereDebitEqualToZero()
        {
            IApplyPostingLineCommand sut = CreateSut(out Mock<IAccounting> accountingMock, hasDebit: false);

            IPostingLine result = sut.ToDomain(accountingMock.Object);

            Assert.That(result.Debit, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalledApplyPostingLineCommandHasDebit_ReturnsPostingLineWhereDebitEqualToDebitFromApplyPostingLineCommand()
        {
            decimal debit = _fixture.Create<decimal>();
            IApplyPostingLineCommand sut = CreateSut(out Mock<IAccounting> accountingMock, debit: debit);

            IPostingLine result = sut.ToDomain(accountingMock.Object);

            Assert.That(result.Debit, Is.EqualTo(debit));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalledApplyPostingLineCommandHasNoCredit_ReturnsPostingLineWhereCreditEqualToZero()
        {
            IApplyPostingLineCommand sut = CreateSut(out Mock<IAccounting> accountingMock, hasCredit: false);

            IPostingLine result = sut.ToDomain(accountingMock.Object);

            Assert.That(result.Credit, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalledApplyPostingLineCommandHasCredit_ReturnsPostingLineWhereCreditEqualToCreditFromApplyPostingLineCommand()
        {
            decimal credit = _fixture.Create<decimal>();
            IApplyPostingLineCommand sut = CreateSut(out Mock<IAccounting> accountingMock, credit: credit);

            IPostingLine result = sut.ToDomain(accountingMock.Object);

            Assert.That(result.Credit, Is.EqualTo(credit));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalledApplyPostingLineCommandHasNoContactAccountNumber_ReturnsPostingLineWhereContactAccountEqualToNull()
        {
            IApplyPostingLineCommand sut = CreateSut(out Mock<IAccounting> accountingMock, hasContactAccountNumber: false);

            IPostingLine result = sut.ToDomain(accountingMock.Object);

            Assert.That(result.ContactAccount, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalledApplyPostingLineCommandHasContactAccountNumber_ReturnsPostingLineWhereContactAccountNotEqualToNull()
        {
            IApplyPostingLineCommand sut = CreateSut(out Mock<IAccounting> accountingMock);

            IPostingLine result = sut.ToDomain(accountingMock.Object);

            Assert.That(result.ContactAccount, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalledApplyPostingLineCommandHasContactAccountNumber_ReturnsPostingLineWhereContactAccountMatchesContactAccountNumberFromApplyPostingLineCommand()
        {
            string contactAccountNumber = _fixture.Create<string>();
            IApplyPostingLineCommand sut = CreateSut(out Mock<IAccounting> accountingMock, contactAccountNumber: contactAccountNumber);

            IPostingLine result = sut.ToDomain(accountingMock.Object);

            Assert.That(result.ContactAccount.AccountNumber, Is.EqualTo(contactAccountNumber.ToUpper()));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsPostingLineWhereSortOrderEqualToSortOrderFromApplyPostingLineCommand()
        {
            int sortOrder = _fixture.Create<int>();
            IApplyPostingLineCommand sut = CreateSut(out Mock<IAccounting> accountingMock, sortOrder: sortOrder);

            IPostingLine result = sut.ToDomain(accountingMock.Object);

            Assert.That(result.SortOrder, Is.EqualTo(sortOrder));
        }

        private IApplyPostingLineCommand CreateSut(out Mock<IAccounting> accountingMock, bool hasIdentifier = false, Guid? identifier = null, DateTime? postingDate = null, bool hasReference = true, string reference = null, string accountNumber = null, string details = null, bool hasBudgetAccountNumber = true, string budgetAccountNumber = null, bool hasDebit = true, decimal? debit = null, bool hasCredit = true, decimal? credit = null, bool hasContactAccountNumber = true, string contactAccountNumber = null, int? sortOrder = null)
        {
            IAccounting emptyAccounting = _fixture.BuildAccountingMock(isEmpty: true).Object;

            IAccount[] accountCollection = BuildAccounts(emptyAccounting, accountNumber).ToArray();
            IBudgetAccount[] budgetAccountCollection = BuildBudgetAccounts(emptyAccounting, budgetAccountNumber).ToArray();
            IContactAccount[] contactAccountCollection = BuildContactAccounts(emptyAccounting, contactAccountNumber).ToArray();
            accountingMock = _fixture.BuildAccountingMock(
                accountCollection: _fixture.BuildAccountCollectionMock(accountCollection: accountCollection).Object,
                budgetAccountCollection: _fixture.BuildBudgetAccountCollectionMock(budgetAccountCollection: budgetAccountCollection).Object,
                contactAccountCollection: _fixture.BuildContactAccountCollectionMock(contactAccountCollection: contactAccountCollection).Object);

            return _fixture.Build<BusinessLogic.Accounting.Commands.ApplyPostingLineCommand>()
                .With(m => m.Identifier, hasIdentifier ? identifier ?? Guid.NewGuid() : null)
                .With(m => m.PostingDate, postingDate ?? DateTime.Today.AddDays(_random.Next(0, 30) * -1))
                .With(m => m.Reference, hasReference ? reference ?? _fixture.Create<string>() : null)
                .With(m => m.AccountNumber, (accountNumber ?? accountCollection[_random.Next(0, accountCollection.Length - 1)].AccountNumber).ToUpper)
                .With(m => m.Details, details ?? _fixture.Create<string>())
                .With(m => m.BudgetAccountNumber, hasBudgetAccountNumber ? (budgetAccountNumber ?? budgetAccountCollection[_random.Next(0, budgetAccountCollection.Length - 1)].AccountNumber).ToUpper() : null)
                .With(m => m.Debit, hasDebit ? debit ?? _fixture.Create<decimal>() : null)
                .With(m => m.Credit, hasCredit ? credit ?? _fixture.Create<decimal>() : null)
                .With(m => m.ContactAccountNumber, hasContactAccountNumber ? (contactAccountNumber ?? contactAccountCollection[_random.Next(0, contactAccountCollection.Length - 1)].AccountNumber).ToUpper() : null)
                .With(m => m.SortOrder, sortOrder ?? _fixture.Create<int>())
                .Create();
        }

        private IEnumerable<IAccount> BuildAccounts(IAccounting accounting, string accountNumber)
        {
            NullGuard.NotNull(accounting, nameof(accounting));

            return new[]
            {
                _fixture.BuildAccountMock(accounting, isEmpty: true).Object,
                _fixture.BuildAccountMock(accounting, isEmpty: true).Object,
                _fixture.BuildAccountMock(accounting, isEmpty: true).Object,
                _fixture.BuildAccountMock(accounting, string.IsNullOrWhiteSpace(accountNumber) ? null : accountNumber.ToUpper(), isEmpty: true).Object,
                _fixture.BuildAccountMock(accounting, isEmpty: true).Object,
                _fixture.BuildAccountMock(accounting, isEmpty: true).Object,
                _fixture.BuildAccountMock(accounting, isEmpty: true).Object
            };
        }

        private IEnumerable<IBudgetAccount> BuildBudgetAccounts(IAccounting accounting, string budgetAccountNumber)
        {
            NullGuard.NotNull(accounting, nameof(accounting));

            return new[]
            {
                _fixture.BuildBudgetAccountMock(accounting, isEmpty: true).Object,
                _fixture.BuildBudgetAccountMock(accounting, isEmpty: true).Object,
                _fixture.BuildBudgetAccountMock(accounting, isEmpty: true).Object,
                _fixture.BuildBudgetAccountMock(accounting, string.IsNullOrWhiteSpace(budgetAccountNumber) ? null : budgetAccountNumber.ToUpper(), isEmpty: true).Object,
                _fixture.BuildBudgetAccountMock(accounting, isEmpty: true).Object,
                _fixture.BuildBudgetAccountMock(accounting, isEmpty: true).Object,
                _fixture.BuildBudgetAccountMock(accounting, isEmpty: true).Object
            };
        }

        private IEnumerable<IContactAccount> BuildContactAccounts(IAccounting accounting, string contactAccountNumber)
        {
            NullGuard.NotNull(accounting, nameof(accounting));

            return new[]
            {
                _fixture.BuildContactAccountMock(accounting, isEmpty: true).Object,
                _fixture.BuildContactAccountMock(accounting, isEmpty: true).Object,
                _fixture.BuildContactAccountMock(accounting, isEmpty: true).Object,
                _fixture.BuildContactAccountMock(accounting, string.IsNullOrWhiteSpace(contactAccountNumber) ? null : contactAccountNumber.ToUpper(), isEmpty: true).Object,
                _fixture.BuildContactAccountMock(accounting, isEmpty: true).Object,
                _fixture.BuildContactAccountMock(accounting, isEmpty: true).Object,
                _fixture.BuildContactAccountMock(accounting, isEmpty: true).Object
            };
        }
    }
}