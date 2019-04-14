using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.WebApi.Models.Accounting;
using OSDevGrp.OSIntranet.WebApi.Models.Core;
using Controller=OSDevGrp.OSIntranet.WebApi.Controllers.AccountingController;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Controllers.AccountingController
{
    [TestFixture]
    public class BudgetAccountGroupsAsyncTests
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
            _fixture.Customize<IBudgetAccountGroup>(builder => builder.FromFactory(() => _fixture.BuildBudgetAccountGroupMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task BudgetAccountGroupsAsync_WhenCalled_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            await sut.BudgetAccountGroupsAsync();

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<IBudgetAccountGroup>>(It.IsNotNull<EmptyQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task BudgetAccountGroupsAsync_WhenCalled_ReturnsOkObjectResult()
        {
            Controller sut = CreateSut();

            ActionResult<IEnumerable<BudgetAccountGroupModel>> result = await sut.BudgetAccountGroupsAsync();

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task BudgetAccountGroupsAsync_WhenCalled_AssertOkObjectResultContainsBudgetAccountGroups()
        {
            IList<IBudgetAccountGroup> budgetAccountGroupMockCollection = _fixture.CreateMany<IBudgetAccountGroup>(_random.Next(5, 10)).ToList();
            Controller sut = CreateSut(budgetAccountGroupMockCollection);

            OkObjectResult result = (OkObjectResult) (await sut.BudgetAccountGroupsAsync()).Result;

            Assert.That(result.Value, Is.Not.Null);

            IList<BudgetAccountGroupModel> budgetAccountGroupModels = ((IEnumerable<BudgetAccountGroupModel>) result.Value).ToList();
            Assert.That(budgetAccountGroupModels, Is.Not.Null);
            Assert.That(budgetAccountGroupModels, Is.Not.Empty);
            Assert.That(budgetAccountGroupModels.Count, Is.EqualTo(budgetAccountGroupMockCollection.Count));
        }

        [Test]
        [Category("UnitTest")]
        public async Task BudgetAccountGroupsAsync_WhenCalledAndIntranetExceptionOccurs_ReturnsBadRequestObjectResult()
        {
            IntranetRepositoryException intranetRepositoryException = new IntranetRepositoryException(_fixture.Create<ErrorCode>(), _fixture.Create<string>());
            Controller sut = CreateSut(exception: intranetRepositoryException);

            ActionResult<IEnumerable<BudgetAccountGroupModel>> result = await sut.BudgetAccountGroupsAsync();

            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task BudgetAccountGroupsAsync_WhenCalledAndIntranetExceptionOccurs_AssertBadRequestObjectResultContainsErrorModel()
        {
            IntranetRepositoryException intranetRepositoryException = new IntranetRepositoryException(_fixture.Create<ErrorCode>(), _fixture.Create<string>());
            Controller sut = CreateSut(exception: intranetRepositoryException);

            BadRequestObjectResult result = (BadRequestObjectResult) (await sut.BudgetAccountGroupsAsync()).Result;

            Assert.That(result.Value, Is.TypeOf<ErrorModel>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task BudgetAccountGroupsAsync_WhenCalledAndAggregateExceptionOccurs_ReturnsBadRequestObjectResult()
        {
            AggregateException aggregateException = new AggregateException(new IntranetRepositoryException(_fixture.Create<ErrorCode>(), _fixture.Create<string>()));
            Controller sut = CreateSut(exception: aggregateException);

            ActionResult<IEnumerable<BudgetAccountGroupModel>> result = await sut.BudgetAccountGroupsAsync();

            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task BudgetAccountGroupsAsync_WhenCalledAndAggregateExceptionOccurs_AssertBadRequestObjectResultContainsErrorModel()
        {
            AggregateException aggregateException = new AggregateException(new IntranetRepositoryException(_fixture.Create<ErrorCode>(), _fixture.Create<string>()));
            Controller sut = CreateSut(exception: aggregateException);

            BadRequestObjectResult result = (BadRequestObjectResult) (await sut.BudgetAccountGroupsAsync()).Result;

            Assert.That(result.Value, Is.TypeOf<ErrorModel>());
        }

        private Controller CreateSut(IEnumerable<IBudgetAccountGroup> budgetAccountGroups = null, Exception exception = null)
        {
            if (exception == null)
            {
                _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<IBudgetAccountGroup>>(It.IsAny<EmptyQuery>()))
                    .Returns(Task.Run(() => budgetAccountGroups ?? _fixture.CreateMany<IBudgetAccountGroup>(_random.Next(5, 10)).ToList()));
            }
            else
            {
                _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<IBudgetAccountGroup>>(It.IsAny<EmptyQuery>()))
                    .Throws(exception);
            }

            return new Controller(_commandBusMock.Object, _queryBusMock.Object);
        }
    }
}
