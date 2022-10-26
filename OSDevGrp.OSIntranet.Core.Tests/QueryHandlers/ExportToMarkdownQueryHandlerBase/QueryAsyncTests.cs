using System;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Converters;
using OSDevGrp.OSIntranet.Core.Interfaces.Queries;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.QueryHandlers;

namespace OSDevGrp.OSIntranet.Core.Tests.QueryHandlers.ExportToMarkdownQueryHandlerBase
{
    [TestFixture]
    public class QueryAsyncTests
    {
        #region Private variables

        private Mock<IDomainObjectToMarkdownConverter<object>> _domainObjectToMarkdownConverterMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _domainObjectToMarkdownConverterMock = new Mock<IDomainObjectToMarkdownConverter<object>>();
            _fixture = new Fixture();
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
        public async Task QueryAsync_WhenCalled_AssertGetExportDataAsyncWasCalledOnExportToMarkdownQueryHandlerBase()
        {
            IQueryHandler<IExportQuery, byte[]> sut = CreateSut();

            await sut.QueryAsync(CreateExportQuery());

            Assert.That(((MyExportToMarkdownQueryHandler)sut).GetExportDataAsyncWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetExportDataAsyncWasCalledOnExportToMarkdownQueryHandlerBaseWithExportQuery()
        {
            IQueryHandler<IExportQuery, byte[]> sut = CreateSut();

            IExportQuery query = CreateExportQuery();
            await sut.QueryAsync(query);

            Assert.That(((MyExportToMarkdownQueryHandler)sut).GetExportDataAsyncExportQuery, Is.EqualTo(query));
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
        public async Task QueryAsync_WhenNoMarkdownContentWasReturnedForExportData_ReturnsEmptyByteArray()
        {
            IQueryHandler<IExportQuery, byte[]> sut = CreateSut(hasMarkdownContent: false);

            byte[] result = await sut.QueryAsync(CreateExportQuery());

            Assert.That(result.Length, Is.EqualTo(0));

        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoMarkdownContentWasReturnedForExportData_ReturnsNonEmptyByteArray()
        {
            IQueryHandler<IExportQuery, byte[]> sut = CreateSut();

            byte[] result = await sut.QueryAsync(CreateExportQuery());

            Assert.That(result.Length, Is.GreaterThan(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoMarkdownContentWasReturnedForExportData_ReturnsMarkdownContentInByteArray()
        {
            string markdownContent = _fixture.Create<string>();
            IQueryHandler<IExportQuery, byte[]> sut = CreateSut(markdownContent: markdownContent);

            byte[] result = await sut.QueryAsync(CreateExportQuery());

            Assert.That(Encoding.UTF8.GetString(result), Is.EqualTo(markdownContent));
        }

        private IQueryHandler<IExportQuery, byte[]> CreateSut(bool hasDomainObject = true, object domainObject = null, bool hasMarkdownContent = true, string markdownContent = null)
        {
            _domainObjectToMarkdownConverterMock.Setup(m => m.ConvertAsync(It.IsAny<object>()))
                .Returns(Task.FromResult(hasMarkdownContent ? markdownContent ?? _fixture.Create<string>() : null));

            return new MyExportToMarkdownQueryHandler(hasDomainObject, domainObject ?? _fixture.Create<object>(), _domainObjectToMarkdownConverterMock.Object, false);
        }

        private IExportQuery CreateExportQuery()
        {
            return new Mock<IExportQuery>().Object;
        }

        private class MyExportToMarkdownQueryHandler : ExportToMarkdownQueryHandlerBase<IExportQuery, object, IDomainObjectToMarkdownConverter<object>>
        {
            #region Private variables

            private readonly bool _hasDomainObject;
            private readonly object _domainObject;

            #endregion

            #region Constructor

            public MyExportToMarkdownQueryHandler(bool hasDomainObject, object domainObject, IDomainObjectToMarkdownConverter<object> domainObjectToMarkdownConverter, bool encoderShouldEmitUtf8Identifier)
                : base(domainObjectToMarkdownConverter, encoderShouldEmitUtf8Identifier)
            {
                Core.NullGuard.NotNull(domainObject, nameof(domainObject));

                _hasDomainObject = hasDomainObject;
                _domainObject = domainObject;
            }

            #endregion

            #region Protperties

            public bool GetExportDataAsyncWasCalled { get; private set; }

            public IExportQuery GetExportDataAsyncExportQuery { get; private set; }

            #endregion

            #region Methods

            protected override Task<object> GetExportDataAsync(IExportQuery query)
            {
                Core.NullGuard.NotNull(query, nameof(query));

                GetExportDataAsyncWasCalled = true;
                GetExportDataAsyncExportQuery = query;

                return Task.FromResult(_hasDomainObject ? _domainObject : null);
            }

            #endregion
        }
    }
}