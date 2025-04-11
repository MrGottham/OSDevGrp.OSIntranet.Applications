using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Bff.WebApi.Filters.ErrorHandling;
using OSDevGrp.OSIntranet.Bff.WebApi.Filters.SchemaValidation;
using System.Net;
using System.Security;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Filters.ErrorHandling.ProblemDetailsFactory;

[TestFixture]
public class CreateProblemDetailsTests : CreateProblemDetailsTestBase
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

        Uri requestUrl = CreateRequestUrl(_fixture!);
        HttpRequest httpRequest = CreateHttpRequest(_fixture!, requestUrl: requestUrl);
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

        Uri requestUrl = CreateRequestUrl(_fixture!);
        HttpRequest httpRequest = CreateHttpRequest(_fixture!, requestUrl: requestUrl);
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

        Uri requestUrl = CreateRequestUrl(_fixture!);
        HttpRequest httpRequest = CreateHttpRequest(_fixture!, requestUrl: requestUrl);
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

        Uri requestUrl = CreateRequestUrl(_fixture!);
        HttpRequest httpRequest = CreateHttpRequest(_fixture!, requestUrl: requestUrl);
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

        Uri requestUrl = CreateRequestUrl(_fixture!);
        HttpRequest httpRequest = CreateHttpRequest(_fixture!, requestUrl: requestUrl);
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

        Uri requestUrl = CreateRequestUrl(_fixture!);
        HttpRequest httpRequest = CreateHttpRequest(_fixture!, requestUrl: requestUrl);
        Exception exception = new Exception(_fixture.Create<string>());
        ProblemDetails problemDetails = sut.CreateProblemDetails(httpRequest, exception);

        VerifyProblemDetails(problemDetails, 
            HttpStatusCode.InternalServerError,
            GetExpectedTitel(HttpStatusCode.InternalServerError), 
            GetExpectedDetail(HttpStatusCode.InternalServerError), 
            requestUrl);
    }
}