using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.ClaimResolver
{
    [TestFixture]
    public class GetTokenTests
    {
        #region Private variables

        private Mock<IPrincipalResolver> _principalResolverMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _principalResolverMock = new Mock<IPrincipalResolver>();
            _fixture = new Fixture();
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
            IPrincipal principal = CreateClaimsPrincipal(new[] {new Claim(_fixture.Create<string>(), _fixture.Create<string>())});
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
            IPrincipal principal = CreateClaimsPrincipal(new[] {new Claim(_fixture.Create<string>(), _fixture.Create<string>())});
            IClaimResolver sut = CreateSut(principal);

            IToken result = sut.GetToken<IToken>(value => value);

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GetToken_WhenUnprotectIsNotNullAndPrincipalHasTokenClaim_AssertUnprotectWasCalled()
        {
            IToken token = new Token(_fixture.Create<string>(), _fixture.Create<string>(), DateTime.Now.AddMinutes(5));
            IPrincipal principal = CreateClaimsPrincipal(new[] {new Claim(_fixture.Create<string>(), _fixture.Create<string>()), ClaimHelper.CreateTokenClaim(token, value => value)});
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
            IToken token = new Token(_fixture.Create<string>(), _fixture.Create<string>(), DateTime.Now.AddMinutes(5));
            IPrincipal principal = CreateClaimsPrincipal(new[] {new Claim(_fixture.Create<string>(), _fixture.Create<string>()), ClaimHelper.CreateTokenClaim(token, value => value)});
            IClaimResolver sut = CreateSut(principal);

            string unprotectCalledWithValue = null;
            sut.GetToken<IToken>(value =>
            {
                unprotectCalledWithValue = value;
                return value;
            });

            Assert.That(unprotectCalledWithValue, Is.EqualTo(token.ToBase64()));
        }

        [Test]
        [Category("UnitTest")]
        public void GetToken_WhenUnprotectIsNotNullAndPrincipalHasTokenClaim_ReturnsNotNull()
        {
            IToken token = new Token(_fixture.Create<string>(), _fixture.Create<string>(), DateTime.Now.AddMinutes(5));
            IPrincipal principal = CreateClaimsPrincipal(new[] {new Claim(_fixture.Create<string>(), _fixture.Create<string>()), ClaimHelper.CreateTokenClaim(token, value => value)});
            IClaimResolver sut = CreateSut(principal);

            IToken result = sut.GetToken<IToken>(value => value);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GetToken_WhenUnprotectIsNotNullAndPrincipalHasTokenClaim_ReturnsToken()
        {
            IToken token = new Token(_fixture.Create<string>(), _fixture.Create<string>(), DateTime.Now.AddMinutes(5));
            IPrincipal principal = CreateClaimsPrincipal(new[] {new Claim(_fixture.Create<string>(), _fixture.Create<string>()), ClaimHelper.CreateTokenClaim(token, value => value)});
            IClaimResolver sut = CreateSut(principal);

            IToken result = sut.GetToken<IToken>(value => value);

            Assert.That(result, Is.TypeOf<Token>());
        }

        private IClaimResolver CreateSut(IPrincipal principal = null)
        {
            _principalResolverMock.Setup(m => m.GetCurrentPrincipal())
                .Returns(principal ?? CreateClaimsPrincipal());

            return new BusinessLogic.Security.Logic.ClaimResolver(_principalResolverMock.Object);
        }

        private IPrincipal CreateClaimsPrincipal(IEnumerable<Claim> claimCollection = null)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(claimCollection ?? new Claim[] { }));
        }
    }
}