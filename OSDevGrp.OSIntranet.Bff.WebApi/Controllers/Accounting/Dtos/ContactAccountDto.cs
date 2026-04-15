using OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class ContactAccountDto : ContactAccountInfoDto
{
    internal static ContactAccountDto Map(ContactAccountModel contactAccountModel)
    {
        return new ContactAccountDto
        {
            Accounting = AccountingIdentificationDto.Map(contactAccountModel.Accounting),
            AccountNumber = contactAccountModel.AccountNumber,
            AccountName = contactAccountModel.AccountName
        };
    }
}