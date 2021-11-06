using System;
using System.Linq;
using System.Text.Json;
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
    public class AddPostingLineToPostingJournalTests
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
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyIsNull_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), null, BuildPostingLineJson());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyIsNull_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), null, BuildPostingLineJson());

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyIsEmpty_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), string.Empty, BuildPostingLineJson());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyIsEmpty_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), string.Empty, BuildPostingLineJson());

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyIsWhiteSpace_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), " ", BuildPostingLineJson());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyIsWhiteSpace_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), " ", BuildPostingLineJson());

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingLineIsNull_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingLineIsNull_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), null);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingLineIsEmpty_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), string.Empty);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingLineIsEmpty_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), string.Empty);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingLineIsWhiteSpace_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), " ");

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingLineIsWhiteSpace_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), " ");

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingLineCannotBeDeserializedToApplyPostingLineViewModel_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), _fixture.Create<string>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingLineCannotBeDeserializedToApplyPostingLineViewModel_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingLineIsInvalid_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            string postingLine = BuildPostingLineJson(hasDetails: false);
            IActionResult result = await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), postingLine);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingLineIsInvalid_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            string postingLine = BuildPostingLineJson(hasDetails: false);
            IActionResult result = await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), postingLine);

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingLineIsInvalid_ReturnsBadRequestObjectResultWhereValueIsNotNull()
        {
            Controller sut = CreateSut();

            string postingLine = BuildPostingLineJson(hasDetails: false);
            BadRequestObjectResult result = (BadRequestObjectResult)await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), postingLine);

            Assert.That(result.Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingLineIsInvalid_ReturnsBadRequestObjectResultWhereValueIsString()
        {
            Controller sut = CreateSut();

            string postingLine = BuildPostingLineJson(hasDetails: false);
            BadRequestObjectResult result = (BadRequestObjectResult)await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), postingLine);

            Assert.That(result.Value, Is.TypeOf<string>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingLineIsInvalid_ReturnsBadRequestObjectResultWhereValueIsStringStartingWithSubmittedMessageIsInvalid()
        {
            Controller sut = CreateSut();

            string postingLine = BuildPostingLineJson(hasDetails: false);
            BadRequestObjectResult result = (BadRequestObjectResult)await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), postingLine);

            string errorMessage = (string)result.Value;

            Assert.That(errorMessage.StartsWith("The submitted message is invalid:"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValid_AssertQueryAsyncWasCalledOnQueryBusWithPullKeyValueEntryQueryForPostingJournalKey()
        {
            Controller sut = CreateSut();

            string postingJournalKey = _fixture.Create<string>();
            await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), postingJournalKey, BuildPostingLineJson());

            _queryBusMock.Verify(m => m.QueryAsync<IPullKeyValueEntryQuery, IKeyValueEntry>(It.Is<IPullKeyValueEntryQuery>(query => query != null && string.CompareOrdinal(query.Key, postingJournalKey) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValidAndKeyValueEntryForPostingJournalWasReturnedFromQueryBus_AssertToObjectWasCalledOnKeyValueEntryForPostingJournal()
        {
            Mock<IKeyValueEntry> keyValueEntryForPostingJournalMock = BuildKeyValueEntryForPostingJournalMock();
            Controller sut = CreateSut(keyValueEntryForPostingJournal: keyValueEntryForPostingJournalMock.Object);

            await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), BuildPostingLineJson());

            keyValueEntryForPostingJournalMock.Verify(m => m.ToObject<It.IsSubtype<ApplyPostingJournalViewModel>>(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValidAndNoKeyValueEntryForPostingJournalWasReturnedFromQueryBus_AssertPublishAsyncWasCalledOnCommandBusWithPushKeyValueEntryCommandForPostingJournal()
        {
            Controller sut = CreateSut(false);

            int accountingNumber = _fixture.Create<int>();
            string postingJournalKey = _fixture.Create<string>();
            Guid postingLineIdentifier = Guid.NewGuid();
            string postingLine = BuildPostingLineJson(postingLineIdentifier);
            await sut.AddPostingLineToPostingJournal(accountingNumber, postingJournalKey, postingLine);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IPushKeyValueEntryCommand>(command => command != null && string.CompareOrdinal(command.Key, postingJournalKey) == 0 && command.Value != null && command.Value.GetType() == typeof(ApplyPostingJournalViewModel) && ((ApplyPostingJournalViewModel)command.Value).AccountingNumber == accountingNumber && ((ApplyPostingJournalViewModel)command.Value).ApplyPostingLines != null && ((ApplyPostingJournalViewModel)command.Value).ApplyPostingLines.Count == 1 && ((ApplyPostingJournalViewModel)command.Value).ApplyPostingLines.Any(applyPostingLine => applyPostingLine.Identifier == postingLineIdentifier))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValidAndNoKeyValueEntryForPostingJournalWasReturnedFromQueryBus_ReturnsNotNull()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), BuildPostingLineJson());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValidAndNoKeyValueEntryForPostingJournalWasReturnedFromQueryBus_ReturnsPartialViewResult()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), BuildPostingLineJson());

            Assert.That(result, Is.TypeOf<PartialViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValidAndNoKeyValueEntryForPostingJournalWasReturnedFromQueryBus_ReturnsPartialViewResultWhereViewNameIsNotNull()
        {
            Controller sut = CreateSut(false);

            PartialViewResult result = (PartialViewResult)await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), BuildPostingLineJson());

            Assert.That(result.ViewName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValidAndNoKeyValueEntryForPostingJournalWasReturnedFromQueryBus_ReturnsPartialViewResultWhereViewNameIsEqualToPostingJournalPartial()
        {
            Controller sut = CreateSut(false);

            PartialViewResult result = (PartialViewResult)await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), BuildPostingLineJson());

            Assert.That(result.ViewName, Is.EqualTo("_PostingJournalPartial"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValidAndNoKeyValueEntryForPostingJournalWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsNotNull()
        {
            Controller sut = CreateSut(false);

            PartialViewResult result = (PartialViewResult)await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), BuildPostingLineJson());

            Assert.That(result.Model, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValidAndNoKeyValueEntryForPostingJournalWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsApplyPostingJournalViewModel()
        {
            Controller sut = CreateSut(false);

            PartialViewResult result = (PartialViewResult)await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), BuildPostingLineJson());

            Assert.That(result.Model, Is.TypeOf<ApplyPostingJournalViewModel>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValidAndNoKeyValueEntryForPostingJournalWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsApplyPostingJournalViewModelWithAccountingNumberEqualToAccountingNumberFromArguments()
        {
            Controller sut = CreateSut(false);

            int accountingNumber = _fixture.Create<int>();
            PartialViewResult result = (PartialViewResult)await sut.AddPostingLineToPostingJournal(accountingNumber, _fixture.Create<string>(), BuildPostingLineJson());

            ApplyPostingJournalViewModel applyPostingJournalViewModel = (ApplyPostingJournalViewModel)result.Model;

            Assert.That(applyPostingJournalViewModel.AccountingNumber, Is.EqualTo(accountingNumber));
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValidAndNoKeyValueEntryForPostingJournalWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsApplyPostingJournalViewModelWithApplyPostingLinesNotEqualToNull()
        {
            Controller sut = CreateSut(false);

            int accountingNumber = _fixture.Create<int>();
            PartialViewResult result = (PartialViewResult)await sut.AddPostingLineToPostingJournal(accountingNumber, _fixture.Create<string>(), BuildPostingLineJson());

            ApplyPostingJournalViewModel applyPostingJournalViewModel = (ApplyPostingJournalViewModel)result.Model;

            Assert.That(applyPostingJournalViewModel.ApplyPostingLines, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValidAndNoKeyValueEntryForPostingJournalWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsApplyPostingJournalViewModelWithApplyPostingLinesNotEmpty()
        {
            Controller sut = CreateSut(false);

            int accountingNumber = _fixture.Create<int>();
            PartialViewResult result = (PartialViewResult)await sut.AddPostingLineToPostingJournal(accountingNumber, _fixture.Create<string>(), BuildPostingLineJson());

            ApplyPostingJournalViewModel applyPostingJournalViewModel = (ApplyPostingJournalViewModel)result.Model;

            Assert.That(applyPostingJournalViewModel.ApplyPostingLines, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValidAndNoKeyValueEntryForPostingJournalWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsApplyPostingJournalViewModelWithApplyPostingLinesContainingOneItem()
        {
            Controller sut = CreateSut(false);

            int accountingNumber = _fixture.Create<int>();
            PartialViewResult result = (PartialViewResult)await sut.AddPostingLineToPostingJournal(accountingNumber, _fixture.Create<string>(), BuildPostingLineJson());

            ApplyPostingJournalViewModel applyPostingJournalViewModel = (ApplyPostingJournalViewModel)result.Model;

            Assert.That(applyPostingJournalViewModel.ApplyPostingLines.Count, Is.EqualTo(1));
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValidAndNoKeyValueEntryForPostingJournalWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsApplyPostingJournalViewModelWithApplyPostingLinesContainingPostingLineFromArguments()
        {
            Controller sut = CreateSut(false);

            Guid postingLineIdentifier = Guid.NewGuid();
            string postingLineJson = BuildPostingLineJson(postingLineIdentifier);
            PartialViewResult result = (PartialViewResult)await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), postingLineJson);

            ApplyPostingJournalViewModel applyPostingJournalViewModel = (ApplyPostingJournalViewModel)result.Model;

            Assert.That(applyPostingJournalViewModel.ApplyPostingLines.SingleOrDefault(applyPostingLine => applyPostingLine.Identifier == postingLineIdentifier), Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValidAndKeyValueEntryForPostingJournalWasReturnedFromQueryBus_AssertPublishAsyncWasCalledOnCommandBusWithPushKeyValueEntryCommandForPostingJournal()
        {
            int accountingNumber = _fixture.Create<int>();
            ApplyPostingLineCollectionViewModel applyPostingLineCollectionViewModel = BuildApplyPostingLineCollectionViewModel(_fixture.Create<ApplyPostingLineViewModel>(), _fixture.Create<ApplyPostingLineViewModel>());
            ApplyPostingJournalViewModel applyPostingJournalViewModel = BuildApplyPostingJournalViewModel(accountingNumber, applyPostingLineCollectionViewModel);
            IKeyValueEntry keyValueEntryForPostingJournal = BuildKeyValueEntryForPostingJournal(applyPostingJournalViewModel);
            Controller sut = CreateSut(keyValueEntryForPostingJournal: keyValueEntryForPostingJournal);

            string postingJournalKey = _fixture.Create<string>();
            Guid postingLineIdentifier = Guid.NewGuid();
            string postingLine = BuildPostingLineJson(postingLineIdentifier);
            await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), postingJournalKey, postingLine);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IPushKeyValueEntryCommand>(command => command != null && string.CompareOrdinal(command.Key, postingJournalKey) == 0 && command.Value != null && command.Value.GetType() == typeof(ApplyPostingJournalViewModel) && ((ApplyPostingJournalViewModel)command.Value).AccountingNumber == accountingNumber && ((ApplyPostingJournalViewModel)command.Value).ApplyPostingLines != null && ((ApplyPostingJournalViewModel)command.Value).ApplyPostingLines.Count == 3 && ((ApplyPostingJournalViewModel)command.Value).ApplyPostingLines.Any(applyPostingLine => applyPostingLine.Identifier == postingLineIdentifier))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValidAndKeyValueEntryForPostingJournalWasReturnedFromQueryBus_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), BuildPostingLineJson());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValidAndKeyValueEntryForPostingJournalWasReturnedFromQueryBus_ReturnsPartialViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), BuildPostingLineJson());

            Assert.That(result, Is.TypeOf<PartialViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValidAndKeyValueEntryForPostingJournalWasReturnedFromQueryBus_ReturnsPartialViewResultWhereViewNameIsNotNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult)await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), BuildPostingLineJson());

            Assert.That(result.ViewName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValidAndKeyValueEntryForPostingJournalWasReturnedFromQueryBus_ReturnsPartialViewResultWhereViewNameIsEqualToPostingJournalPartial()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult)await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), BuildPostingLineJson());

            Assert.That(result.ViewName, Is.EqualTo("_PostingJournalPartial"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValidAndKeyValueEntryForPostingJournalWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsNotNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult)await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), BuildPostingLineJson());

            Assert.That(result.Model, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValidAndKeyValueEntryForPostingJournalWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsApplyPostingJournalViewModel()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult)await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), BuildPostingLineJson());

            Assert.That(result.Model, Is.TypeOf<ApplyPostingJournalViewModel>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValidAndKeyValueEntryForPostingJournalWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsApplyPostingJournalViewModelWithAccountingNumberEqualToAccountingNumberFromReturnedPostingJournal()
        {
            int accountingNumber = _fixture.Create<int>();
            ApplyPostingJournalViewModel savedApplyPostingJournalViewModel = BuildApplyPostingJournalViewModel(accountingNumber);
            IKeyValueEntry keyValueEntryForPostingJournal = BuildKeyValueEntryForPostingJournal(savedApplyPostingJournalViewModel);
            Controller sut = CreateSut(keyValueEntryForPostingJournal: keyValueEntryForPostingJournal);

            PartialViewResult result = (PartialViewResult)await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), BuildPostingLineJson());

            ApplyPostingJournalViewModel applyPostingJournalViewModel = (ApplyPostingJournalViewModel)result.Model;

            Assert.That(applyPostingJournalViewModel.AccountingNumber, Is.EqualTo(accountingNumber));
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValidAndKeyValueEntryForPostingJournalWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsApplyPostingJournalViewModelWithApplyPostingLinesNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult)await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), BuildPostingLineJson());

            ApplyPostingJournalViewModel applyPostingJournalViewModel = (ApplyPostingJournalViewModel)result.Model;

            Assert.That(applyPostingJournalViewModel.ApplyPostingLines, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValidAndKeyValueEntryForPostingJournalWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsApplyPostingJournalViewModelWithApplyPostingLinesNotEmpty()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult)await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), BuildPostingLineJson());

            ApplyPostingJournalViewModel applyPostingJournalViewModel = (ApplyPostingJournalViewModel)result.Model;

            Assert.That(applyPostingJournalViewModel.ApplyPostingLines, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValidAndKeyValueEntryForPostingJournalWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsApplyPostingJournalViewModelWithApplyPostingLinesContainingExistingItemsPlusOne()
        {
            ApplyPostingLineCollectionViewModel applyPostingLineCollectionViewModel = BuildApplyPostingLineCollectionViewModel(
                _fixture.Create<ApplyPostingLineViewModel>(),
                _fixture.Create<ApplyPostingLineViewModel>());
            ApplyPostingJournalViewModel savedApplyPostingJournalViewModel = BuildApplyPostingJournalViewModel(applyPostingLineCollectionViewModel: applyPostingLineCollectionViewModel);
            IKeyValueEntry keyValueEntryForPostingJournal = BuildKeyValueEntryForPostingJournal(savedApplyPostingJournalViewModel);
            Controller sut = CreateSut(keyValueEntryForPostingJournal: keyValueEntryForPostingJournal);

            PartialViewResult result = (PartialViewResult)await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), BuildPostingLineJson());

            ApplyPostingJournalViewModel applyPostingJournalViewModel = (ApplyPostingJournalViewModel)result.Model;

            Assert.That(applyPostingJournalViewModel.ApplyPostingLines.Count, Is.EqualTo(3));
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValidAndKeyValueEntryForPostingJournalWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsApplyPostingJournalViewModelWithApplyPostingLinesContainingPostingLineFromArguments()
        {
            ApplyPostingLineCollectionViewModel applyPostingLineCollectionViewModel = BuildApplyPostingLineCollectionViewModel(
                _fixture.Create<ApplyPostingLineViewModel>(),
                _fixture.Create<ApplyPostingLineViewModel>());
            ApplyPostingJournalViewModel savedApplyPostingJournalViewModel = BuildApplyPostingJournalViewModel(applyPostingLineCollectionViewModel: applyPostingLineCollectionViewModel);
            IKeyValueEntry keyValueEntryForPostingJournal = BuildKeyValueEntryForPostingJournal(savedApplyPostingJournalViewModel);
            Controller sut = CreateSut(keyValueEntryForPostingJournal: keyValueEntryForPostingJournal);

            Guid postingLineIdentifier = Guid.NewGuid();
            string postingLineJson = BuildPostingLineJson(postingLineIdentifier);
            PartialViewResult result = (PartialViewResult)await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), postingLineJson);

            ApplyPostingJournalViewModel applyPostingJournalViewModel = (ApplyPostingJournalViewModel)result.Model;

            Assert.That(applyPostingJournalViewModel.ApplyPostingLines.SingleOrDefault(applyPostingLine => applyPostingLine.Identifier == postingLineIdentifier), Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValidAndKeyValueEntryForPostingJournalWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsApplyPostingJournalViewModelWithSortedApplyPostingLines()
        {
            ApplyPostingLineCollectionViewModel applyPostingLineCollectionViewModel = BuildApplyPostingLineCollectionViewModel(
                _fixture.Create<ApplyPostingLineViewModel>(),
                _fixture.Create<ApplyPostingLineViewModel>(),
                _fixture.Create<ApplyPostingLineViewModel>(),
                _fixture.Create<ApplyPostingLineViewModel>(),
                _fixture.Create<ApplyPostingLineViewModel>(),
                _fixture.Create<ApplyPostingLineViewModel>());
            ApplyPostingJournalViewModel savedApplyPostingJournalViewModel = BuildApplyPostingJournalViewModel(applyPostingLineCollectionViewModel: applyPostingLineCollectionViewModel);
            IKeyValueEntry keyValueEntryForPostingJournal = BuildKeyValueEntryForPostingJournal(savedApplyPostingJournalViewModel);
            Controller sut = CreateSut(keyValueEntryForPostingJournal: keyValueEntryForPostingJournal);

            PartialViewResult result = (PartialViewResult)await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), BuildPostingLineJson());

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
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValid_ReturnsPartialViewResultWhereViewDataIsNotEqualToNull()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            PartialViewResult result = (PartialViewResult)await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), BuildPostingLineJson());

            Assert.That(result.ViewData, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValid_ReturnsPartialViewResultWhereViewDataIsNotEmpty()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            PartialViewResult result = (PartialViewResult)await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), BuildPostingLineJson());

            Assert.That(result.ViewData, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValid_ReturnsPartialViewResultWhereViewDataContainingKeyForPostingJournalKey()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            PartialViewResult result = (PartialViewResult)await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), BuildPostingLineJson());

            Assert.That(result.ViewData.ContainsKey("PostingJournalKey"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValid_ReturnsPartialViewResultWhereViewDataContainingPostingJournalKeyWithValueNotEqualToNull()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            PartialViewResult result = (PartialViewResult)await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), BuildPostingLineJson());

            Assert.That(result.ViewData["PostingJournalKey"], Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValid_ReturnsPartialViewResultWhereViewDataContainingPostingJournalKeyWithValueEqualToPostingJournalKeyFromArguments()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            string postingJournalKey = _fixture.Create<string>();
            PartialViewResult result = (PartialViewResult)await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), postingJournalKey, BuildPostingLineJson());

            Assert.That(result.ViewData["PostingJournalKey"], Is.EqualTo(postingJournalKey));
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValidAndPostingJournalHeaderIsNull_ReturnsPartialViewResultWhereWithViewDataDoesNotContainKeyForHeader()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            PartialViewResult result = (PartialViewResult)await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), BuildPostingLineJson());

            Assert.That(result.ViewData.ContainsKey("Header"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValidAndPostingJournalHeaderIsEmpty_ReturnsPartialViewResultWhereWithViewDataDoesNotContainKeyForHeader()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            PartialViewResult result = (PartialViewResult)await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), BuildPostingLineJson(), string.Empty);

            Assert.That(result.ViewData.ContainsKey("Header"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValidAndPostingJournalHeaderIsWhiteSpace_ReturnsPartialViewResultWhereWithViewDataDoesNotContainKeyForHeader()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            PartialViewResult result = (PartialViewResult)await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), BuildPostingLineJson(), " ");

            Assert.That(result.ViewData.ContainsKey("Header"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValidAndPostingJournalHeaderIsNotNullEmptyOrWhiteSpace_ReturnsPartialViewResultWhereWithViewContainingKeyForHeader()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            PartialViewResult result = (PartialViewResult)await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), BuildPostingLineJson(), _fixture.Create<string>());

            Assert.That(result.ViewData.ContainsKey("Header"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValidAndPostingJournalHeaderIsNotNullEmptyOrWhiteSpace_ReturnsPartialViewResultWhereWithViewContainingHeaderWithValueNotEqualToNull()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            PartialViewResult result = (PartialViewResult)await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), BuildPostingLineJson(), _fixture.Create<string>());

            Assert.That(result.ViewData["Header"], Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddPostingLineToPostingJournal_WhenPostingJournalKeyHasValueAndPostingLineIsValidAndPostingJournalHeaderIsNotNullEmptyOrWhiteSpace_ReturnsPartialViewResultWhereWithViewContainingHeaderWithValueEqualToPostingJournalHeaderFromArguments()
        {
            Controller sut = CreateSut(_random.Next(100) > 50);

            string postingJournalHeader = _fixture.Create<string>();
            PartialViewResult result = (PartialViewResult)await sut.AddPostingLineToPostingJournal(_fixture.Create<int>(), _fixture.Create<string>(), BuildPostingLineJson(), postingJournalHeader);

            Assert.That(result.ViewData["Header"], Is.EqualTo(postingJournalHeader));
        }

        private Controller CreateSut(bool hasKeyValueEntryForPostingJournal = true, IKeyValueEntry keyValueEntryForPostingJournal = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<IPullKeyValueEntryQuery, IKeyValueEntry>(It.IsAny<IPullKeyValueEntryQuery>()))
                .Returns(Task.FromResult(hasKeyValueEntryForPostingJournal ? keyValueEntryForPostingJournal ?? BuildKeyValueEntryForPostingJournal() : null));
            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<IPushKeyValueEntryCommand>()))
                .Returns(Task.CompletedTask);

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
        }

        private string BuildPostingLineJson(Guid? identifier = null, bool hasDetails = true)
        {
            ApplyPostingLineViewModel postingLine = _fixture.Build<ApplyPostingLineViewModel>()
                .With(m => m.Identifier, identifier)
                .With(m => m.PostingDate, DateTimeOffset.Now.AddDays(_random.Next(0, 7) * -1).Date)
                .With(m => m.Reference, _random.Next(100) > 50 ? _fixture.Create<string>().Substring(0, 16) : null)
                .With(m => m.AccountNumber, _fixture.Create<string>().Substring(0, 16).ToUpper())
                .With(m => m.Details, hasDetails ? _fixture.Create<string>() : null)
                .With(m => m.BudgetAccountNumber, _random.Next(100) > 50 ? _fixture.Create<string>().Substring(0, 16).ToUpper() : null)
                .With(m => m.Debit, _random.Next(100) > 50 ? Math.Abs(_fixture.Create<decimal>()) : null)
                .With(m => m.Credit, _random.Next(100) > 50 ? Math.Abs(_fixture.Create<decimal>()) : null)
                .With(m => m.ContactAccountNumber, _random.Next(100) > 50 ? _fixture.Create<string>().Substring(0, 16).ToUpper() : null)
                .With(m => m.SortOrder, (int?)null)
                .Create();

            return JsonSerializer.Serialize(postingLine);
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
                .With(m => m.ApplyPostingLines, applyPostingLineCollectionViewModel ?? BuildApplyPostingLineCollectionViewModel())
                .Create();
        }

        private ApplyPostingLineCollectionViewModel BuildApplyPostingLineCollectionViewModel(params ApplyPostingLineViewModel[] applyPostingLineViewModelCollection)
        {
            NullGuard.NotNull(applyPostingLineViewModelCollection, nameof(applyPostingLineViewModelCollection));

            ApplyPostingLineCollectionViewModel applyPostingLineCollectionViewModel = new ApplyPostingLineCollectionViewModel();
            applyPostingLineCollectionViewModel.AddRange(applyPostingLineViewModelCollection);

            return applyPostingLineCollectionViewModel;
        }
    }
}