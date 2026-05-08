using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Queries;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.WebApi.Models.Accounting;
using OSDevGrp.OSIntranet.WebApi.Tests.Helpers.Factories;
using Controller=OSDevGrp.OSIntranet.WebApi.Controllers.AccountingController;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Controllers.AccountingController
{
    [TestFixture]
    public class SavePostingJournalAsyncTests : ApplyPostingJournalAsyncTestBase
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        protected override Fixture Fixture => _fixture;

        protected override Random Random => _random;

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void SavePostingJournalAsync_WhenApplyPostingJournalModelIsNull_ThrowsIntranetValidationException()
        {
            Controller sut = CreateSut();

            Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.SavePostingJournalAsync(_fixture.Create<int>(), null));
        }

        [Test]
        [Category("UnitTest")]
        public void SavePostingJournalAsync_WhenApplyPostingJournalModelIsNull_ThrowsIntranetValidationExceptionWhereErrorCodeIsEqualToValueCannotBeNull()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.SavePostingJournalAsync(_fixture.Create<int>(), null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueCannotBeNull));
        }

        [Test]
        [Category("UnitTest")]
        public void SavePostingJournalAsync_WhenApplyPostingJournalModelIsNull_ThrowsIntranetValidationExceptionWhereValidatingTypeIsTypeOfApplyPostingJournalModel()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.SavePostingJournalAsync(_fixture.Create<int>(), null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ValidatingType, Is.EqualTo(typeof(ApplyPostingJournalModel)));
        }

        [Test]
        [Category("UnitTest")]
        public void SavePostingJournalAsync_WhenApplyPostingJournalModelIsNull_ThrowsIntranetValidationExceptionWhereValidatingFieldIsEqualToPostingJournal()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.SavePostingJournalAsync(_fixture.Create<int>(), null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ValidatingField, Is.EqualTo("postingJournal"));
        }

        [Test]
        [Category("UnitTest")]
        public void SavePostingJournalAsync_WhenApplyPostingJournalModelIsInvalid_ThrowsIntranetValidationException()
        {
            Controller sut = CreateSut(modelIsValid: false);

            Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.SavePostingJournalAsync(_fixture.Create<int>(), CreateApplyPostingJournalModel()));
        }

        [Test]
        [Category("UnitTest")]
        public void SavePostingJournalAsync_WhenApplyPostingJournalModelIsInvalid_ThrowsIntranetValidationExceptionWhereErrorCodeIsEqualToSubmittedMessageInvalid()
        {
            Controller sut = CreateSut(modelIsValid: false);

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.SavePostingJournalAsync(_fixture.Create<int>(), CreateApplyPostingJournalModel()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.SubmittedMessageInvalid));
        }

        [Test]
        [Category("UnitTest")]
        public async Task SavePostingJournalAsync_WhenApplyPostingJournalModelIsValid_AssertQueryAsyncWasCalledOnQueryBusWithGetUserSpecificKeyQuery()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            await sut.SavePostingJournalAsync(accountingNumber, CreateApplyPostingJournalModel());

            _queryBusMock.Verify(m => m.QueryAsync<IGetUserSpecificKeyQuery, string>(It.Is<IGetUserSpecificKeyQuery>(query => query != null && query.KeyElementCollection != null && query.KeyElementCollection.Count() == 2 && string.CompareOrdinal(query.KeyElementCollection.ElementAtOrDefault(0), nameof(ApplyPostingJournalModel)) == 0 && string.CompareOrdinal(query.KeyElementCollection.ElementAt(1), Convert.ToString(accountingNumber)) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task SavePostingJournalAsync_WhenApplyPostingJournalModelIsValid_AssertPublishAsyncWasCalledOnCommandBusWithPushKeyValueEntryCommandWhereKeyIsResolvedPostingJournalKey()
        {
            string postingJournalKey = _fixture.Create<string>();
            Controller sut = CreateSut(postingJournalKey: postingJournalKey);

            await sut.SavePostingJournalAsync(_fixture.Create<int>(), CreateApplyPostingJournalModel());

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IPushKeyValueEntryCommand>(command => command != null && string.CompareOrdinal(command.Key, postingJournalKey) == 0 && command.Value != null)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task SavePostingJournalAsync_WhenApplyPostingJournalModelIsValid_AssertPublishAsyncWasCalledOnCommandBusWithPushKeyValueEntryCommandWhereAccountingNumberIsEqualToRouteValue()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            ApplyPostingJournalModel postingJournalModel = CreateApplyPostingJournalModel(_fixture.Create<int>());
            await sut.SavePostingJournalAsync(accountingNumber, postingJournalModel);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IPushKeyValueEntryCommand>(command => command != null && command.Value != null && command.Value.GetType() == typeof(ApplyPostingJournalModel) && ((ApplyPostingJournalModel)command.Value).AccountingNumber == accountingNumber)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task SavePostingJournalAsync_WhenApplyPostingJournalModelIsValid_AssertPublishAsyncWasCalledOnCommandBusWithPushKeyValueEntryCommandWhereApplyPostingLinesAreOrderedByPostingDateDescendingAndSortOrderDescending()
        {
            Controller sut = CreateSut();

            ApplyPostingLineModel applyPostingLineModel1 = CreateApplyPostingLineModel(Guid.NewGuid(), new DateTimeOffset(2026, 1, 10, 9, 0, 0, TimeSpan.Zero), 1);
            ApplyPostingLineModel applyPostingLineModel2 = CreateApplyPostingLineModel(Guid.NewGuid(), new DateTimeOffset(2026, 1, 12, 9, 0, 0, TimeSpan.Zero), 1);
            ApplyPostingLineModel applyPostingLineModel3 = CreateApplyPostingLineModel(Guid.NewGuid(), new DateTimeOffset(2026, 1, 12, 9, 0, 0, TimeSpan.Zero), 3);
            ApplyPostingJournalModel postingJournalModel = CreateApplyPostingJournalModel(applyPostingLineCollectionModel: CreateApplyPostingLineCollectionModel(new[] { applyPostingLineModel1, applyPostingLineModel2, applyPostingLineModel3 }));

            await sut.SavePostingJournalAsync(_fixture.Create<int>(), postingJournalModel);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IPushKeyValueEntryCommand>(command => command != null && command.Value != null && command.Value.GetType() == typeof(ApplyPostingJournalModel) && ((ApplyPostingJournalModel)command.Value).ApplyPostingLines != null && ((ApplyPostingJournalModel)command.Value).ApplyPostingLines.Select(applyPostingLine => applyPostingLine.Identifier).SequenceEqual(new[] { applyPostingLineModel3.Identifier, applyPostingLineModel2.Identifier, applyPostingLineModel1.Identifier }))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task SavePostingJournalAsync_WhenApplyPostingJournalModelIsValid_ReturnsActionResultWhereResultIsOkObjectResult()
        {
            Controller sut = CreateSut();

            ActionResult<ApplyPostingJournalModel> result = await sut.SavePostingJournalAsync(_fixture.Create<int>(), CreateApplyPostingJournalModel());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task SavePostingJournalAsync_WhenApplyPostingJournalModelIsValid_ReturnsOkObjectResultWhereValueIsApplyPostingJournalModelWithOrderedApplyPostingLines()
        {
            Controller sut = CreateSut();

            ApplyPostingLineModel applyPostingLineModel1 = CreateApplyPostingLineModel(Guid.NewGuid(), new DateTimeOffset(2026, 2, 1, 9, 0, 0, TimeSpan.Zero), 0);
            ApplyPostingLineModel applyPostingLineModel2 = CreateApplyPostingLineModel(Guid.NewGuid(), new DateTimeOffset(2026, 2, 3, 9, 0, 0, TimeSpan.Zero), 0);
            ApplyPostingLineModel applyPostingLineModel3 = CreateApplyPostingLineModel(Guid.NewGuid(), new DateTimeOffset(2026, 2, 3, 9, 0, 0, TimeSpan.Zero), 4);
            ApplyPostingJournalModel postingJournalModel = CreateApplyPostingJournalModel(applyPostingLineCollectionModel: CreateApplyPostingLineCollectionModel(new[] { applyPostingLineModel1, applyPostingLineModel2, applyPostingLineModel3 }));

            OkObjectResult result = (OkObjectResult)(await sut.SavePostingJournalAsync(_fixture.Create<int>(), postingJournalModel)).Result;

            ApplyPostingJournalModel value = (ApplyPostingJournalModel)result.Value;

            Assert.That(value, Is.Not.Null);
            Assert.That(value.ApplyPostingLines.Select(applyPostingLine => applyPostingLine.Identifier), Is.EqualTo(new[] { applyPostingLineModel3.Identifier, applyPostingLineModel2.Identifier, applyPostingLineModel1.Identifier }));
        }

        private Controller CreateSut(string postingJournalKey = null, bool modelIsValid = true, string errorKey = null, string errorMessage = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<IGetUserSpecificKeyQuery, string>(It.IsAny<IGetUserSpecificKeyQuery>()))
                .Returns(Task.FromResult(postingJournalKey ?? _fixture.Create<string>()));
            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<IPushKeyValueEntryCommand>()))
                .Returns(Task.CompletedTask);

            Controller controller = new Controller(_commandBusMock.Object, _queryBusMock.Object, ConverterFactoryCreator.Create());
            if (modelIsValid == false)
            {
                controller.ModelState.AddModelError(errorKey ?? _fixture.Create<string>(), errorMessage ?? _fixture.Create<string>());
            }

            return controller;
        }

        private static ApplyPostingLineModel CreateApplyPostingLineModel(Guid identifier, DateTimeOffset postingDate, int sortOrder)
        {
            return new ApplyPostingLineModel
            {
                Identifier = identifier,
                PostingDate = postingDate,
                AccountNumber = "1000",
                Details = "Test",
                Debit = 100M,
                SortOrder = sortOrder
            };
        }
    }
}