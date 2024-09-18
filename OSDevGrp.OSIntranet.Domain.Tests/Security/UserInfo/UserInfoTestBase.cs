using AutoFixture;
using OSDevGrp.OSIntranet.Core.TestHelpers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.UserInfo
{
    public abstract class UserInfoTestBase
    {
        #region Properties

        protected abstract Fixture Fixture { get; }

        protected abstract Random Random { get; }

        protected IFormatProvider FormatProvider => CultureInfo.InvariantCulture;

        #endregion

        #region Methods

        protected IUserInfo CreateSut(ClaimsPrincipal claimsPrincipal = null)
        {
            return new Domain.Security.UserInfo(claimsPrincipal ?? CreateClaimsPrincipal());
        }

        protected IUserAddress CreateUserAddress(ClaimsPrincipal claimsPrincipal = null)
        {
            return new Domain.Security.UserAddress(claimsPrincipal ?? CreateClaimsPrincipal());
        }

        protected ClaimsPrincipal CreateClaimsPrincipal(bool hasClaims = true, bool hasNameIdentifierClaim = true, bool hasNameIdentifierClaimValue = true, string nameIdentifierClaimValue = null, bool hasNameClaim = true, bool hasNameClaimValue = true, string nameClaimValue = null, bool hasGivenNameClaim = true, bool hasGivenNameClaimValue = true, string givenNameClaimValue = null, bool hasSurnameClaim = true, bool hasSurnameClaimValue = true, string surnameClaimValue = null, bool hasWebpageClaim = true, bool hasWebpageClaimValue = true, Uri webpageClaimValue = null, bool hasEmailClaim = true, bool hasEmailClaimValue = true, string emailClaimValue = null, bool hasGenderClaim = true, bool hasGenderClaimValue = true, string genderClaimValue = null, bool hasBirthdateClaim = true, bool hasBirthdateClaimValue = true, DateTimeOffset? birthdateClaimValue = null, bool hasMobilePhoneClaim = true, bool hasMobilePhoneClaimValue = true, string mobilePhoneClaimValue = null, bool hasHomePhoneClaim = true, bool hasHomePhoneClaimValue = true, string homePhoneClaimValue = null, bool hasOtherPhoneClaim = true, bool hasOtherPhoneClaimValue = true, string otherPhoneClaimValue = null, bool hasStreetAddressClaim = true, bool hasStreetAddressClaimValue = true, string streetAddressClaimValue = null, bool hasLocalityClaim = true, bool hasLocalityClaimValue = true, string localityClaimValue = null, bool hasStateOrProvinceClaim = true, bool hasStateOrProvinceClaimValue = true, string stateOrProvinceClaimValue = null, bool hasPostalCodeClaim = true, bool hasPostalCodeClaimValue = true, string postalCodeClaimValue = null, bool hasCountryClaim = true, bool hasCountryClaimValue = true, string countryClaimValue = null)
        {
            IList<Claim> claims = new List<Claim>(hasClaims ? Fixture.CreateClaims(Random) : []);
            if (hasClaims && hasNameIdentifierClaim)
            {
                claims.Add(Fixture.CreateClaim(ClaimTypes.NameIdentifier, hasValue: hasNameIdentifierClaimValue, value: nameIdentifierClaimValue));
            }
            if (hasClaims && hasNameClaim)
            {
                claims.Add(Fixture.CreateClaim(ClaimTypes.Name, hasValue: hasNameClaimValue, value: nameClaimValue));
            }
            if (hasClaims && hasGivenNameClaim)
            {
                claims.Add(Fixture.CreateClaim(ClaimTypes.GivenName, hasValue: hasGivenNameClaimValue, value: givenNameClaimValue));
            }
            if (hasClaims && hasSurnameClaim)
            {
                claims.Add(Fixture.CreateClaim(ClaimTypes.Surname, hasValue: hasSurnameClaimValue, value: surnameClaimValue));
            }
            if (hasClaims && hasWebpageClaim)
            {
                claims.Add(Fixture.CreateClaim(ClaimTypes.Webpage, hasValue: hasWebpageClaimValue, value: (webpageClaimValue ?? Fixture.CreateEndpoint()).AbsoluteUri));
            }
            if (hasClaims && hasEmailClaim)
            {
                claims.Add(Fixture.CreateClaim(ClaimTypes.Email, hasValue: hasEmailClaimValue, value: emailClaimValue));
            }
            if (hasClaims && hasGenderClaim)
            {
                claims.Add(Fixture.CreateClaim(ClaimTypes.Gender, hasValue: hasGenderClaimValue, value: genderClaimValue));
            }
            if (hasClaims && hasBirthdateClaim)
            {
                claims.Add(Fixture.CreateClaim(ClaimTypes.DateOfBirth, hasValue: hasBirthdateClaimValue, value: (birthdateClaimValue ?? DateTimeOffset.Now.AddDays(Random.Next(365, 365 * 50) * -1)).ToString("O", FormatProvider)));
            }
            if (hasClaims && hasMobilePhoneClaim)
            {
                claims.Add(Fixture.CreateClaim(ClaimTypes.MobilePhone, hasValue: hasMobilePhoneClaimValue, value: mobilePhoneClaimValue));
            }
            if (hasClaims && hasHomePhoneClaim)
            {
                claims.Add(Fixture.CreateClaim(ClaimTypes.HomePhone, hasValue: hasHomePhoneClaimValue, value: homePhoneClaimValue));
            }
            if (hasClaims && hasOtherPhoneClaim)
            {
                claims.Add(Fixture.CreateClaim(ClaimTypes.OtherPhone, hasValue: hasOtherPhoneClaimValue, value: otherPhoneClaimValue));
            }
            if (hasClaims && hasStreetAddressClaim)
            {
                claims.Add(Fixture.CreateClaim(ClaimTypes.StreetAddress, hasValue: hasStreetAddressClaimValue, value: streetAddressClaimValue));
            }
            if (hasClaims && hasLocalityClaim)
            {
                claims.Add(Fixture.CreateClaim(ClaimTypes.Locality, hasValue: hasLocalityClaimValue, value: localityClaimValue));
            }
            if (hasClaims && hasStateOrProvinceClaim)
            {
                claims.Add(Fixture.CreateClaim(ClaimTypes.StateOrProvince, hasValue: hasStateOrProvinceClaimValue, value: stateOrProvinceClaimValue));
            }
            if (hasClaims && hasPostalCodeClaim)
            {
                claims.Add(Fixture.CreateClaim(ClaimTypes.PostalCode, hasValue: hasPostalCodeClaimValue, value: postalCodeClaimValue));
            }
            if (hasClaims && hasCountryClaim)
            {
                claims.Add(Fixture.CreateClaim(ClaimTypes.Country, hasValue: hasCountryClaimValue, value: countryClaimValue));
            }
            return new ClaimsPrincipal(new ClaimsIdentity(claims, Fixture.Create<string>()));
        }

        #endregion
    }
}