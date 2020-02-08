using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Logic;
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
        private Mock<ICountryHelper> _countryHelperMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _contactRepositoryMock = new Mock<IContactRepository>();
            _countryHelperMock = new Mock<ICountryHelper>();
            
            _fixture = new Fixture();
            _fixture.Customize<ICountry>(builder => builder.FromFactory(() => _fixture.BuildCountryMock().Object));
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
        public async Task QueryAsync_WhenCalledAndPostalCodeCollectionWasNotReturnedFromContactRepository_AssertApplyLogicForPrincipalWasNotCalledOnCountryHelperForAnyPostalCode()
        {
            QueryHandler sut = CreateSut(false);

            IGetPostalCodeCollectionQuery query = CreateQueryMock().Object;
            await sut.QueryAsync(query);

            _countryHelperMock.Verify(m => m.ApplyLogicForPrincipal(It.IsAny<ICountry>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledAndPostalCodeCollectionWasNotReturnedFromContactRepository_AssertCountrySetterWasNotCalledOnAnyPostalCode()
        {
            IEnumerable<Mock<IPostalCode>> postalCodeMockCollection = new List<Mock<IPostalCode>>
            {
                _fixture.BuildPostalCodeMock(),
                _fixture.BuildPostalCodeMock(),
                _fixture.BuildPostalCodeMock()
            };
            QueryHandler sut = CreateSut(false, postalCodeMockCollection.Select(m => m.Object).ToList());

            IGetPostalCodeCollectionQuery query = CreateQueryMock().Object;
            await sut.QueryAsync(query);

            foreach (Mock<IPostalCode> postalCodeMock in postalCodeMockCollection)
            {
                postalCodeMock.VerifySet(m => m.Country = It.IsAny<ICountry>(), Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledAndPostalCodeCollectionWasNotReturnedFromContactRepository_ReturnsEmptyPostalCodeCollection()
        {
            QueryHandler sut = CreateSut(false);

            IGetPostalCodeCollectionQuery query = CreateQueryMock().Object;
            IList<IPostalCode> result = (await sut.QueryAsync(query)).ToList();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledAndPostalCodeCollectionWasReturnedFromContactRepository_AssertApplyLogicForPrincipalWasCalledOnCountryHelperForEachPostalCodeFromContactRepository()
        {
            IEnumerable<IPostalCode> postalCodeCollection = _fixture.CreateMany<IPostalCode>(_random.Next(10, 25)).ToList();
            QueryHandler sut = CreateSut(postalCodeCollection: postalCodeCollection);

            IGetPostalCodeCollectionQuery query = CreateQueryMock().Object;
            await sut.QueryAsync(query);

            foreach (IPostalCode postalCode in postalCodeCollection)
            {
                _countryHelperMock.Verify(m => m.ApplyLogicForPrincipal(It.Is<ICountry>(value => value == postalCode.Country)), Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledAndPostalCodeCollectionWasReturnedFromContactRepository_AssertCountrySetterWasCalledOnEachPostalCodeFromContactRepository()
        {
            IEnumerable<Mock<IPostalCode>> postalCodeMockCollection = new List<Mock<IPostalCode>>
            {
                _fixture.BuildPostalCodeMock(),
                _fixture.BuildPostalCodeMock(),
                _fixture.BuildPostalCodeMock()
            };
            ICountry country = _fixture.BuildCountryMock().Object;
            QueryHandler sut = CreateSut(postalCodeCollection: postalCodeMockCollection.Select(m => m.Object).ToList(), country: country);

            IGetPostalCodeCollectionQuery query = CreateQueryMock().Object;
            await sut.QueryAsync(query);

            foreach (Mock<IPostalCode> postalCodeMock in postalCodeMockCollection)
            {
                postalCodeMock.VerifySet(m => m.Country = It.Is<ICountry>(value => value == country), Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledAndPostalCodeCollectionWasReturnedFromContactRepository_ReturnsPostalCodeCollectionFromContactRepository()
        {
            IEnumerable<IPostalCode> postalCodeCollection = _fixture.CreateMany<IPostalCode>(_random.Next(10, 25)).ToList();
            QueryHandler sut = CreateSut(postalCodeCollection: postalCodeCollection);

            IGetPostalCodeCollectionQuery query = CreateQueryMock().Object;
            IEnumerable<IPostalCode> result = await sut.QueryAsync(query);

            Assert.That(result, Is.EqualTo(postalCodeCollection));
        }

        private QueryHandler CreateSut(bool hasPostalCodeCollection = true, IEnumerable<IPostalCode> postalCodeCollection = null, ICountry country = null)
        {
            _contactRepositoryMock.Setup(m => m.GetPostalCodesAsync(It.IsAny<string>()))
                .Returns(Task.Run(() => hasPostalCodeCollection ? postalCodeCollection ?? _fixture.CreateMany<IPostalCode>(_random.Next(10, 25)).ToList() : null));
            _countryHelperMock.Setup(m => m.ApplyLogicForPrincipal(It.IsAny<ICountry>()))
                .Returns(country ?? _fixture.BuildCountryMock().Object);

            return new QueryHandler(_validatorMock.Object, _contactRepositoryMock.Object, _countryHelperMock.Object);
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
