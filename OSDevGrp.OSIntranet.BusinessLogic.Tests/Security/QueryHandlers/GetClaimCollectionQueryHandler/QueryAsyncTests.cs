using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using QueryHandler = OSDevGrp.OSIntranet.BusinessLogic.Security.QueryHandlers.GetClaimCollectionQueryHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.QueryHandlers.GetClaimCollectionQueryHandler
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
            _random = new Random(_fixture.Create<int>());
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
        public async Task QueryAsync_WhenCalled_AssertGetClaimsAsyncWasCalledOnSecurityRepository()
        {
            QueryHandler sut = CreateSut();

            await sut.QueryAsync(new EmptyQuery());

            _securityRepositoryMock.Verify(m => m.GetClaimsAsync(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsClaimsFromSecurityRepository()
        {
            IEnumerable<Claim> claimCollection = _fixture.CreateClaims(_random);
            QueryHandler sut = CreateSut(claimCollection);

            IEnumerable<Claim> result = await sut.QueryAsync(new EmptyQuery());

            Assert.That(result, Is.EqualTo(claimCollection));
        }

        private QueryHandler CreateSut(IEnumerable<Claim> claimCollection = null)
        {
            _securityRepositoryMock.Setup(m => m.GetClaimsAsync())
                .Returns(Task.Run(() => claimCollection ?? _fixture.CreateClaims(_random)));

            return new QueryHandler(_securityRepositoryMock.Object);
        }
    }
}