using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using QueryHandler=OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers.GetBudgetAccountGroupQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.QueryHandlers.GetBudgetAccountGroupQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests : BusinessLogicTestBase
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<IAccountingRepository> _accountingRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _accountingRepositoryMock = new Mock<IAccountingRepository>();

            _fixture = new Fixture();
            _fixture.Customize<IBudgetAccountGroup>(builder => builder.FromFactory(() => _fixture.BuildBudgetAccountGroupMock().Object));
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
        public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnGetBudgetAccountGroupQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetBudgetAccountGroupQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.Validate(
                    It.Is<IValidator>(value => value == _validatorMock.Object),
                    It.Is<IAccountingRepository>(value => value == _accountingRepositoryMock.Object)), 
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertNumberWasCalledOnGetBudgetAccountGroupQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetBudgetAccountGroupQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.Number, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetBudgetAccountGroupAsyncWasCalledOnAccountingRepository()
        {
            QueryHandler sut = CreateSut();

            int number = _fixture.Create<int>();
            IGetBudgetAccountGroupQuery query = CreateQueryMock(number).Object;
            await sut.QueryAsync(query);

            _accountingRepositoryMock.Verify(m => m.GetBudgetAccountGroupAsync(It.Is<int>(value => value == number)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsBudgetAccountGroupFromAccountingRepository()
        {
            IBudgetAccountGroup budgetAccountGroup = _fixture.Create<IBudgetAccountGroup>();
            QueryHandler sut = CreateSut(budgetAccountGroup);

            IGetBudgetAccountGroupQuery query = CreateQueryMock().Object;
            IBudgetAccountGroup result = await sut.QueryAsync(query);

            Assert.That(result, Is.EqualTo(budgetAccountGroup));
        }

        private QueryHandler CreateSut(IBudgetAccountGroup budgetAccountGroup = null)
        {
            _accountingRepositoryMock.Setup(m => m.GetBudgetAccountGroupAsync(It.IsAny<int>()))
                .Returns(Task.Run(() => budgetAccountGroup ?? _fixture.Create<IBudgetAccountGroup>()));

           return new QueryHandler(_validatorMock.Object, _accountingRepositoryMock.Object);
        }
 
        private Mock<IGetBudgetAccountGroupQuery> CreateQueryMock(int? number = null)
        {
            Mock<IGetBudgetAccountGroupQuery> queryMock = new Mock<IGetBudgetAccountGroupQuery>();
            queryMock.Setup(m => m.Number)
                .Returns(number ?? _fixture.Create<int>());
            return queryMock;
        }
    }
}