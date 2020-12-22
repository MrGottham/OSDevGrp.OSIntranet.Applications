using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.WebApi.Models.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;

namespace OSDevGrp.OSIntranet.WebApi.Filters
{
    internal class ErrorHandlerFilter : IActionFilter
    {
        #region Private variables

        private readonly IConverter _coreModelConverter = new CoreModelConverter();

        #endregion

        #region Methods

        public void OnActionExecuting(ActionExecutingContext context)
        {
            NullGuard.NotNull(context, nameof(context));
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            NullGuard.NotNull(context, nameof(context));

            if (context.Exception == null)
            {
                return;
            }

            if (context.Exception is IntranetExceptionBase intranetException)
            {
                context.Result = HandleIntranetException(intranetException);
                context.ExceptionHandled = true;
                return;
            }

            if (context.Exception is AggregateException aggregateException)
            {
                context.Result = HandleAggregateException(aggregateException);
                context.ExceptionHandled = true;
                return;
            }

            context.Result = HandleException(context.Exception);
            context.ExceptionHandled = true;
        }

        private IActionResult HandleIntranetException<T>(T intranetException) where T : IntranetExceptionBase
        {
            NullGuard.NotNull(intranetException, nameof(intranetException));

            if (intranetException is IntranetValidationException intranetValidationException)
            {
                ErrorModel errorModel = _coreModelConverter.Convert<IntranetValidationException, ErrorModel>(intranetValidationException);
                return new BadRequestObjectResult(errorModel);
            }

            if (intranetException is IntranetBusinessException intranetBusinessException)
            {
                ErrorModel errorModel = _coreModelConverter.Convert<IntranetBusinessException, ErrorModel>(intranetBusinessException);
                return new BadRequestObjectResult(errorModel);
            }

            return InternalServerErrorResult(intranetException);
        }

        private IActionResult HandleAggregateException(AggregateException aggregateException)
        {
            NullGuard.NotNull(aggregateException, nameof(aggregateException));

            IActionResult result = null;
            aggregateException.Handle(exception =>
            {
                if (exception is IntranetExceptionBase intranetException)
                {
                    result = HandleIntranetException(intranetException);
                    return true;
                }

                result = HandleException(exception);
                return true;
            });

            return result;
        }

        private IActionResult HandleException(Exception exception)
        {
            NullGuard.NotNull(exception, nameof(exception));

            return InternalServerErrorResult(exception);
        }

        private IActionResult InternalServerErrorResult<T>(T intranetException) where T : IntranetExceptionBase
        {
            NullGuard.NotNull(intranetException, nameof(intranetException));

            ErrorModel errorModel = _coreModelConverter.Convert<T, ErrorModel>(intranetException);
            return new ObjectResult(errorModel)
            {
                StatusCode = (int) HttpStatusCode.InternalServerError
            };
        }

        private IActionResult InternalServerErrorResult(Exception exception)
        {
            NullGuard.NotNull(exception, nameof(exception));

            IntranetExceptionBase intranetException = new IntranetExceptionBuilder(ErrorCode.InternalError, exception.Message)
                .WithInnerException(exception)
                .Build();

            return InternalServerErrorResult(intranetException);
        }

        #endregion
    }
}