using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;

namespace OSDevGrp.OSIntranet.Core.Interfaces.Enums
{
    public enum ErrorCode
    {
        [ErrorCode("An error occurred in the repository method '{0}': {1}", typeof(IntranetRepositoryException))]
        RepositoryError = 1001
    }
}
