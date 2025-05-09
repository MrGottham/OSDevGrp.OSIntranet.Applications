﻿using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;

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
        ValueShouldReferToDeletableEntity = 1028,

        [ErrorCode("No token has been acquired for the repository defined by '{0}'.", typeof(IntranetRepositoryException))]
        NoTokenHasBeenAcquired = 1029,

        [ErrorCode("The acquired token for the repository defined by '{0}' has expired.", typeof(IntranetRepositoryException))]
        TokenHasExpired = 1030,

        [ErrorCode("The value for the object named '{0}' is null.", typeof(IntranetSystemException))]
        ObjectIsNull = 1031,

        [ErrorCode("The date and time value for the submitted field '{0}' should be in the past.", typeof(IntranetValidationException))]
        ValueShouldBePastDateTime = 1032,

        [ErrorCode("The date and time value for the submitted field '{0}' should be in the future.", typeof(IntranetValidationException))]
        ValueShouldBeFutureDateTime = 1033,

        [ErrorCode("A matching object of the type '{0}' already exists.", typeof(IntranetSystemException))]
        ObjectAlreadyExists = 1034,

        [ErrorCode("Unable to delete one or more objects of the type '{0}'.", typeof(IntranetRepositoryException))]
        UnableToDeleteOneOrMoreObjects = 1035,

        [ErrorCode("An internal error has occurred: {0}", typeof(IntranetSystemException))]
        InternalError = 1036,

        [ErrorCode("Cannot retrieve the ACME challenge for token.", typeof(IntranetBusinessException))]
        CannotRetrieveAcmeChallengeForToken = 1037,

        [ErrorCode("Cannot retrieve a JWT token for the authenticated client.", typeof(IntranetBusinessException))]
        CannotRetrieveJwtBearerTokenForAuthenticatedClient = 1038,

        [ErrorCode("The value for the submitted field '{0}' should contain some items.", typeof(IntranetValidationException))]
        ValueShouldContainSomeItems = 1039,

        [ErrorCode("The submitted message is invalid: {0}", typeof(IntranetValidationException))]
        SubmittedMessageInvalid = 1040,

        [ErrorCode("Unable to resolve the identifier for the current user.", typeof(IntranetSystemException))]
        UnableToResolveUserIdentifier = 1041,

        [ErrorCode("The connection string named '{0}' is missing or has no value.", typeof(IntranetSystemException))]
        MissingConnectionString = 1042,

        [ErrorCode("The configuration named '{0}' is missing or has no value.", typeof(IntranetSystemException))]
        MissingConfiguration = 1043,

        [ErrorCode("The value for the configuration named '{0}' is invalid.", typeof(IntranetSystemException))]
        InvalidConfigurationValue = 1044,

        [ErrorCode("The value named '{0}' has not been set on an instance of '{1}'.", typeof(IntranetSystemException))]
        NamedValueNotSetOnObject = 1045,

        [ErrorCode("Unable to translate the value for: {0}", typeof(IntranetSystemException))]
        UnableToTranslateValueFor = 1046,

        [ErrorCode("The operation can’t be completed because you don’t have the necessary permission.", typeof(IntranetBusinessException))]
        MissingNecessaryPermission = 1047,

        [ErrorCode("The value for the submitted field '{0}' should minimum have {1} items.", typeof(IntranetValidationException))]
        ValueShouldContainMinItems = 1048,

        [ErrorCode("The value for the submitted field '{0}' should maximum have {1} items.", typeof(IntranetValidationException))]
        ValueShouldContainMaxItems = 1049,

        [ErrorCode("The date value for the submitted field '{0}' should be a date earlier than {1}.", typeof(IntranetValidationException))]
		ValueShouldBeEarlierThanOffsetDate = 1050,

        [ErrorCode("The date value for the submitted field '{0}' should be a date earlier than or equal to {1}.", typeof(IntranetValidationException))]
		ValueShouldBeEarlierThanOrEqualToOffsetDate = 1051,

        [ErrorCode("Value has not been set by the method named '{0}' on an instance of '{1}'.", typeof(IntranetSystemException))]
		ValueNotSetByNamedMethod = 1052,

        [ErrorCode("Unable to authorize user.", typeof(IntranetBusinessException))]
        UnableToAuthorizeUser = 1053,

        [ErrorCode("Unable to generate the redirect uri.", typeof(IntranetSystemException))]
        UnableToGenerateRedirectUri = 1054,

        [ErrorCode("Unable authenticate the client.", typeof(IntranetBusinessException))]
        UnableAuthenticateClient = 1055,

        [ErrorCode("Cannot retrieve a JWT token for the authenticated user.", typeof(IntranetBusinessException))]
        CannotRetrieveJwtBearerTokenForAuthenticatedUser = 1056,

        [ErrorCode("Unable to generate an id token for the authenticated user.", typeof(IntranetBusinessException))]
        UnableToGenerateIdTokenForAuthenticatedUser = 1057
    }
}