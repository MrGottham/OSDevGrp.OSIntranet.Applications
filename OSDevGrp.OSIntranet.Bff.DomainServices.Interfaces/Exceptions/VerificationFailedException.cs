namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Exceptions;

public class VerificationFailedException : Exception
{
    #region Constructors

    public VerificationFailedException(string message)
        : base(message)
    {
    }

    #endregion
}