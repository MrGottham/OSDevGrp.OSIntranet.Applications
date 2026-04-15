using OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class BudgetAccountDto : BudgetAccountInfoDto
{
    internal static BudgetAccountDto Map(BudgetAccountModel budgetAccountModel)
    {
        return new BudgetAccountDto
        {
            Accounting = AccountingIdentificationDto.Map(budgetAccountModel.Accounting),
            AccountNumber = budgetAccountModel.AccountNumber,
            AccountName = budgetAccountModel.AccountName
        };
    }
}