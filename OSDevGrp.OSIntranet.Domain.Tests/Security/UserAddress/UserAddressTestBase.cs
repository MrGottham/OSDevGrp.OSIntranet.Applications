using AutoFixture;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.UserAddress
{
    public abstract class UserAddressTestBase 
    {
        #region Properties

        protected abstract Fixture Fixture { get; }

        protected abstract Random Random { get; }

        #endregion

        #region Methods

        protected IUserAddress CreateSut(ClaimsPrincipal claimsPrincipal = null)
        {
            return new Domain.Security.UserAddress(claimsPrincipal ?? CreateClaimsPrincipal());
        }

        protected ClaimsPrincipal CreateClaimsPrincipal(bool hasClaims = true, bool hasStreetAddressClaim = true, bool hasStreetAddressClaimValue = true, string streetAddressClaimValue = null, bool hasLocalityClaim = true, bool hasLocalityClaimValue = true, string localityClaimValue = null, bool hasStateOrProvinceClaim = true, bool hasStateOrProvinceClaimValue = true, string stateOrProvinceClaimValue = null, bool hasPostalCodeClaim = true, bool hasPostalCodeClaimValue = true, string postalCodeClaimValue = null, bool hasCountryClaim = true, bool hasCountryClaimValue = true, string countryClaimValue = null)
        {
            IList<Claim> claims = new List<Claim>(hasClaims ? Fixture.CreateClaims(Random) : []);
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