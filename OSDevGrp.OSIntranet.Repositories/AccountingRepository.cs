using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.EventPublisher;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Converters;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Models.Accounting;

namespace OSDevGrp.OSIntranet.Repositories
{
    public class AccountingRepository : DatabaseRepositoryBase<RepositoryContext>, IAccountingRepository
    {
        #region Private variables

        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Constructor

        public AccountingRepository(RepositoryContext repositoryContext, IConfiguration configuration, ILoggerFactory loggerFactory, IEventPublisher eventPublisher)
            : base(repositoryContext, configuration, loggerFactory)
        {
            NullGuard.NotNull(eventPublisher, nameof(eventPublisher));

            _eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        public Task<IEnumerable<IAccounting>> GetAccountingsAsync()
        {
            return ExecuteAsync(async () =>
                {
                    using AccountingModelHandler accountingModelHandler = new AccountingModelHandler(DbContext, AccountingModelConverter.Create(), _eventPublisher, DateTime.Today, false, false);
                    return await accountingModelHandler.ReadAsync();
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<bool> AccountingExistsAsync(int number)
        {
            return ExecuteAsync(async () =>
                {
                    using AccountingModelHandler accountingModelHandler = new AccountingModelHandler(DbContext, AccountingModelConverter.Create(), _eventPublisher, DateTime.Today, false, false);
                    return await accountingModelHandler.ExistsAsync(number);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IAccounting> GetAccountingAsync(int number, DateTime statusDate)
        {
            return ExecuteAsync(async () =>
                {
                    using AccountingModelHandler accountingModelHandler = new AccountingModelHandler(DbContext, AccountingModelConverter.Create(), _eventPublisher, statusDate, true, true);
                    return await accountingModelHandler.ReadAsync(number, new AccountingIdentificationState(number));
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IAccounting> CreateAccountingAsync(IAccounting accounting)
        {
            NullGuard.NotNull(accounting, nameof(accounting));

            return ExecuteAsync(async () =>
                {
                    using AccountingModelHandler accountingModelHandler = new AccountingModelHandler(DbContext, AccountingModelConverter.Create(), _eventPublisher, DateTime.Today, false, false);
                    return await accountingModelHandler.CreateAsync(accounting, new AccountingIdentificationState(accounting.Number));
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IAccounting> UpdateAccountingAsync(IAccounting accounting)
        {
            NullGuard.NotNull(accounting, nameof(accounting));

            return ExecuteAsync(async () =>
                {
                    using AccountingModelHandler accountingModelHandler = new AccountingModelHandler(DbContext, AccountingModelConverter.Create(), _eventPublisher, DateTime.Today, true, true);
                    return await accountingModelHandler.UpdateAsync(accounting, new AccountingIdentificationState(accounting.Number));
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IAccounting> DeleteAccountingAsync(int number)
        {
            return ExecuteAsync(async () =>
                {
                    using AccountingModelHandler accountingModelHandler = new AccountingModelHandler(DbContext, AccountingModelConverter.Create(), _eventPublisher, DateTime.Today, true, true);
                    return await accountingModelHandler.DeleteAsync(number, new AccountingIdentificationState(number));
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IAccountCollection> GetAccountsAsync(int accountingNumber, DateTime statusDate)
        {
            return ExecuteAsync(async () =>
                {
                    using AccountModelHandler accountModelHandler = new AccountModelHandler(DbContext, AccountingModelConverter.Create(), _eventPublisher, statusDate, true, true);

                    IAccountCollection accountCollection = new AccountCollection
                    {
                        await accountModelHandler.ReadAsync(accountModel => accountModel.AccountingIdentifier == accountingNumber, prepareReadState: new AccountingIdentificationState(accountingNumber))
                    };

                    return accountCollection;
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<bool> AccountExistsAsync(int accountingNumber, string accountNumber)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            return ExecuteAsync(async () =>
                {
                    using AccountModelHandler accountModelHandler = new AccountModelHandler(DbContext, AccountingModelConverter.Create(), _eventPublisher, DateTime.Today, false, false);
                    return await accountModelHandler.ExistsAsync(accountingNumber, accountNumber);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IAccount> GetAccountAsync(int accountingNumber, string accountNumber, DateTime statusDate)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            return ExecuteAsync(async () =>
                {
                    using AccountModelHandler accountModelHandler = new AccountModelHandler(DbContext, AccountingModelConverter.Create(), _eventPublisher, statusDate, true, true);
                    return await accountModelHandler.ReadAsync(accountingNumber, accountNumber);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IAccount> CreateAccountAsync(IAccount account)
        {
            NullGuard.NotNull(account, nameof(account));

            return ExecuteAsync(async () =>
                {
                    using AccountModelHandler accountModelHandler = new AccountModelHandler(DbContext, AccountingModelConverter.Create(), _eventPublisher, DateTime.Today, true, false);
                    return await accountModelHandler.CreateAsync(account, new AccountingIdentificationState(account.Accounting.Number));
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IAccount> UpdateAccountAsync(IAccount account)
        {
            NullGuard.NotNull(account, nameof(account));

            return ExecuteAsync(async () =>
                {
                    using AccountModelHandler accountModelHandler = new AccountModelHandler(DbContext, AccountingModelConverter.Create(), _eventPublisher, DateTime.Today, true, true);
                    return await accountModelHandler.UpdateAsync(account, new AccountingIdentificationState(account.Accounting.Number));
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IAccount> DeleteAccountAsync(int accountingNumber, string accountNumber)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            return ExecuteAsync(async () =>
                {
                    using AccountModelHandler accountModelHandler = new AccountModelHandler(DbContext, AccountingModelConverter.Create(), _eventPublisher, DateTime.Today, true, true);
                    return await accountModelHandler.DeleteAsync(new Tuple<int, string>(accountingNumber, accountNumber), new AccountingIdentificationState(accountingNumber));
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IBudgetAccountCollection> GetBudgetAccountsAsync(int accountingNumber, DateTime statusDate)
        {
            return ExecuteAsync(async () =>
                {
                    using BudgetAccountModelHandler budgetAccountModelHandler = new BudgetAccountModelHandler(DbContext, AccountingModelConverter.Create(), _eventPublisher, statusDate, true, true);

                    IBudgetAccountCollection budgetAccountCollection = new BudgetAccountCollection
                    {
                        await budgetAccountModelHandler.ReadAsync(budgetAccountModel => budgetAccountModel.AccountingIdentifier == accountingNumber, prepareReadState: new AccountingIdentificationState(accountingNumber))
                    };

                    return budgetAccountCollection;
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<bool> BudgetAccountExistsAsync(int accountingNumber, string accountNumber)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            return ExecuteAsync(async () =>
                {
                    using BudgetAccountModelHandler budgetAccountModelHandler = new BudgetAccountModelHandler(DbContext, AccountingModelConverter.Create(), _eventPublisher, DateTime.Today, false, false);
                    return await budgetAccountModelHandler.ExistsAsync(accountingNumber, accountNumber);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IBudgetAccount> GetBudgetAccountAsync(int accountingNumber, string accountNumber, DateTime statusDate)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            return ExecuteAsync(async () =>
                {
                    using BudgetAccountModelHandler budgetAccountModelHandler = new BudgetAccountModelHandler(DbContext, AccountingModelConverter.Create(), _eventPublisher, statusDate, true, true);
                    return await budgetAccountModelHandler.ReadAsync(accountingNumber, accountNumber);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IBudgetAccount> CreateBudgetAccountAsync(IBudgetAccount budgetAccount)
        {
            NullGuard.NotNull(budgetAccount, nameof(budgetAccount));

            return ExecuteAsync(async () =>
                {
                    using BudgetAccountModelHandler budgetAccountModelHandler = new BudgetAccountModelHandler(DbContext, AccountingModelConverter.Create(), _eventPublisher, DateTime.Today, true, false);
                    return await budgetAccountModelHandler.CreateAsync(budgetAccount, new AccountingIdentificationState(budgetAccount.Accounting.Number));
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IBudgetAccount> UpdateBudgetAccountAsync(IBudgetAccount budgetAccount)
        {
            NullGuard.NotNull(budgetAccount, nameof(budgetAccount));

            return ExecuteAsync(async () =>
                {
                    using BudgetAccountModelHandler budgetAccountModelHandler = new BudgetAccountModelHandler(DbContext, AccountingModelConverter.Create(), _eventPublisher, DateTime.Today, true, true);
                    return await budgetAccountModelHandler.UpdateAsync(budgetAccount, new AccountingIdentificationState(budgetAccount.Accounting.Number));
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IBudgetAccount> DeleteBudgetAccountAsync(int accountingNumber, string accountNumber)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            return ExecuteAsync(async () =>
                {
                    using BudgetAccountModelHandler budgetAccountModelHandler = new BudgetAccountModelHandler(DbContext, AccountingModelConverter.Create(), _eventPublisher, DateTime.Today, true, true);
                    return await budgetAccountModelHandler.DeleteAsync(new Tuple<int, string>(accountingNumber, accountNumber), new AccountingIdentificationState(accountingNumber));
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IContactAccountCollection> GetContactAccountsAsync(int accountingNumber, DateTime statusDate)
        {
            return ExecuteAsync(async () =>
                {
                    using ContactAccountModelHandler contactAccountModelHandler = new ContactAccountModelHandler(DbContext, AccountingModelConverter.Create(), _eventPublisher, statusDate, true);

                    IContactAccountCollection contactAccountCollection = new ContactAccountCollection
                    {
                        await contactAccountModelHandler.ReadAsync(contactAccountModel => contactAccountModel.AccountingIdentifier == accountingNumber, prepareReadState: new AccountingIdentificationState(accountingNumber))
                    };

                    return contactAccountCollection;
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<bool> ContactAccountExistsAsync(int accountingNumber, string accountNumber)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            return ExecuteAsync(async () =>
                {
                    using ContactAccountModelHandler contactAccountModelHandler = new ContactAccountModelHandler(DbContext, AccountingModelConverter.Create(), _eventPublisher, DateTime.Today, false);
                    return await contactAccountModelHandler.ExistsAsync(accountingNumber, accountNumber);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IContactAccount> GetContactAccountAsync(int accountingNumber, string accountNumber, DateTime statusDate)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            return ExecuteAsync(async () =>
                {
                    using ContactAccountModelHandler contactAccountModelHandler = new ContactAccountModelHandler(DbContext, AccountingModelConverter.Create(), _eventPublisher, statusDate, true);
                    return await contactAccountModelHandler.ReadAsync(accountingNumber, accountNumber);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IContactAccount> CreateContactAccountAsync(IContactAccount contactAccount)
        {
            NullGuard.NotNull(contactAccount, nameof(contactAccount));

            return ExecuteAsync(async () =>
                {
                    using ContactAccountModelHandler contactAccountModelHandler = new ContactAccountModelHandler(DbContext, AccountingModelConverter.Create(), _eventPublisher, DateTime.Today, false);
                    return await contactAccountModelHandler.CreateAsync(contactAccount, new AccountingIdentificationState(contactAccount.Accounting.Number));
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IContactAccount> UpdateContactAccountAsync(IContactAccount contactAccount)
        {
            NullGuard.NotNull(contactAccount, nameof(contactAccount));

            return ExecuteAsync(async () =>
                {
                    using ContactAccountModelHandler contactAccountModelHandler = new ContactAccountModelHandler(DbContext, AccountingModelConverter.Create(), _eventPublisher, DateTime.Today, true);
                    return await contactAccountModelHandler.UpdateAsync(contactAccount, new AccountingIdentificationState(contactAccount.Accounting.Number));
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IContactAccount> DeleteContactAccountAsync(int accountingNumber, string accountNumber)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            return ExecuteAsync(async () =>
                {
                    using ContactAccountModelHandler contactAccountModelHandler = new ContactAccountModelHandler(DbContext, AccountingModelConverter.Create(), _eventPublisher, DateTime.Today, true);
                    return await contactAccountModelHandler.DeleteAsync(new Tuple<int, string>(accountingNumber, accountNumber), new AccountingIdentificationState(accountingNumber));
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IPostingLineCollection> GetPostingLinesAsync(int accountingNumber, DateTime statusDate, int numberOfPostingLines)
        {
            return ExecuteAsync(async () =>
                {
                    using PostingLineModelHandler postingLineModelHandler = new PostingLineModelHandler(DbContext, AccountingModelConverter.Create(), _eventPublisher, DateTime.MinValue, statusDate, true, true, numberOfPostingLines);

                    IPostingLineCollection postingLineCollection = new PostingLineCollection
                    {
                        await postingLineModelHandler.ReadAsync(accountingNumber)
                    };

                    return postingLineCollection;
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IPostingJournalResult> ApplyPostingJournalAsync(IPostingJournal postingJournal, IPostingWarningCalculator postingWarningCalculator)
        {
            NullGuard.NotNull(postingJournal, nameof(postingJournal))
                .NotNull(postingWarningCalculator, nameof(postingWarningCalculator));

            return ExecuteAsync(async () =>
                {
                    IPostingLineCollection postingLineCollection = new PostingLineCollection();
                    foreach (IGrouping<int, IPostingLine> group in postingJournal.PostingLineCollection.GroupBy(postingLine => postingLine.Accounting.Number))
                    {
                        using PostingLineModelHandler postingLineModelHandler = new PostingLineModelHandler(DbContext, AccountingModelConverter.Create(), _eventPublisher, DateTime.MinValue, DateTime.Today, true, true, applyingPostingLines: true);

                        AccountingIdentificationState accountingIdentificationState = new AccountingIdentificationState(group.Key);

                        IPostingLine[] createdPostingLineCollection = (await postingLineModelHandler.CreateAsync(group.OrderBy(m => m.PostingDate).ThenBy(m => m.SortOrder), accountingIdentificationState, true)).ToArray();

                        IPostingLine[] updatedPostingLineCollection = (await postingLineModelHandler.UpdateAsync(createdPostingLineCollection.OrderBy(m => m.PostingDate.Date).ThenBy(m => m.SortOrder), accountingIdentificationState)).ToArray();
                        foreach (IPostingLine postingLine in updatedPostingLineCollection)
                        {
                            IPostingLine[] affectedPostingCollection = (await postingLineModelHandler.ReadAsync(m => m.AccountingIdentifier == accountingIdentificationState.AccountingIdentifier && (m.PostingDate == postingLine.PostingDate && m.PostingLineIdentifier > postingLine.SortOrder || m.PostingDate > postingLine.PostingDate), prepareReadState: accountingIdentificationState)).ToArray();
                            foreach (IPostingLine affectedPostingLine in affectedPostingCollection.Where(m => updatedPostingLineCollection.Contains(m) == false))
                            {
                                await postingLineModelHandler.UpdateAsync(affectedPostingLine, accountingIdentificationState);
                            }
                        }

                        postingLineCollection.Add(updatedPostingLineCollection);
                    }

                    return (IPostingJournalResult) new PostingJournalResult(postingLineCollection, postingWarningCalculator);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IEnumerable<IAccountGroup>> GetAccountGroupsAsync()
        {
            return ExecuteAsync(async () =>
                {
                    using AccountGroupModelHandler accountGroupModelHandler = new AccountGroupModelHandler(DbContext, AccountingModelConverter.Create());
                    return await accountGroupModelHandler.ReadAsync();
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IAccountGroup> GetAccountGroupAsync(int number)
        {
            return ExecuteAsync(async () =>
                {
                    using AccountGroupModelHandler accountGroupModelHandler = new AccountGroupModelHandler(DbContext, AccountingModelConverter.Create());
                    return await accountGroupModelHandler.ReadAsync(number);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IAccountGroup> CreateAccountGroupAsync(IAccountGroup accountGroup)
        {
            NullGuard.NotNull(accountGroup, nameof(accountGroup));

            return ExecuteAsync(async () =>
                {
                    using AccountGroupModelHandler accountGroupModelHandler = new AccountGroupModelHandler(DbContext, AccountingModelConverter.Create());
                    return await accountGroupModelHandler.CreateAsync(accountGroup);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IAccountGroup> UpdateAccountGroupAsync(IAccountGroup accountGroup)
        {
            NullGuard.NotNull(accountGroup, nameof(accountGroup));

            return ExecuteAsync(async () =>
                {
                    using AccountGroupModelHandler accountGroupModelHandler = new AccountGroupModelHandler(DbContext, AccountingModelConverter.Create());
                    return await accountGroupModelHandler.UpdateAsync(accountGroup);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IAccountGroup> DeleteAccountGroupAsync(int number)
        {
            return ExecuteAsync(async () =>
                {
                    using AccountGroupModelHandler accountGroupModelHandler = new AccountGroupModelHandler(DbContext, AccountingModelConverter.Create());
                    return await accountGroupModelHandler.DeleteAsync(number);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IEnumerable<IBudgetAccountGroup>> GetBudgetAccountGroupsAsync()
        {
            return ExecuteAsync(async () =>
                {
                    using BudgetAccountGroupModelHandler budgetAccountGroupModelHandler = new BudgetAccountGroupModelHandler(DbContext, AccountingModelConverter.Create());
                    return await budgetAccountGroupModelHandler.ReadAsync();
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IBudgetAccountGroup> GetBudgetAccountGroupAsync(int number)
        {
            return ExecuteAsync(async () =>
                {
                    using BudgetAccountGroupModelHandler budgetAccountGroupModelHandler = new BudgetAccountGroupModelHandler(DbContext, AccountingModelConverter.Create());
                    return await budgetAccountGroupModelHandler.ReadAsync(number);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IBudgetAccountGroup> CreateBudgetAccountGroupAsync(IBudgetAccountGroup budgetAccountGroup)
        {
            NullGuard.NotNull(budgetAccountGroup, nameof(budgetAccountGroup));

            return ExecuteAsync(async () =>
                {
                    using BudgetAccountGroupModelHandler budgetAccountGroupModelHandler = new BudgetAccountGroupModelHandler(DbContext, AccountingModelConverter.Create());
                    return await budgetAccountGroupModelHandler.CreateAsync(budgetAccountGroup);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IBudgetAccountGroup> UpdateBudgetAccountGroupAsync(IBudgetAccountGroup budgetAccountGroup)
        {
            NullGuard.NotNull(budgetAccountGroup, nameof(budgetAccountGroup));

            return ExecuteAsync(async () =>
                {
                    using BudgetAccountGroupModelHandler budgetAccountGroupModelHandler = new BudgetAccountGroupModelHandler(DbContext, AccountingModelConverter.Create());
                    return await budgetAccountGroupModelHandler.UpdateAsync(budgetAccountGroup);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IBudgetAccountGroup> DeleteBudgetAccountGroupAsync(int number)
        {
            return ExecuteAsync(async () =>
                {
                    using BudgetAccountGroupModelHandler budgetAccountGroupModelHandler = new BudgetAccountGroupModelHandler(DbContext, AccountingModelConverter.Create());
                    return await budgetAccountGroupModelHandler.DeleteAsync(number);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IEnumerable<IPaymentTerm>> GetPaymentTermsAsync()
        {
            return ExecuteAsync(async () =>
                {
                    using PaymentTermModelHandler paymentTermModelHandler = new PaymentTermModelHandler(DbContext, AccountingModelConverter.Create());
                    return await paymentTermModelHandler.ReadAsync();
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IPaymentTerm> GetPaymentTermAsync(int number)
        {
            return ExecuteAsync(async () =>
                {
                    using PaymentTermModelHandler paymentTermModelHandler = new PaymentTermModelHandler(DbContext, AccountingModelConverter.Create());
                    return await paymentTermModelHandler.ReadAsync(number);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IPaymentTerm> CreatePaymentTermAsync(IPaymentTerm paymentTerm)
        {
            NullGuard.NotNull(paymentTerm, nameof(paymentTerm));

            return ExecuteAsync(async () =>
                {
                    using PaymentTermModelHandler paymentTermModelHandler = new PaymentTermModelHandler(DbContext, AccountingModelConverter.Create());
                    return await paymentTermModelHandler.CreateAsync(paymentTerm);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IPaymentTerm> UpdatePaymentTermAsync(IPaymentTerm paymentTerm)
        {
            NullGuard.NotNull(paymentTerm, nameof(paymentTerm));

            return ExecuteAsync(async () =>
                {
                    using PaymentTermModelHandler paymentTermModelHandler = new PaymentTermModelHandler(DbContext, AccountingModelConverter.Create());
                    return await paymentTermModelHandler.UpdateAsync(paymentTerm);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IPaymentTerm> DeletePaymentTermAsync(int number)
        {
            return ExecuteAsync(async () =>
                {
                    using PaymentTermModelHandler paymentTermModelHandler = new PaymentTermModelHandler(DbContext, AccountingModelConverter.Create());
                    return await paymentTermModelHandler.DeleteAsync(number);
                },
                MethodBase.GetCurrentMethod());
        }

        #endregion
    }
}