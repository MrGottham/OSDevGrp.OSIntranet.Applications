﻿using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using QueryHandler=OSDevGrp.OSIntranet.BusinessLogic.Accounting.QueryHandlers.GetCreditorAccountCollectionQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.QueryHandlers.GetCreditorAccountCollectionQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests
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
        public async Task QueryAsync_WhenCalled_AssertAccountingNumberWasCalledOnGetCreditorAccountCollectionQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetCreditorAccountCollectionQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.AccountingNumber, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertStatusDateWasCalledOnGetCreditorAccountCollectionQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetCreditorAccountCollectionQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.StatusDate, Times.Exactly(3));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetContactAccountsAsyncWasCalledOnAccountingRepository()
        {
            QueryHandler sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            DateTime statusDate = _fixture.Create<DateTime>().Date;
            IGetCreditorAccountCollectionQuery query = CreateQuery(accountingNumber, statusDate);
            await sut.QueryAsync(query);

            _accountingRepositoryMock.Verify(m => m.GetContactAccountsAsync(
                    It.Is<int>(value => value == accountingNumber),
                    It.Is<DateTime>(value => value == statusDate)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoContactAccountCollectionWasReturnedFromAccountingRepository_ReturnsNotNull()
        {
            QueryHandler sut = CreateSut(false);

            IGetCreditorAccountCollectionQuery query = CreateQuery();
            IContactAccountCollection result = await sut.QueryAsync(query);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoContactAccountCollectionWasReturnedFromAccountingRepository_ReturnsEmptyContactAccountCollection()
        {
            QueryHandler sut = CreateSut(false);

            IGetCreditorAccountCollectionQuery query = CreateQuery();
            IContactAccountCollection result = await sut.QueryAsync(query);

            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoContactAccountCollectionWasReturnedFromAccountingRepository_ReturnsContactAccountCollectionWhereStatusDateIsEqualToStatusDateFromGetContactAccountCollectionQuery()
        {
            QueryHandler sut = CreateSut(false);

            DateTime statusDate = _fixture.Create<DateTime>().Date;
            IGetCreditorAccountCollectionQuery query = CreateQuery(statusDate: statusDate);
            IContactAccountCollection result = await sut.QueryAsync(query);

            Assert.That(result.StatusDate, Is.EqualTo(statusDate));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenContactAccountCollectionWasReturnedFromAccountingRepository_AssertCalculateAsyncWasCalledOnContactAccountCollectionFromAccountingRepository()
        {
            IContactAccountCollection calculatedContactAccountCollection = _fixture.BuildContactAccountCollectionMock().Object;
            Mock<IContactAccountCollection> contactAccountCollectionMock = _fixture.BuildContactAccountCollectionMock(calculatedContactAccountCollection: calculatedContactAccountCollection);
            QueryHandler sut = CreateSut(contactAccountCollection: contactAccountCollectionMock.Object);

            DateTime statusDate = _fixture.Create<DateTime>();
            IGetCreditorAccountCollectionQuery query = CreateQuery(statusDate: statusDate);
            await sut.QueryAsync(query);

            contactAccountCollectionMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenContactAccountCollectionWasReturnedFromAccountingRepository_AssertFindCreditorsAsyncWasCalledOnCalculatedContactAccountCollection()
        {
            Mock<IContactAccountCollection> calculatedContactAccountCollectionMock = _fixture.BuildContactAccountCollectionMock();
            IContactAccountCollection contactAccountCollection = _fixture.BuildContactAccountCollectionMock(calculatedContactAccountCollection: calculatedContactAccountCollectionMock.Object).Object;
            QueryHandler sut = CreateSut(contactAccountCollection: contactAccountCollection);

            DateTime statusDate = _fixture.Create<DateTime>();
            IGetCreditorAccountCollectionQuery query = CreateQuery(statusDate: statusDate);
            await sut.QueryAsync(query);

            calculatedContactAccountCollectionMock.Verify(m => m.FindCreditorsAsync(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenContactAccountCollectionWasReturnedFromAccountingRepository_ReturnsCalculatedCreditorAccountCollection()
        {
            IContactAccountCollection calculatedCreditorAccountCollection = _fixture.BuildContactAccountCollectionMock().Object;
            IContactAccountCollection creditorAccountCollection = _fixture.BuildContactAccountCollectionMock(calculatedContactAccountCollection: calculatedCreditorAccountCollection).Object;
            IContactAccountCollection calculatedContactAccountCollection = _fixture.BuildContactAccountCollectionMock(findCreditorContactAccountCollection: creditorAccountCollection).Object;
            IContactAccountCollection contactAccountCollection = _fixture.BuildContactAccountCollectionMock(calculatedContactAccountCollection: calculatedContactAccountCollection).Object;
            QueryHandler sut = CreateSut(contactAccountCollection: contactAccountCollection);

            IGetCreditorAccountCollectionQuery query = CreateQuery();
            IContactAccountCollection result = await sut.QueryAsync(query);

            Assert.That(result, Is.EqualTo(calculatedCreditorAccountCollection));
        }

        private QueryHandler CreateSut(bool hasContactAccountCollection = true, IContactAccountCollection contactAccountCollection = null)
        {
            _accountingRepositoryMock.Setup(m => m.GetContactAccountsAsync(It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(hasContactAccountCollection ? contactAccountCollection ?? _fixture.BuildContactAccountCollectionMock().Object : null));

            return new QueryHandler(_validatorMock.Object, _accountingRepositoryMock.Object);
        }

        private IGetCreditorAccountCollectionQuery CreateQuery(int? accountingNumber = null, DateTime? statusDate = null)
        {
            return CreateQueryMock(accountingNumber, statusDate).Object;
        }

        private Mock<IGetCreditorAccountCollectionQuery> CreateQueryMock(int? accountingNumber = null, DateTime? statusDate = null)
        {
            Mock<IGetCreditorAccountCollectionQuery> queryMock = new Mock<IGetCreditorAccountCollectionQuery>();
            queryMock.Setup(m => m.AccountingNumber)
                .Returns(accountingNumber ?? _fixture.Create<int>());
            queryMock.Setup(m => m.StatusDate)
                .Returns(statusDate ?? _fixture.Create<DateTime>().Date);
            return queryMock;
        }
    }
}