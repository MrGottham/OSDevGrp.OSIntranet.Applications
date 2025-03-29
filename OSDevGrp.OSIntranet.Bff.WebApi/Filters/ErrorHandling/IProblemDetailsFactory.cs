using Microsoft.AspNetCore.Mvc;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Filters.ErrorHandling;

public interface IProblemDetailsFactory
{
    ProblemDetails CreateProblemDetails(HttpRequest httpRequest, Exception exception);
}