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
using QueryHandler = OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers.GetAccountGroupQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.QueryHandlers.GetAccountGroupQueryHandler
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
            _fixture.Customize<IAccountGroup>(builder => builder.FromFactory(() => _fixture.BuildAccountGroupMock().Object));
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
        public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnGetAccountGroupQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetAccountGroupQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.Validate(
                    It.Is<IValidator>(value => value == _validatorMock.Object),
                    It.Is<IAccountingRepository>(value => value == _accountingRepositoryMock.Object)), 
                Times.Once);
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
            await sut.QueryAsync(CreateQuery(number));

            _accountingRepositoryMock.Verify(m => m.GetAccountGroupAsync(It.Is<int>(value => value == number)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenAccountGroupWasReturnedFromAccountingRepository_AssertIsAccountingAdministratorWasCalledOnClaimResolver()
        {
            QueryHandler sut = CreateSut();

            await sut.QueryAsync(CreateQuery());

            _claimResolverMock.Verify(m => m.IsAccountingAdministrator(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenIsAccountingAdministratorOnClaimResolverReturnsTrue_AssertApplyProtectionWasNotCalledOnAccountGroupFromAccountingRepository()
        {
            Mock<IAccountGroup> accountGroupMock = _fixture.BuildAccountGroupMock();
            QueryHandler sut = CreateSut(accountGroup: accountGroupMock.Object, isAccountingAdministrator: true);

            await sut.QueryAsync(CreateQuery());

            accountGroupMock.Verify(m => m.ApplyProtection(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenIsAccountingAdministratorOnClaimResolverReturnsFalse_AssertApplyProtectionWasCalledOnAccountGroupFromAccountingRepository()
        {
            Mock<IAccountGroup> accountGroupMock = _fixture.BuildAccountGroupMock();
            QueryHandler sut = CreateSut(accountGroup: accountGroupMock.Object, isAccountingAdministrator: false);

            await sut.QueryAsync(CreateQuery());

            accountGroupMock.Verify(m => m.ApplyProtection(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoAccountGroupWasReturnedFromAccountingRepository_AssertIsAccountingAdministratorWasNotCalledOnClaimResolver()
        {
            QueryHandler sut = CreateSut(false);

            await sut.QueryAsync(CreateQuery());

            _claimResolverMock.Verify(m => m.IsAccountingAdministrator(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenAccountGroupWasReturnedFromAccountingRepository_ReturnsNotNull()
        {
            IAccountGroup accountGroup = _fixture.Create<IAccountGroup>();
            QueryHandler sut = CreateSut(accountGroup: accountGroup);

            IAccountGroup result = await sut.QueryAsync(CreateQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenAccountGroupWasReturnedFromAccountingRepository_ReturnsAccountGroupFromAccountingRepository()
        {
            IAccountGroup accountGroup = _fixture.Create<IAccountGroup>();
            QueryHandler sut = CreateSut(accountGroup: accountGroup);

            IAccountGroup result = await sut.QueryAsync(CreateQuery());

            Assert.That(result, Is.EqualTo(accountGroup));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenAccountGroupWasReturnedFromAccountingRepository_ReturnsNull()
        {
            QueryHandler sut = CreateSut(false);

            IAccountGroup result = await sut.QueryAsync(CreateQuery());

            Assert.That(result, Is.Null);
        }

        private QueryHandler CreateSut(bool hasAccountGroup = true, IAccountGroup accountGroup = null, bool? isAccountingAdministrator = null)
        {
            _accountingRepositoryMock.Setup(m => m.GetAccountGroupAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(hasAccountGroup ? accountGroup ?? _fixture.Create<IAccountGroup>() : null));

            _claimResolverMock.Setup(m => m.IsAccountingAdministrator())
                .Returns(isAccountingAdministrator ?? _fixture.Create<bool>());

           return new QueryHandler(_validatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object);
        }

        private IGetAccountGroupQuery CreateQuery(int? number = null)
        {
            return CreateQueryMock(number).Object;
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