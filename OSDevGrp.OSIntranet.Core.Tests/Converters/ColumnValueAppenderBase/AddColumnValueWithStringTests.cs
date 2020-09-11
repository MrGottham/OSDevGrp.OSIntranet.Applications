using System;
using System.Globalization;
using System.Linq;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Converters;

namespace OSDevGrp.OSIntranet.Core.Tests.Converters.ColumnValueAppenderBase
{
    [TestFixture]
    public class AddColumnValueWithStringTests
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

            IColumnValueAppender result = sut.AddColumnValue((string) null);

            Assert.That(result.ColumnValues.ElementAt(0), Is.EqualTo(string.Empty));
        }

        [Test]
        [Category("UnitTest")]
        public void AddColumnValue_WhenValueIsEmpty_AssertColumnWithEmptyValueHasBeenAdded()
        {
            IColumnValueAppender sut = CreateSut();

            IColumnValueAppender result = sut.AddColumnValue(string.Empty);

            Assert.That(result.ColumnValues.ElementAt(0), Is.EqualTo(string.Empty));
        }

        [Test]
        [Category("UnitTest")]
        public void AddColumnValue_WhenValueIsWhiteSpace_AssertColumnWithEmptyValueHasBeenAdded()
        {
            IColumnValueAppender sut = CreateSut();

            IColumnValueAppender result = sut.AddColumnValue(" ");

            Assert.That(result.ColumnValues.ElementAt(0), Is.EqualTo(string.Empty));
        }

        [Test]
        [Category("UnitTest")]
        public void AddColumnValue_WhenValueIsNotNullEmptyOrWhiteSpace_AssertColumnWithValueHasBeenAdded()
        {
            IColumnValueAppender sut = CreateSut();

            string value = _fixture.Create<string>();
            IColumnValueAppender result = sut.AddColumnValue(value);

            Assert.That(result.ColumnValues.ElementAt(0), Is.EqualTo(value));
        }

        [Test]
        [Category("UnitTest")]
        public void AddColumnValue_WhenCalledMultipleTimes_AssertColumnValueHasBeenAddedForEachCall()
        {
            IColumnValueAppender sut = CreateSut();

            string[] values = _fixture.CreateMany<string>(_random.Next(5, 10)).ToArray();
            foreach (string value in values)
            {
                sut = sut.AddColumnValue(value);
            }

            Assert.That(sut.ColumnValues.Count(), Is.EqualTo(values.Length));
            for (int i = 0; i < values.Length; i++)
            {
                Assert.That(sut.ColumnValues.ElementAt(i), Is.EqualTo(values[i]));
            }
        }

        [Test]
        [Category("UnitTest")]
        public void AddColumnValue_WhenCalled_ReturnsMyColumnValueAppender()
        {
            IColumnValueAppender sut = CreateSut();

            IColumnValueAppender result = sut.AddColumnValue(_fixture.Create<string>());

            Assert.That(result, Is.TypeOf<MyColumnValueAppender>());
        }

        [Test]
        [Category("UnitTest")]
        public void AddColumnValue_WhenCalled_ReturnsSameInstanceOfColumnValueAppender()
        {
            IColumnValueAppender sut = CreateSut();

            IColumnValueAppender result = sut.AddColumnValue(_fixture.Create<string>());

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