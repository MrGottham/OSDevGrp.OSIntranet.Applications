using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using OSDevGrp.OSIntranet.Core.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;
using QueryHandler = OSDevGrp.OSIntranet.BusinessLogic.Security.QueryHandlers.GetAuthorizeUriForMicrosoftGraphQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.QueryHandlers.GetAuthorizeUriForMicrosoftGraphQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests
    {
        #region Private variables

        private Mock<IMicrosoftGraphRepository> _microsoftGraphRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _microsoftGraphRepositoryMock = new Mock<IMicrosoftGraphRepository>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void QueryAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            QueryHandler sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("query"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertRedirectUriWasCalledOnQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetAuthorizeUriForMicrosoftGraphQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.RedirectUri, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertStateIdentifierWasCalledOnQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetAuthorizeUriForMicrosoftGraphQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.StateIdentifier, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetAuthorizeUriAsyncWasCalledOnMicrosoftGraphRepository()
        {
            QueryHandler sut = CreateSut();

            Uri redirectUri = _fixture.CreateEndpoint();
            Guid stateIdentifier = Guid.NewGuid();
            IGetAuthorizeUriForMicrosoftGraphQuery query= CreateQueryMock(redirectUri, stateIdentifier).Object;
            await sut.QueryAsync(query);

            _microsoftGraphRepositoryMock.Verify(m => m.GetAuthorizeUriAsync(It.Is<Uri>(value => value == redirectUri), It.Is<Guid>(value => value == stateIdentifier)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnAuthorizeUriFromMicrosoftGraphRepository()
        {
            Uri authorizeUri = _fixture.CreateEndpoint();
            QueryHandler sut = CreateSut(authorizeUri);

            IGetAuthorizeUriForMicrosoftGraphQuery query = CreateQueryMock().Object;
            Uri result = await sut.QueryAsync(query);

            Assert.That(result, Is.EqualTo(authorizeUri));
        }

        private QueryHandler CreateSut(Uri authorizeUri = null)
        {
            _microsoftGraphRepositoryMock.Setup(m => m.GetAuthorizeUriAsync(It.IsAny<Uri>(), It.IsAny<Guid>()))
                .Returns(Task.Run(() => authorizeUri ?? _fixture.CreateEndpoint()));

            return new QueryHandler(_microsoftGraphRepositoryMock.Object);
        }

        private Mock<IGetAuthorizeUriForMicrosoftGraphQuery> CreateQueryMock(Uri redirectUri = null, Guid? stateIdentifier = null)
        {
            Mock<IGetAuthorizeUriForMicrosoftGraphQuery> queryMock = new Mock<IGetAuthorizeUriForMicrosoftGraphQuery>();
            queryMock.Setup(m => m.RedirectUri)
                .Returns(redirectUri ?? _fixture.CreateEndpoint());
            queryMock.Setup(m => m.StateIdentifier)
                .Returns(stateIdentifier ?? Guid.NewGuid());
            return queryMock;
        }
    }
}