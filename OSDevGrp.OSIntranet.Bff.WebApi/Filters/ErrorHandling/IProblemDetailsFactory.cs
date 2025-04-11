using Microsoft.AspNetCore.Mvc;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Filters.ErrorHandling;

public interface IProblemDetailsFactory
{
    ProblemDetails CreateProblemDetails(HttpRequest httpRequest, Exception exception);

    ProblemDetails CreateProblemDetailsForBadRequest(HttpRequest httpRequest, string detail);

    ProblemDetails CreateProblemDetailsForUnauthorized(HttpRequest httpRequest);

    ProblemDetails CreateProblemDetailsForInternalServerError(HttpRequest httpRequest);
}