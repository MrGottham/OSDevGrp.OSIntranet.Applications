using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Mvc.Models.Core;

namespace OSDevGrp.OSIntranet.Mvc.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        #region Methods

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            string requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            IExceptionHandlerPathFeature exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionHandlerPathFeature == null || exceptionHandlerPathFeature.Error == null)
            {
                return View(new ErrorViewModel {RequestId = requestId});
            }

            return ExceptionToView(exceptionHandlerPathFeature.Error, requestId);
        }

        private IActionResult ExceptionToView(Exception exception, string requestId)
        {
            NullGuard.NotNull(exception, nameof(exception));

            IntranetExceptionBase intranetException = exception as IntranetBusinessException;
            if (intranetException != null)
            {
                return ExceptionToView(intranetException, requestId);
            }

            AggregateException aggregateException = exception as AggregateException;
            if (aggregateException != null)
            {
                return ExceptionToView(aggregateException, requestId);
            }

            return View(new ErrorViewModel {RequestId = requestId});
        }

        private IActionResult ExceptionToView(IntranetExceptionBase intranetException, string requestId)
        {
            NullGuard.NotNull(intranetException, nameof(intranetException));

            IntranetBusinessException intranetBusinessException = intranetException as IntranetBusinessException;
            if (intranetBusinessException != null)
            {
                return View(new ErrorViewModel {RequestId = requestId, ErrorCode = (int) intranetBusinessException.ErrorCode, ErrorMesssage = intranetBusinessException.Message});
            }

            return View(new ErrorViewModel {RequestId = requestId});
        }

        private IActionResult ExceptionToView(AggregateException aggregateException, string requestId)
        {
            NullGuard.NotNull(aggregateException, nameof(aggregateException));

            Exception exception = null;
            aggregateException.Handle(ex => 
            {
                exception = ex;
                return true;
            });

            return ExceptionToView(exception, requestId);
        }

        #endregion
    }
}
