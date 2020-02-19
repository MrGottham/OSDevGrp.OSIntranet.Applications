using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts.Enums;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using QueryHandler=OSDevGrp.OSIntranet.BusinessLogic.Contacts.QueryHandlers.GetMatchingContactCollectionQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.QueryHandlers.GetMatchingContactCollectionQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests
    {
        #region Private variables

        private ValidatorMockContext _validatorMockContext;
        private Mock<IMicrosoftGraphRepository> _microsoftGraphRepositoryMock;
        private Mock<IContactRepository> _contactRepositoryMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMockContext = new ValidatorMockContext();
            _microsoftGraphRepositoryMock = new Mock<IMicrosoftGraphRepository>();
            _contactRepositoryMock = new Mock<IContactRepository>();

            _fixture = new Fixture();
            _fixture.Customize<IContactGroup>(builder => builder.FromFactory(() => _fixture.BuildContactGroupMock().Object));
            _fixture.Customize<IPaymentTerm>(builder => builder.FromFactory(() => _fixture.BuildPaymentTermMock().Object));
            _fixture.Customize<ICompany>(builder => builder.FromFactory(() => _fixture.BuildCompanyMock().Object));
            _fixture.Customize<IContact>(builder => builder.FromFactory(() => _fixture.BuildContactMock().Object));

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
        public async Task QueryAsync_WhenCalled_AssertSearchForWasCalledOnGetMatchingContactCollectionQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetMatchingContactCollectionQuery> queryMock = CreateGetMatchingContactCollectionQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.SearchFor, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertSearchOptionsWasCalledOnGetMatchingContactCollectionQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetMatchingContactCollectionQuery> queryMock = CreateGetMatchingContactCollectionQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.SearchOptions, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetContactsAsyncWasCalledOnMicrosoftGraphRepository()
        {
            QueryHandler sut = CreateSut();

            IRefreshableToken token = _fixture.BuildRefreshableTokenMock().Object;
            IGetMatchingContactCollectionQuery query = CreateGetMatchingContactCollectionQueryMock(refreshableToken: token).Object;
            await sut.QueryAsync(query);

            _microsoftGraphRepositoryMock.Verify(m => m.GetContactsAsync(It.Is<IRefreshableToken>(value => value == token)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledAndContactCollectionWasNotReturnedFromMicrosoftGraphRepository_AssertIsMatchWasNotCalledOnAnyContact()
        {
            IEnumerable<Mock<IContact>> microsoftGraphContactMockCollection = new List<Mock<IContact>>
            {
                _fixture.BuildContactMock(),
                _fixture.BuildContactMock(),
                _fixture.BuildContactMock(),
                _fixture.BuildContactMock(),
                _fixture.BuildContactMock(),
                _fixture.BuildContactMock(),
                _fixture.BuildContactMock()
            };
            QueryHandler sut = CreateSut(false, microsoftGraphContactMockCollection.Select(m => m.Object).ToArray());

            IGetMatchingContactCollectionQuery query = CreateGetMatchingContactCollectionQueryMock().Object;
            await sut.QueryAsync(query);

            foreach (Mock<IContact> contactMock in microsoftGraphContactMockCollection)
            {
                contactMock.Verify(m => m.IsMatch(
                        It.IsAny<string>(),
                        It.IsAny<SearchOptions>()),
                    Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledAndContactCollectionWasNotReturnedFromMicrosoftGraphRepository_AssertApplyContactSupplementAsyncWasNotCalledOnContactRepository()
        {
            QueryHandler sut = CreateSut(false);

            IGetMatchingContactCollectionQuery query = CreateGetMatchingContactCollectionQueryMock().Object;
            await sut.QueryAsync(query);

            _contactRepositoryMock.Verify(m => m.ApplyContactSupplementAsync(It.IsAny<IEnumerable<IContact>>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledAndContactCollectionWasNotReturnedFromMicrosoftGraphRepository_ReturnsEmptyContactCollection()
        {
            QueryHandler sut = CreateSut(false);

            IGetMatchingContactCollectionQuery query = CreateGetMatchingContactCollectionQueryMock().Object;
            IList<IContact> result = (await sut.QueryAsync(query)).ToList();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledAndContactCollectionWasReturnedFromMicrosoftGraphRepository_AssertIsMatchWasCalledOnEachContactWithinContactCollectionFromMicrosoftGraphRepository()
        {
            IEnumerable<Mock<IContact>> microsoftGraphContactMockCollection = new List<Mock<IContact>>
            {
                _fixture.BuildContactMock(),
                _fixture.BuildContactMock(),
                _fixture.BuildContactMock(),
                _fixture.BuildContactMock(),
                _fixture.BuildContactMock(),
                _fixture.BuildContactMock(),
                _fixture.BuildContactMock()
            };
            QueryHandler sut = CreateSut(microsoftGraphContactCollection: microsoftGraphContactMockCollection.Select(m => m.Object).ToArray());

            string searchFor = _fixture.Create<string>();
            SearchOptions searchOptions = _fixture.Create<SearchOptions>();
            IGetMatchingContactCollectionQuery query = CreateGetMatchingContactCollectionQueryMock(searchFor, searchOptions).Object;
            await sut.QueryAsync(query);

            foreach (Mock<IContact> contactMock in microsoftGraphContactMockCollection)
            {
                contactMock.Verify(m => m.IsMatch(
                        It.Is<string>(value => string.CompareOrdinal(value, searchFor) == 0),
                        It.Is<SearchOptions>(value => value == searchOptions)),
                    Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledAndContactCollectionWasReturnedFromMicrosoftGraphRepository_AssertApplyContactSupplementAsyncWasCalledOnContactRepositoryWithMatchingContactCollection()
        {
            IEnumerable<IContact> microsoftGraphContactCollection = new List<IContact>
            {
                _fixture.BuildContactMock(isMatch: true).Object,
                _fixture.BuildContactMock(isMatch: false).Object,
                _fixture.BuildContactMock(isMatch: true).Object,
                _fixture.BuildContactMock(isMatch: false).Object,
                _fixture.BuildContactMock(isMatch: true).Object,
                _fixture.BuildContactMock(isMatch: false).Object,
                _fixture.BuildContactMock(isMatch: true).Object
            };
            QueryHandler sut = CreateSut(microsoftGraphContactCollection: microsoftGraphContactCollection);

            IGetMatchingContactCollectionQuery query = CreateGetMatchingContactCollectionQueryMock().Object;
            await sut.QueryAsync(query);

            _contactRepositoryMock.Verify(m => m.ApplyContactSupplementAsync(It.Is<IEnumerable<IContact>>(value => value.All(contact => contact.IsMatch(It.IsAny<string>(), It.IsAny<SearchOptions>())))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalledAndAppliedContactSupplementCollectionWasReturnedFromContactRepository_ReturnsAppliedContactSupplementCollectionFromContactRepository()
        {
            IEnumerable<IContact> appliedContactSupplementCollection = _fixture.CreateMany<IContact>(_random.Next(5, 15)).ToList();
            QueryHandler sut = CreateSut(appliedContactSupplementCollection: appliedContactSupplementCollection);

            IGetMatchingContactCollectionQuery query = CreateGetMatchingContactCollectionQueryMock().Object;
            IEnumerable<IContact> result = await sut.QueryAsync(query);

            Assert.That(result, Is.EqualTo(appliedContactSupplementCollection));
        }

        private QueryHandler CreateSut(bool hasMicrosoftGraphContactCollection = true, IEnumerable<IContact> microsoftGraphContactCollection = null, IEnumerable<IContact> appliedContactSupplementCollection = null)
        {
            _microsoftGraphRepositoryMock.Setup(m => m.GetContactsAsync(It.IsAny<IRefreshableToken>()))
                .Returns(Task.Run(() => hasMicrosoftGraphContactCollection ? microsoftGraphContactCollection ?? _fixture.CreateMany<IContact>(_random.Next(5, 15)).ToList() : null));
            _contactRepositoryMock.Setup(m => m.ApplyContactSupplementAsync(It.IsAny<IEnumerable<IContact>>()))
                .Returns(Task.Run(() => appliedContactSupplementCollection ?? _fixture.CreateMany<IContact>(_random.Next(5, 15)).ToList()));

            return new QueryHandler(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object);
        }

        private Mock<IGetMatchingContactCollectionQuery> CreateGetMatchingContactCollectionQueryMock(string searchFor = null, SearchOptions? searchOptions = null, IRefreshableToken refreshableToken = null)
        {
            Mock<IGetMatchingContactCollectionQuery> getMatchingContactCollectionQueryMock = new Mock<IGetMatchingContactCollectionQuery>();
            getMatchingContactCollectionQueryMock.Setup(m => m.SearchFor)
                .Returns(searchFor ?? _fixture.Create<string>());
            getMatchingContactCollectionQueryMock.Setup(m => m.SearchOptions)
                .Returns(searchOptions ?? _fixture.Create<SearchOptions>());
            getMatchingContactCollectionQueryMock.Setup(m => m.ToToken())
                .Returns(refreshableToken ?? _fixture.BuildRefreshableTokenMock().Object);
            return getMatchingContactCollectionQueryMock;
        }
    }
}