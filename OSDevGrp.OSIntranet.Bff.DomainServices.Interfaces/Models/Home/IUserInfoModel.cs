namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Models.Home;

public interface IUserInfoModel
{
    string? Name { get; }

    bool HasAccountingAccess { get; }

    int? DefaultAccountingNumber { get; }

    IReadOnlyDictionary<int, string> Accountings { get; }
}