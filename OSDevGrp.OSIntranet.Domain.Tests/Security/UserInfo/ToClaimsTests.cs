using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.TestHelpers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.UserInfo
{
    [TestFixture]
    public class ToClaimsTests : UserInfoTestBase
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
        public void ToClaims_WhenClaimsPrincipalHasNoClaims_ReturnsNotNull()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaims: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalHasNoClaims_ReturnsNonEmptyCollectionOfClaims()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaims: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalHasNoClaims_ReturnsNonEmptyCollectionOfClaimsWithNameIdentifierClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasClaims: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "sub"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalDoesNotHaveNameIdentifierClaim_ReturnsNonEmptyCollectionOfClaimsWithNameIdentifierClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameIdentifierClaim: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "sub"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalHasNameIdentifierClaimWithoutValue_ReturnsNonEmptyCollectionOfClaimsWithNameIdentifierClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "sub"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalHasNameIdentifierClaimWithValue_ReturnsNonEmptyCollectionOfClaimsWithNameIdentifierClaim()
        {
            string nameIdentifierClaimValue = _fixture.Create<string>();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: true, nameIdentifierClaimValue: nameIdentifierClaimValue);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "sub", nameIdentifierClaimValue), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalDoesNotHaveNameClaim_ReturnsNonEmptyCollectionOfClaimsWithoutNameClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameClaim: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "name"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalHasNameClaimWithoutValue_ReturnsNonEmptyCollectionOfClaimsWithoutNameClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameClaim: true, hasNameClaimValue: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "name"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalHasNameClaimWithValue_ReturnsNonEmptyCollectionOfClaimsWithNameClaim()
        {
            string nameClaimValue = _fixture.Create<string>();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasNameClaim: true, hasNameClaimValue: true, nameClaimValue: nameClaimValue);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "name", nameClaimValue), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalDoesNotHaveGivenNameClaim_ReturnsNonEmptyCollectionOfClaimsWithoutGivenNameClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasGivenNameClaim: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "given_name"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalHasGivenNameClaimWithoutValue_ReturnsNonEmptyCollectionOfClaimsWithoutGivenNameClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasGivenNameClaim: true, hasGivenNameClaimValue: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "given_name"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalHasGivenNameClaimWithValue_ReturnsNonEmptyCollectionOfClaimsWithGivenNameClaim()
        {
            string givenNameClaimValue = _fixture.Create<string>();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasGivenNameClaim: true, hasGivenNameClaimValue: true, givenNameClaimValue: givenNameClaimValue);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "given_name", givenNameClaimValue), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalDoesNotHaveSurnameClaim_ReturnsNonEmptyCollectionOfClaimsWithoutSurnameClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasSurnameClaim: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "family_name"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalHasSurnameClaimWithoutValue_ReturnsNonEmptyCollectionOfClaimsWithoutSurnameClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasSurnameClaim: true, hasSurnameClaimValue: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "family_name"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalHasSurnameClaimWithValue_ReturnsNonEmptyCollectionOfClaimsWithSurnameClaim()
        {
            string surnameClaimValue = _fixture.Create<string>();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasSurnameClaim: true, hasSurnameClaimValue: true, surnameClaimValue: surnameClaimValue);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "family_name", surnameClaimValue), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenCalled_ReturnsNonEmptyCollectionOfClaimsWithoutMiddleNameClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal();
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "middle_name"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenCalled_ReturnsNonEmptyCollectionOfClaimsWithoutNickNameClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal();
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "nickname"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenCalled_ReturnsNonEmptyCollectionOfClaimsWithoutPreferredUsernameClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal();
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "preferred_username"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenCalled_ReturnsNonEmptyCollectionOfClaimsWithoutProfileClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal();
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "profile"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenCalled_ReturnsNonEmptyCollectionOfClaimsWithoutPictureClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal();
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "picture"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalDoesNotHaveWebpageClaim_ReturnsNonEmptyCollectionOfClaimsWithoutWebpageClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasWebpageClaim: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "website"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalHasWebpageClaimWithoutValue_ReturnsNonEmptyCollectionOfClaimsWithoutWebpageClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasWebpageClaim: true, hasWebpageClaimValue: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "website"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalHasWebpageClaimWithValue_ReturnsNonEmptyCollectionOfClaimsWithWebpageClaim()
        {
            Uri webpageClaimValue = _fixture.CreateEndpoint();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasWebpageClaim: true, hasWebpageClaimValue: true, webpageClaimValue: webpageClaimValue);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "website", webpageClaimValue.AbsoluteUri), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalDoesNotHaveEmailClaim_ReturnsNonEmptyCollectionOfClaimsWithoutEmailClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasEmailClaim: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "email"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalHasEmailClaimWithoutValue_ReturnsNonEmptyCollectionOfClaimsWithoutEmailClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasEmailClaim: true, hasEmailClaimValue: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "email"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalHasEmailClaimWithValue_ReturnsNonEmptyCollectionOfClaimsWithEmailClaim()
        {
            string emailClaimValue = _fixture.Create<string>();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasEmailClaim: true, hasEmailClaimValue: true, emailClaimValue: emailClaimValue);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "email", emailClaimValue), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenCalled_ReturnsNonEmptyCollectionOfClaimsWithoutEmailVerifiedClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal();
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "email_verified"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalDoesNotHaveGenderClaim_ReturnsNonEmptyCollectionOfClaimsWithoutGenderClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasGenderClaim: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "gender"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalHasGenderClaimWithoutValue_ReturnsNonEmptyCollectionOfClaimsWithoutGenderClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasGenderClaim: true, hasGenderClaimValue: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "gender"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalHasGenderClaimWithValue_ReturnsNonEmptyCollectionOfClaimsWithGenderClaim()
        {
            string genderClaimValue = _fixture.Create<string>();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasGenderClaim: true, hasGenderClaimValue: true, genderClaimValue: genderClaimValue);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "gender", genderClaimValue), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalDoesNotHaveBirthdateClaim_ReturnsNonEmptyCollectionOfClaimsWithoutBirthdateClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasBirthdateClaim: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "birthdate"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalHasBirthdateClaimWithoutValue_ReturnsNonEmptyCollectionOfClaimsWithoutBirthdateClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasBirthdateClaim: true, hasBirthdateClaimValue: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "birthdate"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalHasBirthdateClaimWithValue_ReturnsNonEmptyCollectionOfClaimsWithBirthdateClaim()
        {
            DateTimeOffset birthdateClaimValue = DateTimeOffset.Now.AddDays(Random.Next(365, 365 * 50) * -1);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasBirthdateClaim: true, hasBirthdateClaimValue: true, birthdateClaimValue: birthdateClaimValue);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "birthdate", birthdateClaimValue.ToString("yyyy-MM-dd", FormatProvider)), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenCalled_ReturnsNonEmptyCollectionOfClaimsWithoutTimeZoneClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal();
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "zoneinfo"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenCalled_ReturnsNonEmptyCollectionOfClaimsWithoutLocaleClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal();
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "locale"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalDoesNotHaveMobilePhoneClaimNorHomePhoneClaimNorOtherPhoneClaim_ReturnsNonEmptyCollectionOfClaimsWithoutPhoneNumberClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasMobilePhoneClaim: false, hasHomePhoneClaim: false, hasOtherPhoneClaim: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "phone_number"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalHasMobilePhoneClaimWithoutValueAndNoHomePhoneClaimNorOtherPhoneClaim_ReturnsNonEmptyCollectionOfClaimsWithoutPhoneNumberClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasMobilePhoneClaim: true, hasMobilePhoneClaimValue: false, hasHomePhoneClaim: false, hasOtherPhoneClaim: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "phone_number"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalHasHomePhoneClaimWithoutValueAndNoMobilePhoneClaimNorOtherPhoneClaim_ReturnsNonEmptyCollectionOfClaimsWithoutPhoneNumberClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasMobilePhoneClaim: false, hasHomePhoneClaim: true, hasHomePhoneClaimValue: false, hasOtherPhoneClaim: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "phone_number"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalHasOtherPhoneClaimWithoutValueAndNoMobilePhoneClaimNorHomePhoneClaim_ReturnsNonEmptyCollectionOfClaimsWithoutPhoneNumberClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasMobilePhoneClaim: false, hasHomePhoneClaim: false, hasOtherPhoneClaim: true, hasOtherPhoneClaimValue: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "phone_number"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void ToClaims_WhenClaimsPrincipalHasMobilePhoneClaimWithValue_ReturnsNonEmptyCollectionOfClaimsWithPhoneNumberClaim(bool hasHomePhoneClaim, bool hasOtherPhoneClaim)
        {
            string mobilePhoneClaimValue = _fixture.Create<string>();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasMobilePhoneClaim: true, hasMobilePhoneClaimValue: true, mobilePhoneClaimValue: mobilePhoneClaimValue, hasHomePhoneClaim: hasHomePhoneClaim, hasOtherPhoneClaim: hasOtherPhoneClaim);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "phone_number", mobilePhoneClaimValue), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void ToClaims_WhenClaimsPrincipalDoesNotHaveMobilePhoneClaimButHomePhoneClaimWithValue_ReturnsNonEmptyCollectionOfClaimsWithPhoneNumberClaim(bool hasOtherPhoneClaim)
        {
            string homePhoneClaimValue = _fixture.Create<string>();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasMobilePhoneClaim: false, hasHomePhoneClaim: true, hasHomePhoneClaimValue: true, homePhoneClaimValue: homePhoneClaimValue, hasOtherPhoneClaim: hasOtherPhoneClaim);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "phone_number", homePhoneClaimValue), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalDoesNotHaveMobilePhoneClaimNorHomePhoneClaimButOtherPhoneClaimWithValue_ReturnsNonEmptyCollectionOfClaimsWithPhoneNumberClaim()
        {
            string otherPhoneClaimValue = _fixture.Create<string>();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasMobilePhoneClaim: false, hasHomePhoneClaim: false, hasOtherPhoneClaim: true, hasOtherPhoneClaimValue: true, otherPhoneClaimValue: otherPhoneClaimValue);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "phone_number", otherPhoneClaimValue), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenCalled_ReturnsNonEmptyCollectionOfClaimsWithoutPhoneNumberVerifiedClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal();
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "phone_number_verified"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalDoesNotHaveStreetAddressClaimNorLocalityClaimNorStateOrProvinceClaimNorPostalCodeClaimNorCountryClaim_ReturnsNonEmptyCollectionOfClaimsWithoutAddressClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasStreetAddressClaim: false, hasLocalityClaim: false, hasStateOrProvinceClaim: false, hasPostalCodeClaim: false, hasCountryClaim: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "address"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalHasStreetAddressClaimWithoutValueAndNoLocalityClaimNorStateOrProvinceClaimNorPostalCodeClaimNorCountryClaim_ReturnsNonEmptyCollectionOfClaimsWithoutAddressClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasStreetAddressClaim: true, hasStreetAddressClaimValue: false, hasLocalityClaim: false, hasStateOrProvinceClaim: false, hasPostalCodeClaim: false, hasCountryClaim: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "address"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalHasLocalityClaimWithoutValueAndNoStreetAddressClaimNorStateOrProvinceClaimNorPostalCodeClaimNorCountryClaim_ReturnsNonEmptyCollectionOfClaimsWithoutAddressClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasStreetAddressClaim: false, hasLocalityClaim: true, hasLocalityClaimValue: false, hasStateOrProvinceClaim: false, hasPostalCodeClaim: false, hasCountryClaim: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "address"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalHasStateOrProvinceClaimWithoutValueAndNoStreetAddressClaimNorLocalityClaimNorPostalCodeClaimNorCountryClaim_ReturnsNonEmptyCollectionOfClaimsWithoutAddressClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasStreetAddressClaim: false, hasLocalityClaim: false, hasStateOrProvinceClaim: true, hasStateOrProvinceClaimValue: false, hasPostalCodeClaim: false, hasCountryClaim: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "address"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalHasPostalCodeClaimWithoutValueAndNoStreetAddressClaimNorLocalityClaimNorStateOrProvinceClaimNorCountryClaim_ReturnsNonEmptyCollectionOfClaimsWithoutAddressClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasStreetAddressClaim: false, hasLocalityClaim: false, hasStateOrProvinceClaim: false, hasPostalCodeClaim: true, hasPostalCodeClaimValue: false, hasCountryClaim: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "address"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenClaimsPrincipalHasCountryClaimWithoutValueAndNoStreetAddressClaimNorLocalityClaimNorStateOrProvinceClaimNorPostalCodeClaim_ReturnsNonEmptyCollectionOfClaimsWithoutAddressClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasStreetAddressClaim: false, hasLocalityClaim: false, hasStateOrProvinceClaim: false, hasPostalCodeClaim: false, hasCountryClaim: true, hasCountryClaimValue: false);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "address"), Is.False);
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
        public void ToClaims_WhenClaimsPrincipalHasStreetAddressClaimWithValue_ReturnsNonEmptyCollectionOfClaimsWithAddressClaim(bool hasLocalityClaim, bool hasStateOrProvinceClaim, bool hasPostalCodeClaim, bool hasCountryClaim)
        {
            string streetAddressClaimValue = _fixture.Create<string>();
            string localityClaimValue = hasLocalityClaim ? _fixture.Create<string>() : string.Empty;
            string stateOrProvinceClaimValue = hasStateOrProvinceClaim ? _fixture.Create<string>() : string.Empty;
            string postalCodeClaimValue = hasPostalCodeClaim ? _fixture.Create<string>() : string.Empty;
            string countryClaimValue = hasCountryClaim ? _fixture.Create<string>() : string.Empty;
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasStreetAddressClaim: true, streetAddressClaimValue: streetAddressClaimValue, hasLocalityClaim: hasLocalityClaim, localityClaimValue: localityClaimValue, hasStateOrProvinceClaim: hasStateOrProvinceClaim, stateOrProvinceClaimValue: stateOrProvinceClaimValue, hasPostalCodeClaim: hasPostalCodeClaim, postalCodeClaimValue: postalCodeClaimValue, hasCountryClaim: hasCountryClaim, countryClaimValue: countryClaimValue);
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "address", CreateUserAddress(claimsPrincipal).ToJson()), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ToClaims_WhenCalled_ReturnsNonEmptyCollectionOfClaimsWithoutUpdatedAtClaim()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal();
            IUserInfo sut = CreateSut(claimsPrincipal);

            IEnumerable<Claim> result = sut.ToClaims();

            Assert.That(HasMatchingClaim(result, "updated_at"), Is.False);
        }

        private static bool HasMatchingClaim(IEnumerable<Claim> claims, string claimName)
        {
            NullGuard.NotNull(claims, nameof(claims))
                .NotNullOrWhiteSpace(claimName, nameof(claimName));

            return ResolveClaim(claims, claimName) != null;
        }

        private static bool HasMatchingClaim(IEnumerable<Claim> claims, string claimName, string claimValue)
        {
            NullGuard.NotNull(claims, nameof(claims))
                .NotNullOrWhiteSpace(claimName, nameof(claimName))
                .NotNullOrWhiteSpace(claimValue, nameof(claimValue));

            Claim claim = ResolveClaim(claims, claimName);
            if (claim == null)
            {
                return false;
            }

            return string.CompareOrdinal(claim.Value, claimValue) == 0;
        }

        private static Claim ResolveClaim(IEnumerable<Claim> claims, string claimName)
        {
            NullGuard.NotNull(claims, nameof(claims))
                .NotNullOrWhiteSpace(claimName, nameof(claimName));

            return new ClaimsIdentity(claims).FindFirst(claimName);
        }
    }
}