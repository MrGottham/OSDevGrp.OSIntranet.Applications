using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Validators;

namespace OSDevGrp.OSIntranet.WebApi.Filters
{
    internal class SchemaValidationFilter : IActionFilter
    {
        #region Methods

        public void OnActionExecuting(ActionExecutingContext context)
        {
            NullGuard.NotNull(context, nameof(context));

            SchemaValidator.Validate(context.ModelState);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            NullGuard.NotNull(context, nameof(context));

            ObjectResult objectResult = context.Result as ObjectResult;
            if (objectResult == null)
            {
                return;
            }

            SchemaValidator.Validate(objectResult.Value);
        }

        #endregion
    }
}