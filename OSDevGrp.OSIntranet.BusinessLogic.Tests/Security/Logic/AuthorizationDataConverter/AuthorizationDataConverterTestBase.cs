using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.AuthorizationDataConverter
{
    public abstract class AuthorizationDataConverterTestBase
    {
        protected abstract Fixture Fixture { get; }

        protected abstract Random Random { get; }

        protected IEnumerable<Claim> CreateClaims()
        {
            return Fixture.CreateMany<string>(Random.Next(5, 10))
                .Select(claimType => new Claim(claimType, Random.Next(100) > 50 ? Fixture.Create<string>() : string.Empty, Random.Next(100) > 50 ? Fixture.Create<string>() : null, Random.Next(100) > 50 ? Fixture.Create<string>() : null))
                .ToArray();
        }
    }
}