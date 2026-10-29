namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Exceptions;

public class IdentifierAlreadyExistsException : IdentifierExceptionBase
{
    #region Constructors

    public IdentifierAlreadyExistsException(
        Guid identifier,
        string message = "The identifier already exists.")
        : base(message, identifier)
    {
    }

    #endregion
}