using System;
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
using QueryHandler=OSDevGrp.OSIntranet.BusinessLogic.Contacts.QueryHandlers.GetPostalCodeQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.QueryHandlers.GetPostalCodeQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<IContactRepository> _contactRepositoryMock;
        private Mock<ICountryHelper> _countryHelperMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _contactRepositoryMock = new Mock<IContactRepository>();
            _countryHelperMock = new Mock<ICountryHelper>();

            _fixture = new Fixture();
            _fixture.Customize<ICountry>(builder => builder.FromFactory(() => _fixture.BuildCountryMock().Object));
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
        public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnGetPostalCodeQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetPostalCodeQuery> query = CreateQueryMock();
            await sut.QueryAsync(query.Object);

            query.Verify(m => m.Validate(
                    It.Is<IValidator>(value => value == _validatorMock.Object),
                    It.Is<IContactRepository>(value => value == _contactRepositoryMock.Object)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertCountryCodeWasCalledOnGetPostalCodeQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetPostalCodeQuery> query = CreateQueryMock();
            await sut.QueryAsync(query.Object);

            query.Verify(m => m.CountryCode, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertPostalCodeWasCalledOnGetPostalCodeQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetPostalCodeQuery> query = CreateQueryMock();
            await sut.QueryAsync(query.Object);

            query.Verify(m => m.PostalCode, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetPostalCodeAsyncWasCalledOnContactRepository()
        {
            QueryHandler sut = CreateSut();

            string countryCode = _fixture.Create<string>();
            string postalCode = _fixture.Create<string>();
            IGetPostalCodeQuery query = CreateQueryMock(countryCode, postalCode).Object;
            await sut.QueryAsync(query);

            _contactRepositoryMock.Verify(m => m.GetPostalCodeAsync(
                    It.Is<string>(value => string.CompareOrdinal(value, countryCode) == 0),
                    It.Is<string>(value => string.CompareOrdinal(value, postalCode) == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledAndPostalCodeWasNotReturnedFromContactRepository_AssertApplyLogicForPrincipalWasNotCalledOnCountryHelper()
        {
            QueryHandler sut = CreateSut(false);

            IGetPostalCodeQuery query = CreateQueryMock().Object;
            await sut.QueryAsync(query);

            _countryHelperMock.Verify(m => m.ApplyLogicForPrincipal(It.IsAny<ICountry>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledAndPostalCodeWasNotReturnedFromContactRepository_AssertCountrySetterWasNotCalledOnPostalCode()
        {
            Mock<IPostalCode> postalCodeMock = _fixture.BuildPostalCodeMock();
            QueryHandler sut = CreateSut(false, postalCodeMock.Object);

            IGetPostalCodeQuery query = CreateQueryMock().Object;
            await sut.QueryAsync(query);

            postalCodeMock.VerifySet(m => m.Country = It.IsAny<ICountry>(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledAndPostalCodeWasNotReturnedFromContactRepository_ReturnsNull()
        {
            QueryHandler sut = CreateSut(false);

            IGetPostalCodeQuery query = CreateQueryMock().Object;
            IPostalCode result = await sut.QueryAsync(query);

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledAndPostalCodeWasReturnedFromContactRepository_AssertApplyLogicForPrincipalWasCalledOnCountryHelper()
        {
            ICountry country = _fixture.Create<ICountry>();
            IPostalCode postalCode = _fixture.BuildPostalCodeMock(country).Object;
            QueryHandler sut = CreateSut(postalCode: postalCode);

            IGetPostalCodeQuery query = CreateQueryMock().Object;
            await sut.QueryAsync(query);

            _countryHelperMock.Verify(m => m.ApplyLogicForPrincipal(It.Is<ICountry>(value => value == country)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledAndPostalCodeWasReturnedFromContactRepository_AssertCountrySetterWasCalledOnPostalCode()
        {
            Mock<IPostalCode> postalCodeMock = _fixture.BuildPostalCodeMock();
            ICountry country = _fixture.Create<ICountry>();
            QueryHandler sut = CreateSut(postalCode: postalCodeMock.Object, country: country);

            IGetPostalCodeQuery query = CreateQueryMock().Object;
            await sut.QueryAsync(query);

            postalCodeMock.VerifySet(m => m.Country = It.Is<ICountry>(value => value == country), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledAndPostalCodeWasReturnedFromContactRepository_ReturnsPostalCodeCollectionFromContactRepository()
        {
            IPostalCode postalCode = _fixture.BuildPostalCodeMock().Object;
            QueryHandler sut = CreateSut(postalCode: postalCode);

            IGetPostalCodeQuery query = CreateQueryMock().Object;
            IPostalCode result = await sut.QueryAsync(query);

            Assert.That(result, Is.EqualTo(postalCode));
        }

        private QueryHandler CreateSut(bool hasPostalCode = true, IPostalCode postalCode = null, ICountry country = null)
        {
            _contactRepositoryMock.Setup(m => m.GetPostalCodeAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.Run(() => hasPostalCode ? postalCode ?? _fixture.BuildPostalCodeMock().Object : null));
            _countryHelperMock.Setup(m => m.ApplyLogicForPrincipal(It.IsAny<ICountry>()))
                .Returns(country ?? _fixture.BuildCountryMock().Object);

            return new QueryHandler(_validatorMock.Object, _contactRepositoryMock.Object, _countryHelperMock.Object);
        }

        private Mock<IGetPostalCodeQuery> CreateQueryMock(string countryCode = null, string postalCode = null)
        {
            Mock<IGetPostalCodeQuery> queryMock = new Mock<IGetPostalCodeQuery>();
            queryMock.Setup(m => m.CountryCode)
                .Returns(countryCode ?? _fixture.Create<string>());
            queryMock.Setup(m => m.PostalCode)
                .Returns(postalCode ?? _fixture.Create<string>());
            return queryMock;
        }
    }
}
