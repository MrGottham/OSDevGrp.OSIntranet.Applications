using System;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Commands
{
    public abstract class AccountIdentificationCommandBase : AccountingIdentificationCommandBase, IAccountIdentificationCommand
    {
        #region Private variables

        private string _accountNumber;
        private IAccount _account;
        private IBudgetAccount _budgetAccount;
        private IContactAccount _contactAccount;

        #endregion

        #region Properties

        public string AccountNumber
        {
            get => _accountNumber;
            set
            {
                NullGuard.NotNullOrWhiteSpace(value, nameof(value));

                _accountNumber = value.Trim().ToUpper();
            }
        }

        #endregion

        #region Methods

        public override IValidator Validate(IValidator validator, IAccountingRepository accountingRepository, ICommonRepository commonRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(accountingRepository, nameof(accountingRepository))
                .NotNull(commonRepository, nameof(commonRepository));

            return base.Validate(validator, accountingRepository, commonRepository)
                .Object.ShouldBeKnownValue(AccountingNumber, accountingNumber => AccountingExistsAsync(accountingRepository), GetType(), nameof(AccountingNumber))
                .ValidateAccountIdentifier(AccountNumber, GetType(), nameof(AccountNumber));
        }

        protected Task<bool> AccountExistsAsync(IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            return AccountExistsAsync(accountingRepository, AccountNumber);
        }

        protected Task<bool> AccountExistsAsync(IAccountingRepository accountingRepository, string accountNumber)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository))
                .NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            return accountingRepository.AccountExistsAsync(AccountingNumber, accountNumber);
        }

        protected Task<IAccount> GetAccountAsync(IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            return GetAccountAsync(accountingRepository, AccountNumber);
        }

        protected Task<IAccount> GetAccountAsync(IAccountingRepository accountingRepository, string accountNumber)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository))
                .NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            return Task.FromResult(accountNumber.GetAccount(AccountingNumber, DateTime.Today, accountingRepository, ref _account));
        }

        protected Task<bool> BudgetAccountExistsAsync(IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            return BudgetAccountExistsAsync(accountingRepository, AccountNumber);
        }

        protected Task<bool> BudgetAccountExistsAsync(IAccountingRepository accountingRepository, string accountNumber)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository))
                .NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            return accountingRepository.BudgetAccountExistsAsync(AccountingNumber, accountNumber);
        }

        protected Task<IBudgetAccount> GetBudgetAccountAsync(IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            return GetBudgetAccountAsync(accountingRepository, AccountNumber);
        }

        protected Task<IBudgetAccount> GetBudgetAccountAsync(IAccountingRepository accountingRepository, string accountNumber)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository))
                .NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            return Task.FromResult(accountNumber.GetBudgetAccount(AccountingNumber, DateTime.Today, accountingRepository, ref _budgetAccount));
        }

        protected Task<bool> ContactAccountExistsAsync(IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            return ContactAccountExistsAsync(accountingRepository, AccountNumber);
        }

        protected Task<bool> ContactAccountExistsAsync(IAccountingRepository accountingRepository, string accountNumber)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository))
                .NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            return accountingRepository.ContactAccountExistsAsync(AccountingNumber, accountNumber);
        }

        protected Task<IContactAccount> GetContactAccountAsync(IAccountingRepository accountingRepository)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository));

            return GetContactAccountAsync(accountingRepository, AccountNumber);
        }

        protected Task<IContactAccount> GetContactAccountAsync(IAccountingRepository accountingRepository, string accountNumber)
        {
            NullGuard.NotNull(accountingRepository, nameof(accountingRepository))
                .NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            return Task.FromResult(accountNumber.GetContactAccount(AccountingNumber, DateTime.Today, accountingRepository, ref _contactAccount));
        }

        #endregion
    }
}