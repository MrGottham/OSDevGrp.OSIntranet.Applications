using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.UserHelper;

public static class PermissionCheckerMockExtensions
{
    #region Methods

    internal static void Setup(this Mock<IPermissionChecker> permissionCheckerMock, Fixture fixture, bool isAuthenticated = true, bool? hasAccountingAccess = null, bool? isAccountingAdministrator = null, bool? isAccountingCreator = null, bool? isAccountingModifier = null, bool? isAccountingViewer = null, bool? hasCommonDataAccess = null)
    {
        hasAccountingAccess ??= fixture.Create<bool>();
        hasCommonDataAccess ??= fixture.Create<bool>();

        permissionCheckerMock.Setup(m => m.IsAuthenticated(It.IsAny<ClaimsPrincipal>()))
            .Returns(isAuthenticated);
        permissionCheckerMock.Setup(m => m.HasAccountingAccess(It.IsAny<ClaimsPrincipal>()))
            .Returns(hasAccountingAccess.Value);
        permissionCheckerMock.Setup(m => m.IsAccountingAdministrator(It.IsAny<ClaimsPrincipal>()))
            .Returns(hasAccountingAccess.Value ? isAccountingAdministrator ?? fixture.Create<bool>() : false);
        permissionCheckerMock.Setup(m => m.IsAccountingCreator(It.IsAny<ClaimsPrincipal>()))
            .Returns(hasAccountingAccess.Value ? isAccountingCreator ?? fixture.Create<bool>() : false);
        permissionCheckerMock.Setup(m => m.IsAccountingModifier(It.IsAny<ClaimsPrincipal>(), It.IsAny<int?>()))
            .Returns(hasAccountingAccess.Value ? isAccountingModifier ?? fixture.Create<bool>() : false);
        permissionCheckerMock.Setup(m => m.IsAccountingViewer(It.IsAny<ClaimsPrincipal>(), It.IsAny<int?>()))
            .Returns(hasAccountingAccess.Value ? isAccountingViewer ?? fixture.Create<bool>() : false);
        permissionCheckerMock.Setup(m => m.HasCommonDataAccess(It.IsAny<ClaimsPrincipal>()))
            .Returns(hasCommonDataAccess.Value);
    }

    #endregion
}