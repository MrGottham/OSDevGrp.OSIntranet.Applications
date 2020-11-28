using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.BudgetInfo
{
    [TestFixture]
    public class IncomeTests
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
        public void Income_WhenSetterCalledWithValueBelowZero_ThrowsArgumentException()
        {
            IBudgetInfo sut = CreateSut();

            ArgumentException result = Assert.Throws<ArgumentException>(() => sut.Income = Math.Abs(_fixture.Create<decimal>() + 0.75M) * -1);

            Assert.That(result.Message.StartsWith("The income value cannot be below 0."), Is.True);
            Assert.That(result.ParamName, Is.EqualTo("value"));
        }

        [Test]
        [Category("UnitTest")]
        public void Income_WhenSetterCalledWithValueEqualToZero_SetValueToZero()
        {
            IBudgetInfo sut = CreateSut();

            sut.Income = 0M;

            Assert.That(sut.Income, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public void Income_WhenSetterCalledWithValueGreaterToZero_SetValueToGivenValue()
        {
            IBudgetInfo sut = CreateSut();

            decimal value = Math.Abs(_fixture.Create<decimal>() + 0.75M);
            sut.Income = value;

            Assert.That(sut.Income, Is.EqualTo(value));
        }

        private IBudgetInfo CreateSut()
        {
            short year = (short) _random.Next(InfoBase<IBudgetInfo>.MinYear, InfoBase<IBudgetInfo>.MaxYear);
            short month = (short) _random.Next(InfoBase<IBudgetInfo>.MinMonth, InfoBase<IBudgetInfo>.MaxMonth);

            return new Domain.Accounting.BudgetInfo(_fixture.BuildBudgetAccountMock().Object, year, month, Math.Abs(_fixture.Create<decimal>()), Math.Abs(_fixture.Create<decimal>()));
        }
    }
}