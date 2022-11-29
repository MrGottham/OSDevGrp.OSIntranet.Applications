using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Logic.StatusDateExtensions
{
    [TestFixture]
    public class GetEndDateOfLastMonthTests
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
        public void GetEndDateOfLastMonth_WhenStatusDateIncludesTime_ReturnsEndDateOfLastMonthWithoutTime()
        {
            DateTime statusDateIncludingTime = DateTime.Today.AddDays(_random.Next(0, 365) * -1).AddMinutes(_random.Next(1, 24 * 60 - 1));

            DateTime result = statusDateIncludingTime.GetEndDateOfLastMonth();

            Assert.That(result, Is.EqualTo(statusDateIncludingTime.AddDays(statusDateIncludingTime.Day * -1).Date));
        }

        [Test]
        [Category("UnitTest")]
        public void GetEndDateOfLastMonth_WhenStatusDateExcludesTime_ReturnsEndDateOfLastMonthWithoutTime()
        {
            DateTime statusDateExcludingTime = DateTime.Today.AddDays(_random.Next(0, 365) * -1);

            DateTime result = statusDateExcludingTime.GetEndDateOfLastMonth();

            Assert.That(result, Is.EqualTo(statusDateExcludingTime.AddDays(statusDateExcludingTime.Day * -1)));
        }
    }
}