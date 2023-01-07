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
using QueryHandler = OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers.GetPaymentTermQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.QueryHandlers.GetPaymentTermQueryHandler
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
        public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnGetPaymentTermQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetPaymentTermQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.Validate(
                    It.Is<IValidator>(value => value == _validatorMock.Object),
                    It.Is<IAccountingRepository>(value => value == _accountingRepositoryMock.Object)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertNumberWasCalledOnGetPaymentTermQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetPaymentTermQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.Number, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetPaymentTermAsyncWasCalledOnAccountingRepository()
        {
            QueryHandler sut = CreateSut();

            int number = _fixture.Create<int>();
            await sut.QueryAsync(CreateQuery(number));

            _accountingRepositoryMock.Verify(m => m.GetPaymentTermAsync(It.Is<int>(value => value == number)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenPaymentTermWasReturnedFromAccountingRepository_AssertIsAccountingAdministratorWasCalledOnClaimResolver()
        {
            QueryHandler sut = CreateSut();

            await sut.QueryAsync(CreateQuery());

            _claimResolverMock.Verify(m => m.IsAccountingAdministrator(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenIsAccountingAdministratorOnClaimResolverReturnsTrue_AssertApplyProtectionWasNotCalledOnPaymentTermFromAccountingRepository()
        {
            Mock<IPaymentTerm> paymentTermMock = _fixture.BuildPaymentTermMock();
            QueryHandler sut = CreateSut(paymentTerm: paymentTermMock.Object, isAccountingAdministrator: true);

            await sut.QueryAsync(CreateQuery());

            paymentTermMock.Verify(m => m.ApplyProtection(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenIsAccountingAdministratorOnClaimResolverReturnsFalse_AssertApplyProtectionWasCalledOnPaymentTermFromAccountingRepository()
        {
            Mock<IPaymentTerm> paymentTermMock = _fixture.BuildPaymentTermMock();
            QueryHandler sut = CreateSut(paymentTerm: paymentTermMock.Object, isAccountingAdministrator: false);

            await sut.QueryAsync(CreateQuery());

            paymentTermMock.Verify(m => m.ApplyProtection(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoPaymentTermWasReturnedFromAccountingRepository_AssertIsAccountingAdministratorWasNotCalledOnClaimResolver()
        {
            QueryHandler sut = CreateSut(false);

            await sut.QueryAsync(CreateQuery());

            _claimResolverMock.Verify(m => m.IsAccountingAdministrator(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenPaymentTermWasReturnedFromAccountingRepository_ReturnsNotNull()
        {
            QueryHandler sut = CreateSut();

            IPaymentTerm result = await sut.QueryAsync(CreateQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenPaymentTermWasReturnedFromAccountingRepository_ReturnsPaymentTermFromAccountingRepository()
        {
            IPaymentTerm paymentTerm = _fixture.BuildPaymentTermMock().Object;
            QueryHandler sut = CreateSut(paymentTerm: paymentTerm);

            IPaymentTerm result = await sut.QueryAsync(CreateQuery());

            Assert.That(result, Is.EqualTo(paymentTerm));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoPaymentTermWasReturnedFromAccountingRepository_ReturnsNull()
        {
            QueryHandler sut = CreateSut(false);

            IPaymentTerm result = await sut.QueryAsync(CreateQuery());

            Assert.That(result, Is.Null);
        }

        private QueryHandler CreateSut(bool hasPaymentTerm = true, IPaymentTerm paymentTerm = null, bool? isAccountingAdministrator = null)
        {
            _accountingRepositoryMock.Setup(m => m.GetPaymentTermAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(hasPaymentTerm ? paymentTerm ?? _fixture.BuildPaymentTermMock().Object : null));

            _claimResolverMock.Setup(m => m.IsAccountingAdministrator())
                .Returns(isAccountingAdministrator ?? _fixture.Create<bool>());

            return new QueryHandler(_validatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object);
        }

        private IGetPaymentTermQuery CreateQuery(int? number = null)
        {
            return CreateQueryMock(number).Object;
        }

        private Mock<IGetPaymentTermQuery> CreateQueryMock(int? number = null)
        {
            Mock<IGetPaymentTermQuery> queryMock = new Mock<IGetPaymentTermQuery>();
            queryMock.Setup(m => m.Number)
                .Returns(number ?? _fixture.Create<int>());
            return queryMock;
        }
    }
}