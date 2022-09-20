using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Logic.StatusDateExtensions
{
    [TestFixture]
    public class GetEndDateOfLastYearTests
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
        public void GetEndDateOfLastYear_WhenStatusDateIncludesTime_ReturnsEndDateOfLastYearWithoutTime()
        {
            DateTime statusDateIncludingTime = DateTime.Today.AddDays(_random.Next(0, 365) * -1).AddMinutes(_random.Next(1, 24 * 60 - 1));

            DateTime result = statusDateIncludingTime.GetEndDateOfLastYear();

            Assert.That(result, Is.EqualTo(new DateTime(statusDateIncludingTime.Year - 1, 12, 31)));
        }

        [Test]
        [Category("UnitTest")]
        public void GetEndDateOfLastYear_WhenStatusDateExcludesTime_ReturnsEndDateOfLastYearWithoutTime()
        {
            DateTime statusDateExcludingTime = DateTime.Today.AddDays(_random.Next(0, 365) * -1);

            DateTime result = statusDateExcludingTime.GetEndDateOfLastYear();

            Assert.That(result, Is.EqualTo(new DateTime(statusDateExcludingTime.Year - 1, 12, 31)));
        }
    }
}