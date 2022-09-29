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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.QueryHandlers.ExportContactAccountCollectionQuery
{
    [TestFixture]
    public class QueryAsyncTests
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<IAccountingRepository> _accountingRepositoryMock;
        private Mock<IStatusDateSetter> _statusDateSetterMock;
        private Mock<IContactAccountToCsvConverter> _contactAccountToCsvConverterMock;
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
            _contactAccountToCsvConverterMock = new Mock<IContactAccountToCsvConverter>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void QueryAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            IQueryHandler<IExportContactAccountCollectionQuery, byte[]> sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("query"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertAccountingNumberWasCalledOnExportContactAccountCollectionQuery()
        {
            IQueryHandler<IExportContactAccountCollectionQuery, byte[]> sut = CreateSut();

            Mock<IExportContactAccountCollectionQuery> exportContactAccountCollectionQueryMock = CreateExportContactAccountCollectionQueryMock();
            await sut.QueryAsync(exportContactAccountCollectionQueryMock.Object);

            exportContactAccountCollectionQueryMock.Verify(m => m.AccountingNumber, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertStatusDateWasCalledTwiceOnExportContactAccountCollectionQuery()
        {
            IQueryHandler<IExportContactAccountCollectionQuery, byte[]> sut = CreateSut();

            Mock<IExportContactAccountCollectionQuery> exportContactAccountCollectionQueryMock = CreateExportContactAccountCollectionQueryMock();
            await sut.QueryAsync(exportContactAccountCollectionQueryMock.Object);

            exportContactAccountCollectionQueryMock.Verify(m => m.StatusDate, Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetContactAccountsAsyncWasCalledOnAccountingRepository()
        {
            IQueryHandler<IExportContactAccountCollectionQuery, byte[]> sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            DateTime statusDate = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
            IExportContactAccountCollectionQuery exportContactAccountCollectionQuery = CreateExportContactAccountCollectionQuery(accountingNumber, statusDate);
            await sut.QueryAsync(exportContactAccountCollectionQuery);

            _accountingRepositoryMock.Verify(m => m.GetContactAccountsAsync(
                    It.Is<int>(value => value == accountingNumber),
                    It.Is<DateTime>(value => value == statusDate)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenContactAccountCollectionWasReturned_AssertCalculateAsyncWasCalledOnContactAccountCollection()
        {
            Mock<IContactAccountCollection> contactAccountCollectionMock = _fixture.BuildContactAccountCollectionMock();
            IQueryHandler<IExportContactAccountCollectionQuery, byte[]> sut = CreateSut(contactAccountCollection: contactAccountCollectionMock.Object);

            DateTime statusDate = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
            IExportContactAccountCollectionQuery exportContactAccountCollectionQuery = CreateExportContactAccountCollectionQuery(statusDate: statusDate);
            await sut.QueryAsync(exportContactAccountCollectionQuery);

            contactAccountCollectionMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoContactAccountCollectionWasReturned_ReturnsNotNull()
        {
            IQueryHandler<IExportContactAccountCollectionQuery, byte[]> sut = CreateSut(false);

            IExportContactAccountCollectionQuery exportContactAccountCollectionQuery = CreateExportContactAccountCollectionQuery();
            byte[] result = await sut.QueryAsync(exportContactAccountCollectionQuery);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoContactAccountCollectionWasReturned_ReturnsNonEmptyByteArray()
        {
            IQueryHandler<IExportContactAccountCollectionQuery, byte[]> sut = CreateSut(false);

            IExportContactAccountCollectionQuery exportContactAccountCollectionQuery = CreateExportContactAccountCollectionQuery();
            byte[] result = await sut.QueryAsync(exportContactAccountCollectionQuery);

            Assert.That(result.Length, Is.GreaterThan(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoContactAccountCollectionWasReturned_ReturnsNonEmptyByteArrayWithHeaderLineOnly()
        {
            IQueryHandler<IExportContactAccountCollectionQuery, byte[]> sut = CreateSut(false);

            IExportContactAccountCollectionQuery exportContactAccountCollectionQuery = CreateExportContactAccountCollectionQuery();
            byte[] result = await sut.QueryAsync(exportContactAccountCollectionQuery);

            using MemoryStream memoryStream = new MemoryStream(result);
            using StreamReader reader = new StreamReader(memoryStream);

            MatchCollection match = NewLineRegex.Matches(await reader.ReadToEndAsync());
            Assert.That(match, Is.Not.Null);
            Assert.That(match.Count, Is.EqualTo(1));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenContactAccountCollectionWasReturned_ReturnsNotNull()
        {
            IQueryHandler<IExportContactAccountCollectionQuery, byte[]> sut = CreateSut();

            IExportContactAccountCollectionQuery exportContactAccountCollectionQuery = CreateExportContactAccountCollectionQuery();
            byte[] result = await sut.QueryAsync(exportContactAccountCollectionQuery);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenContactAccountCollectionWasReturned_ReturnsNonEmptyByteArray()
        {
            IQueryHandler<IExportContactAccountCollectionQuery, byte[]> sut = CreateSut();

            IExportContactAccountCollectionQuery exportContactAccountCollectionQuery = CreateExportContactAccountCollectionQuery();
            byte[] result = await sut.QueryAsync(exportContactAccountCollectionQuery);

            Assert.That(result.Length, Is.GreaterThan(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenContactAccountCollectionWasReturned_ReturnsNonEmptyByteArrayWithHeaderLineAndOneLinePrContactAccount()
        {
            IContactAccount[] contactAccounts =
            {
                _fixture.BuildContactAccountMock().Object,
                _fixture.BuildContactAccountMock().Object,
                _fixture.BuildContactAccountMock().Object
            };
            IContactAccountCollection calculatedContactAccountCollection = _fixture.BuildContactAccountCollectionMock(contactAccountCollection: contactAccounts).Object;
            IContactAccountCollection contactAccountCollection = _fixture.BuildContactAccountCollectionMock(calculatedContactAccountCollection: calculatedContactAccountCollection).Object;
            IQueryHandler<IExportContactAccountCollectionQuery, byte[]> sut = CreateSut(contactAccountCollection: contactAccountCollection);

            IExportContactAccountCollectionQuery exportContactAccountCollectionQuery = CreateExportContactAccountCollectionQuery();
            byte[] result = await sut.QueryAsync(exportContactAccountCollectionQuery);

            using MemoryStream memoryStream = new MemoryStream(result);
            using StreamReader reader = new StreamReader(memoryStream);

            MatchCollection match = NewLineRegex.Matches(await reader.ReadToEndAsync());
            Assert.That(match, Is.Not.Null);
            Assert.That(match.Count, Is.EqualTo(1 + contactAccounts.Length));
        }

        private IQueryHandler<IExportContactAccountCollectionQuery, byte[]> CreateSut(bool hasContactAccountCollection = true, IContactAccountCollection contactAccountCollection = null)
        {
            _accountingRepositoryMock.Setup(m => m.GetContactAccountsAsync(It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(hasContactAccountCollection ? contactAccountCollection ?? _fixture.BuildContactAccountCollectionMock().Object : null));

            int columns = _random.Next(5, 10);
            _contactAccountToCsvConverterMock.Setup(m => m.GetColumnNamesAsync())
                .Returns(Task.FromResult(_fixture.CreateMany<string>(columns).ToArray().AsEnumerable()));
            _contactAccountToCsvConverterMock.Setup(m => m.ConvertAsync(It.IsAny<IContactAccount>()))
                .Returns(Task.FromResult(_fixture.CreateMany<string>(columns).ToArray().AsEnumerable()));

            return new BusinessLogic.Accounting.QueryHandlers.ExportContactAccountCollectionQueryHandler(_validatorMock.Object, _accountingRepositoryMock.Object, _statusDateSetterMock.Object, _contactAccountToCsvConverterMock.Object);
        }

        private IExportContactAccountCollectionQuery CreateExportContactAccountCollectionQuery(int? accountingNumber = null, DateTime? statusDate = null)
        {
            return CreateExportContactAccountCollectionQueryMock(accountingNumber, statusDate).Object;
        }

        private Mock<IExportContactAccountCollectionQuery> CreateExportContactAccountCollectionQueryMock(int? accountingNumber = null, DateTime? statusDate = null)
        {
            Mock<IExportContactAccountCollectionQuery> exportContactAccountCollectionQueryMock = new Mock<IExportContactAccountCollectionQuery>();
            exportContactAccountCollectionQueryMock.Setup(m => m.AccountingNumber)
                .Returns(accountingNumber ?? _fixture.Create<int>());
            exportContactAccountCollectionQueryMock.Setup(m => m.StatusDate)
                .Returns(statusDate ?? DateTime.Today.AddDays(_random.Next(0, 7) * -1));
            return exportContactAccountCollectionQueryMock;
        }
    }
}