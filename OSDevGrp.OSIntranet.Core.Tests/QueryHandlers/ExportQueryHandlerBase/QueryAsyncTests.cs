using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Builders;
using OSDevGrp.OSIntranet.Core.Interfaces.Queries;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.QueryHandlers;

namespace OSDevGrp.OSIntranet.Core.Tests.QueryHandlers.ExportQueryHandlerBase
{
    [TestFixture]
    public class QueryAsyncTests
    {
        #region Private variables

        private Mock<IExportDataContentBuilder> _exportDataContentBuilderMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _exportDataContentBuilderMock = new Mock<IExportDataContentBuilder>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void QueryAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            IQueryHandler<IExportQuery, byte[]> sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("query"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertValidateQueryAsyncWasCalledOnExportQueryHandlerBase()
        {
            IQueryHandler<IExportQuery, byte[]> sut = CreateSut();

            await sut.QueryAsync(CreateExportQuery());

            Assert.That(((MyExportQueryHandler)sut).ValidateQueryAsyncWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertValidateQueryAsyncWasCalledOnExportQueryHandlerBaseWithExportQuery()
        {
            IQueryHandler<IExportQuery, byte[]> sut = CreateSut();

            IExportQuery query = CreateExportQuery();
            await sut.QueryAsync(query);

            Assert.That(((MyExportQueryHandler)sut).ValidateQueryAsyncExportQuery, Is.EqualTo(query));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertBeforeExportAsyncWasCalledOnExportQueryHandlerBase()
        {
            IQueryHandler<IExportQuery, byte[]> sut = CreateSut();

            await sut.QueryAsync(CreateExportQuery());

            Assert.That(((MyExportQueryHandler)sut).BeforeExportAsyncWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertBeforeExportAsyncWasCalledOnExportQueryHandlerBaseWithExportQuery()
        {
            IQueryHandler<IExportQuery, byte[]> sut = CreateSut();

            IExportQuery query = CreateExportQuery();
            await sut.QueryAsync(query);

            Assert.That(((MyExportQueryHandler)sut).BeforeExportAsyncExportQuery, Is.EqualTo(query));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetExportDataAsyncWasCalledOnExportQueryHandlerBase()
        {
            IQueryHandler<IExportQuery, byte[]> sut = CreateSut();

            await sut.QueryAsync(CreateExportQuery());

            Assert.That(((MyExportQueryHandler) sut).GetExportDataAsyncWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetExportDataAsyncWasCalledOnExportQueryHandlerBaseWithExportQuery()
        {
            IQueryHandler<IExportQuery, byte[]> sut = CreateSut();

            IExportQuery query = CreateExportQuery();
            await sut.QueryAsync(query);

            Assert.That(((MyExportQueryHandler) sut).GetExportDataAsyncExportQuery, Is.EqualTo(query));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoExportDataWasReturned_AssertCreateExportDataContentBuilderAsyncWasNotCalledOnExportQueryHandlerBaseWithExportQuery()
        {
            IQueryHandler<IExportQuery, byte[]> sut = CreateSut(false);

            await sut.QueryAsync(CreateExportQuery());

            Assert.That(((MyExportQueryHandler) sut).CreateExportDataContentBuilderAsyncWasCalled, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoExportDataWasReturned_AssertWithHeaderAsyncWasNotCalledOnExportDataContentBuilder()
        {
            IQueryHandler<IExportQuery, byte[]> sut = CreateSut(false);

            await sut.QueryAsync(CreateExportQuery());

            _exportDataContentBuilderMock.Verify(m => m.WithHeaderAsync(It.IsAny<IExportQuery>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoExportDataWasReturned_AssertWithContentAsyncWasNotCalledOnExportDataContentBuilder()
        {
            IQueryHandler<IExportQuery, byte[]> sut = CreateSut(false);

            await sut.QueryAsync(CreateExportQuery());

            _exportDataContentBuilderMock.Verify(m => m.WithContentAsync(It.IsAny<IExportQuery>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoExportDataWasReturned_AssertWithFooterAsyncWasNotCalledOnExportDataContentBuilder()
        {
            IQueryHandler<IExportQuery, byte[]> sut = CreateSut(false);

            await sut.QueryAsync(CreateExportQuery());

            _exportDataContentBuilderMock.Verify(m => m.WithFooterAsync(It.IsAny<IExportQuery>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoExportDataWasReturned_AssertBuildAsyncWasNotCalledOnExportDataContentBuilder()
        {
            IQueryHandler<IExportQuery, byte[]> sut = CreateSut(false);

            await sut.QueryAsync(CreateExportQuery());

            _exportDataContentBuilderMock.Verify(m => m.BuildAsync(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoExportDataWasReturned_AssertDisposeWasNotCalledOnExportDataContentBuilder()
        {
            IQueryHandler<IExportQuery, byte[]> sut = CreateSut(false);

            await sut.QueryAsync(CreateExportQuery());

            _exportDataContentBuilderMock.Verify(m => m.Dispose(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoExportDataWasReturned_ReturnsNotNull()
        {
            IQueryHandler<IExportQuery, byte[]> sut = CreateSut(false);

            byte[] result = await sut.QueryAsync(CreateExportQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoExportDataWasReturned_ReturnsByteArray()
        {
            IQueryHandler<IExportQuery, byte[]> sut = CreateSut(false);

            byte[] result = await sut.QueryAsync(CreateExportQuery());

            Assert.That(result, Is.TypeOf<byte[]>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoExportDataWasReturned_ReturnsEmptyByteArray()
        {
            IQueryHandler<IExportQuery, byte[]> sut = CreateSut(false);

            byte[] result = await sut.QueryAsync(CreateExportQuery());

            Assert.That(result.Length, Is.EqualTo(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenExportDataWasReturned_AssertCreateExportDataContentBuilderAsyncWasCalledOnExportQueryHandlerBaseWithExportQuery()
        {
            IQueryHandler<IExportQuery, byte[]> sut = CreateSut();

            await sut.QueryAsync(CreateExportQuery());

            Assert.That(((MyExportQueryHandler) sut).CreateExportDataContentBuilderAsyncWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenExportDataWasReturned_AssertWithHeaderAsyncWasCalledOnExportDataContentBuilder()
        {
            IQueryHandler<IExportQuery, byte[]> sut = CreateSut();

            IExportQuery query = CreateExportQuery();
            await sut.QueryAsync(query);

            _exportDataContentBuilderMock.Verify(m => m.WithHeaderAsync(It.Is<IExportQuery>(value => value != null && value == query)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenExportDataWasReturned_AssertWithContentAsyncWasCalledOnExportDataContentBuilder()
        {
            string exportData = _fixture.Create<string>();
            IQueryHandler<IExportQuery, byte[]> sut = CreateSut(exportData: exportData);

            IExportQuery query = CreateExportQuery();
            await sut.QueryAsync(query);

            _exportDataContentBuilderMock.Verify(m => m.WithContentAsync(
                    It.Is<IExportQuery>(value => value != null && value == query),
                    It.Is<string>(value => string.CompareOrdinal(value, exportData) == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenExportDataWasReturned_AssertWithFooterAsyncWasCalledOnExportDataContentBuilder()
        {
            IQueryHandler<IExportQuery, byte[]> sut = CreateSut();

            IExportQuery query = CreateExportQuery();
            await sut.QueryAsync(query);

            _exportDataContentBuilderMock.Verify(m => m.WithFooterAsync(It.Is<IExportQuery>(value => value != null && value == query)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenExportDataWasReturned_AssertBuildAsyncWasNotCalledOnExportDataContentBuilder()
        {
            IQueryHandler<IExportQuery, byte[]> sut = CreateSut();

            await sut.QueryAsync(CreateExportQuery());

            _exportDataContentBuilderMock.Verify(m => m.BuildAsync(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenExportDataWasReturned_AssertDisposeWasNotCalledOnExportDataContentBuilder()
        {
            IQueryHandler<IExportQuery, byte[]> sut = CreateSut();

            await sut.QueryAsync(CreateExportQuery());

            _exportDataContentBuilderMock.Verify(m => m.Dispose(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenExportDataWasReturned_ReturnsNotNull()
        {
            IQueryHandler<IExportQuery, byte[]> sut = CreateSut();

            byte[] result = await sut.QueryAsync(CreateExportQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenExportDataWasReturned_ReturnsByteArray()
        {
            IQueryHandler<IExportQuery, byte[]> sut = CreateSut();

            byte[] result = await sut.QueryAsync(CreateExportQuery());

            Assert.That(result, Is.TypeOf<byte[]>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenExportDataWasReturned_ReturnsNonEmptyByteArray()
        {
            IQueryHandler<IExportQuery, byte[]> sut = CreateSut();

            byte[] result = await sut.QueryAsync(CreateExportQuery());

            Assert.That(result.Length, Is.GreaterThan(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenExportDataWasReturned_ReturnsBuildAsyncResultFromExportDataContentBuilder()
        {
            byte[] exportDataContent = _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray();
            IQueryHandler<IExportQuery, byte[]> sut = CreateSut(exportDataContent: exportDataContent);

            byte[] result = await sut.QueryAsync(CreateExportQuery());

            Assert.That(result, Is.EqualTo(exportDataContent));
        }

        private IQueryHandler<IExportQuery, byte[]> CreateSut(bool hasExportData = true, string exportData = null, byte[] exportDataContent = null)
        {
            _exportDataContentBuilderMock.Setup(m => m.WithHeaderAsync(It.IsAny<IExportQuery>()))
                .Returns(Task.CompletedTask);

            _exportDataContentBuilderMock.Setup(m => m.WithContentAsync(It.IsAny<IExportQuery>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            _exportDataContentBuilderMock.Setup(m => m.WithFooterAsync(It.IsAny<IExportQuery>()))
                .Returns(Task.CompletedTask);

            _exportDataContentBuilderMock.Setup(m => m.BuildAsync())
                .Returns(Task.FromResult(exportDataContent ?? _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray()));

            return new MyExportQueryHandler(hasExportData, exportData ?? _fixture.Create<string>(), _exportDataContentBuilderMock.Object);
        }

        private IExportQuery CreateExportQuery()
        {
            return new Mock<IExportQuery>().Object;
        }

        private class MyExportQueryHandler : ExportQueryHandlerBase<IExportQuery, string>
        {
            #region Private variables

            private readonly bool _hasExportData;
            private readonly string _exportData;
            private readonly IExportDataContentBuilder _exportDataContentBuilder;

            #endregion

            #region Constructor

            public MyExportQueryHandler(bool hasExportData, string exportData, IExportDataContentBuilder exportDataContentBuilder)
            {
                Core.NullGuard.NotNull(exportData, nameof(exportData))
                    .NotNull(exportDataContentBuilder, nameof(exportDataContentBuilder));

                _hasExportData = hasExportData;
                _exportData = exportData;
                _exportDataContentBuilder = exportDataContentBuilder;
            }

            #endregion

            #region Protperties

            public bool ValidateQueryAsyncWasCalled { get; private set; }

            public IExportQuery ValidateQueryAsyncExportQuery { get; private set; }

            public bool BeforeExportAsyncWasCalled { get; private set; }

            public IExportQuery BeforeExportAsyncExportQuery { get; private set; }

            public bool GetExportDataAsyncWasCalled { get; private set; }

            public IExportQuery GetExportDataAsyncExportQuery { get; private set; }

            public bool CreateExportDataContentBuilderAsyncWasCalled { get; private set; }

            #endregion

            #region Methods

            protected override Task ValidateQueryAsync(IExportQuery query)
            {
                Core.NullGuard.NotNull(query, nameof(query));

                ValidateQueryAsyncWasCalled = true;
                ValidateQueryAsyncExportQuery = query;

                return base.ValidateQueryAsync(query);
            }

            protected override Task BeforeExportAsync(IExportQuery query)
            {
                Core.NullGuard.NotNull(query, nameof(query));

                BeforeExportAsyncWasCalled = true;
                BeforeExportAsyncExportQuery = query;

                return base.BeforeExportAsync(query);
            }

            protected override Task<string> GetExportDataAsync(IExportQuery query)
            {
                Core.NullGuard.NotNull(query, nameof(query));

                GetExportDataAsyncWasCalled = true;
                GetExportDataAsyncExportQuery = query;

                return Task.FromResult(_hasExportData ? _exportData : null);
            }

            protected override Task<IExportDataContentBuilder> CreateExportDataContentBuilderAsync()
            {
                CreateExportDataContentBuilderAsyncWasCalled = true;

                return Task.FromResult(_exportDataContentBuilder);
            }

            #endregion
        }
    }
}