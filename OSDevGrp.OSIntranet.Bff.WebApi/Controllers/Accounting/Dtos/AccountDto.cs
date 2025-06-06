using OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class AccountDto : AccountInfoDto
{
    internal static AccountDto Map(AccountModel accountModel)
    {
        return new AccountDto
        {
            Accounting = AccountingIdentificationDto.Map(accountModel.Accounting),
            AccountNumber = accountModel.AccountNumber,
            AccountName = accountModel.AccountName
        };
    }
}