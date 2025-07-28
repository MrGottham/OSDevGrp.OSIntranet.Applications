using AutoFixture;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.AccountingPreCreation;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries.Accounting.AccountingPreCreation.AccountingPreCreationFeature;

public abstract class AccountingPreCreationFeatureTestBase
{
    #region Methods

    protected static AccountingPreCreationRequest CreateAccountingPreCreationRequestRequest(Fixture fixture, IFormatProvider? formatProvider = null, ISecurityContext? securityContext = null)
    {
        return new AccountingPreCreationRequest(Guid.NewGuid(), formatProvider ?? CultureInfo.InvariantCulture, securityContext ?? fixture.CreateSecurityContext());
    }

    #endregion
}