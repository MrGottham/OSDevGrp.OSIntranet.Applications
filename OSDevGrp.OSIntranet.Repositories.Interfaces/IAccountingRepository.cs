using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces
{
    public interface IAccountingRepository : IRepository
    {
        Task<IEnumerable<IAccounting>> GetAccountingsAsync();

        Task<bool> AccountingExistsAsync(int number);

        Task<IAccounting> GetAccountingAsync(int number, DateTime statusDate);

        Task<IAccounting> CreateAccountingAsync(IAccounting accounting);

        Task<IAccounting> UpdateAccountingAsync(IAccounting accounting);

        Task<IAccounting> DeleteAccountingAsync(int number);

        Task<IAccountCollection> GetAccountsAsync(int accountingNumber, DateTime statusDate);

        Task<bool> AccountExistsAsync(int accountingNumber, string accountNumber);

        Task<IAccount> GetAccountAsync(int accountingNumber, string accountNumber, DateTime statusDate);

        Task<IAccount> CreateAccountAsync(IAccount account);

        Task<IAccount> UpdateAccountAsync(IAccount account);

        Task<IAccount> DeleteAccountAsync(int accountingNumber, string accountNumber);

        Task<IBudgetAccountCollection> GetBudgetAccountsAsync(int accountingNumber, DateTime statusDate);

        Task<bool> BudgetAccountExistsAsync(int accountingNumber, string accountNumber);

        Task<IBudgetAccount> GetBudgetAccountAsync(int accountingNumber, string accountNumber, DateTime statusDate);

        Task<IBudgetAccount> CreateBudgetAccountAsync(IBudgetAccount budgetAccount);

        Task<IBudgetAccount> UpdateBudgetAccountAsync(IBudgetAccount budgetAccount);

        Task<IBudgetAccount> DeleteBudgetAccountAsync(int accountingNumber, string accountNumber);

        Task<IContactAccountCollection> GetContactAccountsAsync(int accountingNumber, DateTime statusDate);

        Task<bool> ContactAccountExistsAsync(int accountingNumber, string accountNumber);

        Task<IContactAccount> GetContactAccountAsync(int accountingNumber, string accountNumber, DateTime statusDate);

        Task<IContactAccount> CreateContactAccountAsync(IContactAccount contactAccount);

        Task<IContactAccount> UpdateContactAccountAsync(IContactAccount contactAccount);

        Task<IContactAccount> DeleteContactAccountAsync(int accountingNumber, string accountNumber);

        Task<IPostingLineCollection> GetPostingLinesAsync(int accountingNumber, DateTime statusDate, int numberOfPostingLines);

        Task<IPostingJournalResult> ApplyPostingJournalAsync(IPostingJournal postingJournal, IPostingWarningCalculator postingWarningCalculator);

        Task<IEnumerable<IAccountGroup>> GetAccountGroupsAsync();

        Task<IAccountGroup> GetAccountGroupAsync(int number);

        Task<IAccountGroup> CreateAccountGroupAsync(IAccountGroup accountGroup);

        Task<IAccountGroup> UpdateAccountGroupAsync(IAccountGroup accountGroup);

        Task<IAccountGroup> DeleteAccountGroupAsync(int number);

        Task<IEnumerable<IBudgetAccountGroup>> GetBudgetAccountGroupsAsync();

        Task<IBudgetAccountGroup> GetBudgetAccountGroupAsync(int number);

        Task<IBudgetAccountGroup> CreateBudgetAccountGroupAsync(IBudgetAccountGroup budgetAccountGroup);

        Task<IBudgetAccountGroup> UpdateBudgetAccountGroupAsync(IBudgetAccountGroup budgetAccountGroup);

        Task<IBudgetAccountGroup> DeleteBudgetAccountGroupAsync(int number);

        Task<IEnumerable<IPaymentTerm>> GetPaymentTermsAsync();

        Task<IPaymentTerm> GetPaymentTermAsync(int number);

        Task<IPaymentTerm> CreatePaymentTermAsync(IPaymentTerm paymentTerm);

        Task<IPaymentTerm> UpdatePaymentTermAsync(IPaymentTerm paymentTerm);

        Task<IPaymentTerm> DeletePaymentTermAsync(int number);
    }
}