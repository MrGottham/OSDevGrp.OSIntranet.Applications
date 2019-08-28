using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using QueryHandler=OSDevGrp.OSIntranet.BusinessLogic.Contacts.QueryHandlers.GetPostalCodeCollectionQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.QueryHandlers.GetPostalCodeCollectionQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<IContactRepository> _contactRepositoryMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _contactRepositoryMock = new Mock<IContactRepository>();
            
            _fixture = new Fixture();
            _fixture.Customize<IPostalCode>(builder => builder.FromFactory(() => _fixture.BuildPostalCodeMock().Object));
            
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
        public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnGetPostalCodeCollectionQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetPostalCodeCollectionQuery> query = CreateQueryMock();
            await sut.QueryAsync(query.Object);

            query.Verify(m => m.Validate(
                    It.Is<IValidator>(value => value == _validatorMock.Object),
                    It.Is<IContactRepository>(value => value == _contactRepositoryMock.Object)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertCountryCodeWasCalledOnGetPostalCodeCollectionQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetPostalCodeCollectionQuery> query = CreateQueryMock();
            await sut.QueryAsync(query.Object);

            query.Verify(m => m.CountryCode, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetPostalCodesAsyncWasCalledOnContactRepository()
        {
            QueryHandler sut = CreateSut();

            string countryCode = _fixture.Create<string>();
            IGetPostalCodeCollectionQuery query = CreateQueryMock(countryCode).Object;
            await sut.QueryAsync(query);

            _contactRepositoryMock.Verify(m => m.GetPostalCodesAsync(It.Is<string>(value => string.CompareOrdinal(value, countryCode) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsPostalCodeCollectionFromCommonRepository()
        {
            IEnumerable<IPostalCode> postalCodeCollection = _fixture.CreateMany<IPostalCode>(_random.Next(10, 25)).ToList();
            QueryHandler sut = CreateSut(postalCodeCollection);

            IGetPostalCodeCollectionQuery query = CreateQueryMock().Object;
            IEnumerable<IPostalCode> result = await sut.QueryAsync(query);

            Assert.That(result, Is.EqualTo(postalCodeCollection));
        }

        private QueryHandler CreateSut(IEnumerable<IPostalCode> postalCodeCollection = null)
        {
            _contactRepositoryMock.Setup(m => m.GetPostalCodesAsync(It.IsAny<string>()))
                .Returns(Task.Run(() => postalCodeCollection ?? _fixture.CreateMany<IPostalCode>(_random.Next(10, 25)).ToList()));

            return new QueryHandler(_validatorMock.Object, _contactRepositoryMock.Object);
        }

        private Mock<IGetPostalCodeCollectionQuery> CreateQueryMock(string countryCode = null)
        {
            Mock<IGetPostalCodeCollectionQuery> queryMock = new Mock<IGetPostalCodeCollectionQuery>();
            queryMock.Setup(m => m.CountryCode)
                .Returns(countryCode ?? _fixture.Create<string>());
            return queryMock;
        }
    }
}
