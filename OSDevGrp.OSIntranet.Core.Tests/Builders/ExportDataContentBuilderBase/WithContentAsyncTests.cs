using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Builders;
using OSDevGrp.OSIntranet.Core.Interfaces.Queries;

namespace OSDevGrp.OSIntranet.Core.Tests.Builders.ExportDataContentBuilderBase
{
    [TestFixture]
    public class WithContentAsyncTests
    {
        #region Private variables

        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void WithContentAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            using IExportDataContentBuilder sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.WithContentAsync<IExportQuery, object>(null, _fixture.Create<object>()));

            Assert.That(result.ParamName, Is.EqualTo("query"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithContentAsync_WhenDataIsNull_ThrowsArgumentNullException()
        {
            using IExportDataContentBuilder sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.WithContentAsync<IExportQuery, object>(CreateExportQuery(), null));

            Assert.That(result.ParamName, Is.EqualTo("data"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task WithContentAsync_WhenQueryAndDataIsNotNull_ThrowsNoException()
        {
            using IExportDataContentBuilder sut = CreateSut();

            await sut.WithContentAsync(CreateExportQuery(), _fixture.Create<object>());
        }

        private IExportDataContentBuilder CreateSut()
        {
            return new MyExportDataContentBuilder();
        }

        private IExportQuery CreateExportQuery()
        {
            return new Mock<IExportQuery>().Object;
        }

        private class MyExportDataContentBuilder : Core.Builders.ExportDataContentBuilderBase
        {
        }
    }
}