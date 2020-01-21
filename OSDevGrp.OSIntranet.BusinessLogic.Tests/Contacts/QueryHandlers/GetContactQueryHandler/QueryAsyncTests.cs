using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using QueryHandler=OSDevGrp.OSIntranet.BusinessLogic.Contacts.QueryHandlers.GetContactQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.QueryHandlers.GetContactQueryHandler
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
        public async Task QueryAsync_WhenCalled_AssertGetContactAsyncWasCalledOnMicrosoftGraphRepository()
        {
            QueryHandler sut = CreateSut();

            string externalIdentifier = _fixture.Create<string>();
            IRefreshableToken token = _fixture.BuildRefreshableTokenMock().Object;
            IGetContactQuery query = CreateGetContactQueryMock(externalIdentifier, token).Object;
            await sut.QueryAsync(query);

            _microsoftGraphRepositoryMock.Verify(m => m.GetContactAsync(
                    It.Is<IRefreshableToken>(value => value == token),
                    It.Is<string>(value => string.CompareOrdinal(value, externalIdentifier) == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertApplyContactSupplementAsyncWasCalledOnContactRepositoryWithContactFromMicrosoftGraphRepository()
        {
            IContact microsoftGraphContact = _fixture.BuildContactMock().Object;
            QueryHandler sut = CreateSut(microsoftGraphContact);

            IGetContactQuery query = CreateGetContactQueryMock().Object;
            await sut.QueryAsync(query);

            _contactRepositoryMock.Verify(m => m.ApplyContactSupplementAsync(It.Is<IContact>(value => value == microsoftGraphContact)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsAppliedContactSupplementFromContactRepository()
        {
            IContact appliedContactSupplement = _fixture.BuildContactMock().Object;
            QueryHandler sut = CreateSut(appliedContactSupplement: appliedContactSupplement);

            IGetContactQuery query = CreateGetContactQueryMock().Object;
            IContact result = await sut.QueryAsync(query);

            Assert.That(result, Is.EqualTo(appliedContactSupplement));
        }

        private QueryHandler CreateSut(IContact microsoftGraphContact = null, IContact appliedContactSupplement = null)
        {
            _microsoftGraphRepositoryMock.Setup(m => m.GetContactAsync(It.IsAny<IRefreshableToken>(), It.IsAny<string>()))
                .Returns(Task.Run(() => microsoftGraphContact ?? _fixture.BuildContactMock().Object));
            _contactRepositoryMock.Setup(m => m.ApplyContactSupplementAsync(It.IsAny<IContact>()))
                .Returns(Task.Run(() => appliedContactSupplement ?? _fixture.BuildContactMock().Object));

            return new QueryHandler(_validatorMockContext.ValidatorMock.Object, _microsoftGraphRepositoryMock.Object, _contactRepositoryMock.Object);
        }

        private Mock<IGetContactQuery> CreateGetContactQueryMock(string externalIdentifier = null, IRefreshableToken refreshableToken = null)
        {
            Mock<IGetContactQuery> getContactQueryMock = new Mock<IGetContactQuery>();
            getContactQueryMock.Setup(m => m.ExternalIdentifier)
                .Returns(externalIdentifier ?? _fixture.Create<string>());
            getContactQueryMock.Setup(m => m.ToToken())
                .Returns(refreshableToken ?? _fixture.BuildRefreshableTokenMock().Object);
            return getContactQueryMock;
        }
    }
}