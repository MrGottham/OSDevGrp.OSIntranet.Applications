using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using QueryHandler=OSDevGrp.OSIntranet.BusinessLogic.Contacts.QueryHandlers.GetCountryCollectionQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.QueryHandlers.GetCountryCollectionQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests
    {
        #region Private variables

        private Mock<IContactRepository> _contactRepositoryMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _contactRepositoryMock = new Mock<IContactRepository>();

            _fixture = new Fixture();
            _fixture.Customize<ICountry>(builder => builder.FromFactory(() => _fixture.BuildCountryMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void QueryAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            QueryHandler sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("query"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetCountriesAsyncWasCalledOnContactRepository()
        {
            QueryHandler sut = CreateSut();

            await sut.QueryAsync(new EmptyQuery());

            _contactRepositoryMock.Verify(m => m.GetCountriesAsync(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsCountryCollectionFromContactRepository()
        {
            IEnumerable<ICountry> countryCollection = _fixture.CreateMany<ICountry>(_random.Next(5, 10)).ToList();
            QueryHandler sut = CreateSut(countryCollection);

            IEnumerable<ICountry> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.EqualTo(countryCollection));
        }

        private QueryHandler CreateSut(IEnumerable<ICountry> countryCollection = null)
        {
            _contactRepositoryMock.Setup(m => m.GetCountriesAsync())
                .Returns(Task.Run(() => countryCollection ?? _fixture.CreateMany<ICountry>(_random.Next(5, 10)).ToList()));

            return new QueryHandler(_contactRepositoryMock.Object);
        }
    }
}
