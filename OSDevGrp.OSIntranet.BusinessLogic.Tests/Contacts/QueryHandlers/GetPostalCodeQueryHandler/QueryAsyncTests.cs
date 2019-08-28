using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
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
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _contactRepositoryMock = new Mock<IContactRepository>();
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
        public async Task QueryAsync_WhenCalled_ReturnsPostalCodeCollectionFromCommonRepository()
        {
            IPostalCode postalCode = _fixture.BuildPostalCodeMock().Object;
            QueryHandler sut = CreateSut(postalCode);

            IGetPostalCodeQuery query = CreateQueryMock().Object;
            IPostalCode result = await sut.QueryAsync(query);

            Assert.That(result, Is.EqualTo(postalCode));
        }

        private QueryHandler CreateSut(IPostalCode postalCode = null)
        {
            _contactRepositoryMock.Setup(m => m.GetPostalCodeAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.Run(() => postalCode ?? _fixture.BuildPostalCodeMock().Object));

            return new QueryHandler(_validatorMock.Object, _contactRepositoryMock.Object);
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
