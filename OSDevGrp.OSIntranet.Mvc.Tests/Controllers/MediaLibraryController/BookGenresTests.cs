using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Controller = OSDevGrp.OSIntranet.Mvc.Controllers.MediaLibraryController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.MediaLibraryController
{
	[TestFixture]
    public class BookGenresTests
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
            _fixture.Customize<IBookGenre>(builder => builder.FromFactory(() => _fixture.BuildBookGenreMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task BookGenres_WhenCalled_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            await sut.BookGenres();

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<IBookGenre>>(It.IsNotNull<EmptyQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task BookGenres_WhenCalled_AssertIsMediaLibraryModifierWasCalledOnClaimResolver()
        {
	        Controller sut = CreateSut();

	        await sut.BookGenres();

	        _claimResolverMock.Verify(m => m.IsMediaLibraryModifier(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public async Task BookGenres_WhenCalled_ReturnsNotNull(bool hasBookGenreCollection)
        {
            Controller sut = CreateSut(hasBookGenreCollection);

            IActionResult result = await sut.BookGenres();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public async Task BookGenres_WhenCalled_ReturnsViewResult(bool hasBookGenreCollection)
        {
            Controller sut = CreateSut(hasBookGenreCollection);

            IActionResult result = await sut.BookGenres();

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public async Task BookGenres_WhenCalled_ReturnsViewResultWhereViewNameIsNotNull(bool hasBookGenreCollection)
        {
            Controller sut = CreateSut(hasBookGenreCollection);

            ViewResult result = (ViewResult)await sut.BookGenres();

            Assert.That(result.ViewName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public async Task BookGenres_WhenCalled_ReturnsViewResultWhereViewNameIsNotEmpty(bool hasBookGenreCollection)
        {
            Controller sut = CreateSut(hasBookGenreCollection);

            ViewResult result = (ViewResult)await sut.BookGenres();

            Assert.That(result.ViewName, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public async Task BookGenres_WhenCalled_ReturnsViewResultWhereViewNameIsEqualToGenericCategories(bool hasBookGenreCollection)
        {
            Controller sut = CreateSut(hasBookGenreCollection);

            ViewResult result = (ViewResult)await sut.BookGenres();

            Assert.That(result.ViewName, Is.EqualTo("GenericCategories"));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public async Task BookGenres_WhenCalled_ReturnsViewResultWhereModelIsNotNull(bool hasBookGenreCollection)
        {
            Controller sut = CreateSut(hasBookGenreCollection);

            ViewResult result = (ViewResult)await sut.BookGenres();

            Assert.That(result.Model, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public async Task BookGenres_WhenCalled_ReturnsViewResultWhereModelIsGenericCategoryCollectionViewModel(bool hasBookGenreCollection, bool isMediaLibraryModifier)
        {
            Controller sut = CreateSut(hasBookGenreCollection, isMediaLibraryModifier: isMediaLibraryModifier);

            ViewResult result = (ViewResult)await sut.BookGenres();

            Assert.That(result.Model, Is.TypeOf<GenericCategoryCollectionViewModel>());

            GenericCategoryCollectionViewModel genericCategoryCollectionViewModel = (GenericCategoryCollectionViewModel)result.Model;
            Assert.That(genericCategoryCollectionViewModel, Is.Not.Null);
            Assert.That(genericCategoryCollectionViewModel.Header, Is.Not.Null);
            Assert.That(genericCategoryCollectionViewModel.Header, Is.Not.Empty);
            Assert.That(genericCategoryCollectionViewModel.Header, Is.EqualTo("Litterære genre"));
            Assert.That(genericCategoryCollectionViewModel.Controller, Is.Not.Null);
            Assert.That(genericCategoryCollectionViewModel.Controller, Is.Not.Empty);
            Assert.That(genericCategoryCollectionViewModel.Controller, Is.EqualTo("MediaLibrary"));
            Assert.That(genericCategoryCollectionViewModel.CreateAction, Is.Not.Null);
            Assert.That(genericCategoryCollectionViewModel.CreateAction, Is.Not.Empty);
            Assert.That(genericCategoryCollectionViewModel.CreateAction, Is.EqualTo("CreateBookGenre"));
            Assert.That(genericCategoryCollectionViewModel.UpdateAction, Is.Not.Null);
            Assert.That(genericCategoryCollectionViewModel.UpdateAction, Is.Not.Empty);
            Assert.That(genericCategoryCollectionViewModel.UpdateAction, Is.EqualTo("UpdateBookGenre"));
            Assert.That(genericCategoryCollectionViewModel.DeletionUrlGetter, Is.Not.Null);
            Assert.That(genericCategoryCollectionViewModel.AllowCreation, Is.EqualTo(isMediaLibraryModifier));
            Assert.That(genericCategoryCollectionViewModel.AllowModification, Is.EqualTo(isMediaLibraryModifier));
            Assert.That(genericCategoryCollectionViewModel.AllowDeletion, Is.EqualTo(isMediaLibraryModifier));
        }

		[Test]
        [Category("UnitTest")]
        public async Task BookGenres_WhenBookGenresWasReturnedFromQueryBus_ReturnsViewResultWhereModelIsNotEmpty()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult)await sut.BookGenres();

            Assert.That(result.Model, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task BookGenres_WhenNoBookGenresWasReturnedFromQueryBus_ReturnsViewResultWhereModelIsEmpty()
        {
            Controller sut = CreateSut(false);

            ViewResult result = (ViewResult)await sut.BookGenres();

            Assert.That(result.Model, Is.Empty);
        }

        private Controller CreateSut(bool hasBookGenreCollection = true, IEnumerable<IBookGenre> bookGenreCollection = null, bool? isMediaLibraryModifier = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<IBookGenre>>(It.IsAny<EmptyQuery>()))
                .Returns(Task.FromResult(hasBookGenreCollection ? bookGenreCollection ?? _fixture.CreateMany<IBookGenre>(_random.Next(5, 10)).ToArray() : null));

            _claimResolverMock.Setup(m => m.IsMediaLibraryModifier())
	            .Returns(isMediaLibraryModifier ?? _fixture.Create<bool>());

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
        }
    }
}