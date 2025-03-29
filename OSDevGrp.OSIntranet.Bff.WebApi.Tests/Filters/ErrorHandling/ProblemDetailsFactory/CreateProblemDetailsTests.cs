using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Bff.WebApi.Filters.ErrorHandling;
using OSDevGrp.OSIntranet.Bff.WebApi.Filters.SchemaValidation;
using System.Net;
using System.Security;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Filters.ErrorHandling.ProblemDetailsFactory;

[TestFixture]
public class CreateProblemDetailsTests
{
    #region Private variables

    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    public void CreateProblemDetails_WhenCalledWithServiceGatewayBadRequestException_ReturnsExpectedProblemDeails()
    {
        IProblemDetailsFactory sut = CreateSut();

        Uri requestUrl = CreateRequestUrl();
        HttpRequest httpRequest = CreateHttpRequest(requestUrl: requestUrl);
        string exceptionMessage = _fixture.Create<string>();
        ServiceGatewayBadRequestException exception = new ServiceGatewayBadRequestException(exceptionMessage);
        ProblemDetails problemDetails = sut.CreateProblemDetails(httpRequest, exception);

        VerifyProblemDetails(problemDetails, 
            HttpStatusCode.BadRequest,
            GetExpectedTitel(HttpStatusCode.BadRequest), 
            exceptionMessage, 
            requestUrl);
    }

    [Test]
    [Category("UnitTest")]
    public void CreateProblemDetails_WhenCalledWithServiceGatewayUnauthorizedException_ReturnsExpectedProblemDeails()
    {
        IProblemDetailsFactory sut = CreateSut();

        Uri requestUrl = CreateRequestUrl();
        HttpRequest httpRequest = CreateHttpRequest(requestUrl: requestUrl);
        ServiceGatewayUnauthorizedException exception = new ServiceGatewayUnauthorizedException(_fixture.Create<string>());
        ProblemDetails problemDetails = sut.CreateProblemDetails(httpRequest, exception);

        VerifyProblemDetails(problemDetails, 
            HttpStatusCode.Unauthorized,
            GetExpectedTitel(HttpStatusCode.Unauthorized), 
            GetExpectedDetail(HttpStatusCode.Unauthorized), 
            requestUrl);
    }

    [Test]
    [Category("UnitTest")]
    public void CreateProblemDetails_WhenCalledWithServiceGatewayServerErrorException_ReturnsExpectedProblemDeails()
    {
        IProblemDetailsFactory sut = CreateSut();

        Uri requestUrl = CreateRequestUrl();
        HttpRequest httpRequest = CreateHttpRequest(requestUrl: requestUrl);
        ServiceGatewayServerErrorException exception = new ServiceGatewayServerErrorException(_fixture.Create<string>());
        ProblemDetails problemDetails = sut.CreateProblemDetails(httpRequest, exception);

        VerifyProblemDetails(problemDetails, 
            HttpStatusCode.InternalServerError,
            GetExpectedTitel(HttpStatusCode.InternalServerError), 
            GetExpectedDetail(HttpStatusCode.InternalServerError), 
            requestUrl);
    }

    [Test]
    [Category("UnitTest")]
    public void CreateProblemDetails_WhenCalledWithSchemaValidationException_ReturnsExpectedProblemDeails()
    {
        IProblemDetailsFactory sut = CreateSut();

        Uri requestUrl = CreateRequestUrl();
        HttpRequest httpRequest = CreateHttpRequest(requestUrl: requestUrl);
        string exceptionMessage = _fixture.Create<string>();
        SchemaValidationException exception = new SchemaValidationException(exceptionMessage);
        ProblemDetails problemDetails = sut.CreateProblemDetails(httpRequest, exception);

        VerifyProblemDetails(problemDetails, 
            HttpStatusCode.BadRequest,
            GetExpectedTitel(HttpStatusCode.BadRequest), 
            exceptionMessage, 
            requestUrl);
    }

    [Test]
    [Category("UnitTest")]
    public void CreateProblemDetails_WhenCalledWithSecurityException_ReturnsExpectedProblemDeails()
    {
        IProblemDetailsFactory sut = CreateSut();

        Uri requestUrl = CreateRequestUrl();
        HttpRequest httpRequest = CreateHttpRequest(requestUrl: requestUrl);
        SecurityException exception = new SecurityException(_fixture.Create<string>());
        ProblemDetails problemDetails = sut.CreateProblemDetails(httpRequest, exception);

        VerifyProblemDetails(problemDetails, 
            HttpStatusCode.Unauthorized,
            GetExpectedTitel(HttpStatusCode.Unauthorized), 
            GetExpectedDetail(HttpStatusCode.Unauthorized), 
            requestUrl);
    }

    [Test]
    [Category("UnitTest")]
    public void CreateProblemDetails_WhenCalledWithUnmappedException_ReturnsExpectedProblemDeails()
    {
        IProblemDetailsFactory sut = CreateSut();

        Uri requestUrl = CreateRequestUrl();
        HttpRequest httpRequest = CreateHttpRequest(requestUrl: requestUrl);
        Exception exception = new Exception(_fixture.Create<string>());
        ProblemDetails problemDetails = sut.CreateProblemDetails(httpRequest, exception);

        VerifyProblemDetails(problemDetails, 
            HttpStatusCode.InternalServerError,
            GetExpectedTitel(HttpStatusCode.InternalServerError), 
            GetExpectedDetail(HttpStatusCode.InternalServerError), 
            requestUrl);
    }

    private static IProblemDetailsFactory CreateSut()
    {
        return new WebApi.Filters.ErrorHandling.ProblemDetailsFactory();
    }

    private void VerifyProblemDetails(ProblemDetails problemDetails, HttpStatusCode expectedStatus, string expectedTitel, string expectedDetail, Uri expectedInstance, string? expectedType = null)
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

    private HttpRequest CreateHttpRequest(Uri? requestUrl = null)
    {
        return CreateHttpRequestMock(requestUrl).Object;
    }

    private Mock<HttpRequest> CreateHttpRequestMock(Uri? requestUrl = null)
    {
        requestUrl ??= CreateRequestUrl();

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

    private Uri CreateRequestUrl()
    {
        return new Uri($"https://{_fixture.Create<string>()}.local/{_fixture.Create<string>()}?argument1={_fixture.Create<string>()}&argument2={_fixture.Create<string>()}");
    }

    private static string GetExpectedTitel(HttpStatusCode httpStatusCode)
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

    private static string GetExpectedDetail(HttpStatusCode httpStatusCode)
    {
        switch (httpStatusCode)
        {
            case HttpStatusCode.InternalServerError:
                return "An internal server error occurred while processing your request.";

            case HttpStatusCode.Unauthorized:
                return "You are not authorized to perform the requested operation.";

            default:
                throw new ArgumentOutOfRangeException(nameof(httpStatusCode), httpStatusCode, null);
        }
    }
}