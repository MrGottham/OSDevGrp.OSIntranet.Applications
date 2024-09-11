using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.UserAddress
{
    [TestFixture]
    public class ToJsonTests : UserAddressTestBase
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
        public void ToJson_WhenClaimsPrincipalHasNoClaims_ReturnsNull()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaims: false);
            IUserAddress sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasClaimsButNoneAddressClaims_ReturnsNull()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasStreetAddressClaim: false, hasLocalityClaim: false, hasStateOrProvinceClaim: false, hasPostalCodeClaim: false, hasCountryClaim: false);
            IUserAddress sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasAddressClaims_ReturnsNotNull()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal();
            IUserAddress sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasAddressClaims_ReturnsNotEmpty()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal();
            IUserAddress sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalDoesNotHaveStreetAddressClaim_ReturnsJsonWithoutStreetAddress()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasStreetAddressClaim: false);
            IUserAddress sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "street_address"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasStreetAddressClaimWithoutValue_ReturnsJsonWithoutStreetAddress()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasStreetAddressClaim: true, hasStreetAddressClaimValue: false);
            IUserAddress sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "street_address"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasStreetAddressClaimWithValue_ReturnsJsonWithStreetAddress()
        {
            string streetAddressClaimValue = _fixture.Create<string>();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasStreetAddressClaim: true, hasStreetAddressClaimValue: true, streetAddressClaimValue: streetAddressClaimValue);
            IUserAddress sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "street_address", streetAddressClaimValue), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalDoesNotHaveLocalityClaim_ReturnsJsonWithoutLocality()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasLocalityClaim: false);
            IUserAddress sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "locality"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasLocalityClaimWithoutValue_ReturnsJsonWithoutLocality()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasLocalityClaim: true, hasLocalityClaimValue: false);
            IUserAddress sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "locality"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasLocalityClaimWithValue_ReturnsJsonWithLocality()
        {
            string localityClaimValue = _fixture.Create<string>();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasLocalityClaim: true, hasLocalityClaimValue: true, localityClaimValue: localityClaimValue);
            IUserAddress sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "locality", localityClaimValue), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalDoesNotHaveStateOrProvinceClaim_ReturnsJsonWithoutStateOrProvince()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasStateOrProvinceClaim: false);
            IUserAddress sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "region"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasStateOrProvinceClaimWithoutValue_ReturnsJsonWithoutStateOrProvince()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasStateOrProvinceClaim: true, hasStateOrProvinceClaimValue: false);
            IUserAddress sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "region"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasStateOrProvinceClaimWithValue_ReturnsJsonWithStateOrProvince()
        {
            string stateOrProvinceClaimValue = _fixture.Create<string>();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasStateOrProvinceClaim: true, hasStateOrProvinceClaimValue: true, stateOrProvinceClaimValue: stateOrProvinceClaimValue);
            IUserAddress sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "region", stateOrProvinceClaimValue), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalDoesNotHavePostalCodeClaim_ReturnsJsonWithoutPostalCode()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasPostalCodeClaim: false);
            IUserAddress sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "postal_code"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasPostalCodeClaimWithoutValue_ReturnsJsonWithoutPostalCode()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasPostalCodeClaim: true, hasPostalCodeClaimValue: false);
            IUserAddress sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "postal_code"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasPostalCodeClaimWithValue_ReturnsJsonWithPostalCode()
        {
            string postalCodeClaimValue = _fixture.Create<string>();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasPostalCodeClaim: true, hasPostalCodeClaimValue: true, postalCodeClaimValue: postalCodeClaimValue);
            IUserAddress sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "postal_code", postalCodeClaimValue), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalDoesNotHaveCountryClaim_ReturnsJsonWithoutCountry()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasCountryClaim: false);
            IUserAddress sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "country"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasCountryClaimWithoutValue_ReturnsJsonWithoutCountry()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasCountryClaim: true, hasCountryClaimValue: false);
            IUserAddress sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "country"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasCountryClaimWithValue_ReturnsJsonWithCountry()
        {
            string countryClaimValue = _fixture.Create<string>();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasCountryClaim: true, hasCountryClaimValue: true, countryClaimValue: countryClaimValue);
            IUserAddress sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "country", countryClaimValue), Is.True);
        }

        private static bool HasMatchingJsonProperty(string json, string propertyName)
        {
            NullGuard.NotNullOrWhiteSpace(json, nameof(json))
                .NotNullOrWhiteSpace(propertyName, nameof(propertyName));

            return HasMatchingJson(json, $"(\"{propertyName}\"\\s*:(\\s*null)?){{1}}");
        }

        private static bool HasMatchingJsonProperty(string json, string propertyName, string value)
        {
            NullGuard.NotNullOrWhiteSpace(json, nameof(json))
                .NotNullOrWhiteSpace(propertyName, nameof(propertyName))
                .NotNullOrWhiteSpace(value, nameof(value));

            return HasMatchingJson(json, $"(\"{propertyName}\"\\s*:\\s*\"{value}\"){{1}}");
        }

        private static bool HasMatchingJson(string json, string matchingJsonPattern)
        {
            NullGuard.NotNullOrWhiteSpace(json, nameof(json))
                .NotNullOrWhiteSpace(matchingJsonPattern, nameof(matchingJsonPattern));

            return Regex.IsMatch(json, matchingJsonPattern, RegexOptions.Compiled, TimeSpan.FromMilliseconds(32));
        }
    }
}