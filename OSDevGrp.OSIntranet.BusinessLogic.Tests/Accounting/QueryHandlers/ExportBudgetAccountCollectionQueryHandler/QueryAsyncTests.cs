using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.QueryHandlers.ExportBudgetAccountCollectionQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<IClaimResolver> _claimResolverMock;
        private Mock<IAccountingRepository> _accountingRepositoryMock;
        private Mock<IStatusDateSetter> _statusDateSetterMock;
        private Mock<IBudgetAccountToCsvConverter> _budgetAccountToCsvConverterMock;
        private Fixture _fixture;
        private Random _random;
        private static readonly Regex NewLineRegex = new(Environment.NewLine, RegexOptions.Compiled);

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _claimResolverMock = new Mock<IClaimResolver>();
            _accountingRepositoryMock = new Mock<IAccountingRepository>();
            _statusDateSetterMock = new Mock<IStatusDateSetter>();
            _budgetAccountToCsvConverterMock = new Mock<IBudgetAccountToCsvConverter>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void QueryAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            IQueryHandler<IExportBudgetAccountCollectionQuery, byte[]> sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("query"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertAccountingNumberWasCalledOnExportBudgetAccountCollectionQuery()
        {
            IQueryHandler<IExportBudgetAccountCollectionQuery, byte[]> sut = CreateSut();

            Mock<IExportBudgetAccountCollectionQuery> exportBudgetAccountCollectionQueryMock = CreateExportBudgetAccountCollectionQueryMock();
            await sut.QueryAsync(exportBudgetAccountCollectionQueryMock.Object);

            exportBudgetAccountCollectionQueryMock.Verify(m => m.AccountingNumber, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertStatusDateWasCalledTwiceOnExportBudgetAccountCollectionQuery()
        {
            IQueryHandler<IExportBudgetAccountCollectionQuery, byte[]> sut = CreateSut();

            Mock<IExportBudgetAccountCollectionQuery> exportBudgetAccountCollectionQueryMock = CreateExportBudgetAccountCollectionQueryMock();
            await sut.QueryAsync(exportBudgetAccountCollectionQueryMock.Object);

            exportBudgetAccountCollectionQueryMock.Verify(m => m.StatusDate, Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetBudgetAccountsAsyncWasCalledOnAccountingRepository()
        {
            IQueryHandler<IExportBudgetAccountCollectionQuery, byte[]> sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            DateTime statusDate = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
            IExportBudgetAccountCollectionQuery exportBudgetAccountCollectionQuery = CreateExportBudgetAccountCollectionQuery(accountingNumber, statusDate);
            await sut.QueryAsync(exportBudgetAccountCollectionQuery);

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
            IQueryHandler<IExportBudgetAccountCollectionQuery, byte[]> sut = CreateSut(budgetAccountCollection: budgetAccountCollectionMock.Object);

            DateTime statusDate = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
            IExportBudgetAccountCollectionQuery exportBudgetAccountCollectionQuery = CreateExportBudgetAccountCollectionQuery(statusDate: statusDate);
            await sut.QueryAsync(exportBudgetAccountCollectionQuery);

            budgetAccountCollectionMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoBudgetAccountCollectionWasReturned_ReturnsNotNull()
        {
            IQueryHandler<IExportBudgetAccountCollectionQuery, byte[]> sut = CreateSut(false);

            IExportBudgetAccountCollectionQuery exportBudgetAccountCollectionQuery = CreateExportBudgetAccountCollectionQuery();
            byte[] result = await sut.QueryAsync(exportBudgetAccountCollectionQuery);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoBudgetAccountCollectionWasReturned_ReturnsNonEmptyByteArray()
        {
            IQueryHandler<IExportBudgetAccountCollectionQuery, byte[]> sut = CreateSut(false);

            IExportBudgetAccountCollectionQuery exportBudgetAccountCollectionQuery = CreateExportBudgetAccountCollectionQuery();
            byte[] result = await sut.QueryAsync(exportBudgetAccountCollectionQuery);

            Assert.That(result.Length, Is.GreaterThan(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoBudgetAccountCollectionWasReturned_ReturnsNonEmptyByteArrayWithHeaderLineOnly()
        {
            IQueryHandler<IExportBudgetAccountCollectionQuery, byte[]> sut = CreateSut(false);

            IExportBudgetAccountCollectionQuery exportBudgetAccountCollectionQuery = CreateExportBudgetAccountCollectionQuery();
            byte[] result = await sut.QueryAsync(exportBudgetAccountCollectionQuery);

            using MemoryStream memoryStream = new MemoryStream(result);
            using StreamReader reader = new StreamReader(memoryStream);

            MatchCollection match = NewLineRegex.Matches(await reader.ReadToEndAsync());
            Assert.That(match, Is.Not.Null);
            Assert.That(match.Count, Is.EqualTo(1));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenBudgetAccountCollectionWasReturned_ReturnsNotNull()
        {
            IQueryHandler<IExportBudgetAccountCollectionQuery, byte[]> sut = CreateSut();

            IExportBudgetAccountCollectionQuery exportBudgetAccountCollectionQuery = CreateExportBudgetAccountCollectionQuery();
            byte[] result = await sut.QueryAsync(exportBudgetAccountCollectionQuery);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenBudgetAccountCollectionWasReturned_ReturnsNonEmptyByteArray()
        {
            IQueryHandler<IExportBudgetAccountCollectionQuery, byte[]> sut = CreateSut();

            IExportBudgetAccountCollectionQuery exportBudgetAccountCollectionQuery = CreateExportBudgetAccountCollectionQuery();
            byte[] result = await sut.QueryAsync(exportBudgetAccountCollectionQuery);

            Assert.That(result.Length, Is.GreaterThan(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenBudgetAccountCollectionWasReturned_ReturnsNonEmptyByteArrayWithHeaderLineAndOneLinePrBudgetAccount()
        {
            IBudgetAccount[] budgetAccounts =
            {
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object,
                _fixture.BuildBudgetAccountMock().Object
            };
            IBudgetAccountCollection calculatedBudgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock(budgetAccountCollection: budgetAccounts).Object;
            IBudgetAccountCollection budgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock(calculatedBudgetAccountCollection: calculatedBudgetAccountCollection).Object;
            IQueryHandler<IExportBudgetAccountCollectionQuery, byte[]> sut = CreateSut(budgetAccountCollection: budgetAccountCollection);

            IExportBudgetAccountCollectionQuery exportBudgetAccountCollectionQuery = CreateExportBudgetAccountCollectionQuery();
            byte[] result = await sut.QueryAsync(exportBudgetAccountCollectionQuery);

            using MemoryStream memoryStream = new MemoryStream(result);
            using StreamReader reader = new StreamReader(memoryStream);

            MatchCollection match = NewLineRegex.Matches(await reader.ReadToEndAsync());
            Assert.That(match, Is.Not.Null);
            Assert.That(match.Count, Is.EqualTo(1 + budgetAccounts.Length));
        }

        private IQueryHandler<IExportBudgetAccountCollectionQuery, byte[]> CreateSut(bool hasBudgetAccountCollection = true, IBudgetAccountCollection budgetAccountCollection = null)
        {
            _accountingRepositoryMock.Setup(m => m.GetBudgetAccountsAsync(It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(hasBudgetAccountCollection ? budgetAccountCollection ?? _fixture.BuildBudgetAccountCollectionMock().Object : null));

            int columns = _random.Next(5, 10);
            _budgetAccountToCsvConverterMock.Setup(m => m.GetColumnNamesAsync())
                .Returns(Task.FromResult(_fixture.CreateMany<string>(columns).ToArray().AsEnumerable()));
            _budgetAccountToCsvConverterMock.Setup(m => m.ConvertAsync(It.IsAny<IBudgetAccount>()))
                .Returns(Task.FromResult(_fixture.CreateMany<string>(columns).ToArray().AsEnumerable()));

            return new BusinessLogic.Accounting.QueryHandlers.ExportBudgetAccountCollectionQueryHandler(_validatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object, _statusDateSetterMock.Object, _budgetAccountToCsvConverterMock.Object);
        }

        private IExportBudgetAccountCollectionQuery CreateExportBudgetAccountCollectionQuery(int? accountingNumber = null, DateTime? statusDate = null)
        {
            return CreateExportBudgetAccountCollectionQueryMock(accountingNumber, statusDate).Object;
        }

        private Mock<IExportBudgetAccountCollectionQuery> CreateExportBudgetAccountCollectionQueryMock(int? accountingNumber = null, DateTime? statusDate = null)
        {
            Mock<IExportBudgetAccountCollectionQuery> exportBudgetAccountCollectionQueryMock = new Mock<IExportBudgetAccountCollectionQuery>();
            exportBudgetAccountCollectionQueryMock.Setup(m => m.AccountingNumber)
                .Returns(accountingNumber ?? _fixture.Create<int>());
            exportBudgetAccountCollectionQueryMock.Setup(m => m.StatusDate)
                .Returns(statusDate ?? DateTime.Today.AddDays(_random.Next(0, 7) * -1));
            return exportBudgetAccountCollectionQueryMock;
        }
    }
}