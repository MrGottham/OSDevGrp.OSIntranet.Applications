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
using QueryHandler = OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers.GetAccountGroupCollectionQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.QueryHandlers.GetAccountGroupCollectionQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests : BusinessLogicTestBase
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
            _fixture.Customize<IAccountGroup>(builder => builder.FromFactory(() => _fixture.BuildAccountGroupMock().Object));

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
        public async Task QueryAsync_WhenCalled_AssertGetAccountGroupsAsyncWasCalledOnAccountingRepository()
        {
            QueryHandler sut = CreateSut();

            await sut.QueryAsync(new EmptyQuery());

            _accountingRepositoryMock.Verify(m => m.GetAccountGroupsAsync(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenAccountGroupCollectionWasReturnedFromAccountingRepository_AssertIsAccountingAdministratorWasCalledOnClaimResolver()
        {
            QueryHandler sut = CreateSut();

            await sut.QueryAsync(new EmptyQuery());

            _claimResolverMock.Verify(m => m.IsAccountingAdministrator(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenIsAccountingAdministratorOnClaimResolverReturnsTrue_AssertApplyProtectionWasNotCalledOnAnyAccountGroupFromAccountingRepository()
        {
            Mock<IAccountGroup>[] accountGroupMockCollection =
            {
                _fixture.BuildAccountGroupMock(),
                _fixture.BuildAccountGroupMock(),
                _fixture.BuildAccountGroupMock()
            };
            QueryHandler sut = CreateSut(accountGroupCollection: accountGroupMockCollection.Select(m => m.Object).ToArray(), isAccountingAdministrator: true);

            await sut.QueryAsync(new EmptyQuery());

            foreach (Mock<IAccountGroup> accountGroupMock in accountGroupMockCollection)
            {
                accountGroupMock.Verify(m => m.ApplyProtection(), Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenIsAccountingAdministratorOnClaimResolverReturnsFalse_AssertApplyProtectionWasCalledOnEachAccountGroupFromAccountingRepository()
        {
            Mock<IAccountGroup>[] accountGroupMockCollection =
            {
                _fixture.BuildAccountGroupMock(),
                _fixture.BuildAccountGroupMock(),
                _fixture.BuildAccountGroupMock()
            };
            QueryHandler sut = CreateSut(accountGroupCollection: accountGroupMockCollection.Select(m => m.Object).ToArray(), isAccountingAdministrator: false);

            await sut.QueryAsync(new EmptyQuery());

            foreach (Mock<IAccountGroup> accountGroupMock in accountGroupMockCollection)
            {
                accountGroupMock.Verify(m => m.ApplyProtection(), Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoAccountGroupCollectionWasReturnedFromAccountingRepository_AssertIsAccountingAdministratorWasNotCalledOnClaimResolver()
        {
            QueryHandler sut = CreateSut(false);

            await sut.QueryAsync(new EmptyQuery());

            _claimResolverMock.Verify(m => m.IsAccountingAdministrator(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenAccountGroupCollectionWasReturnedFromAccountingRepository_ReturnsNotNull()
        {
            QueryHandler sut = CreateSut();

            IEnumerable<IAccountGroup> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenAccountGroupCollectionWasReturnedFromAccountingRepository_ReturnsNonEmptyAccountGroupCollection()
        {
            QueryHandler sut = CreateSut();

            IEnumerable<IAccountGroup> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenAccountGroupCollectionWasReturnedFromAccountingRepository_ReturnsAccountGroupCollectionFromAccountingRepository()
        {
            IEnumerable<IAccountGroup> accountGroupCollection = _fixture.CreateMany<IAccountGroup>(_random.Next(5, 10)).ToArray();
            QueryHandler sut = CreateSut(accountGroupCollection: accountGroupCollection);

            IEnumerable<IAccountGroup> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.EqualTo(accountGroupCollection));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoAccountGroupCollectionWasReturnedFromAccountingRepository_ReturnsNotNull()
        {
            QueryHandler sut = CreateSut(false);

            IEnumerable<IAccountGroup> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoAccountGroupCollectionWasReturnedFromAccountingRepository_ReturnsEmptyAccountGroupCollection()
        {
            QueryHandler sut = CreateSut(false);

            IEnumerable<IAccountGroup> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Empty);
        }

        private QueryHandler CreateSut(bool hasAccountGroupCollection = true, IEnumerable<IAccountGroup> accountGroupCollection = null, bool? isAccountingAdministrator = null)
        {
            _accountingRepositoryMock.Setup(m => m.GetAccountGroupsAsync())
                .Returns(Task.FromResult(hasAccountGroupCollection ? accountGroupCollection ?? _fixture.CreateMany<IAccountGroup>(_random.Next(5, 10)).ToArray() : null));

            _claimResolverMock.Setup(m => m.IsAccountingAdministrator())
                .Returns(isAccountingAdministrator ?? _fixture.Create<bool>());

           return new QueryHandler(_accountingRepositoryMock.Object, _claimResolverMock.Object);
        }
    }
}