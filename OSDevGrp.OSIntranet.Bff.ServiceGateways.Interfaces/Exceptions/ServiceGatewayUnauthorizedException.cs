namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.Exceptions;

public class ServiceGatewayUnauthorizedException : ServiceGatewayExceptionBase
{
    #region Constructors

    public ServiceGatewayUnauthorizedException(string message) 
        : base(message)
    {
    }

    public ServiceGatewayUnauthorizedException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }

    #endregion
}