using System;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.Accounting
{
    [TestFixture]
    public class CalculateAsyncTests
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
        public async Task CalculateAsync_WhenCalled_AssertStatusDateEqualDateFromCall()
        {
            ICalculable<IAccounting> sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.CalculateAsync(statusDate);

            Assert.That(sut.StatusDate, Is.EqualTo(statusDate.Date));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSameCalculable()
        {
            ICalculable<IAccounting> sut = CreateSut();

            ICalculable<IAccounting> result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result, Is.SameAs(sut));
        }

        private ICalculable<IAccounting> CreateSut()
        {
            return new Domain.Accounting.Accounting(_fixture.Create<int>(), _fixture.Create<string>(), _fixture.BuildLetterHeadMock().Object);
        }
   }
}