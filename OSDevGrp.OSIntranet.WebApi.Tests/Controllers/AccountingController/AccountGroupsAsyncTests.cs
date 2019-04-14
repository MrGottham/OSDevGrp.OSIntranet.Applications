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
    public class AccountGroupsAsyncTests
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
            _fixture.Customize<IAccountGroup>(builder => builder.FromFactory(() => _fixture.BuildAccountGroupMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountGroupsAsync_WhenCalled_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            await sut.AccountGroupsAsync();

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<IAccountGroup>>(It.IsNotNull<EmptyQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountGroupsAsync_WhenCalled_ReturnsOkObjectResult()
        {
            Controller sut = CreateSut();

            ActionResult<IEnumerable<AccountGroupModel>> result = await sut.AccountGroupsAsync();

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountGroupsAsync_WhenCalled_AssertOkObjectResultContainsAccountGroups()
        {
            IList<IAccountGroup> accountGroupMockCollection = _fixture.CreateMany<IAccountGroup>(_random.Next(5, 10)).ToList();
            Controller sut = CreateSut(accountGroupMockCollection);

            OkObjectResult result = (OkObjectResult) (await sut.AccountGroupsAsync()).Result;

            Assert.That(result.Value, Is.Not.Null);

            IList<AccountGroupModel> accountGroupModels = ((IEnumerable<AccountGroupModel>) result.Value).ToList();
            Assert.That(accountGroupModels, Is.Not.Null);
            Assert.That(accountGroupModels, Is.Not.Empty);
            Assert.That(accountGroupModels.Count, Is.EqualTo(accountGroupMockCollection.Count));
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountGroupsAsync_WhenCalledAndIntranetExceptionOccurs_ReturnsBadRequestObjectResult()
        {
            IntranetRepositoryException intranetRepositoryException = new IntranetRepositoryException(_fixture.Create<ErrorCode>(), _fixture.Create<string>());
            Controller sut = CreateSut(exception: intranetRepositoryException);

            ActionResult<IEnumerable<AccountGroupModel>> result = await sut.AccountGroupsAsync();

            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountGroupsAsync_WhenCalledAndIntranetExceptionOccurs_AssertBadRequestObjectResultContainsErrorModel()
        {
            IntranetRepositoryException intranetRepositoryException = new IntranetRepositoryException(_fixture.Create<ErrorCode>(), _fixture.Create<string>());
            Controller sut = CreateSut(exception: intranetRepositoryException);

            BadRequestObjectResult result = (BadRequestObjectResult) (await sut.AccountGroupsAsync()).Result;

            Assert.That(result.Value, Is.TypeOf<ErrorModel>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountGroupsAsync_WhenCalledAndAggregateExceptionOccurs_ReturnsBadRequestObjectResult()
        {
            AggregateException aggregateException = new AggregateException(new IntranetRepositoryException(_fixture.Create<ErrorCode>(), _fixture.Create<string>()));
            Controller sut = CreateSut(exception: aggregateException);

            ActionResult<IEnumerable<AccountGroupModel>> result = await sut.AccountGroupsAsync();

            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountGroupsAsync_WhenCalledAndAggregateExceptionOccurs_AssertBadRequestObjectResultContainsErrorModel()
        {
            AggregateException aggregateException = new AggregateException(new IntranetRepositoryException(_fixture.Create<ErrorCode>(), _fixture.Create<string>()));
            Controller sut = CreateSut(exception: aggregateException);

            BadRequestObjectResult result = (BadRequestObjectResult) (await sut.AccountGroupsAsync()).Result;

            Assert.That(result.Value, Is.TypeOf<ErrorModel>());
        }

        private Controller CreateSut(IEnumerable<IAccountGroup> accountGroups = null, Exception exception = null)
        {
            if (exception == null)
            {
                _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<IAccountGroup>>(It.IsAny<EmptyQuery>()))
                    .Returns(Task.Run(() => accountGroups ?? _fixture.CreateMany<IAccountGroup>(_random.Next(5, 10)).ToList()));
            }
            else
            {
                _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<IAccountGroup>>(It.IsAny<EmptyQuery>()))
                    .Throws(exception);
            }

            return new Controller(_commandBusMock.Object, _queryBusMock.Object);
        }
    }
}
