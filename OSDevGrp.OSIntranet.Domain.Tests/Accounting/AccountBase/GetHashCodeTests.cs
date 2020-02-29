using System;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.AccountBase
{
    [TestFixture]
    public class GetHashCodeTests
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
        public void GetHashCode_WhenCalled_AssertNumberWasCalledOnAccounting()
        {
            Mock<IAccounting> accountingMock = _fixture.BuildAccountingMock();
            IAccountBase<IAccountBase> sut = CreateSut(accountingMock.Object);

            sut.GetHashCode();

            accountingMock.Verify(m => m.Number, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void  GetHashCode_WhenCalled_ReturnsHashCodeBasedOnAccountingNumberAndAccountNumber()
        {
            int accountingNumber = _fixture.Create<int>();
            IAccounting accounting = _fixture.BuildAccountingMock(accountingNumber).Object;
            string accountNumber = _fixture.Create<string>();
            IAccountBase<IAccountBase> sut = CreateSut(accounting, accountNumber);

            Assert.That(sut.GetHashCode(), Is.EqualTo(string.GetHashCode($"{accountNumber.ToUpper()}@{accountingNumber}")));
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