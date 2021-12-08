using System;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.PostingLine
{
    [TestFixture]
    public class ApplyCalculationAsyncWithAccountingTests
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
        public void ApplyCalculationAsync_WhenCalculatedAccountingIsNull_ThrowsArgumentNullException()
        {
            IPostingLine sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ApplyCalculationAsync((IAccounting)null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("calculatedAccounting"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_ReturnsNotNull()
        {
            IPostingLine sut = CreateSut();

            IPostingLine result = await sut.ApplyCalculationAsync(_fixture.BuildAccountingMock().Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_ReturnsSamePostingLine()
        {
            IPostingLine sut = CreateSut();

            IPostingLine result = await sut.ApplyCalculationAsync(_fixture.BuildAccountingMock().Object);

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_ReturnsSamePostingLineWhereAccountingIsNotNull()
        {
            IPostingLine sut = CreateSut();

            IPostingLine result = await sut.ApplyCalculationAsync(_fixture.BuildAccountingMock().Object);

            Assert.That(result.Accounting, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_ReturnsSamePostingLineWhereAccountingIsEqualToCalculatedAccounting()
        {
            IPostingLine sut = CreateSut();

            IAccounting calculatedAccounting = _fixture.BuildAccountingMock().Object;
            IPostingLine result = await sut.ApplyCalculationAsync(calculatedAccounting);

            Assert.That(result.Accounting, Is.EqualTo(calculatedAccounting));
        }

        private IPostingLine CreateSut()
        {
            int year = _random.Next(InfoBase<ICreditInfo>.MinYear, Math.Min(DateTime.Today.Year, InfoBase<ICreditInfo>.MaxYear));
            int month = _random.Next(InfoBase<ICreditInfo>.MinMonth, Math.Min(DateTime.Today.Month, InfoBase<ICreditInfo>.MaxMonth));
            int day = _random.Next(1, DateTime.DaysInMonth(year, month));

            return new Domain.Accounting.PostingLine(Guid.NewGuid(), new DateTime(year, month, day), _fixture.Create<string>(), _fixture.BuildAccountMock().Object, _fixture.Create<string>(), null, Math.Abs(_fixture.Create<decimal>()), Math.Abs(_fixture.Create<decimal>()), null, Math.Abs(_fixture.Create<int>()));
        }
    }
}