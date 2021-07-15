using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.WebApi.Filters
{
    internal class SchemaValidationFilter : IActionFilter
    {
        #region Methods

        public void OnActionExecuting(ActionExecutingContext context)
        {
            NullGuard.NotNull(context, nameof(context));
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            NullGuard.NotNull(context, nameof(context));

            ObjectResult objectResult = context.Result as ObjectResult;
            if (objectResult == null)
            {
                return;
            }

            SchemaValidator schemaValidator = new SchemaValidator();
            schemaValidator.Validate(objectResult.Value);
        }

        #endregion
    }
}