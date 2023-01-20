using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Common.QueryHandlers.GetLanguageCollectionQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests
    {
        #region Private variables

        private Mock<ICommonRepository> _commonRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commonRepositoryMock = new Mock<ICommonRepository>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void QueryAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            IQueryHandler<EmptyQuery, IEnumerable<ILanguage>> sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("query"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetLanguagesAsyncWasCalledOnCommonRepository()
        {
            IQueryHandler<EmptyQuery, IEnumerable<ILanguage>> sut = CreateSut();

            await sut.QueryAsync(new EmptyQuery());

            _commonRepositoryMock.Verify(m => m.GetLanguagesAsync(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenLanguagesWasReturnedFromCommonRepository_ReturnsNotNull()
        {
            IQueryHandler<EmptyQuery, IEnumerable<ILanguage>> sut = CreateSut();

            IEnumerable<ILanguage> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenLanguagesWasReturnedFromCommonRepository_ReturnsNonEmptyCollectionOfLanguages()
        {
            IQueryHandler<EmptyQuery, IEnumerable<ILanguage>> sut = CreateSut();

            IEnumerable<ILanguage> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenLanguagesWasReturnedFromCommonRepository_ReturnsLanguageCollectionFromCommonRepository()
        {
            ILanguage[] languageCollection = BuildLanguageCollection();
            IQueryHandler<EmptyQuery, IEnumerable<ILanguage>> sut = CreateSut(languageCollection: languageCollection);

            IEnumerable<ILanguage> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.EqualTo(languageCollection));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoLanguagesWasReturnedFromCommonRepository_ReturnsNotNull()
        {
            IQueryHandler<EmptyQuery, IEnumerable<ILanguage>> sut = CreateSut(false);

            IEnumerable<ILanguage> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoLanguagesWasReturnedFromCommonRepository_ReturnsEmptyCollectionOfLanguages()
        {
            IQueryHandler<EmptyQuery, IEnumerable<ILanguage>> sut = CreateSut(false);

            IEnumerable<ILanguage> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Empty);
        }

        public IQueryHandler<EmptyQuery, IEnumerable<ILanguage>> CreateSut(bool hasLanguageCollection = true, IEnumerable<ILanguage> languageCollection = null)
        {
            _commonRepositoryMock.Setup(m => m.GetLanguagesAsync())
                .Returns(Task.FromResult(hasLanguageCollection ? languageCollection ?? BuildLanguageCollection() : null));

            return new BusinessLogic.Common.QueryHandlers.GetLanguageCollectionQueryHandler(_commonRepositoryMock.Object);
        }

        private ILanguage[] BuildLanguageCollection()
        {
            return new[]
            {
                _fixture.BuildLanguageMock().Object,
                _fixture.BuildLanguageMock().Object,
                _fixture.BuildLanguageMock().Object,
                _fixture.BuildLanguageMock().Object,
                _fixture.BuildLanguageMock().Object
            };
        }
    }
}