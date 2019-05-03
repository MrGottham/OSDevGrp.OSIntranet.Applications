using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using QueryHandler=OSDevGrp.OSIntranet.BusinessLogic.Security.QueryHandlers.GetClientSecretIdentityQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.QueryHandlers.GetClientSecretIdentityQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests
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
            _fixture.Customize<IClientSecretIdentity>(builder => builder.FromFactory(() => _fixture.BuildClientSecretIdentityMock().Object));
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
        public async Task QueryAsync_WhenCalled_AssertIdentityIdentifierWasCalledOnGetClientSecretIdentityQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetClientSecretIdentityQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.IdentityIdentifier, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetClientSecretIdentityAsyncWasCalledOnSecurityRepository()
        {
            QueryHandler sut = CreateSut();

            int identityIdentifier = _fixture.Create<int>();
            IGetClientSecretIdentityQuery query = CreateQueryMock(identityIdentifier).Object;
            await sut.QueryAsync(query);

            _securityRepositoryMock.Verify(m => m.GetClientSecretIdentityAsync(It.Is<int>(value => value == identityIdentifier)), Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsClientSecretIdentityFromSecurityRepository()
        {
            IClientSecretIdentity clientSecretIdentity = _fixture.Create<IClientSecretIdentity>();
            QueryHandler sut = CreateSut(clientSecretIdentity);

            IGetClientSecretIdentityQuery query = CreateQueryMock().Object;
            IClientSecretIdentity result = await sut.QueryAsync(query);

            Assert.That(result, Is.EqualTo(clientSecretIdentity));
        }

        private QueryHandler CreateSut(IClientSecretIdentity clientSecretIdentity = null)
        {
            _securityRepositoryMock.Setup(m => m.GetClientSecretIdentityAsync(It.IsAny<int>()))
                .Returns(Task.Run(() => clientSecretIdentity ?? _fixture.Create<IClientSecretIdentity>()));

            return new QueryHandler(_securityRepositoryMock.Object);
        }

        private Mock<IGetClientSecretIdentityQuery> CreateQueryMock(int? identityIdentifier = null)
        {
            Mock<IGetClientSecretIdentityQuery> queryMock = new Mock<IGetClientSecretIdentityQuery>();
            queryMock.Setup(m => m.IdentityIdentifier)
                .Returns(identityIdentifier ?? _fixture.Create<int>());
            return queryMock;
        }
    }
}
