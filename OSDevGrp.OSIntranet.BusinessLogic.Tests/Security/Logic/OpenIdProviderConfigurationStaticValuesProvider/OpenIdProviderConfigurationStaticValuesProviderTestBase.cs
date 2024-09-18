using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.OpenIdProviderConfigurationStaticValuesProvider
{
    public abstract class OpenIdProviderConfigurationStaticValuesProviderTestBase
    {
        protected static IOpenIdProviderConfigurationStaticValuesProvider CreateSut(Mock<ISupportedScopesProvider> supportedScopesProviderMock, Fixture fixture, IDictionary<string, IScope> supportedScopes = null)
        {
            NullGuard.NotNull(supportedScopesProviderMock, nameof(supportedScopesProviderMock));

            supportedScopes ??= new Dictionary<string, IScope>
            {
                {fixture.Create<string>(), fixture.BuildScopeMock().Object},
                {fixture.Create<string>(), fixture.BuildScopeMock().Object},
                {fixture.Create<string>(), fixture.BuildScopeMock().Object}
            };

            supportedScopesProviderMock.Setup(m => m.SupportedScopes)
                .Returns(new ReadOnlyDictionary<string, IScope>(supportedScopes));

            return new BusinessLogic.Security.Logic.OpenIdProviderConfigurationStaticValuesProvider(supportedScopesProviderMock.Object);
        }

        protected static bool HasRs256Algorithm(IReadOnlyCollection<string> values)
        {
            NullGuard.NotNull(values, nameof(values));

            return values.Contains("RS256");
        }

        protected static bool HasNoneAlgorithm(IReadOnlyCollection<string> values)
        {
            NullGuard.NotNull(values, nameof(values));

            return values.Contains("none");
        }

        protected static bool HasEnUsLocal(IReadOnlyCollection<string> values)
        {
            NullGuard.NotNull(values, nameof(values));

            return values.Contains("en-US");
        }
    }
}