using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.AccountBase
{
    [TestFixture]
    public class EqualsTests
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
        public void Equals_WhenObjectIsNull_ReturnsFalse()
        {
            IAccountBase<IAccountBase> sut = CreateSut();

            bool result = sut.Equals(null);

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void Equals_WhenObjectIsNoneAccountBase_ReturnsFalse()
        {
            IAccountBase<IAccountBase> sut = CreateSut();

            bool result = sut.Equals(_fixture.Create<object>());

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void Equals_WhenObjectIsAccountBaseWithDifferentAccountingAndDifferentAccountNumber_ReturnsFalse()
        {
            int accountingNumber = _fixture.Create<int>();
            IAccounting accounting = _fixture.BuildAccountingMock(accountingNumber).Object;
            string accountNumber = _fixture.Create<string>();
            IAccountBase<IAccountBase> sut = CreateSut(accounting, accountNumber);

            bool result = sut.Equals(CreateSut(_fixture.BuildAccountingMock(accountingNumber).Object, _fixture.Create<string>()));

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void Equals_WhenObjectIsAccountBaseWithSameAccountingAndDifferentAccountNumber_ReturnsFalse()
        {
            int accountingNumber = _fixture.Create<int>();
            IAccounting accounting = _fixture.BuildAccountingMock(accountingNumber).Object;
            string accountNumber = _fixture.Create<string>();
            IAccountBase<IAccountBase> sut = CreateSut(accounting, accountNumber);

            bool result = sut.Equals(CreateSut(accounting, _fixture.Create<string>()));

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void Equals_WhenObjectIsAccountBaseWithDifferentAccountingAndSameAccountNumber_ReturnsFalse()
        {
            int accountingNumber = _fixture.Create<int>();
            IAccounting accounting = _fixture.BuildAccountingMock(accountingNumber).Object;
            string accountNumber = _fixture.Create<string>();
            IAccountBase<IAccountBase> sut = CreateSut(accounting, accountNumber);

            bool result = sut.Equals(CreateSut(_fixture.BuildAccountingMock(accountingNumber + 1).Object, accountNumber));

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void Equals_WhenObjectIsAccountBaseWithSameAccountingAndSameAccountNumber_ReturnsTrue()
        {
            int accountingNumber = _fixture.Create<int>();
            IAccounting accounting = _fixture.BuildAccountingMock(accountingNumber).Object;
            string accountNumber = _fixture.Create<string>();
            IAccountBase<IAccountBase> sut = CreateSut(accounting, accountNumber);

            bool result = sut.Equals(CreateSut(accounting, accountNumber));

            Assert.That(result, Is.True);
        }

        private IAccountBase<IAccountBase> CreateSut(IAccounting accounting = null, string accountNumber = null)
        {
            return new Sut(accounting ?? _fixture.BuildAccountingMock().Object, accountNumber ?? _fixture.Create<string>(), _fixture.Create<string>());
        }

        private class Sut : Domain.Accounting.AccountBase<IAccountBase>
        {
            #region Constructor

            public Sut(IAccounting accounting, string accountNumber, string accountName)
                : base(accounting, accountNumber, accountName)
            {
            }

            #endregion

            #region Methods

            protected override IAccountBase Calculate(DateTime statusDate)
            {
                return this;
            }

            #endregion
        }
    }
}