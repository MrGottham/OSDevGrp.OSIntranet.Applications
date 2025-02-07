using OSDevGrp.OSIntranet.Bff.ServiceGateways.Extensions;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.ServiceGateways;

internal class AccountingGateway : ServiceGatewayBase, IAccountingGateway
{
    #region Constructor

    public AccountingGateway(IWebApiClient webApiClient)
        : base(webApiClient)
    {
    }

    #endregion

    #region Methods

    public async Task<IEnumerable<AccountingModel>> GetAccountingsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await WebApiClient.AccountingsAsync(cancellationToken);
        }
        catch (WebApiClientException<ErrorModel> ex)
        {
            throw ex.ToServiceGatewayException();
        }
        catch (WebApiClientException ex)
        {
            throw ex.ToServiceGatewayException();
        }
    }

    public async Task<AccountingModel> GetAccountingAsync(int accountingNumber, DateTimeOffset statusDate, CancellationToken cancellationToken = default)
    {
        try
        {
            return await WebApiClient.AccountingAsync(accountingNumber, statusDate, cancellationToken);
        }
        catch (WebApiClientException<ErrorModel> ex)
        {
            throw ex.ToServiceGatewayException();
        }
        catch (WebApiClientException ex)
        {
            throw ex.ToServiceGatewayException();
        }
    }

    public async Task<IEnumerable<AccountGroupModel>> GetAccountGroupsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await WebApiClient.AccountgroupsAsync(cancellationToken);
        }
        catch (WebApiClientException<ErrorModel> ex)
        {
            throw ex.ToServiceGatewayException();
        }
        catch (WebApiClientException ex)
        {
            throw ex.ToServiceGatewayException();
        }
    }

    public async Task<IEnumerable<BudgetAccountGroupModel>> GetBudgetAccountGroupsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await WebApiClient.BudgetaccountgroupsAsync(cancellationToken);
        }
        catch (WebApiClientException<ErrorModel> ex)
        {
            throw ex.ToServiceGatewayException();
        }
        catch (WebApiClientException ex)
        {
            throw ex.ToServiceGatewayException();
        }
    }

    public async Task<IEnumerable<PaymentTermModel>> GetPaymentTermsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await WebApiClient.PaymenttermsAsync(cancellationToken);
        }
        catch (WebApiClientException<ErrorModel> ex)
        {
            throw ex.ToServiceGatewayException();
        }
        catch (WebApiClientException ex)
        {
            throw ex.ToServiceGatewayException();
        }
    }

    #endregion
}