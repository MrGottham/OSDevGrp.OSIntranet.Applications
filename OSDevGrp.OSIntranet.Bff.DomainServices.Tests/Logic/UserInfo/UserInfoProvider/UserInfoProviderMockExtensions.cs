using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.UserInfo;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Models.Home;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Models.Home.UserInfoModel;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.UserInfo.UserInfoProvider;

internal static class UserInfoProviderMockExtensions
{
    #region Methods

    internal static void Setup(this Mock<IUserInfoProvider> userInfoProviderMock, Fixture fixture, Random random, bool? isAuthenticated = null, bool hasUserInfo = true, IUserInfoModel? userInfoModel = null)
    {
        userInfoProviderMock.Setup(m => m.IsAuthenticated(It.IsNotNull<ClaimsPrincipal>()))
            .Returns<ClaimsPrincipal>(claimsPrincipal => isAuthenticated ?? claimsPrincipal.Identity?.IsAuthenticated ?? false);
        userInfoProviderMock.Setup(m => m.GetUserInfoAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(hasUserInfo ? userInfoModel ?? fixture.CreateUserInfoModel(random) : null));
    }

    #endregion
}