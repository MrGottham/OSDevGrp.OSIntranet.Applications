using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.WebApi.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSDevGrp.OSIntranet.WebApi.Helpers.Resolvers
{
    internal static class ErrorResponseModelResolver
    {
        #region Private variables

        private static readonly IReadOnlyCollection<string> ValidErrors = new[]
        {
            "invalid_request",
            "unauthorized_client",
            "access_denied",
            "unsupported_response_type",
            "invalid_scope",
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

        internal static ErrorResponseModel Resolve(Exception exception, string state)
        {
            NullGuard.NotNull(exception, nameof(exception));

            return Resolve("server_error", exception.Message, null, state);
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