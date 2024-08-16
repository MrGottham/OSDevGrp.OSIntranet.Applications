using AutoFixture;
using OSDevGrp.OSIntranet.Core;
using System;
using System.Linq;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Domain.TestHelpers
{
    public static class FixtureExtensions
    {
        #region Methods

        public static Claim CreateClaim(this Fixture fixture, string type = null, bool hasValue = true, string value = null, string valueType = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            if (string.IsNullOrWhiteSpace(valueType))
            {
                return new Claim(fixture.GetClaimType(type), fixture.GetClaimValue(hasValue, value));
            }

            return new Claim(fixture.GetClaimType(type), fixture.GetClaimValue(hasValue, value), valueType);
        }

        public static Claim[] CreateClaims(this Fixture fixture, Random random)
        {
            NullGuard.NotNull(fixture, nameof(fixture))
                .NotNull(random, nameof(random));

            return fixture.CreateMany<string>(random.Next(5, 10))
                .Select(claimType => fixture.CreateClaim(claimType))
                .ToArray();
        }

        private static string GetClaimType(this Fixture fixture, string type)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return string.IsNullOrWhiteSpace(type) == false ? type : fixture.Create<string>();
        }

        private static string GetClaimValue(this Fixture fixture, bool hasValue, string value)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return hasValue ? value ?? fixture.Create<string>() : string.Empty;
        }

        #endregion
    }
}