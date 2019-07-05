using System;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic
{
    internal static class AccountGroupIdentifierHelper
    {
        #region Methods

        internal static IValidator ValidateAccountGroupIdentifier(this IValidator validator, int value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            return validator.Integer.ShouldBeBetween(value, 1, 99, validatingType, validatingField);
        }

        internal static IAccountGroup GetAccountGroup(this int number, IAccountingRepository accountingRepository, ref IAccountGroup accountGroup)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            return accountGroup ?? (accountGroup = accountingRepository.GetAccountGroupAsync(number).GetAwaiter().GetResult());
        }

        internal static IBudgetAccountGroup GetBudgetAccountGroup(this int number, IAccountingRepository accountingRepository, ref IBudgetAccountGroup budgetAccountGroup)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            return budgetAccountGroup ?? (budgetAccountGroup = accountingRepository.GetBudgetAccountGroupAsync(number).GetAwaiter().GetResult());
        }

        #endregion
    }
}