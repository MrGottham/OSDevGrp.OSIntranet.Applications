using System;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.ContactAccount
{
    [TestFixture]
    public class ContactAccountTypeTests
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
        public void ContactAccountType_WhenCalled_AssertBalanceBelowZeroWasCalledOnAccounting()
        {
            Mock<IAccounting> accountingMock = _fixture.BuildAccountingMock();
            IContactAccount sut = CreateSut(accountingMock.Object);

            ContactAccountType result = sut.ContactAccountType;
            Assert.That(result, Is.AnyOf(ContactAccountType.None, ContactAccountType.Debtor, ContactAccountType.Creditor));

            accountingMock.Verify(m => m.BalanceBelowZero, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ContactAccountType_WhenCalled_AssertValuesAtStatusDateWasCalledOnContactInfoCollection()
        {
            Mock<IContactInfoCollection> contactInfoCollectionMock = _fixture.BuildContactInfoCollectionMock();
            IContactAccount sut = CreateSut(contactInfoCollection: contactInfoCollectionMock.Object);

            ContactAccountType result = sut.ContactAccountType;
            Assert.That(result, Is.AnyOf(ContactAccountType.None, ContactAccountType.Debtor, ContactAccountType.Creditor));

            contactInfoCollectionMock.Verify(m => m.ValuesAtStatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ContactAccountType_WhenCalled_AssertBalanceWasCalledOnValuesAtStatusDateInContactInfoCollection()
        {
            Mock<IContactInfoValues> valuesAtStatusDateMock = _fixture.BuildContactInfoValuesMock();
            IContactInfoCollection contactInfoCollection = _fixture.BuildContactInfoCollectionMock(valuesAtStatusDate: valuesAtStatusDateMock.Object).Object;
            IContactAccount sut = CreateSut(contactInfoCollection: contactInfoCollection);

            ContactAccountType result = sut.ContactAccountType;
            Assert.That(result, Is.AnyOf(ContactAccountType.None, ContactAccountType.Debtor, ContactAccountType.Creditor));

            valuesAtStatusDateMock.Verify(m => m.Balance, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ContactAccountType_WhenBalanceBelowZeroOnAccountingIsDebtorsAndBalanceInValuesAtStatusDateIsBelowZero_ReturnsDebtor()
        {
            IAccounting accounting = _fixture.BuildAccountingMock(balanceBelowZero: BalanceBelowZeroType.Debtors).Object;
            IContactInfoValues valuesAtStatusDate = _fixture.BuildContactInfoValuesMock(Math.Abs(_fixture.Create<decimal>()) * -1).Object;
            IContactInfoCollection contactInfoCollection = _fixture.BuildContactInfoCollectionMock(valuesAtStatusDate: valuesAtStatusDate).Object;
            IContactAccount sut = CreateSut(accounting, contactInfoCollection);

            ContactAccountType result = sut.ContactAccountType;

            Assert.That(result, Is.EqualTo(ContactAccountType.Debtor));
        }

        [Test]
        [Category("UnitTest")]
        public void ContactAccountType_WhenBalanceBelowZeroOnAccountingIsDebtorsAndBalanceInValuesAtStatusDateIsEqualToZero_ReturnsNone()
        {
            IAccounting accounting = _fixture.BuildAccountingMock(balanceBelowZero: BalanceBelowZeroType.Debtors).Object;
            IContactInfoValues valuesAtStatusDate = _fixture.BuildContactInfoValuesMock(0M).Object;
            IContactInfoCollection contactInfoCollection = _fixture.BuildContactInfoCollectionMock(valuesAtStatusDate: valuesAtStatusDate).Object;
            IContactAccount sut = CreateSut(accounting, contactInfoCollection);

            ContactAccountType result = sut.ContactAccountType;

            Assert.That(result, Is.EqualTo(ContactAccountType.None));
        }

        [Test]
        [Category("UnitTest")]
        public void ContactAccountType_WhenBalanceBelowZeroOnAccountingIsDebtorsAndBalanceInValuesAtStatusDateAboveToZero_ReturnsCreditor()
        {
            IAccounting accounting = _fixture.BuildAccountingMock(balanceBelowZero: BalanceBelowZeroType.Debtors).Object;
            IContactInfoValues valuesAtStatusDate = _fixture.BuildContactInfoValuesMock(Math.Abs(_fixture.Create<decimal>())).Object;
            IContactInfoCollection contactInfoCollection = _fixture.BuildContactInfoCollectionMock(valuesAtStatusDate: valuesAtStatusDate).Object;
            IContactAccount sut = CreateSut(accounting, contactInfoCollection);

            ContactAccountType result = sut.ContactAccountType;

            Assert.That(result, Is.EqualTo(ContactAccountType.Creditor));
        }

        [Test]
        [Category("UnitTest")]
        public void ContactAccountType_WhenBalanceBelowZeroOnAccountingIsCreditorsAndBalanceInValuesAtStatusDateIsBelowZero_ReturnsCreditor()
        {
            IAccounting accounting = _fixture.BuildAccountingMock(balanceBelowZero: BalanceBelowZeroType.Creditors).Object;
            IContactInfoValues valuesAtStatusDate = _fixture.BuildContactInfoValuesMock(Math.Abs(_fixture.Create<decimal>()) * -1).Object;
            IContactInfoCollection contactInfoCollection = _fixture.BuildContactInfoCollectionMock(valuesAtStatusDate: valuesAtStatusDate).Object;
            IContactAccount sut = CreateSut(accounting, contactInfoCollection);

            ContactAccountType result = sut.ContactAccountType;

            Assert.That(result, Is.EqualTo(ContactAccountType.Creditor));
        }

        [Test]
        [Category("UnitTest")]
        public void ContactAccountType_WhenBalanceBelowZeroOnAccountingIsCreditorsAndBalanceInValuesAtStatusDateIsEqualToZero_ReturnsNone()
        {
            IAccounting accounting = _fixture.BuildAccountingMock(balanceBelowZero: BalanceBelowZeroType.Creditors).Object;
            IContactInfoValues valuesAtStatusDate = _fixture.BuildContactInfoValuesMock(0M).Object;
            IContactInfoCollection contactInfoCollection = _fixture.BuildContactInfoCollectionMock(valuesAtStatusDate: valuesAtStatusDate).Object;
            IContactAccount sut = CreateSut(accounting, contactInfoCollection);

            ContactAccountType result = sut.ContactAccountType;

            Assert.That(result, Is.EqualTo(ContactAccountType.None));
        }

        [Test]
        [Category("UnitTest")]
        public void ContactAccountType_WhenBalanceBelowZeroOnAccountingIsCreditorsAndBalanceInValuesAtStatusDateAboveToZero_ReturnsDebtor()
        {
            IAccounting accounting = _fixture.BuildAccountingMock(balanceBelowZero: BalanceBelowZeroType.Creditors).Object;
            IContactInfoValues valuesAtStatusDate = _fixture.BuildContactInfoValuesMock(Math.Abs(_fixture.Create<decimal>())).Object;
            IContactInfoCollection contactInfoCollection = _fixture.BuildContactInfoCollectionMock(valuesAtStatusDate: valuesAtStatusDate).Object;
            IContactAccount sut = CreateSut(accounting, contactInfoCollection);

            ContactAccountType result = sut.ContactAccountType;

            Assert.That(result, Is.EqualTo(ContactAccountType.Debtor));
        }

        private IContactAccount CreateSut(IAccounting accounting = null, IContactInfoCollection contactInfoCollection = null)
        {
            return new Domain.Accounting.ContactAccount(accounting ?? _fixture.BuildAccountingMock().Object, _fixture.Create<string>(), _fixture.Create<string>(), _fixture.BuildPaymentTermMock().Object, contactInfoCollection ?? _fixture.BuildContactInfoCollectionMock().Object, _fixture.BuildPostingLineCollectionMock().Object);
        }
    }
}