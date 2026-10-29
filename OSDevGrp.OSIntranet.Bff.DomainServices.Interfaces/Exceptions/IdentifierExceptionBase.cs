namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Exceptions;

public abstract class IdentifierExceptionBase : ValidationExceptionBase
{
    #region Constructors

    protected IdentifierExceptionBase(string message, Guid identifier)
        : base(message)
    {
        Identifier = identifier;
    }

    #endregion

    #region Properties

    public Guid Identifier { get; }

    #endregion
}