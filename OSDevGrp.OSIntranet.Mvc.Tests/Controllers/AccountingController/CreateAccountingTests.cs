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
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Models.Accounting;
using OSDevGrp.OSIntranet.Mvc.Models.Core;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.AccountingController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.AccountingController
{
    [TestFixture]
    public class CreateAccountingTests
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

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccounting_WhenCalled_AssertQueryAsyncWasCalledOnQueryBusWithEmptyQueryForLetterHeadCollection()
        {
            Controller sut = CreateSut();

            await sut.CreateAccounting();

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<ILetterHead>>(It.IsNotNull<EmptyQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccounting_WhenCalled_ReturnsPartialViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.CreateAccounting();

            Assert.That(result, Is.TypeOf<PartialViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccounting_WhenCalled_ReturnsPartialViewResultWhereViewNameIsEqualToPresentAccountingPartial()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateAccounting();

            Assert.That(result.ViewName, Is.EqualTo("_PresentAccountingPartial"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccounting_WhenCalled_ReturnsPartialViewResultWhereModelIsAccountingViewModel()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateAccounting();

            Assert.That(result.Model, Is.TypeOf<AccountingViewModel>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccounting_WhenCalled_ReturnsPartialViewResultWhereModelIsAccountingViewModelWhereLetterHeadIsEqualToFirstLetterHeadFromLetterHeadCollectionFromQueryBus()
        {
            IEnumerable<ILetterHead> letterHeadCollection = _fixture.CreateMany<ILetterHead>(_random.Next(5, 10)).OrderBy(letterHead => letterHead.Number).ToList();
            Controller sut = CreateSut(letterHeadCollection);

            PartialViewResult result = (PartialViewResult) await sut.CreateAccounting();

            AccountingViewModel accountingViewModel = (AccountingViewModel) result.Model;

            Assert.That(accountingViewModel.LetterHead, Is.Not.Null);
            Assert.That(accountingViewModel.LetterHead.Number, Is.EqualTo(letterHeadCollection.First().Number));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccounting_WhenCalled_ReturnsPartialViewResultWhereModelIsAccountingViewModelWhereBalanceBelowZeroIsEqualToCreditors()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateAccounting();

            AccountingViewModel accountingViewModel = (AccountingViewModel) result.Model;

            Assert.That(accountingViewModel.BalanceBelowZero, Is.EqualTo(BalanceBelowZeroType.Creditors));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccounting_WhenCalled_ReturnsPartialViewResultWhereModelIsAccountingViewModelWhereBackDatingIsEqualTo30()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateAccounting();

            AccountingViewModel accountingViewModel = (AccountingViewModel) result.Model;

            Assert.That(accountingViewModel.BackDating, Is.EqualTo(30));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccounting_WhenCalled_ReturnsPartialViewResultWhereModelIsAccountingViewModelWhereEditModeIsEqualToCreate()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateAccounting();

            AccountingViewModel accountingViewModel = (AccountingViewModel) result.Model;

            Assert.That(accountingViewModel.EditMode, Is.EqualTo(EditMode.Create));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccounting_WhenCalled_ReturnsPartialViewResultWhereModelIsAccountingViewModelWhereLetterHeadsIsEqualToLetterHeadCollectionFromQueryBus()
        {
            IEnumerable<ILetterHead> letterHeadCollection = _fixture.CreateMany<ILetterHead>(_random.Next(5, 10)).ToList();
            Controller sut = CreateSut(letterHeadCollection);

            PartialViewResult result = (PartialViewResult) await sut.CreateAccounting();

            AccountingViewModel accountingViewModel = (AccountingViewModel) result.Model;

            Assert.That(accountingViewModel.LetterHeads, Is.Not.Null);
            Assert.That(accountingViewModel.LetterHeads.Count, Is.EqualTo(letterHeadCollection.Count()));
            Assert.That(accountingViewModel.LetterHeads.All(letterHeadViewModel => letterHeadCollection.Any(letterHead => letterHead.Number == letterHeadViewModel.Number)), Is.True);
        }

        private Controller CreateSut(IEnumerable<ILetterHead> letterHeadCollection = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<ILetterHead>>(It.IsAny<EmptyQuery>()))
                .Returns(Task.FromResult(letterHeadCollection ?? _fixture.CreateMany<ILetterHead>(_random.Next(5, 10)).ToList()));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
        }
    }
}