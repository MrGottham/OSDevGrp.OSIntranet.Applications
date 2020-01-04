using System;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.WebApi.Models.Accounting;
using OSDevGrp.OSIntranet.WebApi.Models.Core;
using Controller=OSDevGrp.OSIntranet.WebApi.Controllers.AccountingController;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Controllers.AccountingController
{
    [TestFixture]
    public class AccountingAsyncTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();

            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountingAsync_WhenCalledWithoutStatusDate_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            await sut.AccountingAsync(accountingNumber);

            _queryBusMock.Verify(m => m.QueryAsync<IGetAccountingQuery, IAccounting>(It.Is<IGetAccountingQuery>(value => value.AccountingNumber == accountingNumber && value.StatusDate == DateTime.Today)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountingAsync_WhenCalledWithStatusDate_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.AccountingAsync(accountingNumber, statusDate);

            _queryBusMock.Verify(m => m.QueryAsync<IGetAccountingQuery, IAccounting>(It.Is<IGetAccountingQuery>(value => value.AccountingNumber == accountingNumber && value.StatusDate == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountingAsync_WhenCalled_ReturnsOkObjectResult()
        {
            Controller sut = CreateSut();

            ActionResult<AccountingModel> result = await sut.AccountingAsync(_fixture.Create<int>());

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountingAsync_WhenCalled_AssertOkObjectResultContainsAccounting()
        {
            IAccounting accountingMock = _fixture.BuildAccountingMock().Object;
            Controller sut = CreateSut(accountingMock);

            OkObjectResult result = (OkObjectResult) (await sut.AccountingAsync(_fixture.Create<int>())).Result;

            Assert.That(result.Value, Is.Not.Null);

            AccountingModel accountingModel = (AccountingModel) result.Value;
            Assert.That(accountingModel, Is.Not.Null);
            Assert.That(accountingModel.Number, Is.EqualTo(accountingMock.Number));
            Assert.That(accountingModel.Name, Is.EqualTo(accountingMock.Name));
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountingAsync_WhenCalledAndIntranetExceptionOccurs_ReturnsBadRequestObjectResult()
        {
            IntranetRepositoryException intranetRepositoryException = new IntranetRepositoryException(_fixture.Create<ErrorCode>(), _fixture.Create<string>());
            Controller sut = CreateSut(exception: intranetRepositoryException);

            ActionResult<AccountingModel> result = await sut.AccountingAsync(_fixture.Create<int>());

            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountingAsync_WhenCalledAndIntranetExceptionOccurs_AssertBadRequestObjectResultContainsErrorModel()
        {
            IntranetRepositoryException intranetRepositoryException = new IntranetRepositoryException(_fixture.Create<ErrorCode>(), _fixture.Create<string>());
            Controller sut = CreateSut(exception: intranetRepositoryException);

            BadRequestObjectResult result = (BadRequestObjectResult) (await sut.AccountingAsync(_fixture.Create<int>())).Result;

            Assert.That(result.Value, Is.TypeOf<ErrorModel>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountingAsync_WhenCalledAndAggregateExceptionOccurs_ReturnsBadRequestObjectResult()
        {
            AggregateException aggregateException = new AggregateException(new IntranetRepositoryException(_fixture.Create<ErrorCode>(), _fixture.Create<string>()));
            Controller sut = CreateSut(exception: aggregateException);

            ActionResult<AccountingModel> result = await sut.AccountingAsync(_fixture.Create<int>());

            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountingAsync_WhenCalledAndAggregateExceptionOccurs_AssertBadRequestObjectResultContainsErrorModel()
        {
            AggregateException aggregateException = new AggregateException(new IntranetRepositoryException(_fixture.Create<ErrorCode>(), _fixture.Create<string>()));
            Controller sut = CreateSut(exception: aggregateException);

            BadRequestObjectResult result = (BadRequestObjectResult) (await sut.AccountingAsync(_fixture.Create<int>())).Result;

            Assert.That(result.Value, Is.TypeOf<ErrorModel>());
        }

        private Controller CreateSut(IAccounting accounting = null, Exception exception = null)
        {
            if (exception == null)
            {
                _queryBusMock.Setup(m => m.QueryAsync<IGetAccountingQuery, IAccounting>(It.IsAny<IGetAccountingQuery>()))
                    .Returns(Task.Run(() => accounting ?? _fixture.BuildAccountingMock().Object));
            }
            else
            {
                _queryBusMock.Setup(m => m.QueryAsync<IGetAccountingQuery, IAccounting>(It.IsAny<IGetAccountingQuery>()))
                    .Throws(exception);
            }

            return new Controller(_commandBusMock.Object, _queryBusMock.Object);
        }
    }
}
