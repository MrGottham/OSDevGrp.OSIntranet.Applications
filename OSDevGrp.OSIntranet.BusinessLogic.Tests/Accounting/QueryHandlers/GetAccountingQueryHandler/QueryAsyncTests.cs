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
    public class QueryAsyncTests
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

            queryMock.Verify(m => m.StatusDate, Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetAccountingAsyncWasCalledOnAccountingRepository()
        {
            QueryHandler sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            DateTime statusDate = _fixture.Create<DateTime>().Date;
            IGetAccountingQuery query = CreateQuery(accountingNumber, statusDate);
            await sut.QueryAsync(query);

            _accountingRepositoryMock.Verify(m => m.GetAccountingAsync(
                    It.Is<int>(value => value == accountingNumber),
                    It.Is<DateTime>(value => value == statusDate)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenAccountingWasNotReturnedFromAccountingRepository_AssertApplyLogicForPrincipalWasNotCalledOnAccountingHelper()
        {
            QueryHandler sut = CreateSut(false);

            await sut.QueryAsync(CreateQuery());

            _accountingHelperMock.Verify(m => m.ApplyLogicForPrincipal(It.IsAny<IAccounting>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenAccountingWasNotReturnedFromAccountingRepository_ReturnsNull()
        {
            QueryHandler sut = CreateSut(false);

            IAccounting result = await sut.QueryAsync(CreateQuery());

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenAccountingWasReturnedFromAccountingRepository_AssertApplyLogicForPrincipalWasCalledOnAccountingHelperWithAccounting()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            QueryHandler sut = CreateSut(accounting: accounting);

            await sut.QueryAsync(CreateQuery());

            _accountingHelperMock.Verify(m => m.ApplyLogicForPrincipal(It.Is<IAccounting>(value => value == accounting)), Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenAccountingWasReturnedFromAccountingRepository_ReturnsCalculatedAccounting()
        {
            IAccounting calculatedAccounting = _fixture.BuildAccountingMock().Object;
            IAccounting accounting = _fixture.BuildAccountingMock(calculatedAccounting: calculatedAccounting).Object;
            QueryHandler sut = CreateSut(accounting: accounting);

            IAccounting result = await sut.QueryAsync(CreateQuery());

            Assert.That(result, Is.EqualTo(calculatedAccounting));
        }

        private QueryHandler CreateSut(bool hasAccounting = true, IAccounting accounting = null)
        {
            _accountingRepositoryMock.Setup(m => m.GetAccountingAsync(It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(Task.Run(() => hasAccounting ? accounting ?? _fixture.BuildAccountingMock().Object : null));
            _accountingHelperMock.Setup(m => m.ApplyLogicForPrincipal(It.IsAny<IAccounting>()))
                .Returns(accounting ?? _fixture.BuildAccountingMock().Object);

            return new QueryHandler(_validatorMock.Object, _accountingRepositoryMock.Object, _accountingHelperMock.Object);
        }

        private IGetAccountingQuery CreateQuery(int? accountingNumber = null, DateTime? statusDate = null)
        {
            return CreateQueryMock(accountingNumber, statusDate).Object;
        }

        private Mock<IGetAccountingQuery> CreateQueryMock(int? accountingNumber = null, DateTime? statusDate = null)
        {
            Mock<IGetAccountingQuery> queryMock = new Mock<IGetAccountingQuery>();
            queryMock.Setup(m => m.AccountingNumber)
                .Returns(accountingNumber ?? _fixture.Create<int>());
            queryMock.Setup(m => m.StatusDate)
                .Returns(statusDate ?? _fixture.Create<DateTime>().Date);
            return queryMock;
        }
    }
}