using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
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
    public class ApplyPostingJournalTests
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
        public void ApplyPostingJournal_WhenApplyPostingJournalViewModelIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ApplyPostingJournal(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("applyPostingJournalViewModel"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyPostingJournal_WhenApplyPostingJournalViewModelIsInvalid_ThrowsIntranetValidationExceptionWhereMessageIsNotNull()
        {
            Controller sut = CreateSut(false);

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.Message, Is.Not.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyPostingJournal_WhenApplyPostingJournalViewModelIsInvalid_ThrowsIntranetValidationExceptionWhereMessageIsNotEmpty()
        {
            Controller sut = CreateSut(false);

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.Message, Is.Not.Empty);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyPostingJournal_WhenApplyPostingJournalViewModelIsInvalid_ThrowsIntranetValidationExceptionWhereMessageContainsValidationError()
        {
            string validationError = _fixture.Create<string>();
            Controller sut = CreateSut(false, validationError);

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.Message.Contains(validationError), Is.True);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyPostingJournal_WhenApplyPostingJournalViewModelIsInvalid_ThrowsIntranetValidationExceptionWhereErrorCodeIsEqualToSubmittedMessageInvalid()
        {
            Controller sut = CreateSut(false);

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.SubmittedMessageInvalid));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyPostingJournal_WhenApplyPostingJournalViewModelIsInvalid_ThrowsIntranetValidationExceptionWhereValidatingTypeIsNotNull()
        {
            Controller sut = CreateSut(false);

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ValidatingType, Is.Not.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyPostingJournal_WhenApplyPostingJournalViewModelIsInvalid_ThrowsIntranetValidationExceptionWhereValidatingTypeIsEqualToApplyPostingJournalViewModel()
        {
            Controller sut = CreateSut(false);

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ValidatingType, Is.EqualTo(typeof(ApplyPostingJournalViewModel)));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyPostingJournal_WhenApplyPostingJournalViewModelIsInvalid_ThrowsIntranetValidationExceptionWhereValidatingFieldIsNotNull()
        {
            Controller sut = CreateSut(false);

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ValidatingField, Is.Not.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyPostingJournal_WhenApplyPostingJournalViewModelIsInvalid_ThrowsIntranetValidationExceptionWhereValidatingFieldIsNotEmpty()
        {
            Controller sut = CreateSut(false);

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ValidatingField, Is.Not.Empty);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyPostingJournal_WhenApplyPostingJournalViewModelIsInvalid_ThrowsIntranetValidationExceptionWhereValidatingFieldIsEqualToApplyPostingJournalViewModel()
        {
            Controller sut = CreateSut(false);

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ValidatingField, Is.EqualTo("applyPostingJournalViewModel"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyPostingJournal_WhenApplyPostingJournalViewModelIsInvalid_ThrowsIntranetValidationExceptionWhereInnerExceptionIsNull()
        {
            Controller sut = CreateSut(false);

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValid_AssertCanModifyAccountingWasCalledOnClaimResolver()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel(accountingNumber));

            _claimResolverMock.Verify(m => m.CanModifyAccounting(It.Is<int>(value => value == accountingNumber)), Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenCanModifyAccountingReturnsFalse_ReturnsNotNull()
        {
            Controller sut = CreateSut(canModifyAccounting: false);

            IActionResult result = await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenCanModifyAccountingReturnsFalse_ReturnsForbidResult()
        {
            Controller sut = CreateSut(canModifyAccounting: false);

            IActionResult result = await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel());

            Assert.That(result, Is.TypeOf<ForbidResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValid_AssertQueryAsyncWasCalledOnQueryBusWithGetUserSpecificKeyQueryForPostingJournalKey()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            ApplyPostingJournalViewModel applyPostingJournalViewModel = BuildApplyPostingJournalViewModel(accountingNumber);
            await sut.ApplyPostingJournal(applyPostingJournalViewModel);

            _queryBusMock.Verify(m => m.QueryAsync<IGetUserSpecificKeyQuery, string>(It.Is<IGetUserSpecificKeyQuery>(query => query != null && query.KeyElementCollection != null && query.KeyElementCollection.Count() == 2 && string.CompareOrdinal(query.KeyElementCollection.ElementAtOrDefault(0), nameof(ApplyPostingJournalViewModel)) == 0 && string.CompareOrdinal(query.KeyElementCollection.ElementAt(1), Convert.ToString(accountingNumber)) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValid_AssertQueryAsyncWasCalledOnQueryBusWithGetUserSpecificKeyQueryForPostingJournalResultKey()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            ApplyPostingJournalViewModel applyPostingJournalViewModel = BuildApplyPostingJournalViewModel(accountingNumber);
            await sut.ApplyPostingJournal(applyPostingJournalViewModel);

            _queryBusMock.Verify(m => m.QueryAsync<IGetUserSpecificKeyQuery, string>(It.Is<IGetUserSpecificKeyQuery>(query => query != null && query.KeyElementCollection != null && query.KeyElementCollection.Count() == 2 && string.CompareOrdinal(query.KeyElementCollection.ElementAtOrDefault(0), nameof(ApplyPostingJournalResultViewModel)) == 0 && string.CompareOrdinal(query.KeyElementCollection.ElementAt(1), Convert.ToString(accountingNumber)) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValid_AssertQueryAsyncWasCalledOnQueryBusWithPullKeyValueEntryQueryForPostingJournalKey()
        {
            string postingJournalKey = _fixture.Create<string>();
            Controller sut = CreateSut(postingJournalKey: postingJournalKey);

            await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel());

            _queryBusMock.Verify(m => m.QueryAsync<IPullKeyValueEntryQuery, IKeyValueEntry>(It.Is<IPullKeyValueEntryQuery>(query => query != null && string.CompareOrdinal(query.Key, postingJournalKey) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValidAndKeyValueEntryForPostingJournalKeyWasReturnedFromQueryBus_AssertToObjectWasCalledOnKeyValueEntryForPostingJournalKey()
        {
            Mock<IKeyValueEntry> keyValueEntryForPostingJournalKeyMock = BuildKeyValueEntryForPostingJournalKeyMock();
            Controller sut = CreateSut(keyValueEntryForPostingJournalKey: keyValueEntryForPostingJournalKeyMock.Object);

            await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel());

            keyValueEntryForPostingJournalKeyMock.Verify(m => m.ToObject<It.IsSubtype<ApplyPostingJournalViewModel>>(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValid_AssertQueryAsyncWasCalledOnQueryBusWithPullKeyValueEntryQueryForPostingJournalResultKey()
        {
            string postingJournalResultKey = _fixture.Create<string>();
            Controller sut = CreateSut(postingJournalResultKey: postingJournalResultKey);

            await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel());

            _queryBusMock.Verify(m => m.QueryAsync<IPullKeyValueEntryQuery, IKeyValueEntry>(It.Is<IPullKeyValueEntryQuery>(query => query != null && string.CompareOrdinal(query.Key, postingJournalResultKey) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValidAndKeyValueEntryForPostingJournalResultKeyWasReturnedFromQueryBus_AssertToObjectWasCalledOnKeyValueEntryForPostingJournalResultKey()
        {
            Mock<IKeyValueEntry> keyValueEntryForPostingJournalResultKeyMock = BuildKeyValueEntryForPostingJournalResultKeyMock();
            Controller sut = CreateSut(keyValueEntryForPostingJournalResultKey: keyValueEntryForPostingJournalResultKeyMock.Object);

            await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel());

            keyValueEntryForPostingJournalResultKeyMock.Verify(m => m.ToObject<It.IsSubtype<ApplyPostingJournalResultViewModel>>(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValid_AssertPublishAsyncWasCalledOnCommandBusWithApplyPostingJournalCommand()
        {
            Controller sut = CreateSut();

            await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel());

            _commandBusMock.Verify(m => m.PublishAsync<IApplyPostingJournalCommand, IPostingJournalResult>(It.IsNotNull<IApplyPostingJournalCommand>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValid_AssertPublishAsyncWasCalledOnCommandBusWithApplyPostingJournalCommandWhereAccountingNumberIsEqualToAccountingNumberFromApplyPostingJournalViewModel()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            ApplyPostingJournalViewModel applyPostingJournalViewModel = BuildApplyPostingJournalViewModel(accountingNumber);
            await sut.ApplyPostingJournal(applyPostingJournalViewModel);

            _commandBusMock.Verify(m => m.PublishAsync<IApplyPostingJournalCommand, IPostingJournalResult>(It.Is<IApplyPostingJournalCommand>(command => command != null && command.AccountingNumber == accountingNumber)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValid_AssertPublishAsyncWasCalledOnCommandBusWithApplyPostingJournalCommandWherePostingLineCollectionIsNotNull()
        {
            Controller sut = CreateSut();

            ApplyPostingLineCollectionViewModel applyPostingLineCollectionViewModel = BuildApplyPostingLineCollectionViewModel(BuildPostingLineIdentifierCollection());
            ApplyPostingJournalViewModel applyPostingJournalViewModel = BuildApplyPostingJournalViewModel(applyPostingLineCollectionViewModel: applyPostingLineCollectionViewModel);
            await sut.ApplyPostingJournal(applyPostingJournalViewModel);

            _commandBusMock.Verify(m => m.PublishAsync<IApplyPostingJournalCommand, IPostingJournalResult>(It.Is<IApplyPostingJournalCommand>(command => command != null && command.PostingLineCollection != null)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValid_AssertPublishAsyncWasCalledOnCommandBusWithApplyPostingJournalCommandWherePostingLineCollectionIsNotEmpty()
        {
            Controller sut = CreateSut();

            ApplyPostingLineCollectionViewModel applyPostingLineCollectionViewModel = BuildApplyPostingLineCollectionViewModel(BuildPostingLineIdentifierCollection());
            ApplyPostingJournalViewModel applyPostingJournalViewModel = BuildApplyPostingJournalViewModel(applyPostingLineCollectionViewModel: applyPostingLineCollectionViewModel);
            await sut.ApplyPostingJournal(applyPostingJournalViewModel);

            _commandBusMock.Verify(m => m.PublishAsync<IApplyPostingJournalCommand, IPostingJournalResult>(It.Is<IApplyPostingJournalCommand>(command => command != null && command.PostingLineCollection.Any())), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValid_AssertPublishAsyncWasCalledOnCommandBusWithApplyPostingJournalCommandWherePostingLineCollectionContainsSameAmountOfPostingLinesAsApplyPostingJournalViewModel()
        {
            Controller sut = CreateSut();

            Guid[] postingLineIdentifierCollection = BuildPostingLineIdentifierCollection();
            ApplyPostingLineCollectionViewModel applyPostingLineCollectionViewModel = BuildApplyPostingLineCollectionViewModel(postingLineIdentifierCollection);
            ApplyPostingJournalViewModel applyPostingJournalViewModel = BuildApplyPostingJournalViewModel(applyPostingLineCollectionViewModel: applyPostingLineCollectionViewModel);
            await sut.ApplyPostingJournal(applyPostingJournalViewModel);

            _commandBusMock.Verify(m => m.PublishAsync<IApplyPostingJournalCommand, IPostingJournalResult>(It.Is<IApplyPostingJournalCommand>(command => command != null && command.PostingLineCollection.Count() == postingLineIdentifierCollection.Length)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValid_AssertPublishAsyncWasCalledOnCommandBusWithApplyPostingJournalCommandWherePostingLineCollectionContainsAllPostingLinesFromApplyPostingJournalViewModel()
        {
            Controller sut = CreateSut();

            Guid[] postingLineIdentifierCollection = BuildPostingLineIdentifierCollection();
            ApplyPostingLineCollectionViewModel applyPostingLineCollectionViewModel = BuildApplyPostingLineCollectionViewModel(postingLineIdentifierCollection);
            ApplyPostingJournalViewModel applyPostingJournalViewModel = BuildApplyPostingJournalViewModel(applyPostingLineCollectionViewModel: applyPostingLineCollectionViewModel);
            await sut.ApplyPostingJournal(applyPostingJournalViewModel);

            _commandBusMock.Verify(m => m.PublishAsync<IApplyPostingJournalCommand, IPostingJournalResult>(It.Is<IApplyPostingJournalCommand>(command => command != null && command.PostingLineCollection.All(applyPostingLineCommand => postingLineIdentifierCollection.Contains(applyPostingLineCommand.Identifier ?? Guid.Empty)))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValidAndApplyPostingJournalViewModelFromKeyValueEntryContainsSamePostingLinesAsPostingJournalResultFormCommandBus_AssertQueryAsyncWasCalledTwiceOnQueryBusWithPullKeyValueEntryQueryForPostingJournalKey()
        {
            Guid[] postingLineIdentifierCollection = BuildPostingLineIdentifierCollection();
            ApplyPostingLineCollectionViewModel applyPostingLineCollectionViewModelFromKeyValueEntry = BuildApplyPostingLineCollectionViewModel(postingLineIdentifierCollection);
            ApplyPostingJournalViewModel applyPostingJournalViewModelFromKeyValueEntry = BuildApplyPostingJournalViewModel(applyPostingLineCollectionViewModel: applyPostingLineCollectionViewModelFromKeyValueEntry);
            string postingJournalKey = _fixture.Create<string>();
            IKeyValueEntry keyValueEntryForPostingJournalKey = BuildKeyValueEntryForPostingJournalKey(applyPostingJournalViewModelFromKeyValueEntry);
            IPostingLineCollection postingLineCollection = BuildPostingLineCollection(postingLineIdentifierCollection);
            IPostingJournalResult postingJournalResult = BuildPostingJournalResult(postingLineCollection);
            Controller sut = CreateSut(postingJournalKey: postingJournalKey, keyValueEntryForPostingJournalKey: keyValueEntryForPostingJournalKey, postingJournalResult: postingJournalResult);

            await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel());

            _queryBusMock.Verify(m => m.QueryAsync<IPullKeyValueEntryQuery, IKeyValueEntry>(It.Is<IPullKeyValueEntryQuery>(query => query != null && string.CompareOrdinal(query.Key, postingJournalKey) == 0)), Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValidAndApplyPostingJournalViewModelFromKeyValueEntryContainsSamePostingLinesAsPostingJournalResultFormCommandBus_AssertPublishAsyncWasCalledOnCommandBusWithDeleteKeyValueEntryCommandForPostingJournalKey()
        {
            Guid[] postingLineIdentifierCollection = BuildPostingLineIdentifierCollection();
            ApplyPostingLineCollectionViewModel applyPostingLineCollectionViewModelFromKeyValueEntry = BuildApplyPostingLineCollectionViewModel(postingLineIdentifierCollection);
            ApplyPostingJournalViewModel applyPostingJournalViewModelFromKeyValueEntry = BuildApplyPostingJournalViewModel(applyPostingLineCollectionViewModel: applyPostingLineCollectionViewModelFromKeyValueEntry);
            string postingJournalKey = _fixture.Create<string>();
            IKeyValueEntry keyValueEntryForPostingJournalKey = BuildKeyValueEntryForPostingJournalKey(applyPostingJournalViewModelFromKeyValueEntry);
            IPostingLineCollection postingLineCollection = BuildPostingLineCollection(postingLineIdentifierCollection);
            IPostingJournalResult postingJournalResult = BuildPostingJournalResult(postingLineCollection);
            Controller sut = CreateSut(postingJournalKey: postingJournalKey, keyValueEntryForPostingJournalKey: keyValueEntryForPostingJournalKey, postingJournalResult: postingJournalResult);

            await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel());

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IDeleteKeyValueEntryCommand>(command => command != null && string.CompareOrdinal(command.Key, postingJournalKey) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValidAndApplyPostingJournalViewModelFromKeyValueEntryContainsSamePostingLinesAsPostingJournalResultFormCommandBus_AssertPublishAsyncWasNotCalledOnCommandBusWithPushKeyValueEntryCommandForPostingJournalKey()
        {
            Guid[] postingLineIdentifierCollection = BuildPostingLineIdentifierCollection();
            ApplyPostingLineCollectionViewModel applyPostingLineCollectionViewModelFromKeyValueEntry = BuildApplyPostingLineCollectionViewModel(postingLineIdentifierCollection);
            ApplyPostingJournalViewModel applyPostingJournalViewModelFromKeyValueEntry = BuildApplyPostingJournalViewModel(applyPostingLineCollectionViewModel: applyPostingLineCollectionViewModelFromKeyValueEntry);
            string postingJournalKey = _fixture.Create<string>();
            IKeyValueEntry keyValueEntryForPostingJournalKey = BuildKeyValueEntryForPostingJournalKey(applyPostingJournalViewModelFromKeyValueEntry);
            IPostingLineCollection postingLineCollection = BuildPostingLineCollection(postingLineIdentifierCollection);
            IPostingJournalResult postingJournalResult = BuildPostingJournalResult(postingLineCollection);
            Controller sut = CreateSut(postingJournalKey: postingJournalKey, keyValueEntryForPostingJournalKey: keyValueEntryForPostingJournalKey, postingJournalResult: postingJournalResult);

            await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel());

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IPushKeyValueEntryCommand>(command => command != null && string.CompareOrdinal(command.Key, postingJournalKey) == 0 && command.Value != null)), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValidAndApplyPostingJournalViewModelFromKeyValueEntryContainsOtherPostingLinesThanPostingJournalResultFormCommandBus_AssertQueryAsyncWasCalledOnceOnQueryBusWithPullKeyValueEntryQueryForPostingJournalKey()
        {
            ApplyPostingLineCollectionViewModel applyPostingLineCollectionViewModelFromKeyValueEntry = BuildApplyPostingLineCollectionViewModel(BuildPostingLineIdentifierCollection());
            ApplyPostingJournalViewModel applyPostingJournalViewModelFromKeyValueEntry = BuildApplyPostingJournalViewModel(applyPostingLineCollectionViewModel: applyPostingLineCollectionViewModelFromKeyValueEntry);
            string postingJournalKey = _fixture.Create<string>();
            IKeyValueEntry keyValueEntryForPostingJournalKey = BuildKeyValueEntryForPostingJournalKey(applyPostingJournalViewModelFromKeyValueEntry);
            IPostingLineCollection postingLineCollection = BuildPostingLineCollection(BuildPostingLineIdentifierCollection());
            IPostingJournalResult postingJournalResult = BuildPostingJournalResult(postingLineCollection);
            Controller sut = CreateSut(postingJournalKey: postingJournalKey, keyValueEntryForPostingJournalKey: keyValueEntryForPostingJournalKey, postingJournalResult: postingJournalResult);

            await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel());

            _queryBusMock.Verify(m => m.QueryAsync<IPullKeyValueEntryQuery, IKeyValueEntry>(It.Is<IPullKeyValueEntryQuery>(query => query != null && string.CompareOrdinal(query.Key, postingJournalKey) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValidAndApplyPostingJournalViewModelFromKeyValueEntryContainsOtherPostingLinesThanPostingJournalResultFormCommandBus_AssertPublishAsyncWasNotCalledOnCommandBusWithDeleteKeyValueEntryCommandForPostingJournalKey()
        {
            ApplyPostingLineCollectionViewModel applyPostingLineCollectionViewModelFromKeyValueEntry = BuildApplyPostingLineCollectionViewModel(BuildPostingLineIdentifierCollection());
            ApplyPostingJournalViewModel applyPostingJournalViewModelFromKeyValueEntry = BuildApplyPostingJournalViewModel(applyPostingLineCollectionViewModel: applyPostingLineCollectionViewModelFromKeyValueEntry);
            string postingJournalKey = _fixture.Create<string>();
            IKeyValueEntry keyValueEntryForPostingJournalKey = BuildKeyValueEntryForPostingJournalKey(applyPostingJournalViewModelFromKeyValueEntry);
            IPostingLineCollection postingLineCollection = BuildPostingLineCollection(BuildPostingLineIdentifierCollection());
            IPostingJournalResult postingJournalResult = BuildPostingJournalResult(postingLineCollection);
            Controller sut = CreateSut(postingJournalKey: postingJournalKey, keyValueEntryForPostingJournalKey: keyValueEntryForPostingJournalKey, postingJournalResult: postingJournalResult);

            await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel());

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IDeleteKeyValueEntryCommand>(command => command != null && string.CompareOrdinal(command.Key, postingJournalKey) == 0)), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValidAndApplyPostingJournalViewModelFromKeyValueEntryContainsOtherPostingLinesThanPostingJournalResultFormCommandBus_AssertPublishAsyncWasCalledOnCommandBusWithPushKeyValueEntryCommandForPostingJournalKey()
        {
            int accountingNumber = _fixture.Create<int>();
            Guid[] postingLineIdentifierCollection = BuildPostingLineIdentifierCollection();
            ApplyPostingLineCollectionViewModel applyPostingLineCollectionViewModelFromKeyValueEntry = BuildApplyPostingLineCollectionViewModel(postingLineIdentifierCollection);
            ApplyPostingJournalViewModel applyPostingJournalViewModelFromKeyValueEntry = BuildApplyPostingJournalViewModel(accountingNumber, applyPostingLineCollectionViewModelFromKeyValueEntry);
            string postingJournalKey = _fixture.Create<string>();
            IKeyValueEntry keyValueEntryForPostingJournalKey = BuildKeyValueEntryForPostingJournalKey(applyPostingJournalViewModelFromKeyValueEntry);
            IPostingLineCollection postingLineCollection = BuildPostingLineCollection(BuildPostingLineIdentifierCollection());
            IPostingJournalResult postingJournalResult = BuildPostingJournalResult(postingLineCollection);
            Controller sut = CreateSut(postingJournalKey: postingJournalKey, keyValueEntryForPostingJournalKey: keyValueEntryForPostingJournalKey, postingJournalResult: postingJournalResult);

            await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel());

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IPushKeyValueEntryCommand>(command => command != null && string.CompareOrdinal(command.Key, postingJournalKey) == 0 && command.Value != null && command.Value.GetType() == typeof(ApplyPostingJournalViewModel) && ((ApplyPostingJournalViewModel)command.Value).AccountingNumber == accountingNumber && ((ApplyPostingJournalViewModel)command.Value).ApplyPostingLines != null && ((ApplyPostingJournalViewModel)command.Value).ApplyPostingLines.Count == postingLineIdentifierCollection.Length && ((ApplyPostingJournalViewModel)command.Value).ApplyPostingLines.All(applyPostingLineViewModel => postingLineIdentifierCollection.Any(postingLineIdentifier => applyPostingLineViewModel.Identifier == postingLineIdentifier)))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValidAndApplyPostingJournalResultViewModelFromKeyValueEntryDoesNotContainAnyPostingWarningsAndPostingJournalResultFromCommandBusDoesNotContainAnyPostingWarnings_AssertQueryAsyncWasCalledTwiceOnQueryBusWithPullKeyValueEntryQueryForPostingJournalResultKey()
        {
            PostingWarningCollectionViewModel postingWarningCollectionViewModelFromKeyValueEntry = BuildPostingWarningCollectionViewModel(Array.Empty<Guid>());
            ApplyPostingJournalResultViewModel applyPostingJournalResultViewModelFromKeyValueEntry = BuildApplyPostingJournalResultViewModel(postingWarningCollectionViewModel: postingWarningCollectionViewModelFromKeyValueEntry);
            string postingJournalResultKey = _fixture.Create<string>();
            IKeyValueEntry keyValueEntryForPostingJournalResultKey = BuildKeyValueEntryForPostingJournalResultKey(applyPostingJournalResultViewModelFromKeyValueEntry);
            IPostingWarningCollection postingWarningCollection = BuildPostingWarningCollection(Array.Empty<IPostingWarning>());
            IPostingJournalResult postingJournalResult = BuildPostingJournalResult(postingWarningCollection: postingWarningCollection);
            Controller sut = CreateSut(postingJournalResultKey: postingJournalResultKey, keyValueEntryForPostingJournalResultKey: keyValueEntryForPostingJournalResultKey, postingJournalResult: postingJournalResult);

            await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel());

            _queryBusMock.Verify(m => m.QueryAsync<IPullKeyValueEntryQuery, IKeyValueEntry>(It.Is<IPullKeyValueEntryQuery>(query => query != null && string.CompareOrdinal(query.Key, postingJournalResultKey) == 0)), Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValidAndApplyPostingJournalResultViewModelFromKeyValueEntryDoesNotContainAnyPostingWarningsAndPostingJournalResultFromCommandBusDoesNotContainAnyPostingWarnings_AssertPublishAsyncWasCalledOnCommandBusWithDeleteKeyValueEntryCommandForPostingJournalResultKey()
        {
            PostingWarningCollectionViewModel postingWarningCollectionViewModelFromKeyValueEntry = BuildPostingWarningCollectionViewModel(Array.Empty<Guid>());
            ApplyPostingJournalResultViewModel applyPostingJournalResultViewModelFromKeyValueEntry = BuildApplyPostingJournalResultViewModel(postingWarningCollectionViewModel: postingWarningCollectionViewModelFromKeyValueEntry);
            string postingJournalResultKey = _fixture.Create<string>();
            IKeyValueEntry keyValueEntryForPostingJournalResultKey = BuildKeyValueEntryForPostingJournalResultKey(applyPostingJournalResultViewModelFromKeyValueEntry);
            IPostingWarningCollection postingWarningCollection = BuildPostingWarningCollection(Array.Empty<IPostingWarning>());
            IPostingJournalResult postingJournalResult = BuildPostingJournalResult(postingWarningCollection: postingWarningCollection);
            Controller sut = CreateSut(postingJournalResultKey: postingJournalResultKey, keyValueEntryForPostingJournalResultKey: keyValueEntryForPostingJournalResultKey, postingJournalResult: postingJournalResult);

            await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel());

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IDeleteKeyValueEntryCommand>(command => command != null && string.CompareOrdinal(command.Key, postingJournalResultKey) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValidAndApplyPostingJournalResultViewModelFromKeyValueEntryDoesNotContainAnyPostingWarningsAndPostingJournalResultFromCommandBusDoesNotContainAnyPostingWarnings_AssertPublishAsyncWasNotCalledOnCommandBusWithPushKeyValueEntryCommandForPostingJournalResultKey()
        {
            PostingWarningCollectionViewModel postingWarningCollectionViewModelFromKeyValueEntry = BuildPostingWarningCollectionViewModel(Array.Empty<Guid>());
            ApplyPostingJournalResultViewModel applyPostingJournalResultViewModelFromKeyValueEntry = BuildApplyPostingJournalResultViewModel(postingWarningCollectionViewModel: postingWarningCollectionViewModelFromKeyValueEntry);
            string postingJournalResultKey = _fixture.Create<string>();
            IKeyValueEntry keyValueEntryForPostingJournalResultKey = BuildKeyValueEntryForPostingJournalResultKey(applyPostingJournalResultViewModelFromKeyValueEntry);
            IPostingWarningCollection postingWarningCollection = BuildPostingWarningCollection(Array.Empty<IPostingWarning>());
            IPostingJournalResult postingJournalResult = BuildPostingJournalResult(postingWarningCollection: postingWarningCollection);
            Controller sut = CreateSut(postingJournalResultKey: postingJournalResultKey, keyValueEntryForPostingJournalResultKey: keyValueEntryForPostingJournalResultKey, postingJournalResult: postingJournalResult);

            await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel());

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IPushKeyValueEntryCommand>(command => command != null && string.CompareOrdinal(command.Key, postingJournalResultKey) == 0 && command.Value != null)), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValidAndApplyPostingJournalResultViewModelFromKeyValueEntryContainsPostingWarningsAndPostingJournalResultFromCommandBusDoesNotContainAnyPostingWarnings_AssertQueryAsyncWasCalledOnceOnQueryBusWithPullKeyValueEntryQueryForPostingJournalResultKey()
        {
            PostingWarningCollectionViewModel postingWarningCollectionViewModelFromKeyValueEntry = BuildPostingWarningCollectionViewModel(BuildPostingWarningIdentifierCollection());
            ApplyPostingJournalResultViewModel applyPostingJournalResultViewModelFromKeyValueEntry = BuildApplyPostingJournalResultViewModel(postingWarningCollectionViewModel: postingWarningCollectionViewModelFromKeyValueEntry);
            string postingJournalResultKey = _fixture.Create<string>();
            IKeyValueEntry keyValueEntryForPostingJournalResultKey = BuildKeyValueEntryForPostingJournalResultKey(applyPostingJournalResultViewModelFromKeyValueEntry);
            IPostingWarningCollection postingWarningCollection = BuildPostingWarningCollection(Array.Empty<IPostingWarning>());
            IPostingJournalResult postingJournalResult = BuildPostingJournalResult(postingWarningCollection: postingWarningCollection);
            Controller sut = CreateSut(postingJournalResultKey: postingJournalResultKey, keyValueEntryForPostingJournalResultKey: keyValueEntryForPostingJournalResultKey, postingJournalResult: postingJournalResult);

            await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel());

            _queryBusMock.Verify(m => m.QueryAsync<IPullKeyValueEntryQuery, IKeyValueEntry>(It.Is<IPullKeyValueEntryQuery>(query => query != null && string.CompareOrdinal(query.Key, postingJournalResultKey) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValidAndApplyPostingJournalResultViewModelFromKeyValueEntryContainsPostingWarningsAndPostingJournalResultFromCommandBusDoesNotContainAnyPostingWarnings_AssertPublishAsyncWasNotCalledOnCommandBusWithDeleteKeyValueEntryCommandForPostingJournalResultKey()
        {
            PostingWarningCollectionViewModel postingWarningCollectionViewModelFromKeyValueEntry = BuildPostingWarningCollectionViewModel(BuildPostingWarningIdentifierCollection());
            ApplyPostingJournalResultViewModel applyPostingJournalResultViewModelFromKeyValueEntry = BuildApplyPostingJournalResultViewModel(postingWarningCollectionViewModel: postingWarningCollectionViewModelFromKeyValueEntry);
            string postingJournalResultKey = _fixture.Create<string>();
            IKeyValueEntry keyValueEntryForPostingJournalResultKey = BuildKeyValueEntryForPostingJournalResultKey(applyPostingJournalResultViewModelFromKeyValueEntry);
            IPostingWarningCollection postingWarningCollection = BuildPostingWarningCollection(Array.Empty<IPostingWarning>());
            IPostingJournalResult postingJournalResult = BuildPostingJournalResult(postingWarningCollection: postingWarningCollection);
            Controller sut = CreateSut(postingJournalResultKey: postingJournalResultKey, keyValueEntryForPostingJournalResultKey: keyValueEntryForPostingJournalResultKey, postingJournalResult: postingJournalResult);

            await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel());

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IDeleteKeyValueEntryCommand>(command => command != null && string.CompareOrdinal(command.Key, postingJournalResultKey) == 0)), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValidAndApplyPostingJournalResultViewModelFromKeyValueEntryContainsPostingWarningsAndPostingJournalResultFromCommandBusDoesNotContainAnyPostingWarnings_AssertPublishAsyncWasCalledOnCommandBusWithPushKeyValueEntryCommandForPostingJournalResultKey()
        {
            Guid[] postingWarningIdentifierCollection = BuildPostingWarningIdentifierCollection();
            PostingWarningCollectionViewModel postingWarningCollectionViewModelFromKeyValueEntry = BuildPostingWarningCollectionViewModel(postingWarningIdentifierCollection);
            ApplyPostingJournalResultViewModel applyPostingJournalResultViewModelFromKeyValueEntry = BuildApplyPostingJournalResultViewModel(postingWarningCollectionViewModel: postingWarningCollectionViewModelFromKeyValueEntry);
            string postingJournalResultKey = _fixture.Create<string>();
            IKeyValueEntry keyValueEntryForPostingJournalResultKey = BuildKeyValueEntryForPostingJournalResultKey(applyPostingJournalResultViewModelFromKeyValueEntry);
            IPostingWarningCollection postingWarningCollection = BuildPostingWarningCollection(Array.Empty<IPostingWarning>());
            IPostingJournalResult postingJournalResult = BuildPostingJournalResult(postingWarningCollection: postingWarningCollection);
            Controller sut = CreateSut(postingJournalResultKey: postingJournalResultKey, keyValueEntryForPostingJournalResultKey: keyValueEntryForPostingJournalResultKey, postingJournalResult: postingJournalResult);

            await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel());

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IPushKeyValueEntryCommand>(command => command != null && string.CompareOrdinal(command.Key, postingJournalResultKey) == 0 && command.Value != null && command.Value.GetType() == typeof(ApplyPostingJournalResultViewModel) && ((ApplyPostingJournalResultViewModel)command.Value).PostingLines != null && ((ApplyPostingJournalResultViewModel)command.Value).PostingLines.Any() == false && ((ApplyPostingJournalResultViewModel)command.Value).PostingWarnings != null && ((ApplyPostingJournalResultViewModel)command.Value).PostingWarnings.Count == postingWarningIdentifierCollection.Length && ((ApplyPostingJournalResultViewModel)command.Value).PostingWarnings != null && ((ApplyPostingJournalResultViewModel)command.Value).PostingWarnings.All(postingWarningViewModel => postingWarningIdentifierCollection.Any(postingWarningIdentifier => postingWarningViewModel.Identifier == postingWarningIdentifier)))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValidAndApplyPostingJournalResultViewModelFromKeyValueEntryDoesNotContainAnyPostingWarningsAndPostingJournalResultFromCommandBusContainsPostingWarnings_AssertQueryAsyncWasCalledOnceOnQueryBusWithPullKeyValueEntryQueryForPostingJournalResultKey()
        {
            PostingWarningCollectionViewModel postingWarningCollectionViewModelFromKeyValueEntry = BuildPostingWarningCollectionViewModel(Array.Empty<Guid>());
            ApplyPostingJournalResultViewModel applyPostingJournalResultViewModelFromKeyValueEntry = BuildApplyPostingJournalResultViewModel(postingWarningCollectionViewModel: postingWarningCollectionViewModelFromKeyValueEntry);
            string postingJournalResultKey = _fixture.Create<string>();
            IKeyValueEntry keyValueEntryForPostingJournalResultKey = BuildKeyValueEntryForPostingJournalResultKey(applyPostingJournalResultViewModelFromKeyValueEntry);
            IPostingWarningCollection postingWarningCollection = BuildPostingWarningCollection(BuildPostingWarning(), BuildPostingWarning(), BuildPostingWarning());
            IPostingJournalResult postingJournalResult = BuildPostingJournalResult(postingWarningCollection: postingWarningCollection);
            Controller sut = CreateSut(postingJournalResultKey: postingJournalResultKey, keyValueEntryForPostingJournalResultKey: keyValueEntryForPostingJournalResultKey, postingJournalResult: postingJournalResult);

            await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel());

            _queryBusMock.Verify(m => m.QueryAsync<IPullKeyValueEntryQuery, IKeyValueEntry>(It.Is<IPullKeyValueEntryQuery>(query => query != null && string.CompareOrdinal(query.Key, postingJournalResultKey) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValidAndApplyPostingJournalResultViewModelFromKeyValueEntryDoesNotContainAnyPostingWarningsAndPostingJournalResultFromCommandBusContainsPostingWarnings_AssertPublishAsyncWasNotCalledOnCommandBusWithDeleteKeyValueEntryCommandForPostingJournalResultKey()
        {
            PostingWarningCollectionViewModel postingWarningCollectionViewModelFromKeyValueEntry = BuildPostingWarningCollectionViewModel(Array.Empty<Guid>());
            ApplyPostingJournalResultViewModel applyPostingJournalResultViewModelFromKeyValueEntry = BuildApplyPostingJournalResultViewModel(postingWarningCollectionViewModel: postingWarningCollectionViewModelFromKeyValueEntry);
            string postingJournalResultKey = _fixture.Create<string>();
            IKeyValueEntry keyValueEntryForPostingJournalResultKey = BuildKeyValueEntryForPostingJournalResultKey(applyPostingJournalResultViewModelFromKeyValueEntry);
            IPostingWarningCollection postingWarningCollection = BuildPostingWarningCollection(BuildPostingWarning(), BuildPostingWarning(), BuildPostingWarning());
            IPostingJournalResult postingJournalResult = BuildPostingJournalResult(postingWarningCollection: postingWarningCollection);
            Controller sut = CreateSut(postingJournalResultKey: postingJournalResultKey, keyValueEntryForPostingJournalResultKey: keyValueEntryForPostingJournalResultKey, postingJournalResult: postingJournalResult);

            await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel());

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IDeleteKeyValueEntryCommand>(command => command != null && string.CompareOrdinal(command.Key, postingJournalResultKey) == 0)), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValidAndApplyPostingJournalResultViewModelFromKeyValueEntryDoesNotContainAnyPostingWarningsAndPostingJournalResultFromCommandBusContainsPostingWarnings_AssertPublishAsyncWasCalledOnCommandBusWithPushKeyValueEntryCommandForPostingJournalResultKey()
        {
            PostingWarningCollectionViewModel postingWarningCollectionViewModelFromKeyValueEntry = BuildPostingWarningCollectionViewModel(Array.Empty<Guid>());
            ApplyPostingJournalResultViewModel applyPostingJournalResultViewModelFromKeyValueEntry = BuildApplyPostingJournalResultViewModel(postingWarningCollectionViewModel: postingWarningCollectionViewModelFromKeyValueEntry);
            string postingJournalResultKey = _fixture.Create<string>();
            IKeyValueEntry keyValueEntryForPostingJournalResultKey = BuildKeyValueEntryForPostingJournalResultKey(applyPostingJournalResultViewModelFromKeyValueEntry);
            IPostingWarningCollection postingWarningCollection = BuildPostingWarningCollection(BuildPostingWarning(), BuildPostingWarning(), BuildPostingWarning());
            IPostingJournalResult postingJournalResult = BuildPostingJournalResult(postingWarningCollection: postingWarningCollection);
            Controller sut = CreateSut(postingJournalResultKey: postingJournalResultKey, keyValueEntryForPostingJournalResultKey: keyValueEntryForPostingJournalResultKey, postingJournalResult: postingJournalResult);

            await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel());

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IPushKeyValueEntryCommand>(command => command != null && string.CompareOrdinal(command.Key, postingJournalResultKey) == 0 && command.Value != null && command.Value.GetType() == typeof(ApplyPostingJournalResultViewModel) && ((ApplyPostingJournalResultViewModel)command.Value).PostingLines != null && ((ApplyPostingJournalResultViewModel)command.Value).PostingLines.Any() == false && ((ApplyPostingJournalResultViewModel)command.Value).PostingWarnings != null && ((ApplyPostingJournalResultViewModel)command.Value).PostingWarnings.Count == 3)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValidAndApplyPostingJournalResultViewModelFromKeyValueEntryContainsPostingWarningsAndPostingJournalResultFromCommandBusContainsPostingWarnings_AssertQueryAsyncWasCalledOnceOnQueryBusWithPullKeyValueEntryQueryForPostingJournalResultKey()
        {
            PostingWarningCollectionViewModel postingWarningCollectionViewModelFromKeyValueEntry = BuildPostingWarningCollectionViewModel(BuildPostingWarningIdentifierCollection());
            ApplyPostingJournalResultViewModel applyPostingJournalResultViewModelFromKeyValueEntry = BuildApplyPostingJournalResultViewModel(postingWarningCollectionViewModel: postingWarningCollectionViewModelFromKeyValueEntry);
            string postingJournalResultKey = _fixture.Create<string>();
            IKeyValueEntry keyValueEntryForPostingJournalResultKey = BuildKeyValueEntryForPostingJournalResultKey(applyPostingJournalResultViewModelFromKeyValueEntry);
            IPostingWarningCollection postingWarningCollection = BuildPostingWarningCollection(BuildPostingWarning(), BuildPostingWarning(), BuildPostingWarning());
            IPostingJournalResult postingJournalResult = BuildPostingJournalResult(postingWarningCollection: postingWarningCollection);
            Controller sut = CreateSut(postingJournalResultKey: postingJournalResultKey, keyValueEntryForPostingJournalResultKey: keyValueEntryForPostingJournalResultKey, postingJournalResult: postingJournalResult);

            await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel());

            _queryBusMock.Verify(m => m.QueryAsync<IPullKeyValueEntryQuery, IKeyValueEntry>(It.Is<IPullKeyValueEntryQuery>(query => query != null && string.CompareOrdinal(query.Key, postingJournalResultKey) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValidAndApplyPostingJournalResultViewModelFromKeyValueEntryContainsPostingWarningsAndPostingJournalResultFromCommandBusContainsPostingWarnings_AssertPublishAsyncWasNotCalledOnCommandBusWithDeleteKeyValueEntryCommandForPostingJournalResultKey()
        {
            PostingWarningCollectionViewModel postingWarningCollectionViewModelFromKeyValueEntry = BuildPostingWarningCollectionViewModel(BuildPostingWarningIdentifierCollection());
            ApplyPostingJournalResultViewModel applyPostingJournalResultViewModelFromKeyValueEntry = BuildApplyPostingJournalResultViewModel(postingWarningCollectionViewModel: postingWarningCollectionViewModelFromKeyValueEntry);
            string postingJournalResultKey = _fixture.Create<string>();
            IKeyValueEntry keyValueEntryForPostingJournalResultKey = BuildKeyValueEntryForPostingJournalResultKey(applyPostingJournalResultViewModelFromKeyValueEntry);
            IPostingWarningCollection postingWarningCollection = BuildPostingWarningCollection(BuildPostingWarning(), BuildPostingWarning(), BuildPostingWarning());
            IPostingJournalResult postingJournalResult = BuildPostingJournalResult(postingWarningCollection: postingWarningCollection);
            Controller sut = CreateSut(postingJournalResultKey: postingJournalResultKey, keyValueEntryForPostingJournalResultKey: keyValueEntryForPostingJournalResultKey, postingJournalResult: postingJournalResult);

            await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel());

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IDeleteKeyValueEntryCommand>(command => command != null && string.CompareOrdinal(command.Key, postingJournalResultKey) == 0)), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValidAndApplyPostingJournalResultViewModelFromKeyValueEntryContainsPostingWarningsAndPostingJournalResultFromCommandBusContainsPostingWarnings_AssertPublishAsyncWasCalledOnCommandBusWithPushKeyValueEntryCommandForPostingJournalResultKey()
        {
            Guid[] postingWarningIdentifierCollection = BuildPostingWarningIdentifierCollection();
            PostingWarningCollectionViewModel postingWarningCollectionViewModelFromKeyValueEntry = BuildPostingWarningCollectionViewModel(postingWarningIdentifierCollection);
            ApplyPostingJournalResultViewModel applyPostingJournalResultViewModelFromKeyValueEntry = BuildApplyPostingJournalResultViewModel(postingWarningCollectionViewModel: postingWarningCollectionViewModelFromKeyValueEntry);
            string postingJournalResultKey = _fixture.Create<string>();
            IKeyValueEntry keyValueEntryForPostingJournalResultKey = BuildKeyValueEntryForPostingJournalResultKey(applyPostingJournalResultViewModelFromKeyValueEntry);
            IPostingWarningCollection postingWarningCollection = BuildPostingWarningCollection(BuildPostingWarning(), BuildPostingWarning(), BuildPostingWarning());
            IPostingJournalResult postingJournalResult = BuildPostingJournalResult(postingWarningCollection: postingWarningCollection);
            Controller sut = CreateSut(postingJournalResultKey: postingJournalResultKey, keyValueEntryForPostingJournalResultKey: keyValueEntryForPostingJournalResultKey, postingJournalResult: postingJournalResult);

            await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel());

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IPushKeyValueEntryCommand>(command => command != null && string.CompareOrdinal(command.Key, postingJournalResultKey) == 0 && command.Value != null && command.Value.GetType() == typeof(ApplyPostingJournalResultViewModel) && ((ApplyPostingJournalResultViewModel)command.Value).PostingLines != null && ((ApplyPostingJournalResultViewModel)command.Value).PostingLines.Any() == false && ((ApplyPostingJournalResultViewModel)command.Value).PostingWarnings != null && ((ApplyPostingJournalResultViewModel)command.Value).PostingWarnings.Count == postingWarningIdentifierCollection.Length + 3 && ((ApplyPostingJournalResultViewModel)command.Value).PostingWarnings != null && postingWarningIdentifierCollection.All(postingWarningIdentifier => ((ApplyPostingJournalResultViewModel)command.Value).PostingWarnings.Any(postingWarningViewModel => postingWarningViewModel.Identifier == postingWarningIdentifier)))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValid_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValid_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel());

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValid_ReturnsRedirectToActionResultWhereActionNameIsNotNull()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult)await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel());

            Assert.That(result.ActionName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValid_ReturnsRedirectToActionResultWhereActionNameIsEqualToAccountings()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult)await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel());

            Assert.That(result.ActionName, Is.EqualTo("Accountings"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValid_ReturnsRedirectToActionResultWhereControllerNameIsNotNull()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult)await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel());

            Assert.That(result.ControllerName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValid_ReturnsRedirectToActionResultWhereControllerNameIsEqualToAccounting()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult)await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel());

            Assert.That(result.ControllerName, Is.EqualTo("Accounting"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValid_ReturnsRedirectToActionResultWhereRouteValuesIsNotNull()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult)await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel());

            Assert.That(result.RouteValues, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValid_ReturnsRedirectToActionResultWhereRouteValuesIsEmpty()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult)await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel());

            Assert.That(result.RouteValues, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValid_ReturnsRedirectToActionResultWhereRouteValuesContainsKeyForAccountingNumber()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult)await sut.ApplyPostingJournal(BuildApplyPostingJournalViewModel());

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.RouteValues["accountingNumber"], Is.Not.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournal_WhenApplyPostingJournalViewModelIsValid_ReturnsRedirectToActionResultWhereRouteValuesContainsValueForAccountingNumber()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            ApplyPostingJournalViewModel applyPostingJournalViewModel = BuildApplyPostingJournalViewModel(accountingNumber);
            RedirectToActionResult result = (RedirectToActionResult)await sut.ApplyPostingJournal(applyPostingJournalViewModel);

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.RouteValues["accountingNumber"], Is.EqualTo(accountingNumber));
            // ReSharper restore PossibleNullReferenceException
        }

        private Controller CreateSut(bool modelIsValid = true, string validationError = null, bool canModifyAccounting = true, string postingJournalKey = null, bool hasKeyValueEntryForPostingJournalKey = true, IKeyValueEntry keyValueEntryForPostingJournalKey = null, string postingJournalResultKey = null, bool hasKeyValueEntryForPostingJournalResultKey = true, IKeyValueEntry keyValueEntryForPostingJournalResultKey = null, IPostingJournalResult postingJournalResult = null)
        {
            postingJournalKey ??= _fixture.Create<string>();
            postingJournalResultKey ??= _fixture.Create<string>();

            _claimResolverMock.Setup(m => m.CanModifyAccounting(It.IsAny<int>()))
                .Returns(canModifyAccounting);

            _queryBusMock.Setup(m => m.QueryAsync<IGetUserSpecificKeyQuery, string>(It.Is<IGetUserSpecificKeyQuery>(query => query != null && query.KeyElementCollection != null && query.KeyElementCollection.Contains(nameof(ApplyPostingJournalViewModel)))))
                .Returns(Task.FromResult(postingJournalKey));
            _queryBusMock.Setup(m => m.QueryAsync<IPullKeyValueEntryQuery, IKeyValueEntry>(It.Is<IPullKeyValueEntryQuery>(query => query != null && string.CompareOrdinal(query.Key, postingJournalKey) == 0)))
                .Returns(Task.FromResult(hasKeyValueEntryForPostingJournalKey ? keyValueEntryForPostingJournalKey ?? BuildKeyValueEntryForPostingJournalKey() : null));
            _queryBusMock.Setup(m => m.QueryAsync<IGetUserSpecificKeyQuery, string>(It.Is<IGetUserSpecificKeyQuery>(query => query != null && query.KeyElementCollection != null && query.KeyElementCollection.Contains(nameof(ApplyPostingJournalResultViewModel)))))
                .Returns(Task.FromResult(postingJournalResultKey));
            _queryBusMock.Setup(m => m.QueryAsync<IPullKeyValueEntryQuery, IKeyValueEntry>(It.Is<IPullKeyValueEntryQuery>(query => query != null && string.CompareOrdinal(query.Key, postingJournalResultKey) == 0)))
                .Returns(Task.FromResult(hasKeyValueEntryForPostingJournalResultKey ? keyValueEntryForPostingJournalResultKey ?? BuildKeyValueEntryForPostingJournalResultKey() : null));
            _commandBusMock.Setup(m => m.PublishAsync<IApplyPostingJournalCommand, IPostingJournalResult>(It.IsAny<IApplyPostingJournalCommand>()))
                .Returns(Task.FromResult(postingJournalResult ?? BuildPostingJournalResult()));
            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<IPushKeyValueEntryCommand>()))
                .Returns(Task.CompletedTask);
            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<IDeleteKeyValueEntryCommand>()))
                .Returns(Task.CompletedTask);

            Controller controller = new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
            if (modelIsValid == false)
            {
                controller.ModelState.AddModelError(_fixture.Create<string>(), validationError ?? _fixture.Create<string>());
            }
            return controller;
        }

        private IKeyValueEntry BuildKeyValueEntryForPostingJournalKey(ApplyPostingJournalViewModel applyPostingJournalViewModel = null)
        {
            return BuildKeyValueEntryForPostingJournalKeyMock(applyPostingJournalViewModel).Object;
        }

        private Mock<IKeyValueEntry> BuildKeyValueEntryForPostingJournalKeyMock(ApplyPostingJournalViewModel applyPostingLineCollectionViewModel = null)
        {
            return _fixture.BuildKeyValueEntryMock(toObject: applyPostingLineCollectionViewModel ?? BuildApplyPostingJournalViewModel());
        }

        private ApplyPostingJournalViewModel BuildApplyPostingJournalViewModel(int? accountingNumber = null, ApplyPostingLineCollectionViewModel applyPostingLineCollectionViewModel = null)
        {
            return _fixture.Build<ApplyPostingJournalViewModel>()
                .With(m => m.AccountingNumber, accountingNumber ?? _fixture.Create<int>())
                .With(m => m.ApplyPostingLines, applyPostingLineCollectionViewModel ?? BuildApplyPostingLineCollectionViewModel(BuildPostingLineIdentifierCollection()))
                .Create();
        }

        private ApplyPostingLineCollectionViewModel BuildApplyPostingLineCollectionViewModel(params Guid[] applyPostingLineIdentifierCollection)
        {
            NullGuard.NotNull(applyPostingLineIdentifierCollection, nameof(applyPostingLineIdentifierCollection));

            ApplyPostingLineCollectionViewModel applyPostingLineCollection = new ApplyPostingLineCollectionViewModel();
            applyPostingLineCollection.AddRange(applyPostingLineIdentifierCollection.Select(BuildApplyPostingLineViewModel).ToArray());

            return applyPostingLineCollection;
        }

        private ApplyPostingLineViewModel BuildApplyPostingLineViewModel(Guid applyPostingLineIdentifier)
        {
            return _fixture.Build<ApplyPostingLineViewModel>()
                .With(m => m.Identifier, applyPostingLineIdentifier)
                .With(m => m.PostingDate, DateTime.Today.AddDays(_random.Next(0, 30) * -1))
                .With(m => m.SortOrder, _fixture.Create<int>())
                .Create();
        }

        private IKeyValueEntry BuildKeyValueEntryForPostingJournalResultKey(ApplyPostingJournalResultViewModel applyPostingJournalResultViewModel = null)
        {
            return BuildKeyValueEntryForPostingJournalResultKeyMock(applyPostingJournalResultViewModel).Object;
        }

        private Mock<IKeyValueEntry> BuildKeyValueEntryForPostingJournalResultKeyMock(ApplyPostingJournalResultViewModel applyPostingJournalResultViewModel = null)
        {
            return _fixture.BuildKeyValueEntryMock(toObject: applyPostingJournalResultViewModel ?? BuildApplyPostingJournalResultViewModel());
        }

        private ApplyPostingJournalResultViewModel BuildApplyPostingJournalResultViewModel(PostingLineCollectionViewModel postingLineCollectionViewModel = null, PostingWarningCollectionViewModel postingWarningCollectionViewModel = null)
        {
            return _fixture.Build<ApplyPostingJournalResultViewModel>()
                .With(m => m.PostingLines, postingLineCollectionViewModel ?? BuildPostingLineCollectionViewModel(BuildPostingLineIdentifierCollection()))
                .With(m => m.PostingWarnings, postingWarningCollectionViewModel ?? BuildPostingWarningCollectionViewModel(BuildPostingWarningIdentifierCollection()))
                .Create();
        }

        private PostingLineCollectionViewModel BuildPostingLineCollectionViewModel(params Guid[] postingLineIdentifierCollection)
        {
            NullGuard.NotNull(postingLineIdentifierCollection, nameof(postingLineIdentifierCollection));

            PostingLineCollectionViewModel postingLineCollectionViewModel = new PostingLineCollectionViewModel();
            postingLineCollectionViewModel.AddRange(postingLineIdentifierCollection.Select(BuildPostingLineViewModel).ToArray());
            return postingLineCollectionViewModel;
        }

        private PostingLineViewModel BuildPostingLineViewModel(Guid postingLineIdentifier)
        {
            return _fixture.Build<PostingLineViewModel>()
                .With(m => m.Identifier, postingLineIdentifier)
                .With(m => m.PostingDate, DateTime.Today.AddDays(_random.Next(0, 30) * -1))
                .With(m => m.SortOrder, _fixture.Create<int>())
                .Create();
        }

        private PostingWarningCollectionViewModel BuildPostingWarningCollectionViewModel(params Guid[] postingWarningIdentifierCollection)
        {
            NullGuard.NotNull(postingWarningIdentifierCollection, nameof(postingWarningIdentifierCollection));

            PostingWarningCollectionViewModel postingWarningCollectionViewModel = new PostingWarningCollectionViewModel();
            postingWarningCollectionViewModel.AddRange(postingWarningIdentifierCollection.Select(BuildPostingWarningViewModel).ToList());

            return postingWarningCollectionViewModel;
        }

        private PostingWarningViewModel BuildPostingWarningViewModel(Guid postingWarningIdentifier)
        {
            return _fixture.Build<PostingWarningViewModel>()
                .With(m => m.Identifier, postingWarningIdentifier)
                .With(m => m.PostingLine, BuildPostingLineViewModel(Guid.NewGuid()))
                .Create();
        }

        private IPostingJournalResult BuildPostingJournalResult(IPostingLineCollection postingLineCollection = null, IPostingWarningCollection postingWarningCollection = null)
        {
            return _fixture.BuildPostingJournalResultMock(postingLineCollection ?? BuildPostingLineCollection(BuildPostingLineIdentifierCollection()), postingWarningCollection ?? BuildPostingWarningCollection(BuildPostingWarning(), BuildPostingWarning(), BuildPostingWarning())).Object;
        }

        private IPostingLineCollection BuildPostingLineCollection(params Guid[] postingLineIdentifierCollection)
        {
            NullGuard.NotNull(postingLineIdentifierCollection, nameof(postingLineIdentifierCollection));

            return _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineIdentifierCollection.Select(BuildPostingLine).ToArray()).Object;
        }

        private IPostingLine BuildPostingLine(Guid postingLineIdentifier)
        {
            return _fixture.BuildPostingLineMock(postingLineIdentifier, DateTime.Today.AddDays(_random.Next(0, 30) * -1), sortOrder: _fixture.Create<int>()).Object;
        }

        private IPostingWarningCollection BuildPostingWarningCollection(params IPostingWarning[] postingWarningCollection)
        {
            NullGuard.NotNull(postingWarningCollection, nameof(postingWarningCollection));

            return _fixture.BuildPostingWarningCollectionMock(postingWarningCollection).Object;
        }

        private IPostingWarning BuildPostingWarning()
        {
            return _fixture.BuildPostingWarningMock().Object;
        }

        private Guid[] BuildPostingLineIdentifierCollection()
        {
            return _fixture.CreateMany<Guid>(_random.Next(5, 10)).ToArray();
        }

        private Guid[] BuildPostingWarningIdentifierCollection()
        {
            return _fixture.CreateMany<Guid>(_random.Next(5, 10)).ToArray();
        }
    }
}