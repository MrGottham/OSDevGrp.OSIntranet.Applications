namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Exceptions;

public class UnknownIdentifierException : IdentifierExceptionBase
{
    #region Constructors

    public UnknownIdentifierException(
        Guid identifier,
        string message = "The identifier is unknown.")
        : base(message, identifier)
    {
    }

    #endregion
}