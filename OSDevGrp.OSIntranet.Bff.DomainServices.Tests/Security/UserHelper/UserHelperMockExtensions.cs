using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.UserHelper;

public static class UserHelperMockExtensions
{
    #region Methods

    internal static void Setup(this Mock<IUserHelper> userHelperMock, Fixture fixture, bool isAuthenticated = true, bool hasNameIdentifier = true, string? nameIdentifier = null, bool hasFullName = true, string? fullName = null, bool hasMailAddress = true, string? mailAddress = null, bool? hasAccountingAccess = null, bool hasDefaultAccountingNumber = true, int? defaultAccountingNumber = null, bool? isAccountingAdministrator = null, bool? isAccountingCreator = null, bool? isAccountingModifier = null, bool? isAccountingViewer = null, bool? hasCommonDataAccess = null)
    {
        hasAccountingAccess ??= fixture.Create<bool>();
        hasCommonDataAccess ??= fixture.Create<bool>();

        userHelperMock.Setup(m => m.IsAuthenticated(It.IsAny<ClaimsPrincipal>()))
            .Returns(isAuthenticated);
        userHelperMock.Setup(m => m.GetNameIdentifier(It.IsAny<ClaimsPrincipal>()))
            .Returns(hasNameIdentifier ? nameIdentifier ?? fixture.Create<string>() : null);
        userHelperMock.Setup(m => m.GetFullName(It.IsAny<ClaimsPrincipal>()))
            .Returns(hasFullName ? fullName ?? fixture.Create<string>() : null);
        userHelperMock.Setup(m => m.GetMailAddress(It.IsAny<ClaimsPrincipal>()))
            .Returns(hasMailAddress ? mailAddress ?? $"{fixture.Create<string>()}@{fixture.Create<string>()}.local" : null);
        userHelperMock.Setup(m => m.HasAccountingAccess(It.IsAny<ClaimsPrincipal>()))
            .Returns(hasAccountingAccess.Value);
        userHelperMock.Setup(m => m.GetDefaultAccountingNumber(It.IsAny<ClaimsPrincipal>()))
            .Returns(hasAccountingAccess.Value && hasDefaultAccountingNumber ? defaultAccountingNumber ?? fixture.Create<int>() : null);
        userHelperMock.Setup(m => m.IsAccountingAdministrator(It.IsAny<ClaimsPrincipal>()))
            .Returns(hasAccountingAccess.Value ? isAccountingAdministrator ?? fixture.Create<bool>() : false);
        userHelperMock.Setup(m => m.IsAccountingCreator(It.IsAny<ClaimsPrincipal>()))
            .Returns(hasAccountingAccess.Value ? isAccountingCreator ?? fixture.Create<bool>() : false);
        userHelperMock.Setup(m => m.IsAccountingModifier(It.IsAny<ClaimsPrincipal>(), It.IsAny<int?>()))
            .Returns(hasAccountingAccess.Value ? isAccountingModifier ?? fixture.Create<bool>() : false);
        userHelperMock.Setup(m => m.IsAccountingViewer(It.IsAny<ClaimsPrincipal>(), It.IsAny<int?>()))
            .Returns(hasAccountingAccess.Value ? isAccountingViewer ?? fixture.Create<bool>() : false);
        userHelperMock.Setup(m => m.HasCommonDataAccess(It.IsAny<ClaimsPrincipal>()))
            .Returns(hasCommonDataAccess.Value);
    }

    #endregion
}