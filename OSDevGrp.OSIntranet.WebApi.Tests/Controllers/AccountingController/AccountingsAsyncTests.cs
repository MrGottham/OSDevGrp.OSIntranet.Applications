using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.WebApi.Models.Accounting;
using Controller=OSDevGrp.OSIntranet.WebApi.Controllers.AccountingController;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Controllers.AccountingController
{
    [TestFixture]
    public class AccountingsAsyncTests
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
            _fixture.Customize<ILetterHead>(builder => builder.FromFactory(() => _fixture.BuildLetterHeadMock().Object));
            _fixture.Customize<IAccounting>(builder => builder.FromFactory(() => _fixture.BuildAccountingMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountingsAsync_WhenCalled_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            await sut.AccountingsAsync();

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<IAccounting>>(It.IsNotNull<EmptyQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountingsAsync_WhenCalled_ReturnsOkObjectResult()
        {
            Controller sut = CreateSut();

            ActionResult<IEnumerable<AccountingModel>> result = await sut.AccountingsAsync();

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountingsAsync_WhenCalled_AssertOkObjectResultContainsAccountings()
        {
            IList<IAccounting> accountingMockCollection = _fixture.CreateMany<IAccounting>(_random.Next(5, 10)).ToList();
            Controller sut = CreateSut(accountingMockCollection);

            OkObjectResult result = (OkObjectResult) (await sut.AccountingsAsync()).Result;

            Assert.That(result.Value, Is.Not.Null);

            IList<AccountingModel> accountingModels = ((IEnumerable<AccountingModel>) result.Value).ToList();
            Assert.That(accountingModels, Is.Not.Null);
            Assert.That(accountingModels, Is.Not.Empty);
            Assert.That(accountingModels.Count, Is.EqualTo(accountingMockCollection.Count));
        }

        private Controller CreateSut(IEnumerable<IAccounting> accountings = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<IAccounting>>(It.IsAny<EmptyQuery>()))
                .Returns(Task.FromResult(accountings ?? _fixture.CreateMany<IAccounting>(_random.Next(5, 10)).ToList()));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object);
        }
    }
}