using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Filters.ErrorHandling.ProblemDetailsFactory;

internal static class FixtureExtensions
{
    #region Methods

    internal static ProblemDetails CreateProblemDetails(this Fixture fixture, HttpStatusCode? httpStatusCode = null, string? title = null, string? detail = null, Uri? instance = null)
    {
        return new ProblemDetails
        {
            Status = (int) (httpStatusCode ?? HttpStatusCode.InternalServerError),
            Title = title ?? fixture.Create<string>(),
            Detail = detail ?? fixture.Create<string>(),
            Instance = (instance ?? fixture.CreateInstance()).AbsoluteUri
        };
    }

    internal static Uri CreateInstance(this Fixture fixture)
    {
        return new Uri($"http://{fixture.Create<string>()}.local/{fixture.Create<string>()}", UriKind.Absolute);
    }

    #endregion
}