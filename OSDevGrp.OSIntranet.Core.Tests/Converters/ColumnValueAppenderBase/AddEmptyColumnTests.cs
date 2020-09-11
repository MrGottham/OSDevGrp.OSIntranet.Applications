using System;
using System.Globalization;
using System.Linq;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Converters;

namespace OSDevGrp.OSIntranet.Core.Tests.Converters.ColumnValueAppenderBase
{
    public class AddEmptyColumnTests
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
        public void AddEmptyColumn_WhenCalled_AssertColumnWithEmptyValueHasBeenAdded()
        {
            IColumnValueAppender sut = CreateSut();

            IColumnValueAppender result = sut.AddEmptyColumn();

            Assert.That(result.ColumnValues.ElementAt(0), Is.EqualTo(string.Empty));
        }

        [Test]
        [Category("UnitTest")]
        public void AddEmptyColumn_WhenCalledMultipleTimes_AssertEmptyColumnHasBeenAddedForEachCall()
        {
            IColumnValueAppender sut = CreateSut();

            int times = _random.Next(5, 10);
            for (int i = 0; i < times; i++)
            {
                sut = sut.AddEmptyColumn();
            }

            Assert.That(sut.ColumnValues.Count(), Is.EqualTo(times));
            Assert.That(sut.ColumnValues.All(columnValue => string.CompareOrdinal(columnValue, string.Empty) == 0), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void AddEmptyColumn_WhenCalled_ReturnsMyColumnValueAppender()
        {
            IColumnValueAppender sut = CreateSut();

            IColumnValueAppender result = sut.AddEmptyColumn();

            Assert.That(result, Is.TypeOf<MyColumnValueAppender>());
        }

        [Test]
        [Category("UnitTest")]
        public void AddEmptyColumn_WhenCalled_ReturnsSameInstanceOfColumnValueAppender()
        {
            IColumnValueAppender sut = CreateSut();

            IColumnValueAppender result = sut.AddEmptyColumn();

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