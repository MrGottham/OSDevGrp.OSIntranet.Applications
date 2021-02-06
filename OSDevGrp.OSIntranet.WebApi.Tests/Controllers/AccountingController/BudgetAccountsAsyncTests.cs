using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.WebApi.Models.Accounting;
using Controller=OSDevGrp.OSIntranet.WebApi.Controllers.AccountingController;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Controllers.AccountingController
{
    [TestFixture]
    public class BudgetAccountsAsyncTests
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
            _fixture.Customize<IBudgetAccount>(builder => builder.FromFactory(() => _fixture.BuildBudgetAccountMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task BudgetAccountsAsync_WhenCalledWithoutStatusDate_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            await sut.BudgetAccountsAsync(accountingNumber);

            _queryBusMock.Verify(m => m.QueryAsync<IGetBudgetAccountCollectionQuery, IBudgetAccountCollection>(It.Is<IGetBudgetAccountCollectionQuery>(value => value.AccountingNumber == accountingNumber && value.StatusDate == DateTime.Today)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task BudgetAccountsAsync_WhenCalledWithStatusDate_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.BudgetAccountsAsync(accountingNumber, statusDate);

            _queryBusMock.Verify(m => m.QueryAsync<IGetBudgetAccountCollectionQuery, IBudgetAccountCollection>(It.Is<IGetBudgetAccountCollectionQuery>(value => value.AccountingNumber == accountingNumber && value.StatusDate == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task BudgetAccountsAsync_WhenCalled_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            ActionResult<BudgetAccountCollectionModel> result = await sut.BudgetAccountsAsync(_fixture.Create<int>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task BudgetAccountsAsync_WhenCalled_ReturnsActionResultWhereResultIsNotNull()
        {
            Controller sut = CreateSut();

            ActionResult<BudgetAccountCollectionModel> result = await sut.BudgetAccountsAsync(_fixture.Create<int>());

            Assert.That(result.Result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task BudgetAccountsAsync_WhenCalled_ReturnsActionResultWhereResultIsOkObjectResult()
        {
            Controller sut = CreateSut();

            ActionResult<BudgetAccountCollectionModel> result = await sut.BudgetAccountsAsync(_fixture.Create<int>());

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task BudgetAccountsAsync_WhenCalled_ReturnsOkObjectResultWhereValueIsNotNull()
        {
            Controller sut = CreateSut();

            OkObjectResult result = (OkObjectResult) (await sut.BudgetAccountsAsync(_fixture.Create<int>())).Result;

            Assert.That(result.Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task BudgetAccountsAsync_WhenCalled_ReturnsOkObjectResultWhereValueIsBudgetAccountCollectionModel()
        {
            Controller sut = CreateSut();

            OkObjectResult result = (OkObjectResult) (await sut.BudgetAccountsAsync(_fixture.Create<int>())).Result;

            Assert.That(result.Value, Is.TypeOf<BudgetAccountCollectionModel>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task BudgetAccountsAsync_WhenCalled_ReturnsOkObjectResultWhereValueIsBudgetAccountCollectionModelContainingAllBudgetAccounts()
        {
            IList<IBudgetAccount> budgetAccounts = _fixture.CreateMany<IBudgetAccount>(_random.Next(5, 10)).ToList();
            IBudgetAccountCollection budgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock(budgetAccountCollection: budgetAccounts).Object;
            Controller sut = CreateSut(budgetAccountCollection);

            OkObjectResult result = (OkObjectResult) (await sut.BudgetAccountsAsync(_fixture.Create<int>())).Result;

            BudgetAccountCollectionModel budgetAccountCollectionModel = (BudgetAccountCollectionModel) result.Value;
            Assert.That(budgetAccountCollectionModel, Is.Not.Null);
            Assert.That(budgetAccountCollectionModel.Count, Is.EqualTo(budgetAccounts.Count));
            Assert.That(budgetAccountCollectionModel.All(budgetAccountModel => budgetAccounts.SingleOrDefault(budgetAccount => string.CompareOrdinal(budgetAccountModel.AccountNumber, budgetAccount.AccountNumber) == 0) != null), Is.True);
        }

        private Controller CreateSut(IBudgetAccountCollection budgetAccountCollection = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<IGetBudgetAccountCollectionQuery, IBudgetAccountCollection>(It.IsAny<IGetBudgetAccountCollectionQuery>()))
                .Returns(Task.FromResult(budgetAccountCollection ?? _fixture.BuildBudgetAccountCollectionMock().Object));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object);
        }
    }
}