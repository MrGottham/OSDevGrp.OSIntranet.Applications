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
    public class ContactAccountsAsyncTests
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
            _fixture.Customize<IPaymentTerm>(builder => builder.FromFactory(() => _fixture.BuildPaymentTermMock().Object));
            _fixture.Customize<IContactAccount>(builder => builder.FromFactory(() => _fixture.BuildContactAccountMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ContactAccountsAsync_WhenCalledWithoutStatusDate_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            await sut.ContactAccountsAsync(accountingNumber);

            _queryBusMock.Verify(m => m.QueryAsync<IGetContactAccountCollectionQuery, IContactAccountCollection>(It.Is<IGetContactAccountCollectionQuery>(value => value.AccountingNumber == accountingNumber && value.StatusDate == DateTime.Today)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ContactAccountsAsync_WhenCalledWithStatusDate_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.ContactAccountsAsync(accountingNumber, statusDate);

            _queryBusMock.Verify(m => m.QueryAsync<IGetContactAccountCollectionQuery, IContactAccountCollection>(It.Is<IGetContactAccountCollectionQuery>(value => value.AccountingNumber == accountingNumber && value.StatusDate == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ContactAccountsAsync_WhenCalled_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            ActionResult<ContactAccountCollectionModel> result = await sut.ContactAccountsAsync(_fixture.Create<int>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ContactAccountsAsync_WhenCalled_ReturnsActionResultWhereResultIsNotNull()
        {
            Controller sut = CreateSut();

            ActionResult<ContactAccountCollectionModel> result = await sut.ContactAccountsAsync(_fixture.Create<int>());

            Assert.That(result.Result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ContactAccountsAsync_WhenCalled_ReturnsActionResultWhereResultIsOkObjectResult()
        {
            Controller sut = CreateSut();

            ActionResult<ContactAccountCollectionModel> result = await sut.ContactAccountsAsync(_fixture.Create<int>());

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ContactAccountsAsync_WhenCalled_ReturnsOkObjectResultWhereValueIsNotNull()
        {
            Controller sut = CreateSut();

            OkObjectResult result = (OkObjectResult) (await sut.ContactAccountsAsync(_fixture.Create<int>())).Result;

            Assert.That(result.Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ContactAccountsAsync_WhenCalled_ReturnsOkObjectResultWhereValueIsContactAccountCollectionModel()
        {
            Controller sut = CreateSut();

            OkObjectResult result = (OkObjectResult) (await sut.ContactAccountsAsync(_fixture.Create<int>())).Result;

            Assert.That(result.Value, Is.TypeOf<ContactAccountCollectionModel>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ContactAccountsAsync_WhenCalled_ReturnsOkObjectResultWhereValueIsContactAccountCollectionModelContainingAllContactAccounts()
        {
            IList<IContactAccount> contactAccounts = _fixture.CreateMany<IContactAccount>(_random.Next(5, 10)).ToList();
            IContactAccountCollection contactAccountCollection = _fixture.BuildContactAccountCollectionMock(contactAccountCollection: contactAccounts).Object;
            Controller sut = CreateSut(contactAccountCollection);

            OkObjectResult result = (OkObjectResult) (await sut.ContactAccountsAsync(_fixture.Create<int>())).Result;

            ContactAccountCollectionModel contactAccountCollectionModel = (ContactAccountCollectionModel) result.Value;
            Assert.That(contactAccountCollectionModel, Is.Not.Null);
            Assert.That(contactAccountCollectionModel.Count, Is.EqualTo(contactAccounts.Count));
            Assert.That(contactAccountCollectionModel.All(contactAccountModel => contactAccounts.SingleOrDefault(contactAccount => string.CompareOrdinal(contactAccountModel.AccountNumber, contactAccount.AccountNumber) == 0) != null), Is.True);
        }

        private Controller CreateSut(IContactAccountCollection contactAccountCollection = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<IGetContactAccountCollectionQuery, IContactAccountCollection>(It.IsAny<IGetContactAccountCollectionQuery>()))
                .Returns(Task.FromResult(contactAccountCollection ?? _fixture.BuildContactAccountCollectionMock().Object));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object);
        }
    }
}