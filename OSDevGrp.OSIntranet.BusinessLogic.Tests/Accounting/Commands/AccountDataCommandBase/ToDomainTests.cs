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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Commands.AccountDataCommandBase
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
            _fixture.Customize<ICreditInfoCommand>(builder => builder.FromFactory(() => CreateCreditInfoCommand()));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenAccountingRepositoryIsNull_ThrowsArgumentNullException()
        {
            IAccountDataCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ToDomain(null));

            Assert.That(result.ParamName, Is.EqualTo("accountingRepository"));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertGetAccountingAsyncWasCalledOnAccountingRepository()
        {
            int accountingNumber = _fixture.Create<int>();
            IAccountDataCommand sut = CreateSut(accountingNumber);

            sut.ToDomain(_accountingRepositoryMock.Object);

            _accountingRepositoryMock.Verify(m => m.GetAccountingAsync(
                    It.Is<int>(value => value == accountingNumber),
                    It.Is<DateTime>(value => value == DateTime.Today)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertGetAccountGroupAsyncWasCalledOnAccountingRepository()
        {
            int accountGroupNumber = _fixture.Create<int>();
            IAccountDataCommand sut = CreateSut(accountGroupNumber: accountGroupNumber);

            sut.ToDomain(_accountingRepositoryMock.Object);

            _accountingRepositoryMock.Verify(m => m.GetAccountGroupAsync(It.Is<int>(value => value == accountGroupNumber)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertToDomainWasCalledOnEachCreditInfoCommandInCreditInfoCollection()
        {
            Mock<ICreditInfoCommand>[] creditInfoCommandMockCollection =
            {
                CreateCreditInfoCommandMock(),
                CreateCreditInfoCommandMock(),
                CreateCreditInfoCommandMock()
            };
            IAccountDataCommand sut = CreateSut(creditInfoCommandCollection: creditInfoCommandMockCollection.Select(creditInfoCommandMock => creditInfoCommandMock.Object).ToArray());

            sut.ToDomain(_accountingRepositoryMock.Object);

            foreach (Mock<ICreditInfoCommand> creditInfoCommandMock in creditInfoCommandMockCollection)
            {
                creditInfoCommandMock.Verify(m => m.ToDomain(It.IsNotNull<IAccount>()), Times.Once());
            }
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsAccount()
        {
            IAccountDataCommand sut = CreateSut();

            IAccount account = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(account, Is.TypeOf<Account>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsAccountWhereAccountingIsEqualToAccountingFromAccountingRepository()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccountDataCommand sut = CreateSut(accounting: accounting);

            IAccount account = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(account.Accounting, Is.EqualTo(accounting));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsAccountWhereAccountNumberIsEqualToAccountNumberFromAccountDataCommand()
        {
            string accountNumber = _fixture.Create<string>().ToUpper();
            IAccountDataCommand sut = CreateSut(accountNumber: accountNumber);

            IAccount account = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(account.AccountNumber, Is.EqualTo(accountNumber));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsAccountWhereAccountNameIsEqualToAccountNameFromAccountDataCommand()
        {
            string accountName = _fixture.Create<string>();
            IAccountDataCommand sut = CreateSut(accountName: accountName);

            IAccount account = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(account.AccountName, Is.EqualTo(accountName));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenDescriptionWasNotGivenInAccountDataCommand_ReturnsAccountWhereDescriptionIsNull()
        {
            IAccountDataCommand sut = CreateSut(hasDescription: false);

            IAccount account = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(account.Description, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenDescriptionWasGivenInAccountDataCommand_ReturnsAccountWhereDescriptionIsEqualToDescriptionFromAccountDataCommand()
        {
            string description = _fixture.Create<string>();
            IAccountDataCommand sut = CreateSut(description: description);

            IAccount account = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(account.Description, Is.EqualTo(description));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenNoteWasNotGivenInAccountDataCommand_ReturnsAccountWhereNoteIsNull()
        {
            IAccountDataCommand sut = CreateSut(hasNote: false);

            IAccount account = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(account.Note, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenNoteWasGivenInAccountDataCommand_ReturnsAccountWhereNoteIsEqualToNoteFromAccountDataCommand()
        {
            string note = _fixture.Create<string>();
            IAccountDataCommand sut = CreateSut(note: note);

            IAccount account = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(account.Note, Is.EqualTo(note));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsAccountWhereAccountGroupIsEqualToAccountGroupFromAccountingRepository()
        {
            IAccountGroup accountGroup = _fixture.BuildAccountGroupMock().Object;
            IAccountDataCommand sut = CreateSut(accountGroup: accountGroup);

            IAccount account = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(account.AccountGroup, Is.EqualTo(accountGroup));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsAccountWhereCreditInfoCollectionIsNotNull()
        {
            IAccountDataCommand sut = CreateSut();

            IAccount account = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(account.CreditInfoCollection, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsAccountWhereCreditInfoCollectionIsNotEmpty()
        {
            IAccountDataCommand sut = CreateSut(creditInfoCommandCollection: _fixture.CreateMany<ICreditInfoCommand>(_random.Next(5, 10)).ToArray());

            IAccount account = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(account.CreditInfoCollection, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsAccountWhereCreditInfoCollectionContainsAllCreditInfoFromCreditInfoCollectionOnAccountDataCommand()
        {
            ICreditInfo[] creditInfoCollection =
            {
                _fixture.BuildCreditInfoMock().Object,
                _fixture.BuildCreditInfoMock().Object,
                _fixture.BuildCreditInfoMock().Object,
                _fixture.BuildCreditInfoMock().Object,
                _fixture.BuildCreditInfoMock().Object
            };
            IAccountDataCommand sut = CreateSut(creditInfoCommandCollection: creditInfoCollection.Select(CreateCreditInfoCommand).ToArray());

            IAccount account = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(creditInfoCollection.All(creditInfo => account.CreditInfoCollection.Contains(creditInfo)), Is.True);
        }

        private IAccountDataCommand CreateSut(int? accountingNumber = null, IAccounting accounting = null, string accountNumber = null, string accountName = null, bool hasDescription = true, string description = null, bool hasNote = true, string note = null, int? accountGroupNumber = null, IAccountGroup accountGroup = null, IEnumerable<ICreditInfoCommand> creditInfoCommandCollection = null)
        {
            _accountingRepositoryMock.Setup(m => m.GetAccountingAsync(It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(accounting ?? _fixture.BuildAccountingMock().Object));
            _accountingRepositoryMock.Setup(m => m.GetAccountGroupAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(accountGroup ?? _fixture.BuildAccountGroupMock().Object));

            return _fixture.Build<Sut>()
                .With(m => m.AccountingNumber, accountingNumber ?? _fixture.Create<int>())
                .With(m => m.AccountNumber, accountNumber ?? _fixture.Create<string>().ToUpper())
                .With(m => m.AccountName, accountName ?? _fixture.Create<string>())
                .With(m => m.Description, hasDescription ? description ?? _fixture.Create<string>() : null)
                .With(m => m.Note, hasNote ? note ?? _fixture.Create<string>() : null)
                .With(m => m.AccountGroupNumber, accountGroupNumber ?? _fixture.Create<int>())
                .With(m => m.CreditInfoCollection, creditInfoCommandCollection ?? _fixture.CreateMany<ICreditInfoCommand>(_random.Next(5, 10)).ToArray())
                .Create();
        }

        private ICreditInfoCommand CreateCreditInfoCommand(ICreditInfo creditInfo = null)
        {
            return CreateCreditInfoCommandMock(creditInfo).Object;
        }

        private Mock<ICreditInfoCommand> CreateCreditInfoCommandMock(ICreditInfo creditInfo = null)
        {
            Mock<ICreditInfoCommand> creditInfoCommandMock = new Mock<ICreditInfoCommand>();
            creditInfoCommandMock.Setup(m => m.ToDomain(It.IsAny<IAccount>()))
                .Returns(creditInfo ?? _fixture.BuildCreditInfoMock().Object);
            return creditInfoCommandMock;
        }

        private class Sut : BusinessLogic.Accounting.Commands.AccountDataCommandBase
        {
        }
    }
}