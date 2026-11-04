namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Exceptions;

public abstract class ValidationExceptionBase : Exception
{
    #region Constructors

    protected ValidationExceptionBase(string message) : base(message)
    {
    }

    #endregion
}