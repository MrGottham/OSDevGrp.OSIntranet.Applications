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
using QueryHandler=OSDevGrp.OSIntranet.BusinessLogic.Contacts.QueryHandlers.GetContactCollectionQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.QueryHandlers.GetContactCollectionQueryHandler
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
        public async Task QueryAsync_WhenCalled_AssertGetContactsAsyncWasCalledOnMicrosoftGraphRepository()
        {
            QueryHandler sut = CreateSut();

            IRefreshableToken token = _fixture.BuildRefreshableTokenMock().Object;
            IGetContactCollectionQuery query = CreateGetContactCollectionQueryMock(token).Object;
            await sut.QueryAsync(query);

            _microsoftGraphRepositoryMock.Verify(m => m.GetContactsAsync(It.Is<IRefreshableToken>(value => value == token)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertApplyContactSupplementAsyncWasCalledOnContactRepositoryWithContactCollectionFromMicrosoftGraphRepository()
        {
            IEnumerable<IContact> microsoftGraphContactCollection = _fixture.CreateMany<IContact>(_random.Next(5, 15)).ToList();
            QueryHandler sut = CreateSut(microsoftGraphContactCollection);

            IGetContactCollectionQuery query = CreateGetContactCollectionQueryMock().Object;
            await sut.QueryAsync(query);

            _contactRepositoryMock.Verify(m => m.ApplyContactSupplementAsync(It.Is<IEnumerable<IContact>>(value => value.Equals(microsoftGraphContactCollection))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsAppliedContactSupplementCollectionFromContactRepository()
        {
            IEnumerable<IContact> appliedContactSupplementCollection = _fixture.CreateMany<IContact>(_random.Next(5, 15)).ToList();
            QueryHandler sut = CreateSut(appliedContactSupplementCollection: appliedContactSupplementCollection);

            IGetContactCollectionQuery query = CreateGetContactCollectionQueryMock().Object;
            IEnumerable<IContact> result = await sut.QueryAsync(query);

            Assert.That(result, Is.EqualTo(appliedContactSupplementCollection));
        }

        private QueryHandler CreateSut(IEnumerable<IContact> microsoftGraphContactCollection = null, IEnumerable<IContact> appliedContactSupplementCollection = null)
        {
            _microsoftGraphRepositoryMock.Setup(m => m.GetContactsAsync(It.IsAny<IRefreshableToken>()))
                .Returns(Task.Run(() => microsoftGraphContactCollection ?? _fixture.CreateMany<IContact>(_random.Next(5, 15)).ToList()));
            _contactRepositoryMock.Setup(m => m.ApplyContactSupplementAsync(It.IsAny<IEnumerable<IContact>>()))
                .Returns(Task.Run(() => appliedContactSupplementCollection ?? _fixture.CreateMany<IContact>(_random.Next(5, 15)).ToList()));

            return new QueryHandler(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object);
        }

        private Mock<IGetContactCollectionQuery> CreateGetContactCollectionQueryMock(IRefreshableToken refreshableToken = null)
        {
            Mock<IGetContactCollectionQuery> getContactCollectionQueryMock = new Mock<IGetContactCollectionQuery>();
            getContactCollectionQueryMock.Setup(m => m.ToToken())
                .Returns(refreshableToken ?? _fixture.BuildRefreshableTokenMock().Object);
            return getContactCollectionQueryMock;
        }
    }
}