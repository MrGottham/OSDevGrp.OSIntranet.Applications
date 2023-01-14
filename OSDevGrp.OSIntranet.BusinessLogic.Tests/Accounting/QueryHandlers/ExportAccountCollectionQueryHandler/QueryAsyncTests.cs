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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.QueryHandlers.ExportAccountCollectionQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<IClaimResolver> _claimResolverMock;
        private Mock<IAccountingRepository> _accountingRepositoryMock;
        private Mock<IStatusDateSetter> _statusDateSetterMock;
        private Mock<IAccountToCsvConverter> _accountToCsvConverterMock;
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
            _accountToCsvConverterMock = new Mock<IAccountToCsvConverter>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void QueryAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            IQueryHandler<IExportAccountCollectionQuery, byte[]> sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("query"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertAccountingNumberWasCalledOnExportAccountCollectionQuery()
        {
            IQueryHandler<IExportAccountCollectionQuery, byte[]> sut = CreateSut();

            Mock<IExportAccountCollectionQuery> exportAccountCollectionQueryMock = CreateExportAccountCollectionQueryMock();
            await sut.QueryAsync(exportAccountCollectionQueryMock.Object);

            exportAccountCollectionQueryMock.Verify(m => m.AccountingNumber, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertStatusDateWasCalledTwiceOnExportAccountCollectionQuery()
        {
            IQueryHandler<IExportAccountCollectionQuery, byte[]> sut = CreateSut();

            Mock<IExportAccountCollectionQuery> exportAccountCollectionQueryMock = CreateExportAccountCollectionQueryMock();
            await sut.QueryAsync(exportAccountCollectionQueryMock.Object);

            exportAccountCollectionQueryMock.Verify(m => m.StatusDate, Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetAccountsAsyncWasCalledOnAccountingRepository()
        {
            IQueryHandler<IExportAccountCollectionQuery, byte[]> sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            DateTime statusDate = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
            IExportAccountCollectionQuery exportAccountCollectionQuery = CreateExportAccountCollectionQuery(accountingNumber, statusDate);
            await sut.QueryAsync(exportAccountCollectionQuery);

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
            IQueryHandler<IExportAccountCollectionQuery, byte[]> sut = CreateSut(accountCollection: accountCollectionMock.Object);

            DateTime statusDate = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
            IExportAccountCollectionQuery exportAccountCollectionQuery = CreateExportAccountCollectionQuery(statusDate: statusDate);
            await sut.QueryAsync(exportAccountCollectionQuery);

            accountCollectionMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoAccountCollectionWasReturned_ReturnsNotNull()
        {
            IQueryHandler<IExportAccountCollectionQuery, byte[]> sut = CreateSut(false);

            IExportAccountCollectionQuery exportAccountCollectionQuery = CreateExportAccountCollectionQuery();
            byte[] result = await sut.QueryAsync(exportAccountCollectionQuery);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoAccountCollectionWasReturned_ReturnsNonEmptyByteArray()
        {
            IQueryHandler<IExportAccountCollectionQuery, byte[]> sut = CreateSut(false);

            IExportAccountCollectionQuery exportAccountCollectionQuery = CreateExportAccountCollectionQuery();
            byte[] result = await sut.QueryAsync(exportAccountCollectionQuery);

            Assert.That(result.Length, Is.GreaterThan(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoAccountCollectionWasReturned_ReturnsNonEmptyByteArrayWithHeaderLineOnly()
        {
            IQueryHandler<IExportAccountCollectionQuery, byte[]> sut = CreateSut(false);

            IExportAccountCollectionQuery exportAccountCollectionQuery = CreateExportAccountCollectionQuery();
            byte[] result = await sut.QueryAsync(exportAccountCollectionQuery);

            using MemoryStream memoryStream = new MemoryStream(result);
            using StreamReader reader = new StreamReader(memoryStream);

            MatchCollection match = NewLineRegex.Matches(await reader.ReadToEndAsync());
            Assert.That(match, Is.Not.Null);
            Assert.That(match.Count, Is.EqualTo(1));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenAccountCollectionWasReturned_ReturnsNotNull()
        {
            IQueryHandler<IExportAccountCollectionQuery, byte[]> sut = CreateSut();

            IExportAccountCollectionQuery exportAccountCollectionQuery = CreateExportAccountCollectionQuery();
            byte[] result = await sut.QueryAsync(exportAccountCollectionQuery);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenAccountCollectionWasReturned_ReturnsNonEmptyByteArray()
        {
            IQueryHandler<IExportAccountCollectionQuery, byte[]> sut = CreateSut();

            IExportAccountCollectionQuery exportAccountCollectionQuery = CreateExportAccountCollectionQuery();
            byte[] result = await sut.QueryAsync(exportAccountCollectionQuery);

            Assert.That(result.Length, Is.GreaterThan(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenAccountCollectionWasReturned_ReturnsNonEmptyByteArrayWithHeaderLineAndOneLinePrAccount()
        {
            IAccount[] accounts =
            {
                _fixture.BuildAccountMock().Object,
                _fixture.BuildAccountMock().Object,
                _fixture.BuildAccountMock().Object
            };
            IAccountCollection calculatedAccountCollection = _fixture.BuildAccountCollectionMock(accountCollection: accounts).Object;
            IAccountCollection accountCollection = _fixture.BuildAccountCollectionMock(calculatedAccountCollection: calculatedAccountCollection).Object;
            IQueryHandler<IExportAccountCollectionQuery, byte[]> sut = CreateSut(accountCollection: accountCollection);

            IExportAccountCollectionQuery exportAccountCollectionQuery = CreateExportAccountCollectionQuery();
            byte[] result = await sut.QueryAsync(exportAccountCollectionQuery);

            using MemoryStream memoryStream = new MemoryStream(result);
            using StreamReader reader = new StreamReader(memoryStream);

            MatchCollection match = NewLineRegex.Matches(await reader.ReadToEndAsync());
            Assert.That(match, Is.Not.Null);
            Assert.That(match.Count, Is.EqualTo(1 + accounts.Length));
        }

        private IQueryHandler<IExportAccountCollectionQuery, byte[]> CreateSut(bool hasAccountCollection = true, IAccountCollection accountCollection = null)
        {
            _accountingRepositoryMock.Setup(m => m.GetAccountsAsync(It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(hasAccountCollection ? accountCollection ?? _fixture.BuildAccountCollectionMock().Object : null));

            int columns = _random.Next(5, 10);
            _accountToCsvConverterMock.Setup(m => m.GetColumnNamesAsync())
                .Returns(Task.FromResult(_fixture.CreateMany<string>(columns).ToArray().AsEnumerable()));
            _accountToCsvConverterMock.Setup(m => m.ConvertAsync(It.IsAny<IAccount>()))
                .Returns(Task.FromResult(_fixture.CreateMany<string>(columns).ToArray().AsEnumerable()));

            return new BusinessLogic.Accounting.QueryHandlers.ExportAccountCollectionQueryHandler(_validatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object, _statusDateSetterMock.Object, _accountToCsvConverterMock.Object);
        }

        private IExportAccountCollectionQuery CreateExportAccountCollectionQuery(int? accountingNumber = null, DateTime? statusDate = null)
        {
            return CreateExportAccountCollectionQueryMock(accountingNumber, statusDate).Object;
        }

        private Mock<IExportAccountCollectionQuery> CreateExportAccountCollectionQueryMock(int? accountingNumber = null, DateTime? statusDate = null)
        {
            Mock<IExportAccountCollectionQuery> exportAccountCollectionQueryMock = new Mock<IExportAccountCollectionQuery>();
            exportAccountCollectionQueryMock.Setup(m => m.AccountingNumber)
                .Returns(accountingNumber ?? _fixture.Create<int>());
            exportAccountCollectionQueryMock.Setup(m => m.StatusDate)
                .Returns(statusDate ?? DateTime.Today.AddDays(_random.Next(0, 7) * -1));
            return exportAccountCollectionQueryMock;
        }
    }
}