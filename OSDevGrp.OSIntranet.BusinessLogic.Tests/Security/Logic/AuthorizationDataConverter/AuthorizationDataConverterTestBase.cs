using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.AuthorizationDataConverter
{
    public abstract class AuthorizationDataConverterTestBase
    {
        protected abstract Fixture Fixture { get; }

        protected abstract Random Random { get; }

        protected IReadOnlyDictionary<string, string> CreateAuthorizationData()
        {
            return Fixture.CreateMany<string>(Random.Next(5, 10))
                .ToDictionary(value => value, _ => Fixture.Create<string>())
                .AsReadOnly();
        }
    }
}