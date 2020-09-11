using System;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Builders;
using OSDevGrp.OSIntranet.Core.Interfaces.Queries;

namespace OSDevGrp.OSIntranet.Core.Tests.Builders.ExportDataContentBuilderBase
{
    [TestFixture]
    public class BuildAsyncTests
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
        public async Task BuildAsync_WhenNoContentHasBeenWritten_ReturnsNotNull()
        {
            using IExportDataContentBuilder sut = CreateSut();

            byte[] result = await sut.BuildAsync();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task BuildAsync_WhenNoContentHasBeenWritten_ReturnsByteArray()
        {
            using IExportDataContentBuilder sut = CreateSut();

            byte[] result = await sut.BuildAsync();

            Assert.That(result, Is.TypeOf<byte[]>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task BuildAsync_WhenNoContentHasBeenWritten_ReturnsEmptyByteArray()
        {
            using IExportDataContentBuilder sut = CreateSut();

            byte[] result = await sut.BuildAsync();

            Assert.That(result.Length, Is.EqualTo(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task BuildAsync_WhenContentHasBeenWritten_ReturnsNotNull()
        {
            using IExportDataContentBuilder sut = CreateSut();

            await sut.WithContentAsync(CreateExportQuery(), _fixture.Create<string>());

            byte[] result = await sut.BuildAsync();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task BuildAsync_WhenContentHasBeenWritten_ReturnsByteArray()
        {
            using IExportDataContentBuilder sut = CreateSut();

            await sut.WithContentAsync(CreateExportQuery(), _fixture.Create<string>());

            byte[] result = await sut.BuildAsync();

            Assert.That(result, Is.TypeOf<byte[]>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task BuildAsync_WhenContentHasBeenWritten_ReturnsNonEmptyByteArray()
        {
            using IExportDataContentBuilder sut = CreateSut();

            await sut.WithContentAsync(CreateExportQuery(), _fixture.Create<string>());

            byte[] result = await sut.BuildAsync();

            Assert.That(result.Length, Is.GreaterThan(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task BuildAsync_WhenContentHasBeenWritten_ReturnsUTF8ByteArrayForContent()
        {
            using IExportDataContentBuilder sut = CreateSut();

            string content = _fixture.Create<string>();
            await sut.WithContentAsync(CreateExportQuery(), content);

            byte[] result = await sut.BuildAsync();

            Assert.That(Encoding.UTF8.GetString(result), Is.EqualTo(content));
        }

        private IExportDataContentBuilder CreateSut()
        {
            return new MyExportDataContentBuilder(false);
        }

        private IExportQuery CreateExportQuery()
        {
            return new Mock<IExportQuery>().Object;
        }

        private class MyExportDataContentBuilder : Core.Builders.ExportDataContentBuilderBase
        {
            #region Constructor

            public MyExportDataContentBuilder(bool encoderShouldEmitUtf8Identifier) 
                : base(encoderShouldEmitUtf8Identifier)
            {
            }

            #endregion

            #region Methods

            public override async Task WithContentAsync<TExportQuery, TExportData>(TExportQuery query, TExportData data)
            {
                Core.NullGuard.NotNull(query, nameof(query))
                    .NotNull(data, nameof(data));

                await Writer.WriteAsync(Convert.ToString(data));
            }

            #endregion
        }
    }
}