using System;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.QueryHandlers.MakeMarkdownForBudgetAccountQueryHandlerBase
{
    [TestFixture]
    public class QueryAsyncTests
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<IAccountingRepository> _accountingRepositoryMock;
        private Mock<IStatusDateSetter> _statusDateSetterMock;
        private Mock<IAccountToMarkdownConverter<IBudgetAccount>> _budgetAccountToMarkdownConverterMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _accountingRepositoryMock = new Mock<IAccountingRepository>();
            _statusDateSetterMock = new Mock<IStatusDateSetter>();
            _budgetAccountToMarkdownConverterMock = new Mock<IAccountToMarkdownConverter<IBudgetAccount>>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void QueryAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            IQueryHandler<IExportFromAccountQuery, byte[]> sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("query"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertAccountingNumberWasCalledOnExportFromAccountQuery()
        {
            IQueryHandler<IExportFromAccountQuery, byte[]> sut = CreateSut();

            Mock<IExportFromAccountQuery> exportFromAccountQueryMock = CreateExportFromAccountQueryMock();
            await sut.QueryAsync(exportFromAccountQueryMock.Object);

            exportFromAccountQueryMock.Verify(m => m.AccountingNumber, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertAccountNumberWasCalledOnExportFromAccountQuery()
        {
            IQueryHandler<IExportFromAccountQuery, byte[]> sut = CreateSut();

            Mock<IExportFromAccountQuery> exportFromAccountQueryMock = CreateExportFromAccountQueryMock();
            await sut.QueryAsync(exportFromAccountQueryMock.Object);

            exportFromAccountQueryMock.Verify(m => m.AccountNumber, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertStatusDateWasCalledTwiceOnExportFromAccountQuery()
        {
            IQueryHandler<IExportFromAccountQuery, byte[]> sut = CreateSut();

            Mock<IExportFromAccountQuery> exportFromAccountQueryMock = CreateExportFromAccountQueryMock();
            await sut.QueryAsync(exportFromAccountQueryMock.Object);

            exportFromAccountQueryMock.Verify(m => m.StatusDate, Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetBudgetAccountAsyncWasCalledOnAccountingRepository()
        {
            IQueryHandler<IExportFromAccountQuery, byte[]> sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            string accountNumber = _fixture.Create<string>();
            DateTime statusDate = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
            IExportFromAccountQuery exportFromAccountQuery = CreateExportFromAccountQuery(accountingNumber, accountNumber, statusDate);
            await sut.QueryAsync(exportFromAccountQuery);

            _accountingRepositoryMock.Verify(m => m.GetBudgetAccountAsync(
                    It.Is<int>(value => value == accountingNumber),
                    It.Is<string>(value => string.CompareOrdinal(value, accountNumber) == 0),
                    It.Is<DateTime>(value => value == statusDate)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenBudgetAccountWasReturned_AssertCalculateAsyncWasCalledOnBudgetAccount()
        {
            Mock<IBudgetAccount> budgetAccountMock = _fixture.BuildBudgetAccountMock();
            IQueryHandler<IExportFromAccountQuery, byte[]> sut = CreateSut(budgetAccount: budgetAccountMock.Object);

            DateTime statusDate = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
            IExportFromAccountQuery exportFromAccountQuery = CreateExportFromAccountQuery(statusDate: statusDate);
            await sut.QueryAsync(exportFromAccountQuery);

            budgetAccountMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalculatedBudgetAccountWasReturnedFromBudgetAccount_AssertConvertAsyncWasCalledOnBudgetAccountToMarkdownConverterWithCalculatedBudgetAccount()
        {
            IBudgetAccount calculatedBudgetAccount = _fixture.BuildBudgetAccountMock().Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(calculatedBudgetAccount: calculatedBudgetAccount).Object;
            IQueryHandler<IExportFromAccountQuery, byte[]> sut = CreateSut(budgetAccount: budgetAccount);

            IExportFromAccountQuery exportFromAccountQuery = CreateExportFromAccountQuery();
            await sut.QueryAsync(exportFromAccountQuery);

            _budgetAccountToMarkdownConverterMock.Verify(m => m.ConvertAsync(It.Is<IBudgetAccount>(value => value != null && value == calculatedBudgetAccount)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoBudgetAccountWasReturned_ReturnsNotNull()
        {
            IQueryHandler<IExportFromAccountQuery, byte[]> sut = CreateSut(false);

            IExportFromAccountQuery exportFromAccountQuery = CreateExportFromAccountQuery();
            byte[] result = await sut.QueryAsync(exportFromAccountQuery);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoBudgetAccountWasReturned_ReturnsEmptyByteArray()
        {
            IQueryHandler<IExportFromAccountQuery, byte[]> sut = CreateSut(false);

            IExportFromAccountQuery exportFromAccountQuery = CreateExportFromAccountQuery();
            byte[] result = await sut.QueryAsync(exportFromAccountQuery);

            Assert.That(result.Length, Is.EqualTo(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoCalculatedBudgetAccountWasReturnedFromBudgetAccount_ReturnsNotNull()
        {
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(hasCalculatedBudgetAccount: false).Object;
            IQueryHandler<IExportFromAccountQuery, byte[]> sut = CreateSut(budgetAccount: budgetAccount);

            IExportFromAccountQuery exportFromAccountQuery = CreateExportFromAccountQuery();
            byte[] result = await sut.QueryAsync(exportFromAccountQuery);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoCalculatedBudgetAccountWasReturnedFromBudgetAccount_ReturnsEmptyByteArray()
        {
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(hasCalculatedBudgetAccount: false).Object;
            IQueryHandler<IExportFromAccountQuery, byte[]> sut = CreateSut(budgetAccount: budgetAccount);

            IExportFromAccountQuery exportFromAccountQuery = CreateExportFromAccountQuery();
            byte[] result = await sut.QueryAsync(exportFromAccountQuery);

            Assert.That(result.Length, Is.EqualTo(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoMarkdownWasReturnedForCalculatedBudgetAccount_ReturnsNotNull()
        {
            IQueryHandler<IExportFromAccountQuery, byte[]> sut = CreateSut(hasMarkdownContent: false);

            IExportFromAccountQuery exportFromAccountQuery = CreateExportFromAccountQuery();
            byte[] result = await sut.QueryAsync(exportFromAccountQuery);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoMarkdownWasReturnedForCalculatedBudgetAccount_ReturnsEmptyByteArray()
        {
            IQueryHandler<IExportFromAccountQuery, byte[]> sut = CreateSut(hasMarkdownContent: false);

            IExportFromAccountQuery exportFromAccountQuery = CreateExportFromAccountQuery();
            byte[] result = await sut.QueryAsync(exportFromAccountQuery);

            Assert.That(result.Length, Is.EqualTo(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenMarkdownWasReturnedForCalculatedBudgetAccount_ReturnsNotNull()
        {
            IQueryHandler<IExportFromAccountQuery, byte[]> sut = CreateSut();

            IExportFromAccountQuery exportFromAccountQuery = CreateExportFromAccountQuery();
            byte[] result = await sut.QueryAsync(exportFromAccountQuery);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenMarkdownWasReturnedForCalculatedBudgetAccount_ReturnsNonEmptyByteArray()
        {
            IQueryHandler<IExportFromAccountQuery, byte[]> sut = CreateSut();

            IExportFromAccountQuery exportFromAccountQuery = CreateExportFromAccountQuery();
            byte[] result = await sut.QueryAsync(exportFromAccountQuery);

            Assert.That(result.Length, Is.GreaterThan(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenMarkdownWasReturnedForCalculatedBudgetAccount_ReturnsByteArrayForMarkdownContent()
        {
            string markdownContent = _fixture.Create<string>();
            IQueryHandler<IExportFromAccountQuery, byte[]> sut = CreateSut(markdownContent: markdownContent);

            IExportFromAccountQuery exportFromAccountQuery = CreateExportFromAccountQuery();
            byte[] result = await sut.QueryAsync(exportFromAccountQuery);

            Assert.That(Encoding.UTF8.GetString(result), Is.EqualTo(markdownContent));
        }

        private IQueryHandler<IExportFromAccountQuery, byte[]> CreateSut(bool hasBudgetAccount = true, IBudgetAccount budgetAccount = null, bool hasMarkdownContent = true, string markdownContent = null)
        {
            _accountingRepositoryMock.Setup(m => m.GetBudgetAccountAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(hasBudgetAccount ? budgetAccount ?? _fixture.BuildBudgetAccountMock().Object : null));

            _budgetAccountToMarkdownConverterMock.Setup(m => m.ConvertAsync(It.IsAny<IBudgetAccount>()))
                .Returns(Task.FromResult(hasMarkdownContent ? markdownContent ?? _fixture.Create<string>() : null));

            return new MyMakeMarkdownForBudgetAccountQueryHandler(_validatorMock.Object, _accountingRepositoryMock.Object, _statusDateSetterMock.Object, _budgetAccountToMarkdownConverterMock.Object, false);
        }

        private IExportFromAccountQuery CreateExportFromAccountQuery(int? accountingNumber = null, string accountNumber = null, DateTime? statusDate = null)
        {
            return CreateExportFromAccountQueryMock(accountingNumber, accountNumber, statusDate).Object;
        }

        private Mock<IExportFromAccountQuery> CreateExportFromAccountQueryMock(int? accountingNumber = null, string accountNumber = null, DateTime? statusDate = null)
        {
            Mock<IExportFromAccountQuery> exportFromAccountQueryMock = new Mock<IExportFromAccountQuery>();
            exportFromAccountQueryMock.Setup(m => m.AccountingNumber)
                .Returns(accountingNumber ?? _fixture.Create<int>());
            exportFromAccountQueryMock.Setup(m => m.AccountNumber)
                .Returns(accountNumber ?? _fixture.Create<string>());
            exportFromAccountQueryMock.Setup(m => m.StatusDate)
                .Returns(statusDate ?? DateTime.Today.AddDays(_random.Next(0, 7) * -1));
            return exportFromAccountQueryMock;
        }

        private class MyMakeMarkdownForBudgetAccountQueryHandler : MakeMarkdownForBudgetAccountQueryHandlerBase<IExportFromAccountQuery, IAccountToMarkdownConverter<IBudgetAccount>>
        {
            #region Constructor

            public MyMakeMarkdownForBudgetAccountQueryHandler(IValidator validator, IAccountingRepository accountingRepository, IStatusDateSetter statusDateSetter, IAccountToMarkdownConverter<IBudgetAccount> budgetAccountToMarkdownConverter, bool encoderShouldEmitUtf8Identifier = true)
                : base(validator, accountingRepository, statusDateSetter, budgetAccountToMarkdownConverter, encoderShouldEmitUtf8Identifier)
            {
            }

            #endregion
        }
    }
}