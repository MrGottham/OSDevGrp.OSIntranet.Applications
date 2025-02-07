using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;

public interface IAccountingGateway : IServiceGateway
{
    Task<IEnumerable<AccountingModel>> GetAccountingsAsync(CancellationToken cancellationToken = default);

    Task<AccountingModel> GetAccountingAsync(int accountingNumber, DateTimeOffset statusDate, CancellationToken cancellationToken = default);

    Task<IEnumerable<AccountGroupModel>> GetAccountGroupsAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<BudgetAccountGroupModel>> GetBudgetAccountGroupsAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<PaymentTermModel>> GetPaymentTermsAsync(CancellationToken cancellationToken = default);
}