using System;
using System.Globalization;
using System.Linq;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Converters;

namespace OSDevGrp.OSIntranet.Core.Tests.Converters.ColumnValueAppenderBase
{
    public class ResetColumnsTests
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
        public void ResetColumns_WhenNoColumnsHasBeenAdded_AssertThatColumnValuesIsEmpty()
        {
            IColumnValueAppender sut = CreateSut();

            IColumnValueAppender result = sut.ResetColumns();

            Assert.That(result.ColumnValues.Count(), Is.EqualTo(0));
        }

        [Test]
        [Category("UnitTest")]
        public void ResetColumns_WhenColumnsHasBeenAdded_AssertThatColumnValuesIsEmpty()
        {
            IColumnValueAppender sut = CreateSut();

            int numberOfColumns = _random.Next(10, 15);
            while (sut.ColumnValues.Count() < numberOfColumns)
            {
                sut = sut.AddColumnValue(_fixture.Create<string>());
            }

            IColumnValueAppender result = sut.ResetColumns();

            Assert.That(result.ColumnValues.Count(), Is.EqualTo(0));
        }

        [Test]
        [Category("UnitTest")]
        public void ResetColumns_WhenCalled_ReturnsMyColumnValueAppender()
        {
            IColumnValueAppender sut = CreateSut();

            IColumnValueAppender result = sut.ResetColumns();

            Assert.That(result, Is.TypeOf<MyColumnValueAppender>());
        }

        [Test]
        [Category("UnitTest")]
        public void ResetColumns_WhenCalled_ReturnsSameInstanceOfColumnValueAppender()
        {
            IColumnValueAppender sut = CreateSut();

            IColumnValueAppender result = sut.ResetColumns();

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