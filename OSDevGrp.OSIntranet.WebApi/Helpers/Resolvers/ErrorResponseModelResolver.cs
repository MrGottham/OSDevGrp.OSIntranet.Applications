﻿using Microsoft.AspNetCore.Authentication;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.WebApi.Models.Security;
using OSDevGrp.OSIntranet.WebApi.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.WebApi.Helpers.Resolvers
{
    internal static class ErrorResponseModelResolver
    {
        #region Private variables

        private static readonly IReadOnlyCollection<string> ValidErrors = new[]
        {
            "invalid_request",
            "invalid_client",
            "invalid_grant",
            "invalid_scope",
            "unauthorized_client",
            "access_denied",
            "unsupported_response_type",
            "unsupported_grant_type",
            "server_error",
            "temporarily_unavailable"
        };

        #endregion

        #region Methods

        internal static ErrorResponseModel Resolve(IntranetValidationException intranetValidationException, string state)
        {
            NullGuard.NotNull(intranetValidationException, nameof(intranetValidationException));

            switch (intranetValidationException.ValidatingField)
            {
                case "ResponseType":
                    return Resolve("unsupported_response_type", intranetValidationException.Message, null, state);

                case "ClientId":
                case "RedirectUri":
                case "State":
                    return Resolve("invalid_request", intranetValidationException.Message, null, state);

                case "Scopes":
                    return Resolve("invalid_scope", intranetValidationException.Message, null, state);

                default:
                    return Resolve("invalid_request", intranetValidationException.Message, null, state);
            }
        }

        internal static ErrorResponseModel Resolve(IntranetBusinessException intranetBusinessException, string state)
        {
            NullGuard.NotNull(intranetBusinessException, nameof(intranetBusinessException));

            switch (intranetBusinessException.ErrorCode)
            {
                case ErrorCode.CannotRetrieveJwtBearerTokenForAuthenticatedClient:
                case ErrorCode.MissingNecessaryPermission:
                    return Resolve("unauthorized_client", intranetBusinessException.Message, null, state);

                case ErrorCode.UnableToAuthorizeUser:
                    return Resolve("access_denied", intranetBusinessException.Message, null, state);

                case ErrorCode.UnableAuthenticateClient:
                    return Resolve("invalid_client", intranetBusinessException.Message, null, state);

                default:
                    throw new NotSupportedException($"Unsupported {intranetBusinessException.GetType().Name}: {intranetBusinessException.ErrorCode}");
            }
        }

        internal static ErrorResponseModel Resolve(Exception exception, string state)
        {
            NullGuard.NotNull(exception, nameof(exception));

            return Resolve("server_error", exception.Message, null, state);
        }

        internal static ErrorResponseModel Resolve(AuthenticateResult authenticateResult, string state)
        {
            NullGuard.NotNull(authenticateResult, nameof(authenticateResult));

            if (authenticateResult.None)
            {
                return Resolve("access_denied", ErrorDescriptionResolver.Resolve(ErrorCode.UnableToAuthorizeUser), null, state);
            }

            if (authenticateResult.Failure != null)
            {
                return Resolve("access_denied", ErrorDescriptionResolver.Resolve(ErrorCode.UnableToAuthorizeUser), null, state);
            }

            if (authenticateResult.Succeeded && authenticateResult.Principal?.Identity == null)
            {
                return Resolve("access_denied", ErrorDescriptionResolver.Resolve(ErrorCode.UnableToAuthorizeUser), null, state);
            }

            throw new NotSupportedException();
        }

        internal static ErrorResponseModel Resolve(AuthenticationProperties authenticationProperties, string state)
        {
            NullGuard.NotNull(authenticationProperties, nameof(authenticationProperties));

            if (authenticationProperties.Items.Count == 0 || authenticationProperties.Items.ContainsKey(KeyNames.AuthorizationStateKey) == false || string.IsNullOrWhiteSpace(authenticationProperties.Items[KeyNames.AuthorizationStateKey]))
            {
                return Resolve("access_denied", ErrorDescriptionResolver.Resolve(ErrorCode.UnableToAuthorizeUser), null, state);
            }

            throw new NotSupportedException();
        }

        internal static ErrorResponseModel Resolve(ClaimsIdentity claimsIdentity, string state)
        {
            NullGuard.NotNull(claimsIdentity, nameof(claimsIdentity));

            if (claimsIdentity.IsAuthenticated == false)
            {
                return Resolve("access_denied", ErrorDescriptionResolver.Resolve(ErrorCode.UnableToAuthorizeUser), null, state);
            }

            string emailAddress = claimsIdentity.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                return Resolve("access_denied", ErrorDescriptionResolver.Resolve(ErrorCode.UnableToAuthorizeUser), null, state);
            }

            throw new NotSupportedException();
        }

        internal static ErrorResponseModel Resolve(string error, string errorDescription, Uri errorUri, string state)
        {
            NullGuard.NotNullOrWhiteSpace(error, nameof(error));

            if (ValidErrors.Contains(error) == false)
            {
                throw new NotSupportedException($"Unsupported error: {error}");
            }

            return new ErrorResponseModel
            {
                Error = error,
                ErrorDescription = string.IsNullOrWhiteSpace(errorDescription) == false ? errorDescription : null,
                ErrorUri = errorUri?.AbsoluteUri,
                State = state
            };
        }

        #endregion
    }
}