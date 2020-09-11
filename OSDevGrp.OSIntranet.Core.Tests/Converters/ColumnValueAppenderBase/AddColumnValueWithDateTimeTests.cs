using System;
using System.Globalization;
using System.Linq;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Converters;

namespace OSDevGrp.OSIntranet.Core.Tests.Converters.ColumnValueAppenderBase
{
    public class AddColumnValueWithDateTimeTests
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
        public void AddColumnValue_WhenCalledWithoutFormatAndValueIsNull_AssertColumnWithEmptyValueHasBeenAdded()
        {
            IColumnValueAppender sut = CreateSut();

            IColumnValueAppender result = sut.AddColumnValue((DateTime?) null);

            Assert.That(result.ColumnValues.ElementAt(0), Is.EqualTo(string.Empty));
        }

        [Test]
        [Category("UnitTest")]
        public void AddColumnValue_WhenCalledWithoutFormatAndValueIsNotNull_AssertColumnWithValueHasBeenAdded()
        {
            IColumnValueAppender sut = CreateSut();

            DateTime value = _fixture.Create<DateTime>();
            IColumnValueAppender result = sut.AddColumnValue(value);

            Assert.That(result.ColumnValues.ElementAt(0), Is.EqualTo($"{value.ToString("yyyy-MM-dd", CultureInfo.CurrentUICulture)}T{value.ToString("HH:mm:ss", CultureInfo.CurrentUICulture)}{value.ToString("zzz", CultureInfo.CurrentUICulture)}"));
        }

        [Test]
        [Category("UnitTest")]
        public void AddColumnValue_WhenCalledMultipleTimesWithoutFormat_AssertColumnValueHasBeenAddedForEachCall()
        {
            IColumnValueAppender sut = CreateSut();

            DateTime[] values = _fixture.CreateMany<DateTime>(_random.Next(5, 10)).ToArray();
            foreach (DateTime value in values)
            {
                sut = sut.AddColumnValue(value);
            }

            Assert.That(sut.ColumnValues.Count(), Is.EqualTo(values.Length));
            for (int i = 0; i < values.Length; i++)
            {
                Assert.That(sut.ColumnValues.ElementAt(i), Is.EqualTo($"{values[i].ToString("yyyy-MM-dd", CultureInfo.CurrentUICulture)}T{values[i].ToString("HH:mm:ss", CultureInfo.CurrentUICulture)}{values[i].ToString("zzz", CultureInfo.CurrentUICulture)}"));
            }
        }

        [Test]
        [Category("UnitTest")]
        public void AddColumnValue_WhenCalledWithoutFormat_ReturnsMyColumnValueAppender()
        {
            IColumnValueAppender sut = CreateSut();

            IColumnValueAppender result = sut.AddColumnValue(_fixture.Create<DateTime>());

            Assert.That(result, Is.TypeOf<MyColumnValueAppender>());
        }

        [Test]
        [Category("UnitTest")]
        public void AddColumnValue_WhenCalledWithoutFormat_ReturnsSameInstanceOfColumnValueAppender()
        {
            IColumnValueAppender sut = CreateSut();

            IColumnValueAppender result = sut.AddColumnValue(_fixture.Create<DateTime>());

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public void AddColumnValue_WhenFormatIsNull_ThrowsArgumentNullException()
        {
            IColumnValueAppender sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddColumnValue(_fixture.Create<DateTime>(), null));

            Assert.That(result.ParamName, Is.EqualTo("format"));
        }

        [Test]
        [Category("UnitTest")]
        public void AddColumnValue_WhenFormatIsEmpty_ThrowsArgumentNullException()
        {
            IColumnValueAppender sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddColumnValue(_fixture.Create<DateTime>(), string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("format"));
        }

        [Test]
        [Category("UnitTest")]
        public void AddColumnValue_WhenFormatIsWhiteSpace_ThrowsArgumentNullException()
        {
            IColumnValueAppender sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AddColumnValue(_fixture.Create<DateTime>(), " "));

            Assert.That(result.ParamName, Is.EqualTo("format"));
        }

        [Test]
        [Category("UnitTest")]
        public void AddColumnValue_WhenCalledWithFormatAndValueIsNull_AssertColumnWithEmptyValueHasBeenAdded()
        {
            IColumnValueAppender sut = CreateSut();

            IColumnValueAppender result = sut.AddColumnValue(null, "yyyy-MM-dd");

            Assert.That(result.ColumnValues.ElementAt(0), Is.EqualTo(string.Empty));
        }

        [Test]
        [Category("UnitTest")]
        public void AddColumnValue_WhenCalledWithFormatAndValueIsNotNull_AssertColumnWithValueHasBeenAdded()
        {
            IColumnValueAppender sut = CreateSut();

            DateTime value = _fixture.Create<DateTime>();
            IColumnValueAppender result = sut.AddColumnValue(value, "yyyy-MM-dd");

            Assert.That(result.ColumnValues.ElementAt(0), Is.EqualTo(value.ToString("yyyy-MM-dd", CultureInfo.CurrentUICulture)));
        }

        [Test]
        [Category("UnitTest")]
        public void AddColumnValue_WhenCalledMultipleTimesWithFormat_AssertColumnValueHasBeenAddedForEachCall()
        {
            IColumnValueAppender sut = CreateSut();

            DateTime[] values = _fixture.CreateMany<DateTime>(_random.Next(5, 10)).ToArray();
            foreach (DateTime value in values)
            {
                sut = sut.AddColumnValue(value, "yyyy-MM-dd");
            }

            Assert.That(sut.ColumnValues.Count(), Is.EqualTo(values.Length));
            for (int i = 0; i < values.Length; i++)
            {
                Assert.That(sut.ColumnValues.ElementAt(i), Is.EqualTo(values[i].ToString("yyyy-MM-dd", CultureInfo.CurrentUICulture)));
            }
        }

        [Test]
        [Category("UnitTest")]
        public void AddColumnValue_WhenCalledWithFormat_ReturnsMyColumnValueAppender()
        {
            IColumnValueAppender sut = CreateSut();

            IColumnValueAppender result = sut.AddColumnValue(_fixture.Create<DateTime>(), "yyyy-MM-dd");

            Assert.That(result, Is.TypeOf<MyColumnValueAppender>());
        }

        [Test]
        [Category("UnitTest")]
        public void AddColumnValue_WhenCalledWithFormat_ReturnsSameInstanceOfColumnValueAppender()
        {
            IColumnValueAppender sut = CreateSut();

            IColumnValueAppender result = sut.AddColumnValue(_fixture.Create<DateTime>(), "yyyy-MM-dd");

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