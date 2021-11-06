using System;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Models.Accounting;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.AccountingController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.AccountingController
{
    [TestFixture]
    public class RemovePostingLineFromPostingJournalTests
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
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyIsNull_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), null, Guid.NewGuid());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyIsNull_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), null, Guid.NewGuid());

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyIsEmpty_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), string.Empty, Guid.NewGuid());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyIsEmpty_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), string.Empty, Guid.NewGuid());

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyIsWhiteSpace_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), " ", Guid.NewGuid());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyIsWhiteSpace_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), " ", Guid.NewGuid());

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValue_AssertQueryAsyncWasCalledOnQueryBusWithPullKeyValueEntryQueryForPostingJournalKey()
        {
            Controller sut = CreateSut();

            string postingJournalKey = _fixture.Create<string>();
            await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), postingJournalKey, Guid.NewGuid());

            _queryBusMock.Verify(m => m.QueryAsync<IPullKeyValueEntryQuery, IKeyValueEntry>(It.Is<IPullKeyValueEntryQuery>(query => query != null && string.CompareOrdinal(query.Key, postingJournalKey) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValueAndKeyValueEntryForPostingJournalWasReturnedFromQueryBus_AssertToObjectWasCalledOnKeyValueEntryForPostingJournal()
        {
            Mock<IKeyValueEntry> keyValueEntryForPostingJournalMock = BuildKeyValueEntryForPostingJournalMock();
            Controller sut = CreateSut(keyValueEntryForPostingJournal: keyValueEntryForPostingJournalMock.Object);

            await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            keyValueEntryForPostingJournalMock.Verify(m => m.ToObject<It.IsSubtype<ApplyPostingJournalViewModel>>(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValueAndPostingJournalFromKeyValueEntryDoesNotContainPostingLineForPostingLineIdentifier_AssertPublishAsyncWasCalledOnCommandBusWithPushKeyValueEntryCommandForPostingJournal()
        {
            int accountingNumber = _fixture.Create<int>();
            ApplyPostingLineViewModel applyPostingLineViewModel1 = BuildApplyPostingLineViewModel();
            ApplyPostingLineViewModel applyPostingLineViewModel2 = BuildApplyPostingLineViewModel();
            ApplyPostingLineViewModel applyPostingLineViewModel3 = BuildApplyPostingLineViewModel();
            ApplyPostingLineCollectionViewModel applyPostingLineCollectionViewModel = BuildApplyPostingLineCollectionViewModel(applyPostingLineViewModel1, applyPostingLineViewModel2, applyPostingLineViewModel3);
            ApplyPostingJournalViewModel applyPostingJournalViewModel = BuildApplyPostingJournalViewModel(accountingNumber, applyPostingLineCollectionViewModel);
            IKeyValueEntry keyValueEntryForPostingJournal = BuildKeyValueEntryForPostingJournal(applyPostingJournalViewModel);
            Controller sut = CreateSut(keyValueEntryForPostingJournal: keyValueEntryForPostingJournal);

            string postingJournalKey = _fixture.Create<string>();
            await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), postingJournalKey, Guid.NewGuid());

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IPushKeyValueEntryCommand>(command => command != null && string.CompareOrdinal(command.Key, postingJournalKey) == 0 && command.Value != null && command.Value.GetType() == typeof(ApplyPostingJournalViewModel) && ((ApplyPostingJournalViewModel)command.Value).AccountingNumber == accountingNumber && ((ApplyPostingJournalViewModel)command.Value).ApplyPostingLines != null && ((ApplyPostingJournalViewModel)command.Value).ApplyPostingLines.Count == 3 && ((ApplyPostingJournalViewModel)command.Value).ApplyPostingLines.Contains(applyPostingLineViewModel1) && ((ApplyPostingJournalViewModel)command.Value).ApplyPostingLines.Contains(applyPostingLineViewModel2) && ((ApplyPostingJournalViewModel)command.Value).ApplyPostingLines.Contains(applyPostingLineViewModel3))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValueAndPostingJournalFromKeyValueEntryDoesNotContainPostingLineForPostingLineIdentifier_AssertQueryAsyncWasCalledOnceOnQueryBusWithPullKeyValueEntryQueryForPostingJournalKey()
        {
            ApplyPostingLineViewModel applyPostingLineViewModel1 = BuildApplyPostingLineViewModel();
            ApplyPostingLineViewModel applyPostingLineViewModel2 = BuildApplyPostingLineViewModel();
            ApplyPostingLineViewModel applyPostingLineViewModel3 = BuildApplyPostingLineViewModel();
            ApplyPostingLineCollectionViewModel applyPostingLineCollectionViewModel = BuildApplyPostingLineCollectionViewModel(applyPostingLineViewModel1, applyPostingLineViewModel2, applyPostingLineViewModel3);
            ApplyPostingJournalViewModel applyPostingJournalViewModel = BuildApplyPostingJournalViewModel(applyPostingLineCollectionViewModel: applyPostingLineCollectionViewModel);
            IKeyValueEntry keyValueEntryForPostingJournal = BuildKeyValueEntryForPostingJournal(applyPostingJournalViewModel);
            Controller sut = CreateSut(keyValueEntryForPostingJournal: keyValueEntryForPostingJournal);

            string postingJournalKey = _fixture.Create<string>();
            await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), postingJournalKey, Guid.NewGuid());

            _queryBusMock.Verify(m => m.QueryAsync<IPullKeyValueEntryQuery, IKeyValueEntry>(It.Is<IPullKeyValueEntryQuery>(query => query != null && string.CompareOrdinal(query.Key, postingJournalKey) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValueAndPostingJournalFromKeyValueEntryDoesNotContainPostingLineForPostingLineIdentifier_AssertPublishAsyncWasNotCalledOnCommandBusWithDeleteKeyValueEntryCommandForPostingJournal()
        {
            ApplyPostingLineViewModel applyPostingLineViewModel1 = BuildApplyPostingLineViewModel();
            ApplyPostingLineViewModel applyPostingLineViewModel2 = BuildApplyPostingLineViewModel();
            ApplyPostingLineViewModel applyPostingLineViewModel3 = BuildApplyPostingLineViewModel();
            ApplyPostingLineCollectionViewModel applyPostingLineCollectionViewModel = BuildApplyPostingLineCollectionViewModel(applyPostingLineViewModel1, applyPostingLineViewModel2, applyPostingLineViewModel3);
            ApplyPostingJournalViewModel applyPostingJournalViewModel = BuildApplyPostingJournalViewModel(applyPostingLineCollectionViewModel: applyPostingLineCollectionViewModel);
            IKeyValueEntry keyValueEntryForPostingJournal = BuildKeyValueEntryForPostingJournal(applyPostingJournalViewModel);
            Controller sut = CreateSut(keyValueEntryForPostingJournal: keyValueEntryForPostingJournal);

            await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            _commandBusMock.Verify(m => m.PublishAsync(It.IsAny<IDeleteKeyValueEntryCommand>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValueAndPostingJournalFromKeyValueContainsMultiplePostingLinesWhereOneMatchesPostingLineIdentifier_AssertPublishAsyncWasCalledOnCommandBusWithPushKeyValueEntryCommandForPostingJournal()
        {
            int accountingNumber = _fixture.Create<int>();
            Guid postingLineIdentifier = Guid.NewGuid();
            ApplyPostingLineViewModel applyPostingLineViewModel1 = BuildApplyPostingLineViewModel();
            ApplyPostingLineViewModel applyPostingLineViewModel2 = BuildApplyPostingLineViewModel(postingLineIdentifier);
            ApplyPostingLineViewModel applyPostingLineViewModel3 = BuildApplyPostingLineViewModel();
            ApplyPostingLineCollectionViewModel applyPostingLineCollectionViewModel = BuildApplyPostingLineCollectionViewModel(applyPostingLineViewModel1, applyPostingLineViewModel2, applyPostingLineViewModel3);
            ApplyPostingJournalViewModel applyPostingJournalViewModel = BuildApplyPostingJournalViewModel(accountingNumber, applyPostingLineCollectionViewModel);
            IKeyValueEntry keyValueEntryForPostingJournal = BuildKeyValueEntryForPostingJournal(applyPostingJournalViewModel);
            Controller sut = CreateSut(keyValueEntryForPostingJournal: keyValueEntryForPostingJournal);

            string postingJournalKey = _fixture.Create<string>();
            await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), postingJournalKey, postingLineIdentifier);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IPushKeyValueEntryCommand>(command => command != null && string.CompareOrdinal(command.Key, postingJournalKey) == 0 && command.Value != null && command.Value.GetType() == typeof(ApplyPostingJournalViewModel) && ((ApplyPostingJournalViewModel)command.Value).AccountingNumber == accountingNumber && ((ApplyPostingJournalViewModel)command.Value).ApplyPostingLines != null && ((ApplyPostingJournalViewModel)command.Value).ApplyPostingLines.Count == 2 && ((ApplyPostingJournalViewModel)command.Value).ApplyPostingLines.Contains(applyPostingLineViewModel1) && ((ApplyPostingJournalViewModel)command.Value).ApplyPostingLines.Contains(applyPostingLineViewModel2) == false && ((ApplyPostingJournalViewModel)command.Value).ApplyPostingLines.Contains(applyPostingLineViewModel3))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValueAndPostingJournalFromKeyValueContainsMultiplePostingLinesWhereOneMatchesPostingLineIdentifier_AssertQueryAsyncWasCalledOnceOnQueryBusWithPullKeyValueEntryQueryForPostingJournalKey()
        {
            Guid postingLineIdentifier = Guid.NewGuid();
            ApplyPostingLineViewModel applyPostingLineViewModel1 = BuildApplyPostingLineViewModel();
            ApplyPostingLineViewModel applyPostingLineViewModel2 = BuildApplyPostingLineViewModel(postingLineIdentifier);
            ApplyPostingLineViewModel applyPostingLineViewModel3 = BuildApplyPostingLineViewModel();
            ApplyPostingLineCollectionViewModel applyPostingLineCollectionViewModel = BuildApplyPostingLineCollectionViewModel(applyPostingLineViewModel1, applyPostingLineViewModel2, applyPostingLineViewModel3);
            ApplyPostingJournalViewModel applyPostingJournalViewModel = BuildApplyPostingJournalViewModel(applyPostingLineCollectionViewModel: applyPostingLineCollectionViewModel);
            IKeyValueEntry keyValueEntryForPostingJournal = BuildKeyValueEntryForPostingJournal(applyPostingJournalViewModel);
            Controller sut = CreateSut(keyValueEntryForPostingJournal: keyValueEntryForPostingJournal);

            string postingJournalKey = _fixture.Create<string>();
            await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), postingJournalKey, postingLineIdentifier);

            _queryBusMock.Verify(m => m.QueryAsync<IPullKeyValueEntryQuery, IKeyValueEntry>(It.Is<IPullKeyValueEntryQuery>(query => query != null && string.CompareOrdinal(query.Key, postingJournalKey) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValueAndPostingJournalFromKeyValueContainsMultiplePostingLinesWhereOneMatchesPostingLineIdentifier_AssertPublishAsyncWasNotCalledOnCommandBusWithDeleteKeyValueEntryCommandForPostingJournal()
        {
            Guid postingLineIdentifier = Guid.NewGuid();
            ApplyPostingLineViewModel applyPostingLineViewModel1 = BuildApplyPostingLineViewModel();
            ApplyPostingLineViewModel applyPostingLineViewModel2 = BuildApplyPostingLineViewModel(postingLineIdentifier);
            ApplyPostingLineViewModel applyPostingLineViewModel3 = BuildApplyPostingLineViewModel();
            ApplyPostingLineCollectionViewModel applyPostingLineCollectionViewModel = BuildApplyPostingLineCollectionViewModel(applyPostingLineViewModel1, applyPostingLineViewModel2, applyPostingLineViewModel3);
            ApplyPostingJournalViewModel applyPostingJournalViewModel = BuildApplyPostingJournalViewModel(applyPostingLineCollectionViewModel: applyPostingLineCollectionViewModel);
            IKeyValueEntry keyValueEntryForPostingJournal = BuildKeyValueEntryForPostingJournal(applyPostingJournalViewModel);
            Controller sut = CreateSut(keyValueEntryForPostingJournal: keyValueEntryForPostingJournal);

            await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), postingLineIdentifier);

            _commandBusMock.Verify(m => m.PublishAsync(It.IsAny<IDeleteKeyValueEntryCommand>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValueAndPostingJournalFromKeyValueContainsOnePostingLinesMatchingPostingLineIdentifier_AssertPublishAsyncWasNotCalledOnCommandBusWithPushKeyValueEntryCommandForPostingJournal()
        {
            Guid postingLineIdentifier = Guid.NewGuid();
            ApplyPostingLineViewModel applyPostingLineViewModel = BuildApplyPostingLineViewModel(postingLineIdentifier);
            ApplyPostingLineCollectionViewModel applyPostingLineCollectionViewModel = BuildApplyPostingLineCollectionViewModel(applyPostingLineViewModel);
            ApplyPostingJournalViewModel applyPostingJournalViewModel = BuildApplyPostingJournalViewModel(applyPostingLineCollectionViewModel: applyPostingLineCollectionViewModel);
            IKeyValueEntry keyValueEntryForPostingJournal = BuildKeyValueEntryForPostingJournal(applyPostingJournalViewModel);
            Controller sut = CreateSut(keyValueEntryForPostingJournal: keyValueEntryForPostingJournal);

            string postingJournalKey = _fixture.Create<string>();
            await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), postingJournalKey, postingLineIdentifier);

            _commandBusMock.Verify(m => m.PublishAsync(It.IsAny<IPushKeyValueEntryCommand>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValueAndPostingJournalFromKeyValueContainsOnePostingLinesMatchingPostingLineIdentifier_AssertQueryAsyncWasCalledTwiceOnQueryBusWithPullKeyValueEntryQueryForPostingJournalKey()
        {
            Guid postingLineIdentifier = Guid.NewGuid();
            ApplyPostingLineViewModel applyPostingLineViewModel = BuildApplyPostingLineViewModel(postingLineIdentifier);
            ApplyPostingLineCollectionViewModel applyPostingLineCollectionViewModel = BuildApplyPostingLineCollectionViewModel(applyPostingLineViewModel);
            ApplyPostingJournalViewModel applyPostingJournalViewModel = BuildApplyPostingJournalViewModel(applyPostingLineCollectionViewModel: applyPostingLineCollectionViewModel);
            IKeyValueEntry keyValueEntryForPostingJournal = BuildKeyValueEntryForPostingJournal(applyPostingJournalViewModel);
            Controller sut = CreateSut(keyValueEntryForPostingJournal: keyValueEntryForPostingJournal);

            string postingJournalKey = _fixture.Create<string>();
            await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), postingJournalKey, postingLineIdentifier);

            _queryBusMock.Verify(m => m.QueryAsync<IPullKeyValueEntryQuery, IKeyValueEntry>(It.Is<IPullKeyValueEntryQuery>(query => query != null && string.CompareOrdinal(query.Key, postingJournalKey) == 0)), Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValueAndPostingJournalFromKeyValueContainsOnePostingLinesMatchingPostingLineIdentifier_AssertPublishAsyncWasCalledOnCommandBusWithDeleteKeyValueEntryCommandForPostingJournal()
        {
            Guid postingLineIdentifier = Guid.NewGuid();
            ApplyPostingLineViewModel applyPostingLineViewModel = BuildApplyPostingLineViewModel(postingLineIdentifier);
            ApplyPostingLineCollectionViewModel applyPostingLineCollectionViewModel = BuildApplyPostingLineCollectionViewModel(applyPostingLineViewModel);
            ApplyPostingJournalViewModel applyPostingJournalViewModel = BuildApplyPostingJournalViewModel(applyPostingLineCollectionViewModel: applyPostingLineCollectionViewModel);
            IKeyValueEntry keyValueEntryForPostingJournal = BuildKeyValueEntryForPostingJournal(applyPostingJournalViewModel);
            Controller sut = CreateSut(keyValueEntryForPostingJournal: keyValueEntryForPostingJournal);

            string postingJournalKey = _fixture.Create<string>();
            await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), postingJournalKey, postingLineIdentifier);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IDeleteKeyValueEntryCommand>(command => command != null && string.CompareOrdinal(command.Key, postingJournalKey) == 0)), Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValueAndNoKeyValueEntryForPostingJournalWasReturnedFromQueryBus_AssertPublishAsyncWasNotCalledOnCommandBusWithPushKeyValueEntryCommandForPostingJournal()
        {
            Controller sut = CreateSut(false);

            await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            _commandBusMock.Verify(m => m.PublishAsync(It.IsAny<IPushKeyValueEntryCommand>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValueAndNoKeyValueEntryForPostingJournalWasReturnedFromQueryBus_AssertQueryAsyncWasCalledTwiceOnQueryBusWithPullKeyValueEntryQueryForPostingJournalKey()
        {
            Controller sut = CreateSut(false);

            string postingJournalKey = _fixture.Create<string>();
            await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), postingJournalKey, Guid.NewGuid());

            _queryBusMock.Verify(m => m.QueryAsync<IPullKeyValueEntryQuery, IKeyValueEntry>(It.Is<IPullKeyValueEntryQuery>(query => query != null && string.CompareOrdinal(query.Key, postingJournalKey) == 0)), Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValueAndNoKeyValueEntryForPostingJournalWasReturnedFromQueryBus_AssertPublishAsyncWasNotCalledOnCommandBusWithDeleteKeyValueEntryCommandForPostingJournal()
        {
            Controller sut = CreateSut(false);

            await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            _commandBusMock.Verify(m => m.PublishAsync(It.IsAny<IDeleteKeyValueEntryCommand>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValue_ReturnsNotNull()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            IActionResult result = await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValue_ReturnsPartialViewResult()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            IActionResult result = await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            Assert.That(result, Is.TypeOf<PartialViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValue_ReturnsPartialViewResultWhereViewNameIsNotNull()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            PartialViewResult result = (PartialViewResult)await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            Assert.That(result.ViewName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValue_ReturnsPartialViewResultWhereViewNameIsEqualToPostingJournalPartial()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            PartialViewResult result = (PartialViewResult)await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            Assert.That(result.ViewName, Is.EqualTo("_PostingJournalPartial"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValue_ReturnsPartialViewResultWhereModelIsNotNull()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            PartialViewResult result = (PartialViewResult)await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            Assert.That(result.Model, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValue_ReturnsPartialViewResultWhereModelIsApplyPostingJournalViewModel()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            PartialViewResult result = (PartialViewResult)await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            Assert.That(result.Model, Is.TypeOf<ApplyPostingJournalViewModel>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValueAndKeyValueEntryForPostingJournalWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsApplyPostingJournalViewModelWithAccountingNumberEqualToAccountingNumberFromReturnedPostingJournal()
        {
            int accountingNumber = _fixture.Create<int>();
            ApplyPostingJournalViewModel savedApplyPostingJournalViewModel = BuildApplyPostingJournalViewModel(accountingNumber);
            IKeyValueEntry keyValueEntryForPostingJournal = BuildKeyValueEntryForPostingJournal(savedApplyPostingJournalViewModel);
            Controller sut = CreateSut(keyValueEntryForPostingJournal: keyValueEntryForPostingJournal);

            PartialViewResult result = (PartialViewResult)await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            ApplyPostingJournalViewModel applyPostingJournalViewModel = (ApplyPostingJournalViewModel)result.Model;

            Assert.That(applyPostingJournalViewModel.AccountingNumber, Is.EqualTo(accountingNumber));
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValueAndKeyValueEntryForPostingJournalWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsApplyPostingJournalViewModelWithApplyPostingLinesNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult)await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            ApplyPostingJournalViewModel applyPostingJournalViewModel = (ApplyPostingJournalViewModel)result.Model;

            Assert.That(applyPostingJournalViewModel.ApplyPostingLines, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValueAndPostingJournalFromKeyValueEntryDoesNotContainPostingLineForPostingLineIdentifier_ReturnsPartialViewResultWhereModelIsApplyPostingJournalViewModelWithApplyPostingLinesFromReturnedPostingJournal()
        {
            ApplyPostingLineViewModel postingLine1 = BuildApplyPostingLineViewModel();
            ApplyPostingLineViewModel postingLine2 = BuildApplyPostingLineViewModel();
            ApplyPostingLineViewModel postingLine3 = BuildApplyPostingLineViewModel();
            ApplyPostingLineCollectionViewModel postingLineCollection = BuildApplyPostingLineCollectionViewModel(postingLine1, postingLine2, postingLine3);
            ApplyPostingJournalViewModel postingJournal = BuildApplyPostingJournalViewModel(applyPostingLineCollectionViewModel: postingLineCollection);
            IKeyValueEntry keyValueEntryForPostingJournal = BuildKeyValueEntryForPostingJournal(postingJournal);
            Controller sut = CreateSut(keyValueEntryForPostingJournal: keyValueEntryForPostingJournal);

            PartialViewResult result = (PartialViewResult)await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            ApplyPostingJournalViewModel applyPostingJournalViewModel = (ApplyPostingJournalViewModel)result.Model;

            Assert.That(applyPostingJournalViewModel.ApplyPostingLines, Is.Not.Empty);
            Assert.That(applyPostingJournalViewModel.ApplyPostingLines.Contains(postingLine1), Is.True);
            Assert.That(applyPostingJournalViewModel.ApplyPostingLines.Contains(postingLine2), Is.True);
            Assert.That(applyPostingJournalViewModel.ApplyPostingLines.Contains(postingLine3), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValueAndPostingJournalFromKeyValueContainsMultiplePostingLinesWhereOneMatchesPostingLineIdentifier_ReturnsPartialViewResultWhereModelIsApplyPostingJournalViewModelWithApplyPostingLinesFromReturnedPostingJournalExceptPostingLineForPostingLineIdentifier()
        {
            Guid postingLineIdentifier = Guid.NewGuid();
            ApplyPostingLineViewModel postingLine1 = BuildApplyPostingLineViewModel();
            ApplyPostingLineViewModel postingLine2 = BuildApplyPostingLineViewModel(postingLineIdentifier);
            ApplyPostingLineViewModel postingLine3 = BuildApplyPostingLineViewModel();
            ApplyPostingLineCollectionViewModel postingLineCollection = BuildApplyPostingLineCollectionViewModel(postingLine1, postingLine2, postingLine3);
            ApplyPostingJournalViewModel postingJournal = BuildApplyPostingJournalViewModel(applyPostingLineCollectionViewModel: postingLineCollection);
            IKeyValueEntry keyValueEntryForPostingJournal = BuildKeyValueEntryForPostingJournal(postingJournal);
            Controller sut = CreateSut(keyValueEntryForPostingJournal: keyValueEntryForPostingJournal);

            PartialViewResult result = (PartialViewResult)await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), postingLineIdentifier);

            ApplyPostingJournalViewModel applyPostingJournalViewModel = (ApplyPostingJournalViewModel)result.Model;

            Assert.That(applyPostingJournalViewModel.ApplyPostingLines, Is.Not.Empty);
            Assert.That(applyPostingJournalViewModel.ApplyPostingLines.Contains(postingLine1), Is.True);
            Assert.That(applyPostingJournalViewModel.ApplyPostingLines.Contains(postingLine2), Is.False);
            Assert.That(applyPostingJournalViewModel.ApplyPostingLines.Contains(postingLine3), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValueAndPostingJournalFromKeyValueContainsOnePostingLinesMatchingPostingLineIdentifier_ReturnsPartialViewResultWhereModelIsApplyPostingJournalViewModelWithEmptyApplyPostingLines()
        {
            Guid postingLineIdentifier = Guid.NewGuid();
            ApplyPostingLineViewModel postingLine = BuildApplyPostingLineViewModel(postingLineIdentifier);
            ApplyPostingLineCollectionViewModel postingLineCollection = BuildApplyPostingLineCollectionViewModel(postingLine);
            ApplyPostingJournalViewModel postingJournal = BuildApplyPostingJournalViewModel(applyPostingLineCollectionViewModel: postingLineCollection);
            IKeyValueEntry keyValueEntryForPostingJournal = BuildKeyValueEntryForPostingJournal(postingJournal);
            Controller sut = CreateSut(keyValueEntryForPostingJournal: keyValueEntryForPostingJournal);

            PartialViewResult result = (PartialViewResult)await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), postingLineIdentifier);

            ApplyPostingJournalViewModel applyPostingJournalViewModel = (ApplyPostingJournalViewModel)result.Model;

            Assert.That(applyPostingJournalViewModel.ApplyPostingLines, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValueAndKeyValueEntryForPostingJournalWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsApplyPostingJournalViewModelWithSortedApplyPostingLines()
        {
            ApplyPostingLineCollectionViewModel postingLineCollection = BuildApplyPostingLineCollectionViewModel(
                BuildApplyPostingLineViewModel(),
                BuildApplyPostingLineViewModel(),
                BuildApplyPostingLineViewModel(),
                BuildApplyPostingLineViewModel(),
                BuildApplyPostingLineViewModel(),
                BuildApplyPostingLineViewModel());
            ApplyPostingJournalViewModel postingJournal = BuildApplyPostingJournalViewModel(applyPostingLineCollectionViewModel: postingLineCollection);
            IKeyValueEntry keyValueEntryForPostingJournal = BuildKeyValueEntryForPostingJournal(postingJournal);
            Controller sut = CreateSut(keyValueEntryForPostingJournal: keyValueEntryForPostingJournal);

            PartialViewResult result = (PartialViewResult)await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            ApplyPostingJournalViewModel applyPostingJournalViewModel = (ApplyPostingJournalViewModel)result.Model;

            for (int i = 1; i < applyPostingJournalViewModel.ApplyPostingLines.Count; i++)
            {
                Assert.That(applyPostingJournalViewModel.ApplyPostingLines[i].PostingDate.UtcDateTime.Date, Is.LessThanOrEqualTo(applyPostingJournalViewModel.ApplyPostingLines[i - 1].PostingDate.UtcDateTime.Date));
                if (applyPostingJournalViewModel.ApplyPostingLines[i].PostingDate.UtcDateTime.Date != applyPostingJournalViewModel.ApplyPostingLines[i - 1].PostingDate.UtcDateTime.Date)
                {
                    continue;
                }

                Assert.That(applyPostingJournalViewModel.ApplyPostingLines[i].SortOrder ?? 0, Is.LessThanOrEqualTo(applyPostingJournalViewModel.ApplyPostingLines[i - 1].SortOrder ?? 0));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValueAndNoKeyValueEntryForPostingJournalWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsApplyPostingJournalViewModelWithAccountingNumberEqualToAccountingNumberFromArguments()
        {
            Controller sut = CreateSut(false);

            int accountingNumber = _fixture.Create<int>();
            PartialViewResult result = (PartialViewResult)await sut.RemovePostingLineFromPostingJournal(accountingNumber, _fixture.Create<string>(), Guid.NewGuid());

            ApplyPostingJournalViewModel applyPostingJournalViewModel = (ApplyPostingJournalViewModel)result.Model;

            Assert.That(applyPostingJournalViewModel.AccountingNumber, Is.EqualTo(accountingNumber));
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValueAndNoKeyValueEntryForPostingJournalWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsApplyPostingJournalViewModelWithApplyPostingLinesNotEqualToNull()
        {
            Controller sut = CreateSut(false);

            PartialViewResult result = (PartialViewResult)await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            ApplyPostingJournalViewModel applyPostingJournalViewModel = (ApplyPostingJournalViewModel)result.Model;

            Assert.That(applyPostingJournalViewModel.ApplyPostingLines, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValueAndNoKeyValueEntryForPostingJournalWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsApplyPostingJournalViewModelWithEmptyApplyPostingLines()
        {
            Controller sut = CreateSut(false);

            PartialViewResult result = (PartialViewResult)await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            ApplyPostingJournalViewModel applyPostingJournalViewModel = (ApplyPostingJournalViewModel)result.Model;

            Assert.That(applyPostingJournalViewModel.ApplyPostingLines, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValue_ReturnsPartialViewResultWhereViewDataIsNotEqualToNull()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            PartialViewResult result = (PartialViewResult)await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            Assert.That(result.ViewData, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValue_ReturnsPartialViewResultWhereViewDataIsNotEmpty()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            PartialViewResult result = (PartialViewResult)await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            Assert.That(result.ViewData, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValue_ReturnsPartialViewResultWhereViewDataContainingKeyForPostingJournalKey()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            PartialViewResult result = (PartialViewResult)await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            Assert.That(result.ViewData.ContainsKey("PostingJournalKey"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValue_ReturnsPartialViewResultWhereViewDataContainingPostingJournalKeyWithValueNotEqualToNull()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            PartialViewResult result = (PartialViewResult)await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            Assert.That(result.ViewData["PostingJournalKey"], Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValue_ReturnsPartialViewResultWhereViewDataContainingPostingJournalKeyWithValueEqualToPostingJournalKeyFromArguments()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            string postingJournalKey = _fixture.Create<string>();
            PartialViewResult result = (PartialViewResult)await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), postingJournalKey, Guid.NewGuid());

            Assert.That(result.ViewData["PostingJournalKey"], Is.EqualTo(postingJournalKey));
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValueAndPostingJournalHeaderIsNull_ReturnsPartialViewResultWhereWithViewDataDoesNotContainKeyForHeader()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            PartialViewResult result = (PartialViewResult)await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            Assert.That(result.ViewData.ContainsKey("Header"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValueAndPostingJournalHeaderIsEmpty_ReturnsPartialViewResultWhereWithViewDataDoesNotContainKeyForHeader()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            PartialViewResult result = (PartialViewResult)await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid(), string.Empty);

            Assert.That(result.ViewData.ContainsKey("Header"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValueAndPostingJournalHeaderIsWhiteSpace_ReturnsPartialViewResultWhereWithViewDataDoesNotContainKeyForHeader()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            PartialViewResult result = (PartialViewResult)await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid(), " ");

            Assert.That(result.ViewData.ContainsKey("Header"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValueAndPostingJournalHeaderIsNotNullEmptyOrWhiteSpace_ReturnsPartialViewResultWhereWithViewContainingKeyForHeader()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            PartialViewResult result = (PartialViewResult)await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid(), _fixture.Create<string>());

            Assert.That(result.ViewData.ContainsKey("Header"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValueAndPostingJournalHeaderIsNotNullEmptyOrWhiteSpace_ReturnsPartialViewResultWhereWithViewContainingHeaderWithValueNotEqualToNull()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            PartialViewResult result = (PartialViewResult)await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid(), _fixture.Create<string>());

            Assert.That(result.ViewData["Header"], Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingLineFromPostingJournal_WhenPostingJournalKeyHasValueAndPostingJournalHeaderIsNotNullEmptyOrWhiteSpace_ReturnsPartialViewResultWhereWithViewContainingHeaderWithValueEqualToPostingJournalHeaderFromArguments()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            string postingJournalHeader = _fixture.Create<string>();
            PartialViewResult result = (PartialViewResult)await sut.RemovePostingLineFromPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid(), postingJournalHeader);

            Assert.That(result.ViewData["Header"], Is.EqualTo(postingJournalHeader));
        }

        private Controller CreateSut(bool hasKeyValueEntryForPostingJournal = true, IKeyValueEntry keyValueEntryForPostingJournal = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<IPullKeyValueEntryQuery, IKeyValueEntry>(It.IsAny<IPullKeyValueEntryQuery>()))
                .Returns(Task.FromResult(hasKeyValueEntryForPostingJournal ? keyValueEntryForPostingJournal ?? BuildKeyValueEntryForPostingJournal() : null));
            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<IPushKeyValueEntryCommand>()))
                .Returns(Task.CompletedTask);
            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<IDeleteKeyValueEntryCommand>()))
                .Returns(Task.CompletedTask);

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
        }

        private IKeyValueEntry BuildKeyValueEntryForPostingJournal(ApplyPostingJournalViewModel applyPostingJournalViewModel = null)
        {
            return BuildKeyValueEntryForPostingJournalMock(applyPostingJournalViewModel).Object;
        }

        private Mock<IKeyValueEntry> BuildKeyValueEntryForPostingJournalMock(ApplyPostingJournalViewModel applyPostingJournalViewModel = null)
        {
            return _fixture.BuildKeyValueEntryMock(toObject: applyPostingJournalViewModel ?? BuildApplyPostingJournalViewModel());
        }

        private ApplyPostingJournalViewModel BuildApplyPostingJournalViewModel(int? accountingNumber = null, ApplyPostingLineCollectionViewModel applyPostingLineCollectionViewModel = null)
        {
            return _fixture.Build<ApplyPostingJournalViewModel>()
                .With(m => m.AccountingNumber, accountingNumber ?? _fixture.Create<int>())
                .With(m => m.ApplyPostingLines, applyPostingLineCollectionViewModel ?? BuildApplyPostingLineCollectionViewModel(BuildApplyPostingLineViewModel(), BuildApplyPostingLineViewModel(), BuildApplyPostingLineViewModel()))
                .Create();
        }

        private ApplyPostingLineCollectionViewModel BuildApplyPostingLineCollectionViewModel(params ApplyPostingLineViewModel[] applyPostingLineViewModelCollection)
        {
            NullGuard.NotNull(applyPostingLineViewModelCollection, nameof(applyPostingLineViewModelCollection));

            ApplyPostingLineCollectionViewModel applyPostingLineCollectionViewModel = new ApplyPostingLineCollectionViewModel();
            applyPostingLineCollectionViewModel.AddRange(applyPostingLineViewModelCollection);

            return applyPostingLineCollectionViewModel;
        }

        private ApplyPostingLineViewModel BuildApplyPostingLineViewModel(Guid? identifier = null)
        {
            return _fixture.Build<ApplyPostingLineViewModel>()
                .With(m => m.Identifier, identifier ?? Guid.NewGuid())
                .Create();
        }
    }
}