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
using QueryHandler=OSDevGrp.OSIntranet.BusinessLogic.Security.QueryHandlers.GetUserIdentityQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.QueryHandlers.GetUserIdentityQueryHandler
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
        public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnGetUserIdentityQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetUserIdentityQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.Validate(
                    It.Is<IValidator>(value => value == _validatorMock.Object),
                    It.Is<ISecurityRepository>(value => value == _securityRepositoryMock.Object)), 
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertIdentifierWasCalledOnGetUserIdentityQuery()
        {
            QueryHandler sut = CreateSut();

            Mock<IGetUserIdentityQuery> queryMock = CreateQueryMock();
            await sut.QueryAsync(queryMock.Object);

            queryMock.Verify(m => m.Identifier, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetUserIdentityAsyncWasCalledOnSecurityRepository()
        {
            QueryHandler sut = CreateSut();

            int identifier = _fixture.Create<int>();
            IGetUserIdentityQuery query = CreateQueryMock(identifier).Object;
            await sut.QueryAsync(query);

            _securityRepositoryMock.Verify(m => m.GetUserIdentityAsync(It.Is<int>(value => value == identifier)), Times.Once());
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

            return new QueryHandler(_validatorMock.Object, _securityRepositoryMock.Object);
        }

        private Mock<IGetUserIdentityQuery> CreateQueryMock(int? identifier = null)
        {
            Mock<IGetUserIdentityQuery> queryMock = new Mock<IGetUserIdentityQuery>();
            queryMock.Setup(m => m.Identifier)
                .Returns(identifier ?? _fixture.Create<int>());
            return queryMock;
        }
    }
}
