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
    public class AccountsAsyncTests
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
            _fixture.Customize<IAccount>(builder => builder.FromFactory(() => _fixture.BuildAccountMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountsAsync_WhenCalledWithoutStatusDate_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            await sut.AccountsAsync(accountingNumber);

            _queryBusMock.Verify(m => m.QueryAsync<IGetAccountCollectionQuery, IAccountCollection>(It.Is<IGetAccountCollectionQuery>(value => value.AccountingNumber == accountingNumber && value.StatusDate == DateTime.Today)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountsAsync_WhenCalledWithStatusDate_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.AccountsAsync(accountingNumber, statusDate);

            _queryBusMock.Verify(m => m.QueryAsync<IGetAccountCollectionQuery, IAccountCollection>(It.Is<IGetAccountCollectionQuery>(value => value.AccountingNumber == accountingNumber && value.StatusDate == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountsAsync_WhenCalled_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            ActionResult<AccountCollectionModel> result = await sut.AccountsAsync(_fixture.Create<int>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountsAsync_WhenCalled_ReturnsActionResultWhereResultIsNotNull()
        {
            Controller sut = CreateSut();

            ActionResult<AccountCollectionModel> result = await sut.AccountsAsync(_fixture.Create<int>());

            Assert.That(result.Result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountsAsync_WhenCalled_ReturnsActionResultWhereResultIsOkObjectResult()
        {
            Controller sut = CreateSut();

            ActionResult<AccountCollectionModel> result = await sut.AccountsAsync(_fixture.Create<int>());

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountsAsync_WhenCalled_ReturnsOkObjectResultWhereValueIsNotNull()
        {
            Controller sut = CreateSut();

            OkObjectResult result = (OkObjectResult) (await sut.AccountsAsync(_fixture.Create<int>())).Result;

            Assert.That(result.Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountsAsync_WhenCalled_ReturnsOkObjectResultWhereValueIsAccountCollectionModel()
        {
            Controller sut = CreateSut();

            OkObjectResult result = (OkObjectResult) (await sut.AccountsAsync(_fixture.Create<int>())).Result;

            Assert.That(result.Value, Is.TypeOf<AccountCollectionModel>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountsAsync_WhenCalled_ReturnsOkObjectResultWhereValueIsAccountCollectionModelContainingAllAccounts()
        {
            IList<IAccount> accounts = _fixture.CreateMany<IAccount>(_random.Next(5, 10)).ToList();
            IAccountCollection accountCollection = _fixture.BuildAccountCollectionMock(accountCollection: accounts).Object;
            Controller sut = CreateSut(accountCollection);

            OkObjectResult result = (OkObjectResult) (await sut.AccountsAsync(_fixture.Create<int>())).Result;

            AccountCollectionModel accountCollectionModel = (AccountCollectionModel) result.Value;
            Assert.That(accountCollectionModel, Is.Not.Null);
            Assert.That(accountCollectionModel.Count, Is.EqualTo(accounts.Count));
            Assert.That(accountCollectionModel.All(accountModel => accounts.SingleOrDefault(account => string.CompareOrdinal(accountModel.AccountNumber, account.AccountNumber) == 0) != null), Is.True);
        }

        private Controller CreateSut(IAccountCollection accountCollection = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<IGetAccountCollectionQuery, IAccountCollection>(It.IsAny<IGetAccountCollectionQuery>()))
                .Returns(Task.FromResult(accountCollection ?? _fixture.BuildAccountCollectionMock().Object));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object);
        }
    }
}