using AutoFixture;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.ScopeBuilder
{
    public abstract class ScopeBuilderTestBase
    {
        protected static IScopeBuilder CreateSut(Fixture fixture, string name = null, string description = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return new Domain.Security.ScopeBuilder(
                name ?? fixture.Create<string>(),
                description ?? fixture.Create<string>());
        }
    }
}