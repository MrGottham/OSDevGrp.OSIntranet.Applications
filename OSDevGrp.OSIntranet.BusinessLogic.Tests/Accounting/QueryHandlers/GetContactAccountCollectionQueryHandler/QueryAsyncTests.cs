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
using System.Linq;
using System.Threading.Tasks;
using QueryHandler = OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers.GetContactAccountCollectionQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.QueryHandlers.GetContactAccountCollectionQueryHandler
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
        public async Task QueryAsync_WhenCalled_AssertAccountingNumberWasCalledOnGetContactAccountCollectionQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetContactAccountCollectionQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.AccountingNumber, Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertStatusDateWasCalledOnGetContactAccountCollectionQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetContactAccountCollectionQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.StatusDate, Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetContactAccountsAsyncWasCalledOnAccountingRepository()
        {
            QueryHandler sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            DateTime statusDate = _fixture.Create<DateTime>().Date;
            IGetContactAccountCollectionQuery query = CreateQuery(accountingNumber, statusDate);
            await sut.QueryAsync(query);

            _accountingRepositoryMock.Verify(m => m.GetContactAccountsAsync(
                    It.Is<int>(value => value == accountingNumber),
                    It.Is<DateTime>(value => value == statusDate)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoContactAccountCollectionWasReturnedFromAccountingRepository_ReturnsNotNull()
        {
            QueryHandler sut = CreateSut(false);

            IGetContactAccountCollectionQuery query = CreateQuery();
            IContactAccountCollection result = await sut.QueryAsync(query);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoContactAccountCollectionWasReturnedFromAccountingRepository_ReturnsEmptyContactAccountCollection()
        {
            QueryHandler sut = CreateSut(false);

            IGetContactAccountCollectionQuery query = CreateQuery();
            IContactAccountCollection result = await sut.QueryAsync(query);

            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoContactAccountCollectionWasReturnedFromAccountingRepository_ReturnsContactAccountCollectionWhereStatusDateIsEqualToStatusDateFromGetContactAccountCollectionQuery()
        {
            QueryHandler sut = CreateSut(false);

            DateTime statusDate = _fixture.Create<DateTime>().Date;
            IGetContactAccountCollectionQuery query = CreateQuery(statusDate: statusDate);
            IContactAccountCollection result = await sut.QueryAsync(query);

            Assert.That(result.StatusDate, Is.EqualTo(statusDate));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenContactAccountCollectionWasReturnedFromAccountingRepository_ReturnsCalculatedContactAccountCollection()
        {
            IContactAccountCollection calculatedContactAccountCollection = _fixture.BuildContactAccountCollectionMock().Object;
            IContactAccountCollection contactAccountCollection = _fixture.BuildContactAccountCollectionMock(calculatedContactAccountCollection: calculatedContactAccountCollection).Object;
            QueryHandler sut = CreateSut(contactAccountCollection: contactAccountCollection);

            IGetContactAccountCollectionQuery query = CreateQuery();
            IContactAccountCollection result = await sut.QueryAsync(query);

            Assert.That(result, Is.EqualTo(calculatedContactAccountCollection));
        }

        private QueryHandler CreateSut(bool hasContactAccountCollection = true, IContactAccountCollection contactAccountCollection = null)
        {
            _claimResolverMock.Setup(m => m.CanModifyAccounting(It.IsAny<int>()))
                .Returns(_fixture.Create<bool>());

            _accountingRepositoryMock.Setup(m => m.GetContactAccountsAsync(It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(hasContactAccountCollection ? contactAccountCollection ?? _fixture.BuildContactAccountCollectionMock().Object : null));

            return new QueryHandler(_validatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object);
        }

        private IGetContactAccountCollectionQuery CreateQuery(int? accountingNumber = null, DateTime? statusDate = null)
        {
            return CreateQueryMock(accountingNumber, statusDate).Object;
        }

        private Mock<IGetContactAccountCollectionQuery> CreateQueryMock(int? accountingNumber = null, DateTime? statusDate = null)
        {
            Mock<IGetContactAccountCollectionQuery> queryMock = new Mock<IGetContactAccountCollectionQuery>();
            queryMock.Setup(m => m.AccountingNumber)
                .Returns(accountingNumber ?? _fixture.Create<int>());
            queryMock.Setup(m => m.StatusDate)
                .Returns(statusDate ?? _fixture.Create<DateTime>().Date);
            return queryMock;
        }
    }
}