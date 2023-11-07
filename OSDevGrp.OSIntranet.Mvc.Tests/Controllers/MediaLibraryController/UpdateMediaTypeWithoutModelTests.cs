using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Models.Core;
using System.Threading.Tasks;
using Controller = OSDevGrp.OSIntranet.Mvc.Controllers.MediaLibraryController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.MediaLibraryController
{
	[TestFixture]
    public class UpdateMediaTypeWithoutModelTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Mock<IClaimResolver> _claimResolverMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _claimResolverMock = new Mock<IClaimResolver>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateMediaType_WhenCalled_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            int number = _fixture.Create<int>();
            await sut.UpdateMediaType(number);

            _queryBusMock.Verify(m => m.QueryAsync<IGetMediaTypeQuery, IMediaType>(It.Is<IGetMediaTypeQuery>(value => value != null && value.Number == number)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateMediaType_WhenNoMediaTypeWasReturnedFromQueryBus_ReturnsNotNull()
        {
            Controller sut = CreateSut(hasMediaType: false);

            IActionResult result = await sut.UpdateMediaType(_fixture.Create<int>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateMediaType_WhenNoMediaTypeWasReturnedFromQueryBus_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut(hasMediaType: false);

            IActionResult result = await sut.UpdateMediaType(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateMediaType_WhenMediaTypeWasReturnedFromQueryBus_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.UpdateMediaType(_fixture.Create<int>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateMediaType_WhenMediaTypeWasReturnedFromQueryBus_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.UpdateMediaType(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateMediaType_WhenMediaTypeWasReturnedFromQueryBus_ReturnsViewResultWhereViewNameIsNotNull()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult)await sut.UpdateMediaType(_fixture.Create<int>());

            Assert.That(result.ViewName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateMediaType_WhenMediaTypeWasReturnedFromQueryBus_ReturnsViewResultWhereViewNameIsNotEmpty()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult)await sut.UpdateMediaType(_fixture.Create<int>());

            Assert.That(result.ViewName, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateMediaType_WhenMediaTypeWasReturnedFromQueryBus_ReturnsViewResultWhereViewNameIsEqualToUpdateGenericCategory()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult)await sut.UpdateMediaType(_fixture.Create<int>());

            Assert.That(result.ViewName, Is.EqualTo("UpdateGenericCategory"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateMediaType_WhenMediaTypeWasReturnedFromQueryBus_ReturnsViewResultWhereModelIsNotNull()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult)await sut.UpdateMediaType(_fixture.Create<int>());

            Assert.That(result.Model, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateMediaType_WhenMediaTypeWasReturnedFromQueryBus_ReturnsViewResultWhereModelIsGenericCategoryViewModel()
        {
            int number = _fixture.Create<int>();
            string name = _fixture.Create<string>();
            bool deletable = _fixture.Create<bool>();
            IMediaType mediaType = _fixture.BuildMediaTypeMock(number, name, deletable).Object;
            Controller sut = CreateSut(mediaType: mediaType);

            ViewResult result = (ViewResult)await sut.UpdateMediaType(_fixture.Create<int>());

            Assert.That(result.Model, Is.TypeOf<GenericCategoryViewModel>());

            GenericCategoryViewModel genericCategoryViewModel = (GenericCategoryViewModel)result.Model;
            Assert.That(genericCategoryViewModel, Is.Not.Null);
            Assert.That(genericCategoryViewModel.Number, Is.EqualTo(number));
            Assert.That(genericCategoryViewModel.Name, Is.Not.Null);
            Assert.That(genericCategoryViewModel.Name, Is.Not.Empty);
            Assert.That(genericCategoryViewModel.Name, Is.EqualTo(name));
            Assert.That(genericCategoryViewModel.Deletable, Is.EqualTo(deletable));
            Assert.That(genericCategoryViewModel.Header, Is.Not.Null);
            Assert.That(genericCategoryViewModel.Header, Is.Not.Empty);
            Assert.That(genericCategoryViewModel.Header, Is.EqualTo("Redigér medietype"));
            Assert.That(genericCategoryViewModel.Controller, Is.Not.Null);
            Assert.That(genericCategoryViewModel.Controller, Is.Not.Empty);
            Assert.That(genericCategoryViewModel.Controller, Is.EqualTo("MediaLibrary"));
            Assert.That(genericCategoryViewModel.SubmitText, Is.Not.Null);
            Assert.That(genericCategoryViewModel.SubmitText, Is.Not.Empty);
            Assert.That(genericCategoryViewModel.SubmitText, Is.EqualTo("Opdatér"));
            Assert.That(genericCategoryViewModel.SubmitAction, Is.Not.Null);
            Assert.That(genericCategoryViewModel.SubmitAction, Is.Not.Empty);
            Assert.That(genericCategoryViewModel.SubmitAction, Is.EqualTo("UpdateMediaType"));
            Assert.That(genericCategoryViewModel.CancelText, Is.Not.Null);
            Assert.That(genericCategoryViewModel.CancelText, Is.Not.Empty);
            Assert.That(genericCategoryViewModel.CancelText, Is.EqualTo("Fortryd"));
            Assert.That(genericCategoryViewModel.CancelAction, Is.Not.Null);
            Assert.That(genericCategoryViewModel.CancelAction, Is.Not.Empty);
            Assert.That(genericCategoryViewModel.CancelAction, Is.EqualTo("MediaTypes"));
            Assert.That(genericCategoryViewModel.EditMode, Is.EqualTo(EditMode.Edit));
            Assert.That(genericCategoryViewModel.CreatedByIdentifier, Is.Not.Null);
            Assert.That(genericCategoryViewModel.CreatedByIdentifier, Is.Not.Empty);
            Assert.That(genericCategoryViewModel.CreatedByIdentifier, Is.EqualTo(mediaType.CreatedByIdentifier));
            Assert.That(genericCategoryViewModel.CreatedDateTime, Is.EqualTo(mediaType.CreatedDateTime));
            Assert.That(genericCategoryViewModel.ModifiedByIdentifier, Is.Not.Null);
            Assert.That(genericCategoryViewModel.ModifiedByIdentifier, Is.Not.Empty);
            Assert.That(genericCategoryViewModel.ModifiedByIdentifier, Is.EqualTo(mediaType.ModifiedByIdentifier));
            Assert.That(genericCategoryViewModel.ModifiedDateTime, Is.EqualTo(mediaType.ModifiedDateTime));
        }

        private Controller CreateSut(bool hasMediaType = true, IMediaType mediaType = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<IGetMediaTypeQuery, IMediaType>(It.IsAny<IGetMediaTypeQuery>()))
                .Returns(Task.FromResult(hasMediaType ? mediaType ?? _fixture.BuildMediaTypeMock().Object : null));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
        }
    }
}