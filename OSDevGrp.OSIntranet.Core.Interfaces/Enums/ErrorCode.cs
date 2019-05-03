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
        RepositoryError = 1007,

        [ErrorCode("The value for the submitted field '{0}' should be greater than 0.", typeof(IntranetValidationException))]
        ValueNotGreaterThanZero = 1008,

        [ErrorCode("The value for the submitted field '{0}' should be greater than or equal to 0.", typeof(IntranetValidationException))]
        ValueNotGreaterThanOrEqualToZero = 1009,

        [ErrorCode("The value for the submitted field '{0}' should be between {1} and {2}.", typeof(IntranetValidationException))]
        ValueNotBetween = 1010,

        [ErrorCode("The value for the submitted field '{0}' cannot be null.", typeof(IntranetValidationException))]
        ValueCannotBeNull = 1011,

        [ErrorCode("The value for the submitted field '{0}' cannot be null or empty.", typeof(IntranetValidationException))]
        ValueCannotBeNullOrEmpty = 1012,

        [ErrorCode("The value for the submitted field '{0}' cannot be null, empty or white space.", typeof(IntranetValidationException))]
        ValueCannotBeNullOrWhiteSpace = 1013,

        [ErrorCode("The value for the submitted field '{0}' should have a minimum length of {1}.", typeof(IntranetValidationException))]
        ValueShouldHaveMinLength = 1014,

        [ErrorCode("The value for the submitted field '{0}' should have a maximum length of {1}.", typeof(IntranetValidationException))]
        ValueShouldHaveMaxLength = 1015,

        [ErrorCode("The value for the submitted field '{0}' should match the following pattern: {1}.", typeof(IntranetValidationException))]
        ValueShouldMatchPattern = 1016,

        [ErrorCode("The date value for the submitted field '{0}' should be in the past.", typeof(IntranetValidationException))]
        ValueShouldBePastDate = 1017,

        [ErrorCode("The date value for the submitted field '{0}' should be in the past or today.", typeof(IntranetValidationException))]
        ValueShouldBePastDateOrToday = 1018,

        [ErrorCode("The date value for the submitted field '{0}' should be today.", typeof(IntranetValidationException))]
        ValueShouldBeToday = 1019,

        [ErrorCode("The date value for the submitted field '{0}' should be in the future.", typeof(IntranetValidationException))]
        ValueShouldBeFutureDate = 1020,
        
        [ErrorCode("The date value for the submitted field '{0}' should be today or in the future.", typeof(IntranetValidationException))]
        ValueShouldBeFutureDateOrToday = 1021,

        [ErrorCode("The date value for the submitted field '{0}' should be a past date within {1} {2} from {3}.", typeof(IntranetValidationException))]
        ValueShouldBePastDateWithinDaysFromOffsetDate = 1022,

        [ErrorCode("The date value for the submitted field '{0}' should be a future date within {1} {2} from {3}.", typeof(IntranetValidationException))]
        ValueShouldBeFutureDateWithinDaysFromOffsetDate = 1023,

        [ErrorCode("The date value for the submitted field '{0}' should be a date later than {1}.", typeof(IntranetValidationException))]
        ValueShouldBeLaterThanOffsetDate = 1024,

        [ErrorCode("The date value for the submitted field '{0}' should be a date later than or equal to {1}.", typeof(IntranetValidationException))]
        ValueShouldBeLaterThanOrEqualToOffsetDate = 1025,

        [ErrorCode("The value for the submitted field '{0}' should be known within the system.", typeof(IntranetValidationException))]
        ValueShouldBeKnown = 1026,

        [ErrorCode("The value for the submitted field '{0}' should be unknown within the system.", typeof(IntranetValidationException))]
        ValueShouldBeUnknown = 1027,

        [ErrorCode("The value for the submitted field '{0}' should refer to a deletable entity within the system.", typeof(IntranetValidationException))]
        ValueShouldReferToDeletableEntity = 1028
    }
}
