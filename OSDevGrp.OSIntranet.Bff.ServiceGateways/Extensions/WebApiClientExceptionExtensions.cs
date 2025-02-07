using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using System.Net;

namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.Extensions;

public static class WebApiClientExceptionExtensions
{
    #region Private constants

    private const string UnauthorizedText = "You are not authorized to perform the requested operation.";
    private const string ServerErrorText = "An external server error has occurred.";

    #endregion

    #region Methods

    public static ServiceGatewayExceptionBase ToServiceGatewayException(this WebApiClientException webApiClientException)
    {  
        return webApiClientException.ToServiceGatewayException(webApiClientException.Message, webApiClientException);
    }

    public static ServiceGatewayExceptionBase ToServiceGatewayException(this WebApiClientException<ErrorModel> webApiClientException)
    {  
        return webApiClientException.ToServiceGatewayException(webApiClientException.Result.ErrorMessage, webApiClientException);
    }

    public static ServiceGatewayExceptionBase ToServiceGatewayException(this WebApiClientException<ErrorResponseModel> webApiClientException)
    {  
        ErrorResponseModel errorResponseModel = webApiClientException.Result;
        return webApiClientException.ToServiceGatewayException($"{errorResponseModel.Error}{Environment.NewLine}{errorResponseModel.Error_description}", webApiClientException);
    }

    private static ServiceGatewayExceptionBase ToServiceGatewayException(this WebApiClientException webApiClientException, string message, Exception innerException)
    {
        if (webApiClientException.IsBadRequest())
        {
            return new ServiceGatewayBadRequestException(message, innerException);
        }

        if (webApiClientException.IsUnauthorized())
        {
            return new ServiceGatewayUnauthorizedException(UnauthorizedText, innerException);
        }

        if (webApiClientException.IsServerError())
        {
            return new ServiceGatewayServerErrorException(ServerErrorText, innerException);
        }

        throw new NotSupportedException($"Unsupported status code: {webApiClientException.StatusCode}", webApiClientException);
    }

    private static bool IsBadRequest(this WebApiClientException webApiClientException)
    {
        return (HttpStatusCode) webApiClientException.StatusCode == HttpStatusCode.BadRequest;
    }

    private static bool IsUnauthorized(this WebApiClientException webApiClientException)
    {
        return (HttpStatusCode) webApiClientException.StatusCode == HttpStatusCode.Unauthorized;
    }

    private static bool IsServerError(this WebApiClientException webApiClientException)
    {
        return webApiClientException.StatusCode >= 500 && webApiClientException.StatusCode < 600;
    }

    #endregion
}