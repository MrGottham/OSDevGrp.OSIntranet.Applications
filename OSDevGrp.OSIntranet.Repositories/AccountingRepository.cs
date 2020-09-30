using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Converters;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Models.Accounting;

namespace OSDevGrp.OSIntranet.Repositories
{
    public class AccountingRepository : RepositoryBase, IAccountingRepository
    {
        #region Private variables

        private readonly IConverter _accountingModelConverter = new AccountingModelConverter();

        #endregion

        #region Constructor

        public AccountingRepository(IConfiguration configuration, IPrincipalResolver principalResolver, ILoggerFactory loggerFactory)
            : base(configuration, principalResolver, loggerFactory)
        {
        }

        #endregion

        #region Methods

        public Task<IEnumerable<IAccounting>> GetAccountingsAsync()
        {
            return ExecuteAsync(async () =>
                {
                    using AccountingModelHandler accountingModelHandler = new AccountingModelHandler(CreateRepositoryContext(), _accountingModelConverter, DateTime.Today, false, false);
                    return await accountingModelHandler.ReadAsync();
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<bool> AccountingExistsAsync(int number)
        {
            return ExecuteAsync(async () =>
                {
                    using AccountingModelHandler accountingModelHandler = new AccountingModelHandler(CreateRepositoryContext(), _accountingModelConverter, DateTime.Today, false, false);
                    return await accountingModelHandler.ExistsAsync(number);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IAccounting> GetAccountingAsync(int number, DateTime statusDate)
        {
            return ExecuteAsync(async () =>
                {
                    using AccountingModelHandler accountingModelHandler = new AccountingModelHandler(CreateRepositoryContext(), _accountingModelConverter, statusDate, true, true);
                    return await accountingModelHandler.ReadAsync(number);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IAccounting> CreateAccountingAsync(IAccounting accounting)
        {
            NullGuard.NotNull(accounting, nameof(accounting));

            return ExecuteAsync(async () =>
                {
                    using AccountingModelHandler accountingModelHandler = new AccountingModelHandler(CreateRepositoryContext(), _accountingModelConverter, DateTime.Today, false, false);
                    return await accountingModelHandler.CreateAsync(accounting);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IAccounting> UpdateAccountingAsync(IAccounting accounting)
        {
            NullGuard.NotNull(accounting, nameof(accounting));

            return ExecuteAsync(async () =>
                {
                    using AccountingModelHandler accountingModelHandler = new AccountingModelHandler(CreateRepositoryContext(), _accountingModelConverter, DateTime.Today, true, true);
                    return await accountingModelHandler.UpdateAsync(accounting);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IAccounting> DeleteAccountingAsync(int number)
        {
            return ExecuteAsync(async () =>
                {
                    using AccountingModelHandler accountingModelHandler = new AccountingModelHandler(CreateRepositoryContext(), _accountingModelConverter, DateTime.Today, true, true);
                    return await accountingModelHandler.DeleteAsync(number);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IAccountCollection> GetAccountsAsync(int accountingNumber, DateTime statusDate)
        {
            return ExecuteAsync(async () =>
                {
                    using AccountModelHandler accountModelHandler = new AccountModelHandler(CreateRepositoryContext(), _accountingModelConverter, statusDate, true, true);

                    IAccountCollection accountCollection = new AccountCollection();
                    accountCollection.Add(await accountModelHandler.ReadAsync(accountModel => accountModel.AccountingIdentifier == accountingNumber));

                    return accountCollection;
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<bool> AccountExistsAsync(int accountingNumber, string accountNumber)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            return ExecuteAsync(async () =>
                {
                    using AccountModelHandler accountModelHandler = new AccountModelHandler(CreateRepositoryContext(), _accountingModelConverter, DateTime.Today, false, false);
                    return await accountModelHandler.ExistsAsync(accountingNumber, accountNumber);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IAccount> GetAccountAsync(int accountingNumber, string accountNumber, DateTime statusDate)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            return ExecuteAsync(async () =>
                {
                    using AccountModelHandler accountModelHandler = new AccountModelHandler(CreateRepositoryContext(), _accountingModelConverter, statusDate, true, true);
                    return await accountModelHandler.ReadAsync(accountingNumber, accountNumber);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IAccount> CreateAccountAsync(IAccount account)
        {
            NullGuard.NotNull(account, nameof(account));

            return ExecuteAsync(async () =>
                {
                    using AccountModelHandler accountModelHandler = new AccountModelHandler(CreateRepositoryContext(), _accountingModelConverter, DateTime.Today, true, false);
                    return await accountModelHandler.CreateAsync(account);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IAccount> UpdateAccountAsync(IAccount account)
        {
            NullGuard.NotNull(account, nameof(account));

            return ExecuteAsync(async () =>
                {
                    using AccountModelHandler accountModelHandler = new AccountModelHandler(CreateRepositoryContext(), _accountingModelConverter, DateTime.Today, true, true);
                    return await accountModelHandler.UpdateAsync(account);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IAccount> DeleteAccountAsync(int accountingNumber, string accountNumber)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            return ExecuteAsync(async () =>
                {
                    using AccountModelHandler accountModelHandler = new AccountModelHandler(CreateRepositoryContext(), _accountingModelConverter, DateTime.Today, true, true);
                    return await accountModelHandler.DeleteAsync(new Tuple<int, string>(accountingNumber, accountNumber));
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IBudgetAccountCollection> GetBudgetAccountsAsync(int accountingNumber, DateTime statusDate)
        {
            return ExecuteAsync(async () =>
                {
                    using BudgetAccountModelHandler budgetAccountModelHandler = new BudgetAccountModelHandler(CreateRepositoryContext(), _accountingModelConverter, statusDate, true, true);

                    IBudgetAccountCollection budgetAccountCollection = new BudgetAccountCollection();
                    budgetAccountCollection.Add(await budgetAccountModelHandler.ReadAsync(budgetAccountModel => budgetAccountModel.AccountingIdentifier == accountingNumber));

                    return budgetAccountCollection;
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<bool> BudgetAccountExistsAsync(int accountingNumber, string accountNumber)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            return ExecuteAsync(async () =>
                {
                    using BudgetAccountModelHandler budgetAccountModelHandler = new BudgetAccountModelHandler(CreateRepositoryContext(), _accountingModelConverter, DateTime.Today, false, false);
                    return await budgetAccountModelHandler.ExistsAsync(accountingNumber, accountNumber);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IBudgetAccount> GetBudgetAccountAsync(int accountingNumber, string accountNumber, DateTime statusDate)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            return ExecuteAsync(async () =>
                {
                    using BudgetAccountModelHandler budgetAccountModelHandler = new BudgetAccountModelHandler(CreateRepositoryContext(), _accountingModelConverter, statusDate, true, true);
                    return await budgetAccountModelHandler.ReadAsync(accountingNumber, accountNumber);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IBudgetAccount> CreateBudgetAccountAsync(IBudgetAccount budgetAccount)
        {
            NullGuard.NotNull(budgetAccount, nameof(budgetAccount));

            return ExecuteAsync(async () =>
                {
                    using BudgetAccountModelHandler budgetAccountModelHandler = new BudgetAccountModelHandler(CreateRepositoryContext(), _accountingModelConverter, DateTime.Today, true, false);
                    return await budgetAccountModelHandler.CreateAsync(budgetAccount);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IBudgetAccount> UpdateBudgetAccountAsync(IBudgetAccount budgetAccount)
        {
            NullGuard.NotNull(budgetAccount, nameof(budgetAccount));

            return ExecuteAsync(async () =>
                {
                    using BudgetAccountModelHandler budgetAccountModelHandler = new BudgetAccountModelHandler(CreateRepositoryContext(), _accountingModelConverter, DateTime.Today, true, true);
                    return await budgetAccountModelHandler.UpdateAsync(budgetAccount);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IBudgetAccount> DeleteBudgetAccountAsync(int accountingNumber, string accountNumber)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            return ExecuteAsync(async () =>
                {
                    using BudgetAccountModelHandler budgetAccountModelHandler = new BudgetAccountModelHandler(CreateRepositoryContext(), _accountingModelConverter, DateTime.Today, true, true);
                    return await budgetAccountModelHandler.DeleteAsync(new Tuple<int, string>(accountingNumber, accountNumber));
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IContactAccountCollection> GetContactAccountsAsync(int accountingNumber, DateTime statusDate)
        {
            return ExecuteAsync(async () =>
                {
                    using ContactAccountModelHandler contactAccountModelHandler = new ContactAccountModelHandler(CreateRepositoryContext(), _accountingModelConverter, statusDate, true);

                    IContactAccountCollection contactAccountCollection = new ContactAccountCollection();
                    contactAccountCollection.Add(await contactAccountModelHandler.ReadAsync(contactAccountModel => contactAccountModel.AccountingIdentifier == accountingNumber));

                    return contactAccountCollection;
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<bool> ContactAccountExistsAsync(int accountingNumber, string accountNumber)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            return ExecuteAsync(async () =>
                {
                    using ContactAccountModelHandler contactAccountModelHandler = new ContactAccountModelHandler(CreateRepositoryContext(), _accountingModelConverter, DateTime.Today, false);
                    return await contactAccountModelHandler.ExistsAsync(accountingNumber, accountNumber);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IContactAccount> GetContactAccountAsync(int accountingNumber, string accountNumber, DateTime statusDate)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            return ExecuteAsync(async () =>
                {
                    using ContactAccountModelHandler contactAccountModelHandler = new ContactAccountModelHandler(CreateRepositoryContext(), _accountingModelConverter, statusDate, true);
                    return await contactAccountModelHandler.ReadAsync(accountingNumber, accountNumber);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IContactAccount> CreateContactAccountAsync(IContactAccount contactAccount)
        {
            NullGuard.NotNull(contactAccount, nameof(contactAccount));

            return ExecuteAsync(async () =>
                {
                    using ContactAccountModelHandler contactAccountModelHandler = new ContactAccountModelHandler(CreateRepositoryContext(), _accountingModelConverter, DateTime.Today, false);
                    return await contactAccountModelHandler.CreateAsync(contactAccount);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IContactAccount> UpdateContactAccountAsync(IContactAccount contactAccount)
        {
            NullGuard.NotNull(contactAccount, nameof(contactAccount));

            return ExecuteAsync(async () =>
                {
                    using ContactAccountModelHandler contactAccountModelHandler = new ContactAccountModelHandler(CreateRepositoryContext(), _accountingModelConverter, DateTime.Today, true);
                    return await contactAccountModelHandler.UpdateAsync(contactAccount);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IContactAccount> DeleteContactAccountAsync(int accountingNumber, string accountNumber)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            return ExecuteAsync(async () =>
                {
                    using ContactAccountModelHandler contactAccountModelHandler = new ContactAccountModelHandler(CreateRepositoryContext(), _accountingModelConverter, DateTime.Today, true);
                    return await contactAccountModelHandler.DeleteAsync(new Tuple<int, string>(accountingNumber, accountNumber));
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IEnumerable<IAccountGroup>> GetAccountGroupsAsync()
        {
            return ExecuteAsync(async () =>
                {
                    using AccountGroupModelHandler accountGroupModelHandler = new AccountGroupModelHandler(CreateRepositoryContext(), _accountingModelConverter);
                    return await accountGroupModelHandler.ReadAsync();
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IAccountGroup> GetAccountGroupAsync(int number)
        {
            return ExecuteAsync(async () =>
                {
                    using AccountGroupModelHandler accountGroupModelHandler = new AccountGroupModelHandler(CreateRepositoryContext(), _accountingModelConverter);
                    return await accountGroupModelHandler.ReadAsync(number);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IAccountGroup> CreateAccountGroupAsync(IAccountGroup accountGroup)
        {
            NullGuard.NotNull(accountGroup, nameof(accountGroup));

            return ExecuteAsync(async () =>
                {
                    using AccountGroupModelHandler accountGroupModelHandler = new AccountGroupModelHandler(CreateRepositoryContext(), _accountingModelConverter);
                    return await accountGroupModelHandler.CreateAsync(accountGroup);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IAccountGroup> UpdateAccountGroupAsync(IAccountGroup accountGroup)
        {
            NullGuard.NotNull(accountGroup, nameof(accountGroup));

            return ExecuteAsync(async () =>
                {
                    using AccountGroupModelHandler accountGroupModelHandler = new AccountGroupModelHandler(CreateRepositoryContext(), _accountingModelConverter);
                    return await accountGroupModelHandler.UpdateAsync(accountGroup);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IAccountGroup> DeleteAccountGroupAsync(int number)
        {
            return ExecuteAsync(async () =>
                {
                    using AccountGroupModelHandler accountGroupModelHandler = new AccountGroupModelHandler(CreateRepositoryContext(), _accountingModelConverter);
                    return await accountGroupModelHandler.DeleteAsync(number);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IEnumerable<IBudgetAccountGroup>> GetBudgetAccountGroupsAsync()
        {
            return ExecuteAsync(async () =>
                {
                    using BudgetAccountGroupModelHandler budgetAccountGroupModelHandler = new BudgetAccountGroupModelHandler(CreateRepositoryContext(), _accountingModelConverter);
                    return await budgetAccountGroupModelHandler.ReadAsync();
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IBudgetAccountGroup> GetBudgetAccountGroupAsync(int number)
        {
            return ExecuteAsync(async () =>
                {
                    using BudgetAccountGroupModelHandler budgetAccountGroupModelHandler = new BudgetAccountGroupModelHandler(CreateRepositoryContext(), _accountingModelConverter);
                    return await budgetAccountGroupModelHandler.ReadAsync(number);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IBudgetAccountGroup> CreateBudgetAccountGroupAsync(IBudgetAccountGroup budgetAccountGroup)
        {
            NullGuard.NotNull(budgetAccountGroup, nameof(budgetAccountGroup));

            return ExecuteAsync(async () =>
                {
                    using BudgetAccountGroupModelHandler budgetAccountGroupModelHandler = new BudgetAccountGroupModelHandler(CreateRepositoryContext(), _accountingModelConverter);
                    return await budgetAccountGroupModelHandler.CreateAsync(budgetAccountGroup);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IBudgetAccountGroup> UpdateBudgetAccountGroupAsync(IBudgetAccountGroup budgetAccountGroup)
        {
            NullGuard.NotNull(budgetAccountGroup, nameof(budgetAccountGroup));

            return ExecuteAsync(async () =>
                {
                    using BudgetAccountGroupModelHandler budgetAccountGroupModelHandler = new BudgetAccountGroupModelHandler(CreateRepositoryContext(), _accountingModelConverter);
                    return await budgetAccountGroupModelHandler.UpdateAsync(budgetAccountGroup);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IBudgetAccountGroup> DeleteBudgetAccountGroupAsync(int number)
        {
            return ExecuteAsync(async () =>
                {
                    using BudgetAccountGroupModelHandler budgetAccountGroupModelHandler = new BudgetAccountGroupModelHandler(CreateRepositoryContext(), _accountingModelConverter);
                    return await budgetAccountGroupModelHandler.DeleteAsync(number);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IEnumerable<IPaymentTerm>> GetPaymentTermsAsync()
        {
            return ExecuteAsync(async () =>
                {
                    using PaymentTermModelHandler paymentTermModelHandler = new PaymentTermModelHandler(CreateRepositoryContext(), _accountingModelConverter);
                    return await paymentTermModelHandler.ReadAsync();
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IPaymentTerm> GetPaymentTermAsync(int number)
        {
            return ExecuteAsync(async () =>
                {
                    using PaymentTermModelHandler paymentTermModelHandler = new PaymentTermModelHandler(CreateRepositoryContext(), _accountingModelConverter);
                    return await paymentTermModelHandler.ReadAsync(number);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IPaymentTerm> CreatePaymentTermAsync(IPaymentTerm paymentTerm)
        {
            NullGuard.NotNull(paymentTerm, nameof(paymentTerm));

            return ExecuteAsync(async () =>
                {
                    using PaymentTermModelHandler paymentTermModelHandler = new PaymentTermModelHandler(CreateRepositoryContext(), _accountingModelConverter);
                    return await paymentTermModelHandler.CreateAsync(paymentTerm);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IPaymentTerm> UpdatePaymentTermAsync(IPaymentTerm paymentTerm)
        {
            NullGuard.NotNull(paymentTerm, nameof(paymentTerm));

            return ExecuteAsync(async () =>
                {
                    using PaymentTermModelHandler paymentTermModelHandler = new PaymentTermModelHandler(CreateRepositoryContext(), _accountingModelConverter);
                    return await paymentTermModelHandler.UpdateAsync(paymentTerm);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IPaymentTerm> DeletePaymentTermAsync(int number)
        {
            return ExecuteAsync(async () =>
                {
                    using PaymentTermModelHandler paymentTermModelHandler = new PaymentTermModelHandler(CreateRepositoryContext(), _accountingModelConverter);
                    return await paymentTermModelHandler.DeleteAsync(number);
                },
                MethodBase.GetCurrentMethod());
        }

        #endregion
    }
}