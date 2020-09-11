using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using QueryHandler=OSDevGrp.OSIntranet.BusinessLogic.Contacts.QueryHandlers.ExportContactCollectionQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.QueryHandlers.ExportContactCollectionQuery
{
    [TestFixture]
    public class QueryAsyncTests
    {
        #region Private variables

        private Mock<IQueryHandler<IGetContactCollectionQuery, IEnumerable<IContact>>> _getContactCollectionQueryHandlerMock;
        private Mock<IContactToCsvConverter> _contactToCsvConverterMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _getContactCollectionQueryHandlerMock = new Mock<IQueryHandler<IGetContactCollectionQuery, IEnumerable<IContact>>>();
            _contactToCsvConverterMock = new Mock<IContactToCsvConverter>();

            _fixture = new Fixture();
            _fixture.Customize<IContactGroup>(builder => builder.FromFactory(() => _fixture.BuildContactGroupMock().Object));
            _fixture.Customize<IPaymentTerm>(builder => builder.FromFactory(() => _fixture.BuildPaymentTermMock().Object));
            _fixture.Customize<ICompany>(builder => builder.FromFactory(() => _fixture.BuildCompanyMock().Object));
            _fixture.Customize<IContact>(builder => builder.FromFactory(() => _fixture.BuildContactMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void QueryAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            QueryHandler sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("query"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertToGetContactCollectionQueryWasCalledOnQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IExportContactCollectionQuery> queryMock = CreateExportContactCollectionQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.ToGetContactCollectionQuery(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertQueryAsyncWasCalledOnGetContactCollectionQueryHandler()
        {
            QueryHandler sut = CreateSut();

            IGetContactCollectionQuery getContactCollectionQuery = CreateGetContactCollectionQuery();
            IExportContactCollectionQuery query = CreateExportContactCollectionQuery(getContactCollectionQuery);
            await sut.QueryAsync(query);

            _getContactCollectionQueryHandlerMock.Verify(m => m.QueryAsync(It.Is<IGetContactCollectionQuery>(value => value != null && value == getContactCollectionQuery)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetColumnNamesAsyncWasCalledOnContactToCsvConverter()
        {
            QueryHandler sut = CreateSut();

            await sut.QueryAsync(CreateExportContactCollectionQuery());

            _contactToCsvConverterMock.Verify(m => m.GetColumnNamesAsync(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertConvertAsyncWasCalledOnContactToCsvConverterForEachContactToExport()
        {
            IList<IContact> contactCollection = _fixture.CreateMany<IContact>(_random.Next(25, 50)).ToList();
            QueryHandler sut = CreateSut(contactCollection);

            await sut.QueryAsync(CreateExportContactCollectionQuery());

            foreach (IContact contact in contactCollection)
            {
                _contactToCsvConverterMock.Verify(m => m.ConvertAsync(It.Is<IContact>(value => value != null && value == contact)), Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnNonEmptyByteArray()
        {
            QueryHandler sut = CreateSut();

            byte[] result = await sut.QueryAsync(CreateExportContactCollectionQuery());

            Assert.That(result.Length, Is.GreaterThan(0));
        }

        private QueryHandler CreateSut(IEnumerable<IContact> contactCollection = null)
        {
            _getContactCollectionQueryHandlerMock.Setup(m => m.QueryAsync(It.IsAny<IGetContactCollectionQuery>()))
                .Returns(Task.FromResult(contactCollection ?? _fixture.CreateMany<IContact>(_random.Next(25, 50)).ToList()));

            int numberOfColumns = _random.Next(15, 30);
            _contactToCsvConverterMock.Setup(m => m.GetColumnNamesAsync())
                .Returns(Task.FromResult(_fixture.CreateMany<string>(numberOfColumns).ToArray().AsEnumerable()));
            _contactToCsvConverterMock.Setup(m => m.ConvertAsync(It.IsAny<IContact>()))
                .Returns(Task.FromResult(_fixture.CreateMany<string>(numberOfColumns).ToArray().AsEnumerable()));

            return new QueryHandler(_getContactCollectionQueryHandlerMock.Object, _contactToCsvConverterMock.Object);
        }

        private IExportContactCollectionQuery CreateExportContactCollectionQuery(IGetContactCollectionQuery getContactCollectionQuery = null)
        {
            return CreateExportContactCollectionQueryMock(getContactCollectionQuery).Object;
        }

        private Mock<IExportContactCollectionQuery> CreateExportContactCollectionQueryMock(IGetContactCollectionQuery getContactCollectionQuery = null)
        {
            Mock<IExportContactCollectionQuery> queryMock = new Mock<IExportContactCollectionQuery>();
            queryMock.Setup(m => m.ToGetContactCollectionQuery())
                .Returns(getContactCollectionQuery ?? CreateGetContactCollectionQuery());
            return queryMock;
        }

        private IGetContactCollectionQuery CreateGetContactCollectionQuery()
        {
            return new Mock<IGetContactCollectionQuery>().Object;
        }
    }
}