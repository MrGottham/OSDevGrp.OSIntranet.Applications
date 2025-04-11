using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Models.Home;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Models.Home;

internal class UserInfoModel : IUserInfoModel
{
    #region Constructor

    public UserInfoModel(string? name, bool hasAccountingAccess, int? defaultAccountingNumber, IReadOnlyDictionary<int, string> accountings)
    {
        Name = name;
        HasAccountingAccess = hasAccountingAccess;
        DefaultAccountingNumber = defaultAccountingNumber;
        Accountings = accountings;
    }

    #endregion

    #region Properties

    public string? Name { get; }

    public bool HasAccountingAccess { get; }

    public int? DefaultAccountingNumber { get; }

    public IReadOnlyDictionary<int, string> Accountings { get; }

    #endregion
}