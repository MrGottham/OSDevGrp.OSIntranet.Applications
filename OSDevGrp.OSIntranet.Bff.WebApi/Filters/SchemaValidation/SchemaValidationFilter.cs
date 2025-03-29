using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Filters.SchemaValidation;

internal class SchemaValidationFilter : IActionFilter
{
    #region Private variables

    private readonly ISchemaValidator _schemaValidator;

    #endregion

    #region Constructor

    public SchemaValidationFilter(ISchemaValidator schemaValidator)
    {
        _schemaValidator = schemaValidator ?? throw new ArgumentNullException(nameof(schemaValidator));
    }

    #endregion

    #region Methods

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _schemaValidator.Validate(context.ModelState);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        ObjectResult? objectResult = context.Result as ObjectResult;
        if (objectResult == null || objectResult.Value == null)
        {
            return;
        }

        _schemaValidator.Validate(objectResult.Value);
    }

    #endregion
}