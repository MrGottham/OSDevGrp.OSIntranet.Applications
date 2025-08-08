namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Exceptions;

public class VerificationFailedException : Exception
{
    #region Constructors

    public VerificationFailedException() 
        : base("Unable to verify the given verification code.")
    {
    }

    #endregion
}