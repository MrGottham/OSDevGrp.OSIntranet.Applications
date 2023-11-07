using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Core.QueryHandlers;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Core.QueryHandlers.GetGenericCategoryCollectionQueryHandlerBase
{
    [TestFixture]
    public class QueryAsyncTests
    {
        [Test]
        [Category("UnitTest")]
        public void QueryAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            IQueryHandler<EmptyQuery, IEnumerable<IGenericCategory>> sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("query"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertReadFromRepositoryWasCalledOnGetGenericCategoryCollectionQueryHandlerBase()
        {
            IQueryHandler<EmptyQuery, IEnumerable<IGenericCategory>> sut = CreateSut();

            await sut.QueryAsync(new EmptyQuery());

            Assert.That(((MyGetGenericCategoryCollectionQueryHandler)sut).ReadFromRepositoryWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoGenericCategoriesWasReturnedFromReadFromRepository_ReturnsNotNull()
        {
            IQueryHandler<EmptyQuery, IEnumerable<IGenericCategory>> sut = CreateSut(false);

            IEnumerable<IGenericCategory> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoGenericCategoriesWasReturnedFromReadFromRepository_ReturnsEmptyCollectionOfGenericCategories()
        {
            IQueryHandler<EmptyQuery, IEnumerable<IGenericCategory>> sut = CreateSut(false);

            IEnumerable<IGenericCategory> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenGenericCategoriesWasReturnedFromReadFromRepository_ReturnsNotNull()
        {
            IQueryHandler<EmptyQuery, IEnumerable<IGenericCategory>> sut = CreateSut();

            IEnumerable<IGenericCategory> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenGenericCategoriesWasReturnedFromReadFromRepository_ReturnsNonEmptyCollectionOfGenericCategories()
        {
            IQueryHandler<EmptyQuery, IEnumerable<IGenericCategory>> sut = CreateSut();

            IEnumerable<IGenericCategory> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenGenericCategoriesWasReturnedFromReadFromRepository_ReturnsGenericCategoryCollectionFromReadFromRepository()
        {
            IGenericCategory[] genericCategoryCollection = BuildGenericCategoryCollection();
            IQueryHandler<EmptyQuery, IEnumerable<IGenericCategory>> sut = CreateSut(genericCategoryCollection: genericCategoryCollection);

            IEnumerable<IGenericCategory> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.EqualTo(genericCategoryCollection));
        }

        private IQueryHandler<EmptyQuery, IEnumerable<IGenericCategory>> CreateSut(bool hasGenericCategoryCollection = true, IEnumerable<IGenericCategory> genericCategoryCollection = null)
        {
            return new MyGetGenericCategoryCollectionQueryHandler(hasGenericCategoryCollection, genericCategoryCollection ?? BuildGenericCategoryCollection());
        }

        private static IGenericCategory[] BuildGenericCategoryCollection()
        {
            return new[]
            {
                new Mock<IGenericCategory>().Object,
                new Mock<IGenericCategory>().Object,
                new Mock<IGenericCategory>().Object,
                new Mock<IGenericCategory>().Object,
                new Mock<IGenericCategory>().Object
            };
        }

        private class MyGetGenericCategoryCollectionQueryHandler : GetGenericCategoryCollectionQueryHandlerBase<IGenericCategory>
        {
            #region Private variables

            private readonly bool _hasGenericCategoryCollection;
            private readonly IEnumerable<IGenericCategory> _genericCategoryCollection;

            #endregion

            #region Constructor

            public MyGetGenericCategoryCollectionQueryHandler(bool hasGenericCategoryCollection, IEnumerable<IGenericCategory> genericCategoryCollection)
            {
                NullGuard.NotNull(genericCategoryCollection, nameof(genericCategoryCollection));

                _hasGenericCategoryCollection = hasGenericCategoryCollection;
                _genericCategoryCollection = genericCategoryCollection;
            }

            #endregion

            #region Properties

            public bool ReadFromRepositoryWasCalled { get; private set; }

            #endregion

            #region Methods

            protected override Task<IEnumerable<IGenericCategory>> ReadFromRepository()
            {
                ReadFromRepositoryWasCalled = true;

                return Task.FromResult(_hasGenericCategoryCollection ? _genericCategoryCollection : null);
            }

            #endregion
        }
    }
}