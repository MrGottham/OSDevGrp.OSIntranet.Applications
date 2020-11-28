using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.CreditInfo
{
    [TestFixture]
    public class CreditTests
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
        public void Credit_WhenSetterCalledWithValueBelowZero_ThrowsArgumentException()
        {
            ICreditInfo sut = CreateSut();

            ArgumentException result = Assert.Throws<ArgumentException>(() => sut.Credit = Math.Abs(_fixture.Create<decimal>() + 0.75M) * -1);

            Assert.That(result.Message.StartsWith("The credit value cannot be below 0."), Is.True);
            Assert.That(result.ParamName, Is.EqualTo("value"));
        }

        [Test]
        [Category("UnitTest")]
        public void Credit_WhenSetterCalledWithValueEqualToZero_SetValueToZero()
        {
            ICreditInfo sut = CreateSut();

            sut.Credit = 0M;

            Assert.That(sut.Credit, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public void Credit_WhenSetterCalledWithValueGreaterToZero_SetValueToGivenValue()
        {
            ICreditInfo sut = CreateSut();

            decimal value = Math.Abs(_fixture.Create<decimal>() + 0.75M);
            sut.Credit = value;

            Assert.That(sut.Credit, Is.EqualTo(value));
        }

        private ICreditInfo CreateSut()
        {
            short year = (short) _random.Next(InfoBase<ICreditInfo>.MinYear, InfoBase<ICreditInfo>.MaxYear);
            short month = (short) _random.Next(InfoBase<ICreditInfo>.MinMonth, InfoBase<ICreditInfo>.MaxMonth);

            return new Domain.Accounting.CreditInfo(_fixture.BuildAccountMock().Object, year, month, Math.Abs(_fixture.Create<decimal>()));
        }
    }
}