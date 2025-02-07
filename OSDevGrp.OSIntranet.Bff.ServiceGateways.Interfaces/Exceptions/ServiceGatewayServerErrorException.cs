namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.Exceptions;

public class ServiceGatewayServerErrorException : ServiceGatewayExceptionBase
{
    #region Constructors

    public ServiceGatewayServerErrorException(string message) 
        : base(message)
    {
    }

    public ServiceGatewayServerErrorException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }

    #endregion
}