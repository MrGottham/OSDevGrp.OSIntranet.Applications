using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Commands.BudgetAccountDataCommandBase
{
    [TestFixture]
    public class ToDomainTests
    {
        #region Private variables

        private Mock<IAccountingRepository> _accountingRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _accountingRepositoryMock = new Mock<IAccountingRepository>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenAccountingRepositoryIsNull_ThrowsArgumentNullException()
        {
            IBudgetAccountDataCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ToDomain(null));

            Assert.That(result.ParamName, Is.EqualTo("accountingRepository"));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertGetAccountingAsyncWasCalledOnAccountingRepository()
        {
            int accountingNumber = _fixture.Create<int>();
            IBudgetAccountDataCommand sut = CreateSut(accountingNumber);

            sut.ToDomain(_accountingRepositoryMock.Object);

            _accountingRepositoryMock.Verify(m => m.GetAccountingAsync(
                    It.Is<int>(value => value == accountingNumber),
                    It.Is<DateTime>(value => value == DateTime.Today)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertGetBudgetAccountGroupAsyncWasCalledOnAccountingRepository()
        {
            int budgetAccountGroupNumber = _fixture.Create<int>();
            IBudgetAccountDataCommand sut = CreateSut(budgetAccountGroupNumber: budgetAccountGroupNumber);

            sut.ToDomain(_accountingRepositoryMock.Object);

            _accountingRepositoryMock.Verify(m => m.GetBudgetAccountGroupAsync(It.Is<int>(value => value == budgetAccountGroupNumber)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsBudgetAccount()
        {
            IBudgetAccountDataCommand sut = CreateSut();

            IBudgetAccount budgetAccount = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(budgetAccount, Is.TypeOf<BudgetAccount>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsBudgetAccountWhereAccountingIsEqualToAccountingFromAccountingRepository()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IBudgetAccountDataCommand sut = CreateSut(accounting: accounting);

            IBudgetAccount budgetAccount = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(budgetAccount.Accounting, Is.EqualTo(accounting));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsBudgetAccountWhereAccountNumberIsEqualToAccountNumberFromBudgetAccountDataCommand()
        {
            string accountNumber = _fixture.Create<string>().ToUpper();
            IBudgetAccountDataCommand sut = CreateSut(accountNumber: accountNumber);

            IBudgetAccount budgetAccount = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(budgetAccount.AccountNumber, Is.EqualTo(accountNumber));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsBudgetAccountWhereAccountNameIsEqualToAccountNameFromBudgetAccountDataCommand()
        {
            string accountName = _fixture.Create<string>();
            IBudgetAccountDataCommand sut = CreateSut(accountName: accountName);

            IBudgetAccount budgetAccount = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(budgetAccount.AccountName, Is.EqualTo(accountName));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenDescriptionWasNotGivenInBudgetAccountDataCommand_ReturnsBudgetAccountWhereDescriptionIsNull()
        {
            IBudgetAccountDataCommand sut = CreateSut(hasDescription: false);

            IBudgetAccount budgetAccount = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(budgetAccount.Description, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenDescriptionWasGivenInBudgetAccountDataCommand_ReturnsBudgetAccountWhereDescriptionIsEqualToDescriptionFromBudgetAccountDataCommand()
        {
            string description = _fixture.Create<string>();
            IBudgetAccountDataCommand sut = CreateSut(description: description);

            IBudgetAccount budgetAccount = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(budgetAccount.Description, Is.EqualTo(description));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenNoteWasNotGivenInBudgetAccountDataCommand_ReturnsBudgetAccountWhereNoteIsNull()
        {
            IBudgetAccountDataCommand sut = CreateSut(hasNote: false);

            IBudgetAccount budgetAccount = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(budgetAccount.Note, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenNoteWasGivenInBudgetAccountDataCommand_ReturnsBudgetAccountWhereNoteIsEqualToNoteFromBudgetAccountDataCommand()
        {
            string note = _fixture.Create<string>();
            IBudgetAccountDataCommand sut = CreateSut(note: note);

            IBudgetAccount budgetAccount = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(budgetAccount.Note, Is.EqualTo(note));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsBudgetAccountWhereBudgetAccountGroupIsEqualToBudgetAccountGroupFromAccountingRepository()
        {
            IBudgetAccountGroup budgetAccountGroup = _fixture.BuildBudgetAccountGroupMock().Object;
            IBudgetAccountDataCommand sut = CreateSut(budgetAccountGroup: budgetAccountGroup);

            IBudgetAccount budgetAccount = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(budgetAccount.BudgetAccountGroup, Is.EqualTo(budgetAccountGroup));
        }

        private IBudgetAccountDataCommand CreateSut(int? accountingNumber = null, IAccounting accounting = null, string accountNumber = null, string accountName = null, bool hasDescription = true, string description = null, bool hasNote = true, string note = null, int? budgetAccountGroupNumber = null, IBudgetAccountGroup budgetAccountGroup = null)
        {
            _accountingRepositoryMock.Setup(m => m.GetAccountingAsync(It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(accounting ?? _fixture.BuildAccountingMock().Object));
            _accountingRepositoryMock.Setup(m => m.GetBudgetAccountGroupAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(budgetAccountGroup ?? _fixture.BuildBudgetAccountGroupMock().Object));

            return _fixture.Build<Sut>()
                .With(m => m.AccountingNumber, accountingNumber ?? _fixture.Create<int>())
                .With(m => m.AccountNumber, accountNumber ?? _fixture.Create<string>().ToUpper())
                .With(m => m.AccountName, accountName ?? _fixture.Create<string>())
                .With(m => m.Description, hasDescription ? description ?? _fixture.Create<string>() : null)
                .With(m => m.Note, hasNote ? note ?? _fixture.Create<string>() : null)
                .With(m => m.BudgetAccountGroupNumber, budgetAccountGroupNumber ?? _fixture.Create<int>())
                .Create();
        }

        private class Sut : BusinessLogic.Accounting.Commands.BudgetAccountDataCommandBase
        {
        }
    }
}