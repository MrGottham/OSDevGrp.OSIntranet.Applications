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
using QueryHandler=OSDevGrp.OSIntranet.BusinessLogic.Contacts.QueryHandlers.GetCountryQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.QueryHandlers.GetCountryQueryHandler
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
        public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnGetCountryQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetCountryQuery> query = CreateQueryMock();
            await sut.QueryAsync(query.Object);

            query.Verify(m => m.Validate(
                    It.Is<IValidator>(value => value == _validatorMock.Object),
                    It.Is<IContactRepository>(value => value == _contactRepositoryMock.Object)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertCountryCodeWasCalledOnGetCountryQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetCountryQuery> query = CreateQueryMock();
            await sut.QueryAsync(query.Object);

            query.Verify(m => m.CountryCode, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetCountryAsyncWasCalledOnContactRepository()
        {
            QueryHandler sut = CreateSut();

            string countryCode = _fixture.Create<string>();
            IGetCountryQuery query = CreateQueryMock(countryCode).Object;
            await sut.QueryAsync(query);

            _contactRepositoryMock.Verify(m => m.GetCountryAsync(It.Is<string>(value => string.CompareOrdinal(value, countryCode) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledAndCountryWasNotReturnedFromContactRepository_AssertApplyLogicForPrincipalWasNotCalledOnCountryHelper()
        {
            QueryHandler sut = CreateSut(false);

            IGetCountryQuery query = CreateQueryMock().Object;
            await sut.QueryAsync(query);

            _countryHelperMock.Verify(m => m.ApplyLogicForPrincipal(It.IsAny<ICountry>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledAndCountryWasNotReturnedFromContactRepository_ReturnsNull()
        {
            QueryHandler sut = CreateSut(false);

            IGetCountryQuery query = CreateQueryMock().Object;
            ICountry result = await sut.QueryAsync(query);

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledAndCountryWasReturnedFromContactRepository_AssertApplyLogicForPrincipalWasCalledOnCountryHelperWithCountryFromContactRepository()
        {
            ICountry country = _fixture.BuildCountryMock().Object;
            QueryHandler sut = CreateSut(country: country);

            IGetCountryQuery query = CreateQueryMock().Object;
            await sut.QueryAsync(query);

            _countryHelperMock.Verify(m => m.ApplyLogicForPrincipal(It.Is<ICountry>(value => value == country)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledAndCountryWasReturnedFromContactRepository_ReturnsCountryFromCountryHelper()
        {
            ICountry country = _fixture.BuildCountryMock().Object;
            QueryHandler sut = CreateSut(country: country);

            IGetCountryQuery query = CreateQueryMock().Object;
            ICountry result = await sut.QueryAsync(query);

            Assert.That(result, Is.EqualTo(country));
        }

        private QueryHandler CreateSut(bool hasCountry = true, ICountry country = null)
        {
            _contactRepositoryMock.Setup(m => m.GetCountryAsync(It.IsAny<string>()))
                .Returns(Task.Run(() => hasCountry ? country ?? _fixture.BuildCountryMock().Object : null));
            _countryHelperMock.Setup(m => m.ApplyLogicForPrincipal(It.IsAny<ICountry>()))
                .Returns(country ?? _fixture.BuildCountryMock().Object);

            return new QueryHandler(_validatorMock.Object, _contactRepositoryMock.Object, _countryHelperMock.Object);
        }

        private Mock<IGetCountryQuery> CreateQueryMock(string countryCode = null)
        {
            Mock<IGetCountryQuery> queryMock = new Mock<IGetCountryQuery>();
            queryMock.Setup(m => m.CountryCode)
                .Returns(countryCode ?? _fixture.Create<string>());
            return queryMock;
        }
    }
}
