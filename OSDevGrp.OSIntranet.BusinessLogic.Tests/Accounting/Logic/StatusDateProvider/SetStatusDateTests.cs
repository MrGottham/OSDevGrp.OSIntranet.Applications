using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Logic.StatusDateProvider
{
    [TestFixture]
    public class SetStatusDateTests
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
        public void GetStatusDate_WhenCalledWithStatusDateWhichIncludesTime_SetsStatusDateWithoutTime()
        {
            IStatusDateSetter sut = CreateSut();

            DateTime statusDateWhichIncludesTime = DateTime.Today.AddDays(_random.Next(0, 7) * -1).AddMinutes(_random.Next(1, 24 * 60 - 1));
            sut.SetStatusDate(statusDateWhichIncludesTime);

            DateTime result = ((IStatusDateProvider)sut).GetStatusDate();

            Assert.That(result, Is.EqualTo(statusDateWhichIncludesTime.Date));
        }

        [Test]
        [Category("UnitTest")]
        public void GetStatusDate_WhenCalledWithStatusDateWhichExcludesTime_SetsStatusDateWithoutTime()
        {
            IStatusDateSetter sut = CreateSut();

            DateTime statusDateWhichExcludesTime = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
            sut.SetStatusDate(statusDateWhichExcludesTime);

            DateTime result = ((IStatusDateProvider)sut).GetStatusDate();

            Assert.That(result, Is.EqualTo(statusDateWhichExcludesTime));
        }

        private IStatusDateSetter CreateSut()
        {
            return new BusinessLogic.Accounting.Logic.StatusDateProvider();
        }
    }
}