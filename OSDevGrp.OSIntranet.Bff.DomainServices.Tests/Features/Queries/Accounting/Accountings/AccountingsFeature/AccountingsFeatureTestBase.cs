using AutoFixture;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.Accountings;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries.Accounting.Accountings.AccountingsFeature;

public abstract class AccountingsFeatureTestBase : AccountingPageFeatureTestBase
{
    #region Methods

    protected static AccountingsRequest CreateAccountingsRequest(Fixture fixture, ISecurityContext? securityContext = null)
    {
        return new AccountingsRequest(Guid.NewGuid(), CultureInfo.InvariantCulture, securityContext ?? CreateSecurityContext(fixture));
    }

    #endregion
}