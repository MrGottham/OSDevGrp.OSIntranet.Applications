using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Models.Accounting;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.AccountingController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.AccountingController
{
    [TestFixture]
    public class LoadAccountingsTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Mock<IClaimResolver> _claimResolverMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _claimResolverMock = new Mock<IClaimResolver>();

            _fixture = new Fixture();
            _fixture.Customize<ILetterHead>(builder => builder.FromFactory(() => _fixture.BuildLetterHeadMock().Object));
            _fixture.Customize<IAccounting>(builder => builder.FromFactory(() => _fixture.BuildAccountingMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccountings_WhenCalled_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            await sut.LoadAccountings();

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<IAccounting>>(It.IsNotNull<EmptyQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccountings_WhenCalled_ReturnsPartialViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.LoadAccountings();

            Assert.That(result, Is.TypeOf<PartialViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccountings_WhenCalled_ReturnsPartialViewResultWhereViewNameIsEqualToAccountingCollectionPartial()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.LoadAccountings();

            Assert.That(result.ViewName, Is.EqualTo("_AccountingCollectionPartial"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccountings_WhenCalled_ReturnsPartialViewResultWhereModelIsCollectionOfAccountingIdentificationViewModel()
        {
            IEnumerable<IAccounting> accountingCollection = _fixture.CreateMany<IAccounting>(_random.Next(5, 10)).ToList();
            Controller sut = CreateSut(accountingCollection);

            PartialViewResult result = (PartialViewResult) await sut.LoadAccountings();

            Assert.That(result.Model, Is.TypeOf<List<AccountingIdentificationViewModel>>());

            List<AccountingIdentificationViewModel> accountingViewModelCollection = result.Model as List<AccountingIdentificationViewModel>;
            Assert.That(accountingViewModelCollection, Is.Not.Null);
            Assert.That(accountingViewModelCollection.All(accountingViewModel => accountingCollection.SingleOrDefault(accounting => accounting.Number == accountingViewModel.AccountingNumber) != null), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccountings_WhenCalledWithoutAccountingNumber_ReturnsPartialViewResultWhereViewDataDoesNotContainAccountingNumber()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.LoadAccountings();

            Assert.That(result.ViewData.ContainsKey("AccountingNumber"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccountings_WhenCalledWithAccountingNumber_ReturnsPartialViewResultWhereViewDataContainsAccountingNumber()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            PartialViewResult result = (PartialViewResult) await sut.LoadAccountings(accountingNumber);

            Assert.That(result.ViewData.ContainsKey("AccountingNumber"), Is.True);
            Assert.That(result.ViewData["AccountingNumber"], Is.EqualTo(accountingNumber));
        }

        private Controller CreateSut(IEnumerable<IAccounting> accountingCollection = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<IAccounting>>(It.IsAny<EmptyQuery>()))
                .Returns(Task.FromResult(accountingCollection ?? _fixture.CreateMany<IAccounting>(_random.Next(5, 10)).ToList()));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
        }
    }
}