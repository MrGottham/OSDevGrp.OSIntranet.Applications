using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Logic.StatusDateExtensions
{
    [TestFixture]
    public class GetFirstDateOfMonthTests
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
        public void GetFirstDateOfMonth_WhenStatusDateIncludesTime_ReturnsFirstDateOfMonthWithoutTime()
        {
            DateTime statusDateIncludingTime = DateTime.Today.AddDays(_random.Next(0, 365) * -1).AddMinutes(_random.Next(1, 24 * 60 - 1));

            DateTime result = statusDateIncludingTime.GetFirstDateOfMonth();

            Assert.That(result, Is.EqualTo(new DateTime(statusDateIncludingTime.Year, statusDateIncludingTime.Month, 1)));
        }

        [Test]
        [Category("UnitTest")]
        public void GetFirstDateOfMonth_WhenStatusDateExcludesTime_ReturnsFirstDateOfMonthWithoutTime()
        {
            DateTime statusDateExcludingTime = DateTime.Today.AddDays(_random.Next(0, 365) * -1);

            DateTime result = statusDateExcludingTime.GetFirstDateOfMonth();

            Assert.That(result, Is.EqualTo(new DateTime(statusDateExcludingTime.Year, statusDateExcludingTime.Month, 1)));
        }
    }
}