using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Core.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Core.QueryHandlers.GetGenericCategoryQueryHandlerBase
{
    [TestFixture]
    public class QueryAsyncTests
    {
        #region Methods

        private Mock<IValidator> _validatorMock;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
        }

        [Test]
        [Category("UnitTest")]
        public void QueryAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            IQueryHandler<IGenericCategoryIdentificationQuery, IGenericCategory> sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("query"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnGenericCategoryIdentificationQuery()
        {
            IQueryHandler<IGenericCategoryIdentificationQuery, IGenericCategory> sut = CreateSut();

            Mock<IGenericCategoryIdentificationQuery> genericCategoryIdentificationQueryMock = CreateGenericCategoryIdentificationQueryMock();
            await sut.QueryAsync(genericCategoryIdentificationQueryMock.Object);

            genericCategoryIdentificationQueryMock.Verify(m => m.Validate(It.Is<IValidator>(value => value != null && value == _validatorMock.Object)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertReadFromRepositoryWasCalledOnGetGenericCategoryQueryHandlerBase()
        {
            IQueryHandler<IGenericCategoryIdentificationQuery, IGenericCategory> sut = CreateSut();

            await sut.QueryAsync(CreateGenericCategoryIdentificationQuery());

            Assert.That(((MyGetGenericCategoryQueryHandler)sut).ReadFromRepositoryWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertReadFromRepositoryWasCalledOnGetGenericCategoryQueryHandlerBaseWithGenericCategoryIdentificationQuery()
        {
            IQueryHandler<IGenericCategoryIdentificationQuery, IGenericCategory> sut = CreateSut();

            IGenericCategoryIdentificationQuery genericCategoryIdentificationQuery = CreateGenericCategoryIdentificationQuery();
            await sut.QueryAsync(genericCategoryIdentificationQuery);

            Assert.That(((MyGetGenericCategoryQueryHandler)sut).ReadFromRepositoryWasCalledWithQuery, Is.EqualTo(genericCategoryIdentificationQuery));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoGenericCategoryWasReturnedFromReadFromRepository_ReturnsNull()
        {
            IQueryHandler<IGenericCategoryIdentificationQuery, IGenericCategory> sut = CreateSut(false);

            IGenericCategory result = await sut.QueryAsync(CreateGenericCategoryIdentificationQuery());

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenGenericCategoryWasReturnedFromReadFromRepository_ReturnsNotNull()
        {
            IQueryHandler<IGenericCategoryIdentificationQuery, IGenericCategory> sut = CreateSut();

            IGenericCategory result = await sut.QueryAsync(CreateGenericCategoryIdentificationQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenGenericCategoryWasReturnedFromReadFromRepository_ReturnsGenericCategoryFromReadFromRepository()
        {
            IGenericCategory genericCategory = new Mock<IGenericCategory>().Object;
            IQueryHandler<IGenericCategoryIdentificationQuery, IGenericCategory> sut = CreateSut(genericCategory: genericCategory);

            IGenericCategory result = await sut.QueryAsync(CreateGenericCategoryIdentificationQuery());

            Assert.That(result, Is.EqualTo(genericCategory));
        }

        private IQueryHandler<IGenericCategoryIdentificationQuery, IGenericCategory> CreateSut(bool hasGenericCategory = true, IGenericCategory genericCategory = null)
        {
            return new MyGetGenericCategoryQueryHandler(hasGenericCategory, genericCategory ?? new Mock<IGenericCategory>().Object, _validatorMock.Object);
        }

        private IGenericCategoryIdentificationQuery CreateGenericCategoryIdentificationQuery()
        {
            return CreateGenericCategoryIdentificationQueryMock().Object;
        }

        private Mock<IGenericCategoryIdentificationQuery> CreateGenericCategoryIdentificationQueryMock()
        {
            return new Mock<IGenericCategoryIdentificationQuery>();
        }

        private class MyGetGenericCategoryQueryHandler : BusinessLogic.Core.QueryHandlers.GetGenericCategoryQueryHandlerBase<IGenericCategoryIdentificationQuery, IGenericCategory>
        {
            #region Private variables

            private readonly bool _hasGenericCategory;
            private readonly IGenericCategory _genericCategory;

            #endregion

            #region Constructor

            public MyGetGenericCategoryQueryHandler(bool hasGenericCategory, IGenericCategory genericCategory, IValidator validator) 
                : base(validator)
            {
                NullGuard.NotNull(genericCategory, nameof(genericCategory));

                _hasGenericCategory = hasGenericCategory;
                _genericCategory = genericCategory;
            }

            #endregion

            #region Properties

            public bool ReadFromRepositoryWasCalled { get; private set; }

            public IGenericCategoryIdentificationQuery ReadFromRepositoryWasCalledWithQuery { get; private set; }

            #endregion

            #region Methods

            protected override Task<IGenericCategory> ReadFromRepository(IGenericCategoryIdentificationQuery query)
            {
                NullGuard.NotNull(query, nameof(query));

                ReadFromRepositoryWasCalled = true;
                ReadFromRepositoryWasCalledWithQuery = query;

                return Task.FromResult(_hasGenericCategory ? _genericCategory : null);
            }

            #endregion
        }
    }
}