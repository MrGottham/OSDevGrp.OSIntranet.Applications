using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces
{
    public interface IAccountingRepository : IRepository
    {
        Task<IEnumerable<IAccounting>> GetAccountingsAsync();

        Task<IAccounting> GetAccountingAsync(int number, DateTime statusDate);

        Task<IAccounting> CreateAccountingAsync(IAccounting accounting);

        Task<IAccounting> UpdateAccountingAsync(IAccounting accounting);

        Task<IAccounting> DeleteAccountingAsync(int number);

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