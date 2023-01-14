using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;
using QueryHandler = OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers.GetBudgetAccountQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.QueryHandlers.GetBudgetAccountQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<IClaimResolver> _claimResolverMock;
        private Mock<IAccountingRepository> _accountingRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _claimResolverMock = new Mock<IClaimResolver>();
            _accountingRepositoryMock = new Mock<IAccountingRepository>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void QueryAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            QueryHandler sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("query"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertAccountingNumberWasCalledOnGetBudgetAccountQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetBudgetAccountQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.AccountingNumber, Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertAccountNumberWasCalledOnGetBudgetAccountQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetBudgetAccountQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.AccountNumber, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertStatusDateWasCalledOnGetBudgetAccountQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetBudgetAccountQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.StatusDate, Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetBudgetAccountAsyncWasCalledOnAccountingRepository()
        {
            QueryHandler sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            string accountNumber = _fixture.Create<string>().ToUpper();
            DateTime statusDate = _fixture.Create<DateTime>().Date;
            IGetBudgetAccountQuery query = CreateQuery(accountingNumber, accountNumber, statusDate);
            await sut.QueryAsync(query);

            _accountingRepositoryMock.Verify(m => m.GetBudgetAccountAsync(
                    It.Is<int>(value => value == accountingNumber),
                    It.Is<string>(value => string.CompareOrdinal(value, accountNumber) == 0),
                    It.Is<DateTime>(value => value == statusDate)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoBudgetAccountWasReturnedFromAccountingRepository_ReturnsNull()
        {
            QueryHandler sut = CreateSut(false);

            IGetBudgetAccountQuery query = CreateQuery();
            IBudgetAccount result = await sut.QueryAsync(query);

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenBudgetAccountWasReturnedFromAccountingRepository_ReturnsCalculatedBudgetAccount()
        {
            IBudgetAccount calculatedBudgetAccount = _fixture.BuildBudgetAccountMock().Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(calculatedBudgetAccount: calculatedBudgetAccount).Object;
            QueryHandler sut = CreateSut(budgetAccount: budgetAccount);

            IGetBudgetAccountQuery query = CreateQuery();
            IBudgetAccount result = await sut.QueryAsync(query);

            Assert.That(result, Is.EqualTo(calculatedBudgetAccount));
        }

        private QueryHandler CreateSut(bool hasBudgetAccount = true, IBudgetAccount budgetAccount = null)
        {
            _claimResolverMock.Setup(m => m.CanModifyAccounting(It.IsAny<int>()))
                .Returns(_fixture.Create<bool>());

            _accountingRepositoryMock.Setup(m => m.GetBudgetAccountAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(hasBudgetAccount? budgetAccount ?? _fixture.BuildBudgetAccountMock().Object : null));

            return new QueryHandler(_validatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object);
        }

        private IGetBudgetAccountQuery CreateQuery(int? accountingNumber = null, string accountNumber = null, DateTime? statusDate = null)
        {
            return CreateQueryMock(accountingNumber, accountNumber, statusDate).Object;
        }

        private Mock<IGetBudgetAccountQuery> CreateQueryMock(int? accountingNumber = null, string accountNumber = null, DateTime? statusDate = null)
        {
            Mock<IGetBudgetAccountQuery> queryMock = new Mock<IGetBudgetAccountQuery>();
            queryMock.Setup(m => m.AccountingNumber)
                .Returns(accountingNumber ?? _fixture.Create<int>());
            queryMock.Setup(m => m.AccountNumber)
                .Returns(accountNumber ?? _fixture.Create<string>().ToUpper());
            queryMock.Setup(m => m.StatusDate)
                .Returns(statusDate ?? _fixture.Create<DateTime>().Date);
            return queryMock;
        }
    }
}