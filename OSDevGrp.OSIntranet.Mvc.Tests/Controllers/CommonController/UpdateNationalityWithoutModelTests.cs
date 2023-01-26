using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Queries;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Models.Core;
using System.Threading.Tasks;
using Controller = OSDevGrp.OSIntranet.Mvc.Controllers.CommonController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.CommonController
{
    [TestFixture]
    public class UpdateNationalityWithoutModelTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateNationality_WhenCalled_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            int number = _fixture.Create<int>();
            await sut.UpdateNationality(number);

            _queryBusMock.Verify(m => m.QueryAsync<IGetNationalityQuery, INationality>(It.Is<IGetNationalityQuery>(value => value != null && value.Number == number)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateNationality_WhenNoNationalityWasReturnedFromQueryBus_ReturnsNotNull()
        {
            Controller sut = CreateSut(hasNationality: false);

            IActionResult result = await sut.UpdateNationality(_fixture.Create<int>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateNationality_WhenNoNationalityWasReturnedFromQueryBus_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut(hasNationality: false);

            IActionResult result = await sut.UpdateNationality(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateNationality_WhenNationalityWasReturnedFromQueryBus_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.UpdateNationality(_fixture.Create<int>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateNationality_WhenNationalityWasReturnedFromQueryBus_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.UpdateNationality(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateNationality_WhenNationalityWasReturnedFromQueryBus_ReturnsViewResultWhereViewNameIsNotNull()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) await sut.UpdateNationality(_fixture.Create<int>());

            Assert.That(result.ViewName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateNationality_WhenNationalityWasReturnedFromQueryBus_ReturnsViewResultWhereViewNameIsNotEmpty()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult)await sut.UpdateNationality(_fixture.Create<int>());

            Assert.That(result.ViewName, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateNationality_WhenNationalityWasReturnedFromQueryBus_ReturnsViewResultWhereViewNameIsEqualToUpdateGenericCategory()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult)await sut.UpdateNationality(_fixture.Create<int>());

            Assert.That(result.ViewName, Is.EqualTo("UpdateGenericCategory"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateNationality_WhenNationalityWasReturnedFromQueryBus_ReturnsViewResultWhereModelIsNotNull()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult)await sut.UpdateNationality(_fixture.Create<int>());

            Assert.That(result.Model, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateNationality_WhenNationalityWasReturnedFromQueryBus_ReturnsViewResultWhereModelIsGenericCategoryViewModel()
        {
            int number = _fixture.Create<int>();
            string name = _fixture.Create<string>();
            bool deletable = _fixture.Create<bool>();
            INationality nationality = _fixture.BuildNationalityMock(number, name, deletable).Object;
            Controller sut = CreateSut(nationality: nationality);

            ViewResult result = (ViewResult)await sut.UpdateNationality(_fixture.Create<int>());

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
            Assert.That(genericCategoryViewModel.Header, Is.EqualTo("Redigér nationalitet"));
            Assert.That(genericCategoryViewModel.Controller, Is.Not.Null);
            Assert.That(genericCategoryViewModel.Controller, Is.Not.Empty);
            Assert.That(genericCategoryViewModel.Controller, Is.EqualTo("Common"));
            Assert.That(genericCategoryViewModel.SubmitText, Is.Not.Null);
            Assert.That(genericCategoryViewModel.SubmitText, Is.Not.Empty);
            Assert.That(genericCategoryViewModel.SubmitText, Is.EqualTo("Opdatér"));
            Assert.That(genericCategoryViewModel.SubmitAction, Is.Not.Null);
            Assert.That(genericCategoryViewModel.SubmitAction, Is.Not.Empty);
            Assert.That(genericCategoryViewModel.SubmitAction, Is.EqualTo("UpdateNationality"));
            Assert.That(genericCategoryViewModel.CancelText, Is.Not.Null);
            Assert.That(genericCategoryViewModel.CancelText, Is.Not.Empty);
            Assert.That(genericCategoryViewModel.CancelText, Is.EqualTo("Fortryd"));
            Assert.That(genericCategoryViewModel.CancelAction, Is.Not.Null);
            Assert.That(genericCategoryViewModel.CancelAction, Is.Not.Empty);
            Assert.That(genericCategoryViewModel.CancelAction, Is.EqualTo("Nationalities"));
            Assert.That(genericCategoryViewModel.EditMode, Is.EqualTo(EditMode.Edit));
            Assert.That(genericCategoryViewModel.CreatedByIdentifier, Is.Not.Null);
            Assert.That(genericCategoryViewModel.CreatedByIdentifier, Is.Not.Empty);
            Assert.That(genericCategoryViewModel.CreatedByIdentifier, Is.EqualTo(nationality.CreatedByIdentifier));
            Assert.That(genericCategoryViewModel.CreatedDateTime, Is.EqualTo(nationality.CreatedDateTime));
            Assert.That(genericCategoryViewModel.ModifiedByIdentifier, Is.Not.Null);
            Assert.That(genericCategoryViewModel.ModifiedByIdentifier, Is.Not.Empty);
            Assert.That(genericCategoryViewModel.ModifiedByIdentifier, Is.EqualTo(nationality.ModifiedByIdentifier));
            Assert.That(genericCategoryViewModel.ModifiedDateTime, Is.EqualTo(nationality.ModifiedDateTime));
        }

        private Controller CreateSut(bool hasNationality = true, INationality nationality = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<IGetNationalityQuery, INationality>(It.IsAny<IGetNationalityQuery>()))
                .Returns(Task.FromResult(hasNationality ? nationality ?? _fixture.BuildNationalityMock().Object : null));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object);
        }
    }
}