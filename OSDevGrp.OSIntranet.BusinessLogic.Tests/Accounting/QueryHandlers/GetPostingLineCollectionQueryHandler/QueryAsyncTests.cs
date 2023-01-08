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
using QueryHandler = OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers.GetPostingLineCollectionQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.QueryHandlers.GetPostingLineCollectionQueryHandler
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
        public async Task QueryAsync_WhenCalled_AssertAccountingNumberWasCalledOnGetPostingLineCollectionQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetPostingLineCollectionQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.AccountingNumber, Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertStatusDateWasCalledOnGetPostingLineCollectionQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetPostingLineCollectionQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.StatusDate, Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertNumberOfPostingLinesWasCalledOnGetPostingLineCollectionQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetPostingLineCollectionQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.NumberOfPostingLines, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetPostingLinesAsyncWasCalledOnAccountingRepository()
        {
            QueryHandler sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            DateTime statusDate = _fixture.Create<DateTime>().Date;
            int numberOfPostingLines = _fixture.Create<int>();
            IGetPostingLineCollectionQuery query = CreateQuery(accountingNumber, statusDate, numberOfPostingLines);
            await sut.QueryAsync(query);

            _accountingRepositoryMock.Verify(m => m.GetPostingLinesAsync(
                    It.Is<int>(value => value == accountingNumber),
                    It.Is<DateTime>(value => value == statusDate),
                    It.Is<int>(value => value == numberOfPostingLines)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoPostingLineCollectionWasReturnedFromAccountingRepository_ReturnsNotNull()
        {
            QueryHandler sut = CreateSut(false);

            IGetPostingLineCollectionQuery query = CreateQuery();
            IPostingLineCollection result = await sut.QueryAsync(query);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoPostingLineCollectionWasReturnedFromAccountingRepository_ReturnsEmptyPostingLineCollection()
        {
            QueryHandler sut = CreateSut(false);

            IGetPostingLineCollectionQuery query = CreateQuery();
            IPostingLineCollection result = await sut.QueryAsync(query);

            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoPostingLineCollectionWasReturnedFromAccountingRepository_ReturnsWhereStatusDateIsEqualToStatusDateFromGetPostingLineCollectionQuery()
        {
            QueryHandler sut = CreateSut(false);

            DateTime statusDate = _fixture.Create<DateTime>().Date;
            IGetPostingLineCollectionQuery query = CreateQuery(statusDate: statusDate);
            IPostingLineCollection result = await sut.QueryAsync(query);

            Assert.That(result.StatusDate, Is.EqualTo(statusDate));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenPostingLineCollectionWasReturnedFromAccountingRepository_ReturnsCalculatedPostingLineCollection()
        {
            IPostingLineCollection calculatedPostingLineCollection = _fixture.BuildPostingLineCollectionMock().Object;
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(calculatedPostingLineCollection: calculatedPostingLineCollection).Object;
            QueryHandler sut = CreateSut(postingLineCollection: postingLineCollection);

            IGetPostingLineCollectionQuery query = CreateQuery();
            IPostingLineCollection result = await sut.QueryAsync(query);

            Assert.That(result, Is.EqualTo(calculatedPostingLineCollection));
        }

        private QueryHandler CreateSut(bool hasPostingLineCollection = true, IPostingLineCollection postingLineCollection = null)
        {
            _claimResolverMock.Setup(m => m.CanModifyAccounting(It.IsAny<int>()))
                .Returns(_fixture.Create<bool>());

            _accountingRepositoryMock.Setup(m => m.GetPostingLinesAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(Task.FromResult(hasPostingLineCollection ? postingLineCollection ?? _fixture.BuildPostingLineCollectionMock().Object : null));

            return new QueryHandler(_validatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object);
        }

        private IGetPostingLineCollectionQuery CreateQuery(int? accountingNumber = null, DateTime? statusDate = null, int? numberOfPostingLines = null)
        {
            return CreateQueryMock(accountingNumber, statusDate, numberOfPostingLines).Object;
        }

        private Mock<IGetPostingLineCollectionQuery> CreateQueryMock(int? accountingNumber = null, DateTime? statusDate = null, int? numberOfPostingLines = null)
        {
            Mock<IGetPostingLineCollectionQuery> queryMock = new Mock<IGetPostingLineCollectionQuery>();
            queryMock.Setup(m => m.AccountingNumber)
                .Returns(accountingNumber ?? _fixture.Create<int>());
            queryMock.Setup(m => m.StatusDate)
                .Returns(statusDate ?? _fixture.Create<DateTime>().Date);
            queryMock.Setup(m => m.NumberOfPostingLines)
                .Returns(numberOfPostingLines ?? _fixture.Create<int>());
            return queryMock;
        }
    }
}