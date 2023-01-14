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
using System;
using System.Linq;
using System.Threading.Tasks;
using Controller = OSDevGrp.OSIntranet.Mvc.Controllers.AccountingController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.AccountingController
{
    [TestFixture]
    public class RemovePostingWarningFromPostingJournalResultTests
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
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyIsNull_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.RemovePostingWarningFromPostingJournalResult(_fixture.Create<int>(), null, Guid.NewGuid());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyIsNull_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.RemovePostingWarningFromPostingJournalResult(_fixture.Create<int>(), null, Guid.NewGuid());

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyIsEmpty_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.RemovePostingWarningFromPostingJournalResult(_fixture.Create<int>(), string.Empty, Guid.NewGuid());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyIsEmpty_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.RemovePostingWarningFromPostingJournalResult(_fixture.Create<int>(), string.Empty, Guid.NewGuid());

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyIsWhiteSpace_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.RemovePostingWarningFromPostingJournalResult(_fixture.Create<int>(), " ", Guid.NewGuid());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyIsWhiteSpace_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.RemovePostingWarningFromPostingJournalResult(_fixture.Create<int>(), " ", Guid.NewGuid());

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyHasValue_AssertCanModifyAccountingWasCalledOnClaimResolver()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            await sut.RemovePostingWarningFromPostingJournalResult(accountingNumber, _fixture.Create<string>(), Guid.NewGuid());

            _claimResolverMock.Verify(m => m.CanModifyAccounting(It.Is<int>(value => value == accountingNumber)), Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenCanModifyAccountingReturnsFalse_ReturnsNotNull()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.RemovePostingWarningFromPostingJournalResult(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenCanModifyAccountingReturnsFalse_ReturnsForbidResult()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.RemovePostingWarningFromPostingJournalResult(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            Assert.That(result, Is.TypeOf<ForbidResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyHasValue_AssertQueryAsyncWasCalledOnQueryBusWithPullKeyValueEntryQueryForPostingJournalResultKey()
        {
            Controller sut = CreateSut();

            string postingJournalResultKey = _fixture.Create<string>();
            await sut.RemovePostingWarningFromPostingJournalResult(_fixture.Create<int>(), postingJournalResultKey, Guid.NewGuid());

            _queryBusMock.Verify(m => m.QueryAsync<IPullKeyValueEntryQuery, IKeyValueEntry>(It.Is<IPullKeyValueEntryQuery>(query => query != null && string.CompareOrdinal(query.Key, postingJournalResultKey) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyHasValueAndKeyValueEntryForPostingJournalResultWasReturnedFromQueryBus_AssertToObjectWasCalledOnKeyValueEntryForPostingJournalResult()
        {
            Mock<IKeyValueEntry> keyValueEntryForPostingJournalResultMock = BuildKeyValueEntryForPostingJournalResultMock();
            Controller sut = CreateSut(keyValueEntryForPostingJournalResult: keyValueEntryForPostingJournalResultMock.Object);

            await sut.RemovePostingWarningFromPostingJournalResult(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            keyValueEntryForPostingJournalResultMock.Verify(m => m.ToObject<It.IsSubtype<ApplyPostingJournalResultViewModel>>(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyHasValueAndPostingJournalResultFromKeyValueEntryDoesNotContainPostingWarningForPostingWarningIdentifier_AssertPublishAsyncWasCalledOnCommandBusWithPushKeyValueEntryCommandForPostingJournalResult()
        {
            Guid[] postingWarningIdentifierCollection = BuildPostingWarningIdentifierCollection();
            PostingWarningCollectionViewModel postingWarningCollectionViewModel = BuildPostingWarningCollectionViewModel(postingWarningIdentifierCollection);
            ApplyPostingJournalResultViewModel applyPostingJournalResultViewModel = BuildApplyPostingJournalResultViewModel(postingWarningCollectionViewModel);
            IKeyValueEntry keyValueEntryForPostingJournalResult = BuildKeyValueEntryForPostingJournalResult(applyPostingJournalResultViewModel);
            Controller sut = CreateSut(keyValueEntryForPostingJournalResult: keyValueEntryForPostingJournalResult);

            string postingJournalResultKey = _fixture.Create<string>();
            await sut.RemovePostingWarningFromPostingJournalResult(_fixture.Create<int>(), postingJournalResultKey, Guid.NewGuid());

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IPushKeyValueEntryCommand>(command => command != null && string.CompareOrdinal(command.Key, postingJournalResultKey) == 0 && command.Value != null && command.Value.GetType() == typeof(ApplyPostingJournalResultViewModel) && ((ApplyPostingJournalResultViewModel)command.Value).PostingLines != null && ((ApplyPostingJournalResultViewModel)command.Value).PostingLines.Count == 0 && ((ApplyPostingJournalResultViewModel)command.Value).PostingWarnings != null && ((ApplyPostingJournalResultViewModel)command.Value).PostingWarnings.Count == postingWarningIdentifierCollection.Length && ((ApplyPostingJournalResultViewModel)command.Value).PostingWarnings.All(postingWarningViewModel => postingWarningIdentifierCollection.Any(postingWarningIdentifier => postingWarningViewModel.Identifier == postingWarningIdentifier)))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyHasValueAndPostingJournalResultFromKeyValueEntryDoesNotContainPostingWarningForPostingWarningIdentifier_AssertQueryAsyncWasCalledOnceOnQueryBusWithPullKeyValueEntryQueryForPostingJournalResultKey()
        {
            PostingWarningCollectionViewModel postingWarningCollectionViewModel = BuildPostingWarningCollectionViewModel(BuildPostingWarningIdentifierCollection());
            ApplyPostingJournalResultViewModel applyPostingJournalResultViewModel = BuildApplyPostingJournalResultViewModel(postingWarningCollectionViewModel);
            IKeyValueEntry keyValueEntryForPostingJournalResult = BuildKeyValueEntryForPostingJournalResult(applyPostingJournalResultViewModel);
            Controller sut = CreateSut(keyValueEntryForPostingJournalResult: keyValueEntryForPostingJournalResult);

            string postingJournalResultKey = _fixture.Create<string>();
            await sut.RemovePostingWarningFromPostingJournalResult(_fixture.Create<int>(), postingJournalResultKey, Guid.NewGuid());

            _queryBusMock.Verify(m => m.QueryAsync<IPullKeyValueEntryQuery, IKeyValueEntry>(It.Is<IPullKeyValueEntryQuery>(query => query != null && string.CompareOrdinal(query.Key, postingJournalResultKey) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyHasValueAndPostingJournalResultFromKeyValueEntryDoesNotContainPostingWarningForPostingWarningIdentifier_AssertPublishAsyncWasNotCalledOnCommandBusWithDeleteKeyValueEntryCommandForPostingJournalResult()
        {
            PostingWarningCollectionViewModel postingWarningCollectionViewModel = BuildPostingWarningCollectionViewModel(BuildPostingWarningIdentifierCollection());
            ApplyPostingJournalResultViewModel applyPostingJournalResultViewModel = BuildApplyPostingJournalResultViewModel(postingWarningCollectionViewModel);
            IKeyValueEntry keyValueEntryForPostingJournalResult = BuildKeyValueEntryForPostingJournalResult(applyPostingJournalResultViewModel);
            Controller sut = CreateSut(keyValueEntryForPostingJournalResult: keyValueEntryForPostingJournalResult);

            await sut.RemovePostingWarningFromPostingJournalResult(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            _commandBusMock.Verify(m => m.PublishAsync(It.IsAny<IDeleteKeyValueEntryCommand>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyHasValueAndPostingJournalResultFromKeyValueEntryContainsMultiplePostingWarningsWhereOneMatchesPostingWarningIdentifier_AssertPublishAsyncWasCalledOnCommandBusWithPushKeyValueEntryCommandForPostingJournalResult()
        {
            Guid[] postingWarningIdentifierCollection = BuildPostingWarningIdentifierCollection();
            PostingWarningCollectionViewModel postingWarningCollectionViewModel = BuildPostingWarningCollectionViewModel(postingWarningIdentifierCollection);
            ApplyPostingJournalResultViewModel applyPostingJournalResultViewModel = BuildApplyPostingJournalResultViewModel(postingWarningCollectionViewModel);
            IKeyValueEntry keyValueEntryForPostingJournalResult = BuildKeyValueEntryForPostingJournalResult(applyPostingJournalResultViewModel);
            Controller sut = CreateSut(keyValueEntryForPostingJournalResult: keyValueEntryForPostingJournalResult);

            string postingJournalResultKey = _fixture.Create<string>();
            Guid postingWarningIdentifier = postingWarningIdentifierCollection[_random.Next(0, postingWarningIdentifierCollection.Length - 1)];
            await sut.RemovePostingWarningFromPostingJournalResult(_fixture.Create<int>(), postingJournalResultKey, postingWarningIdentifier);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IPushKeyValueEntryCommand>(command => command != null && string.CompareOrdinal(command.Key, postingJournalResultKey) == 0 && command.Value != null && command.Value.GetType() == typeof(ApplyPostingJournalResultViewModel) && ((ApplyPostingJournalResultViewModel)command.Value).PostingLines != null && ((ApplyPostingJournalResultViewModel)command.Value).PostingLines.Count == 0 && ((ApplyPostingJournalResultViewModel)command.Value).PostingWarnings != null && ((ApplyPostingJournalResultViewModel)command.Value).PostingWarnings.Count == postingWarningIdentifierCollection.Length - 1 && ((ApplyPostingJournalResultViewModel)command.Value).PostingWarnings.All(postingWarningViewModel => postingWarningIdentifierCollection.Where(identifier => identifier != postingWarningIdentifier).Any(identifier => postingWarningViewModel.Identifier == identifier)))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyHasValueAndPostingJournalResultFromKeyValueEntryContainsMultiplePostingWarningsWhereOneMatchesPostingWarningIdentifier_AssertQueryAsyncWasCalledOnceOnQueryBusWithPullKeyValueEntryQueryForPostingJournalResultKey()
        {
            Guid[] postingWarningIdentifierCollection = BuildPostingWarningIdentifierCollection();
            PostingWarningCollectionViewModel postingWarningCollectionViewModel = BuildPostingWarningCollectionViewModel(postingWarningIdentifierCollection);
            ApplyPostingJournalResultViewModel applyPostingJournalResultViewModel = BuildApplyPostingJournalResultViewModel(postingWarningCollectionViewModel);
            IKeyValueEntry keyValueEntryForPostingJournalResult = BuildKeyValueEntryForPostingJournalResult(applyPostingJournalResultViewModel);
            Controller sut = CreateSut(keyValueEntryForPostingJournalResult: keyValueEntryForPostingJournalResult);

            string postingJournalResultKey = _fixture.Create<string>();
            Guid postingWarningIdentifier = postingWarningIdentifierCollection[_random.Next(0, postingWarningIdentifierCollection.Length - 1)];
            await sut.RemovePostingWarningFromPostingJournalResult(_fixture.Create<int>(), postingJournalResultKey, postingWarningIdentifier);

            _queryBusMock.Verify(m => m.QueryAsync<IPullKeyValueEntryQuery, IKeyValueEntry>(It.Is<IPullKeyValueEntryQuery>(query => query != null && string.CompareOrdinal(query.Key, postingJournalResultKey) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyHasValueAndPostingJournalResultFromKeyValueEntryContainsMultiplePostingWarningsWhereOneMatchesPostingWarningIdentifier_AssertPublishAsyncWasNotCalledOnCommandBusWithDeleteKeyValueEntryCommandForPostingJournalResult()
        {
            Guid[] postingWarningIdentifierCollection = BuildPostingWarningIdentifierCollection();
            PostingWarningCollectionViewModel postingWarningCollectionViewModel = BuildPostingWarningCollectionViewModel(postingWarningIdentifierCollection);
            ApplyPostingJournalResultViewModel applyPostingJournalResultViewModel = BuildApplyPostingJournalResultViewModel(postingWarningCollectionViewModel);
            IKeyValueEntry keyValueEntryForPostingJournalResult = BuildKeyValueEntryForPostingJournalResult(applyPostingJournalResultViewModel);
            Controller sut = CreateSut(keyValueEntryForPostingJournalResult: keyValueEntryForPostingJournalResult);

            Guid postingWarningIdentifier = postingWarningIdentifierCollection[_random.Next(0, postingWarningIdentifierCollection.Length - 1)];
            await sut.RemovePostingWarningFromPostingJournalResult(_fixture.Create<int>(), _fixture.Create<string>(), postingWarningIdentifier);

            _commandBusMock.Verify(m => m.PublishAsync(It.IsAny<IDeleteKeyValueEntryCommand>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyHasValueAndPostingJournalResultFromKeyValueEntryContainsOnePostingWarningMatchingPostingWarningIdentifier_AssertPublishAsyncWasNotCalledOnCommandBusWithPushKeyValueEntryCommandForPostingJournalResult()
        {
            Guid postingWarningIdentifier = Guid.NewGuid();
            PostingWarningCollectionViewModel postingWarningCollectionViewModel = BuildPostingWarningCollectionViewModel(postingWarningIdentifier);
            ApplyPostingJournalResultViewModel applyPostingJournalResultViewModel = BuildApplyPostingJournalResultViewModel(postingWarningCollectionViewModel);
            IKeyValueEntry keyValueEntryForPostingJournalResult = BuildKeyValueEntryForPostingJournalResult(applyPostingJournalResultViewModel);
            Controller sut = CreateSut(keyValueEntryForPostingJournalResult: keyValueEntryForPostingJournalResult);

            await sut.RemovePostingWarningFromPostingJournalResult(_fixture.Create<int>(), _fixture.Create<string>(), postingWarningIdentifier);

            _commandBusMock.Verify(m => m.PublishAsync(It.IsAny<IPushKeyValueEntryCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyHasValueAndPostingJournalResultFromKeyValueEntryContainsOnePostingWarningMatchingPostingWarningIdentifier_AssertQueryAsyncWasCalledTwiceOnQueryBusWithPullKeyValueEntryQueryForPostingJournalResultKey()
        {
            Guid postingWarningIdentifier = Guid.NewGuid();
            PostingWarningCollectionViewModel postingWarningCollectionViewModel = BuildPostingWarningCollectionViewModel(postingWarningIdentifier);
            ApplyPostingJournalResultViewModel applyPostingJournalResultViewModel = BuildApplyPostingJournalResultViewModel(postingWarningCollectionViewModel);
            IKeyValueEntry keyValueEntryForPostingJournalResult = BuildKeyValueEntryForPostingJournalResult(applyPostingJournalResultViewModel);
            Controller sut = CreateSut(keyValueEntryForPostingJournalResult: keyValueEntryForPostingJournalResult);

            string postingJournalResultKey = _fixture.Create<string>();
            await sut.RemovePostingWarningFromPostingJournalResult(_fixture.Create<int>(), postingJournalResultKey, postingWarningIdentifier);

            _queryBusMock.Verify(m => m.QueryAsync<IPullKeyValueEntryQuery, IKeyValueEntry>(It.Is<IPullKeyValueEntryQuery>(query => query != null && string.CompareOrdinal(query.Key, postingJournalResultKey) == 0)), Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyHasValueAndPostingJournalResultFromKeyValueEntryContainsOnePostingWarningMatchingPostingWarningIdentifier_AssertPublishAsyncWasCalledOnCommandBusWithDeleteKeyValueEntryCommandForPostingJournalResult()
        {
            Guid postingWarningIdentifier = Guid.NewGuid();
            PostingWarningCollectionViewModel postingWarningCollectionViewModel = BuildPostingWarningCollectionViewModel(postingWarningIdentifier);
            ApplyPostingJournalResultViewModel applyPostingJournalResultViewModel = BuildApplyPostingJournalResultViewModel(postingWarningCollectionViewModel);
            IKeyValueEntry keyValueEntryForPostingJournalResult = BuildKeyValueEntryForPostingJournalResult(applyPostingJournalResultViewModel);
            Controller sut = CreateSut(keyValueEntryForPostingJournalResult: keyValueEntryForPostingJournalResult);

            string postingJournalResultKey = _fixture.Create<string>();
            await sut.RemovePostingWarningFromPostingJournalResult(_fixture.Create<int>(), postingJournalResultKey, postingWarningIdentifier);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IDeleteKeyValueEntryCommand>(command => command != null && string.CompareOrdinal(command.Key, postingJournalResultKey) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyHasValue_ReturnsNotNull()
        {
            Controller sut = CreateSut(hasKeyValueEntryForPostingJournalResult: _random.Next(100) > 50);

            IActionResult result = await sut.RemovePostingWarningFromPostingJournalResult(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyHasValue_ReturnsPartialViewResult()
        {
            Controller sut = CreateSut(hasKeyValueEntryForPostingJournalResult: _random.Next(100) > 50);

            IActionResult result = await sut.RemovePostingWarningFromPostingJournalResult(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            Assert.That(result, Is.TypeOf<PartialViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyHasValue_ReturnsPartialViewResultWhereViewNameIsNotNull()
        {
            Controller sut = CreateSut(hasKeyValueEntryForPostingJournalResult: _random.Next(100) > 50);

            PartialViewResult result = (PartialViewResult)await sut.RemovePostingWarningFromPostingJournalResult(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            Assert.That(result.ViewName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyHasValue_ReturnsPartialViewResultWhereViewNameIsEqualToPostingWarningCollectionPartial()
        {
            Controller sut = CreateSut(hasKeyValueEntryForPostingJournalResult: _random.Next(100) > 50);

            PartialViewResult result = (PartialViewResult)await sut.RemovePostingWarningFromPostingJournalResult(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            Assert.That(result.ViewName, Is.EqualTo("_PostingWarningCollectionPartial"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyHasValue_ReturnsPartialViewResultWhereModelIsNotNull()
        {
            Controller sut = CreateSut(hasKeyValueEntryForPostingJournalResult: _random.Next(100) > 50);

            PartialViewResult result = (PartialViewResult)await sut.RemovePostingWarningFromPostingJournalResult(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            Assert.That(result.Model, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyHasValue_ReturnsPartialViewResultWhereModelIsPostingWarningCollectionViewModel()
        {
            Controller sut = CreateSut(hasKeyValueEntryForPostingJournalResult: _random.Next(100) > 50);

            PartialViewResult result = (PartialViewResult)await sut.RemovePostingWarningFromPostingJournalResult(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            Assert.That(result.Model, Is.TypeOf<PostingWarningCollectionViewModel>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyHasValueAndPostingJournalResultFromKeyValueEntryDoesNotContainPostingWarningForPostingWarningIdentifier_ReturnsPartialViewResultWhereModelIsPostingWarningCollectionViewModelWithPostingWarningsFromReturnedPostingJournalResult()
        {
            Guid[] postingWarningIdentifierCollection = BuildPostingWarningIdentifierCollection();
            PostingWarningCollectionViewModel postingWarningCollectionViewModel = BuildPostingWarningCollectionViewModel(postingWarningIdentifierCollection);
            ApplyPostingJournalResultViewModel applyPostingJournalResultViewModel = BuildApplyPostingJournalResultViewModel(postingWarningCollectionViewModel);
            IKeyValueEntry keyValueEntryForPostingJournalResult = BuildKeyValueEntryForPostingJournalResult(applyPostingJournalResultViewModel);
            Controller sut = CreateSut(keyValueEntryForPostingJournalResult: keyValueEntryForPostingJournalResult);

            PartialViewResult result = (PartialViewResult)await sut.RemovePostingWarningFromPostingJournalResult(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            PostingWarningCollectionViewModel resultViewModel = (PostingWarningCollectionViewModel)result.Model;

            // ReSharper disable AssignNullToNotNullAttribute
            Assert.That(resultViewModel.All(postingWarningViewModel => postingWarningIdentifierCollection.Any(postingWarningIdentifier => postingWarningViewModel.Identifier == postingWarningIdentifier)), Is.True);
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyHasValueAndPostingJournalResultFromKeyValueEntryContainsMultiplePostingWarningsWhereOneMatchesPostingWarningIdentifier_ReturnsPartialViewResultWhereModelIsPostingWarningCollectionViewModelWithPostingWarningsFromReturnedPostingJournalResultExceptPostingWarningForPostingWarningIdentifier()
        {
            Guid[] postingWarningIdentifierCollection = BuildPostingWarningIdentifierCollection();
            PostingWarningCollectionViewModel postingWarningCollectionViewModel = BuildPostingWarningCollectionViewModel(postingWarningIdentifierCollection);
            ApplyPostingJournalResultViewModel applyPostingJournalResultViewModel = BuildApplyPostingJournalResultViewModel(postingWarningCollectionViewModel);
            IKeyValueEntry keyValueEntryForPostingJournalResult = BuildKeyValueEntryForPostingJournalResult(applyPostingJournalResultViewModel);
            Controller sut = CreateSut(keyValueEntryForPostingJournalResult: keyValueEntryForPostingJournalResult);

            Guid postingWarningIdentifier = postingWarningIdentifierCollection[_random.Next(0, postingWarningIdentifierCollection.Length - 1)];
            PartialViewResult result = (PartialViewResult)await sut.RemovePostingWarningFromPostingJournalResult(_fixture.Create<int>(), _fixture.Create<string>(), postingWarningIdentifier);

            PostingWarningCollectionViewModel resultViewModel = (PostingWarningCollectionViewModel)result.Model;

            // ReSharper disable AssignNullToNotNullAttribute
            Assert.That(resultViewModel.All(postingWarningViewModel => postingWarningIdentifierCollection.Where(identifier => identifier != postingWarningIdentifier).Any(identifier => postingWarningViewModel.Identifier == identifier)), Is.True);
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyHasValueAndPostingJournalResultFromKeyValueEntryContainsOnePostingWarningMatchingPostingWarningIdentifier_ReturnsPartialViewResultWhereModelIsPostingWarningCollectionViewModelWithEmptyPostingWarnings()
        {
            Guid postingWarningIdentifier = Guid.NewGuid();
            PostingWarningCollectionViewModel postingWarningCollectionViewModel = BuildPostingWarningCollectionViewModel(postingWarningIdentifier);
            ApplyPostingJournalResultViewModel applyPostingJournalResultViewModel = BuildApplyPostingJournalResultViewModel(postingWarningCollectionViewModel);
            IKeyValueEntry keyValueEntryForPostingJournalResult = BuildKeyValueEntryForPostingJournalResult(applyPostingJournalResultViewModel);
            Controller sut = CreateSut(keyValueEntryForPostingJournalResult: keyValueEntryForPostingJournalResult);

            PartialViewResult result = (PartialViewResult)await sut.RemovePostingWarningFromPostingJournalResult(_fixture.Create<int>(), _fixture.Create<string>(), postingWarningIdentifier);

            PostingWarningCollectionViewModel resultViewModel = (PostingWarningCollectionViewModel)result.Model;

            Assert.That(resultViewModel, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyHasValueAndPostingJournalResultFromKeyValueEntryContainsOnePostingWarningMatchingPostingWarningIdentifier_ReturnsPartialViewResultWhereModelIsPostingWarningCollectionViewModelWithSortedPostingWarnings()
        {
            Guid[] postingWarningIdentifierCollection = BuildPostingWarningIdentifierCollection();
            PostingWarningCollectionViewModel postingWarningCollectionViewModel = BuildPostingWarningCollectionViewModel(postingWarningIdentifierCollection);
            ApplyPostingJournalResultViewModel applyPostingJournalResultViewModel = BuildApplyPostingJournalResultViewModel(postingWarningCollectionViewModel);
            IKeyValueEntry keyValueEntryForPostingJournalResult = BuildKeyValueEntryForPostingJournalResult(applyPostingJournalResultViewModel);
            Controller sut = CreateSut(keyValueEntryForPostingJournalResult: keyValueEntryForPostingJournalResult);

            Guid postingWarningIdentifier = postingWarningIdentifierCollection[_random.Next(0, postingWarningIdentifierCollection.Length - 1)];
            PartialViewResult result = (PartialViewResult)await sut.RemovePostingWarningFromPostingJournalResult(_fixture.Create<int>(), _fixture.Create<string>(), postingWarningIdentifier);

            PostingWarningCollectionViewModel resultViewModel = (PostingWarningCollectionViewModel)result.Model;

            // ReSharper disable PossibleNullReferenceException
            for (int i = 1; i < resultViewModel.Count; i++)
            {
                Assert.That(resultViewModel[i].PostingLine.PostingDate.Date, Is.LessThanOrEqualTo(resultViewModel[i - 1].PostingLine.PostingDate.Date));
                if (resultViewModel[i].PostingLine.PostingDate.Date != resultViewModel[i - 1].PostingLine.PostingDate.Date)
                {
                    continue;
                }

                Assert.That(resultViewModel[i].PostingLine.SortOrder, Is.LessThanOrEqualTo(resultViewModel[i - 1].PostingLine.SortOrder));
                if (resultViewModel[i].PostingLine.SortOrder != resultViewModel[i - 1].PostingLine.SortOrder)
                {
                    continue;
                }

                Assert.That((int)resultViewModel[i].Reason, Is.LessThanOrEqualTo((int)resultViewModel[i - 1].Reason));
            }
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyHasValueAndNoKeyValueEntryForPostingJournalResultWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsPostingWarningCollectionViewModelWithEmptyPostingWarnings()
        {
            Controller sut = CreateSut(hasKeyValueEntryForPostingJournalResult: false);

            PartialViewResult result = (PartialViewResult)await sut.RemovePostingWarningFromPostingJournalResult(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            PostingWarningCollectionViewModel resultViewModel = (PostingWarningCollectionViewModel)result.Model;

            Assert.That(resultViewModel, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyHasValue_ReturnsPartialViewResultWhereViewDataIsNotNull()
        {
            Controller sut = CreateSut(hasKeyValueEntryForPostingJournalResult: _random.Next(100) > 50);

            PartialViewResult result = (PartialViewResult)await sut.RemovePostingWarningFromPostingJournalResult(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            Assert.That(result.ViewData, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyHasValue_ReturnsPartialViewResultWhereViewDataIsNotEmpty()
        {
            Controller sut = CreateSut(hasKeyValueEntryForPostingJournalResult: _random.Next(100) > 50);

            PartialViewResult result = (PartialViewResult)await sut.RemovePostingWarningFromPostingJournalResult(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            Assert.That(result.ViewData, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyHasValue_ReturnsPartialViewResultWhereViewDataContainsKeyForAccountingNumber()
        {
            Controller sut = CreateSut(hasKeyValueEntryForPostingJournalResult: _random.Next(100) > 50);

            PartialViewResult result = (PartialViewResult)await sut.RemovePostingWarningFromPostingJournalResult(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            Assert.That(result.ViewData.ContainsKey("AccountingNumber"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyHasValue_ReturnsPartialViewResultWhereViewDataContainsKeyForAccountingNumberWhereValueIsNotNull()
        {
            Controller sut = CreateSut(hasKeyValueEntryForPostingJournalResult: _random.Next(100) > 50);

            int accountingNumber = _fixture.Create<int>();
            PartialViewResult result = (PartialViewResult)await sut.RemovePostingWarningFromPostingJournalResult(accountingNumber, _fixture.Create<string>(), Guid.NewGuid());

            Assert.That(result.ViewData["AccountingNumber"], Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyHasValue_ReturnsPartialViewResultWhereViewDataContainsKeyForAccountingNumberWhereValueIsEqualToAccountingNumberFromArguments()
        {
            Controller sut = CreateSut(hasKeyValueEntryForPostingJournalResult: _random.Next(100) > 50);

            int accountingNumber = _fixture.Create<int>();
            PartialViewResult result = (PartialViewResult)await sut.RemovePostingWarningFromPostingJournalResult(accountingNumber, _fixture.Create<string>(), Guid.NewGuid());

            Assert.That(result.ViewData["AccountingNumber"], Is.EqualTo(accountingNumber));
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyHasValue_ReturnsPartialViewResultWhereViewDataContainsKeyForPostingJournalResultKey()
        {
            Controller sut = CreateSut(hasKeyValueEntryForPostingJournalResult: _random.Next(100) > 50);

            PartialViewResult result = (PartialViewResult)await sut.RemovePostingWarningFromPostingJournalResult(_fixture.Create<int>(), _fixture.Create<string>(), Guid.NewGuid());

            Assert.That(result.ViewData.ContainsKey("PostingJournalResultKey"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyHasValue_ReturnsPartialViewResultWhereViewDataContainsKeyForPostingJournalResultKeyWhereValueIsNotNul()
        {
            Controller sut = CreateSut(hasKeyValueEntryForPostingJournalResult: _random.Next(100) > 50);

            string postingJournalResultKey = _fixture.Create<string>();
            PartialViewResult result = (PartialViewResult)await sut.RemovePostingWarningFromPostingJournalResult(_fixture.Create<int>(), postingJournalResultKey, Guid.NewGuid());

            Assert.That(result.ViewData["PostingJournalResultKey"], Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RemovePostingWarningFromPostingJournalResult_WhenPostingJournalResultKeyHasValue_ReturnsPartialViewResultWhereViewDataContainsKeyForPostingJournalResultKeyWhereValueIsEqualToPostingJournalResultKeyFromArguments()
        {
            Controller sut = CreateSut(hasKeyValueEntryForPostingJournalResult: _random.Next(100) > 50);

            string postingJournalResultKey = _fixture.Create<string>();
            PartialViewResult result = (PartialViewResult)await sut.RemovePostingWarningFromPostingJournalResult(_fixture.Create<int>(), postingJournalResultKey, Guid.NewGuid());

            Assert.That(result.ViewData["PostingJournalResultKey"], Is.EqualTo(postingJournalResultKey));
        }

        private Controller CreateSut(bool canModifyAccounting = true, bool hasKeyValueEntryForPostingJournalResult = true, IKeyValueEntry keyValueEntryForPostingJournalResult = null)
        {
            _claimResolverMock.Setup(m => m.CanModifyAccounting(It.IsAny<int>()))
                .Returns(canModifyAccounting);

            _queryBusMock.Setup(m => m.QueryAsync<IPullKeyValueEntryQuery, IKeyValueEntry>(It.IsAny<IPullKeyValueEntryQuery>()))
                .Returns(Task.FromResult(hasKeyValueEntryForPostingJournalResult ? keyValueEntryForPostingJournalResult ?? BuildKeyValueEntryForPostingJournalResult() : null));
            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<IPushKeyValueEntryCommand>()))
                .Returns(Task.CompletedTask);
            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<IDeleteKeyValueEntryCommand>()))
                .Returns(Task.CompletedTask);

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
        }

        private IKeyValueEntry BuildKeyValueEntryForPostingJournalResult(ApplyPostingJournalResultViewModel applyPostingJournalResultViewModel = null)
        {
            return BuildKeyValueEntryForPostingJournalResultMock(applyPostingJournalResultViewModel).Object;
        }

        private Mock<IKeyValueEntry> BuildKeyValueEntryForPostingJournalResultMock(ApplyPostingJournalResultViewModel applyPostingJournalResultViewModel = null)
        {
            return _fixture.BuildKeyValueEntryMock(toObject: applyPostingJournalResultViewModel ?? BuildApplyPostingJournalResultViewModel());
        }

        private ApplyPostingJournalResultViewModel BuildApplyPostingJournalResultViewModel(PostingWarningCollectionViewModel postingWarningCollectionViewModel = null)
        {
            return _fixture.Build<ApplyPostingJournalResultViewModel>()
                .With(m => m.PostingLines, BuildPostingLineCollectionViewModel)
                .With(m => m.PostingWarnings, postingWarningCollectionViewModel ?? BuildPostingWarningCollectionViewModel(BuildPostingWarningIdentifierCollection()))
                .Create();
        }

        private PostingLineCollectionViewModel BuildPostingLineCollectionViewModel()
        {
            PostingLineCollectionViewModel postingLineCollectionViewModel = new PostingLineCollectionViewModel();
            postingLineCollectionViewModel.AddRange(new[] { BuildPostingLineViewModel(), BuildPostingLineViewModel(), BuildPostingLineViewModel() });
            return postingLineCollectionViewModel;
        }

        private PostingWarningCollectionViewModel BuildPostingWarningCollectionViewModel(params Guid[] postingWarningIdentifierCollection)
        {
            NullGuard.NotNull(postingWarningIdentifierCollection, nameof(postingWarningIdentifierCollection));

            PostingWarningCollectionViewModel postingWarningCollectionViewModel = new PostingWarningCollectionViewModel();
            postingWarningCollectionViewModel.AddRange(postingWarningIdentifierCollection.Select(BuildPostingWarningViewModel).ToArray());
            return postingWarningCollectionViewModel;
        }

        private PostingWarningViewModel BuildPostingWarningViewModel(Guid postingWarningIdentifier)
        {
            return _fixture.Build<PostingWarningViewModel>()
                .With(m => m.Identifier, postingWarningIdentifier)
                .With(m => m.PostingLine, BuildPostingLineViewModel())
                .Create();
        }

        private PostingLineViewModel BuildPostingLineViewModel()
        {
            return _fixture.Build<PostingLineViewModel>()
                .With(m => m.PostingDate, DateTime.Today.AddDays(_random.Next(0, 7) * -1))
                .With(m => m.SortOrder, _fixture.Create<int>())
                .Create();
        }

        private Guid[] BuildPostingWarningIdentifierCollection()
        {
            return _fixture.CreateMany<Guid>(_random.Next(5, 10)).ToArray();
        }
    }
}