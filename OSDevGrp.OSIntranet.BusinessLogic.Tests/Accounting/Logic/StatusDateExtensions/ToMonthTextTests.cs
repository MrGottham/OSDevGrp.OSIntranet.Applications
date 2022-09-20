using System;
using System.Globalization;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Logic.StatusDateExtensions
{
    [TestFixture]
    public class ToMonthTextTests
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
        public void ToMonthText_WhenFormatProviderIsNull_ThrowsArgumentNullException()
        {
            DateTime statusDate = DateTime.Today.AddDays(_random.Next(0, 365) * -1);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => statusDate.ToMonthText(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("formatProvider"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ToMonthText_WhenCalled_ReturnsNotNull()
        {
            DateTime statusDate = DateTime.Today.AddDays(_random.Next(0, 365) * -1);

            string result = statusDate.ToMonthText(CultureInfo.InvariantCulture);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToMonthText_WhenCalled_ReturnsNotEmpty()
        {
            DateTime statusDate = DateTime.Today.AddDays(_random.Next(0, 365) * -1);

            string result = statusDate.ToMonthText(CultureInfo.InvariantCulture);

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void ToMonthText_WhenCalled_ReturnsTextForMonth()
        {
            DateTime statusDate = DateTime.Today.AddDays(_random.Next(0, 365) * -1);

            IFormatProvider formatProvider = CultureInfo.InvariantCulture;
            string result = statusDate.ToMonthText(formatProvider);

            Assert.That(result, Is.EqualTo(statusDate.ToString("MMMM", formatProvider).ToLower()));
        }
    }
}