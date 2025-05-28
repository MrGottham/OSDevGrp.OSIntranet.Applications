namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.UserInfo;

public interface IUserInfoModel
{
    string? NameIdentifier { get; }

    string? Name { get; }

    string? MailAddress { get; }

    bool HasAccountingAccess { get; }

    int? DefaultAccountingNumber { get; }

    IReadOnlyDictionary<int, string> Accountings { get; }

    bool IsAccountingAdministrator { get; }

    bool IsAccountingCreator { get; }

    bool IsAccountingModifier { get; }

    IReadOnlyDictionary<int, string> ModifiableAccountings { get; }

    bool IsAccountingViewer { get; }

    IReadOnlyDictionary<int, string> ViewableAccountings { get; }

    bool HasCommonDataAccess { get; }
}