using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Controller = OSDevGrp.OSIntranet.Mvc.Controllers.CommonController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.CommonController
{
    [TestFixture]
    public class LanguagesTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();

            _fixture = new Fixture();
            _fixture.Customize<ILanguage>(builder => builder.FromFactory(() => _fixture.BuildLanguageMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task Languages_WhenCalled_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            await sut.Languages();

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<ILanguage>>(It.IsNotNull<EmptyQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public async Task Languages_WhenCalled_ReturnsNotNull(bool hasLanguageCollection)
        {
            Controller sut = CreateSut(hasLanguageCollection);

            IActionResult result = await sut.Languages();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public async Task Languages_WhenCalled_ReturnsViewResult(bool hasLanguageCollection)
        {
            Controller sut = CreateSut(hasLanguageCollection);

            IActionResult result = await sut.Languages();

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public async Task Languages_WhenCalled_ReturnsViewResultWhereViewNameIsNotNull(bool hasLanguageCollection)
        {
            Controller sut = CreateSut(hasLanguageCollection);

            ViewResult result = (ViewResult)await sut.Languages();

            Assert.That(result.ViewName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public async Task Languages_WhenCalled_ReturnsViewResultWhereViewNameIsNotEmpty(bool hasLanguageCollection)
        {
            Controller sut = CreateSut(hasLanguageCollection);

            ViewResult result = (ViewResult)await sut.Languages();

            Assert.That(result.ViewName, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public async Task Languages_WhenCalled_ReturnsViewResultWhereViewNameIsEqualToGenericCategories(bool hasLanguageCollection)
        {
            Controller sut = CreateSut(hasLanguageCollection);

            ViewResult result = (ViewResult)await sut.Languages();

            Assert.That(result.ViewName, Is.EqualTo("GenericCategories"));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public async Task Languages_WhenCalled_ReturnsViewResultWhereModelIsNotNull(bool hasLanguageCollection)
        {
            Controller sut = CreateSut(hasLanguageCollection);

            ViewResult result = (ViewResult)await sut.Languages();

            Assert.That(result.Model, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public async Task Languages_WhenCalled_ReturnsViewResultWhereModelIsGenericCategoryCollectionViewModel(bool hasLanguageCollection)
        {
            Controller sut = CreateSut(hasLanguageCollection);

            ViewResult result = (ViewResult)await sut.Languages();

            Assert.That(result.Model, Is.TypeOf<GenericCategoryCollectionViewModel>());

            GenericCategoryCollectionViewModel genericCategoryCollectionViewModel = (GenericCategoryCollectionViewModel)result.Model;
            Assert.That(genericCategoryCollectionViewModel, Is.Not.Null);
            Assert.That(genericCategoryCollectionViewModel.Header, Is.Not.Null);
            Assert.That(genericCategoryCollectionViewModel.Header, Is.Not.Empty);
            Assert.That(genericCategoryCollectionViewModel.Header, Is.EqualTo("Sprog"));
            Assert.That(genericCategoryCollectionViewModel.Controller, Is.Not.Null);
            Assert.That(genericCategoryCollectionViewModel.Controller, Is.Not.Empty);
            Assert.That(genericCategoryCollectionViewModel.Controller, Is.EqualTo("Common"));
            Assert.That(genericCategoryCollectionViewModel.CreateAction, Is.Not.Null);
            Assert.That(genericCategoryCollectionViewModel.CreateAction, Is.Not.Empty);
            Assert.That(genericCategoryCollectionViewModel.CreateAction, Is.EqualTo("CreateLanguage"));
            Assert.That(genericCategoryCollectionViewModel.UpdateAction, Is.Not.Null);
            Assert.That(genericCategoryCollectionViewModel.UpdateAction, Is.Not.Empty);
            Assert.That(genericCategoryCollectionViewModel.UpdateAction, Is.EqualTo("UpdateLanguage"));
            Assert.That(genericCategoryCollectionViewModel.DeletionUrlGetter, Is.Not.Null);
            Assert.That(genericCategoryCollectionViewModel.AllowCreation, Is.True);
            Assert.That(genericCategoryCollectionViewModel.AllowModification, Is.True);
            Assert.That(genericCategoryCollectionViewModel.AllowDeletion, Is.True);
        }

		[Test]
        [Category("UnitTest")]
        public async Task Languages_WhenLanguagesWasReturnedFromQueryBus_ReturnsViewResultWhereModelIsNotEmpty()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult)await sut.Languages();

            Assert.That(result.Model, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Languages_WhenNoLanguagesWasReturnedFromQueryBus_ReturnsViewResultWhereModelIsEmpty()
        {
            Controller sut = CreateSut(false);

            ViewResult result = (ViewResult)await sut.Languages();

            Assert.That(result.Model, Is.Empty);
        }

        private Controller CreateSut(bool hasLanguageCollection = true, IEnumerable<ILanguage> languageCollection = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<ILanguage>>(It.IsAny<EmptyQuery>()))
                .Returns(Task.FromResult(hasLanguageCollection ? languageCollection ?? _fixture.CreateMany<ILanguage>(_random.Next(5, 10)).ToArray() : null));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object);
        }
    }
}