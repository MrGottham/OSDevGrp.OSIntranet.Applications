using AutoFixture;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;
using System.Linq;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.AuthorizationStateBuilder
{
    public abstract class AuthorizationStateBuilderTestBase
    {
        #region Methods

        protected static IAuthorizationStateBuilder CreateSut(Fixture fixture, Random random, string responseType = null, string clientId = null, Uri redirectUri = null, string[] scopes = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture))
                .NotNull(random, nameof(random));

            return new Domain.Security.AuthorizationStateBuilder(
                responseType ?? fixture.Create<string>(),
                clientId ?? fixture.Create<string>(), redirectUri ?? CreateRedirectUri(fixture),
                scopes ?? CreateScopes(fixture, random));
        }

        protected static Uri CreateRedirectUri(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return new Uri($"https://{fixture.Create<string>().Replace("/", string.Empty)}.local/{fixture.Create<string>().Replace("/", string.Empty)}", UriKind.Absolute);
        }

        protected static string[] CreateScopes(Fixture fixture, Random random)
        {
            NullGuard.NotNull(fixture, nameof(fixture))
                .NotNull(random, nameof(random));

            return fixture.CreateMany<string>(random.Next(5, 10)).ToArray();
        }

        #endregion
    }
}