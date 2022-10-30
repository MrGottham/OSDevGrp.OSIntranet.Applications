using System;
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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.QueryHandlers.MakeMarkdownForDomainObjectFromAccountingQueryHandlerBase
{
    [TestFixture]
    public class QueryAsyncTests
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<IAccountingRepository> _accountingRepositoryMock;
        private Mock<IStatusDateSetter> _statusDateSetterMock;
        private Mock<IDomainObjectToMarkdownConverter<object>> _domainObjectToMarkdownConverterMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _accountingRepositoryMock = new Mock<IAccountingRepository>();
            _statusDateSetterMock = new Mock<IStatusDateSetter>();
            _domainObjectToMarkdownConverterMock = new Mock<IDomainObjectToMarkdownConverter<object>>();
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

            Assert.That(((MyMakeMarkdownForDomainObjectFromAccountingQueryHandler)sut).GetExportDataAsyncWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetExportDataAsyncWasCalledOnExportDomainCollectionFromAccountingToCsvQueryHandlerBaseWithExportFromAccountingQuery()
        {
            IQueryHandler<IExportFromAccountingQuery, byte[]> sut = CreateSut();

            IExportFromAccountingQuery exportFromAccountingQuery = CreateExportFromAccountingQuery();
            await sut.QueryAsync(exportFromAccountingQuery);

            Assert.That(((MyMakeMarkdownForDomainObjectFromAccountingQueryHandler)sut).GetExportDataAsyncExportFromAccountingQuery, Is.EqualTo(exportFromAccountingQuery));
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
        public async Task QueryAsync_WhenNoMarkdownContentWasReturnedForExportData_ReturnsEmptyByteArray()
        {
            IQueryHandler<IExportFromAccountingQuery, byte[]> sut = CreateSut(hasMarkdownContent: false);

            byte[] result = await sut.QueryAsync(CreateExportFromAccountingQuery());

            Assert.That(result.Length, Is.EqualTo(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenMarkdownContentWasReturnedForExportData_ReturnsNonEmptyByteArray()
        {
            IQueryHandler<IExportFromAccountingQuery, byte[]> sut = CreateSut();

            byte[] result = await sut.QueryAsync(CreateExportFromAccountingQuery());

            Assert.That(result.Length, Is.GreaterThan(0));
        }

        private IQueryHandler<IExportFromAccountingQuery, byte[]> CreateSut(bool hasDomainObject = true, bool hasMarkdownContent = true)
        {
            _domainObjectToMarkdownConverterMock.Setup(m => m.ConvertAsync(It.IsAny<object>()))
                .Returns(Task.FromResult(hasMarkdownContent ? _fixture.Create<string>() : null));

            return new MyMakeMarkdownForDomainObjectFromAccountingQueryHandler(hasDomainObject, _fixture.Create<object>(), _validatorMock.Object, _accountingRepositoryMock.Object, _statusDateSetterMock.Object, _domainObjectToMarkdownConverterMock.Object, false);
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

        private class MyMakeMarkdownForDomainObjectFromAccountingQueryHandler : MakeMarkdownForDomainObjectFromAccountingQueryHandlerBase<IExportFromAccountingQuery, object, IDomainObjectToMarkdownConverter<object>>
        {
            #region Private variables

            private readonly bool _hasDomainObject;
            private readonly object _domainObject;

            #endregion

            #region Constructor

            public MyMakeMarkdownForDomainObjectFromAccountingQueryHandler(bool hasDomainObject, object domainObject, IValidator validator, IAccountingRepository accountingRepository, IStatusDateSetter statusDateSetter, IDomainObjectToMarkdownConverter<object> domainObjectToMarkdownConverter, bool encoderShouldEmitUtf8Identifier = true)
                : base(validator, accountingRepository, statusDateSetter, domainObjectToMarkdownConverter, encoderShouldEmitUtf8Identifier)
            {
                NullGuard.NotNull(domainObject, nameof(domainObject));

                _hasDomainObject = hasDomainObject;
                _domainObject = domainObject;
            }

            #endregion

            #region Protperties

            public bool GetExportDataAsyncWasCalled { get; private set; }

            public IExportFromAccountingQuery GetExportDataAsyncExportFromAccountingQuery { get; private set; }

            #endregion

            #region Methods

            protected override Task<object> GetExportDataAsync(IExportFromAccountingQuery query)
            {
                NullGuard.NotNull(query, nameof(query));

                GetExportDataAsyncWasCalled = true;
                GetExportDataAsyncExportFromAccountingQuery = query;

                return Task.FromResult(_hasDomainObject ? _domainObject : null);
            }

            #endregion
        }
    }
}