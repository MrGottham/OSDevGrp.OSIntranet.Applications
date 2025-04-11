using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.WebApi.Filters.ErrorHandling;
using System.Net;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Filters.ErrorHandling.ProblemDetailsFactory;

[TestFixture]
public class CreateProblemDetailsForInternalServerErrorTests : CreateProblemDetailsTestBase
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
    public void CreateProblemDetailsForInternalServerError_WhenCalled_ReturnsExpectedProblemDeails()
    {
        IProblemDetailsFactory sut = CreateSut();

        Uri requestUrl = CreateRequestUrl(_fixture!);
        HttpRequest httpRequest = CreateHttpRequest(_fixture!, requestUrl: requestUrl);
        ProblemDetails problemDetails = sut.CreateProblemDetailsForInternalServerError(httpRequest);

        VerifyProblemDetails(problemDetails, 
            HttpStatusCode.InternalServerError,
            GetExpectedTitel(HttpStatusCode.InternalServerError), 
            GetExpectedDetail(HttpStatusCode.InternalServerError), 
            requestUrl);
    }
}