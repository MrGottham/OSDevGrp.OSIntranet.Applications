using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Models.Accounting;
using OSDevGrp.OSIntranet.Mvc.Models.Core;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.AccountingController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.AccountingController
{
    [TestFixture]
    public class LoadAccountingTests
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
        public async Task LoadAccounting_WhenCalled_AssertQueryAsyncWasCalledOnQueryBusWithEmptyQueryForLetterHeadCollection()
        {
            Controller sut = CreateSut();

            await sut.LoadAccounting(_fixture.Create<int>());

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<ILetterHead>>(It.IsNotNull<EmptyQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccounting_WhenCalled_AssertQueryAsyncWasCalledOnQueryBusWithGetAccountGroupQuery()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            await sut.LoadAccounting(accountingNumber);

            _queryBusMock.Verify(m => m.QueryAsync<IGetAccountingQuery, IAccounting>(It.Is<IGetAccountingQuery>(value => value != null && value.AccountingNumber == accountingNumber && value.StatusDate == DateTime.Today)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccounting_WhenNoAccountingWasReturnedFromQueryBus_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut(hasAccounting: false);

            IActionResult result = await sut.LoadAccounting(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccounting_WhenAccountingWasReturnedFromQueryBus_ReturnsPartialViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.LoadAccounting(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<PartialViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccounting_WhenAccountingWasReturnedFromQueryBus_ReturnsPartialViewResultWhereViewNameIsEqualToPresentAccountingPartial()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.LoadAccounting(_fixture.Create<int>());

            Assert.That(result.ViewName, Is.EqualTo("_PresentAccountingPartial"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccounting_WhenAccountingWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsAccountingViewModel()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.LoadAccounting(_fixture.Create<int>());

            Assert.That(result.Model, Is.TypeOf<AccountingViewModel>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccounting_WhenAccountingWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsAccountingViewModelWithAccountsNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.LoadAccounting(_fixture.Create<int>());

            AccountingViewModel accountingViewModel = (AccountingViewModel) result.Model;

            Assert.That(accountingViewModel.Accounts, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccounting_WhenAccountingWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsAccountingViewModelWithBudgetAccountsNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.LoadAccounting(_fixture.Create<int>());

            AccountingViewModel accountingViewModel = (AccountingViewModel) result.Model;

            Assert.That(accountingViewModel.BudgetAccounts, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccounting_WhenAccountingWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsAccountingViewModelWithContactAccountsNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.LoadAccounting(_fixture.Create<int>());

            AccountingViewModel accountingViewModel = (AccountingViewModel) result.Model;

            Assert.That(accountingViewModel.ContactAccounts, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccounting_WhenAccountingWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsAccountingViewModelWithPostingLinesNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.LoadAccounting(_fixture.Create<int>());

            AccountingViewModel accountingViewModel = (AccountingViewModel) result.Model;

            Assert.That(accountingViewModel.PostingLines, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccounting_WhenAccountingWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsAccountingViewModelWhereEditModeIsEqualToNone()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.LoadAccounting(_fixture.Create<int>());

            AccountingViewModel accountingViewModel = (AccountingViewModel) result.Model;

            Assert.That(accountingViewModel.EditMode, Is.EqualTo(EditMode.None));
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccounting_WhenAccountingWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsAccountingViewModelWhereLetterHeadsIsEqualToLetterHeadCollectionFromQueryBus()
        {
            IEnumerable<ILetterHead> letterHeadCollection = _fixture.CreateMany<ILetterHead>(_random.Next(5, 10)).ToList();
            Controller sut = CreateSut(letterHeadCollection);

            PartialViewResult result = (PartialViewResult) await sut.LoadAccounting(_fixture.Create<int>());

            AccountingViewModel accountingViewModel = (AccountingViewModel) result.Model;

            Assert.That(accountingViewModel.LetterHeads, Is.Not.Null);
            Assert.That(accountingViewModel.LetterHeads.Count, Is.EqualTo(letterHeadCollection.Count()));
            Assert.That(accountingViewModel.LetterHeads.All(letterHeadViewModel => letterHeadCollection.Any(letterHead => letterHead.Number == letterHeadViewModel.Number)), Is.True);
        }

        private Controller CreateSut(IEnumerable<ILetterHead> letterHeadCollection = null, bool hasAccounting = true, IAccounting accounting = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<ILetterHead>>(It.IsAny<EmptyQuery>()))
                .Returns(Task.FromResult(letterHeadCollection ?? _fixture.CreateMany<ILetterHead>(_random.Next(5, 10)).ToList()));
            _queryBusMock.Setup(m => m.QueryAsync<IGetAccountingQuery, IAccounting>(It.IsAny<IGetAccountingQuery>()))
                .Returns(Task.FromResult(hasAccounting ? accounting ?? _fixture.BuildAccountingMock().Object : accounting));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
        }
    }
}