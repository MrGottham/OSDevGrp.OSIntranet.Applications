using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using System.Net.Mime;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Filters.ErrorHandling;

internal class ErrorHandlerFilter : IExceptionFilter
{
    #region Private variables

    private readonly IProblemDetailsFactory _problemDetailsFactory;

    #endregion

    #region Constructor

    public ErrorHandlerFilter(IProblemDetailsFactory problemDetailsFactory)
    {
        _problemDetailsFactory = problemDetailsFactory ?? throw new ArgumentNullException(nameof(problemDetailsFactory)); 
    }

    #endregion

    #region Methods

    public void OnException(ExceptionContext context)
    {
        ProblemDetails problemDetails = _problemDetailsFactory.CreateProblemDetails(context.HttpContext.Request, context.Exception);

        ObjectResult result = new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };
        result.ContentTypes.Add(MediaTypeHeaderValue.Parse(MediaTypeNames.Application.ProblemJson));

        context.Result = result;
        context.ExceptionHandled = true;
    }

    #endregion
}