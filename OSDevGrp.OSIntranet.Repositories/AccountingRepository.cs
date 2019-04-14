using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Contexts;
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

        public AccountingRepository(IConfiguration configuration)
            : base(configuration)
        {
        }

        #endregion

        #region Methods

        public Task<IEnumerable<IAccountGroup>> GetAccountGroupsAsync()
        {
            return Task.Run(() => GetAccountGroups());
        }

        public Task<IAccountGroup> GetAccountGroupAsync(int number)
        {
            return Task.Run(() => GetAccountGroup(number));
        }

        public Task<IEnumerable<IBudgetAccountGroup>> GetBudgetAccountGroupsAsync()
        {
            return Task.Run(() => GetBudgetAccountGroups());
        }

        public Task<IBudgetAccountGroup> GetBudgetAccountGroupAsync(int number)
        {
            return Task.Run(() => GetBudgetAccountGroup(number));
        }

        private IEnumerable<IAccountGroup> GetAccountGroups()
        {
            return Execute(() =>
                {
                    using (AccountingContext context = new AccountingContext(Configuration))
                    {
                        return context.AccountGroups.AsParallel()
                            .Select(accountGroupModel => _accountingModelConverter.Convert<AccountGroupModel, IAccountGroup>(accountGroupModel))
                            .OrderBy(accountGroup => accountGroup.Number)
                            .ToList();
                    }
                },
                MethodBase.GetCurrentMethod());
        }

        private IAccountGroup GetAccountGroup(int number)
        {
            return Execute(() =>
                {
                    using (AccountingContext context = new AccountingContext(Configuration))
                    {
                        AccountGroupModel accountGroupModel = context.AccountGroups.Find(number);
                        if (accountGroupModel == null)
                        {
                            return null;
                        }

                        return  _accountingModelConverter.Convert<AccountGroupModel, IAccountGroup>(accountGroupModel);
                    }
                },
                MethodBase.GetCurrentMethod());
        }

        private IEnumerable<IBudgetAccountGroup> GetBudgetAccountGroups()
        {
            return Execute(() =>
                {
                    using (AccountingContext context = new AccountingContext(Configuration))
                    {
                        return context.BudgetAccountGroups.AsParallel()
                            .Select(budgetAccountGroupModel => _accountingModelConverter.Convert<BudgetAccountGroupModel, IBudgetAccountGroup>(budgetAccountGroupModel))
                            .OrderBy(budgetAccountGroup => budgetAccountGroup.Number)
                            .ToList();
                    }
                },
                MethodBase.GetCurrentMethod());
        }

        private IBudgetAccountGroup GetBudgetAccountGroup(int number)
        {
            return Execute(() =>
                {
                    using (AccountingContext context = new AccountingContext(Configuration))
                    {
                        BudgetAccountGroupModel budgetAccountGroupModel = context.BudgetAccountGroups.Find(number);
                        if (budgetAccountGroupModel == null)
                        {
                            return null;
                        }

                        return  _accountingModelConverter.Convert<BudgetAccountGroupModel, IBudgetAccountGroup>(budgetAccountGroupModel);
                    }
                },
                MethodBase.GetCurrentMethod());
        }

        #endregion
    }
}