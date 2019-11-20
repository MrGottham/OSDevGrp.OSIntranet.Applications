using System;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic
{
    internal static class AccountingIdentifierHelper
    {
        #region Methods

        internal static IValidator ValidateAccountingIdentifier(this IValidator validator, int value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            return validator.Integer.ShouldBeBetween(value, 1, 99, validatingType, validatingField);
        }

        internal static IAccounting GetAccounting(this int accountingNumber, DateTime statusDate, IAccountingRepository accountingRepository, ref IAccounting accounting)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            return accounting ?? (accounting = accountingRepository.GetAccountingAsync(accountingNumber, statusDate).GetAwaiter().GetResult());
        }

        #endregion
    }
}