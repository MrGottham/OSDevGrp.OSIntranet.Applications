using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using QueryHandler=OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers.GetAccountingCollectionQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.QueryHandlers.GetAccountingCollectionQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests : BusinessLogicTestBase
    {
        #region Private variables

        private Mock<IAccountingRepository> _accountingRepositoryMock;
        private Mock<IAccountingHelper> _accountingHelperMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _accountingRepositoryMock = new Mock<IAccountingRepository>();
            _accountingHelperMock = new Mock<IAccountingHelper>();

            _fixture = new Fixture();
            _fixture.Customize<ILetterHead>(builder => builder.FromFactory(() => _fixture.BuildLetterHeadMock().Object));
            _fixture.Customize<IAccounting>(builder => builder.FromFactory(() => _fixture.BuildAccountingMock().Object));

            _random = new Random(_fixture.Create<int>());
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
        public async Task QueryAsync_WhenCalled_AssertGetAccountingsAsyncWasCalledOnAccountingRepository()
        {
            QueryHandler sut = CreateSut();

            await sut.QueryAsync(new EmptyQuery());

            _accountingRepositoryMock.Verify(m => m.GetAccountingsAsync(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertApplyLogicForPrincipalWasCalledOnAccountingHelper()
        {
            IEnumerable<IAccounting> accountingMockCollection = _fixture.CreateMany<IAccounting>(_random.Next(5, 10)).ToList();
            QueryHandler sut = CreateSut(accountingMockCollection);

            await sut.QueryAsync(new EmptyQuery());

            _accountingHelperMock.Verify(m => m.ApplyLogicForPrincipal(It.Is<IEnumerable<IAccounting>>(value => value == accountingMockCollection)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnAccountingCollectionFromAccountingHelper()
        {
            IEnumerable<IAccounting> accountingMockCollection = _fixture.CreateMany<IAccounting>(_random.Next(5, 10)).ToList();
            QueryHandler sut = CreateSut(accountingMockCollection);

            IEnumerable<IAccounting> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.EqualTo(accountingMockCollection));
        }

        private QueryHandler CreateSut(IEnumerable<IAccounting> accountingCollection = null)
        {
            _accountingRepositoryMock.Setup(m => m.GetAccountingsAsync())
                .Returns(Task.Run(() => accountingCollection ?? _fixture.CreateMany<IAccounting>(_random.Next(5, 10)).ToList()));
            _accountingHelperMock.Setup(m => m.ApplyLogicForPrincipal(It.IsAny<IEnumerable<IAccounting>>()))
                .Returns(accountingCollection ?? _fixture.CreateMany<IAccounting>(_random.Next(5, 10)).ToList());

           return new QueryHandler(_accountingRepositoryMock.Object, _accountingHelperMock.Object);
        }
    }
}