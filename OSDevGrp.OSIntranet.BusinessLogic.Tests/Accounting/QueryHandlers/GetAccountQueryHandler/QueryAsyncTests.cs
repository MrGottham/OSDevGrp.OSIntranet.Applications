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
using QueryHandler = OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers.GetAccountQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.QueryHandlers.GetAccountQueryHandler
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
        public async Task QueryAsync_WhenCalled_AssertAccountingNumberWasCalledOnGetAccountQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetAccountQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.AccountingNumber, Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertAccountNumberWasCalledOnGetAccountQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetAccountQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.AccountNumber, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertStatusDateWasCalledOnGetAccountQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetAccountQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.StatusDate, Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetAccountAsyncWasCalledOnAccountingRepository()
        {
            QueryHandler sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            string accountNumber = _fixture.Create<string>().ToUpper();
            DateTime statusDate = _fixture.Create<DateTime>().Date;
            IGetAccountQuery query = CreateQuery(accountingNumber, accountNumber, statusDate);
            await sut.QueryAsync(query);

            _accountingRepositoryMock.Verify(m => m.GetAccountAsync(
                    It.Is<int>(value => value == accountingNumber),
                    It.Is<string>(value => string.CompareOrdinal(value, accountNumber) == 0),
                    It.Is<DateTime>(value => value == statusDate)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoAccountWasReturnedFromAccountingRepository_ReturnsNull()
        {
            QueryHandler sut = CreateSut(false);

            IGetAccountQuery query = CreateQuery();
            IAccount result = await sut.QueryAsync(query);

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenAccountWasReturnedFromAccountingRepository_ReturnsCalculatedAccount()
        {
            IAccount calculatedAccount = _fixture.BuildAccountMock().Object;
            IAccount account = _fixture.BuildAccountMock(calculatedAccount: calculatedAccount).Object;
            QueryHandler sut = CreateSut(account: account);

            IGetAccountQuery query = CreateQuery();
            IAccount result = await sut.QueryAsync(query);

            Assert.That(result, Is.EqualTo(calculatedAccount));
        }

        private QueryHandler CreateSut(bool hasAccount = true, IAccount account = null)
        {
            _claimResolverMock.Setup(m => m.CanModifyAccounting(It.IsAny<int>()))
                .Returns(_fixture.Create<bool>());

            _accountingRepositoryMock.Setup(m => m.GetAccountAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(hasAccount ? account ?? _fixture.BuildAccountMock().Object : null));

            return new QueryHandler(_validatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object);
        }

        private IGetAccountQuery CreateQuery(int? accountingNumber = null, string accountNumber = null, DateTime? statusDate = null)
        {
            return CreateQueryMock(accountingNumber, accountNumber, statusDate).Object;
        }

        private Mock<IGetAccountQuery> CreateQueryMock(int? accountingNumber = null, string accountNumber = null, DateTime? statusDate = null)
        {
            Mock<IGetAccountQuery> queryMock = new Mock<IGetAccountQuery>();
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