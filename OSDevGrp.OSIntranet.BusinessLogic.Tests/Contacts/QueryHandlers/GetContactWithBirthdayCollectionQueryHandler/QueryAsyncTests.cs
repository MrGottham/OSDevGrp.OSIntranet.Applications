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
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using QueryHandler=OSDevGrp.OSIntranet.BusinessLogic.Contacts.QueryHandlers.GetContactWithBirthdayCollectionQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.QueryHandlers.GetContactWithBirthdayCollectionQueryHandler
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
        public async Task QueryAsync_WhenCalled_AssertBirthdayWithinDaysWasCalledOnGetContactWithBirthdayCollectionQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetContactWithBirthdayCollectionQuery> queryMock = CreateGetContactWithBirthdayCollectionQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.BirthdayWithinDays, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetContactsAsyncWasCalledOnMicrosoftGraphRepository()
        {
            QueryHandler sut = CreateSut();

            IRefreshableToken token = _fixture.BuildRefreshableTokenMock().Object;
            IGetContactWithBirthdayCollectionQuery query = CreateGetContactWithBirthdayCollectionQueryMock(refreshableToken: token).Object;
            await sut.QueryAsync(query);

            _microsoftGraphRepositoryMock.Verify(m => m.GetContactsAsync(It.Is<IRefreshableToken>(value => value == token)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertApplyContactSupplementAsyncWasCalledOnContactRepositoryWithContactCollectionFromMicrosoftGraphRepository()
        {
            IEnumerable<IContact> microsoftGraphContactCollection = _fixture.CreateMany<IContact>(_random.Next(5, 15)).ToList();
            QueryHandler sut = CreateSut(microsoftGraphContactCollection);

            IGetContactWithBirthdayCollectionQuery query = CreateGetContactWithBirthdayCollectionQueryMock().Object;
            await sut.QueryAsync(query);

            _contactRepositoryMock.Verify(m => m.ApplyContactSupplementAsync(It.Is<IEnumerable<IContact>>(value => value.Equals(microsoftGraphContactCollection))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertHasBirthdayWithinDaysWasCalledOnEachContactWithinAppliedContactSupplementCollectionFromContactRepository()
        {
            IEnumerable<Mock<IContact>> appliedContactSupplementMockCollection = new List<Mock<IContact>>
            {
                _fixture.BuildContactMock(),
                _fixture.BuildContactMock(),
                _fixture.BuildContactMock(),
                _fixture.BuildContactMock(),
                _fixture.BuildContactMock(),
                _fixture.BuildContactMock(),
                _fixture.BuildContactMock()
            };
            QueryHandler sut = CreateSut(appliedContactSupplementCollection: appliedContactSupplementMockCollection.Select(m => m.Object).ToArray());

            int birthdayWithinDays = _fixture.Create<int>();
            IGetContactWithBirthdayCollectionQuery query = CreateGetContactWithBirthdayCollectionQueryMock(birthdayWithinDays).Object;
            await sut.QueryAsync(query);

            foreach (Mock<IContact> contactMock in appliedContactSupplementMockCollection)
            {
                contactMock.Verify(m => m.HasBirthdayWithinDays(It.Is<int>(value => value == birthdayWithinDays)), Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsContactWithBirthdayCollectionBasedOnAppliedContactSupplementCollection()
        {
            IEnumerable<IContact> appliedContactSupplementCollection = new List<IContact>
            {
                _fixture.BuildContactMock(hasBirthdayWithinDays: true).Object,
                _fixture.BuildContactMock(hasBirthdayWithinDays: false).Object,
                _fixture.BuildContactMock(hasBirthdayWithinDays: true).Object,
                _fixture.BuildContactMock(hasBirthdayWithinDays: false).Object,
                _fixture.BuildContactMock(hasBirthdayWithinDays: true).Object,
                _fixture.BuildContactMock(hasBirthdayWithinDays: false).Object,
                _fixture.BuildContactMock(hasBirthdayWithinDays: true).Object
            };
            QueryHandler sut = CreateSut(appliedContactSupplementCollection: appliedContactSupplementCollection);

            IGetContactWithBirthdayCollectionQuery query = CreateGetContactWithBirthdayCollectionQueryMock().Object;
            IEnumerable<IContact> result = await sut.QueryAsync(query);

            Assert.That(result.All(contact => contact.HasBirthdayWithinDays(It.IsAny<int>())), Is.True);
        }

        private QueryHandler CreateSut(IEnumerable<IContact> microsoftGraphContactCollection = null, IEnumerable<IContact> appliedContactSupplementCollection = null)
        {
            _microsoftGraphRepositoryMock.Setup(m => m.GetContactsAsync(It.IsAny<IRefreshableToken>()))
                .Returns(Task.Run(() => microsoftGraphContactCollection ?? _fixture.CreateMany<IContact>(_random.Next(5, 15)).ToList()));
            _contactRepositoryMock.Setup(m => m.ApplyContactSupplementAsync(It.IsAny<IEnumerable<IContact>>()))
                .Returns(Task.Run(() => appliedContactSupplementCollection ?? _fixture.CreateMany<IContact>(_random.Next(5, 15)).ToList()));

            return new QueryHandler(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object);
        }

        private Mock<IGetContactWithBirthdayCollectionQuery> CreateGetContactWithBirthdayCollectionQueryMock(int? birthdayWithinDays = null, IRefreshableToken refreshableToken = null)
        {
            Mock<IGetContactWithBirthdayCollectionQuery> getContactWithBirthdayCollectionQueryMock = new Mock<IGetContactWithBirthdayCollectionQuery>();
            getContactWithBirthdayCollectionQueryMock.Setup(m => m.BirthdayWithinDays)
                .Returns(birthdayWithinDays ?? _fixture.Create<int>());
            getContactWithBirthdayCollectionQueryMock.Setup(m => m.ToToken())
                .Returns(refreshableToken ?? _fixture.BuildRefreshableTokenMock().Object);
            return getContactWithBirthdayCollectionQueryMock;
        }
    }
}