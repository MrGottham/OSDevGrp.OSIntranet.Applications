using AutoFixture;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Security.UserInfo;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries.Security.UserInfo.UserInfoFeature;

public abstract class UserInfoFeatureTestBase : SecurityPageFeatureTestBase
{
    #region Methods

    protected UserInfoRequest CreateUserInfoRequest(Fixture fixture, ISecurityContext? securityContext = null)
    {
        return new UserInfoRequest(Guid.NewGuid(), CultureInfo.InvariantCulture, securityContext ?? CreateSecurityContext(fixture));
    }

    #endregion
}