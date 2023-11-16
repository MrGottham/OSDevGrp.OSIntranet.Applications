using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.ClaimResolver
{
	[TestFixture]
    public class GetTokenTests
    {
        #region Private variables

        private Mock<IPrincipalResolver> _principalResolverMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _principalResolverMock = new Mock<IPrincipalResolver>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void GetToken_WhenUnprotectIsNull_ThrowsArgumentNullException()
        {
            IClaimResolver sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.GetToken<IToken>(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("unprotect"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void GetToken_WhenUnprotectIsNotNull_AssertGetCurrentPrincipalWasCalledOnPrincipalResolver()
        {
            IClaimResolver sut = CreateSut();

            sut.GetToken<IToken>(value => value);

            _principalResolverMock.Verify(m => m.GetCurrentPrincipal(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void GetToken_WhenUnprotectIsNotNullAndPrincipalDoesNotHaveTokenClaim_AssertUnprotectWasNotCalled()
        {
	        IPrincipal principal = CreateClaimsPrincipal(false);
            IClaimResolver sut = CreateSut(principal);

            bool unprotectCalled = false;
            sut.GetToken<IToken>(value =>
            {
                unprotectCalled = true;
                return value;
            });

            Assert.That(unprotectCalled, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void GetToken_WhenUnprotectIsNotNullAndPrincipalDoesNotHaveTokenClaim_ReturnsNull()
        {
            IPrincipal principal = CreateClaimsPrincipal(false);
            IClaimResolver sut = CreateSut(principal);

            IToken result = sut.GetToken<IToken>(value => value);

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GetToken_WhenUnprotectIsNotNullAndPrincipalHasTokenClaim_AssertUnprotectWasCalled()
        {
            IPrincipal principal = CreateClaimsPrincipal();
            IClaimResolver sut = CreateSut(principal);

            bool unprotectCalled = false;
            sut.GetToken<IToken>(value =>
            {
                unprotectCalled = true;
                return value;
            });

            Assert.That(unprotectCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void GetToken_WhenUnprotectIsNotNullAndPrincipalHasTokenClaim_AssertUnprotectWasCalledWithBase64ForToken()
        {
	        IToken token = CreateToken();
            IPrincipal principal = CreateClaimsPrincipal(token: token);
            IClaimResolver sut = CreateSut(principal);

            string unprotectCalledWithValue = null;
            sut.GetToken<IToken>(value =>
            {
                unprotectCalledWithValue = value;
                return value;
            });

            Assert.That(unprotectCalledWithValue, Is.EqualTo(token.ToBase64String()));
        }

        [Test]
        [Category("UnitTest")]
        public void GetToken_WhenUnprotectIsNotNullAndPrincipalHasTokenClaim_ReturnsNotNull()
        {
            IPrincipal principal = CreateClaimsPrincipal();
            IClaimResolver sut = CreateSut(principal);

            IToken result = sut.GetToken<IToken>(value => value);

            Assert.That(result, Is.Not.Null);
        }

        private IClaimResolver CreateSut(IPrincipal principal = null)
        {
            _principalResolverMock.Setup(m => m.GetCurrentPrincipal())
                .Returns(principal ?? CreateClaimsPrincipal());

            return new BusinessLogic.Security.Logic.ClaimResolver(_principalResolverMock.Object);
        }

        private IPrincipal CreateClaimsPrincipal(bool withTokenClaim = true, IToken token = null)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(CreateClaimCollection(withTokenClaim, token)));
        }

        private IEnumerable<Claim> CreateClaimCollection(bool withTokenClaim = true, IToken token = null)
        {
	        if (withTokenClaim)
	        {
		        return new[]
		        {
			        new Claim(_fixture.Create<string>(), _fixture.Create<string>()),
			        new Claim(_fixture.Create<string>(), _fixture.Create<string>()),
			        ClaimHelper.CreateTokenClaim(ClaimHelper.TokenClaimType, token ?? CreateToken(), value => value),
			        new Claim(_fixture.Create<string>(), _fixture.Create<string>()),
			        new Claim(_fixture.Create<string>(), _fixture.Create<string>())
		        };
	        }

	        return new[]
	        {
		        new Claim(_fixture.Create<string>(), _fixture.Create<string>()),
		        new Claim(_fixture.Create<string>(), _fixture.Create<string>()),
		        new Claim(_fixture.Create<string>(), _fixture.Create<string>())
	        };
        }

        private IToken CreateToken()
        {
	        return TokenFactory.Create()
		        .WithTokenType(_fixture.Create<string>())
		        .WithAccessToken(_fixture.Create<string>())
		        .WithExpires(DateTime.UtcNow.AddSeconds(_random.Next(60, 3600)))
		        .Build();
        }
    }
}