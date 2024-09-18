using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.TestHelpers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;
using System.Security.Claims;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.UserInfo
{
    [TestFixture]
    public class ToJsonTests : UserInfoTestBase
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
        public void ToJson_WhenClaimsPrincipalHasNoClaims_ReturnsNotNull()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaims: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasNoClaims_ReturnsNotEmpty()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaims: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasNoClaims_ReturnsJsonWithNameIdentifier()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaims: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(result, Is.EqualTo("{\"sub\":null}"));
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalDoesNotHaveNameIdentifierClaim_ReturnsJsonWithNameIdentifier()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameIdentifierClaim: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "sub"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasNameIdentifierClaimWithoutValue_ReturnsJsonWithNameIdentifier()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "sub"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasNameIdentifierClaimWithValue_ReturnsJsonWithNameIdentifier()
        {
            string nameIdentifierClaimValue = _fixture.Create<string>();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: true, nameIdentifierClaimValue: nameIdentifierClaimValue);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "sub", nameIdentifierClaimValue), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalDoesNotHaveNameClaim_ReturnsJsonWithoutName()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameClaim: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "name"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasNameClaimWithoutValue_ReturnsJsonWithoutName()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameClaim: true, hasNameClaimValue: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "name"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasNameClaimWithValue_ReturnsJsonWithName()
        {
            string nameClaimValue = _fixture.Create<string>();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameClaim: true, hasNameClaimValue: true, nameClaimValue: nameClaimValue);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "name", nameClaimValue), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalDoesNotHaveGivenNameClaim_ReturnsJsonWithoutGivenName()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasGivenNameClaim: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "given_name"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasGivenNameClaimWithoutValue_ReturnsJsonWithoutGivenName()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasGivenNameClaim: true, hasGivenNameClaimValue: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "given_name"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasGivenNameClaimWithValue_ReturnsJsonWithGivenName()
        {
            string givenNameClaimValue = _fixture.Create<string>();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasGivenNameClaim: true, hasGivenNameClaimValue: true, givenNameClaimValue: givenNameClaimValue);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "given_name", givenNameClaimValue), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalDoesNotHaveSurnameClaim_ReturnsJsonWithoutSurname()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasSurnameClaim: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "family_name"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasSurnameClaimWithoutValue_ReturnsJsonWithoutSurname()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasSurnameClaim: true, hasSurnameClaimValue: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "family_name"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasSurnameClaimWithValue_ReturnsJsonWithSurname()
        {
            string surnameClaimValue = _fixture.Create<string>();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasSurnameClaim: true, hasSurnameClaimValue: true, surnameClaimValue: surnameClaimValue);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "family_name", surnameClaimValue), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenCalled_ReturnsJsonWithoutMiddleName()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal();
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "middle_name"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenCalled_ReturnsJsonWithoutNickName()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal();
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "nickname"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenCalled_ReturnsJsonWithoutPreferredUsername()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal();
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "preferred_username"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenCalled_ReturnsJsonWithoutProfile()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal();
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "profile"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenCalled_ReturnsJsonWithoutPicture()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal();
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "picture"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalDoesNotHaveWebpageClaim_ReturnsJsonWithoutWebpage()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasWebpageClaim: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "website"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasWebpageClaimWithoutValue_ReturnsJsonWithoutWebpage()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasWebpageClaim: true, hasWebpageClaimValue: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "website"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasWebpageClaimWithValue_ReturnsJsonWithWebpage()
        {
            Uri webpageClaimValue = _fixture.CreateEndpoint();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasWebpageClaim: true, hasWebpageClaimValue: true, webpageClaimValue: webpageClaimValue);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "website", webpageClaimValue.AbsoluteUri), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalDoesNotHaveEmailClaim_ReturnsJsonWithoutEmail()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasEmailClaim: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "email"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasEmailClaimWithoutValue_ReturnsJsonWithoutEmail()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasEmailClaim: true, hasEmailClaimValue: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "email"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasEmailClaimWithValue_ReturnsJsonWithEmail()
        {
            string emailClaimValue = _fixture.Create<string>();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasEmailClaim: true, hasEmailClaimValue: true, emailClaimValue: emailClaimValue);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "email", emailClaimValue), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenCalled_ReturnsJsonWithoutEmailVerified()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal();
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "email_verified"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalDoesNotHaveGenderClaim_ReturnsJsonWithoutGender()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasGenderClaim: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "gender"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasGenderClaimWithoutValue_ReturnsJsonWithoutGender()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasGenderClaim: true, hasGenderClaimValue: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "gender"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasGenderClaimWithValue_ReturnsJsonWithGender()
        {
            string genderClaimValue = _fixture.Create<string>();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasGenderClaim: true, hasGenderClaimValue: true, genderClaimValue: genderClaimValue);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "gender", genderClaimValue), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalDoesNotHaveBirthdateClaim_ReturnsJsonWithoutBirthdate()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasBirthdateClaim: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "birthdate"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasBirthdateClaimWithoutValue_ReturnsJsonWithoutBirthdate()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasBirthdateClaim: true, hasBirthdateClaimValue: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "birthdate"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasBirthdateClaimWithValue_ReturnsJsonWithBirthdate()
        {
            DateTimeOffset birthdateClaimValue = DateTimeOffset.Now.AddDays(Random.Next(365, 365 * 50) * -1);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasBirthdateClaim: true, hasBirthdateClaimValue: true, birthdateClaimValue: birthdateClaimValue);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "birthdate", birthdateClaimValue.ToString("yyyy-MM-dd", FormatProvider)), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenCalled_ReturnsJsonWithoutTimeZone()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal();
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "zoneinfo"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenCalled_ReturnsJsonWithoutLocale()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal();
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "locale"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalDoesNotHaveMobilePhoneClaimNorHomePhoneClaimNorOtherPhoneClaim_ReturnsJsonWithoutPhoneNumber()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasMobilePhoneClaim: false, hasHomePhoneClaim: false, hasOtherPhoneClaim: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "phone_number"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasMobilePhoneClaimWithoutValueAndNoHomePhoneClaimNorOtherPhoneClaim_ReturnsJsonWithoutPhoneNumber()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasMobilePhoneClaim: true, hasMobilePhoneClaimValue: false, hasHomePhoneClaim: false, hasOtherPhoneClaim: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "phone_number"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasHomePhoneClaimWithoutValueAndNoMobilePhoneClaimNorOtherPhoneClaim_ReturnsJsonWithoutPhoneNumber()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasMobilePhoneClaim: false, hasHomePhoneClaim: true, hasHomePhoneClaimValue: false, hasOtherPhoneClaim: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "phone_number"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasOtherPhoneClaimWithoutValueAndNoMobilePhoneClaimNorHomePhoneClaim_ReturnsJsonWithoutPhoneNumber()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasMobilePhoneClaim: false, hasHomePhoneClaim: false, hasOtherPhoneClaim: true, hasOtherPhoneClaimValue: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "phone_number"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void ToJson_WhenClaimsPrincipalHasMobilePhoneClaimWithValue_ReturnsJsonWithPhoneNumber(bool hasHomePhoneClaim, bool hasOtherPhoneClaim)
        {
            string mobilePhoneClaimValue = _fixture.Create<string>();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasMobilePhoneClaim: true, hasMobilePhoneClaimValue: true, mobilePhoneClaimValue: mobilePhoneClaimValue, hasHomePhoneClaim: hasHomePhoneClaim, hasOtherPhoneClaim: hasOtherPhoneClaim);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "phone_number", mobilePhoneClaimValue), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void ToJson_WhenClaimsPrincipalDoesNotHaveMobilePhoneClaimButHomePhoneClaimWithValue_ReturnsJsonWithPhoneNumber(bool hasOtherPhoneClaim)
        {
            string homePhoneClaimValue = _fixture.Create<string>();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasMobilePhoneClaim: false, hasHomePhoneClaim: true, hasHomePhoneClaimValue: true, homePhoneClaimValue: homePhoneClaimValue, hasOtherPhoneClaim: hasOtherPhoneClaim);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "phone_number", homePhoneClaimValue), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalDoesNotHaveMobilePhoneClaimNorHomePhoneClaimButOtherPhoneClaimWithValue_ReturnsJsonWithPhoneNumber()
        {
            string otherPhoneClaimValue = _fixture.Create<string>();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasMobilePhoneClaim: false, hasHomePhoneClaim: false, hasOtherPhoneClaim: true, hasOtherPhoneClaimValue: true, otherPhoneClaimValue: otherPhoneClaimValue);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "phone_number", otherPhoneClaimValue), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenCalled_ReturnsJsonWithoutPhoneNumberVerified()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal();
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "phone_number_verified"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalDoesNotHaveStreetAddressClaimNorLocalityClaimNorStateOrProvinceClaimNorPostalCodeClaimNorCountryClaim_ReturnsJsonWithoutAddress()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasStreetAddressClaim: false, hasLocalityClaim: false, hasStateOrProvinceClaim: false, hasPostalCodeClaim: false, hasCountryClaim: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "address"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasStreetAddressClaimWithoutValueAndNoLocalityClaimNorStateOrProvinceClaimNorPostalCodeClaimNorCountryClaim_ReturnsJsonWithoutAddress()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasStreetAddressClaim: true, hasStreetAddressClaimValue: false, hasLocalityClaim: false, hasStateOrProvinceClaim: false, hasPostalCodeClaim: false, hasCountryClaim: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "address"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasLocalityClaimWithoutValueAndNoStreetAddressClaimNorStateOrProvinceClaimNorPostalCodeClaimNorCountryClaim_ReturnsJsonWithoutAddress()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasStreetAddressClaim: false, hasLocalityClaim: true, hasLocalityClaimValue: false, hasStateOrProvinceClaim: false, hasPostalCodeClaim: false, hasCountryClaim: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "address"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasStateOrProvinceClaimWithoutValueAndNoStreetAddressClaimNorLocalityClaimNorPostalCodeClaimNorCountryClaim_ReturnsJsonWithoutAddress()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasStreetAddressClaim: false, hasLocalityClaim: false, hasStateOrProvinceClaim: true, hasStateOrProvinceClaimValue: false, hasPostalCodeClaim: false, hasCountryClaim: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "address"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasPostalCodeClaimWithoutValueAndNoStreetAddressClaimNorLocalityClaimNorStateOrProvinceClaimNorCountryClaim_ReturnsJsonWithoutAddress()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasStreetAddressClaim: false, hasLocalityClaim: false, hasStateOrProvinceClaim: false, hasPostalCodeClaim: true, hasPostalCodeClaimValue: false, hasCountryClaim: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "address"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenClaimsPrincipalHasCountryClaimWithoutValueAndNoStreetAddressClaimNorLocalityClaimNorStateOrProvinceClaimNorPostalCodeClaim_ReturnsJsonWithoutAddress()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasStreetAddressClaim: false, hasLocalityClaim: false, hasStateOrProvinceClaim: false, hasPostalCodeClaim: false, hasCountryClaim: true, hasCountryClaimValue: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "address"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true, true, true, true)]
        [TestCase(true, true, true, false)]
        [TestCase(true, true, false, true)]
        [TestCase(true, true, false, false)]
        [TestCase(true, false, true, true)]
        [TestCase(true, false, true, false)]
        [TestCase(true, false, false, true)]
        [TestCase(true, false, false, false)]
        [TestCase(false, true, true, true)]
        [TestCase(false, true, true, false)]
        [TestCase(false, true, false, true)]
        [TestCase(false, true, false, false)]
        [TestCase(false, false, true, true)]
        [TestCase(false, false, true, false)]
        [TestCase(false, false, false, true)]
        [TestCase(false, false, false, false)]
        public void ToJson_WhenClaimsPrincipalHasStreetAddressClaimWithValue_ReturnsJsonWithAddress(bool hasLocalityClaim, bool hasStateOrProvinceClaim, bool hasPostalCodeClaim, bool hasCountryClaim)
        {
            string streetAddressClaimValue = _fixture.Create<string>();
            string localityClaimValue = hasLocalityClaim ? _fixture.Create<string>() : string.Empty;
            string stateOrProvinceClaimValue = hasStateOrProvinceClaim ? _fixture.Create<string>() : string.Empty;
            string postalCodeClaimValue = hasPostalCodeClaim ? _fixture.Create<string>() : string.Empty;
            string countryClaimValue = hasCountryClaim ? _fixture.Create<string>() : string.Empty;
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasStreetAddressClaim: true, streetAddressClaimValue: streetAddressClaimValue, hasLocalityClaim: hasLocalityClaim, localityClaimValue: localityClaimValue, hasStateOrProvinceClaim: hasStateOrProvinceClaim, stateOrProvinceClaimValue: stateOrProvinceClaimValue, hasPostalCodeClaim: hasPostalCodeClaim, postalCodeClaimValue: postalCodeClaimValue, hasCountryClaim: hasCountryClaim, countryClaimValue: countryClaimValue);
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "address", JsonEncodedText.Encode(CreateUserAddress(claimsPrincipal).ToJson()).Value), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToJson_WhenCalled_ReturnsJsonWithoutUpdatedAt()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal();
            IUserInfo sut = CreateSut(claimsPrincipal);

            string result = sut.ToJson();

            Assert.That(HasMatchingJsonProperty(result, "updated_at"), Is.False);
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

            return HasMatchingJson(json, $"(\"{propertyName}\"\\s*:\\s*\"{value.Replace(@"\", @"\\")}\"){{1}}");
        }

        private static bool HasMatchingJson(string json, string matchingJsonPattern)
        {
            NullGuard.NotNullOrWhiteSpace(json, nameof(json))
                .NotNullOrWhiteSpace(matchingJsonPattern, nameof(matchingJsonPattern));

            return Regex.IsMatch(json, matchingJsonPattern, RegexOptions.Compiled, TimeSpan.FromMilliseconds(32));
        }
    }
}