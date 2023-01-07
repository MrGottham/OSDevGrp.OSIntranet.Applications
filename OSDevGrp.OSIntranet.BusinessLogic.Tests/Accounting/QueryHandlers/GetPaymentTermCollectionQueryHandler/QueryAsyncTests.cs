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
using QueryHandler = OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers.GetPaymentTermCollectionQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.QueryHandlers.GetPaymentTermCollectionQueryHandler
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
            _fixture.Customize<IPaymentTerm>(builder => builder.FromFactory(() => _fixture.BuildPaymentTermMock().Object));

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
        public async Task QueryAsync_WhenCalled_AssertGetPaymentTermsAsyncWasCalledOnAccountingRepository()
        {
            QueryHandler sut = CreateSut();

            await sut.QueryAsync(new EmptyQuery());

            _accountingRepositoryMock.Verify(m => m.GetPaymentTermsAsync(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenPaymentTermCollectionWasReturnedFromAccountingRepository_AssertIsAccountingAdministratorWasCalledOnClaimResolver()
        {
            QueryHandler sut = CreateSut();

            await sut.QueryAsync(new EmptyQuery());

            _claimResolverMock.Verify(m => m.IsAccountingAdministrator(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenIsAccountingAdministratorOnClaimResolverReturnsTrue_AssertApplyProtectionWasNotCalledOnAnyPaymentTermFromAccountingRepository()
        {
            Mock<IPaymentTerm>[] paymentTermMockCollection =
            {
                _fixture.BuildPaymentTermMock(),
                _fixture.BuildPaymentTermMock(),
                _fixture.BuildPaymentTermMock()
            };
            QueryHandler sut = CreateSut(paymentTermCollection: paymentTermMockCollection.Select(m => m.Object).ToArray(), isAccountingAdministrator: true);

            await sut.QueryAsync(new EmptyQuery());

            foreach (Mock<IPaymentTerm> paymentTermMock in paymentTermMockCollection)
            {
                paymentTermMock.Verify(m => m.ApplyProtection(), Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenIsAccountingAdministratorOnClaimResolverReturnsFalse_AssertApplyProtectionWasCalledOnEachPaymentTermFromAccountingRepository()
        {
            Mock<IPaymentTerm>[] paymentTermMockCollection =
            {
                _fixture.BuildPaymentTermMock(),
                _fixture.BuildPaymentTermMock(),
                _fixture.BuildPaymentTermMock()
            };
            QueryHandler sut = CreateSut(paymentTermCollection: paymentTermMockCollection.Select(m => m.Object).ToArray(), isAccountingAdministrator: false);

            await sut.QueryAsync(new EmptyQuery());

            foreach (Mock<IPaymentTerm> paymentTermMock in paymentTermMockCollection)
            {
                paymentTermMock.Verify(m => m.ApplyProtection(), Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoPaymentTermCollectionWasReturnedFromAccountingRepository_AssertIsAccountingAdministratorWasNotCalledOnClaimResolver()
        {
            QueryHandler sut = CreateSut(false);

            await sut.QueryAsync(new EmptyQuery());

            _claimResolverMock.Verify(m => m.IsAccountingAdministrator(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenPaymentTermCollectionWasReturnedFromAccountingRepository_ReturnsNotNull()
        {
            QueryHandler sut = CreateSut();

            IEnumerable<IPaymentTerm> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenPaymentTermCollectionWasReturnedFromAccountingRepository_ReturnsNonEmptyPaymentTermCollection()
        {
            QueryHandler sut = CreateSut();

            IEnumerable<IPaymentTerm> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenPaymentTermCollectionWasReturnedFromAccountingRepository_ReturnsPaymentTermCollectionFromAccountingRepository()
        {
            IEnumerable<IPaymentTerm> paymentTermCollection = _fixture.CreateMany<IPaymentTerm>(_random.Next(5, 10)).ToArray();
            QueryHandler sut = CreateSut(paymentTermCollection: paymentTermCollection);

            IEnumerable<IPaymentTerm> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.EqualTo(paymentTermCollection));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoPaymentTermCollectionWasReturnedFromAccountingRepository_ReturnsNotNull()
        {
            QueryHandler sut = CreateSut(false);

            IEnumerable<IPaymentTerm> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoPaymentTermCollectionWasReturnedFromAccountingRepository_ReturnsEmptyPaymentTermCollection()
        {
            QueryHandler sut = CreateSut(false);

            IEnumerable<IPaymentTerm> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Empty);
        }

        private QueryHandler CreateSut(bool hasPaymentTermCollection = true, IEnumerable<IPaymentTerm> paymentTermCollection = null, bool? isAccountingAdministrator = null)
        {
            _accountingRepositoryMock.Setup(m => m.GetPaymentTermsAsync())
                .Returns(Task.FromResult(hasPaymentTermCollection ? paymentTermCollection ?? _fixture.CreateMany<IPaymentTerm>(_random.Next(5, 10)).ToArray() : null));

            _claimResolverMock.Setup(m => m.IsAccountingAdministrator())
                .Returns(isAccountingAdministrator ?? _fixture.Create<bool>());

            return new QueryHandler(_accountingRepositoryMock.Object, _claimResolverMock.Object);
        }
    }
}