using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;

namespace OSDevGrp.OSIntranet.Core.Interfaces.Enums
{
    public enum ErrorCode
    {
        [ErrorCode("Unable to find a command handler which supports the command type '{0}'.", typeof(IntranetCommandBusException))]
        NoCommandHandlerSupportingCommandWithoutResultType = 1001,

        [ErrorCode("Unable to find a command handler which supports the command type '{0}' and the result type '{1}'.", typeof(IntranetCommandBusException))]
        NoCommandHandlerSupportingCommandWithResultType = 1002,

        [ErrorCode("Unable to find a query handler which supports the query type '{0}' and the result type '{1}'.", typeof(IntranetQueryBusException))]
        NoQueryHandlerSupportingQuery = 1003,

        [ErrorCode("An error occurred while publishing the command type '{0}': {1}", typeof(IntranetCommandBusException))]
        ErrorWhilePublishingCommandWithoutResultType = 1004,

        [ErrorCode("An error occurred while publishing the command type '{0}' to get the result type '{1}': {2}", typeof(IntranetCommandBusException))]
        ErrorWhilePublishingCommandWithResultType = 1005,

        [ErrorCode("An error occurred while querying the query type '{0}' to get the result type '{1}': {2}", typeof(IntranetQueryBusException))]
        ErrorWhileQueryingQuery = 1006,

        [ErrorCode("An error occurred in the repository method '{0}': {1}", typeof(IntranetRepositoryException))]
        RepositoryError = 1007
    }
}
