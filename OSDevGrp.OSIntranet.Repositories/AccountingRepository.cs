using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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

        public AccountingRepository(IConfiguration configuration, IPrincipalResolver principalResolver, ILoggerFactory loggerFactory)
            : base(configuration, principalResolver, loggerFactory)
        {
        }

        #endregion

        #region Methods

        public Task<IEnumerable<IAccounting>> GetAccountingsAsync()
        {
            return Task.Run(GetAccountings);
        }

        public Task<IAccounting> GetAccountingAsync(int number, DateTime statusDate)
        {
            return Task.Run(() => GetAccounting(number, statusDate));
        }

        public Task<IAccounting> CreateAccountingAsync(IAccounting accounting)
        {
            NullGuard.NotNull(accounting, nameof(accounting));

            return Task.Run(() => CreateAccounting(accounting));
        }

        public Task<IAccounting> UpdateAccountingAsync(IAccounting accounting)
        {
            NullGuard.NotNull(accounting, nameof(accounting));

            return Task.Run(() => UpdateAccounting(accounting));
        }

        public Task<IAccounting> DeleteAccountingAsync(int number)
        {
            return Task.Run(() => DeleteAccounting(number));
        }

        public Task<IEnumerable<IAccountGroup>> GetAccountGroupsAsync()
        {
            return Task.Run(GetAccountGroups);
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

        public Task<IAccountGroup> DeleteAccountGroupAsync(int number)
        {
            return Task.Run(() => DeleteAccountGroup(number));
        }

        public Task<IEnumerable<IBudgetAccountGroup>> GetBudgetAccountGroupsAsync()
        {
            return Task.Run(GetBudgetAccountGroups);
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

        public Task<IBudgetAccountGroup> DeleteBudgetAccountGroupAsync(int number)
        {
            return Task.Run(() => DeleteBudgetAccountGroup(number));
        }

        public Task<IEnumerable<IPaymentTerm>> GetPaymentTermsAsync()
        {
            return Task.Run(GetPaymentTerms);
        }

        public Task<IPaymentTerm> GetPaymentTermAsync(int number)
        {
            return Task.Run(() => GetPaymentTerm(number));
        }

        public Task<IPaymentTerm> CreatePaymentTermAsync(IPaymentTerm paymentTerm)
        {
            NullGuard.NotNull(paymentTerm, nameof(paymentTerm));

            return Task.Run(() => CreatePaymentTerm(paymentTerm));
        }

        public Task<IPaymentTerm> UpdatePaymentTermAsync(IPaymentTerm paymentTerm)
        {
            NullGuard.NotNull(paymentTerm, nameof(paymentTerm));

            return Task.Run(() => UpdatePaymentTerm(paymentTerm));
        }

        public Task<IPaymentTerm> DeletePaymentTermAsync(int number)
        {
            return Task.Run(() => DeletePaymentTerm(number));
        }

        private IEnumerable<IAccounting> GetAccountings()
        {
            return Execute(() =>
                {
                    using AccountingContext context = new AccountingContext(Configuration, PrincipalResolver, LoggerFactory);
                    return context.Accountings
                        .Include(accountingModel => accountingModel.LetterHead)
                        .AsParallel()
                        .Select(accountingModel => 
                        {
                            using (AccountingContext subContext = new AccountingContext(Configuration, PrincipalResolver, LoggerFactory))
                            {
                                accountingModel.Deletable = CanDeleteAccounting(subContext, accountingModel.AccountingIdentifier);
                            }

                            return _accountingModelConverter.Convert<AccountingModel, IAccounting>(accountingModel);
                        })
                        .OrderBy(accounting => accounting.Number)
                        .ToList();
                },
                MethodBase.GetCurrentMethod());
        }

        private IAccounting GetAccounting(int number, DateTime statusDate)
        {
            return Execute(() =>
                {
                    using AccountingContext context = new AccountingContext(Configuration, PrincipalResolver, LoggerFactory);
                    AccountingModel accountingModel = context.Accountings
                        .Include(model => model.LetterHead)
                        .SingleOrDefault(model => model.AccountingIdentifier == number);
                    if (accountingModel == null)
                    {
                        return null;
                    }

                    accountingModel.Deletable = CanDeleteAccounting(context, accountingModel.AccountingIdentifier);

                    return  _accountingModelConverter.Convert<AccountingModel, IAccounting>(accountingModel);
                },
                MethodBase.GetCurrentMethod());
        }

        private IAccounting CreateAccounting(IAccounting accounting)
        {
            NullGuard.NotNull(accounting, nameof(accounting));

            return Execute(() =>
                {
                    using AccountingContext context = new AccountingContext(Configuration, PrincipalResolver, LoggerFactory);
                    AccountingModel accountingModel = _accountingModelConverter.Convert<IAccounting, AccountingModel>(accounting);

                    accountingModel.LetterHead = context.LetterHeads.Single(letterHeadModel => letterHeadModel.LetterHeadIdentifier == accountingModel.LetterHeadIdentifier);

                    context.Accountings.Add(accountingModel);

                    context.SaveChanges();

                    return GetAccounting(accounting.Number, DateTime.Today);
                },
                MethodBase.GetCurrentMethod());
        }

        private IAccounting UpdateAccounting(IAccounting accounting)
        {
            NullGuard.NotNull(accounting, nameof(accounting));

            return Execute(() =>
                {
                    using AccountingContext context = new AccountingContext(Configuration, PrincipalResolver, LoggerFactory);
                    AccountingModel accountingModel = context.Accountings
                        .Include(model => model.LetterHead)
                        .SingleOrDefault(model => model.AccountingIdentifier == accounting.Number);
                    if (accountingModel == null)
                    {
                        return null;
                    }

                    accountingModel.Name = accounting.Name;
                    accountingModel.LetterHeadIdentifier = accounting.LetterHead.Number;
                    accountingModel.LetterHead = context.LetterHeads.Single(letterHeadModel => letterHeadModel.LetterHeadIdentifier == accounting.LetterHead.Number);
                    accountingModel.BalanceBelowZero = accounting.BalanceBelowZero;
                    accountingModel.BackDating = accounting.BackDating;

                    context.SaveChanges();

                    return GetAccounting(accounting.Number, DateTime.Today);
                },
                MethodBase.GetCurrentMethod());
        }

        private IAccounting DeleteAccounting(int number)
        {
            return Execute(() =>
                {
                    using AccountingContext context = new AccountingContext(Configuration, PrincipalResolver, LoggerFactory);
                    AccountingModel accountingModel = context.Accountings
                        .Include(model => model.LetterHead)
                        .SingleOrDefault(model => model.AccountingIdentifier == number);
                    if (accountingModel == null)
                    {
                        return null;
                    }

                    if (CanDeleteAccounting(context, accountingModel.AccountingIdentifier) == false)
                    {
                        return GetAccounting(accountingModel.AccountingIdentifier, DateTime.Today);
                    }

                    context.Accountings.Remove(accountingModel);

                    context.SaveChanges();

                    return null;
                },
                MethodBase.GetCurrentMethod());
        }

        private bool CanDeleteAccounting(AccountingContext context, int accountingIdentifier)
        {
            NullGuard.NotNull(context, nameof(context));

            return false;
        }

        private IEnumerable<IAccountGroup> GetAccountGroups()
        {
            return Execute(() =>
                {
                    using AccountingContext context = new AccountingContext(Configuration, PrincipalResolver, LoggerFactory);
                    return context.AccountGroups.AsParallel()
                        .Select(accountGroupModel => 
                        {
                            using (AccountingContext subContext = new AccountingContext(Configuration, PrincipalResolver, LoggerFactory))
                            {
                                accountGroupModel.Deletable = CanDeleteAccountGroup(subContext, accountGroupModel.AccountGroupIdentifier);
                            }

                            return _accountingModelConverter.Convert<AccountGroupModel, IAccountGroup>(accountGroupModel);
                        })
                        .OrderBy(accountGroup => accountGroup.Number)
                        .ToList();
                },
                MethodBase.GetCurrentMethod());
        }

        private IAccountGroup GetAccountGroup(int number)
        {
            return Execute(() =>
                {
                    using AccountingContext context = new AccountingContext(Configuration, PrincipalResolver, LoggerFactory);
                    AccountGroupModel accountGroupModel = context.AccountGroups.Find(number);
                    if (accountGroupModel == null)
                    {
                        return null;
                    }

                    accountGroupModel.Deletable = CanDeleteAccountGroup(context, accountGroupModel.AccountGroupIdentifier);

                    return  _accountingModelConverter.Convert<AccountGroupModel, IAccountGroup>(accountGroupModel);
                },
                MethodBase.GetCurrentMethod());
        }

        private IAccountGroup CreateAccountGroup(IAccountGroup accountGroup)
        {
            NullGuard.NotNull(accountGroup, nameof(accountGroup));

            return Execute(() =>
                {
                    using AccountingContext context = new AccountingContext(Configuration, PrincipalResolver, LoggerFactory);
                    AccountGroupModel accountGroupModel = _accountingModelConverter.Convert<IAccountGroup, AccountGroupModel>(accountGroup);

                    context.AccountGroups.Add(accountGroupModel);

                    context.SaveChanges();

                    return GetAccountGroup(accountGroup.Number);
                },
                MethodBase.GetCurrentMethod());
        }

        private IAccountGroup UpdateAccountGroup(IAccountGroup accountGroup)
        {
            NullGuard.NotNull(accountGroup, nameof(accountGroup));

            return Execute(() =>
                {
                    using AccountingContext context = new AccountingContext(Configuration, PrincipalResolver, LoggerFactory);
                    AccountGroupModel accountGroupModel = context.AccountGroups.Find(accountGroup.Number);
                    if (accountGroupModel == null)
                    {
                        return null;
                    }

                    accountGroupModel.Name = accountGroup.Name;
                    accountGroupModel.AccountGroupType = accountGroup.AccountGroupType;

                    context.SaveChanges();

                    return GetAccountGroup(accountGroup.Number);
                },
                MethodBase.GetCurrentMethod());
        }

        private IAccountGroup DeleteAccountGroup(int number)
        {
            return Execute(() =>
                {
                    using AccountingContext context = new AccountingContext(Configuration, PrincipalResolver, LoggerFactory);
                    AccountGroupModel accountGroupModel = context.AccountGroups.Find(number);
                    if (accountGroupModel == null)
                    {
                        return null;
                    }

                    if (CanDeleteAccountGroup(context, accountGroupModel.AccountGroupIdentifier) == false)
                    {
                        return GetAccountGroup(accountGroupModel.AccountGroupIdentifier);
                    }

                    context.AccountGroups.Remove(accountGroupModel);

                    context.SaveChanges();

                    return null;
                },
                MethodBase.GetCurrentMethod());
        }

        private bool CanDeleteAccountGroup(AccountingContext context, int accountGroupIdentifier)
        {
            NullGuard.NotNull(context, nameof(context));

            return false;
        }

        private IEnumerable<IBudgetAccountGroup> GetBudgetAccountGroups()
        {
            return Execute(() =>
                {
                    using AccountingContext context = new AccountingContext(Configuration, PrincipalResolver, LoggerFactory);
                    return context.BudgetAccountGroups.AsParallel()
                        .Select(budgetAccountGroupModel => 
                        {
                            using (AccountingContext subContext = new AccountingContext(Configuration, PrincipalResolver, LoggerFactory))
                            {
                                budgetAccountGroupModel.Deletable = CanDeleteBudgetAccountGroup(subContext, budgetAccountGroupModel.BudgetAccountGroupIdentifier);
                            }

                            return _accountingModelConverter.Convert<BudgetAccountGroupModel, IBudgetAccountGroup>(budgetAccountGroupModel);
                        })
                        .OrderBy(budgetAccountGroup => budgetAccountGroup.Number)
                        .ToList();
                },
                MethodBase.GetCurrentMethod());
        }

        private IBudgetAccountGroup GetBudgetAccountGroup(int number)
        {
            return Execute(() =>
                {
                    using AccountingContext context = new AccountingContext(Configuration, PrincipalResolver, LoggerFactory);
                    BudgetAccountGroupModel budgetAccountGroupModel = context.BudgetAccountGroups.Find(number);
                    if (budgetAccountGroupModel == null)
                    {
                        return null;
                    }

                    budgetAccountGroupModel.Deletable = CanDeleteBudgetAccountGroup(context, budgetAccountGroupModel.BudgetAccountGroupIdentifier);

                    return  _accountingModelConverter.Convert<BudgetAccountGroupModel, IBudgetAccountGroup>(budgetAccountGroupModel);
                },
                MethodBase.GetCurrentMethod());
        }

        private IBudgetAccountGroup CreateBudgetAccountGroup(IBudgetAccountGroup budgetAccountGroup)
        {
            NullGuard.NotNull(budgetAccountGroup, nameof(budgetAccountGroup));

            return Execute(() =>
                {
                    using AccountingContext context = new AccountingContext(Configuration, PrincipalResolver, LoggerFactory);
                    BudgetAccountGroupModel budgetAccountGroupModel = _accountingModelConverter.Convert<IBudgetAccountGroup, BudgetAccountGroupModel>(budgetAccountGroup);

                    context.BudgetAccountGroups.Add(budgetAccountGroupModel);

                    context.SaveChanges();

                    return GetBudgetAccountGroup(budgetAccountGroup.Number);
                },
                MethodBase.GetCurrentMethod());
        }

        private IBudgetAccountGroup UpdateBudgetAccountGroup(IBudgetAccountGroup budgetAccountGroup)
        {
            NullGuard.NotNull(budgetAccountGroup, nameof(budgetAccountGroup));

            return Execute(() =>
                {
                    using AccountingContext context = new AccountingContext(Configuration, PrincipalResolver, LoggerFactory);
                    BudgetAccountGroupModel budgetAccountGroupModel = context.BudgetAccountGroups.Find(budgetAccountGroup.Number);
                    if (budgetAccountGroupModel == null)
                    {
                        return null;
                    }

                    budgetAccountGroupModel.Name = budgetAccountGroup.Name;

                    context.SaveChanges();

                    return GetBudgetAccountGroup(budgetAccountGroup.Number);
                },
                MethodBase.GetCurrentMethod());
        }

        private IBudgetAccountGroup DeleteBudgetAccountGroup(int number)
        {
            return Execute(() =>
                {
                    using AccountingContext context = new AccountingContext(Configuration, PrincipalResolver, LoggerFactory);
                    BudgetAccountGroupModel budgetAccountGroupModel = context.BudgetAccountGroups.Find(number);
                    if (budgetAccountGroupModel == null)
                    {
                        return null;
                    }

                    if (CanDeleteBudgetAccountGroup(context, budgetAccountGroupModel.BudgetAccountGroupIdentifier) == false)
                    {
                        return GetBudgetAccountGroup(budgetAccountGroupModel.BudgetAccountGroupIdentifier);
                    }

                    context.BudgetAccountGroups.Remove(budgetAccountGroupModel);

                    context.SaveChanges();

                    return null;
                },
                MethodBase.GetCurrentMethod());
        }

        private bool CanDeleteBudgetAccountGroup(AccountingContext context, int budgetAccountGroupIdentifier)
        {
            NullGuard.NotNull(context, nameof(context));

            return false;
        }

        private IEnumerable<IPaymentTerm> GetPaymentTerms()
        {
            return Execute(() =>
                {
                    using AccountingContext context = new AccountingContext(Configuration, PrincipalResolver, LoggerFactory);
                    return context.PaymentTerms.AsParallel()
                        .Select(paymentTermModel =>
                        {
                            using (AccountingContext subContext = new AccountingContext(Configuration, PrincipalResolver, LoggerFactory))
                            {
                                paymentTermModel.Deletable = CanDeletePaymentTerm(subContext, paymentTermModel.PaymentTermIdentifier);
                            }

                            return _accountingModelConverter.Convert<PaymentTermModel, IPaymentTerm>(paymentTermModel);
                        })
                        .OrderBy(paymentTerm => paymentTerm.Number)
                        .ToList();
                },
                MethodBase.GetCurrentMethod());
        }

        private IPaymentTerm GetPaymentTerm(int number)
        {
            return Execute(() =>
                {
                    using AccountingContext context = new AccountingContext(Configuration, PrincipalResolver, LoggerFactory);
                    PaymentTermModel paymentTermModel = context.PaymentTerms.Find(number);
                    if (paymentTermModel == null)
                    {
                        return null;
                    }

                    paymentTermModel.Deletable = CanDeletePaymentTerm(context, paymentTermModel.PaymentTermIdentifier);

                    return _accountingModelConverter.Convert<PaymentTermModel, IPaymentTerm>(paymentTermModel);
                },
                MethodBase.GetCurrentMethod());
        }

        private IPaymentTerm CreatePaymentTerm(IPaymentTerm paymentTerm)
        {
            NullGuard.NotNull(paymentTerm, nameof(paymentTerm));

            return Execute(() =>
                {
                    using AccountingContext context = new AccountingContext(Configuration, PrincipalResolver, LoggerFactory);
                    PaymentTermModel paymentTermModel = _accountingModelConverter.Convert<IPaymentTerm, PaymentTermModel>(paymentTerm);

                    context.PaymentTerms.Add(paymentTermModel);

                    context.SaveChanges();

                    return GetPaymentTerm(paymentTerm.Number);
                },
                MethodBase.GetCurrentMethod());
        }

        private IPaymentTerm UpdatePaymentTerm(IPaymentTerm paymentTerm)
        {
            NullGuard.NotNull(paymentTerm, nameof(paymentTerm));

            return Execute(() =>
                {
                    using AccountingContext context = new AccountingContext(Configuration, PrincipalResolver, LoggerFactory);
                    PaymentTermModel paymentTermModel = context.PaymentTerms.Find(paymentTerm.Number);
                    if (paymentTermModel == null)
                    {
                        return null;
                    }

                    paymentTermModel.Name = paymentTerm.Name;

                    context.SaveChanges();

                    return GetPaymentTerm(paymentTerm.Number);
                },
                MethodBase.GetCurrentMethod());
        }

        private IPaymentTerm DeletePaymentTerm(int number)
        {
            return Execute(() =>
                {
                    using AccountingContext context = new AccountingContext(Configuration, PrincipalResolver, LoggerFactory);
                    PaymentTermModel paymentTermModel = context.PaymentTerms.Find(number);
                    if (paymentTermModel == null)
                    {
                        return null;
                    }

                    if (CanDeletePaymentTerm(context, paymentTermModel.PaymentTermIdentifier) == false)
                    {
                        return GetPaymentTerm(paymentTermModel.PaymentTermIdentifier);
                    }

                    context.PaymentTerms.Remove(paymentTermModel);

                    context.SaveChanges();

                    return null;
                },
                MethodBase.GetCurrentMethod());
        }

        private bool CanDeletePaymentTerm(AccountingContext context, int paymentTermIdentifier)
        {
            NullGuard.NotNull(context, nameof(context));

            return false;
        }

        #endregion
    }
}