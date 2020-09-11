using System;
using System.IO;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Builders;

namespace OSDevGrp.OSIntranet.Core.Tests.Builders.ExportDataContentBuilderBase
{
    [TestFixture]
    public class DisposeTests
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
        public void Dispose_WhenCalledMultipleTimes_ThrowsNoException()
        {
            using IExportDataContentBuilder sut = CreateSut();

            int numberOfTimes = _random.Next(5, 10);
            while (numberOfTimes > 0)
            {
                sut.Dispose();
                numberOfTimes--;
            }
        }

        [Test]
        [Category("UnitTest")]
        public void Dispose_WhenCalled_AssertWriterHasBeenDisposed()
        {
            using IExportDataContentBuilder sut = CreateSut();

            StreamWriter writer = ((MyExportDataContentBuilder) sut).GetWriter();

            sut.Dispose();

            Assert.ThrowsAsync<ObjectDisposedException>(async () => await writer.WriteAsync(_fixture.Create<string>()));
        }

        private IExportDataContentBuilder CreateSut()
        {
            return new MyExportDataContentBuilder();
        }

        private class MyExportDataContentBuilder : Core.Builders.ExportDataContentBuilderBase
        {
            #region Methods

            public StreamWriter GetWriter()
            {
                return Writer;
            }

            #endregion
        }
    }
}