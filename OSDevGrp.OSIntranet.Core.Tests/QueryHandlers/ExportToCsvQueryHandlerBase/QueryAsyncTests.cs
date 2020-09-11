using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Converters;
using OSDevGrp.OSIntranet.Core.Interfaces.Queries;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.QueryHandlers;

namespace OSDevGrp.OSIntranet.Core.Tests.QueryHandlers.ExportToCsvQueryHandlerBase
{
    [TestFixture]
    public class QueryAsyncTests
    {
        #region Private variables

        private Mock<IDomainObjectToCsvConverter<object>> _domainObjectToCsvConverterMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _domainObjectToCsvConverterMock = new Mock<IDomainObjectToCsvConverter<object>>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void QueryAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            IQueryHandler<IExportQuery, byte[]> sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("query"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetExportDataAsyncWasCalledOnExportToCsvQueryHandlerBase()
        {
            IQueryHandler<IExportQuery, byte[]> sut = CreateSut();

            await sut.QueryAsync(CreateExportQuery());

            Assert.That(((MyExportToCsvQueryHandler) sut).GetExportDataAsyncWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetExportDataAsyncWasCalledOnExportToCsvQueryHandlerBaseWithExportQuery()
        {
            IQueryHandler<IExportQuery, byte[]> sut = CreateSut();

            IExportQuery query = CreateExportQuery();
            await sut.QueryAsync(query);

            Assert.That(((MyExportToCsvQueryHandler) sut).GetExportDataAsyncExportQuery, Is.EqualTo(query));
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
        public async Task QueryAsync_WhenExportDataWasReturned_ReturnsNonEmptyByteArray()
        {
            IQueryHandler<IExportQuery, byte[]> sut = CreateSut();

            byte[] result = await sut.QueryAsync(CreateExportQuery());

            Assert.That(result.Length, Is.GreaterThan(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenExportDataWasReturned_ReturnsCsvContentInByteArray()
        {
            int numberOfColumns = _random.Next(5, 10);
            IQueryHandler<IExportQuery, byte[]> sut = CreateSut(numberOfColumns: numberOfColumns);

            byte[] result = await sut.QueryAsync(CreateExportQuery());

            Regex allowCsvContentRegularExpression = GetAllowCsvContentRegularExpression(numberOfColumns);

            await using MemoryStream memoryStream = new MemoryStream(result);
            using StreamReader streamReader = new StreamReader(memoryStream, Encoding.UTF8);

            string headerContent = await streamReader.ReadLineAsync();
            Assert.That(allowCsvContentRegularExpression.IsMatch(headerContent ?? string.Empty), Is.True);

            while (streamReader.EndOfStream == false)
            {
                string dataContent = await streamReader.ReadLineAsync();
                Assert.That(allowCsvContentRegularExpression.IsMatch(dataContent ?? string.Empty), Is.True);
            }

            streamReader.Close();
        }

        private IQueryHandler<IExportQuery, byte[]> CreateSut(bool hasDomainObjectCollection = true, IEnumerable<object> domainObjectCollection = null, int? numberOfColumns = null)
        {
            int columns = numberOfColumns ?? _random.Next(5, 10);

            _domainObjectToCsvConverterMock.Setup(m => m.GetColumnNamesAsync())
                .Returns(Task.FromResult(_fixture.CreateMany<string>(columns).ToArray().AsEnumerable()));
            _domainObjectToCsvConverterMock.Setup(m => m.ConvertAsync(It.IsAny<object>()))
                .Returns(Task.FromResult(_fixture.CreateMany<string>(columns).ToArray().AsEnumerable()));

            return new MyExportToCsvQueryHandler(hasDomainObjectCollection, domainObjectCollection ?? _fixture.CreateMany<object>(_random.Next(50, 100)).ToArray(), _domainObjectToCsvConverterMock.Object, false);
        }

        private IExportQuery CreateExportQuery()
        {
            return new Mock<IExportQuery>().Object;
        }

        private Regex GetAllowCsvContentRegularExpression(int numberOfColumns)
        {
            StringBuilder allowedCsvContent = new StringBuilder("^");
            for (int i = 0; i < numberOfColumns; i++)
            {
                if (i > 0)
                {
                    allowedCsvContent.Append(";");
                }
                allowedCsvContent.Append("\"[0-9A-Za-z\\-]*\"");
            }
            allowedCsvContent.Append("$");

            return new Regex(allowedCsvContent.ToString(), RegexOptions.Compiled);
        }

        private class MyExportToCsvQueryHandler : ExportToCsvQueryHandlerBase<IExportQuery, object, IDomainObjectToCsvConverter<object>>
        {
            #region Private variables

            private readonly bool _hasDomainObjectCollection;
            private readonly IEnumerable<object> _domainObjectCollection;

            #endregion

            #region Constructor

            public MyExportToCsvQueryHandler(bool hasDomainObjectCollection, IEnumerable<object> domainObjectCollection, IDomainObjectToCsvConverter<object> domainObjectToCsvConverter, bool encoderShouldEmitUtf8Identifier) 
                : base(domainObjectToCsvConverter, encoderShouldEmitUtf8Identifier)
            {
                Core.NullGuard.NotNull(domainObjectCollection, nameof(domainObjectCollection));

                _hasDomainObjectCollection = hasDomainObjectCollection;
                _domainObjectCollection = domainObjectCollection;
            }

            #endregion

            #region Protperties

            public bool GetExportDataAsyncWasCalled { get; private set; }

            public IExportQuery GetExportDataAsyncExportQuery { get; private set; }

            #endregion

            #region Methods

            protected override Task<IEnumerable<object>> GetExportDataAsync(IExportQuery query)
            {
                Core.NullGuard.NotNull(query, nameof(query));

                GetExportDataAsyncWasCalled = true;
                GetExportDataAsyncExportQuery = query;

                return Task.FromResult(_hasDomainObjectCollection ? _domainObjectCollection : null);
            }

            #endregion
        }
    }
}