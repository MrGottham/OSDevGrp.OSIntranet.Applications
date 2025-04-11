using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.WebApi.Filters.ErrorHandling;
using OSDevGrp.OSIntranet.Bff.WebApi.Tests.Filters.ErrorHandling.ProblemDetailsFactory;
using System.Net;
using System.Net.Mime;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Filters.ErrorHandling.ErrorHandlerFilter;

[TestFixture]
public class OnExceptionTests
{
    #region Private variables

    private Mock<IProblemDetailsFactory>? _problemDetailsFactoryMock;
    private Fixture? _fixture = new Fixture();

    #endregion

    [SetUp]
    public void SetUp()
    {
        _problemDetailsFactoryMock = new Mock<IProblemDetailsFactory>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    public void OnException_WhenCalled_AssertCreateProblemDetailsWasCalledOnProblemDetailsFactoryWithHttpRequestFromHttpContextAtExceptionContext()
    {
        IExceptionFilter sut = CreateSut();

        HttpContext httpContext = CreateHttpContext();
        ActionContext actionContext = CreateActionContext(httpContext: httpContext);
        ExceptionContext exceptionContext = CreateExceptionContext(actionContext: actionContext);
        sut.OnException(exceptionContext);

        _problemDetailsFactoryMock!.Verify(m => m.CreateProblemDetails(
                It.Is<HttpRequest>(value => value == httpContext.Request),
                It.IsAny<Exception>()),
            Times.Once());
    }

    [Test]
    [Category("UnitTest")]
    public void OnException_WhenCalled_AssertCreateProblemDetailsWasCalledOnProblemDetailsFactoryWithExceptionAtExceptionContext()
    {
        IExceptionFilter sut = CreateSut();

        Exception exception = new Exception(_fixture!.Create<string>());
        ExceptionContext exceptionContext = CreateExceptionContext(exception: exception);
        sut.OnException(exceptionContext);

        _problemDetailsFactoryMock!.Verify(m => m.CreateProblemDetails(
                It.IsAny<HttpRequest>(),
                It.Is<Exception>(value => value == exception)),
            Times.Once());
    }

    [Test]
    [Category("UnitTest")]
    public void OnException_WhenCalled_ExpectResultAtExceptionContextIsNotNull()
    {
        IExceptionFilter sut = CreateSut();

        Exception exception = new Exception(_fixture!.Create<string>());
        ExceptionContext exceptionContext = CreateExceptionContext(exception: exception);
        sut.OnException(exceptionContext);

        Assert.That(exceptionContext.Result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void OnException_WhenCalled_ExpectResultAtExceptionContextIsSetToObjectResult()
    {
        IExceptionFilter sut = CreateSut();

        Exception exception = new Exception(_fixture!.Create<string>());
        ExceptionContext exceptionContext = CreateExceptionContext(exception: exception);
        sut.OnException(exceptionContext);

        Assert.That(exceptionContext.Result!, Is.TypeOf<ObjectResult>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(HttpStatusCode.BadRequest)]
    [TestCase(HttpStatusCode.Unauthorized)]
    [TestCase(HttpStatusCode.InternalServerError)]
    public void OnException_WhenCalled_ExpectResultAtExceptionContextIsSetToObjectResultWithStatusCodeEqualToStatusCodeFromCreatedProblemDetails(HttpStatusCode httpStatusCode)
    {
        ProblemDetails problemDetails = _fixture!.CreateProblemDetails(httpStatusCode: httpStatusCode);
        IExceptionFilter sut = CreateSut(problemDetails: problemDetails);

        Exception exception = new Exception(_fixture!.Create<string>());
        ExceptionContext exceptionContext = CreateExceptionContext(exception: exception);
        sut.OnException(exceptionContext);

        Assert.That(((ObjectResult) exceptionContext.Result!).StatusCode, Is.EqualTo((int) httpStatusCode));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(HttpStatusCode.BadRequest)]
    [TestCase(HttpStatusCode.Unauthorized)]
    [TestCase(HttpStatusCode.InternalServerError)]
    public void OnException_WhenCalled_ExpectResultAtExceptionContextIsSetToObjectResultWithValueEqualToCreatedProblemDetails(HttpStatusCode httpStatusCode)
    {
        ProblemDetails problemDetails = _fixture!.CreateProblemDetails(httpStatusCode: httpStatusCode);
        IExceptionFilter sut = CreateSut(problemDetails: problemDetails);

        Exception exception = new Exception(_fixture!.Create<string>());
        ExceptionContext exceptionContext = CreateExceptionContext(exception: exception);
        sut.OnException(exceptionContext);

        Assert.That(((ObjectResult) exceptionContext.Result!).Value, Is.EqualTo(problemDetails));
    }

    [Test]
    [Category("UnitTest")]
    public void OnException_WhenCalled_ExpectResultAtExceptionContextIsSetToObjectResultWithNonEmptyContentTypes()
    {
        ProblemDetails problemDetails = _fixture!.CreateProblemDetails();
        IExceptionFilter sut = CreateSut(problemDetails: problemDetails);

        Exception exception = new Exception(_fixture!.Create<string>());
        ExceptionContext exceptionContext = CreateExceptionContext(exception: exception);
        sut.OnException(exceptionContext);

        Assert.That(((ObjectResult) exceptionContext.Result!).ContentTypes, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    public void OnException_WhenCalled_ExpectResultAtExceptionContextIsSetToObjectResultWithContentTypesContainingMediaTypeForApplicationAndProblemJson()
    {
        ProblemDetails problemDetails = _fixture!.CreateProblemDetails();
        IExceptionFilter sut = CreateSut(problemDetails: problemDetails);

        Exception exception = new Exception(_fixture!.Create<string>());
        ExceptionContext exceptionContext = CreateExceptionContext(exception: exception);
        sut.OnException(exceptionContext);

        Assert.That(((ObjectResult) exceptionContext.Result!).ContentTypes.SingleOrDefault(mediaType => mediaType == MediaTypeNames.Application.ProblemJson), Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void OnException_WhenCalled_ExpectExceptionHandledAtExceptionContextIsSetToTrue()
    {
        IExceptionFilter sut = CreateSut();

        Exception exception = new Exception(_fixture!.Create<string>());
        ExceptionContext exceptionContext = CreateExceptionContext(exception: exception);
        sut.OnException(exceptionContext);

        Assert.That(exceptionContext.ExceptionHandled, Is.True);
    }

    private IExceptionFilter CreateSut(ProblemDetails? problemDetails = null)
    {
        _problemDetailsFactoryMock!.Setup(_fixture!, problemDetails: problemDetails);

        return new WebApi.Filters.ErrorHandling.ErrorHandlerFilter(_problemDetailsFactoryMock!.Object);
    }

    private ExceptionContext CreateExceptionContext(ActionContext? actionContext = null, Exception? exception = null)
    {
        return new ExceptionContext(actionContext ?? CreateActionContext(), Array.Empty<IFilterMetadata>())
        {
            Exception = exception ?? new Exception(_fixture!.Create<string>())
        };
    }

    private static ActionContext CreateActionContext(HttpContext? httpContext = null)
    {
        return new ActionContext
        {
            HttpContext = httpContext ?? CreateHttpContext(),
            RouteData = new RouteData(),
            ActionDescriptor = new ActionDescriptor()
        };
    }

    private static HttpContext CreateHttpContext()
    {
        return new DefaultHttpContext();
    }
}