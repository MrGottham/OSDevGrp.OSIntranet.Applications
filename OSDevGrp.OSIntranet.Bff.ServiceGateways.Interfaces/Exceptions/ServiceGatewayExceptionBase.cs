namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.Exceptions;

public abstract class ServiceGatewayExceptionBase : Exception
{
    #region Constructors

    protected ServiceGatewayExceptionBase(string message) 
        : base(message)
    {
    }

    protected ServiceGatewayExceptionBase(string message, Exception innerException) 
        : base(message, innerException)
    {
    }

    #endregion
}