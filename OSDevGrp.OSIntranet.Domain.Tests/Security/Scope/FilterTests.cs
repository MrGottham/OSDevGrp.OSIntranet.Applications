using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.Scope
{
    [TestFixture]
    public class FilterTests
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
        public void Filter_WhenClaimsIsNull_ThrowsArgumentNullException()
        {
            IScope sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Filter(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("claims"));
        }

        [Test]
        [Category("UnitTest")]
        public void Filter_WhenClaimsIsEmpty_ReturnsNotNull()
        {
            IScope sut = CreateSut();

            IEnumerable<Claim> result = sut.Filter(Array.Empty<Claim>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Filter_WhenClaimsIsEmpty_ReturnsEmptyCollectionOfClaims()
        {
            IScope sut = CreateSut();

            IEnumerable<Claim> result = sut.Filter(Array.Empty<Claim>());

            Assert.That(result, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Filter_WhenClaimsDoesNotMatchAnyRelatedClaims_ReturnsNotNull()
        {
            IScope sut = CreateSut(ClaimTypes.NameIdentifier, ClaimTypes.Name, ClaimTypes.Email);

            IEnumerable<Claim> claims = CreateClaims(ClaimTypes.GivenName, ClaimTypes.Surname, ClaimTypes.MobilePhone);
            IEnumerable<Claim> result = sut.Filter(claims);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Filter_WhenClaimsDoesNotMatchAnyRelatedClaims_ReturnsEmptyCollectionOfClaims()
        {
            IScope sut = CreateSut(ClaimTypes.NameIdentifier, ClaimTypes.Name, ClaimTypes.Email);

            IEnumerable<Claim> claims = CreateClaims(ClaimTypes.GivenName, ClaimTypes.Surname, ClaimTypes.MobilePhone);
            IEnumerable<Claim> result = sut.Filter(claims);

            Assert.That(result, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Filter_WhenClaimsMatchesSomeRelatedClaims_ReturnsNotNull()
        {
            IScope sut = CreateSut(ClaimTypes.NameIdentifier, ClaimTypes.Name, ClaimTypes.Email);

            IEnumerable<Claim> claims = CreateClaims(ClaimTypes.NameIdentifier, ClaimTypes.Name, ClaimTypes.GivenName, ClaimTypes.Surname, ClaimTypes.MobilePhone);
            IEnumerable<Claim> result = sut.Filter(claims);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Filter_WhenClaimsMatchesSomeRelatedClaims_ReturnsNonEmptyCollectionOfClaims()
        {
            IScope sut = CreateSut(ClaimTypes.NameIdentifier, ClaimTypes.Name, ClaimTypes.Email);

            IEnumerable<Claim> claims = CreateClaims(ClaimTypes.NameIdentifier, ClaimTypes.Name, ClaimTypes.GivenName, ClaimTypes.Surname, ClaimTypes.MobilePhone);
            IEnumerable<Claim> result = sut.Filter(claims);

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Filter_WhenClaimsMatchesSomeRelatedClaims_ReturnsNonEmptyCollectionOfMatchingClaimsOnly()
        {
            IScope sut = CreateSut(ClaimTypes.NameIdentifier, ClaimTypes.Name, ClaimTypes.Email);

            IEnumerable<Claim> claims = CreateClaims(ClaimTypes.NameIdentifier, ClaimTypes.Name, ClaimTypes.GivenName, ClaimTypes.Surname, ClaimTypes.MobilePhone);
            Claim[] result = sut.Filter(claims).ToArray();

            Assert.That(result.Any(claim => claim.Type == ClaimTypes.NameIdentifier), Is.True);
            Assert.That(result.Any(claim => claim.Type == ClaimTypes.Name), Is.True);
            Assert.That(result.Any(claim => claim.Type == ClaimTypes.Email), Is.False);
            Assert.That(result.Any(claim => claim.Type == ClaimTypes.GivenName), Is.False);
            Assert.That(result.Any(claim => claim.Type == ClaimTypes.Surname), Is.False);
            Assert.That(result.Any(claim => claim.Type == ClaimTypes.MobilePhone), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void Filter_WhenClaimsMatchesAllRelatedClaims_ReturnsNotNull()
        {
            IScope sut = CreateSut(ClaimTypes.NameIdentifier, ClaimTypes.Name, ClaimTypes.Email);

            IEnumerable<Claim> claims = CreateClaims(ClaimTypes.NameIdentifier, ClaimTypes.Name, ClaimTypes.GivenName, ClaimTypes.Surname, ClaimTypes.MobilePhone, ClaimTypes.Email);
            IEnumerable<Claim> result = sut.Filter(claims);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Filter_WhenClaimsMatchesAllRelatedClaims_ReturnsNonEmptyCollectionOfClaims()
        {
            IScope sut = CreateSut(ClaimTypes.NameIdentifier, ClaimTypes.Name, ClaimTypes.Email);

            IEnumerable<Claim> claims = CreateClaims(ClaimTypes.NameIdentifier, ClaimTypes.Name, ClaimTypes.GivenName, ClaimTypes.Surname, ClaimTypes.MobilePhone, ClaimTypes.Email);
            IEnumerable<Claim> result = sut.Filter(claims);

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Filter_WhenClaimsMatchesAllRelatedClaims_ReturnsNonEmptyCollectionOfMatchingClaimsOnly()
        {
            IScope sut = CreateSut(ClaimTypes.NameIdentifier, ClaimTypes.Name, ClaimTypes.Email);

            IEnumerable<Claim> claims = CreateClaims(ClaimTypes.NameIdentifier, ClaimTypes.Name, ClaimTypes.GivenName, ClaimTypes.Surname, ClaimTypes.MobilePhone, ClaimTypes.Email);
            Claim[] result = sut.Filter(claims).ToArray();

            Assert.That(result.Any(claim => claim.Type == ClaimTypes.NameIdentifier), Is.True);
            Assert.That(result.Any(claim => claim.Type == ClaimTypes.Name), Is.True);
            Assert.That(result.Any(claim => claim.Type == ClaimTypes.Email), Is.True);
            Assert.That(result.Any(claim => claim.Type == ClaimTypes.GivenName), Is.False);
            Assert.That(result.Any(claim => claim.Type == ClaimTypes.Surname), Is.False);
            Assert.That(result.Any(claim => claim.Type == ClaimTypes.MobilePhone), Is.False);
        }

        private IScope CreateSut(params string[] relatedClaims)
        {
            if (relatedClaims == null || relatedClaims.Length == 0)
            {
                relatedClaims = _fixture.CreateMany<string>(_random.Next(5, 10)).ToArray();
            }

            return new Domain.Security.Scope(_fixture.Create<string>(), _fixture.Create<string>(), relatedClaims);
        }

        private IEnumerable<Claim> CreateClaims(params string[] claimTypes)
        {
            return _fixture.CreateClaims(_random)
                .Concat(claimTypes.Select(claimType => _fixture.CreateClaim(claimType)))
                .ToArray();
        }
    }
}