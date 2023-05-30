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
    public class MediaTypesTests
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
            _fixture.Customize<IMediaType>(builder => builder.FromFactory(() => _fixture.BuildMediaTypeMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task MediaTypes_WhenCalled_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            await sut.MediaTypes();

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<IMediaType>>(It.IsNotNull<EmptyQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MediaTypes_WhenCalled_AssertIsMediaLibraryModifierWasCalledOnClaimResolver()
        {
	        Controller sut = CreateSut();

	        await sut.MediaTypes();

	        _claimResolverMock.Verify(m => m.IsMediaLibraryModifier(), Times.Once);
        }

		[Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public async Task MediaTypes_WhenCalled_ReturnsNotNull(bool hasMediaTypeCollection)
        {
            Controller sut = CreateSut(hasMediaTypeCollection);

            IActionResult result = await sut.MediaTypes();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public async Task MediaTypes_WhenCalled_ReturnsViewResult(bool hasMediaTypeCollection)
        {
            Controller sut = CreateSut(hasMediaTypeCollection);

            IActionResult result = await sut.MediaTypes();

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public async Task MediaTypes_WhenCalled_ReturnsViewResultWhereViewNameIsNotNull(bool hasMediaTypeCollection)
        {
            Controller sut = CreateSut(hasMediaTypeCollection);

            ViewResult result = (ViewResult)await sut.MediaTypes();

            Assert.That(result.ViewName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public async Task MediaTypes_WhenCalled_ReturnsViewResultWhereViewNameIsNotEmpty(bool hasMediaTypeCollection)
        {
            Controller sut = CreateSut(hasMediaTypeCollection);

            ViewResult result = (ViewResult)await sut.MediaTypes();

            Assert.That(result.ViewName, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public async Task MediaTypes_WhenCalled_ReturnsViewResultWhereViewNameIsEqualToGenericCategories(bool hasMediaTypeCollection)
        {
            Controller sut = CreateSut(hasMediaTypeCollection);

            ViewResult result = (ViewResult)await sut.MediaTypes();

            Assert.That(result.ViewName, Is.EqualTo("GenericCategories"));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public async Task MediaTypes_WhenCalled_ReturnsViewResultWhereModelIsNotNull(bool hasMediaTypeCollection)
        {
            Controller sut = CreateSut(hasMediaTypeCollection);

            ViewResult result = (ViewResult)await sut.MediaTypes();

            Assert.That(result.Model, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public async Task MediaTypes_WhenCalled_ReturnsViewResultWhereModelIsGenericCategoryCollectionViewModel(bool hasMediaTypeCollection, bool isMediaLibraryModifier)
        {
            Controller sut = CreateSut(hasMediaTypeCollection, isMediaLibraryModifier: isMediaLibraryModifier);

            ViewResult result = (ViewResult)await sut.MediaTypes();

            Assert.That(result.Model, Is.TypeOf<GenericCategoryCollectionViewModel>());

            GenericCategoryCollectionViewModel genericCategoryCollectionViewModel = (GenericCategoryCollectionViewModel)result.Model;
            Assert.That(genericCategoryCollectionViewModel, Is.Not.Null);
            Assert.That(genericCategoryCollectionViewModel.Header, Is.Not.Null);
            Assert.That(genericCategoryCollectionViewModel.Header, Is.Not.Empty);
            Assert.That(genericCategoryCollectionViewModel.Header, Is.EqualTo("Medietyper"));
            Assert.That(genericCategoryCollectionViewModel.Controller, Is.Not.Null);
            Assert.That(genericCategoryCollectionViewModel.Controller, Is.Not.Empty);
            Assert.That(genericCategoryCollectionViewModel.Controller, Is.EqualTo("MediaLibrary"));
            Assert.That(genericCategoryCollectionViewModel.CreateAction, Is.Not.Null);
            Assert.That(genericCategoryCollectionViewModel.CreateAction, Is.Not.Empty);
            Assert.That(genericCategoryCollectionViewModel.CreateAction, Is.EqualTo("CreateMediaType"));
            Assert.That(genericCategoryCollectionViewModel.UpdateAction, Is.Not.Null);
            Assert.That(genericCategoryCollectionViewModel.UpdateAction, Is.Not.Empty);
            Assert.That(genericCategoryCollectionViewModel.UpdateAction, Is.EqualTo("UpdateMediaType"));
            Assert.That(genericCategoryCollectionViewModel.DeletionUrlGetter, Is.Not.Null);
            Assert.That(genericCategoryCollectionViewModel.AllowCreation, Is.EqualTo(isMediaLibraryModifier));
            Assert.That(genericCategoryCollectionViewModel.AllowModification, Is.EqualTo(isMediaLibraryModifier));
            Assert.That(genericCategoryCollectionViewModel.AllowDeletion, Is.EqualTo(isMediaLibraryModifier));
        }

		[Test]
        [Category("UnitTest")]
        public async Task MediaTypes_WhenMediaTypesWasReturnedFromQueryBus_ReturnsViewResultWhereModelIsNotEmpty()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult)await sut.MediaTypes();

            Assert.That(result.Model, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task MediaTypes_WhenNoMediaTypesWasReturnedFromQueryBus_ReturnsViewResultWhereModelIsEmpty()
        {
            Controller sut = CreateSut(false);

            ViewResult result = (ViewResult)await sut.MediaTypes();

            Assert.That(result.Model, Is.Empty);
        }

        private Controller CreateSut(bool hasMediaTypeCollection = true, IEnumerable<IMediaType> mediaTypeCollection = null, bool? isMediaLibraryModifier = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<IMediaType>>(It.IsAny<EmptyQuery>()))
                .Returns(Task.FromResult(hasMediaTypeCollection? mediaTypeCollection ?? _fixture.CreateMany<IMediaType>(_random.Next(5, 10)).ToArray() : null));

            _claimResolverMock.Setup(m => m.IsMediaLibraryModifier())
	            .Returns(isMediaLibraryModifier ?? _fixture.Create<bool>());

			return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
        }
    }
}