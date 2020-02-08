using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using QueryHandler=OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers.GetAccountingQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.QueryHandlers.GetAccountingQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests : BusinessLogicTestBase
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<IAccountingRepository> _accountingRepositoryMock;
        private Mock<IAccountingHelper> _accountingHelperMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _accountingRepositoryMock = new Mock<IAccountingRepository>();
            _accountingHelperMock = new Mock<IAccountingHelper>();

            _fixture = new Fixture();
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
        public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnGetAccountingQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetAccountingQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.Validate(
                    It.Is<IValidator>(value => value == _validatorMock.Object),
                    It.Is<IAccountingRepository>(value => value == _accountingRepositoryMock.Object)), 
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertAccountingNumberWasCalledOnGetAccountingQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetAccountingQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.AccountingNumber, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertStatusDateWasCalledOnGetAccountingQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetAccountingQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.StatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetAccountingAsyncWasCalledOnAccountingRepository()
        {
            QueryHandler sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            DateTime statusDate = _fixture.Create<DateTime>();
            IGetAccountingQuery query = CreateQueryMock(accountingNumber, statusDate).Object;
            await sut.QueryAsync(query);

            _accountingRepositoryMock.Verify(m => m.GetAccountingAsync(
                    It.Is<int>(value => value == accountingNumber), 
                    It.Is<DateTime>(value => value == statusDate)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledAndAccountingWasNotReturnedFromAccountingRepository_AssertCalculateAsyncWasNotCalledOnAccounting()
        {
            Mock<IAccounting> accountingMock = _fixture.BuildAccountingMock();
            QueryHandler sut = CreateSut(false, accountingMock.Object);

            IGetAccountingQuery query = CreateQueryMock().Object;
            await sut.QueryAsync(query);

            accountingMock.Verify(m => m.CalculateAsync(It.IsAny<DateTime>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledAndAccountingWasNotReturnedFromAccountingRepository_AssertApplyLogicForPrincipalWasNotCalledOnAccountingHelper()
        {
            QueryHandler sut = CreateSut(false);

            IGetAccountingQuery query = CreateQueryMock().Object;
            await sut.QueryAsync(query);

            _accountingHelperMock.Verify(m => m.ApplyLogicForPrincipal(It.IsAny<IAccounting>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledAndAccountingWasNotReturnedFromAccountingRepository_ReturnsNull()
        {
            QueryHandler sut = CreateSut(false);

            IGetAccountingQuery query = CreateQueryMock().Object;
            IAccounting result = await sut.QueryAsync(query);

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledAndAccountingWasReturnedFromAccountingRepository_AssertCalculateAsyncWasCalledOnAccounting()
        {
            Mock<IAccounting> accountingMock = _fixture.BuildAccountingMock();
            QueryHandler sut = CreateSut(accounting: accountingMock.Object);

            DateTime statusDate = _fixture.Create<DateTime>();
            IGetAccountingQuery query = CreateQueryMock(statusDate: statusDate).Object;
            await sut.QueryAsync(query);

            accountingMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledAndCalculatedAccountingWasNotReturnedFromAccounting_AssertApplyLogicForPrincipalWasNotCalledOnAccountingHelper()
        {
            IAccounting accounting = _fixture.BuildAccountingMock(false).Object;
            QueryHandler sut = CreateSut(accounting: accounting);

            IGetAccountingQuery query = CreateQueryMock().Object;
            await sut.QueryAsync(query);

            _accountingHelperMock.Verify(m => m.ApplyLogicForPrincipal(It.IsAny<IAccounting>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledAndCalculatedAccountingWasNotReturnedFromAccounting_ReturnsNull()
        {
            IAccounting accounting = _fixture.BuildAccountingMock(false).Object;
            QueryHandler sut = CreateSut(accounting: accounting);

            IGetAccountingQuery query = CreateQueryMock().Object;
            IAccounting result = await sut.QueryAsync(query);

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledAndCalculatedAccountingWasReturnedFromAccounting_AssertApplyLogicForPrincipalWasCalledOnAccountingHelperWithCalculatedAccounting()
        {
            IAccounting calculatedAccounting = _fixture.BuildAccountingMock().Object;
            IAccounting accounting = _fixture.BuildAccountingMock(calculatedAccounting: calculatedAccounting).Object;
            QueryHandler sut = CreateSut(accounting: accounting);

            IGetAccountingQuery query = CreateQueryMock().Object;
            await sut.QueryAsync(query);

            _accountingHelperMock.Verify(m => m.ApplyLogicForPrincipal(It.Is<IAccounting>(value => value == calculatedAccounting)), Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledAndCalculatedAccountingWasReturnedFromAccounting_ReturnsCalculatedAccountingFromAccountingHelper()
        {
            IAccounting calculatedAccounting = _fixture.BuildAccountingMock().Object;
            IAccounting accounting = _fixture.BuildAccountingMock(calculatedAccounting: calculatedAccounting).Object;
            QueryHandler sut = CreateSut(accounting: accounting, calculatedAccounting: calculatedAccounting);

            IGetAccountingQuery query = CreateQueryMock().Object;
            IAccounting result = await sut.QueryAsync(query);

            Assert.That(result, Is.EqualTo(calculatedAccounting));
        }

        private QueryHandler CreateSut(bool hasAccounting = true, IAccounting accounting = null, IAccounting calculatedAccounting = null)
        {
            _accountingRepositoryMock.Setup(m => m.GetAccountingAsync(It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(Task.Run(() => hasAccounting ? accounting ?? _fixture.BuildAccountingMock().Object : null));
            _accountingHelperMock.Setup(m => m.ApplyLogicForPrincipal(It.IsAny<IAccounting>()))
                .Returns(calculatedAccounting ?? _fixture.BuildAccountingMock().Object);

            return new QueryHandler(_validatorMock.Object, _accountingRepositoryMock.Object, _accountingHelperMock.Object);
        }

        private Mock<IGetAccountingQuery> CreateQueryMock(int? accountingNumber = null, DateTime? statusDate = null)
        {
            Mock<IGetAccountingQuery> queryMock = new Mock<IGetAccountingQuery>();
            queryMock.Setup(m => m.AccountingNumber)
                .Returns(accountingNumber ?? _fixture.Create<int>());
            queryMock.Setup(m => m.StatusDate)
                .Returns(statusDate ?? _fixture.Create<DateTime>());
            return queryMock;
        }
    }
}