using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using QueryHandler=OSDevGrp.OSIntranet.BusinessLogic.Security.QueryHandlers.GetUserIdentityQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.QueryHandlers
{
    [TestFixture]
    public class GetUserIdentityQueryHandlerTests
    {
        #region Private variables

        private Mock<ISecurityRepository> _securityRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _securityRepositoryMock = new Mock<ISecurityRepository>();

            _fixture = new Fixture();
            _fixture.Customize<IUserIdentity>(builder => builder.FromFactory(() => _fixture.BuildUserIdentityMock().Object));
        }

        [Test]
        [Category("UnitTest")]
        public void QueryAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            QueryHandler sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.QueryAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("query"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertIdentityIdentifierWasCalledOnGetUserIdentityQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetUserIdentityQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.IdentityIdentifier, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetUserIdentityAsyncWasCalledOnSecurityRepository()
        {
            QueryHandler sut = CreateSut();

            int identityIdentifier = _fixture.Create<int>();
            IGetUserIdentityQuery query = CreateQueryMock(identityIdentifier).Object;
            await sut.QueryAsync(query);

            _securityRepositoryMock.Verify(m => m.GetUserIdentityAsync(It.Is<int>(value => value == identityIdentifier)), Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsUserIdentityFromSecurityRepository()
        {
            IUserIdentity userIdentity = _fixture.Create<IUserIdentity>();
            QueryHandler sut = CreateSut(userIdentity);

            IGetUserIdentityQuery query = CreateQueryMock().Object;
            IUserIdentity result = await sut.QueryAsync(query);

            Assert.That(result, Is.EqualTo(userIdentity));
        }

        private QueryHandler CreateSut(IUserIdentity userIdentity = null)
        {
            _securityRepositoryMock.Setup(m => m.GetUserIdentityAsync(It.IsAny<int>()))
                .Returns(Task.Run(() => userIdentity ?? _fixture.Create<IUserIdentity>()));

            return new QueryHandler(_securityRepositoryMock.Object);
        }

        private Mock<IGetUserIdentityQuery> CreateQueryMock(int? identityIdentifier = null)
        {
            Mock<IGetUserIdentityQuery> queryMock = new Mock<IGetUserIdentityQuery>();
            queryMock.Setup(m => m.IdentityIdentifier)
                .Returns(identityIdentifier ?? _fixture.Create<int>());
            return queryMock;
        }
    }
}
