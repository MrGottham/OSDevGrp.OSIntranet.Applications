using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using QueryHandler=OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers.GetAccountGroupQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.QueryHandlers.GetAccountGroupQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests : BusinessLogicTestBase
    {
        #region Private variables

        private Mock<IAccountingRepository> _accountingRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _accountingRepositoryMock = new Mock<IAccountingRepository>();

            _fixture = new Fixture();
            _fixture.Customize<IAccountGroup>(builder => builder.FromFactory(() => _fixture.BuildAccountGroupMock().Object));
        }

        [Test]
        [Category("UnitTest")]
        public void QueryAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            QueryHandler sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.QueryAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("query"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertNumberWasCalledOnGetAccountGroupQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetAccountGroupQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.Number, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetAccountGroupAsyncWasCalledOnAccountingRepository()
        {
            QueryHandler sut = CreateSut();

            int number = _fixture.Create<int>();
            IGetAccountGroupQuery query = CreateQueryMock(number).Object;
            await sut.QueryAsync(query);

            _accountingRepositoryMock.Verify(m => m.GetAccountGroupAsync(It.Is<int>(value => value == number)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsAccountGroupFromAccountingRepository()
        {
            IAccountGroup accountGroup = _fixture.Create<IAccountGroup>();
            QueryHandler sut = CreateSut(accountGroup);

            IGetAccountGroupQuery query = CreateQueryMock().Object;
            IAccountGroup result = await sut.QueryAsync(query);

            Assert.That(result, Is.EqualTo(accountGroup));
        }

        private QueryHandler CreateSut(IAccountGroup accountGroup = null)
        {
             _accountingRepositoryMock.Setup(m => m.GetAccountGroupAsync(It.IsAny<int>()))
                .Returns(Task.Run(() => accountGroup ?? _fixture.Create<IAccountGroup>()));

           return new QueryHandler(_accountingRepositoryMock.Object);
        }

        private Mock<IGetAccountGroupQuery> CreateQueryMock(int? number = null)
        {
            Mock<IGetAccountGroupQuery> queryMock = new Mock<IGetAccountGroupQuery>();
            queryMock.Setup(m => m.Number)
                .Returns(number ?? _fixture.Create<int>());
            return queryMock;
        }
    }
}