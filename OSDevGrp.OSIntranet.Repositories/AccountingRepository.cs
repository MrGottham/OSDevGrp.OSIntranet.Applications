using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Converters;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Models.Accounting;

namespace OSDevGrp.OSIntranet.Repositories
{
    public class AccountingRepository : IAccountingRepository
    {
        #region Private variables

        private readonly IConfiguration _configuration;
        private readonly IConverter _accountingModelConverter = new AccountingModelConverter();

        #endregion

        #region Constructor

        public AccountingRepository(IConfiguration configuration)
        {
            NullGuard.NotNull(configuration, nameof(configuration));

            _configuration = configuration;
        }

        #endregion

        #region Methods

        public Task<IEnumerable<IAccountGroup>> GetAccountGroupsAsync()
        {
            return Task.Run(() => GetAccountGroups());
        }

        public Task<IEnumerable<IBudgetAccountGroup>> GetBudgetAccountGroupsAsync()
        {
            return Task.Run(() => GetBudgetAccountGroups());
        }

        private IEnumerable<IAccountGroup> GetAccountGroups()
        {
            return Execute(() =>
                {
                    using (AccountingContext context = new AccountingContext(_configuration))
                    {
                        return context.AccountGroups.AsParallel()
                            .Select(accountGroupModel => _accountingModelConverter.Convert<AccountGroupModel, IAccountGroup>(accountGroupModel))
                            .OrderBy(accountGroup => accountGroup.Number)
                            .ToList();
                    }
                },
                MethodBase.GetCurrentMethod());
        }

        private IEnumerable<IBudgetAccountGroup> GetBudgetAccountGroups()
        {
            return Execute(() =>
                {
                    using (AccountingContext context = new AccountingContext(_configuration))
                    {
                        return context.BudgetAccountGroups.AsParallel()
                            .Select(budgetAccountGroupModel => _accountingModelConverter.Convert<BudgetAccountGroupModel, IBudgetAccountGroup>(budgetAccountGroupModel))
                            .OrderBy(budgetAccountGroup => budgetAccountGroup.Number)
                            .ToList();
                    }
                },
                MethodBase.GetCurrentMethod());
        }

        private T Execute<T>(Func<T> resultGetter, MethodBase methodBase)
        {
            NullGuard.NotNull(resultGetter, nameof(resultGetter))
                .NotNull(methodBase, nameof(methodBase));

            try
            {
                return resultGetter();
            }
            catch (AggregateException aggregateException)
            {
                Exception handledException = null;
                aggregateException.Handle(exception =>
                {
                    handledException = exception;
                    return true;
                });

                throw new IntranetExceptionBuilder(ErrorCode.RepositoryError, methodBase.Name, handledException.Message)
                    .WithInnerException(handledException)
                    .WithMethodBase(methodBase)
                    .Build();
            }
        }
        
        #endregion
    }
}