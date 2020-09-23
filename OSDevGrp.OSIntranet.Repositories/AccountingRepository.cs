using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
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
                    using AccountingModelHandler accountingModelHandler = new AccountingModelHandler(CreateRepositoryContext(), _accountingModelConverter, DateTime.Today, false, false);
                    return await accountingModelHandler.UpdateAsync(accounting);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IAccounting> DeleteAccountingAsync(int number)
        {
            return ExecuteAsync(async () =>
                {
                    using AccountingModelHandler accountingModelHandler = new AccountingModelHandler(CreateRepositoryContext(), _accountingModelConverter, DateTime.Today, false, false);
                    return await accountingModelHandler.DeleteAsync(number);
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