using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.WebApi.Models.Accounting;
using Controller=OSDevGrp.OSIntranet.WebApi.Controllers.AccountingController;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Controllers.AccountingController
{
    [TestFixture]
    public class ApplyPostingJournalAsyncWithApplyPostingJournalModelTests : ApplyPostingJournalAsyncTestBase
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
            _fixture.Customize<IPostingLine>(builder => builder.FromFactory(() => _fixture.BuildPostingLineMock().Object));
            _fixture.Customize<IPostingWarning>(builder => builder.FromFactory(() => _fixture.BuildPostingWarningMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyPostingJournalAsync_WhenApplyPostingJournalModelIsNull_ThrowsIntranetValidationException()
        {
            Controller sut = CreateSut();

            Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.ApplyPostingJournalAsync(null));
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyPostingJournalAsync_WhenApplyPostingJournalModelIsNull_ThrowsIntranetValidationExceptionWhereErrorCodeIsEqualToValueCannotBeNull()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.ApplyPostingJournalAsync(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueCannotBeNull));
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyPostingJournalAsync_WhenApplyPostingJournalModelIsNull_ThrowsIntranetValidationExceptionWhereValidatingTypeIsTypeOfApplyPostingJournalModel()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.ApplyPostingJournalAsync(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ValidatingType, Is.EqualTo(typeof(ApplyPostingJournalModel)));
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyPostingJournalAsync_WhenApplyPostingJournalModelIsNull_ThrowsIntranetValidationExceptionWhereValidatingFieldIsEqualToApplyPostingJournal()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.ApplyPostingJournalAsync(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ValidatingField, Is.EqualTo("applyPostingJournal"));
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyPostingJournalAsync_WhenApplyPostingJournalModelIsInvalid_ThrowsIntranetValidationException()
        {
            Controller sut = CreateSut(modelIsValid: false);

            Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.ApplyPostingJournalAsync(CreateApplyPostingJournalModel()));
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyPostingJournalAsync_WhenApplyPostingJournalModelIsInvalid_ThrowsIntranetValidationExceptionWhereErrorCodeIsEqualToValueCannotBeNull()
        {
            Controller sut = CreateSut(modelIsValid: false);

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.ApplyPostingJournalAsync(CreateApplyPostingJournalModel()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.SubmittedMessageInvalid));
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyPostingJournalAsync_WhenApplyPostingJournalModelIsInvalid_ThrowsIntranetValidationExceptionWhereMessageContainsErrorKey()
        {
            string errorKey = _fixture.Create<string>();
            Controller sut = CreateSut(modelIsValid: false, errorKey: errorKey);

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.ApplyPostingJournalAsync(CreateApplyPostingJournalModel()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Null);
            Assert.That(result.Message.Contains(errorKey), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyPostingJournalAsync_WhenApplyPostingJournalModelIsInvalid_ThrowsIntranetValidationExceptionWhereMessageContainsErrorMessage()
        {
            string errorMessage = _fixture.Create<string>();
            Controller sut = CreateSut(modelIsValid: false, errorMessage: errorMessage);

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.ApplyPostingJournalAsync(CreateApplyPostingJournalModel()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Null);
            Assert.That(result.Message.Contains(errorMessage), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyPostingJournalAsync_WhenApplyPostingJournalModelIsInvalid_ThrowsIntranetValidationExceptionWhereValidatingTypeIsNull()
        {
            Controller sut = CreateSut(modelIsValid: false);

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.ApplyPostingJournalAsync(CreateApplyPostingJournalModel()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ValidatingType, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyPostingJournalAsync_WhenApplyPostingJournalModelIsInvalid_ThrowsIntranetValidationExceptionWhereValidatingFieldIsNull()
        {
            Controller sut = CreateSut(modelIsValid: false);

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.ApplyPostingJournalAsync(CreateApplyPostingJournalModel()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ValidatingField, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournalAsync_WhenApplyPostingJournalModelIsValid_AssertPublishAsyncWasCalledOnCommandBusWithApplyPostingJournalCommand()
        {
            Controller sut = CreateSut();

            await sut.ApplyPostingJournalAsync(CreateApplyPostingJournalModel());

            _commandBusMock.Verify(m => m.PublishAsync<IApplyPostingJournalCommand, IPostingJournalResult>(It.IsNotNull<IApplyPostingJournalCommand>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournalAsync_WhenApplyPostingJournalModelIsValid_AssertPublishAsyncWasCalledOnCommandBusWithApplyPostingJournalCommandWhereAccountingNumberIsEqualToAccountNumberFromApplyPostingJournalModel()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            await sut.ApplyPostingJournalAsync(CreateApplyPostingJournalModel(accountingNumber));

            _commandBusMock.Verify(m => m.PublishAsync<IApplyPostingJournalCommand, IPostingJournalResult>(It.Is<IApplyPostingJournalCommand>(command => command.AccountingNumber == accountingNumber)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournalAsync_WhenApplyPostingJournalModelIsValid_AssertPublishAsyncWasCalledOnCommandBusWithApplyPostingJournalCommandWherePostingLineCollectionIsNotNull()
        {
            Controller sut = CreateSut();

            await sut.ApplyPostingJournalAsync(CreateApplyPostingJournalModel());

            _commandBusMock.Verify(m => m.PublishAsync<IApplyPostingJournalCommand, IPostingJournalResult>(It.Is<IApplyPostingJournalCommand>(command => command.PostingLineCollection != null)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournalAsync_WhenApplyPostingJournalModelIsValid_AssertPublishAsyncWasCalledOnCommandBusWithApplyPostingJournalCommandWherePostingLineCollectionHasSameAmountAsApplyPostingLinesFromApplyPostingJournalModel()
        {
            Controller sut = CreateSut();

            ApplyPostingLineModel[] applyPostingLineModels = _fixture.CreateMany<ApplyPostingLineModel>(_random.Next(10, 15)).ToArray();
            ApplyPostingLineCollectionModel applyPostingLineCollectionModel = CreateApplyPostingLineCollectionModel(applyPostingLineModels);
            await sut.ApplyPostingJournalAsync(CreateApplyPostingJournalModel(applyPostingLineCollectionModel: applyPostingLineCollectionModel));

            _commandBusMock.Verify(m => m.PublishAsync<IApplyPostingJournalCommand, IPostingJournalResult>(It.Is<IApplyPostingJournalCommand>(command => command.PostingLineCollection.Count() == applyPostingLineModels.Length)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournalAsync_WhenApplyPostingJournalModelIsValid_AssertPublishAsyncWasCalledOnCommandBusWithApplyPostingJournalCommandWherePostingLineCollectionContainsMappedApplyPostingLinesFromApplyPostingJournalModel()
        {
            Controller sut = CreateSut();

            ApplyPostingLineModel[] applyPostingLineModels = _fixture.CreateMany<ApplyPostingLineModel>(_random.Next(10, 15)).ToArray();
            ApplyPostingLineCollectionModel applyPostingLineCollectionModel = CreateApplyPostingLineCollectionModel(applyPostingLineModels);
            await sut.ApplyPostingJournalAsync(CreateApplyPostingJournalModel(applyPostingLineCollectionModel: applyPostingLineCollectionModel));

            _commandBusMock.Verify(m => m.PublishAsync<IApplyPostingJournalCommand, IPostingJournalResult>(It.Is<IApplyPostingJournalCommand>(command => command.PostingLineCollection.All(applyPostingLineCommand => applyPostingLineModels.Any(applyPostingLineModel => IsMatch(applyPostingLineCommand, applyPostingLineModel))))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournalAsync_WhenPublishAsyncForApplyPostingJournalCommandReturnsNull_ReturnsNotNull()
        {
            Controller sut = CreateSut(false);

            ActionResult<ApplyPostingJournalResultModel> result = await sut.ApplyPostingJournalAsync(CreateApplyPostingJournalModel());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournalAsync_WhenPublishAsyncForApplyPostingJournalCommandReturnsNull_ReturnsActionResultWhereResultIsNotNull()
        {
            Controller sut = CreateSut(false);

            ActionResult<ApplyPostingJournalResultModel> result = await sut.ApplyPostingJournalAsync(CreateApplyPostingJournalModel());

            Assert.That(result.Result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournalAsync_WhenPublishAsyncForApplyPostingJournalCommandReturnsNull_ReturnsActionResultWhereResultIsOkObjectResult()
        {
            Controller sut = CreateSut(false);

            ActionResult<ApplyPostingJournalResultModel> result = await sut.ApplyPostingJournalAsync(CreateApplyPostingJournalModel());

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournalAsync_WhenPublishAsyncForApplyPostingJournalCommandReturnsNull_ReturnsOkObjectResultWhereValueIsNotNull()
        {
            Controller sut = CreateSut(false);

            OkObjectResult result = (OkObjectResult)(await sut.ApplyPostingJournalAsync(CreateApplyPostingJournalModel())).Result;

            Assert.That(result.Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournalAsync_WhenPublishAsyncForApplyPostingJournalCommandReturnsNull_ReturnsOkObjectResultWhereValueIsApplyPostingJournalResultModel()
        {
            Controller sut = CreateSut(false);

            OkObjectResult result = (OkObjectResult)(await sut.ApplyPostingJournalAsync(CreateApplyPostingJournalModel())).Result;

            Assert.That(result.Value, Is.TypeOf<ApplyPostingJournalResultModel>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournalAsync_WhenPublishAsyncForApplyPostingJournalCommandReturnsNull_ReturnsOkObjectResultWhereValueIsApplyPostingJournalResultModelWithPostingLinesNotEqualToNull()
        {
            Controller sut = CreateSut(false);

            OkObjectResult result = (OkObjectResult)(await sut.ApplyPostingJournalAsync(CreateApplyPostingJournalModel())).Result;

            ApplyPostingJournalResultModel applyPostingJournalResultModel = (ApplyPostingJournalResultModel)result.Value;

            Assert.That(applyPostingJournalResultModel.PostingLines, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournalAsync_WhenPublishAsyncForApplyPostingJournalCommandReturnsNull_ReturnsOkObjectResultWhereValueIsApplyPostingJournalResultModelWithEmptyPostingLines()
        {
            Controller sut = CreateSut(false);

            OkObjectResult result = (OkObjectResult)(await sut.ApplyPostingJournalAsync(CreateApplyPostingJournalModel())).Result;

            ApplyPostingJournalResultModel applyPostingJournalResultModel = (ApplyPostingJournalResultModel)result.Value;

            Assert.That(applyPostingJournalResultModel.PostingLines, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournalAsync_WhenPublishAsyncForApplyPostingJournalCommandReturnsNull_ReturnsOkObjectResultWhereValueIsApplyPostingJournalResultModelWithPostingWarningsNotEqualToNull()
        {
            Controller sut = CreateSut(false);

            OkObjectResult result = (OkObjectResult)(await sut.ApplyPostingJournalAsync(CreateApplyPostingJournalModel())).Result;

            ApplyPostingJournalResultModel applyPostingJournalResultModel = (ApplyPostingJournalResultModel)result.Value;

            Assert.That(applyPostingJournalResultModel.PostingWarnings, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournalAsync_WhenPublishAsyncForApplyPostingJournalCommandReturnsNull_ReturnsOkObjectResultWhereValueIsApplyPostingJournalResultModelWithEmptyPostingWarnings()
        {
            Controller sut = CreateSut(false);

            OkObjectResult result = (OkObjectResult)(await sut.ApplyPostingJournalAsync(CreateApplyPostingJournalModel())).Result;

            ApplyPostingJournalResultModel applyPostingJournalResultModel = (ApplyPostingJournalResultModel)result.Value;

            Assert.That(applyPostingJournalResultModel.PostingWarnings, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournalAsync_WhenPublishAsyncForApplyPostingJournalCommandReturnsPostingJournalResult_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            ActionResult<ApplyPostingJournalResultModel> result = await sut.ApplyPostingJournalAsync(CreateApplyPostingJournalModel());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournalAsync_WhenPublishAsyncForApplyPostingJournalCommandReturnsPostingJournalResult_ReturnsActionResultWhereResultIsNotNull()
        {
            Controller sut = CreateSut();

            ActionResult<ApplyPostingJournalResultModel> result = await sut.ApplyPostingJournalAsync(CreateApplyPostingJournalModel());

            Assert.That(result.Result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournalAsync_WhenPublishAsyncForApplyPostingJournalCommandReturnsPostingJournalResult_ReturnsActionResultWhereResultIsOkObjectResult()
        {
            Controller sut = CreateSut();

            ActionResult<ApplyPostingJournalResultModel> result = await sut.ApplyPostingJournalAsync(CreateApplyPostingJournalModel());

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournalAsync_WhenPublishAsyncForApplyPostingJournalCommandReturnsPostingJournalResult_ReturnsOkObjectResultWhereValueIsNotNull()
        {
            Controller sut = CreateSut();

            OkObjectResult result = (OkObjectResult)(await sut.ApplyPostingJournalAsync(CreateApplyPostingJournalModel())).Result;

            Assert.That(result.Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournalAsync_WhenPublishAsyncForApplyPostingJournalCommandReturnsPostingJournalResult_ReturnsOkObjectResultWhereValueIsApplyPostingJournalResultModel()
        {
            Controller sut = CreateSut();

            OkObjectResult result = (OkObjectResult)(await sut.ApplyPostingJournalAsync(CreateApplyPostingJournalModel())).Result;

            Assert.That(result.Value, Is.TypeOf<ApplyPostingJournalResultModel>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournalAsync_WhenPublishAsyncForApplyPostingJournalCommandReturnsPostingJournalResult_ReturnsOkObjectResultWhereValueIsApplyPostingJournalResultModelWithPostingLinesNotEqualToNull()
        {
            Controller sut = CreateSut();

            OkObjectResult result = (OkObjectResult)(await sut.ApplyPostingJournalAsync(CreateApplyPostingJournalModel())).Result;

            ApplyPostingJournalResultModel applyPostingJournalResultModel = (ApplyPostingJournalResultModel)result.Value;

            Assert.That(applyPostingJournalResultModel.PostingLines, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournalAsync_WhenPublishAsyncForApplyPostingJournalCommandReturnsPostingJournalResult_ReturnsOkObjectResultWhereValueIsApplyPostingJournalResultModelWithSameAmountOfPostingLinesAsPostingLinesInPostingJournalResult()
        {
            IPostingLine[] postingLines = _fixture.CreateMany<IPostingLine>(_random.Next(10, 25)).ToArray();
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLines).Object;
            IPostingJournalResult postingJournalResult = _fixture.BuildPostingJournalResultMock(postingLineCollection).Object;
            Controller sut = CreateSut(postingJournalResult: postingJournalResult);

            OkObjectResult result = (OkObjectResult)(await sut.ApplyPostingJournalAsync(CreateApplyPostingJournalModel())).Result;

            ApplyPostingJournalResultModel applyPostingJournalResultModel = (ApplyPostingJournalResultModel)result.Value;

            Assert.That(applyPostingJournalResultModel.PostingLines.Count, Is.EqualTo(postingLines.Length));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournalAsync_WhenPublishAsyncForApplyPostingJournalCommandReturnsPostingJournalResult_ReturnsOkObjectResultWhereValueIsApplyPostingJournalResultModelWherePostingLinesContainsMappedPostingLinesFromPostingJournalResult()
        {
            IPostingLine[] postingLines = _fixture.CreateMany<IPostingLine>(_random.Next(10, 25)).ToArray();
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLines).Object;
            IPostingJournalResult postingJournalResult = _fixture.BuildPostingJournalResultMock(postingLineCollection).Object;
            Controller sut = CreateSut(postingJournalResult: postingJournalResult);

            OkObjectResult result = (OkObjectResult)(await sut.ApplyPostingJournalAsync(CreateApplyPostingJournalModel())).Result;

            ApplyPostingJournalResultModel applyPostingJournalResultModel = (ApplyPostingJournalResultModel)result.Value;

            Assert.That(applyPostingJournalResultModel.PostingLines.All(postingLineModel => postingLines.Any(postingLine => IsMatch(postingLineModel, postingLine))), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournalAsync_WhenPublishAsyncForApplyPostingJournalCommandReturnsPostingJournalResult_ReturnsOkObjectResultWhereValueIsApplyPostingJournalResultModelWithPostingWarningsNotEqualToNull()
        {
            Controller sut = CreateSut();

            OkObjectResult result = (OkObjectResult)(await sut.ApplyPostingJournalAsync(CreateApplyPostingJournalModel())).Result;

            ApplyPostingJournalResultModel applyPostingJournalResultModel = (ApplyPostingJournalResultModel)result.Value;

            Assert.That(applyPostingJournalResultModel.PostingWarnings, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournalAsync_WhenPublishAsyncForApplyPostingJournalCommandReturnsPostingJournalResult_ReturnsOkObjectResultWhereValueIsApplyPostingJournalResultModelWithSameAmountOfPostingWarningsAsPostingWarningsInPostingJournalResult()
        {
            IPostingWarning[] postingWarnings = _fixture.CreateMany<IPostingWarning>(_random.Next(10, 25)).ToArray();
            IPostingWarningCollection postingWarningCollection = _fixture.BuildPostingWarningCollectionMock(postingWarnings).Object;
            IPostingJournalResult postingJournalResult = _fixture.BuildPostingJournalResultMock(postingWarningCollection: postingWarningCollection).Object;
            Controller sut = CreateSut(postingJournalResult: postingJournalResult);

            OkObjectResult result = (OkObjectResult)(await sut.ApplyPostingJournalAsync(CreateApplyPostingJournalModel())).Result;

            ApplyPostingJournalResultModel applyPostingJournalResultModel = (ApplyPostingJournalResultModel)result.Value;

            Assert.That(applyPostingJournalResultModel.PostingWarnings.Count, Is.EqualTo(postingWarnings.Length));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyPostingJournalAsync_WhenPublishAsyncForApplyPostingJournalCommandReturnsPostingJournalResult_ReturnsOkObjectResultWhereValueIsApplyPostingJournalResultModelWherePostingWarningsContainsMappedPostingWarningsFromPostingJournalResult()
        {
            IPostingWarning[] postingWarnings = _fixture.CreateMany<IPostingWarning>(_random.Next(10, 25)).ToArray();
            IPostingWarningCollection postingWarningCollection = _fixture.BuildPostingWarningCollectionMock(postingWarnings).Object;
            IPostingJournalResult postingJournalResult = _fixture.BuildPostingJournalResultMock(postingWarningCollection: postingWarningCollection).Object;
            Controller sut = CreateSut(postingJournalResult: postingJournalResult);

            OkObjectResult result = (OkObjectResult)(await sut.ApplyPostingJournalAsync(CreateApplyPostingJournalModel())).Result;

            ApplyPostingJournalResultModel applyPostingJournalResultModel = (ApplyPostingJournalResultModel)result.Value;

            Assert.That(applyPostingJournalResultModel.PostingWarnings.All(postingWarningModel => postingWarnings.Any(postingWarning => IsMatch(postingWarningModel, postingWarning))), Is.True);
        }

        private Controller CreateSut(bool hasPostingJournalResult = true, IPostingJournalResult postingJournalResult = null, bool modelIsValid = true, string errorKey = null, string errorMessage = null)
        {
            _commandBusMock.Setup(m => m.PublishAsync<IApplyPostingJournalCommand, IPostingJournalResult>(It.IsAny<IApplyPostingJournalCommand>()))
                .Returns(Task.FromResult(hasPostingJournalResult ? postingJournalResult ?? _fixture.BuildPostingJournalResultMock().Object : null));

            Controller controller = new Controller(_commandBusMock.Object, _queryBusMock.Object);
            if (modelIsValid == false)
            {
                controller.ModelState.AddModelError(errorKey ?? _fixture.Create<string>(), errorMessage ?? _fixture.Create<string>());
            }
            return controller;
        }
    }
}