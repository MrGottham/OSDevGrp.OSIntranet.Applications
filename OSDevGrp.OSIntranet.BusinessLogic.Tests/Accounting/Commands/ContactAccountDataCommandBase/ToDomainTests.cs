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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Commands.ContactAccountDataCommandBase
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
            IContactAccountDataCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ToDomain(null));

            Assert.That(result.ParamName, Is.EqualTo("accountingRepository"));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertGetAccountingAsyncWasCalledOnAccountingRepository()
        {
            int accountingNumber = _fixture.Create<int>();
            IContactAccountDataCommand sut = CreateSut(accountingNumber);

            sut.ToDomain(_accountingRepositoryMock.Object);

            _accountingRepositoryMock.Verify(m => m.GetAccountingAsync(
                    It.Is<int>(value => value == accountingNumber),
                    It.Is<DateTime>(value => value == DateTime.Today)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertGetPaymentTermAsyncWasCalledOnAccountingRepository()
        {
            int paymentTermNumber = _fixture.Create<int>();
            IContactAccountDataCommand sut = CreateSut(paymentTermNumber: paymentTermNumber);

            sut.ToDomain(_accountingRepositoryMock.Object);

            _accountingRepositoryMock.Verify(m => m.GetPaymentTermAsync(It.Is<int>(value => value == paymentTermNumber)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsContactAccount()
        {
            IContactAccountDataCommand sut = CreateSut();

            IContactAccount contactAccount = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(contactAccount, Is.TypeOf<ContactAccount>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsContactAccountWhereAccountingIsEqualToAccountingFromAccountingRepository()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IContactAccountDataCommand sut = CreateSut(accounting: accounting);

            IContactAccount contactAccount = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(contactAccount.Accounting, Is.EqualTo(accounting));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsContactAccountWhereAccountNumberIsEqualToAccountNumberFromContactAccountDataCommand()
        {
            string accountNumber = _fixture.Create<string>().ToUpper();
            IContactAccountDataCommand sut = CreateSut(accountNumber: accountNumber);

            IContactAccount contactAccount = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(contactAccount.AccountNumber, Is.EqualTo(accountNumber));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsContactAccountWhereAccountNameIsEqualToAccountNameFromContactAccountDataCommand()
        {
            string accountName = _fixture.Create<string>();
            IContactAccountDataCommand sut = CreateSut(accountName: accountName);

            IContactAccount contactAccount = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(contactAccount.AccountName, Is.EqualTo(accountName));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenDescriptionWasNotGivenInContactAccountDataCommand_ReturnsContactAccountWhereDescriptionIsNull()
        {
            IContactAccountDataCommand sut = CreateSut(hasDescription: false);

            IContactAccount contactAccount = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(contactAccount.Description, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenDescriptionWasGivenInContactAccountDataCommand_ReturnsContactAccountWhereDescriptionIsEqualToDescriptionFromContactAccountDataCommand()
        {
            string description = _fixture.Create<string>();
            IContactAccountDataCommand sut = CreateSut(description: description);

            IContactAccount contactAccount = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(contactAccount.Description, Is.EqualTo(description));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenNoteWasNotGivenInContactAccountDataCommand_ReturnsContactAccountWhereNoteIsNull()
        {
            IContactAccountDataCommand sut = CreateSut(hasNote: false);

            IContactAccount contactAccount = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(contactAccount.Note, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenNoteWasGivenInContactAccountDataCommand_ReturnsContactAccountWhereNoteIsEqualToNoteFromContactAccountDataCommand()
        {
            string note = _fixture.Create<string>();
            IContactAccountDataCommand sut = CreateSut(note: note);

            IContactAccount contactAccount = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(contactAccount.Note, Is.EqualTo(note));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenMailAddressWasNotGivenInContactAccountDataCommand_ReturnsContactAccountWhereMailAddressIsNull()
        {
            IContactAccountDataCommand sut = CreateSut(hasMailAddress: false);

            IContactAccount contactAccount = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(contactAccount.MailAddress, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenMailAddressWasGivenInContactAccountDataCommand_ReturnsContactAccountWhereMailAddressIsEqualToMailAddressFromContactAccountDataCommand()
        {
            string mailAddress = _fixture.Create<string>();
            IContactAccountDataCommand sut = CreateSut(mailAddress: mailAddress);

            IContactAccount contactAccount = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(contactAccount.MailAddress, Is.EqualTo(mailAddress));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenPrimaryPhoneWasNotGivenInContactAccountDataCommand_ReturnsContactAccountWherePrimaryPhoneIsNull()
        {
            IContactAccountDataCommand sut = CreateSut(hasPrimaryPhone: false);

            IContactAccount contactAccount = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(contactAccount.PrimaryPhone, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenPrimaryPhoneWasGivenInContactAccountDataCommand_ReturnsContactAccountWherePrimaryPhoneIsEqualToPrimaryPhoneFromContactAccountDataCommand()
        {
            string primaryPhone = _fixture.Create<string>();
            IContactAccountDataCommand sut = CreateSut(primaryPhone: primaryPhone);

            IContactAccount contactAccount = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(contactAccount.PrimaryPhone, Is.EqualTo(primaryPhone));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenSecondaryPhoneWasNotGivenInContactAccountDataCommand_ReturnsContactAccountWhereSecondaryPhoneIsNull()
        {
            IContactAccountDataCommand sut = CreateSut(hasSecondaryPhone: false);

            IContactAccount contactAccount = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(contactAccount.SecondaryPhone, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenSecondaryPhoneWasGivenInContactAccountDataCommand_ReturnsContactAccountWhereSecondaryPhoneIsEqualToSecondaryPhoneContactAccountDataCommand()
        {
            string secondaryPhone = _fixture.Create<string>();
            IContactAccountDataCommand sut = CreateSut(secondaryPhone: secondaryPhone);

            IContactAccount contactAccount = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(contactAccount.SecondaryPhone, Is.EqualTo(secondaryPhone));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsContactAccountWherePaymentTermIsEqualToPaymentTermFromAccountingRepository()
        {
            IPaymentTerm paymentTerm = _fixture.BuildPaymentTermMock().Object;
            IContactAccountDataCommand sut = CreateSut(paymentTerm: paymentTerm);

            IContactAccount contactAccount = sut.ToDomain(_accountingRepositoryMock.Object);

            Assert.That(contactAccount.PaymentTerm, Is.EqualTo(paymentTerm));
        }

        private IContactAccountDataCommand CreateSut(int? accountingNumber = null, IAccounting accounting = null, string accountNumber = null, string accountName = null, bool hasDescription = true, string description = null, bool hasNote = true, string note = null, bool hasMailAddress = true, string mailAddress = null, bool hasPrimaryPhone = true, string primaryPhone = null, bool hasSecondaryPhone = true, string secondaryPhone = null, int? paymentTermNumber = null, IPaymentTerm paymentTerm = null)
        {
            _accountingRepositoryMock.Setup(m => m.GetAccountingAsync(It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(accounting ?? _fixture.BuildAccountingMock().Object));
            _accountingRepositoryMock.Setup(m => m.GetPaymentTermAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(paymentTerm ?? _fixture.BuildPaymentTermMock().Object));

            return _fixture.Build<Sut>()
                .With(m => m.AccountingNumber, accountingNumber ?? _fixture.Create<int>())
                .With(m => m.AccountNumber, accountNumber ?? _fixture.Create<string>().ToUpper())
                .With(m => m.AccountName, accountName ?? _fixture.Create<string>())
                .With(m => m.Description, hasDescription ? description ?? _fixture.Create<string>() : null)
                .With(m => m.Note, hasNote ? note ?? _fixture.Create<string>() : null)
                .With(m => m.MailAddress, hasMailAddress ? mailAddress ?? _fixture.Create<string>() : null)
                .With(m => m.PrimaryPhone, hasPrimaryPhone ? primaryPhone ?? _fixture.Create<string>() : null)
                .With(m => m.SecondaryPhone, hasSecondaryPhone ?  secondaryPhone ?? _fixture.Create<string>() : null)
                .With(m => m.PaymentTermNumber, paymentTermNumber ?? _fixture.Create<int>())
                .Create();
        }

        private class Sut : BusinessLogic.Accounting.Commands.ContactAccountDataCommandBase
        {
        }
    }
}