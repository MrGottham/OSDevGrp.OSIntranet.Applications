using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Common.QueryHandlers.GetLanguageQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<ICommonRepository> _commonRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _commonRepositoryMock = new Mock<ICommonRepository>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void QueryAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            IQueryHandler<IGetLanguageQuery, ILanguage> sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("query"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnGetLanguageQuery()
        {
            IQueryHandler<IGetLanguageQuery, ILanguage> sut = CreateSut();

            Mock<IGetLanguageQuery> getLanguageQueryMock = CreateGetLanguageQueryMock();
            await sut.QueryAsync(getLanguageQueryMock.Object);

            getLanguageQueryMock.Verify(m => m.Validate(It.Is<IValidator>(value => value != null && value == _validatorMock.Object)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertNumberWasCalledOnGetLanguageQuery()
        {
            IQueryHandler<IGetLanguageQuery, ILanguage> sut = CreateSut();

            Mock<IGetLanguageQuery> getLanguageQueryMock = CreateGetLanguageQueryMock();
            await sut.QueryAsync(getLanguageQueryMock.Object);

            getLanguageQueryMock.Verify(m => m.Number, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetLanguageAsyncWasCalledOnCommonRepository()
        {
            IQueryHandler<IGetLanguageQuery, ILanguage> sut = CreateSut();

            int number = _fixture.Create<int>();
            await sut.QueryAsync(CreateGetLanguageQuery(number));

            _commonRepositoryMock.Verify(m => m.GetLanguageAsync(It.Is<int>(value => value == number)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenLanguageWasReturnedFromCommonRepository_ReturnsNotNull()
        {
            IQueryHandler<IGetLanguageQuery, ILanguage> sut = CreateSut();

            ILanguage result = await sut.QueryAsync(CreateGetLanguageQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenLanguageWasReturnedFromCommonRepository_ReturnsLanguageFromCommonRepository()
        {
            ILanguage language = _fixture.BuildLanguageMock().Object;
            IQueryHandler<IGetLanguageQuery, ILanguage> sut = CreateSut(language: language);

            ILanguage result = await sut.QueryAsync(CreateGetLanguageQuery());

            Assert.That(result, Is.EqualTo(language));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoLanguageWasReturnedFromCommonRepository_ReturnsNull()
        {
            IQueryHandler<IGetLanguageQuery, ILanguage> sut = CreateSut(false);

            ILanguage result = await sut.QueryAsync(CreateGetLanguageQuery());

            Assert.That(result, Is.Null);
        }

        private IQueryHandler<IGetLanguageQuery, ILanguage> CreateSut(bool hasLanguage = true, ILanguage language = null)
        {
            _commonRepositoryMock.Setup(m => m.GetLanguageAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(hasLanguage ? language ?? _fixture.BuildLanguageMock().Object : null));

            return new BusinessLogic.Common.QueryHandlers.GetLanguageQueryHandler(_validatorMock.Object, _commonRepositoryMock.Object);
        }

        private IGetLanguageQuery CreateGetLanguageQuery(int? number = null)
        {
            return CreateGetLanguageQueryMock(number).Object;
        }

        private Mock<IGetLanguageQuery> CreateGetLanguageQueryMock(int? number = null)
        {
            Mock<IGetLanguageQuery> getLanguageQueryMock = new Mock<IGetLanguageQuery>();
            getLanguageQueryMock.Setup(m => m.Number)
                .Returns(number ?? _fixture.Create<int>());
            return getLanguageQueryMock;
        }
    }
}