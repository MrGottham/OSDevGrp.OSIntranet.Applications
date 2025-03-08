using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Models.Home;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Models.Home.UserInfoModel;

internal static class FixtureExtensions
{
    #region Methods

    internal static IUserInfoModel CreateUserInfoModel(this Fixture fixture, Random random, bool hasName = true, string? name = null, bool hasAccountingAccess = true, bool hasDefaultAccountingNumber = true, int? defaultAccountingNumber = null, IReadOnlyDictionary<int, string>? accountings = null)
    {
        return fixture.CreateUserInfoModelMock(random, hasName, name, hasAccountingAccess, hasDefaultAccountingNumber, defaultAccountingNumber, accountings).Object;
    }

    internal static Mock<IUserInfoModel> CreateUserInfoModelMock(this Fixture fixture, Random random, bool hasName = true, string? name = null, bool hasAccountingAccess = true, bool hasDefaultAccountingNumber = true, int? defaultAccountingNumber = null, IReadOnlyDictionary<int, string>? accountings = null)
    {
        Mock<IUserInfoModel> userInfoModelMock =  new Mock<IUserInfoModel>();
        userInfoModelMock.Setup(m => m.Name)
            .Returns(hasName ? name ?? fixture.Create<string>() : null);
        userInfoModelMock.Setup(m => m.HasAccountingAccess)
            .Returns(hasAccountingAccess);
        userInfoModelMock.Setup(m => m.HasAccountingAccess)
            .Returns(hasAccountingAccess);
        userInfoModelMock.Setup(m => m.DefaultAccountingNumber)
            .Returns(hasAccountingAccess && hasDefaultAccountingNumber ? defaultAccountingNumber ?? fixture.Create<int>() : null);
        userInfoModelMock.Setup(m => m.Accountings)
            .Returns(hasAccountingAccess ? accountings ?? fixture.CreateMany<int>(random.Next(5, 10)).ToDictionary(value => value, _ => fixture.Create<string>()).AsReadOnly() : new Dictionary<int, string>().AsReadOnly());
        return userInfoModelMock;
    }

    #endregion
}