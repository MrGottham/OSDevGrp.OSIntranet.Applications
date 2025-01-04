using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.QueryHandlers.GetUserInfoAsTokenQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests
    {
        #region Private variables

        private Mock<IPrincipalResolver> _principalResolverMock;
        private Mock<IUserInfoFactory> _userInfoFactoryMock;
        private Mock<ITokenGenerator> _tokenGeneratorMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _principalResolverMock = new Mock<IPrincipalResolver>();
            _userInfoFactoryMock = new Mock<IUserInfoFactory>();
            _tokenGeneratorMock = new Mock<ITokenGenerator>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void QueryAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            IQueryHandler<IGetUserInfoAsTokenQuery, IToken> sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("query"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGetCurrentPrincipalWasCalledOnPrincipalResolver()
        {
            IQueryHandler<IGetUserInfoAsTokenQuery, IToken> sut = CreateSut();

            await sut.QueryAsync(CreateGetUserInfoAsTokenQuery());

            _principalResolverMock.Verify(m => m.GetCurrentPrincipal(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoCurrentPrincipalCouldBeResolved_AssertFromPrincipalWasNotCalledOnUserInfoFactory()
        {
            IQueryHandler<IGetUserInfoAsTokenQuery, IToken> sut = CreateSut(hasCurrentPrincipal: false);

            await sut.QueryAsync(CreateGetUserInfoAsTokenQuery());

            _userInfoFactoryMock.Verify(m => m.FromPrincipal(It.IsAny<ClaimsPrincipal>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoCurrentPrincipalCouldBeResolved_AssertGenerateWasNotCalledOnTokenGenerator()
        {
            IQueryHandler<IGetUserInfoAsTokenQuery, IToken> sut = CreateSut(hasCurrentPrincipal: false);

            await sut.QueryAsync(CreateGetUserInfoAsTokenQuery());

            _tokenGeneratorMock.Verify(m => m.Generate(
                    It.IsAny<ClaimsIdentity>(),
                    It.IsAny<TimeSpan>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoCurrentPrincipalCouldBeResolved_ReturnsNull()
        {
            IQueryHandler<IGetUserInfoAsTokenQuery, IToken> sut = CreateSut(hasCurrentPrincipal: false);

            IToken result = await sut.QueryAsync(CreateGetUserInfoAsTokenQuery());

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCurrentPrincipalCouldBeResolved_AssertFromPrincipalWasCalledOnUserInfoFactory()
        {
            ClaimsPrincipal currentPrincipal = CreateClaimsPrincipal();
            IQueryHandler<IGetUserInfoAsTokenQuery, IToken> sut = CreateSut(hasCurrentPrincipal: true, currentPrincipal: currentPrincipal);

            await sut.QueryAsync(CreateGetUserInfoAsTokenQuery());

            _userInfoFactoryMock.Verify(m => m.FromPrincipal(It.Is<ClaimsPrincipal>(value => value != null && value == currentPrincipal)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoUserInfoCouldBeCreatedForCurrentPrincipal_AssertGenerateWasNotCalledOnTokenGenerator()
        {
            IQueryHandler<IGetUserInfoAsTokenQuery, IToken> sut = CreateSut(hasUserInfo: false);

            await sut.QueryAsync(CreateGetUserInfoAsTokenQuery());

            _tokenGeneratorMock.Verify(m => m.Generate(
                    It.IsAny<ClaimsIdentity>(),
                    It.IsAny<TimeSpan>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenNoUserInfoCouldBeCreatedForCurrentPrincipal_ReturnsNull()
        {
            IQueryHandler<IGetUserInfoAsTokenQuery, IToken> sut = CreateSut(hasUserInfo: false);

            IToken result = await sut.QueryAsync(CreateGetUserInfoAsTokenQuery());

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenUserInfoCouldBeCreatedForCurrentPrincipal_AssertToClaimsWasCalledOnUserInfo()
        {
            Mock<IUserInfo> userInfoMock = _fixture.BuildUserInfoMock();
            IQueryHandler<IGetUserInfoAsTokenQuery, IToken> sut = CreateSut(hasUserInfo: true, userInfo: userInfoMock.Object);

            await sut.QueryAsync(CreateGetUserInfoAsTokenQuery());

            userInfoMock.Verify(m => m.ToClaims(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenUserInfoCouldBeCreatedForCurrentPrincipal_AssertGenerateWasCalledOnTokenGeneratorWithClaimsIdentity()
        {
            IQueryHandler<IGetUserInfoAsTokenQuery, IToken> sut = CreateSut(hasUserInfo: true);

            await sut.QueryAsync(CreateGetUserInfoAsTokenQuery());

            _tokenGeneratorMock.Verify(m => m.Generate(
                    It.IsNotNull<ClaimsIdentity>(),
                    It.IsAny<TimeSpan>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenUserInfoCouldBeCreatedForCurrentPrincipal_AssertGenerateWasCalledOnTokenGeneratorWithClaimsIdentityContainingClaimsFromUserInfo()
        {
            Claim[] claims = _fixture.CreateClaims(_random);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claims).Object;
            IQueryHandler<IGetUserInfoAsTokenQuery, IToken> sut = CreateSut(hasUserInfo: true, userInfo: userInfo);

            await sut.QueryAsync(CreateGetUserInfoAsTokenQuery());

            _tokenGeneratorMock.Verify(m => m.Generate(
                    It.Is<ClaimsIdentity>(value => value != null && value.Claims != null && claims.All(claim => value.HasClaim(claim.Type, claim.Value))),
                    It.IsAny<TimeSpan>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenUserInfoCouldBeCreatedForCurrentPrincipal_AssertGenerateWasCalledOnTokenGeneratorWithExpiresInEqualToFiveMinutes()
        {
            Claim[] claims = _fixture.CreateClaims(_random);
            IUserInfo userInfo = _fixture.BuildUserInfoMock(toClaims: claims).Object;
            IQueryHandler<IGetUserInfoAsTokenQuery, IToken> sut = CreateSut(hasUserInfo: true, userInfo: userInfo);

            await sut.QueryAsync(CreateGetUserInfoAsTokenQuery());

            _tokenGeneratorMock.Verify(m => m.Generate(
                    It.IsAny<ClaimsIdentity>(),
                    It.Is<TimeSpan>(value => (int)value.TotalSeconds == 300)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenTokenCouldNotBeGenerated_ReturnsNull()
        {
            IQueryHandler<IGetUserInfoAsTokenQuery, IToken> sut = CreateSut(canGenerateToken: false);

            IToken result = await sut.QueryAsync(CreateGetUserInfoAsTokenQuery());

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenTokenCouldNotBeGenerated_ReturnsNotNull()
        {
            IQueryHandler<IGetUserInfoAsTokenQuery, IToken> sut = CreateSut(canGenerateToken: true);

            IToken result = await sut.QueryAsync(CreateGetUserInfoAsTokenQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenTokenCouldNotBeGenerated_ReturnsGeneratedTokenFromTokenGenerator()
        {
            IToken token = _fixture.BuildTokenMock().Object;
            IQueryHandler<IGetUserInfoAsTokenQuery, IToken> sut = CreateSut(canGenerateToken: true, token: token);

            IToken result = await sut.QueryAsync(CreateGetUserInfoAsTokenQuery());

            Assert.That(result, Is.EqualTo(token));
        }

        private IQueryHandler<IGetUserInfoAsTokenQuery, IToken> CreateSut(bool hasCurrentPrincipal = true, ClaimsPrincipal currentPrincipal = null, bool hasUserInfo = true, IUserInfo userInfo = null, bool canGenerateToken = true, IToken token = null)
        {
            _principalResolverMock.Setup(m => m.GetCurrentPrincipal())
                .Returns(hasCurrentPrincipal ? currentPrincipal ?? CreateClaimsPrincipal() : null);

            _userInfoFactoryMock.Setup(m => m.FromPrincipal(It.IsAny<ClaimsPrincipal>()))
                .Returns(hasUserInfo ? userInfo ?? _fixture.BuildUserInfoMock().Object : null);

            _tokenGeneratorMock.Setup(m => m.Generate(It.IsAny<ClaimsIdentity>(), It.IsAny<TimeSpan>()))
                .Returns(canGenerateToken ? token ?? _fixture.BuildTokenMock().Object : null);

            return new BusinessLogic.Security.QueryHandlers.GetUserInfoAsTokenQueryHandler(_principalResolverMock.Object, _userInfoFactoryMock.Object, _tokenGeneratorMock.Object);
        }

        private IGetUserInfoAsTokenQuery CreateGetUserInfoAsTokenQuery()
        {
            return CreateGetUserInfoAsTokenQueryMock().Object;
        }

        private Mock<IGetUserInfoAsTokenQuery> CreateGetUserInfoAsTokenQueryMock()
        {
            return new Mock<IGetUserInfoAsTokenQuery>();
        }

        private ClaimsPrincipal CreateClaimsPrincipal()
        {
            return new ClaimsPrincipal(new ClaimsIdentity([_fixture.CreateClaim(type: ClaimTypes.NameIdentifier, hasValue: true, value: _fixture.Create<string>())]));
        }
    }
}