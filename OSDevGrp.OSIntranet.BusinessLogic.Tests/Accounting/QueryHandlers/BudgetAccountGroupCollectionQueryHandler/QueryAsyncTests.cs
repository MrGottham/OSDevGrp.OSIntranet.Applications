using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using QueryHandler=OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers.BudgetAccountGroupCollectionQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.QueryHandlers.BudgetAccountGroupCollectionQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests : BusinessLogicTestBase
    {
        #region Private variables

        private Mock<IAccountingRepository> _accountingRepositoryMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _accountingRepositoryMock = new Mock<IAccountingRepository>();

            _fixture = new Fixture();
            _fixture.Register(() => new Mock<IBudgetAccountGroup>().Object);

            _random = new Random(_fixture.Create<int>());
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
        public async Task QueryAsync_WhenCalled_AssertGetBudgetAccountGroupsAsyncWasCalledOnAccountingRepository()
        {
            QueryHandler sut = CreateSut();

            await sut.QueryAsync(new EmptyQuery());

            _accountingRepositoryMock.Verify(m => m.GetBudgetAccountGroupsAsync(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnBudgetAccountGroupCollectionFromAccountingRepository()
        {
            IEnumerable<IBudgetAccountGroup> budgetAccountGroupMockCollection = _fixture.CreateMany<IBudgetAccountGroup>(_random.Next(5, 10)).ToList();
            QueryHandler sut = CreateSut(budgetAccountGroupMockCollection);

            IEnumerable<IBudgetAccountGroup> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.EqualTo(budgetAccountGroupMockCollection));
        }

        private QueryHandler CreateSut(IEnumerable<IBudgetAccountGroup> budgetAccountGroupCollection = null)
        {
            _accountingRepositoryMock.Setup(m => m.GetBudgetAccountGroupsAsync())
                .Returns(Task.Run(() => budgetAccountGroupCollection ?? _fixture.CreateMany<IBudgetAccountGroup>(_random.Next(5, 10)).ToList()));

            return new QueryHandler(_accountingRepositoryMock.Object);
        }
    }
}