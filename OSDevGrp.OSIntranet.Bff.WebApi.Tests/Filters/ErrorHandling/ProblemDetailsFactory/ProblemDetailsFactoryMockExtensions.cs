using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OSDevGrp.OSIntranet.Bff.WebApi.Filters.ErrorHandling;
using System.Net;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Filters.ErrorHandling.ProblemDetailsFactory;

internal static class ProblemDetailsFactoryMockExtensions
{
    #region Methods

    internal static void Setup(this Mock<IProblemDetailsFactory> problemDetailsFactoryMock, Fixture fixture, ProblemDetails? problemDetails = null)
    {
        problemDetailsFactoryMock.Setup(m => m.CreateProblemDetails(It.IsAny<HttpRequest>(), It.IsAny<Exception>()))
            .Returns(problemDetails ?? fixture.CreateProblemDetails(HttpStatusCode.InternalServerError));
        problemDetailsFactoryMock.Setup(m => m.CreateProblemDetailsForBadRequest(It.IsAny<HttpRequest>(), It.IsAny<string>()))
            .Returns(problemDetails ?? fixture.CreateProblemDetails(HttpStatusCode.BadGateway));
        problemDetailsFactoryMock.Setup(m => m.CreateProblemDetailsForUnauthorized(It.IsAny<HttpRequest>()))
            .Returns(problemDetails ?? fixture.CreateProblemDetails(HttpStatusCode.Unauthorized));
        problemDetailsFactoryMock.Setup(m => m.CreateProblemDetailsForInternalServerError(It.IsAny<HttpRequest>()))
            .Returns(problemDetails ?? fixture.CreateProblemDetails(HttpStatusCode.InternalServerError));
    }

    #endregion
}