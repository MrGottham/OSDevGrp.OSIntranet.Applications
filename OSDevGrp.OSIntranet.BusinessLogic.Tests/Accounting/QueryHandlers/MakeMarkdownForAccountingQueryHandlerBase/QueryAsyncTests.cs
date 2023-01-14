using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Text;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.QueryHandlers.MakeMarkdownForAccountingQueryHandlerBase
{
    [TestFixture]
    public class QueryAsyncTests
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<IClaimResolver> _claimResolverMock;
        private Mock<IAccountingRepository> _accountingRepositoryMock;
        private Mock<IStatusDateSetter> _statusDateSetterMock;
        private Mock<IAccountingToMarkdownConverter> _accountingToMarkdownConverterMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _claimResolverMock = new Mock<IClaimResolver>();
            _accountingRepositoryMock = new Mock<IAccountingRepository>();
            _statusDateSetterMock = new Mock<IStatusDateSetter>();
            _accountingToMarkdownConverterMock = new Mock<IAccountingToMarkdownConverter>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertAccountingNumberWasCalledOnExportFromAccountingQuery()
        {
            IQueryHandler<IExportFromAccountingQuery, byte[]> sut = CreateSut();

            Mock<IExportFromAccountingQuery> exportFromAccountingQueryMock = CreateExportFromAccountingQueryMock();
            await sut.QueryAsync(exportFromAccountingQueryMock.Object);

            exportFromAccountingQueryMock.Verify(m => m.AccountingNumber, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertStatusDateWasCalledTwiceOnExportFromAccountingQuery()
        {
            IQueryHandler<IExportFromAccountingQuery, byte[]> sut = CreateSut();

            Mock<IExportFromAccountingQuery> exportFromAccountingQueryMock = CreateExportFromAccountingQueryMock();
            await sut.QueryAsync(exportFromAccountingQueryMock.Object);

            exportFromAccountingQueryMock.Verify(m => m.StatusDate, Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetAccountingAsyncWasCalledOnAccountingRepository()
        {
            IQueryHandler<IExportFromAccountingQuery, byte[]> sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            DateTime statusDate = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
            IExportFromAccountingQuery exportFromAccountingQuery = CreateExportFromAccountingQuery(accountingNumber, statusDate);
            await sut.QueryAsync(exportFromAccountingQuery);

            _accountingRepositoryMock.Verify(m => m.GetAccountingAsync(
                    It.Is<int>(value => value == accountingNumber),
                    It.Is<DateTime>(value => value == statusDate)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenAccountingWasReturned_AssertCalculateAsyncWasCalledOnAccounting()
        {
            Mock<IAccounting> accountingMock = _fixture.BuildAccountingMock();
            IQueryHandler<IExportFromAccountingQuery, byte[]> sut = CreateSut(accounting: accountingMock.Object);

            DateTime statusDate = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
            IExportFromAccountingQuery exportFromAccountingQuery = CreateExportFromAccountingQuery(statusDate: statusDate);
            await sut.QueryAsync(exportFromAccountingQuery);

            accountingMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoAccountingWasReturned_ReturnsNotNull()
        {
            IQueryHandler<IExportFromAccountingQuery, byte[]> sut = CreateSut(false);

            IExportFromAccountingQuery exportAccountCollectionQuery = CreateExportFromAccountingQuery();
            byte[] result = await sut.QueryAsync(exportAccountCollectionQuery);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoAccountingWasReturned_ReturnsEmptyByteArray()
        {
            IQueryHandler<IExportFromAccountingQuery, byte[]> sut = CreateSut(false);

            IExportFromAccountingQuery exportAccountCollectionQuery = CreateExportFromAccountingQuery();
            byte[] result = await sut.QueryAsync(exportAccountCollectionQuery);

            Assert.That(result.Length, Is.EqualTo(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoCalculatedAccountingWasReturnedFromAccounting_ReturnsNotNull()
        {
            IAccounting accounting = _fixture.BuildAccountingMock(hasCalculatedAccounting: false).Object;
            IQueryHandler<IExportFromAccountingQuery, byte[]> sut = CreateSut(accounting: accounting);

            IExportFromAccountingQuery exportAccountCollectionQuery = CreateExportFromAccountingQuery();
            byte[] result = await sut.QueryAsync(exportAccountCollectionQuery);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoCalculatedAccountingWasReturnedFromAccounting_ReturnsEmptyByteArray()
        {
            IAccounting accounting = _fixture.BuildAccountingMock(hasCalculatedAccounting: false).Object;
            IQueryHandler<IExportFromAccountingQuery, byte[]> sut = CreateSut(accounting: accounting);

            IExportFromAccountingQuery exportAccountCollectionQuery = CreateExportFromAccountingQuery();
            byte[] result = await sut.QueryAsync(exportAccountCollectionQuery);

            Assert.That(result.Length, Is.EqualTo(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoMarkdownWasReturnedForCalculatedAccounting_ReturnsNotNull()
        {
            IQueryHandler<IExportFromAccountingQuery, byte[]> sut = CreateSut(hasMarkdownContent: false);

            IExportFromAccountingQuery exportAccountCollectionQuery = CreateExportFromAccountingQuery();
            byte[] result = await sut.QueryAsync(exportAccountCollectionQuery);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoMarkdownWasReturnedForCalculatedAccounting_ReturnsEmptyByteArray()
        {
            IQueryHandler<IExportFromAccountingQuery, byte[]> sut = CreateSut(hasMarkdownContent: false);

            IExportFromAccountingQuery exportAccountCollectionQuery = CreateExportFromAccountingQuery();
            byte[] result = await sut.QueryAsync(exportAccountCollectionQuery);

            Assert.That(result.Length, Is.EqualTo(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenMarkdownWasReturnedForCalculatedAccounting_ReturnsNotNull()
        {
            IQueryHandler<IExportFromAccountingQuery, byte[]> sut = CreateSut();

            IExportFromAccountingQuery exportAccountCollectionQuery = CreateExportFromAccountingQuery();
            byte[] result = await sut.QueryAsync(exportAccountCollectionQuery);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenMarkdownWasReturnedForCalculatedAccounting_ReturnsNonEmptyByteArray()
        {
            IQueryHandler<IExportFromAccountingQuery, byte[]> sut = CreateSut();

            IExportFromAccountingQuery exportAccountCollectionQuery = CreateExportFromAccountingQuery();
            byte[] result = await sut.QueryAsync(exportAccountCollectionQuery);

            Assert.That(result.Length, Is.GreaterThan(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenMarkdownWasReturnedForCalculatedAccounting_ReturnsByteArrayForMarkdownContent()
        {
            string markdownContent = _fixture.Create<string>();
            IQueryHandler<IExportFromAccountingQuery, byte[]> sut = CreateSut(markdownContent: markdownContent);

            IExportFromAccountingQuery exportAccountCollectionQuery = CreateExportFromAccountingQuery();
            byte[] result = await sut.QueryAsync(exportAccountCollectionQuery);

            Assert.That(Encoding.UTF8.GetString(result), Is.EqualTo(markdownContent));
        }

        private IQueryHandler<IExportFromAccountingQuery, byte[]> CreateSut(bool hasAccounting = true, IAccounting accounting = null, bool hasMarkdownContent = true, string markdownContent = null)
        {
            _accountingRepositoryMock.Setup(m => m.GetAccountingAsync(It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(hasAccounting ? accounting ?? _fixture.BuildAccountingMock().Object : null));

            _accountingToMarkdownConverterMock.Setup(m => m.ConvertAsync(It.IsAny<IAccounting>()))
                .Returns(Task.FromResult(hasMarkdownContent ? markdownContent ?? _fixture.Create<string>() : null));

            return new MyMakeMarkdownForAccountingQueryHandler(_validatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object, _statusDateSetterMock.Object, _accountingToMarkdownConverterMock.Object, false);
        }

        private IExportFromAccountingQuery CreateExportFromAccountingQuery(int? accountingNumber = null, DateTime? statusDate = null)
        {
            return CreateExportFromAccountingQueryMock(accountingNumber, statusDate).Object;
        }

        private Mock<IExportFromAccountingQuery> CreateExportFromAccountingQueryMock(int? accountingNumber = null, DateTime? statusDate = null)
        {
            Mock<IExportFromAccountingQuery> exportFromAccountingMock = new Mock<IExportFromAccountingQuery>();
            exportFromAccountingMock.Setup(m => m.AccountingNumber)
                .Returns(accountingNumber ?? _fixture.Create<int>());
            exportFromAccountingMock.Setup(m => m.StatusDate)
                .Returns(statusDate ?? DateTime.Today.AddDays(_random.Next(0, 7) * -1));
            return exportFromAccountingMock;
        }

        private class MyMakeMarkdownForAccountingQueryHandler : MakeMarkdownForAccountingQueryHandlerBase<IExportFromAccountingQuery, IAccountingToMarkdownConverter>
        {
            #region Constructor

            public MyMakeMarkdownForAccountingQueryHandler(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository, IStatusDateSetter statusDateSetter, IAccountingToMarkdownConverter accountingToMarkdownConverter, bool encoderShouldEmitUtf8Identifier = true) 
                : base(validator, claimResolver, accountingRepository, statusDateSetter, accountingToMarkdownConverter, encoderShouldEmitUtf8Identifier)
            {
            }

            #endregion
        }
    }
}
