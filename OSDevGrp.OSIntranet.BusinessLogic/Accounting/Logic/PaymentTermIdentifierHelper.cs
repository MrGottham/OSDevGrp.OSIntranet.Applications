using System;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic
{
    internal static class PaymentTermIdentifierHelper
    {
        #region Methods

        internal static IValidator ValidatePaymentTermIdentifier(this IValidator validator, int value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            return validator.Integer.ShouldBeBetween(value, 1, 99, validatingType, validatingField);
        }

        internal static IPaymentTerm GetPaymentTerm(this int number, IAccountingRepository accountingRepository, ref IPaymentTerm paymentTerm)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            return paymentTerm ??= accountingRepository.GetPaymentTermAsync(number).GetAwaiter().GetResult();
        }

        #endregion
    }
}
