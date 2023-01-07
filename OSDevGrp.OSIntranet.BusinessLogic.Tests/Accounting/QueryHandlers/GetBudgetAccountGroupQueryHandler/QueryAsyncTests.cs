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
using QueryHandler = OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers.GetBudgetAccountGroupQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.QueryHandlers.GetBudgetAccountGroupQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests : BusinessLogicTestBase
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
            _fixture.Customize<IBudgetAccountGroup>(builder => builder.FromFactory(() => _fixture.BuildBudgetAccountGroupMock().Object));
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
            await sut.QueryAsync(CreateQuery(number));

            _accountingRepositoryMock.Verify(m => m.GetBudgetAccountGroupAsync(It.Is<int>(value => value == number)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenBudgetAccountGroupWasReturnedFromAccountingRepository_AssertIsAccountingAdministratorWasCalledOnClaimResolver()
        {
            QueryHandler sut = CreateSut();

            await sut.QueryAsync(CreateQuery());

            _claimResolverMock.Verify(m => m.IsAccountingAdministrator(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenIsAccountingAdministratorOnClaimResolverReturnsTrue_AssertApplyProtectionWasNotCalledOnBudgetAccountGroupFromAccountingRepository()
        {
            Mock<IBudgetAccountGroup> budgetAccountGroupMock = _fixture.BuildBudgetAccountGroupMock();
            QueryHandler sut = CreateSut(budgetAccountGroup: budgetAccountGroupMock.Object, isAccountingAdministrator: true);

            await sut.QueryAsync(CreateQuery());

            budgetAccountGroupMock.Verify(m => m.ApplyProtection(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenIsAccountingAdministratorOnClaimResolverReturnsFalse_AssertApplyProtectionWasCalledOnBudgetAccountGroupFromAccountingRepository()
        {
            Mock<IBudgetAccountGroup> budgetAccountGroupMock = _fixture.BuildBudgetAccountGroupMock();
            QueryHandler sut = CreateSut(budgetAccountGroup: budgetAccountGroupMock.Object, isAccountingAdministrator: false);

            await sut.QueryAsync(CreateQuery());

            budgetAccountGroupMock.Verify(m => m.ApplyProtection(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoBudgetAccountGroupWasReturnedFromAccountingRepository_AssertIsAccountingAdministratorWasNotCalledOnClaimResolver()
        {
            QueryHandler sut = CreateSut(false);

            await sut.QueryAsync(CreateQuery());

            _claimResolverMock.Verify(m => m.IsAccountingAdministrator(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenBudgetAccountGroupWasReturnedFromAccountingRepository_ReturnsNotNull()
        {
            QueryHandler sut = CreateSut();

            IBudgetAccountGroup result = await sut.QueryAsync(CreateQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenBudgetAccountGroupWasReturnedFromAccountingRepository_ReturnsBudgetAccountGroupFromAccountingRepository()
        {
            IBudgetAccountGroup budgetAccountGroup = _fixture.Create<IBudgetAccountGroup>();
            QueryHandler sut = CreateSut(budgetAccountGroup: budgetAccountGroup);

            IBudgetAccountGroup result = await sut.QueryAsync(CreateQuery());

            Assert.That(result, Is.EqualTo(budgetAccountGroup));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoBudgetAccountGroupWasReturnedFromAccountingRepository_ReturnsNull()
        {
            QueryHandler sut = CreateSut(false);

            IBudgetAccountGroup result = await sut.QueryAsync(CreateQuery());

            Assert.That(result, Is.Null);
        }

        private QueryHandler CreateSut(bool hasBudgetAccountGroup = true, IBudgetAccountGroup budgetAccountGroup = null, bool? isAccountingAdministrator = null)
        {
            _accountingRepositoryMock.Setup(m => m.GetBudgetAccountGroupAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(hasBudgetAccountGroup ? budgetAccountGroup ?? _fixture.Create<IBudgetAccountGroup>() : null));

            _claimResolverMock.Setup(m => m.IsAccountingAdministrator())
                .Returns(isAccountingAdministrator ?? _fixture.Create<bool>());

           return new QueryHandler(_validatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object);
        }

        private IGetBudgetAccountGroupQuery CreateQuery(int? number = null)
        {
            return CreateQueryMock(number).Object;
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