﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
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

        public AccountingRepository(IConfiguration configuration, IPrincipalResolver principalResolver)
            : base(configuration, principalResolver)
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

        public Task<IAccountGroup> CreateAccountGroupAsync(IAccountGroup accountGroup)
        {
            NullGuard.NotNull(accountGroup, nameof(accountGroup));

            return Task.Run(() => CreateAccountGroup(accountGroup));
        }

        public Task<IAccountGroup> UpdateAccountGroupAsync(IAccountGroup accountGroup)
        {
            NullGuard.NotNull(accountGroup, nameof(accountGroup));

            return Task.Run(() => UpdateAccountGroup(accountGroup));
        }

        public Task<IEnumerable<IBudgetAccountGroup>> GetBudgetAccountGroupsAsync()
        {
            return Task.Run(() => GetBudgetAccountGroups());
        }

        public Task<IBudgetAccountGroup> GetBudgetAccountGroupAsync(int number)
        {
            return Task.Run(() => GetBudgetAccountGroup(number));
        }

        public Task<IBudgetAccountGroup> CreateBudgetAccountGroupAsync(IBudgetAccountGroup budgetAccountGroup)
        {
            NullGuard.NotNull(budgetAccountGroup, nameof(budgetAccountGroup));

            return Task.Run(() => CreateBudgetAccountGroup(budgetAccountGroup));
        }

        public Task<IBudgetAccountGroup> UpdateBudgetAccountGroupAsync(IBudgetAccountGroup budgetAccountGroup)
        {
            NullGuard.NotNull(budgetAccountGroup, nameof(budgetAccountGroup));

            return Task.Run(() => UpdateBudgetAccountGroup(budgetAccountGroup));
        }

        private IEnumerable<IAccountGroup> GetAccountGroups()
        {
            return Execute(() =>
                {
                    using (AccountingContext context = new AccountingContext(Configuration, PrincipalResolver))
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
                    using (AccountingContext context = new AccountingContext(Configuration, PrincipalResolver))
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

        private IAccountGroup CreateAccountGroup(IAccountGroup accountGroup)
        {
            NullGuard.NotNull(accountGroup, nameof(accountGroup));

            return Execute(() =>
                {
                    using (AccountingContext context = new AccountingContext(Configuration, PrincipalResolver))
                    {
                        AccountGroupModel accountGroupModel = _accountingModelConverter.Convert<IAccountGroup, AccountGroupModel>(accountGroup);

                        context.AccountGroups.Add(accountGroupModel);

                        context.SaveChanges();

                        return GetAccountGroup(accountGroup.Number);
                    }
                },
                MethodBase.GetCurrentMethod());
        }

        private IAccountGroup UpdateAccountGroup(IAccountGroup accountGroup)
        {
            NullGuard.NotNull(accountGroup, nameof(accountGroup));

            return Execute(() =>
                {
                    using (AccountingContext context = new AccountingContext(Configuration, PrincipalResolver))
                    {
                        AccountGroupModel accountGroupModel = context.AccountGroups.Find(accountGroup.Number);
                        if (accountGroupModel == null)
                        {
                            return null;
                        }

                        accountGroupModel.Name = accountGroup.Name;
                        accountGroupModel.AccountGroupType = accountGroup.AccountGroupType;

                        context.SaveChanges();

                        return GetAccountGroup(accountGroup.Number);
                    }
                },
                MethodBase.GetCurrentMethod());
        }

        private IEnumerable<IBudgetAccountGroup> GetBudgetAccountGroups()
        {
            return Execute(() =>
                {
                    using (AccountingContext context = new AccountingContext(Configuration, PrincipalResolver))
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
                    using (AccountingContext context = new AccountingContext(Configuration, PrincipalResolver))
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

        private IBudgetAccountGroup CreateBudgetAccountGroup(IBudgetAccountGroup budgetAccountGroup)
        {
            NullGuard.NotNull(budgetAccountGroup, nameof(budgetAccountGroup));

            return Execute(() =>
                {
                    using (AccountingContext context = new AccountingContext(Configuration, PrincipalResolver))
                    {
                        BudgetAccountGroupModel budgetAccountGroupModel = _accountingModelConverter.Convert<IBudgetAccountGroup, BudgetAccountGroupModel>(budgetAccountGroup);

                        context.BudgetAccountGroups.Add(budgetAccountGroupModel);

                        context.SaveChanges();

                        return GetBudgetAccountGroup(budgetAccountGroup.Number);
                    }
                },
                MethodBase.GetCurrentMethod());
        }

        private IBudgetAccountGroup UpdateBudgetAccountGroup(IBudgetAccountGroup budgetAccountGroup)
        {
            NullGuard.NotNull(budgetAccountGroup, nameof(budgetAccountGroup));

            return Execute(() =>
                {
                    using (AccountingContext context = new AccountingContext(Configuration, PrincipalResolver))
                    {
                        BudgetAccountGroupModel budgetAccountGroupModel = context.BudgetAccountGroups.Find(budgetAccountGroup.Number);
                        if (budgetAccountGroupModel == null)
                        {
                            return null;
                        }

                        budgetAccountGroupModel.Name = budgetAccountGroup.Name;;

                        context.SaveChanges();

                        return GetBudgetAccountGroup(budgetAccountGroup.Number);
                    }
                },
                MethodBase.GetCurrentMethod());
        }

        #endregion
    }
}