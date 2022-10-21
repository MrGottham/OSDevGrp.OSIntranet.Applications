using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.QueryHandlers.ExportAccountGroupStatusCollectionQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<IAccountingRepository> _accountingRepositoryMock;
        private Mock<IStatusDateSetter> _statusDateSetterMock;
        private Mock<IAccountGroupStatusToCsvConverter> _accountGroupStatusToCsvConverterMock;
        private Fixture _fixture;
        private Random _random;
        private static readonly Regex NewLineRegex = new Regex(Environment.NewLine, RegexOptions.Compiled);

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _accountingRepositoryMock = new Mock<IAccountingRepository>();
            _statusDateSetterMock = new Mock<IStatusDateSetter>();
            _accountGroupStatusToCsvConverterMock = new Mock<IAccountGroupStatusToCsvConverter>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void QueryAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            IQueryHandler<IExportAccountGroupStatusCollectionQuery, byte[]> sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("query"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertAccountingNumberWasCalledOnExportContactAccountCollectionQuery()
        {
            IQueryHandler<IExportAccountGroupStatusCollectionQuery, byte[]> sut = CreateSut();

            Mock<IExportAccountGroupStatusCollectionQuery> exportAccountGroupStatusCollectionQueryMock = CreateExportAccountGroupStatusCollectionQueryMock();
            await sut.QueryAsync(exportAccountGroupStatusCollectionQueryMock.Object);

            exportAccountGroupStatusCollectionQueryMock.Verify(m => m.AccountingNumber, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertStatusDateWasCalledTwiceOnExportContactAccountCollectionQuery()
        {
            IQueryHandler<IExportAccountGroupStatusCollectionQuery, byte[]> sut = CreateSut();

            Mock<IExportAccountGroupStatusCollectionQuery> exportAccountGroupStatusCollectionQueryMock = CreateExportAccountGroupStatusCollectionQueryMock();
            await sut.QueryAsync(exportAccountGroupStatusCollectionQueryMock.Object);

            exportAccountGroupStatusCollectionQueryMock.Verify(m => m.StatusDate, Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetAccountsAsyncWasCalledOnAccountingRepository()
        {
            IQueryHandler<IExportAccountGroupStatusCollectionQuery, byte[]> sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            DateTime statusDate = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
            IExportAccountGroupStatusCollectionQuery exportAccountGroupStatusCollectionQuery = CreateExportAccountGroupStatusCollectionQuery(accountingNumber, statusDate);
            await sut.QueryAsync(exportAccountGroupStatusCollectionQuery);

            _accountingRepositoryMock.Verify(m => m.GetAccountsAsync(
                    It.Is<int>(value => value == accountingNumber),
                    It.Is<DateTime>(value => value == statusDate)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenAccountCollectionWasReturned_AssertCalculateAsyncWasCalledOnAccountCollection()
        {
            Mock<IAccountCollection> accountCollectionMock = _fixture.BuildAccountCollectionMock();
            IQueryHandler<IExportAccountGroupStatusCollectionQuery, byte[]> sut = CreateSut(accountCollection: accountCollectionMock.Object);

            DateTime statusDate = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
            IExportAccountGroupStatusCollectionQuery exportAccountGroupStatusCollectionQuery = CreateExportAccountGroupStatusCollectionQuery(statusDate: statusDate);
            await sut.QueryAsync(exportAccountGroupStatusCollectionQuery);

            accountCollectionMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalculatedAccountCollectionWasReturnedFromAccountCollection_AssertGroupByAccountGroupAsyncWasCalledOnCalculatedAccountCollection()
        {
            Mock<IAccountCollection> calculatedAccountCollectionMock = _fixture.BuildAccountCollectionMock();
            IAccountCollection accountCollection = _fixture.BuildAccountCollectionMock(calculatedAccountCollection: calculatedAccountCollectionMock.Object).Object;
            IQueryHandler<IExportAccountGroupStatusCollectionQuery, byte[]> sut = CreateSut(accountCollection: accountCollection);

            DateTime statusDate = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
            IExportAccountGroupStatusCollectionQuery exportAccountGroupStatusCollectionQuery = CreateExportAccountGroupStatusCollectionQuery(statusDate: statusDate);
            await sut.QueryAsync(exportAccountGroupStatusCollectionQuery);

            calculatedAccountCollectionMock.Verify(m => m.GroupByAccountGroupAsync(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoAccountCollectionWasReturned_ReturnsNotNull()
        {
            IQueryHandler<IExportAccountGroupStatusCollectionQuery, byte[]> sut = CreateSut(false);

            IExportAccountGroupStatusCollectionQuery exportAccountGroupStatusCollectionQuery = CreateExportAccountGroupStatusCollectionQuery();
            byte[] result = await sut.QueryAsync(exportAccountGroupStatusCollectionQuery);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoAccountCollectionWasReturned_ReturnsNonEmptyByteArray()
        {
            IQueryHandler<IExportAccountGroupStatusCollectionQuery, byte[]> sut = CreateSut(false);

            IExportAccountGroupStatusCollectionQuery exportAccountGroupStatusCollectionQuery = CreateExportAccountGroupStatusCollectionQuery();
            byte[] result = await sut.QueryAsync(exportAccountGroupStatusCollectionQuery);

            Assert.That(result.Length, Is.GreaterThan(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoAccountCollectionWasReturned_ReturnsNonEmptyByteArrayWithHeaderLineOnly()
        {
            IQueryHandler<IExportAccountGroupStatusCollectionQuery, byte[]> sut = CreateSut(false);

            IExportAccountGroupStatusCollectionQuery exportAccountGroupStatusCollectionQuery = CreateExportAccountGroupStatusCollectionQuery();
            byte[] result = await sut.QueryAsync(exportAccountGroupStatusCollectionQuery);

            using MemoryStream memoryStream = new MemoryStream(result);
            using StreamReader reader = new StreamReader(memoryStream);

            MatchCollection match = NewLineRegex.Matches(await reader.ReadToEndAsync());
            Assert.That(match, Is.Not.Null);
            Assert.That(match.Count, Is.EqualTo(1));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoCalculatedAccountCollectionWasReturnedFromAccountCollection_ReturnsNotNull()
        {
            IAccountCollection accountCollection = _fixture.BuildAccountCollectionMock(hasCalculatedAccountCollection: false).Object;
            IQueryHandler<IExportAccountGroupStatusCollectionQuery, byte[]> sut = CreateSut(accountCollection: accountCollection);

            IExportAccountGroupStatusCollectionQuery exportAccountGroupStatusCollectionQuery = CreateExportAccountGroupStatusCollectionQuery();
            byte[] result = await sut.QueryAsync(exportAccountGroupStatusCollectionQuery);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoCalculatedAccountCollectionWasReturnedFromAccountCollection_ReturnsNonEmptyByteArray()
        {
            IAccountCollection accountCollection = _fixture.BuildAccountCollectionMock(hasCalculatedAccountCollection: false).Object;
            IQueryHandler<IExportAccountGroupStatusCollectionQuery, byte[]> sut = CreateSut(accountCollection: accountCollection);

            IExportAccountGroupStatusCollectionQuery exportAccountGroupStatusCollectionQuery = CreateExportAccountGroupStatusCollectionQuery();
            byte[] result = await sut.QueryAsync(exportAccountGroupStatusCollectionQuery);

            Assert.That(result.Length, Is.GreaterThan(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoCalculatedAccountCollectionWasReturnedFromAccountCollection_ReturnsNonEmptyByteArrayWithHeaderLineOnly()
        {
            IAccountCollection accountCollection = _fixture.BuildAccountCollectionMock(hasCalculatedAccountCollection: false).Object;
            IQueryHandler<IExportAccountGroupStatusCollectionQuery, byte[]> sut = CreateSut(accountCollection: accountCollection);

            IExportAccountGroupStatusCollectionQuery exportAccountGroupStatusCollectionQuery = CreateExportAccountGroupStatusCollectionQuery();
            byte[] result = await sut.QueryAsync(exportAccountGroupStatusCollectionQuery);

            using MemoryStream memoryStream = new MemoryStream(result);
            using StreamReader reader = new StreamReader(memoryStream);

            MatchCollection match = NewLineRegex.Matches(await reader.ReadToEndAsync());
            Assert.That(match, Is.Not.Null);
            Assert.That(match.Count, Is.EqualTo(1));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalculatedAccountCollectionWasReturnedFromAccountCollection_ReturnsNotNull()
        {
            IQueryHandler<IExportAccountGroupStatusCollectionQuery, byte[]> sut = CreateSut();

            IExportAccountGroupStatusCollectionQuery exportAccountGroupStatusCollectionQuery = CreateExportAccountGroupStatusCollectionQuery();
            byte[] result = await sut.QueryAsync(exportAccountGroupStatusCollectionQuery);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalculatedAccountCollectionWasReturnedFromAccountCollection_ReturnsNonEmptyByteArray()
        {
            IQueryHandler<IExportAccountGroupStatusCollectionQuery, byte[]> sut = CreateSut();

            IExportAccountGroupStatusCollectionQuery exportAccountGroupStatusCollectionQuery = CreateExportAccountGroupStatusCollectionQuery();
            byte[] result = await sut.QueryAsync(exportAccountGroupStatusCollectionQuery);

            Assert.That(result.Length, Is.GreaterThan(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalculatedAccountCollectionWasReturnedFromAccountCollection_ReturnsNonEmptyByteArrayWithHeaderLineAndOneLinePrAccountGroupStatus()
        {
            IAccountGroupStatus[] groupByAccountGroupCollection =
            {
                _fixture.BuildAccountGroupStatusMock().Object,
                _fixture.BuildAccountGroupStatusMock().Object,
                _fixture.BuildAccountGroupStatusMock().Object
            };
            IAccountCollection calculatedAccountCollection = _fixture.BuildAccountCollectionMock(groupByAccountGroupCollection: groupByAccountGroupCollection).Object;
            IAccountCollection accountCollection = _fixture.BuildAccountCollectionMock(calculatedAccountCollection: calculatedAccountCollection).Object;
            IQueryHandler<IExportAccountGroupStatusCollectionQuery, byte[]> sut = CreateSut(accountCollection: accountCollection);

            IExportAccountGroupStatusCollectionQuery exportAccountGroupStatusCollectionQuery = CreateExportAccountGroupStatusCollectionQuery();
            byte[] result = await sut.QueryAsync(exportAccountGroupStatusCollectionQuery);

            using MemoryStream memoryStream = new MemoryStream(result);
            using StreamReader reader = new StreamReader(memoryStream);

            MatchCollection match = NewLineRegex.Matches(await reader.ReadToEndAsync());
            Assert.That(match, Is.Not.Null);
            Assert.That(match.Count, Is.EqualTo(1 + groupByAccountGroupCollection.Length));
        }

        private IQueryHandler<IExportAccountGroupStatusCollectionQuery, byte[]> CreateSut(bool hasAccountCollection = true, IAccountCollection accountCollection = null)
        {
            _accountingRepositoryMock.Setup(m => m.GetAccountsAsync(It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(hasAccountCollection ? accountCollection ?? _fixture.BuildAccountCollectionMock().Object : null));

            int columns = _random.Next(5, 10);
            _accountGroupStatusToCsvConverterMock.Setup(m => m.GetColumnNamesAsync())
                .Returns(Task.FromResult(_fixture.CreateMany<string>(columns).ToArray().AsEnumerable()));
            _accountGroupStatusToCsvConverterMock.Setup(m => m.ConvertAsync(It.IsAny<IAccountGroupStatus>()))
                .Returns(Task.FromResult(_fixture.CreateMany<string>(columns).ToArray().AsEnumerable()));

            return new BusinessLogic.Accounting.QueryHandlers.ExportAccountGroupStatusCollectionQueryHandler(_validatorMock.Object, _accountingRepositoryMock.Object, _statusDateSetterMock.Object, _accountGroupStatusToCsvConverterMock.Object);
        }

        private IExportAccountGroupStatusCollectionQuery CreateExportAccountGroupStatusCollectionQuery(int? accountingNumber = null, DateTime? statusDate = null)
        {
            return CreateExportAccountGroupStatusCollectionQueryMock(accountingNumber, statusDate).Object;
        }

        private Mock<IExportAccountGroupStatusCollectionQuery> CreateExportAccountGroupStatusCollectionQueryMock(int? accountingNumber = null, DateTime? statusDate = null)
        {
            Mock<IExportAccountGroupStatusCollectionQuery> exportAccountGroupStatusCollectionQueryMock = new Mock<IExportAccountGroupStatusCollectionQuery>();
            exportAccountGroupStatusCollectionQueryMock.Setup(m => m.AccountingNumber)
                .Returns(accountingNumber ?? _fixture.Create<int>());
            exportAccountGroupStatusCollectionQueryMock.Setup(m => m.StatusDate)
                .Returns(statusDate ?? DateTime.Today.AddDays(_random.Next(0, 7) * -1));
            return exportAccountGroupStatusCollectionQueryMock;
        }
    }
}