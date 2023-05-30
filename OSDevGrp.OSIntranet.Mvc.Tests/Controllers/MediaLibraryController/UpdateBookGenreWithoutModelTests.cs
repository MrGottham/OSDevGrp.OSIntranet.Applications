﻿using AutoFixture;
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
    public class UpdateBookGenreWithoutModelTests
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
        public async Task UpdateBookGenre_WhenCalled_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            int number = _fixture.Create<int>();
            await sut.UpdateBookGenre(number);

            _queryBusMock.Verify(m => m.QueryAsync<IGetBookGenreQuery, IBookGenre>(It.Is<IGetBookGenreQuery>(value => value != null && value.Number == number)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBookGenre_WhenNoBookGenreWasReturnedFromQueryBus_ReturnsNotNull()
        {
            Controller sut = CreateSut(hasBookGenre: false);

            IActionResult result = await sut.UpdateBookGenre(_fixture.Create<int>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBookGenre_WhenNoBookGenreWasReturnedFromQueryBus_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut(hasBookGenre: false);

            IActionResult result = await sut.UpdateBookGenre(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBookGenre_WhenBookGenreWasReturnedFromQueryBus_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.UpdateBookGenre(_fixture.Create<int>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBookGenre_WhenBookGenreWasReturnedFromQueryBus_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.UpdateBookGenre(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBookGenre_WhenBookGenreWasReturnedFromQueryBus_ReturnsViewResultWhereViewNameIsNotNull()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult)await sut.UpdateBookGenre(_fixture.Create<int>());

            Assert.That(result.ViewName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBookGenre_WhenBookGenreWasReturnedFromQueryBus_ReturnsViewResultWhereViewNameIsNotEmpty()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult)await sut.UpdateBookGenre(_fixture.Create<int>());

            Assert.That(result.ViewName, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBookGenre_WhenBookGenreWasReturnedFromQueryBus_ReturnsViewResultWhereViewNameIsEqualToUpdateGenericCategory()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult)await sut.UpdateBookGenre(_fixture.Create<int>());

            Assert.That(result.ViewName, Is.EqualTo("UpdateGenericCategory"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBookGenre_WhenBookGenreWasReturnedFromQueryBus_ReturnsViewResultWhereModelIsNotNull()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult)await sut.UpdateBookGenre(_fixture.Create<int>());

            Assert.That(result.Model, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBookGenre_WhenBookGenreWasReturnedFromQueryBus_ReturnsViewResultWhereModelIsGenericCategoryViewModel()
        {
            int number = _fixture.Create<int>();
            string name = _fixture.Create<string>();
            bool deletable = _fixture.Create<bool>();
            IBookGenre bookGenre = _fixture.BuildBookGenreMock(number, name, deletable).Object;
            Controller sut = CreateSut(bookGenre: bookGenre);

            ViewResult result = (ViewResult)await sut.UpdateBookGenre(_fixture.Create<int>());

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
            Assert.That(genericCategoryViewModel.Header, Is.EqualTo("Redigér litterær genre"));
            Assert.That(genericCategoryViewModel.Controller, Is.Not.Null);
            Assert.That(genericCategoryViewModel.Controller, Is.Not.Empty);
            Assert.That(genericCategoryViewModel.Controller, Is.EqualTo("MediaLibrary"));
            Assert.That(genericCategoryViewModel.SubmitText, Is.Not.Null);
            Assert.That(genericCategoryViewModel.SubmitText, Is.Not.Empty);
            Assert.That(genericCategoryViewModel.SubmitText, Is.EqualTo("Opdatér"));
            Assert.That(genericCategoryViewModel.SubmitAction, Is.Not.Null);
            Assert.That(genericCategoryViewModel.SubmitAction, Is.Not.Empty);
            Assert.That(genericCategoryViewModel.SubmitAction, Is.EqualTo("UpdateBookGenre"));
            Assert.That(genericCategoryViewModel.CancelText, Is.Not.Null);
            Assert.That(genericCategoryViewModel.CancelText, Is.Not.Empty);
            Assert.That(genericCategoryViewModel.CancelText, Is.EqualTo("Fortryd"));
            Assert.That(genericCategoryViewModel.CancelAction, Is.Not.Null);
            Assert.That(genericCategoryViewModel.CancelAction, Is.Not.Empty);
            Assert.That(genericCategoryViewModel.CancelAction, Is.EqualTo("BookGenres"));
            Assert.That(genericCategoryViewModel.EditMode, Is.EqualTo(EditMode.Edit));
            Assert.That(genericCategoryViewModel.CreatedByIdentifier, Is.Not.Null);
            Assert.That(genericCategoryViewModel.CreatedByIdentifier, Is.Not.Empty);
            Assert.That(genericCategoryViewModel.CreatedByIdentifier, Is.EqualTo(bookGenre.CreatedByIdentifier));
            Assert.That(genericCategoryViewModel.CreatedDateTime, Is.EqualTo(bookGenre.CreatedDateTime));
            Assert.That(genericCategoryViewModel.ModifiedByIdentifier, Is.Not.Null);
            Assert.That(genericCategoryViewModel.ModifiedByIdentifier, Is.Not.Empty);
            Assert.That(genericCategoryViewModel.ModifiedByIdentifier, Is.EqualTo(bookGenre.ModifiedByIdentifier));
            Assert.That(genericCategoryViewModel.ModifiedDateTime, Is.EqualTo(bookGenre.ModifiedDateTime));
        }

        private Controller CreateSut(bool hasBookGenre = true, IBookGenre bookGenre = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<IGetBookGenreQuery, IBookGenre>(It.IsAny<IGetBookGenreQuery>()))
                .Returns(Task.FromResult(hasBookGenre ? bookGenre ?? _fixture.BuildBookGenreMock().Object : null));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
        }
    }
}