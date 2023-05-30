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
    public class MusicGenresTests
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
            _fixture.Customize<IMusicGenre>(builder => builder.FromFactory(() => _fixture.BuildMusicGenreMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task MusicGenres_WhenCalled_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            await sut.MusicGenres();

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<IMusicGenre>>(It.IsNotNull<EmptyQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MusicGenres_WhenCalled_AssertIsMediaLibraryModifierWasCalledOnClaimResolver()
        {
	        Controller sut = CreateSut();

	        await sut.MusicGenres();

	        _claimResolverMock.Verify(m => m.IsMediaLibraryModifier(), Times.Once);
        }

		[Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public async Task MusicGenres_WhenCalled_ReturnsNotNull(bool hasMusicGenreCollection)
        {
            Controller sut = CreateSut(hasMusicGenreCollection);

            IActionResult result = await sut.MusicGenres();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public async Task MusicGenres_WhenCalled_ReturnsViewResult(bool hasMusicGenreCollection)
        {
            Controller sut = CreateSut(hasMusicGenreCollection);

            IActionResult result = await sut.MusicGenres();

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public async Task MusicGenres_WhenCalled_ReturnsViewResultWhereViewNameIsNotNull(bool hasMusicGenreCollection)
        {
            Controller sut = CreateSut(hasMusicGenreCollection);

            ViewResult result = (ViewResult)await sut.MusicGenres();

            Assert.That(result.ViewName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public async Task MusicGenres_WhenCalled_ReturnsViewResultWhereViewNameIsNotEmpty(bool hasMusicGenreCollection)
        {
            Controller sut = CreateSut(hasMusicGenreCollection);

            ViewResult result = (ViewResult)await sut.MusicGenres();

            Assert.That(result.ViewName, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public async Task MusicGenres_WhenCalled_ReturnsViewResultWhereViewNameIsEqualToGenericCategories(bool hasMusicGenreCollection)
        {
            Controller sut = CreateSut(hasMusicGenreCollection);

            ViewResult result = (ViewResult)await sut.MusicGenres();

            Assert.That(result.ViewName, Is.EqualTo("GenericCategories"));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public async Task MusicGenres_WhenCalled_ReturnsViewResultWhereModelIsNotNull(bool hasMusicGenreCollection)
        {
            Controller sut = CreateSut(hasMusicGenreCollection);

            ViewResult result = (ViewResult)await sut.MusicGenres();

            Assert.That(result.Model, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public async Task MusicGenres_WhenCalled_ReturnsViewResultWhereModelIsGenericCategoryCollectionViewModel(bool hasMusicGenreCollection, bool isMediaLibraryModifier)
        {
            Controller sut = CreateSut(hasMusicGenreCollection, isMediaLibraryModifier: isMediaLibraryModifier);

            ViewResult result = (ViewResult)await sut.MusicGenres();

            Assert.That(result.Model, Is.TypeOf<GenericCategoryCollectionViewModel>());

            GenericCategoryCollectionViewModel genericCategoryCollectionViewModel = (GenericCategoryCollectionViewModel)result.Model;
            Assert.That(genericCategoryCollectionViewModel, Is.Not.Null);
            Assert.That(genericCategoryCollectionViewModel.Header, Is.Not.Null);
            Assert.That(genericCategoryCollectionViewModel.Header, Is.Not.Empty);
            Assert.That(genericCategoryCollectionViewModel.Header, Is.EqualTo("Musikgenre"));
            Assert.That(genericCategoryCollectionViewModel.Controller, Is.Not.Null);
            Assert.That(genericCategoryCollectionViewModel.Controller, Is.Not.Empty);
            Assert.That(genericCategoryCollectionViewModel.Controller, Is.EqualTo("MediaLibrary"));
            Assert.That(genericCategoryCollectionViewModel.CreateAction, Is.Not.Null);
            Assert.That(genericCategoryCollectionViewModel.CreateAction, Is.Not.Empty);
            Assert.That(genericCategoryCollectionViewModel.CreateAction, Is.EqualTo("CreateMusicGenre"));
            Assert.That(genericCategoryCollectionViewModel.UpdateAction, Is.Not.Null);
            Assert.That(genericCategoryCollectionViewModel.UpdateAction, Is.Not.Empty);
            Assert.That(genericCategoryCollectionViewModel.UpdateAction, Is.EqualTo("UpdateMusicGenre"));
            Assert.That(genericCategoryCollectionViewModel.DeletionUrlGetter, Is.Not.Null);
            Assert.That(genericCategoryCollectionViewModel.AllowCreation, Is.EqualTo(isMediaLibraryModifier));
            Assert.That(genericCategoryCollectionViewModel.AllowModification, Is.EqualTo(isMediaLibraryModifier));
            Assert.That(genericCategoryCollectionViewModel.AllowDeletion, Is.EqualTo(isMediaLibraryModifier));
        }

		[Test]
        [Category("UnitTest")]
        public async Task MusicGenres_WhenMusicGenresWasReturnedFromQueryBus_ReturnsViewResultWhereModelIsNotEmpty()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult)await sut.MusicGenres();

            Assert.That(result.Model, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MusicGenres_WhenNoMusicGenresWasReturnedFromQueryBus_ReturnsViewResultWhereModelIsEmpty()
        {
            Controller sut = CreateSut(false);

            ViewResult result = (ViewResult)await sut.MusicGenres();

            Assert.That(result.Model, Is.Empty);
        }

        private Controller CreateSut(bool hasMusicGenreCollection = true, IEnumerable<IMusicGenre> musicGenreCollection = null, bool? isMediaLibraryModifier = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<IMusicGenre>>(It.IsAny<EmptyQuery>()))
                .Returns(Task.FromResult(hasMusicGenreCollection ? musicGenreCollection ?? _fixture.CreateMany<IMusicGenre>(_random.Next(5, 10)).ToArray() : null));

            _claimResolverMock.Setup(m => m.IsMediaLibraryModifier())
	            .Returns(isMediaLibraryModifier ?? _fixture.Create<bool>());

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
        }
    }
}