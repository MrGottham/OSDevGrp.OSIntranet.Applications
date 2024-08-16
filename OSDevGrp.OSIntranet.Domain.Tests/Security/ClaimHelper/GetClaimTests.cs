using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.ClaimHelper
{
    [TestFixture]
    public class GetClaimTests
    {
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void GetClaim_WhenPrincipalIsNull_ThrowsArgumentNullException()
        {
            const IPrincipal principal = null;

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Security.ClaimHelper.GetClaim(principal, _fixture.Create<string>()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("principal"));
        }

        [Test]
        [Category("UnitTest")]
        public void GetClaim_WhenCalledWithPrincipalAndTypeIsNull_ThrowsArgumentNullException()
        {
            IPrincipal principal = CreateGenericPrincipal();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Security.ClaimHelper.GetClaim(principal, null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("type"));
        }

        [Test]
        [Category("UnitTest")]
        public void GetClaim_WhenCalledWithPrincipalAndTypeIsEmpty_ThrowsArgumentNullException()
        {
            IPrincipal principal = CreateGenericPrincipal();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Security.ClaimHelper.GetClaim(principal, string.Empty));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("type"));
        }

        [Test]
        [Category("UnitTest")]
        public void GetClaim_WhenCalledWithPrincipalAndTypeIsWhiteSpace_ThrowsArgumentNullException()
        {
            IPrincipal principal = CreateGenericPrincipal();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Security.ClaimHelper.GetClaim(principal, " "));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("type"));
        }

        [Test]
        [Category("UnitTest")]
        public void GetClaim_WhenCalledWithNonClaimsPrincipalAndNonExistingClaim_ReturnsNull()
        {
            IPrincipal principal = CreateGenericPrincipal();

            Claim result = Domain.Security.ClaimHelper.GetClaim(principal, _fixture.Create<string>());

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GetClaim_WhenClaimsPrincipalIsNull_ThrowsArgumentNullException()
        {
            const ClaimsPrincipal claimsPrincipal = null;

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Security.ClaimHelper.GetClaim(claimsPrincipal, _fixture.Create<string>()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("claimsPrincipal"));
        }

        [Test]
        [Category("UnitTest")]
        public void GetClaim_WhenCalledWithClaimsPrincipalAndTypeIsNull_ThrowsArgumentNullException()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Security.ClaimHelper.GetClaim(claimsPrincipal, null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("type"));
        }

        [Test]
        [Category("UnitTest")]
        public void GetClaim_WhenCalledWithClaimsPrincipalAndTypeIsEmpty_ThrowsArgumentNullException()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Security.ClaimHelper.GetClaim(claimsPrincipal, string.Empty));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("type"));
        }

        [Test]
        [Category("UnitTest")]
        public void GetClaim_WhenCalledWithClaimsPrincipalAndTypeIsWhiteSpace_ThrowsArgumentNullException()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Security.ClaimHelper.GetClaim(claimsPrincipal, " "));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("type"));
        }

        [Test]
        [Category("UnitTest")]
        public void GetClaim_WhenCalledWithClaimsPrincipalAndNonExistingClaimType_ReturnsNull()
        {
            string type = _fixture.Create<string>();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(_fixture.CreateClaims(_random));

            Claim result = Domain.Security.ClaimHelper.GetClaim(claimsPrincipal, type);

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GetClaim_WhenCalledWithClaimsPrincipalAndExistingClaimType_ReturnsClaim()
        {
            string type = _fixture.Create<string>();
            Claim claim = _fixture.CreateClaim(type: type);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(_fixture.CreateClaims(_random).Concat(claim));

            Claim result = Domain.Security.ClaimHelper.GetClaim(claimsPrincipal, type);

            Assert.That(result.Type, Is.EqualTo(claim.Type));
            Assert.That(result.Value, Is.EqualTo(claim.Value));
        }

        private IPrincipal CreateGenericPrincipal()
        {
            return new GenericPrincipal(new GenericIdentity(_fixture.Create<string>()), new string[] { });
        }

        private ClaimsPrincipal CreateClaimsPrincipal(IEnumerable<Claim> claimCollection = null)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(claimCollection ?? new Claim[] { }));
        }
    }
}