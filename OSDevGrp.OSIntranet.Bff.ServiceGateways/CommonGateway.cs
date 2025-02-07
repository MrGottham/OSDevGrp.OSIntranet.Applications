using OSDevGrp.OSIntranet.Bff.ServiceGateways.Extensions;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.ServiceGateways;

internal class CommonGateway : ServiceGatewayBase, ICommonGateway
{
    #region Constructor

    public CommonGateway(IWebApiClient webApiClient)
        : base(webApiClient)
    {
    }

    #endregion

    #region Methods

    public async Task<IEnumerable<LetterHeadModel>> GetLetterHeadsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await WebApiClient.LetterheadsAsync(cancellationToken);
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