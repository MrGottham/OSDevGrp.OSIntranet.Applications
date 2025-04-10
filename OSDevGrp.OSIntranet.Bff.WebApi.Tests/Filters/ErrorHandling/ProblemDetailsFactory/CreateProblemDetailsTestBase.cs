using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.WebApi.Filters.ErrorHandling;
using System.Net;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Filters.ErrorHandling.ProblemDetailsFactory;

public abstract class CreateProblemDetailsTestBase
{
    #region Methods

    protected static IProblemDetailsFactory CreateSut()
    {
        return new WebApi.Filters.ErrorHandling.ProblemDetailsFactory();
    }

    protected static void VerifyProblemDetails(ProblemDetails problemDetails, HttpStatusCode expectedStatus, string expectedTitel, string expectedDetail, Uri expectedInstance, string? expectedType = null)
    {
        Assert.That(problemDetails.Status, Is.EqualTo((int) expectedStatus));
        Assert.That(problemDetails.Title, Is.EqualTo(expectedTitel));
        Assert.That(problemDetails.Detail, Is.EqualTo(expectedDetail));
        Assert.That(problemDetails.Instance, Is.EqualTo(expectedInstance.ToString()));
        if (expectedType != null)
        {
            Assert.That(problemDetails.Type, Is.EqualTo(expectedType));
        }
        else
        {
            Assert.That(problemDetails.Type, Is.Null);
        }
        Assert.That(problemDetails.Extensions, Is.Not.Null);
        Assert.That(problemDetails.Extensions, Is.Empty);
    }

    protected static HttpRequest CreateHttpRequest(Fixture fixture, Uri? requestUrl = null)
    {
        return CreateHttpRequestMock(fixture, requestUrl).Object;
    }

    protected static  Mock<HttpRequest> CreateHttpRequestMock(Fixture fixture, Uri? requestUrl = null)
    {
        requestUrl ??= CreateRequestUrl(fixture);

        Mock<HttpRequest> httpRequestMock = new Mock<HttpRequest>();
        httpRequestMock.Setup(m => m.Scheme)
            .Returns(requestUrl.Scheme);
        httpRequestMock.Setup(m => m.Host)
            .Returns(new HostString(requestUrl.Host));
        httpRequestMock.Setup(m => m.Path)
            .Returns(new PathString(requestUrl.LocalPath));
        httpRequestMock.Setup(m => m.QueryString)    
            .Returns(new QueryString(requestUrl.Query));
        return httpRequestMock;
    }

    protected static Uri CreateRequestUrl(Fixture fixture)
    {
        return new Uri($"https://{fixture.Create<string>()}.local/{fixture.Create<string>()}?argument1={fixture.Create<string>()}&argument2={fixture.Create<string>()}");
    }

    protected static string GetExpectedTitel(HttpStatusCode httpStatusCode)
    {
        switch (httpStatusCode)
        {
            case HttpStatusCode.BadRequest:
                return "Bad Request";

            case HttpStatusCode.Unauthorized:
                return "Unauthorized";

            case HttpStatusCode.InternalServerError:
                return "Internal Server Error";

            default:
                throw new ArgumentOutOfRangeException(nameof(httpStatusCode), httpStatusCode, null);
        }
    }

    protected static string GetExpectedDetail(HttpStatusCode httpStatusCode)
    {
        switch (httpStatusCode)
        {
            case HttpStatusCode.Unauthorized:
                return "You are not authorized to perform the requested operation.";

            case HttpStatusCode.InternalServerError:
                return "An internal server error occurred while processing your request.";

            default:
                throw new ArgumentOutOfRangeException(nameof(httpStatusCode), httpStatusCode, null);
        }
    }

    #endregion
}