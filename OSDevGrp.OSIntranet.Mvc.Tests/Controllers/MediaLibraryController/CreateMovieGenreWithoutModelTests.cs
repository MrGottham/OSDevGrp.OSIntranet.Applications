﻿using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Mvc.Models.Core;
using System;
using Controller = OSDevGrp.OSIntranet.Mvc.Controllers.MediaLibraryController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.MediaLibraryController
{
	[TestFixture]
    public class CreateMovieGenreWithoutModelTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Mock<IClaimResolver> _claimResolverMock;

		#endregion

		[SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _claimResolverMock = new Mock<IClaimResolver>();
        }

		[Test]
        [Category("UnitTest")]
        public void CreateMovieGenre_WhenCalled_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.CreateMovieGenre();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void CreateMovieGenre_WhenCalled_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.CreateMovieGenre();

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void CreateMovieGenre_WhenCalled_ReturnsViewResultWhereViewNameIsNotNull()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult)sut.CreateMovieGenre();

            Assert.That(result.ViewName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void CreateMovieGenre_WhenCalled_ReturnsViewResultWhereViewNameIsNotEmpty()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult)sut.CreateMovieGenre();

            Assert.That(result.ViewName, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void CreateMovieGenre_WhenCalled_ReturnsViewResultWhereViewNameIsEqualToCreateGenericCategory()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult)sut.CreateMovieGenre();

            Assert.That(result.ViewName, Is.EqualTo("CreateGenericCategory"));
        }

        [Test]
        [Category("UnitTest")]
        public void CreateMovieGenre_WhenCalled_ReturnsViewResultWhereModelIsNotNull()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult)sut.CreateMovieGenre();

            Assert.That(result.Model, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void CreateMovieGenre_WhenCalled_ReturnsViewResultWhereModelIsGenericCategoryViewModel()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult)sut.CreateMovieGenre();

            Assert.That(result.Model, Is.TypeOf<GenericCategoryViewModel>());

            GenericCategoryViewModel genericCategoryViewModel = (GenericCategoryViewModel)result.Model;
            Assert.That(genericCategoryViewModel, Is.Not.Null);
            Assert.That(genericCategoryViewModel.Number, Is.EqualTo(default(int)));
            Assert.That(genericCategoryViewModel.Name, Is.Null);
            Assert.That(genericCategoryViewModel.Deletable, Is.EqualTo(default(bool)));
            Assert.That(genericCategoryViewModel.Header, Is.Not.Null);
            Assert.That(genericCategoryViewModel.Header, Is.Not.Empty);
            Assert.That(genericCategoryViewModel.Header, Is.EqualTo("Opret filmgenre"));
            Assert.That(genericCategoryViewModel.Controller, Is.Not.Null);
            Assert.That(genericCategoryViewModel.Controller, Is.Not.Empty);
            Assert.That(genericCategoryViewModel.Controller, Is.EqualTo("MediaLibrary"));
            Assert.That(genericCategoryViewModel.SubmitText, Is.Not.Null);
            Assert.That(genericCategoryViewModel.SubmitText, Is.Not.Empty);
            Assert.That(genericCategoryViewModel.SubmitText, Is.EqualTo("Opret"));
            Assert.That(genericCategoryViewModel.SubmitAction, Is.Not.Null);
            Assert.That(genericCategoryViewModel.SubmitAction, Is.Not.Empty);
            Assert.That(genericCategoryViewModel.SubmitAction, Is.EqualTo("CreateMovieGenre"));
            Assert.That(genericCategoryViewModel.CancelText, Is.Not.Null);
            Assert.That(genericCategoryViewModel.CancelText, Is.Not.Empty);
            Assert.That(genericCategoryViewModel.CancelText, Is.EqualTo("Fortryd"));
            Assert.That(genericCategoryViewModel.CancelAction, Is.Not.Null);
            Assert.That(genericCategoryViewModel.CancelAction, Is.Not.Empty);
            Assert.That(genericCategoryViewModel.CancelAction, Is.EqualTo("MovieGenres"));
            Assert.That(genericCategoryViewModel.EditMode, Is.EqualTo(EditMode.Create));
            Assert.That(genericCategoryViewModel.CreatedByIdentifier, Is.Null);
            Assert.That(genericCategoryViewModel.CreatedDateTime, Is.EqualTo(default(DateTime)));
            Assert.That(genericCategoryViewModel.ModifiedByIdentifier, Is.Null);
            Assert.That(genericCategoryViewModel.ModifiedDateTime, Is.EqualTo(default(DateTime)));
        }

        private Controller CreateSut()
        {
            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
        }
    }
}