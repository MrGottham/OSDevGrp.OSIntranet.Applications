namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.Exceptions;

public class ServiceGatewayBadRequestException : ServiceGatewayExceptionBase
{
    #region Constructors

    public ServiceGatewayBadRequestException(string message) 
        : base(message)
    {
    }

    public ServiceGatewayBadRequestException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }

    #endregion
}