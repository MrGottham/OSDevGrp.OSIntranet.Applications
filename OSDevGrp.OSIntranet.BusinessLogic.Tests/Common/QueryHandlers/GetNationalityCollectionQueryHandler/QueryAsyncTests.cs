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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Common.QueryHandlers.GetNationalityCollectionQueryHandler
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
            IQueryHandler<EmptyQuery, IEnumerable<INationality>> sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("query"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetNationalitiesAsyncWasCalledOnCommonRepository()
        {
            IQueryHandler<EmptyQuery, IEnumerable<INationality>> sut = CreateSut();

            await sut.QueryAsync(new EmptyQuery());

            _commonRepositoryMock.Verify(m => m.GetNationalitiesAsync(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNationalitiesWasReturnedFromCommonRepository_ReturnsNotNull()
        {
            IQueryHandler<EmptyQuery, IEnumerable<INationality>> sut = CreateSut();

            IEnumerable<INationality> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNationalitiesWasReturnedFromCommonRepository_ReturnsNonEmptyCollectionOfNationalities()
        {
            IQueryHandler<EmptyQuery, IEnumerable<INationality>> sut = CreateSut();

            IEnumerable<INationality> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNationalitiesWasReturnedFromCommonRepository_ReturnsNationalityCollectionFromCommonRepository()
        {
            INationality[] nationalityCollection = BuildNationalityCollection();
            IQueryHandler<EmptyQuery, IEnumerable<INationality>> sut = CreateSut(nationalityCollection: nationalityCollection);

            IEnumerable<INationality> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.EqualTo(nationalityCollection));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoNationalitiesWasReturnedFromCommonRepository_ReturnsNotNull()
        {
            IQueryHandler<EmptyQuery, IEnumerable<INationality>> sut = CreateSut(false);

            IEnumerable<INationality> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoNationalitiesWasReturnedFromCommonRepository_ReturnsEmptyCollectionOfNationalities()
        {
            IQueryHandler<EmptyQuery, IEnumerable<INationality>> sut = CreateSut(false);

            IEnumerable<INationality> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.Empty);
        }

        public IQueryHandler<EmptyQuery, IEnumerable<INationality>> CreateSut(bool hasNationalityCollection = true, IEnumerable<INationality> nationalityCollection = null)
        {
            _commonRepositoryMock.Setup(m => m.GetNationalitiesAsync())
                .Returns(Task.FromResult(hasNationalityCollection? nationalityCollection ?? BuildNationalityCollection() : null));

            return new BusinessLogic.Common.QueryHandlers.GetNationalityCollectionQueryHandler(_commonRepositoryMock.Object);
        }

        private INationality[] BuildNationalityCollection()
        {
            return new[]
            {
                _fixture.BuildNationalityMock().Object,
                _fixture.BuildNationalityMock().Object,
                _fixture.BuildNationalityMock().Object,
                _fixture.BuildNationalityMock().Object,
                _fixture.BuildNationalityMock().Object
            };
        }
    }
}