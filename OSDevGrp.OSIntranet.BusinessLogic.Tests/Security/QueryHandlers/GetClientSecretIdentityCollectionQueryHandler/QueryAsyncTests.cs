using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using QueryHandler=OSDevGrp.OSIntranet.BusinessLogic.Security.QueryHandlers.GetClientSecretIdentityCollectionQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.QueryHandlers.GetClientSecretIdentityCollectionQueryHandler
{
    public class QueryAsyncTests
    {
        #region Private variables

        private Mock<ISecurityRepository> _securityRepositoryMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _securityRepositoryMock = new Mock<ISecurityRepository>();

            _fixture = new Fixture();
            _fixture.Customize<IClientSecretIdentity>(builder => builder.FromFactory(() => _fixture.BuildClientSecretIdentityMock().Object));

            _random = new Random(_fixture.Create<int>());
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
        public async Task QueryAsync_WhenCalled_AssertGetClientSecretIdentitiesAsyncWasCalledOnSecurityRepository()
        {
            QueryHandler sut = CreateSut();

            await sut.QueryAsync(new EmptyQuery());

            _securityRepositoryMock.Verify(m => m.GetClientSecretIdentitiesAsync(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsClientSecretIdentitiesFromSecurityRepository()
        {
            IEnumerable<IClientSecretIdentity> clientSecretIdentityCollection = _fixture.CreateMany<IClientSecretIdentity>(_random.Next(5, 10)).ToList();
            QueryHandler sut = CreateSut(clientSecretIdentityCollection);

            IEnumerable<IClientSecretIdentity> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.EqualTo(clientSecretIdentityCollection));
        }

        private QueryHandler CreateSut(IEnumerable<IClientSecretIdentity> clientSecretIdentityCollection = null)
        {
            _securityRepositoryMock.Setup(m => m.GetClientSecretIdentitiesAsync())
                .Returns(Task.Run(() => clientSecretIdentityCollection ?? _fixture.CreateMany<IClientSecretIdentity>(_random.Next(5, 10)).ToList()));

            return new QueryHandler(_securityRepositoryMock.Object);
        }
    }
}
