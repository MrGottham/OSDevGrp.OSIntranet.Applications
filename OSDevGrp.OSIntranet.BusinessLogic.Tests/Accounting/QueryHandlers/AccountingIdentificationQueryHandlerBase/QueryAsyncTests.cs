﻿using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;
using QueryHandler = OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers.AccountingIdentificationQueryHandlerBase<OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries.IAccountingIdentificationQuery, OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.IAccounting>;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.QueryHandlers.AccountingIdentificationQueryHandlerBase
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
        public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnAccountingIdentificationQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IAccountingIdentificationQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.Validate(
                    It.Is<IValidator>(value => value == _validatorMock.Object),
                    It.Is<IClaimResolver>(value => value == _claimResolverMock.Object),
                    It.Is<IAccountingRepository>(value => value == _accountingRepositoryMock.Object)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetDataAsyncWasCalledWasCalledOnAccountingIdentificationQueryHandlerBase()
        {
            QueryHandler sut = CreateSut();

            await sut.QueryAsync(CreateQuery());

            Assert.That(((Sut) sut).GetDataAsyncWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetDataAsyncWasCalledWasCalledOnAccountingIdentificationQueryHandlerBaseWithQuery()
        {
            QueryHandler sut = CreateSut();

            IAccountingIdentificationQuery query = CreateQuery();
            await sut.QueryAsync(query);

            Assert.That(((Sut) sut).GetDataAsyncWasCalledWithQuery, Is.SameAs(query));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoDataWasReturned_AssertGetResultForNoDataAsyncWasCalledWasCalledOnAccountingIdentificationQueryHandlerBase()
        {
            QueryHandler sut = CreateSut(false);

            await sut.QueryAsync(CreateQuery());

            Assert.That(((Sut) sut).GetResultForNoDataAsyncWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoDataWasReturned_AssertGetResultForNoDataAsyncWasCalledWasCalledOnAccountingIdentificationQueryHandlerBaseWithQuery()
        {
            QueryHandler sut = CreateSut(false);

            IAccountingIdentificationQuery query = CreateQuery();
            await sut.QueryAsync(query);

            Assert.That(((Sut) sut).GetResultForNoDataAsyncWasCalledWithQuery, Is.SameAs(query));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoDataWasReturned_AssertStatusDateWasNotCalledOnQuery()
        {
            QueryHandler sut = CreateSut(false);

            Mock<IAccountingIdentificationQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.StatusDate, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoDataWasReturned_AssertAccountingNumberWasNotCalledOnAccountingIdentificationQuery()
        {
            QueryHandler sut = CreateSut(false);

            Mock<IAccountingIdentificationQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.AccountingNumber, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoDataWasReturned_AssertCanModifyAccountingWasNotCalledOnClaimResolver()
        {
            QueryHandler sut = CreateSut(false);

            await sut.QueryAsync(CreateQuery());

            _claimResolverMock.Verify(m => m.CanModifyAccounting(It.IsAny<int>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoDataWasReturned_ReturnsNull()
        {
            QueryHandler sut = CreateSut(false);

            IAccounting result = await sut.QueryAsync(CreateQuery());

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenDataWasReturned_AssertGetResultForNoDataAsyncWasNotCalledWasCalledOnAccountingIdentificationQueryHandlerBase()
        {
            QueryHandler sut = CreateSut();

            await sut.QueryAsync(CreateQuery());

            Assert.That(((Sut) sut).GetResultForNoDataAsyncWasCalled, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenDataWasReturned_AssertGetResultForNoDataAsyncWasNotCalledWasCalledOnAccountingIdentificationQueryHandlerBaseWithQuery()
        {
            QueryHandler sut = CreateSut();

            await sut.QueryAsync(CreateQuery());

            Assert.That(((Sut) sut).GetResultForNoDataAsyncWasCalledWithQuery, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenDataWasReturned_AssertStatusDateWasCalledOnQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IAccountingIdentificationQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.StatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenDataWasReturned_AssertCalculateAsyncWasCalledOnReturnedData()
        {
            Mock<IAccounting> accountingMock = _fixture.BuildAccountingMock();
            QueryHandler sut = CreateSut(accounting: accountingMock.Object);

            DateTime statusDate = _fixture.Create<DateTime>().Date;
            IAccountingIdentificationQuery query = CreateQuery(statusDate: statusDate);
            await sut.QueryAsync(query);

            accountingMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value ==  statusDate)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoCalculatedDataWasReturnedFromDataWasReturned_AssertAccountingNumberWasNotCalledOnAccountingIdentificationQuery()
        {
            IAccounting accounting = _fixture.BuildAccountingMock(hasCalculatedAccounting: false).Object;
            QueryHandler sut = CreateSut(accounting: accounting);

            Mock<IAccountingIdentificationQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.AccountingNumber, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoCalculatedDataWasReturnedFromDataWasReturned_AssertCanModifyAccountingWasNotCalledOnClaimResolver()
        {
            IAccounting accounting = _fixture.BuildAccountingMock(hasCalculatedAccounting: false).Object;
            QueryHandler sut = CreateSut(accounting: accounting);

            await sut.QueryAsync(CreateQuery());

            _claimResolverMock.Verify(m => m.CanModifyAccounting(It.IsAny<int>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoCalculatedDataWasReturnedFromDataWasReturned_ReturnsNull()
        {
            IAccounting accounting = _fixture.BuildAccountingMock(hasCalculatedAccounting: false).Object;
            QueryHandler sut = CreateSut(accounting: accounting);

            IAccounting result = await sut.QueryAsync(CreateQuery());

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalculatedDataWasReturnedFromDataWasReturned_AssertAccountingNumberWasCalledOnAccountingIdentificationQuery()
        {
            IAccounting accounting = _fixture.BuildAccountingMock(hasCalculatedAccounting: true).Object;
            QueryHandler sut = CreateSut(accounting: accounting);

            Mock<IAccountingIdentificationQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.AccountingNumber, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalculatedDataWasReturnedFromDataWasReturned_AssertCanModifyAccountingWasCalledOnClaimResolver()
        {
            IAccounting accounting = _fixture.BuildAccountingMock(hasCalculatedAccounting: true).Object;
            QueryHandler sut = CreateSut(accounting: accounting);

            int accountingNumber = _fixture.Create<int>();
            await sut.QueryAsync(CreateQuery(accountingNumber));

            _claimResolverMock.Verify(m => m.CanModifyAccounting(It.Is<int>(value => value == accountingNumber)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCanModifyAccountingReturnsFalse_AssertApplyProtectionWasCalledOnCalculatedDataFromReturnedData()
        {
            Mock<IAccounting> calculatedAccountingMock = _fixture.BuildAccountingMock();
            IAccounting accounting = _fixture.BuildAccountingMock(hasCalculatedAccounting: true, calculatedAccounting: calculatedAccountingMock.Object).Object;
            QueryHandler sut = CreateSut(accounting: accounting, canModifyAccounting: false);

            int accountingNumber = _fixture.Create<int>();
            await sut.QueryAsync(CreateQuery(accountingNumber));

            calculatedAccountingMock.Verify(m => m.ApplyProtection(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCanModifyAccountingReturnsTrue_AssertApplyProtectionWasNotCalledOnCalculatedDataFromReturnedData()
        {
            Mock<IAccounting> calculatedAccountingMock = _fixture.BuildAccountingMock();
            IAccounting accounting = _fixture.BuildAccountingMock(hasCalculatedAccounting: true, calculatedAccounting: calculatedAccountingMock.Object).Object;
            QueryHandler sut = CreateSut(accounting: accounting, canModifyAccounting: true);

            int accountingNumber = _fixture.Create<int>();
            await sut.QueryAsync(CreateQuery(accountingNumber));

            calculatedAccountingMock.Verify(m => m.ApplyProtection(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoCalculatedDataWasReturnedFromDataWasReturned_ReturnsNotNull()
        {
            IAccounting accounting = _fixture.BuildAccountingMock(hasCalculatedAccounting: true).Object;
            QueryHandler sut = CreateSut(accounting: accounting);

            IAccounting result = await sut.QueryAsync(CreateQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoCalculatedDataWasReturnedFromDataWasReturned_ReturnsCalculatedData()
        {
            IAccounting calculatedAccounting = _fixture.BuildAccountingMock().Object;
            IAccounting accounting = _fixture.BuildAccountingMock(hasCalculatedAccounting: true, calculatedAccounting: calculatedAccounting).Object;
            QueryHandler sut = CreateSut(accounting: accounting);

            IAccounting result = await sut.QueryAsync(CreateQuery());

            Assert.That(result, Is.EqualTo(calculatedAccounting));
        }

        private QueryHandler CreateSut(bool hasAccounting = true, IAccounting accounting = null, bool? canModifyAccounting = null)
        {
            _claimResolverMock.Setup(m => m.CanModifyAccounting(It.IsAny<int>()))
                .Returns(canModifyAccounting ?? _fixture.Create<bool>());

            return new Sut(_validatorMock.Object, _claimResolverMock.Object, _accountingRepositoryMock.Object, hasAccounting ? accounting ?? _fixture.BuildAccountingMock().Object : null);
        }

        private IAccountingIdentificationQuery CreateQuery(int? accountingNumber = null, DateTime? statusDate = null)
        {
            return CreateQueryMock(accountingNumber, statusDate).Object;
        }

        private Mock<IAccountingIdentificationQuery> CreateQueryMock(int? accountingNumber = null, DateTime? statusDate = null)
        {
            Mock<IAccountingIdentificationQuery> queryMock = new Mock<IAccountingIdentificationQuery>();
            queryMock.Setup(m => m.AccountingNumber)
                .Returns(accountingNumber ?? _fixture.Create<int>());
            queryMock.Setup(m => m.StatusDate)
                .Returns(statusDate ?? _fixture.Create<DateTime>().Date);
            return queryMock;
        }

        private class Sut : QueryHandler
        {
            #region Private variables

            private readonly IAccounting _accounting;

            #endregion

            #region Constructor

            public Sut(IValidator validator, IClaimResolver claimResolver, IAccountingRepository accountingRepository, IAccounting accounting) 
                : base(validator, claimResolver, accountingRepository)
            {
                _accounting = accounting;
            }

            #endregion

            #region Properties

            public bool GetDataAsyncWasCalled { get; private set; }

            public IAccountingIdentificationQuery GetDataAsyncWasCalledWithQuery { get; private set; }

            public bool GetResultForNoDataAsyncWasCalled { get; private set; }

            public IAccountingIdentificationQuery GetResultForNoDataAsyncWasCalledWithQuery { get; private set; }

            #endregion

            #region Methods

            protected override Task<IAccounting> GetDataAsync(IAccountingIdentificationQuery query)
            {
                NullGuard.NotNull(query, nameof(query));

                GetDataAsyncWasCalled = true;
                GetDataAsyncWasCalledWithQuery = query;

                return Task.FromResult(_accounting);
            }

            protected override Task<IAccounting> GetResultForNoDataAsync(IAccountingIdentificationQuery query)
            {
                NullGuard.NotNull(query, nameof(query));

                GetResultForNoDataAsyncWasCalled = true;
                GetResultForNoDataAsyncWasCalledWithQuery = query;

                return base.GetResultForNoDataAsync(query);
            }

            #endregion
        }
    }
}