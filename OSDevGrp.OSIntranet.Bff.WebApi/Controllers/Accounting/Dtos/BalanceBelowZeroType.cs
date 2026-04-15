using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public enum BalanceBelowZeroType
{
    Debtors = AccountingRuleSetSpecifications.BalanceBelowZeroDebtorsValue,
    Creditors = AccountingRuleSetSpecifications.BalanceBelowZeroCreditorsValue
}