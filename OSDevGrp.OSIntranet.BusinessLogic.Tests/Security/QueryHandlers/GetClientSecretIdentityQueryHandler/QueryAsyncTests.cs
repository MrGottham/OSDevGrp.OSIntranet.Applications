using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
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

        private Mock<IValidator> _validatorMock;
        private Mock<ISecurityRepository> _securityRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
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
        public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnGetClientSecretIdentityQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetClientSecretIdentityQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.Validate(
                    It.Is<IValidator>(value => value == _validatorMock.Object),
                    It.Is<ISecurityRepository>(value => value == _securityRepositoryMock.Object)), 
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertIdentifierWasCalledOnGetClientSecretIdentityQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetClientSecretIdentityQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.Identifier, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetClientSecretIdentityAsyncWasCalledOnSecurityRepository()
        {
            QueryHandler sut = CreateSut();

            int identifier = _fixture.Create<int>();
            IGetClientSecretIdentityQuery query = CreateQueryMock(identifier).Object;
            await sut.QueryAsync(query);

            _securityRepositoryMock.Verify(m => m.GetClientSecretIdentityAsync(It.Is<int>(value => value == identifier)), Times.Once());
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

            return new QueryHandler(_validatorMock.Object, _securityRepositoryMock.Object);
        }

        private Mock<IGetClientSecretIdentityQuery> CreateQueryMock(int? identifier = null)
        {
            Mock<IGetClientSecretIdentityQuery> queryMock = new Mock<IGetClientSecretIdentityQuery>();
            queryMock.Setup(m => m.Identifier)
                .Returns(identifier ?? _fixture.Create<int>());
            return queryMock;
        }
    }
}
