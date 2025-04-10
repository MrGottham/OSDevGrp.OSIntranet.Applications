using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Bff.WebApi.Filters.SchemaValidation;
using System.Net;
using System.Security;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Filters.ErrorHandling;

internal class ProblemDetailsFactory : IProblemDetailsFactory
{
    #region Private variables

    private IReadOnlyDictionary<Type, Func<HttpRequest, Exception, ProblemDetails>> _problemDetailsBuilders = new Dictionary<Type, Func<HttpRequest, Exception, ProblemDetails>>
    {
        {typeof(ServiceGatewayBadRequestException), (httpRequest, exception) => ToProblemDetails(httpRequest, HttpStatusCode.BadRequest, "Bad Request", exception.Message)},
        {typeof(ServiceGatewayUnauthorizedException), (httpRequest, _) => ToProblemDetails(httpRequest, HttpStatusCode.Unauthorized, "Unauthorized", "You are not authorized to perform the requested operation.")},
        {typeof(ServiceGatewayServerErrorException), (httpRequest, _) => ToProblemDetails(httpRequest)},
        {typeof(SchemaValidationException), (httpRequest, exception) => ToProblemDetails(httpRequest, HttpStatusCode.BadRequest, "Bad Request", exception.Message)},
        {typeof(SecurityException), (httpRequest, _) => ToProblemDetails(httpRequest, HttpStatusCode.Unauthorized, "Unauthorized", "You are not authorized to perform the requested operation.")},
    };

    #endregion

    #region Methods

    public ProblemDetails CreateProblemDetails(HttpRequest httpRequest, Exception exception)
    {
        if (_problemDetailsBuilders.TryGetValue(exception.GetType(), out Func<HttpRequest, Exception, ProblemDetails>? problemDetailsBuilder))
        {
            return problemDetailsBuilder(httpRequest, exception);
        }

        return ToProblemDetails(httpRequest);
    }

    public ProblemDetails CreateProblemDetailsForBadRequest(HttpRequest httpRequest, string detail)
    {
        return ToProblemDetails(httpRequest, HttpStatusCode.BadRequest, "Bad Request", detail);
    }

    public ProblemDetails CreateProblemDetailsForUnauthorized(HttpRequest httpRequest)
    {
        return ToProblemDetails(httpRequest, HttpStatusCode.Unauthorized, "Unauthorized", "You are not authorized to perform the requested operation.");
    }

    public ProblemDetails CreateProblemDetailsForInternalServerError(HttpRequest httpRequest)
    {
        return ToProblemDetails(httpRequest, HttpStatusCode.InternalServerError, "Internal Server Error", "An internal server error occurred while processing your request.");
    }

    private static ProblemDetails ToProblemDetails(HttpRequest httpRequest)
    {
        return ToProblemDetails(httpRequest, HttpStatusCode.InternalServerError, "Internal Server Error", "An internal server error occurred while processing your request.");
    }

    private static ProblemDetails ToProblemDetails(HttpRequest httpRequest, HttpStatusCode httpStatusCode, string title, string detail)
    {
        return new ProblemDetails
        {
            Status = (int) httpStatusCode,
            Title = title,
            Detail = detail,
            Instance = httpRequest.GetEncodedUrl()
        };
    }

    #endregion
}