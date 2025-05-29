using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.UserInfo;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Shared.Dtos;

internal static class FixtureExtensions
{
    #region Methods

    internal static IUserInfoModel CreateUserInfoModel(this Fixture fixture, Random? random = null, bool hasNameIdentifier = true, bool hasName = true, bool hasMailAddress = true, bool hasAccountingAccess = true, bool hasDefaultAccountingNumber = true, bool isAccountingAdministrator = true, bool isAccountingCreator = true, bool isAccountingModifier = true, bool isAccountingViewer = true, bool hasCommonDataAccess = true)
    {
        return fixture.CreateUserInfoModelMock(random, hasNameIdentifier, hasName, hasMailAddress, hasAccountingAccess, hasDefaultAccountingNumber, isAccountingAdministrator, isAccountingCreator, isAccountingModifier, isAccountingViewer, hasCommonDataAccess).Object;
    }

    internal static Mock<IUserInfoModel> CreateUserInfoModelMock(this Fixture fixture, Random? random = null, bool hasNameIdentifier = true, bool hasName = true, bool hasMailAddress = true, bool hasAccountingAccess = true, bool hasDefaultAccountingNumber = true, bool isAccountingAdministrator = true, bool isAccountingCreator = true, bool isAccountingModifier = true, bool isAccountingViewer = true, bool hasCommonDataAccess = true)
    {
        Mock<IUserInfoModel> userInfoModelMock = new Mock<IUserInfoModel>();
        userInfoModelMock.Setup(m => m.NameIdentifier)
            .Returns(hasNameIdentifier ? fixture.Create<string>() : null);
        userInfoModelMock.Setup(m => m.Name)
            .Returns(hasName ? fixture.Create<string>() : null);
        userInfoModelMock.Setup(m => m.MailAddress)
            .Returns(hasMailAddress ? $"{fixture.Create<string>()}@{fixture.Create<string>()}.local" : null);
        userInfoModelMock.Setup(m => m.HasAccountingAccess)
            .Returns(hasAccountingAccess);
        userInfoModelMock.Setup(m => m.DefaultAccountingNumber)
            .Returns(hasAccountingAccess ? hasDefaultAccountingNumber ? fixture.Create<int>() : null : null);
        userInfoModelMock.Setup(m => m.Accountings)
            .Returns(hasAccountingAccess ? fixture.CreateAccontingDictionary(random) : new Dictionary<int, string>());
        userInfoModelMock.Setup(m => m.IsAccountingAdministrator)
            .Returns(hasAccountingAccess && isAccountingAdministrator);
        userInfoModelMock.Setup(m => m.IsAccountingCreator)
            .Returns(hasAccountingAccess && isAccountingCreator);
        userInfoModelMock.Setup(m => m.IsAccountingModifier)
            .Returns(hasAccountingAccess && isAccountingModifier);
        userInfoModelMock.Setup(m => m.ModifiableAccountings)
            .Returns(hasAccountingAccess && isAccountingModifier ? fixture.CreateAccontingDictionary(random) : new Dictionary<int, string>());
        userInfoModelMock.Setup(m => m.IsAccountingViewer)
            .Returns(hasAccountingAccess && isAccountingViewer);
        userInfoModelMock.Setup(m => m.ViewableAccountings)
            .Returns(hasAccountingAccess && isAccountingModifier ? fixture.CreateAccontingDictionary(random) : new Dictionary<int, string>());
        return userInfoModelMock;
    }

    internal static IReadOnlyDictionary<int, string> CreateAccontingDictionary(this Fixture fixture, Random? random = null)
    {
        return fixture.CreateMany<int>(random?.Next(1, 5) ?? 3)
            .Distinct()
            .ToDictionary(accountingNumber => accountingNumber, accountingNumber => fixture.Create<string>());
    }

    internal static IReadOnlyDictionary<StaticTextKey, string> CreateStaticTexts(this Fixture fixture, Random? random = null)
    {
        return fixture.CreateMany<StaticTextKey>(random?.Next(1, 5) ?? 3)
            .Distinct()
            .ToDictionary(staticTextKey => staticTextKey, staticTextKey => fixture.Create<string>());
    }

    #endregion
}