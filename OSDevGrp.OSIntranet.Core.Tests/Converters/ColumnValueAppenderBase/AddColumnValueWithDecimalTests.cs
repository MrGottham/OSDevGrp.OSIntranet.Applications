using System;
using System.Globalization;
using System.Linq;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Converters;

namespace OSDevGrp.OSIntranet.Core.Tests.Converters.ColumnValueAppenderBase
{
    [TestFixture]
    public class AddColumnValueWithDecimalTests
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
        public void AddColumnValue_WhenValueIsNull_AssertColumnWithEmptyValueHasBeenAdded()
        {
            IColumnValueAppender sut = CreateSut();

            IColumnValueAppender result = sut.AddColumnValue((decimal?) null);

            Assert.That(result.ColumnValues.ElementAt(0), Is.EqualTo(string.Empty));
        }

        [Test]
        [Category("UnitTest")]
        public void AddColumnValue_WhenValueIsNotNull_AssertColumnWithValueHasBeenAdded()
        {
            IColumnValueAppender sut = CreateSut();

            decimal value = _fixture.Create<decimal>();
            IColumnValueAppender result = sut.AddColumnValue(value);

            Assert.That(result.ColumnValues.ElementAt(0), Is.EqualTo(Convert.ToString(value, CultureInfo.CurrentUICulture)));
        }

        [Test]
        [Category("UnitTest")]
        public void AddColumnValue_WhenCalledMultipleTimes_AssertColumnValueHasBeenAddedForEachCall()
        {
            IColumnValueAppender sut = CreateSut();

            decimal[] values = _fixture.CreateMany<decimal>(_random.Next(5, 10)).ToArray();
            foreach (decimal value in values)
            {
                sut = sut.AddColumnValue(value);
            }

            Assert.That(sut.ColumnValues.Count(), Is.EqualTo(values.Length));
            for (int i = 0; i < values.Length; i++)
            {
                Assert.That(sut.ColumnValues.ElementAt(i), Is.EqualTo(Convert.ToString(values[i], CultureInfo.CurrentUICulture)));
            }
        }

        [Test]
        [Category("UnitTest")]
        public void AddColumnValue_WhenCalled_ReturnsMyColumnValueAppender()
        {
            IColumnValueAppender sut = CreateSut();

            IColumnValueAppender result = sut.AddColumnValue(_fixture.Create<decimal>());

            Assert.That(result, Is.TypeOf<MyColumnValueAppender>());
        }

        [Test]
        [Category("UnitTest")]
        public void AddColumnValue_WhenCalled_ReturnsSameInstanceOfColumnValueAppender()
        {
            IColumnValueAppender sut = CreateSut();

            IColumnValueAppender result = sut.AddColumnValue(_fixture.Create<decimal>());

            Assert.That(result, Is.SameAs(sut));
        }

        private IColumnValueAppender CreateSut()
        {
            return new MyColumnValueAppender(CultureInfo.CurrentUICulture);
        }

        private class MyColumnValueAppender : Core.Converters.ColumnValueAppenderBase
        {
            #region Constructor

            public MyColumnValueAppender(IFormatProvider formatProvider)
                : base(formatProvider)
            {
            }

            #endregion
        }
    }
}