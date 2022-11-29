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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.QueryHandlers.ExportBudgetAccountGroupStatusCollectionQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<IAccountingRepository> _accountingRepositoryMock;
        private Mock<IStatusDateSetter> _statusDateSetterMock;
        private Mock<IBudgetAccountGroupStatusToCsvConverter> _budgetAccountGroupStatusToCsvConverterMock;
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
            _budgetAccountGroupStatusToCsvConverterMock = new Mock<IBudgetAccountGroupStatusToCsvConverter>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void QueryAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            IQueryHandler<IExportBudgetAccountGroupStatusCollectionQuery, byte[]> sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("query"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertAccountingNumberWasCalledOnExportContactAccountCollectionQuery()
        {
            IQueryHandler<IExportBudgetAccountGroupStatusCollectionQuery, byte[]> sut = CreateSut();

            Mock<IExportBudgetAccountGroupStatusCollectionQuery> exportBudgetAccountGroupStatusCollectionQueryMock = CreateExportBudgetAccountGroupStatusCollectionQueryMock();
            await sut.QueryAsync(exportBudgetAccountGroupStatusCollectionQueryMock.Object);

            exportBudgetAccountGroupStatusCollectionQueryMock.Verify(m => m.AccountingNumber, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertStatusDateWasCalledTwiceOnExportContactAccountCollectionQuery()
        {
            IQueryHandler<IExportBudgetAccountGroupStatusCollectionQuery, byte[]> sut = CreateSut();

            Mock<IExportBudgetAccountGroupStatusCollectionQuery> exportBudgetAccountGroupStatusCollectionQueryMock = CreateExportBudgetAccountGroupStatusCollectionQueryMock();
            await sut.QueryAsync(exportBudgetAccountGroupStatusCollectionQueryMock.Object);

            exportBudgetAccountGroupStatusCollectionQueryMock.Verify(m => m.StatusDate, Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetBudgetAccountsAsyncWasCalledOnAccountingRepository()
        {
            IQueryHandler<IExportBudgetAccountGroupStatusCollectionQuery, byte[]> sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            DateTime statusDate = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
            IExportBudgetAccountGroupStatusCollectionQuery exportBudgetAccountGroupStatusCollectionQuery = CreateExportBudgetAccountGroupStatusCollectionQuery(accountingNumber, statusDate);
            await sut.QueryAsync(exportBudgetAccountGroupStatusCollectionQuery);

            _accountingRepositoryMock.Verify(m => m.GetBudgetAccountsAsync(
                    It.Is<int>(value => value == accountingNumber),
                    It.Is<DateTime>(value => value == statusDate)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenBudgetAccountCollectionWasReturned_AssertCalculateAsyncWasCalledOnBudgetAccountCollection()
        {
            Mock<IBudgetAccountCollection> budgetAccountCollectionMock = _fixture.BuildBudgetAccountCollectionMock();
            IQueryHandler<IExportBudgetAccountGroupStatusCollectionQuery, byte[]> sut = CreateSut(budgetAccountCollection: budgetAccountCollectionMock.Object);

            DateTime statusDate = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
            IExportBudgetAccountGroupStatusCollectionQuery exportBudgetAccountGroupStatusCollectionQuery = CreateExportBudgetAccountGroupStatusCollectionQuery(statusDate: statusDate);
            await sut.QueryAsync(exportBudgetAccountGroupStatusCollectionQuery);

            budgetAccountCollectionMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalculatedBudgetAccountCollectionWasReturnedFromBudgetAccountCollection_AssertGroupByBudgetAccountGroupAsyncWasCalledOnCalculatedBudgetAccountCollection()
        {
            Mock<IBudgetAccountCollection> calculatedBudgetAccountCollectionMock = _fixture.BuildBudgetAccountCollectionMock();
            IBudgetAccountCollection budgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock(calculatedBudgetAccountCollection: calculatedBudgetAccountCollectionMock.Object).Object;
            IQueryHandler<IExportBudgetAccountGroupStatusCollectionQuery, byte[]> sut = CreateSut(budgetAccountCollection: budgetAccountCollection);

            DateTime statusDate = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
            IExportBudgetAccountGroupStatusCollectionQuery exportBudgetAccountGroupStatusCollectionQuery = CreateExportBudgetAccountGroupStatusCollectionQuery(statusDate: statusDate);
            await sut.QueryAsync(exportBudgetAccountGroupStatusCollectionQuery);

            calculatedBudgetAccountCollectionMock.Verify(m => m.GroupByBudgetAccountGroupAsync(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoBudgetAccountCollectionWasReturned_ReturnsNotNull()
        {
            IQueryHandler<IExportBudgetAccountGroupStatusCollectionQuery, byte[]> sut = CreateSut(false);

            IExportBudgetAccountGroupStatusCollectionQuery exportBudgetAccountGroupStatusCollectionQuery = CreateExportBudgetAccountGroupStatusCollectionQuery();
            byte[] result = await sut.QueryAsync(exportBudgetAccountGroupStatusCollectionQuery);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoBudgetAccountCollectionWasReturned_ReturnsNonEmptyByteArray()
        {
            IQueryHandler<IExportBudgetAccountGroupStatusCollectionQuery, byte[]> sut = CreateSut(false);

            IExportBudgetAccountGroupStatusCollectionQuery exportBudgetAccountGroupStatusCollectionQuery = CreateExportBudgetAccountGroupStatusCollectionQuery();
            byte[] result = await sut.QueryAsync(exportBudgetAccountGroupStatusCollectionQuery);

            Assert.That(result.Length, Is.GreaterThan(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoBudgetAccountCollectionWasReturned_ReturnsNonEmptyByteArrayWithHeaderLineOnly()
        {
            IQueryHandler<IExportBudgetAccountGroupStatusCollectionQuery, byte[]> sut = CreateSut(false);

            IExportBudgetAccountGroupStatusCollectionQuery exportBudgetAccountGroupStatusCollectionQuery = CreateExportBudgetAccountGroupStatusCollectionQuery();
            byte[] result = await sut.QueryAsync(exportBudgetAccountGroupStatusCollectionQuery);

            using MemoryStream memoryStream = new MemoryStream(result);
            using StreamReader reader = new StreamReader(memoryStream);

            MatchCollection match = NewLineRegex.Matches(await reader.ReadToEndAsync());
            Assert.That(match, Is.Not.Null);
            Assert.That(match.Count, Is.EqualTo(1));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoCalculatedBudgetAccountCollectionWasReturnedFromBudgetAccountCollection_ReturnsNotNull()
        {
            IBudgetAccountCollection budgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock(hasCalculatedBudgetAccountCollection: false).Object;
            IQueryHandler<IExportBudgetAccountGroupStatusCollectionQuery, byte[]> sut = CreateSut(budgetAccountCollection: budgetAccountCollection);

            IExportBudgetAccountGroupStatusCollectionQuery exportBudgetAccountGroupStatusCollectionQuery = CreateExportBudgetAccountGroupStatusCollectionQuery();
            byte[] result = await sut.QueryAsync(exportBudgetAccountGroupStatusCollectionQuery);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoCalculatedBudgetAccountCollectionWasReturnedFromBudgetAccountCollection_ReturnsNonEmptyByteArray()
        {
            IBudgetAccountCollection budgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock(hasCalculatedBudgetAccountCollection: false).Object;
            IQueryHandler<IExportBudgetAccountGroupStatusCollectionQuery, byte[]> sut = CreateSut(budgetAccountCollection: budgetAccountCollection);

            IExportBudgetAccountGroupStatusCollectionQuery exportBudgetAccountGroupStatusCollectionQuery = CreateExportBudgetAccountGroupStatusCollectionQuery();
            byte[] result = await sut.QueryAsync(exportBudgetAccountGroupStatusCollectionQuery);

            Assert.That(result.Length, Is.GreaterThan(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoCalculatedBudgetAccountCollectionWasReturnedFromBudgetAccountCollection_ReturnsNonEmptyByteArrayWithHeaderLineOnly()
        {
            IBudgetAccountCollection budgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock(hasCalculatedBudgetAccountCollection: false).Object;
            IQueryHandler<IExportBudgetAccountGroupStatusCollectionQuery, byte[]> sut = CreateSut(budgetAccountCollection: budgetAccountCollection);

            IExportBudgetAccountGroupStatusCollectionQuery exportBudgetAccountGroupStatusCollectionQuery = CreateExportBudgetAccountGroupStatusCollectionQuery();
            byte[] result = await sut.QueryAsync(exportBudgetAccountGroupStatusCollectionQuery);

            using MemoryStream memoryStream = new MemoryStream(result);
            using StreamReader reader = new StreamReader(memoryStream);

            MatchCollection match = NewLineRegex.Matches(await reader.ReadToEndAsync());
            Assert.That(match, Is.Not.Null);
            Assert.That(match.Count, Is.EqualTo(1));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalculatedBudgetAccountCollectionWasReturnedFromBudgetAccountCollection_ReturnsNotNull()
        {
            IQueryHandler<IExportBudgetAccountGroupStatusCollectionQuery, byte[]> sut = CreateSut();

            IExportBudgetAccountGroupStatusCollectionQuery exportBudgetAccountGroupStatusCollectionQuery = CreateExportBudgetAccountGroupStatusCollectionQuery();
            byte[] result = await sut.QueryAsync(exportBudgetAccountGroupStatusCollectionQuery);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalculatedBudgetAccountCollectionWasReturnedFromBudgetAccountCollection_ReturnsNonEmptyByteArray()
        {
            IQueryHandler<IExportBudgetAccountGroupStatusCollectionQuery, byte[]> sut = CreateSut();

            IExportBudgetAccountGroupStatusCollectionQuery exportBudgetAccountGroupStatusCollectionQuery = CreateExportBudgetAccountGroupStatusCollectionQuery();
            byte[] result = await sut.QueryAsync(exportBudgetAccountGroupStatusCollectionQuery);

            Assert.That(result.Length, Is.GreaterThan(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalculatedBudgetAccountCollectionWasReturnedFromBudgetAccountCollection_ReturnsNonEmptyByteArrayWithHeaderLineAndOneLinePrBudgetAccountGroupStatus()
        {
            IBudgetAccountGroupStatus[] groupByBudgetAccountGroupCollection =
            {
                _fixture.BuildBudgetAccountGroupStatusMock().Object,
                _fixture.BuildBudgetAccountGroupStatusMock().Object,
                _fixture.BuildBudgetAccountGroupStatusMock().Object
            };
            IBudgetAccountCollection calculatedBudgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock(groupByBudgetAccountGroupCollection: groupByBudgetAccountGroupCollection).Object;
            IBudgetAccountCollection budgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock(calculatedBudgetAccountCollection: calculatedBudgetAccountCollection).Object;
            IQueryHandler<IExportBudgetAccountGroupStatusCollectionQuery, byte[]> sut = CreateSut(budgetAccountCollection: budgetAccountCollection);

            IExportBudgetAccountGroupStatusCollectionQuery exportBudgetAccountGroupStatusCollectionQuery = CreateExportBudgetAccountGroupStatusCollectionQuery();
            byte[] result = await sut.QueryAsync(exportBudgetAccountGroupStatusCollectionQuery);

            using MemoryStream memoryStream = new MemoryStream(result);
            using StreamReader reader = new StreamReader(memoryStream);

            MatchCollection match = NewLineRegex.Matches(await reader.ReadToEndAsync());
            Assert.That(match, Is.Not.Null);
            Assert.That(match.Count, Is.EqualTo(1 + groupByBudgetAccountGroupCollection.Length));
        }

        private IQueryHandler<IExportBudgetAccountGroupStatusCollectionQuery, byte[]> CreateSut(bool hasBudgetAccountCollection = true, IBudgetAccountCollection budgetAccountCollection = null)
        {
            _accountingRepositoryMock.Setup(m => m.GetBudgetAccountsAsync(It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(hasBudgetAccountCollection ? budgetAccountCollection ?? _fixture.BuildBudgetAccountCollectionMock().Object : null));

            int columns = _random.Next(5, 10);
            _budgetAccountGroupStatusToCsvConverterMock.Setup(m => m.GetColumnNamesAsync())
                .Returns(Task.FromResult(_fixture.CreateMany<string>(columns).ToArray().AsEnumerable()));
            _budgetAccountGroupStatusToCsvConverterMock.Setup(m => m.ConvertAsync(It.IsAny<IBudgetAccountGroupStatus>()))
                .Returns(Task.FromResult(_fixture.CreateMany<string>(columns).ToArray().AsEnumerable()));

            return new BusinessLogic.Accounting.QueryHandlers.ExportBudgetAccountGroupStatusCollectionQueryHandler(_validatorMock.Object, _accountingRepositoryMock.Object, _statusDateSetterMock.Object, _budgetAccountGroupStatusToCsvConverterMock.Object);
        }

        private IExportBudgetAccountGroupStatusCollectionQuery CreateExportBudgetAccountGroupStatusCollectionQuery(int? accountingNumber = null, DateTime? statusDate = null)
        {
            return CreateExportBudgetAccountGroupStatusCollectionQueryMock(accountingNumber, statusDate).Object;
        }

        private Mock<IExportBudgetAccountGroupStatusCollectionQuery> CreateExportBudgetAccountGroupStatusCollectionQueryMock(int? accountingNumber = null, DateTime? statusDate = null)
        {
            Mock<IExportBudgetAccountGroupStatusCollectionQuery> exportBudgetAccountGroupStatusCollectionQueryMock = new Mock<IExportBudgetAccountGroupStatusCollectionQuery>();
            exportBudgetAccountGroupStatusCollectionQueryMock.Setup(m => m.AccountingNumber)
                .Returns(accountingNumber ?? _fixture.Create<int>());
            exportBudgetAccountGroupStatusCollectionQueryMock.Setup(m => m.StatusDate)
                .Returns(statusDate ?? DateTime.Today.AddDays(_random.Next(0, 7) * -1));
            return exportBudgetAccountGroupStatusCollectionQueryMock;
        }
    }
}