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
using QueryHandler=OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers.AccountGroupCollectionQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.QueryHandlers.AccountGroupCollectionQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests
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
            _fixture.Register<IAccountGroup>(() => new Mock<IAccountGroup>().Object);

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
        public async Task QueryAsync_WhenCalled_AssertGetAccountGroupsAsyncWasCalledOnAccountingRepository()
        {
            QueryHandler sut = CreateSut();

            await sut.QueryAsync(new EmptyQuery());

            _accountingRepositoryMock.Verify(m => m.GetAccountGroupsAsync(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnAccountGroupCollectionFromAccountingRepository()
        {
            IEnumerable<IAccountGroup> accountGroupMockCollection = _fixture.CreateMany<IAccountGroup>(_random.Next(5, 10)).ToList();
            QueryHandler sut = CreateSut(accountGroupMockCollection);

            IEnumerable<IAccountGroup> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.EqualTo(accountGroupMockCollection));
        }

        private QueryHandler CreateSut(IEnumerable<IAccountGroup> accountGroupCollection = null)
        {
             _accountingRepositoryMock.Setup(m => m.GetAccountGroupsAsync())
                .Returns(Task.Run(() => accountGroupCollection ?? _fixture.CreateMany<IAccountGroup>(_random.Next(5, 10)).ToList()));

           return new QueryHandler(_accountingRepositoryMock.Object);
        }
    }
}