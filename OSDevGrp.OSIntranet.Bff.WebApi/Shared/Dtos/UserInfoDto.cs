using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.UserInfo;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;

public class UserInfoDto
{
    [MinLength(ValidationValues.NameIdentifierMinLength)]
    public string? NameIdentifier { get; init; }

    [MinLength(ValidationValues.NameMinLength)]
    public string? Name { get; init; }

    [MinLength(ValidationValues.MailAddressMinLength)]
    public string? MailAddress { get; init; }

    [Required]
    public required bool HasAccountingAccess { get; init; }

    [Range(ValidationValues.AccountingIdentificationMinValue, ValidationValues.AccountingIdentificationMaxValue)]
    public int? DefaultAccountingNumber { get; init; }

    [Required]
    public required IReadOnlyCollection<AccountingInfoDto> Accountings { get; init; } = [];

    [Required]
    public required bool IsAccountingAdministrator { get; init; }

    [Required]
    public required bool IsAccountingCreator { get; init; }

    [Required]
    public required bool IsAccountingModifier { get; init; }

    [Required]
    public required IReadOnlyCollection<AccountingInfoDto> ModifiableAccountings { get; init; } = [];

    [Required]
    public required bool IsAccountingViewer { get; init; }

    [Required]
    public required IReadOnlyCollection<AccountingInfoDto> ViewableAccountings { get; init; } = [];

    [Required]
    public required bool HasCommonDataAccess { get; init; }

    internal static UserInfoDto Map(IUserInfoModel userInfoModel)
    {
        return new UserInfoDto
        {
            NameIdentifier = userInfoModel.NameIdentifier,
            Name = userInfoModel.Name,
            MailAddress = userInfoModel.MailAddress,
            HasAccountingAccess = userInfoModel.HasAccountingAccess,
            DefaultAccountingNumber = userInfoModel.DefaultAccountingNumber,
            Accountings = userInfoModel.Accountings.Select(AccountingInfoDto.Map).ToArray(),
            IsAccountingAdministrator = userInfoModel.IsAccountingAdministrator,
            IsAccountingCreator = userInfoModel.IsAccountingCreator,
            IsAccountingModifier = userInfoModel.IsAccountingModifier,
            ModifiableAccountings = userInfoModel.ModifiableAccountings.Select(AccountingInfoDto.Map).ToArray(),
            IsAccountingViewer = userInfoModel.IsAccountingViewer,
            ViewableAccountings = userInfoModel.ViewableAccountings.Select(AccountingInfoDto.Map).ToArray(),
            HasCommonDataAccess = userInfoModel.HasCommonDataAccess
        };
    }
}