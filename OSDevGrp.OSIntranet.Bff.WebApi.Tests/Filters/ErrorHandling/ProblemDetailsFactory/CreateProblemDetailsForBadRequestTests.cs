using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.WebApi.Filters.ErrorHandling;
using System.Net;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Filters.ErrorHandling.ProblemDetailsFactory;

[TestFixture]
public class CreateProblemDetailsForBadRequestTests : CreateProblemDetailsTestBase
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
    public void CreateProblemDetailsForBadRequest_WhenCalled_ReturnsExpectedProblemDeails()
    {
        IProblemDetailsFactory sut = CreateSut();

        Uri requestUrl = CreateRequestUrl(_fixture!);
        HttpRequest httpRequest = CreateHttpRequest(_fixture!, requestUrl: requestUrl);
        string detail = _fixture.Create<string>();
        ProblemDetails problemDetails = sut.CreateProblemDetailsForBadRequest(httpRequest, detail);

        VerifyProblemDetails(problemDetails, 
            HttpStatusCode.BadRequest,
            GetExpectedTitel(HttpStatusCode.BadRequest), 
            detail, 
            requestUrl);
    }
}