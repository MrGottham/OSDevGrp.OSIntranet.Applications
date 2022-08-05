using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Queries;
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
        public async Task LoadAccounting_WhenCalled_AssertQueryAsyncWasCalledOnQueryBusWithGetUserSpecificKeyQueryForPostingJournalKey()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            await sut.LoadAccounting(accountingNumber);

            _queryBusMock.Verify(m => m.QueryAsync<IGetUserSpecificKeyQuery, string>(It.Is<IGetUserSpecificKeyQuery>(query => query != null && query.KeyElementCollection != null && query.KeyElementCollection.Count() == 2 && string.CompareOrdinal(query.KeyElementCollection.ElementAtOrDefault(0), nameof(ApplyPostingJournalViewModel)) == 0 && string.CompareOrdinal(query.KeyElementCollection.ElementAt(1), Convert.ToString(accountingNumber)) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccounting_WhenCalled_AssertQueryAsyncWasCalledOnQueryBusWithGetUserSpecificKeyQueryForPostingJournalResultKey()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            await sut.LoadAccounting(accountingNumber);

            _queryBusMock.Verify(m => m.QueryAsync<IGetUserSpecificKeyQuery, string>(It.Is<IGetUserSpecificKeyQuery>(query => query != null && query.KeyElementCollection != null && query.KeyElementCollection.Count() == 2 && string.CompareOrdinal(query.KeyElementCollection.ElementAtOrDefault(0), nameof(ApplyPostingJournalResultViewModel)) == 0 && string.CompareOrdinal(query.KeyElementCollection.ElementAt(1), Convert.ToString(accountingNumber)) == 0)), Times.Once);
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
        public async Task LoadAccounting_WhenCalled_AssertQueryAsyncWasCalledOnQueryBusWithGetAccountingQuery()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            await sut.LoadAccounting(accountingNumber);

            _queryBusMock.Verify(m => m.QueryAsync<IGetAccountingQuery, IAccounting>(It.Is<IGetAccountingQuery>(value => value != null && value.AccountingNumber == accountingNumber && value.StatusDate == DateTime.Today)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccounting_WhenCalled_AssertQueryAsyncWasCalledOnQueryBusWithPullKeyValueEntryQueryForPostingJournalKey()
        {
            string postingJournalKey = _fixture.Create<string>();
            Controller sut = CreateSut(postingJournalKey: postingJournalKey);

            await sut.LoadAccounting(_fixture.Create<int>());

            _queryBusMock.Verify(m => m.QueryAsync<IPullKeyValueEntryQuery, IKeyValueEntry>(It.Is<IPullKeyValueEntryQuery>(query => query != null && string.CompareOrdinal(query.Key, postingJournalKey) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccounting_WhenKeyValueEntryForPostingJournalKeyWasReturnedFromQueryBus_AssertToObjectWasCalledOnKeyValueEntryForPostingJournalKey()
        {
            Mock<IKeyValueEntry> keyValueEntryMockForPostingJournalKey = _fixture.BuildKeyValueEntryMock<ApplyPostingJournalViewModel>();
            Controller sut = CreateSut(keyValueEntryForPostingJournalKey: keyValueEntryMockForPostingJournalKey.Object);

            await sut.LoadAccounting(_fixture.Create<int>());

            keyValueEntryMockForPostingJournalKey.Verify(m => m.ToObject<It.IsSubtype<ApplyPostingJournalViewModel>>(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccounting_WhenCalled_AssertQueryAsyncWasCalledOnQueryBusWithPullKeyValueEntryQueryForPostingJournalResultKey()
        {
            string postingJournalResultKey = _fixture.Create<string>();
            Controller sut = CreateSut(postingJournalResultKey: postingJournalResultKey);

            await sut.LoadAccounting(_fixture.Create<int>());

            _queryBusMock.Verify(m => m.QueryAsync<IPullKeyValueEntryQuery, IKeyValueEntry>(It.Is<IPullKeyValueEntryQuery>(query => query != null && string.CompareOrdinal(query.Key, postingJournalResultKey) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccounting_WhenKeyValueEntryForPostingJournalResultKeyWasReturnedFromQueryBus_AssertToObjectWasCalledOnKeyValueEntryForPostingJournalResultKey()
        {
            Mock<IKeyValueEntry> keyValueEntryMockForPostingJournalResultKey = _fixture.BuildKeyValueEntryMock<ApplyPostingJournalResultViewModel>();
            Controller sut = CreateSut(keyValueEntryForPostingJournalResultKey: keyValueEntryMockForPostingJournalResultKey.Object);

            await sut.LoadAccounting(_fixture.Create<int>());

            keyValueEntryMockForPostingJournalResultKey.Verify(m => m.ToObject<It.IsSubtype<ApplyPostingJournalResultViewModel>>(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccounting_WhenNoAccountingWasReturnedFromQueryBus_ReturnsNotNull()
        {
            Controller sut = CreateSut(hasAccounting: false);

            IActionResult result = await sut.LoadAccounting(_fixture.Create<int>());

            Assert.That(result, Is.Not.Null);
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
        public async Task LoadAccounting_WhenAccountingWasReturnedFromQueryBus_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.LoadAccounting(_fixture.Create<int>());

            Assert.That(result, Is.Not.Null);
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
        public async Task LoadAccounting_WhenAccountingWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsNotNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.LoadAccounting(_fixture.Create<int>());

            Assert.That(result.Model, Is.Not.Null);
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
        public async Task LoadAccounting_WhenAccountingWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsAccountingViewModelWithAccountingNumberEqualToAccountingNumberOnAccountingFromQueryBus()
        {
            int accountingNumber = _fixture.Create<int>();
            IAccounting accounting = _fixture.BuildAccountingMock(accountingNumber).Object;
            Controller sut = CreateSut(accounting: accounting);

            PartialViewResult result = (PartialViewResult) await sut.LoadAccounting(_fixture.Create<int>());

            AccountingViewModel accountingViewModel = (AccountingViewModel) result.Model;

            Assert.That(accountingViewModel.AccountingNumber, Is.EqualTo(accountingNumber));
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

            // ReSharper disable PossibleNullReferenceException
            Assert.That(accountingViewModel.PostingLines, Is.Not.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccounting_WhenAccountingWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsAccountingViewModelWithPostingLinesWhereViewModeIsEqualToWithDebitAndCredit()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult)await sut.LoadAccounting(_fixture.Create<int>());

            AccountingViewModel accountingViewModel = (AccountingViewModel)result.Model;

            // ReSharper disable PossibleNullReferenceException
            Assert.That(accountingViewModel.PostingLines.ViewMode, Is.EqualTo(PostingLineCollectionViewMode.WithDebitAndCredit));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccounting_WhenAccountingWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsAccountingViewModelWithPostingJournalKeyNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult)await sut.LoadAccounting(_fixture.Create<int>());

            AccountingViewModel accountingViewModel = (AccountingViewModel)result.Model;

            Assert.That(accountingViewModel.PostingJournalKey, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccounting_WhenAccountingWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsAccountingViewModelWithPostingJournalKeyEqualToPostingJournalKeyFromQueryBus()
        {
            string postingJournalKey = _fixture.Create<string>();
            Controller sut = CreateSut(postingJournalKey: postingJournalKey);

            PartialViewResult result = (PartialViewResult)await sut.LoadAccounting(_fixture.Create<int>());

            AccountingViewModel accountingViewModel = (AccountingViewModel)result.Model;

            Assert.That(accountingViewModel.PostingJournalKey, Is.EqualTo(postingJournalKey));
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccounting_WhenAccountingWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsAccountingViewModelWithPostingJournalNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult)await sut.LoadAccounting(_fixture.Create<int>());

            AccountingViewModel accountingViewModel = (AccountingViewModel)result.Model;

            Assert.That(accountingViewModel.PostingJournal, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccounting_WhenAccountingWasReturnedFromQueryBusAndNoKeyValueEntryForPostingJournalWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsAccountingViewModelWithPostingJournalWhereAccountingNumberIsEqualToAccountingNumberFromArgument()
        {
            Controller sut = CreateSut(hasKeyValueEntryForPostingJournalKey: false);

            int accountingNumber = _fixture.Create<int>();
            PartialViewResult result = (PartialViewResult)await sut.LoadAccounting(accountingNumber);

            AccountingViewModel accountingViewModel = (AccountingViewModel)result.Model;

            Assert.That(accountingViewModel.PostingJournal.AccountingNumber, Is.EqualTo(accountingNumber));
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccounting_WhenAccountingWasReturnedFromQueryBusAndNoKeyValueEntryForPostingJournalWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsAccountingViewModelWithPostingJournalWhereApplyPostingLinesNotEqualToNull()
        {
            Controller sut = CreateSut(hasKeyValueEntryForPostingJournalKey: false);

            PartialViewResult result = (PartialViewResult)await sut.LoadAccounting(_fixture.Create<int>());

            AccountingViewModel accountingViewModel = (AccountingViewModel)result.Model;

            Assert.That(accountingViewModel.PostingJournal.ApplyPostingLines, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccounting_WhenAccountingWasReturnedFromQueryBusAndNoKeyValueEntryForPostingJournalWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsAccountingViewModelWithPostingJournalWhereApplyPostingLinesIsEmpty()
        {
            Controller sut = CreateSut(hasKeyValueEntryForPostingJournalKey: false);

            PartialViewResult result = (PartialViewResult)await sut.LoadAccounting(_fixture.Create<int>());

            AccountingViewModel accountingViewModel = (AccountingViewModel)result.Model;

            Assert.That(accountingViewModel.PostingJournal.ApplyPostingLines, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccounting_WhenAccountingWasReturnedFromQueryBusAndKeyValueEntryForPostingJournalWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsAccountingViewModelWithPostingJournalEqualToPostingJournalFromQueryBus()
        {
            ApplyPostingJournalViewModel postingJournal = _fixture.Create<ApplyPostingJournalViewModel>();
            IKeyValueEntry keyValueEntryForPostingJournalKey = _fixture.BuildKeyValueEntryMock(toObject: postingJournal).Object;
            Controller sut = CreateSut(keyValueEntryForPostingJournalKey: keyValueEntryForPostingJournalKey);

            PartialViewResult result = (PartialViewResult)await sut.LoadAccounting(_fixture.Create<int>());

            AccountingViewModel accountingViewModel = (AccountingViewModel)result.Model;

            Assert.That(accountingViewModel.PostingJournal, Is.EqualTo(postingJournal));
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccounting_WhenAccountingWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsAccountingViewModelWithPostingJournalResultKeyNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult)await sut.LoadAccounting(_fixture.Create<int>());

            AccountingViewModel accountingViewModel = (AccountingViewModel)result.Model;

            Assert.That(accountingViewModel.PostingJournalResultKey, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccounting_WhenAccountingWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsAccountingViewModelWithPostingJournalResultKeyEqualToPostingJournalResultKeyFromQueryBus()
        {
            string postingJournalResultKey = _fixture.Create<string>();
            Controller sut = CreateSut(postingJournalResultKey: postingJournalResultKey);

            PartialViewResult result = (PartialViewResult)await sut.LoadAccounting(_fixture.Create<int>());

            AccountingViewModel accountingViewModel = (AccountingViewModel)result.Model;

            Assert.That(accountingViewModel.PostingJournalResultKey, Is.EqualTo(postingJournalResultKey));
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccounting_WhenAccountingWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsAccountingViewModelWithPostingJournalResultNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult)await sut.LoadAccounting(_fixture.Create<int>());

            AccountingViewModel accountingViewModel = (AccountingViewModel)result.Model;

            Assert.That(accountingViewModel.PostingJournalResult, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccounting_WhenAccountingWasReturnedFromQueryBusAndNoKeyValueEntryForPostingJournalResultWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsAccountingViewModelWithPostingJournalResultWherePostingLinesNotEqualToNull()
        {
            Controller sut = CreateSut(hasKeyValueEntryForPostingJournalResultKey: false);

            PartialViewResult result = (PartialViewResult)await sut.LoadAccounting(_fixture.Create<int>());

            AccountingViewModel accountingViewModel = (AccountingViewModel)result.Model;

            // ReSharper disable PossibleNullReferenceException
            Assert.That(accountingViewModel.PostingJournalResult.PostingLines, Is.Not.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccounting_WhenAccountingWasReturnedFromQueryBusAndNoKeyValueEntryForPostingJournalResultWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsAccountingViewModelWithPostingJournalResultWherePostingLinesIsEmpty()
        {
            Controller sut = CreateSut(hasKeyValueEntryForPostingJournalResultKey: false);

            PartialViewResult result = (PartialViewResult)await sut.LoadAccounting(_fixture.Create<int>());

            AccountingViewModel accountingViewModel = (AccountingViewModel)result.Model;

            // ReSharper disable PossibleNullReferenceException
            Assert.That(accountingViewModel.PostingJournalResult.PostingLines, Is.Empty);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccounting_WhenAccountingWasReturnedFromQueryBusAndNoKeyValueEntryForPostingJournalResultWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsAccountingViewModelWithPostingJournalResultWhereViewModeOnPostingLinesIsEqualToWithDebitAndCredit()
        {
            Controller sut = CreateSut(hasKeyValueEntryForPostingJournalResultKey: false);

            PartialViewResult result = (PartialViewResult)await sut.LoadAccounting(_fixture.Create<int>());

            AccountingViewModel accountingViewModel = (AccountingViewModel)result.Model;

            // ReSharper disable PossibleNullReferenceException
            Assert.That(accountingViewModel.PostingJournalResult.PostingLines.ViewMode, Is.EqualTo(PostingLineCollectionViewMode.WithDebitAndCredit));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccounting_WhenAccountingWasReturnedFromQueryBusAndNoKeyValueEntryForPostingJournalResultWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsAccountingViewModelWithPostingJournalResultWherePostingWarningsNotEqualToNull()
        {
            Controller sut = CreateSut(hasKeyValueEntryForPostingJournalResultKey: false);

            PartialViewResult result = (PartialViewResult)await sut.LoadAccounting(_fixture.Create<int>());

            AccountingViewModel accountingViewModel = (AccountingViewModel)result.Model;

            Assert.That(accountingViewModel.PostingJournalResult.PostingWarnings, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccounting_WhenAccountingWasReturnedFromQueryBusAndNoKeyValueEntryForPostingJournalResultWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsAccountingViewModelWithPostingJournalResultWherePostingWarningsIsEmpty()
        {
            Controller sut = CreateSut(hasKeyValueEntryForPostingJournalResultKey: false);

            PartialViewResult result = (PartialViewResult)await sut.LoadAccounting(_fixture.Create<int>());

            AccountingViewModel accountingViewModel = (AccountingViewModel)result.Model;

            Assert.That(accountingViewModel.PostingJournalResult.PostingWarnings, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadAccounting_WhenAccountingWasReturnedFromQueryBusAndKeyValueEntryForPostingJournalResultWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsAccountingViewModelWithPostingJournalResultEqualToPostingJournalResultFromQueryBus()
        {
            ApplyPostingJournalResultViewModel postingJournalResult = _fixture.Create<ApplyPostingJournalResultViewModel>();
            IKeyValueEntry keyValueEntryForPostingJournalResultKey = _fixture.BuildKeyValueEntryMock(toObject: postingJournalResult).Object;
            Controller sut = CreateSut(keyValueEntryForPostingJournalResultKey: keyValueEntryForPostingJournalResultKey);

            PartialViewResult result = (PartialViewResult)await sut.LoadAccounting(_fixture.Create<int>());

            AccountingViewModel accountingViewModel = (AccountingViewModel)result.Model;

            Assert.That(accountingViewModel.PostingJournalResult, Is.EqualTo(postingJournalResult));
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

        private Controller CreateSut(IEnumerable<ILetterHead> letterHeadCollection = null, bool hasAccounting = true, IAccounting accounting = null, string postingJournalKey = null, bool hasKeyValueEntryForPostingJournalKey = true, IKeyValueEntry keyValueEntryForPostingJournalKey = null, string postingJournalResultKey = null, bool hasKeyValueEntryForPostingJournalResultKey = true, IKeyValueEntry keyValueEntryForPostingJournalResultKey = null)
        {
            postingJournalKey ??= _fixture.Create<string>();
            postingJournalResultKey ??= _fixture.Create<string>();

            _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<ILetterHead>>(It.IsAny<EmptyQuery>()))
                .Returns(Task.FromResult(letterHeadCollection ?? _fixture.CreateMany<ILetterHead>(_random.Next(5, 10)).ToList()));
            _queryBusMock.Setup(m => m.QueryAsync<IGetAccountingQuery, IAccounting>(It.IsAny<IGetAccountingQuery>()))
                .Returns(Task.FromResult(hasAccounting ? accounting ?? _fixture.BuildAccountingMock().Object : accounting));
            _queryBusMock.Setup(m => m.QueryAsync<IGetUserSpecificKeyQuery, string>(It.Is<IGetUserSpecificKeyQuery>(query => query != null && query.KeyElementCollection != null && query.KeyElementCollection.Contains(nameof(ApplyPostingJournalViewModel)))))
                .Returns(Task.FromResult(postingJournalKey));
            _queryBusMock.Setup(m => m.QueryAsync<IPullKeyValueEntryQuery, IKeyValueEntry>(It.Is<IPullKeyValueEntryQuery>(query => query != null && string.CompareOrdinal(query.Key, postingJournalKey) == 0)))
                .Returns(Task.FromResult(hasKeyValueEntryForPostingJournalKey ? keyValueEntryForPostingJournalKey ?? _fixture.BuildKeyValueEntryMock<ApplyPostingJournalViewModel>().Object : null));
            _queryBusMock.Setup(m => m.QueryAsync<IGetUserSpecificKeyQuery, string>(It.Is<IGetUserSpecificKeyQuery>(query => query != null && query.KeyElementCollection != null && query.KeyElementCollection.Contains(nameof(ApplyPostingJournalResultViewModel)))))
                .Returns(Task.FromResult(postingJournalResultKey));
            _queryBusMock.Setup(m => m.QueryAsync<IPullKeyValueEntryQuery, IKeyValueEntry>(It.Is<IPullKeyValueEntryQuery>(query => query != null && string.CompareOrdinal(query.Key, postingJournalResultKey) == 0)))
                .Returns(Task.FromResult(hasKeyValueEntryForPostingJournalResultKey ? keyValueEntryForPostingJournalResultKey ?? _fixture.BuildKeyValueEntryMock<ApplyPostingJournalResultViewModel>().Object : null));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
        }
    }
}