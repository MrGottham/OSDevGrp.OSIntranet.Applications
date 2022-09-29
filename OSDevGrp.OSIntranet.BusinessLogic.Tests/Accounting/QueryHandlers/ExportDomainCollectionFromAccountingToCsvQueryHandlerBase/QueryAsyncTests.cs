using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Converters;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.QueryHandlers.ExportDomainCollectionFromAccountingToCsvQueryHandlerBase
{
    [TestFixture]
    public class QueryAsyncTests
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<IAccountingRepository> _accountingRepositoryMock;
        private Mock<IStatusDateSetter> _statusDateSetterMock;
        private Mock<IDomainObjectToCsvConverter<object>> _domainObjectToCsvConverterMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _accountingRepositoryMock = new Mock<IAccountingRepository>();
            _statusDateSetterMock = new Mock<IStatusDateSetter>();
            _domainObjectToCsvConverterMock = new Mock<IDomainObjectToCsvConverter<object>>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void QueryAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            IQueryHandler<IExportFromAccountingQuery, byte[]> sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("query"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnExportFromAccountingQuery()
        {
            IQueryHandler<IExportFromAccountingQuery, byte[]> sut = CreateSut();

            Mock<IExportFromAccountingQuery> exportFromAccountingQueryMock = CreateExportFromAccountingQueryMock();
            await sut.QueryAsync(exportFromAccountingQueryMock.Object);

            exportFromAccountingQueryMock.Verify(m => m.Validate(
                    It.Is<IValidator>(value => value != null && value == _validatorMock.Object),
                    It.Is<IAccountingRepository>(value => value != null && value == _accountingRepositoryMock.Object)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertStatusDateWasCalledOnExportFromAccountingQuery()
        {
            IQueryHandler<IExportFromAccountingQuery, byte[]> sut = CreateSut();

            Mock<IExportFromAccountingQuery> exportFromAccountingQueryMock = CreateExportFromAccountingQueryMock();
            await sut.QueryAsync(exportFromAccountingQueryMock.Object);

            exportFromAccountingQueryMock.Verify(m => m.StatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertSetStatusDateWasCalledOnStatusDateSetter()
        {
            IQueryHandler<IExportFromAccountingQuery, byte[]> sut = CreateSut();

            DateTime statusDate = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
            IExportFromAccountingQuery exportFromAccountingQuery = CreateExportFromAccountingQuery(statusDate);
            await sut.QueryAsync(exportFromAccountingQuery);

            _statusDateSetterMock.Verify(m => m.SetStatusDate(It.Is<DateTime>(value => value == statusDate)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetExportDataAsyncWasCalledOnExportDomainCollectionFromAccountingToCsvQueryHandlerBase()
        {
            IQueryHandler<IExportFromAccountingQuery, byte[]> sut = CreateSut();

            await sut.QueryAsync(CreateExportFromAccountingQuery());

            Assert.That(((MyExportDomainCollectionFromAccountingToCsvQueryHandler)sut).GetExportDataAsyncWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetExportDataAsyncWasCalledOnExportDomainCollectionFromAccountingToCsvQueryHandlerBaseWithExportFromAccountingQuery()
        {
            IQueryHandler<IExportFromAccountingQuery, byte[]> sut = CreateSut();

            IExportFromAccountingQuery exportFromAccountingQuery = CreateExportFromAccountingQuery();
            await sut.QueryAsync(exportFromAccountingQuery);

            Assert.That(((MyExportDomainCollectionFromAccountingToCsvQueryHandler)sut).GetExportDataAsyncExportFromAccountingQuery, Is.EqualTo(exportFromAccountingQuery));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoExportDataWasReturned_ReturnsEmptyByteArray()
        {
            IQueryHandler<IExportFromAccountingQuery, byte[]> sut = CreateSut(false);

            byte[] result = await sut.QueryAsync(CreateExportFromAccountingQuery());

            Assert.That(result.Length, Is.EqualTo(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenExportDataWasReturned_ReturnsNonEmptyByteArray()
        {
            IQueryHandler<IExportFromAccountingQuery, byte[]> sut = CreateSut();

            byte[] result = await sut.QueryAsync(CreateExportFromAccountingQuery());

            Assert.That(result.Length, Is.GreaterThan(0));
        }

        private IQueryHandler<IExportFromAccountingQuery, byte[]> CreateSut(bool hasDomainObjectCollection = true, IEnumerable<object> domainObjectCollection = null, int? numberOfColumns = null)
        {
            int columns = numberOfColumns ?? _random.Next(5, 10);

            _domainObjectToCsvConverterMock.Setup(m => m.GetColumnNamesAsync())
                .Returns(Task.FromResult(_fixture.CreateMany<string>(columns).ToArray().AsEnumerable()));
            _domainObjectToCsvConverterMock.Setup(m => m.ConvertAsync(It.IsAny<object>()))
                .Returns(Task.FromResult(_fixture.CreateMany<string>(columns).ToArray().AsEnumerable()));

            return new MyExportDomainCollectionFromAccountingToCsvQueryHandler(hasDomainObjectCollection, domainObjectCollection ?? _fixture.CreateMany<object>(_random.Next(32, 64)).ToArray(), _validatorMock.Object, _accountingRepositoryMock.Object, _statusDateSetterMock.Object, _domainObjectToCsvConverterMock.Object);
        }

        private IExportFromAccountingQuery CreateExportFromAccountingQuery(DateTime? statusDate = null)
        {
            return CreateExportFromAccountingQueryMock(statusDate).Object;
        }

        private Mock<IExportFromAccountingQuery> CreateExportFromAccountingQueryMock(DateTime? statusDate = null)
        {
            Mock<IExportFromAccountingQuery> exportFromAccountingMock = new Mock<IExportFromAccountingQuery>();
            exportFromAccountingMock.Setup(m => m.StatusDate)
                .Returns(statusDate ?? DateTime.Today.AddDays(_random.Next(0, 7) * -1));
            return exportFromAccountingMock;
        }

        private class MyExportDomainCollectionFromAccountingToCsvQueryHandler : ExportDomainCollectionFromAccountingToCsvQueryHandlerBase<IExportFromAccountingQuery, object, IDomainObjectToCsvConverter<object>>
        {
            #region Private variables

            private readonly bool _hasDomainObjectCollection;
            private readonly IEnumerable<object> _domainObjectCollection;

            #endregion

            #region Constructor

            public MyExportDomainCollectionFromAccountingToCsvQueryHandler(bool hasDomainObjectCollection, IEnumerable<object> domainObjectCollection, IValidator validator, IAccountingRepository accountingRepository, IStatusDateSetter statusDateSetter, IDomainObjectToCsvConverter<object> domainObjectToCsvConverter, bool encoderShouldEmitUtf8Identifier = true)
                : base(validator, accountingRepository, statusDateSetter, domainObjectToCsvConverter, encoderShouldEmitUtf8Identifier)
            {
                NullGuard.NotNull(domainObjectCollection, nameof(domainObjectCollection));

                _hasDomainObjectCollection = hasDomainObjectCollection;
                _domainObjectCollection = domainObjectCollection;
            }

            #endregion

            #region Protperties

            public bool GetExportDataAsyncWasCalled { get; private set; }

            public IExportFromAccountingQuery GetExportDataAsyncExportFromAccountingQuery { get; private set; }

            #endregion

            #region Methods

            protected override Task<IEnumerable<object>> GetExportDataAsync(IExportFromAccountingQuery query)
            {
                NullGuard.NotNull(query, nameof(query));

                GetExportDataAsyncWasCalled = true;
                GetExportDataAsyncExportFromAccountingQuery = query;

                return Task.FromResult(_hasDomainObjectCollection ? _domainObjectCollection : null);
            }

            #endregion
        }
    }
}