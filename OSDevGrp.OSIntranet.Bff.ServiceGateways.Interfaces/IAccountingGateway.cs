using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;

public interface IAccountingGateway : IServiceGateway
{
    Task<IEnumerable<AccountingModel>> GetAccountingsAsync(CancellationToken cancellationToken = default);

    Task<AccountingModel> GetAccountingAsync(int accountingNumber, DateTimeOffset statusDate, CancellationToken cancellationToken = default);

    Task<AccountModel> GetAccountAsync(int accountingNumber, string accountNumber, DateTimeOffset statusDate, CancellationToken cancellationToken = default);

    Task<BudgetAccountModel> GetBudgetAccountAsync(int accountingNumber, string accountNumber, DateTimeOffset statusDate, CancellationToken cancellationToken = default);

    Task<ContactAccountModel> GetContactAccountAsync(int accountingNumber, string accountNumber, DateTimeOffset statusDate, CancellationToken cancellationToken = default);

    Task<ApplyPostingJournalModel> GetPostingJournalAsync(int accountingNumber, CancellationToken cancellationToken = default);

    Task<ApplyPostingJournalModel> SavePostingJournalAsync(int accountingNumber, ApplyPostingJournalModel postingJournal, CancellationToken cancellationToken = default);

    Task<IEnumerable<PostingLineModel>> GetPostingLinesAsync(int accountingNumber, DateTimeOffset statusDate, int numberOfPostingLines, Predicate<PostingLineModel> filter, CancellationToken cancellationToken = default);

    Task<IEnumerable<AccountGroupModel>> GetAccountGroupsAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<BudgetAccountGroupModel>> GetBudgetAccountGroupsAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<PaymentTermModel>> GetPaymentTermsAsync(CancellationToken cancellationToken = default);
}