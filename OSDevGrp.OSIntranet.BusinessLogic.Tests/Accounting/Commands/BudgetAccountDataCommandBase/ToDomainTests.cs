using System;
using System.Collections.Generic;
using System.Linq;
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
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _accountingRepositoryMock = new Mock<IAccountingRepository>();

            _fixture = new Fixture();
            _fixture.Customize<IBudgetInfoCommand>(builder => builder.FromFactory(() => CreateBudgetInfoCommand()));

            _random = new Random(_fixture.Create<int>());
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
        public void ToDomain_WhenCalled_AssertToDomainWasCalledOnEachBudgetInfoCommandInBudgetInfoCollection()
        {
            Mock<IBudgetInfoCommand>[] budgetInfoCommandMockCollection =
            {
                CreateBudgetInfoCommandMock(),
                CreateBudgetInfoCommandMock(),
                CreateBudgetInfoCommandMock()
            };
            IBudgetAccountDataCommand sut = CreateSut(budgetInfoCommandCollection: budgetInfoCommandMockCollection.Select(budgetInfoCommandMock => budgetInfoCommandMock.Object).ToArray());

            sut.ToDomain(_accountingRepositoryMock.Object);

            foreach (Mock<IBudgetInfoCommand> budgetInfoCommandMock in budgetInfoCommandMockCollection)
            {
                budgetInfoCommandMock.Verify(m => m.ToDomain(It.IsNotNull<IBudgetAccount>()), Times.Once);
            }
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

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsBudgetAccountWhereBudgetInfoCollectionIsNotNull()
        {
            IBudgetAccountDataCommand sut = CreateSut();

            IBudgetAccount budgetAccount = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(budgetAccount.BudgetInfoCollection, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsBudgetAccountWhereBudgetInfoCollectionIsNotEmpty()
        {
            IBudgetAccountDataCommand sut = CreateSut(budgetInfoCommandCollection: _fixture.CreateMany<IBudgetInfoCommand>(_random.Next(5, 10)).ToArray());

            IBudgetAccount budgetAccount = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(budgetAccount.BudgetInfoCollection, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsBudgetAccountWhereBudgetInfoCollectionContainsAllBudgetInfoFromBudgetInfoCollectionOnBudgetAccountDataCommand()
        {
            IBudgetInfo[] budgetInfoCollection =
            {
                _fixture.BuildBudgetInfoMock().Object,
                _fixture.BuildBudgetInfoMock().Object,
                _fixture.BuildBudgetInfoMock().Object,
                _fixture.BuildBudgetInfoMock().Object,
                _fixture.BuildBudgetInfoMock().Object
            };
            IBudgetAccountDataCommand sut = CreateSut(budgetInfoCommandCollection: budgetInfoCollection.Select(CreateBudgetInfoCommand).ToArray());

            IBudgetAccount budgetAccount = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(budgetInfoCollection.All(budgetInfo => budgetAccount.BudgetInfoCollection.Contains(budgetInfo)), Is.True);
        }

        private IBudgetAccountDataCommand CreateSut(int? accountingNumber = null, IAccounting accounting = null, string accountNumber = null, string accountName = null, bool hasDescription = true, string description = null, bool hasNote = true, string note = null, int? budgetAccountGroupNumber = null, IBudgetAccountGroup budgetAccountGroup = null, IEnumerable<IBudgetInfoCommand> budgetInfoCommandCollection = null)
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
                .With(m => m.BudgetInfoCollection, budgetInfoCommandCollection ?? _fixture.CreateMany<IBudgetInfoCommand>(_random.Next(5, 10)).ToArray())
                .Create();
        }

        private IBudgetInfoCommand CreateBudgetInfoCommand(IBudgetInfo budgetInfo = null)
        {
            return CreateBudgetInfoCommandMock(budgetInfo).Object;
        }

        private Mock<IBudgetInfoCommand> CreateBudgetInfoCommandMock(IBudgetInfo budgetInfo = null)
        {
            Mock<IBudgetInfoCommand> budgetInfoCommandMock = new Mock<IBudgetInfoCommand>();
            budgetInfoCommandMock.Setup(m => m.ToDomain(It.IsAny<IBudgetAccount>()))
                .Returns(budgetInfo ?? _fixture.BuildBudgetInfoMock().Object);
            return budgetInfoCommandMock;
        }

        private class Sut : BusinessLogic.Accounting.Commands.BudgetAccountDataCommandBase
        {
        }
    }
}