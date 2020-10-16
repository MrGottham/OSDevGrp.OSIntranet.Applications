using System;
using System.Text.RegularExpressions;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic
{
    internal static class AccountIdentificationHelper
    {
        #region Private variables

        private static readonly Regex AccountNumberRegex = new Regex(@"[0-9A-Z\-+]{1,16}", RegexOptions.Compiled);

        #endregion

        #region Methods

        internal static IValidator ValidateAccountIdentifier(this IValidator validator, string value, Type validatingType, string validatingField, bool allowNull = false)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            return validator.String.ShouldNotBeNullOrWhiteSpace(value, validatingType, validatingField)
                .String.ShouldHaveMinLength(value, 1, validatingType, validatingField, allowNull)
                .String.ShouldHaveMaxLength(value, 16, validatingType, validatingField, allowNull)
                .String.ShouldMatchPattern(value, AccountNumberRegex, validatingType, validatingField, allowNull);
        }

        internal static IAccount GetAccount(this string accountNumber, int accountingNumber, DateTime statusDate, IAccountingRepository accountingRepository, ref IAccount account)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber))
                .NotNull(accountingRepository, nameof(accountingRepository));

            return account ??= accountingRepository.GetAccountAsync(accountingNumber, accountNumber, statusDate).GetAwaiter().GetResult();
        }

        internal static IBudgetAccount GetBudgetAccount(this string accountNumber, int accountingNumber, DateTime statusDate, IAccountingRepository accountingRepository, ref IBudgetAccount budgetAccount)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber))
                .NotNull(accountingRepository, nameof(accountingRepository));

            return budgetAccount ??= accountingRepository.GetBudgetAccountAsync(accountingNumber, accountNumber, statusDate).GetAwaiter().GetResult();
        }

        internal static IContactAccount GetContactAccount(this string accountNumber, int accountingNumber, DateTime statusDate, IAccountingRepository accountingRepository, ref IContactAccount contactAccount)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber))
                .NotNull(accountingRepository, nameof(accountingRepository));

            return contactAccount ??= accountingRepository.GetContactAccountAsync(accountingNumber, accountNumber, statusDate).GetAwaiter().GetResult();
        }

        #endregion
    }
}