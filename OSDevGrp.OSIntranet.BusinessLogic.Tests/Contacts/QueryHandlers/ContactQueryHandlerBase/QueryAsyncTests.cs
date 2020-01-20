using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using QueryHandler=OSDevGrp.OSIntranet.BusinessLogic.Contacts.QueryHandlers.ContactQueryHandlerBase<OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries.IContactQuery, object>;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.QueryHandlers.ContactQueryHandlerBase
{
    [TestFixture]
    public class QueryAsyncTests
    {
        #region Private variables

        private ValidatorMockContext _validatorMockContext;
        private Mock<IMicrosoftGraphRepository> _microsoftGraphRepositoryMock;
        private Mock<IContactRepository> _contactRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMockContext = new ValidatorMockContext();
            _microsoftGraphRepositoryMock = new Mock<IMicrosoftGraphRepository>();
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
        public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IContactQuery> queryMock = CreateContactQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.Validate(It.Is<IValidator>(value => value == _validatorMockContext.ValidatorMock.Object)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertToTokenWasCalledOnQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IContactQuery> queryMock = CreateContactQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.ToToken(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetDataAsyncWasCalledOnQueryHandler()
        {
            QueryHandler sut = CreateSut();

            IContactQuery query = CreateContactQueryMock().Object;
            await sut.QueryAsync(query);

            Assert.That(((Sut) sut).GetDataAsyncWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetDataAsyncWasCalledOnQueryHandlerWithSameQuery()
        {
            QueryHandler sut = CreateSut();

            IContactQuery query = CreateContactQueryMock().Object;
            await sut.QueryAsync(query);

            Assert.That(((Sut) sut).Query, Is.EqualTo(query));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetDataAsyncWasCalledOnQueryHandlerWithTokenFromQuery()
        {
            QueryHandler sut = CreateSut();

            IRefreshableToken refreshableToken = _fixture.BuildRefreshableTokenMock().Object;
            IContactQuery query = CreateContactQueryMock(refreshableToken).Object;
            await sut.QueryAsync(query);

            Assert.That(((Sut) sut).Token, Is.EqualTo(refreshableToken));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsDataFromGetDataAsync()
        {
            object data = new object();
            QueryHandler sut = CreateSut(data);

            IContactQuery query = CreateContactQueryMock().Object;
            object result = await sut.QueryAsync(query);

            Assert.That(result, Is.EqualTo(data));
        }

        private QueryHandler CreateSut(object result = null)
        {
            return new Sut(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object, result);
        }

        private Mock<IContactQuery> CreateContactQueryMock(IRefreshableToken refreshableToken = null)
        {
            Mock<IContactQuery> contactQueryMock = new Mock<IContactQuery>();
            contactQueryMock.Setup(m => m.ToToken())
                .Returns(refreshableToken ?? _fixture.BuildRefreshableTokenMock().Object);
            return contactQueryMock;
        }

        private class Sut : QueryHandler
        {
            #region Private variables

            private readonly object _result;

            #endregion

            #region Constructor

            public Sut(IValidator validator, IMicrosoftGraphRepository microsoftGraphRepository, IContactRepository contactRepository, object result = null) 
                : base(validator, microsoftGraphRepository, contactRepository)
            {
                _result = result ?? new object();
            }

            #endregion

            #region Properties

            public bool GetDataAsyncWasCalled { get; private set; }

            public IContactQuery Query { get; private set; }

            public IRefreshableToken Token { get; private set; }

            #endregion

            #region Methods

            protected override Task<object> GetDataAsync(IContactQuery query, IRefreshableToken token)
            {
                Assert.That(query, Is.Not.Null);
                Assert.That(token, Is.Not.Null);

                GetDataAsyncWasCalled = true;
                Query = query;
                Token = token;

                return Task.Run(() => _result);
            }

            #endregion
        }
    }
}