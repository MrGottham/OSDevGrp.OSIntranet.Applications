using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.UserInfo;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.UserInfo;

internal class UserInfoModel : IUserInfoModel
{
    #region Constructor

    public UserInfoModel(string? nameIdentifier, string? name, string? mailAddress, bool hasAccountingAccess, int? defaultAccountingNumber, IReadOnlyDictionary<int, string> accountings, bool isAccountingAdministrator, bool isAccountingCreator, bool isAccountingModifier, IReadOnlyDictionary<int, string> modifiableAccountings, bool isAccountingViewer, IReadOnlyDictionary<int, string> viewableAccountings, bool hasCommonDataAccess)
    {
        NameIdentifier = nameIdentifier;
        Name = name;
        MailAddress = mailAddress;
        HasAccountingAccess = hasAccountingAccess;
        DefaultAccountingNumber = defaultAccountingNumber;
        Accountings = accountings;
        IsAccountingAdministrator = isAccountingAdministrator;
        IsAccountingCreator = isAccountingCreator;
        IsAccountingModifier = isAccountingModifier;
        ModifiableAccountings = modifiableAccountings;
        IsAccountingViewer = isAccountingViewer;
        ViewableAccountings = viewableAccountings;
        HasCommonDataAccess = hasCommonDataAccess;
    }

    #endregion

    #region Properties

    public string? NameIdentifier { get; }

    public string? Name { get; }

    public string? MailAddress { get; }

    public bool HasAccountingAccess { get; }

    public int? DefaultAccountingNumber { get; }

    public IReadOnlyDictionary<int, string> Accountings { get; }

    public bool IsAccountingAdministrator { get; }

    public bool IsAccountingCreator { get; }

    public bool IsAccountingModifier { get; }

    public IReadOnlyDictionary<int, string> ModifiableAccountings { get; }

    public bool IsAccountingViewer { get; }

    public IReadOnlyDictionary<int, string> ViewableAccountings { get; }

    public bool HasCommonDataAccess { get; }

    #endregion
}