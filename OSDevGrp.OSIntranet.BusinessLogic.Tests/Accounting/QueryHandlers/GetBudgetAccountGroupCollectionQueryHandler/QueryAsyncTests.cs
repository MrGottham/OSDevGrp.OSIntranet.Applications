using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QueryHandler = OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers.GetBudgetAccountGroupCollectionQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.QueryHandlers.GetBudgetAccountGroupCollectionQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests
    {
        #region Private variables

        private Mock<IAccountingRepository> _accountingRepositoryMock;
        private Mock<IClaimResolver> _claimResolverMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _accountingRepositoryMock = new Mock<IAccountingRepository>();
            _claimResolverMock = new Mock<IClaimResolver>();

            _fixture = new Fixture();
            _fixture.Customize<IBudgetAccountGroup>(builder => builder.FromFactory(() => _fixture.BuildBudgetAccountGroupMock().Object));

            _random = new Random(_fixture.Create<int>());
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
        public async Task QueryAsync_WhenCalled_AssertGetBudgetAccountGroupsAsyncWasCalledOnAccountingRepository()
        {
            QueryHandler sut = CreateSut();

            await sut.QueryAsync(new EmptyQuery());

            _accountingRepositoryMock.Verify(m => m.GetBudgetAccountGroupsAsync(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenBudgetAccountGroupCollectionWasReturnedFromAccountingRepository_AssertIsAccountingAdministratorWasCalledOnClaimResolver()
        {
            QueryHandler sut = CreateSut();

            await sut.QueryAsync(new EmptyQuery());

            _claimResolverMock.Verify(m => m.IsAccountingAdministrator(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenIsAccountingAdministratorOnClaimResolverReturnsTrue_AssertApplyProtectionWasNotCalledOnAnyBudgetAccountGroupFromAccountingRepository()
        {
            Mock<IBudgetAccountGroup>[] budgetAccountGroupMockCollection =
            {
                _fixture.BuildBudgetAccountGroupMock(),
                _fixture.BuildBudgetAccountGroupMock(),
                _fixture.BuildBudgetAccountGroupMock()
            };
            QueryHandler sut = CreateSut(budgetAccountGroupCollection: budgetAccountGroupMockCollection.Select(m => m.Object).ToArray(), isAccountingAdministrator: true);

            await sut.QueryAsync(new EmptyQuery());

            foreach (Mock<IBudgetAccountGroup> budgetAccountGroupMock in budgetAccountGroupMockCollection)
            {
                budgetAccountGroupMock.Verify(m => m.ApplyProtection(), Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenIsAccountingAdministratorOnClaimResolverReturnsFalse_AssertApplyProtectionWasCalledOnEachBudgetAccountGroupFromAccountingRepository()
        {
            Mock<IBudgetAccountGroup>[] budgetAccountGroupMockCollection =
            {
                _fixture.BuildBudgetAccountGroupMock(),
                _fixture.BuildBudgetAccountGroupMock(),
                _fixture.BuildBudgetAccountGroupMock()
            };
            QueryHandler sut = CreateSut(budgetAccountGroupCollection: budgetAccountGroupMockCollection.Select(m => m.Object).ToArray(), isAccountingAdministrator: false);

            await sut.QueryAsync(new EmptyQuery());

            foreach (Mock<IBudgetAccountGroup> budgetAccountGroupMock in budgetAccountGroupMockCollection)
            {
                budgetAccountGroupMock.Verify(m => m.ApplyProtection(), Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoBudgetAccountGroupCollectionWasReturnedFromAccountingRepository_AssertIsAccountingAdministratorWasNotCalledOnClaimResolver()
        {
            QueryHandler sut = CreateSut(false);

            await sut.QueryAsync(new EmptyQuery());

            _claimResolverMock.Verify(m => m.IsAccountingAdministrator(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenBudgetAccountGroupCollectionWasReturnedFromAccountingRepository_ReturnNotNull()
        {
            QueryHandler sut = CreateSut();

            IEnumerable<IBudgetAccountGroup> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenBudgetAccountGroupCollectionWasReturnedFromAccountingRepository_ReturnNonEmptyBudgetAccountGroupCollection()
        {
            QueryHandler sut = CreateSut();

            IEnumerable<IBudgetAccountGroup> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenBudgetAccountGroupCollectionWasReturnedFromAccountingRepository_ReturnBudgetAccountGroupCollectionFromAccountingRepository()
        {
            IEnumerable<IBudgetAccountGroup> budgetAccountGroupCollection = _fixture.CreateMany<IBudgetAccountGroup>(_random.Next(5, 10)).ToArray();
            QueryHandler sut = CreateSut(budgetAccountGroupCollection: budgetAccountGroupCollection);

            IEnumerable<IBudgetAccountGroup> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.EqualTo(budgetAccountGroupCollection));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoBudgetAccountGroupCollectionWasReturnedFromAccountingRepository_ReturnNotNull()
        {
            QueryHandler sut = CreateSut(false);

            IEnumerable<IBudgetAccountGroup> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoBudgetAccountGroupCollectionWasReturnedFromAccountingRepository_ReturnEmptyBudgetAccountGroupCollection()
        {
            QueryHandler sut = CreateSut(false);

            IEnumerable<IBudgetAccountGroup> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Empty);
        }

        private QueryHandler CreateSut(bool hasBudgetAccountGroupCollection = true, IEnumerable<IBudgetAccountGroup> budgetAccountGroupCollection = null, bool? isAccountingAdministrator = null)
        {
            _accountingRepositoryMock.Setup(m => m.GetBudgetAccountGroupsAsync())
                .Returns(Task.FromResult(hasBudgetAccountGroupCollection ? budgetAccountGroupCollection ?? _fixture.CreateMany<IBudgetAccountGroup>(_random.Next(5, 10)).ToArray() : null));

            _claimResolverMock.Setup(m => m.IsAccountingAdministrator())
                .Returns(isAccountingAdministrator ?? _fixture.Create<bool>());

            return new QueryHandler(_accountingRepositoryMock.Object, _claimResolverMock.Object);
        }
    }
}