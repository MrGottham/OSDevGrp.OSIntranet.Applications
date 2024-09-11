using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.UserAddress
{
    [TestFixture]
    public class IsEmptyTests : UserAddressTestBase
    {
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        protected override Fixture Fixture => _fixture;

        protected override Random Random => _random;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void IsEmpty_WhenClaimsPrincipalHasNoClaims_ReturnsTrue()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaims: false);
            IUserAddress sut = CreateSut(claimsPrincipal);

            bool result = sut.IsEmpty();

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void IsEmpty_WhenClaimsPrincipalHasClaimsButNoneAddressClaims_ReturnsTrue()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasStreetAddressClaim: false, hasLocalityClaim: false, hasStateOrProvinceClaim: false, hasPostalCodeClaim: false, hasCountryClaim: false);
            IUserAddress sut = CreateSut(claimsPrincipal);

            bool result = sut.IsEmpty();

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void IsEmpty_WhenClaimsPrincipalHasAddressClaimsButOnlyStreetAddressClaimWithoutValue_ReturnsTrue()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasStreetAddressClaim: true, hasStreetAddressClaimValue: false, hasLocalityClaim: false, hasStateOrProvinceClaim: false, hasPostalCodeClaim: false, hasCountryClaim: false);
            IUserAddress sut = CreateSut(claimsPrincipal);

            bool result = sut.IsEmpty();

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void IsEmpty_WhenClaimsPrincipalHasAddressClaimsButOnlyStreetAddressClaimWithValue_ReturnsFalse()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasStreetAddressClaim: true, hasStreetAddressClaimValue: true, hasLocalityClaim: false, hasStateOrProvinceClaim: false, hasPostalCodeClaim: false, hasCountryClaim: false);
            IUserAddress sut = CreateSut(claimsPrincipal);

            bool result = sut.IsEmpty();

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void IsEmpty_WhenClaimsPrincipalHasAddressClaimsButOnlyLocalityClaimWithoutValue_ReturnsTrue()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasStreetAddressClaim: false, hasLocalityClaim: true, hasLocalityClaimValue: false, hasStateOrProvinceClaim: false, hasPostalCodeClaim: false, hasCountryClaim: false);
            IUserAddress sut = CreateSut(claimsPrincipal);

            bool result = sut.IsEmpty();

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void IsEmpty_WhenClaimsPrincipalHasAddressClaimsButOnlyLocalityClaimWithValue_ReturnsFalse()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasStreetAddressClaim: false, hasLocalityClaim: true, hasLocalityClaimValue: true, hasStateOrProvinceClaim: false, hasPostalCodeClaim: false, hasCountryClaim: false);
            IUserAddress sut = CreateSut(claimsPrincipal);

            bool result = sut.IsEmpty();

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void IsEmpty_WhenClaimsPrincipalHasAddressClaimsButOnlyStateOrProvinceClaimWithoutValue_ReturnsTrue()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasStreetAddressClaim: false, hasLocalityClaim: false, hasStateOrProvinceClaim: true, hasStateOrProvinceClaimValue: false, hasPostalCodeClaim: false, hasCountryClaim: false);
            IUserAddress sut = CreateSut(claimsPrincipal);

            bool result = sut.IsEmpty();

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void IsEmpty_WhenClaimsPrincipalHasAddressClaimsButOnlyStateOrProvinceClaimWithValue_ReturnsFalse()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasStreetAddressClaim: false, hasLocalityClaim: false, hasStateOrProvinceClaim: true, hasStateOrProvinceClaimValue: true, hasPostalCodeClaim: false, hasCountryClaim: false);
            IUserAddress sut = CreateSut(claimsPrincipal);

            bool result = sut.IsEmpty();

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void IsEmpty_WhenClaimsPrincipalHasAddressClaimsButOnlyPostalCodeClaimValueWithoutValue_ReturnsTrue()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasStreetAddressClaim: false, hasLocalityClaim: false, hasStateOrProvinceClaim: false, hasPostalCodeClaim: true, hasPostalCodeClaimValue: false, hasCountryClaim: false);
            IUserAddress sut = CreateSut(claimsPrincipal);

            bool result = sut.IsEmpty();

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void IsEmpty_WhenClaimsPrincipalHasAddressClaimsButOnlyPostalCodeClaimValueWithValue_ReturnsFalse()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasStreetAddressClaim: false, hasLocalityClaim: false, hasStateOrProvinceClaim: false, hasPostalCodeClaim: true, hasPostalCodeClaimValue: true, hasCountryClaim: false);
            IUserAddress sut = CreateSut(claimsPrincipal);

            bool result = sut.IsEmpty();

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void IsEmpty_WhenClaimsPrincipalHasAddressClaimsButOnlyCountryClaimValueWithoutValue_ReturnsTrue()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasStreetAddressClaim: false, hasLocalityClaim: false, hasStateOrProvinceClaim: false, hasPostalCodeClaim: false, hasCountryClaim: true, hasCountryClaimValue: false);
            IUserAddress sut = CreateSut(claimsPrincipal);

            bool result = sut.IsEmpty();

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void IsEmpty_WhenClaimsPrincipalHasAddressClaimsButOnlyCountryClaimValueWithValue_ReturnsFalse()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasStreetAddressClaim: false, hasLocalityClaim: false, hasStateOrProvinceClaim: false, hasPostalCodeClaim: false, hasCountryClaim: true, hasCountryClaimValue: true);
            IUserAddress sut = CreateSut(claimsPrincipal);

            bool result = sut.IsEmpty();

            Assert.That(result, Is.False);
        }
    }
}