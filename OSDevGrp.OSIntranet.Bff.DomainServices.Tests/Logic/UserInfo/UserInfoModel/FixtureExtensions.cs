using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.UserInfo;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.UserInfo.UserInfoModel;

internal static class FixtureExtensions
{
    #region Methods

    internal static IUserInfoModel CreateUserInfoModel(this Fixture fixture, Random random, bool hasNameIdentifier = true, string? nameIdentifier = null, bool hasName = true, string? name = null, bool hasMailAddress = true, string? mailAddress = null, bool hasAccountingAccess = true, bool hasDefaultAccountingNumber = true, int? defaultAccountingNumber = null, IReadOnlyDictionary<int, string>? accountings = null, bool isAccountingAdministrator = true, bool isAccountingCreator = true, bool isAccountingModifier = true, IReadOnlyDictionary<int, string>? modifiableAccountings = null, bool isAccountingViewer = true, IReadOnlyDictionary<int, string>? viewableAccountings = null, bool hasCommonDataAccess = true)
    {
        return fixture.CreateUserInfoModelMock(random, hasNameIdentifier, nameIdentifier, hasName, name, hasMailAddress, mailAddress, hasAccountingAccess, hasDefaultAccountingNumber, defaultAccountingNumber, accountings, isAccountingAdministrator, isAccountingCreator, isAccountingModifier, modifiableAccountings, isAccountingViewer, viewableAccountings, hasCommonDataAccess).Object;
    }

    internal static Mock<IUserInfoModel> CreateUserInfoModelMock(this Fixture fixture, Random random, bool hasNameIdentifier = true, string? nameIdentifier = null, bool hasName = true, string? name = null, bool hasMailAddress = true, string? mailAddress = null, bool hasAccountingAccess = true, bool hasDefaultAccountingNumber = true, int? defaultAccountingNumber = null, IReadOnlyDictionary<int, string>? accountings = null, bool isAccountingAdministrator = true, bool isAccountingCreator = true, bool isAccountingModifier = true, IReadOnlyDictionary<int, string>? modifiableAccountings = null, bool isAccountingViewer = true, IReadOnlyDictionary<int, string>? viewableAccountings = null, bool hasCommonDataAccess = true)
    {
        Mock<IUserInfoModel> userInfoModelMock =  new Mock<IUserInfoModel>();
        userInfoModelMock.Setup(m => m.NameIdentifier)
            .Returns(hasNameIdentifier ? nameIdentifier ?? fixture.Create<string>() : null);
        userInfoModelMock.Setup(m => m.Name)
            .Returns(hasName ? name ?? fixture.Create<string>() : null);
        userInfoModelMock.Setup(m => m.MailAddress)
            .Returns(hasMailAddress ? mailAddress ?? $"{fixture.Create<string>()}@{fixture.Create<string>()}.local" : null);
        userInfoModelMock.Setup(m => m.HasAccountingAccess)
            .Returns(hasAccountingAccess);
        userInfoModelMock.Setup(m => m.DefaultAccountingNumber)
            .Returns(hasAccountingAccess && hasDefaultAccountingNumber ? defaultAccountingNumber ?? fixture.Create<int>() : null);
        userInfoModelMock.Setup(m => m.Accountings)
            .Returns(hasAccountingAccess ? accountings ?? fixture.CreateMany<int>(random.Next(5, 10)).ToDictionary(value => value, _ => fixture.Create<string>()).AsReadOnly() : new Dictionary<int, string>().AsReadOnly());
        userInfoModelMock.Setup(m => m.IsAccountingAdministrator)
            .Returns(hasAccountingAccess && isAccountingAdministrator);
        userInfoModelMock.Setup(m => m.IsAccountingCreator)
            .Returns(hasAccountingAccess && isAccountingCreator);
        userInfoModelMock.Setup(m => m.IsAccountingModifier)
            .Returns(hasAccountingAccess && isAccountingModifier);
        userInfoModelMock.Setup(m => m.ModifiableAccountings)
            .Returns(hasAccountingAccess && isAccountingModifier ? modifiableAccountings ?? fixture.CreateMany<int>(random.Next(5, 10)).ToDictionary(value => value, _ => fixture.Create<string>()).AsReadOnly() : new Dictionary<int, string>().AsReadOnly());
        userInfoModelMock.Setup(m => m.IsAccountingViewer)
            .Returns(hasAccountingAccess && isAccountingViewer);
        userInfoModelMock.Setup(m => m.ViewableAccountings)
            .Returns(hasAccountingAccess && isAccountingViewer ? viewableAccountings ?? fixture.CreateMany<int>(random.Next(5, 10)).ToDictionary(value => value, _ => fixture.Create<string>()).AsReadOnly() : new Dictionary<int, string>().AsReadOnly());
        userInfoModelMock.Setup(m => m.HasCommonDataAccess)
            .Returns(hasCommonDataAccess);
        return userInfoModelMock;
    }

    #endregion
}