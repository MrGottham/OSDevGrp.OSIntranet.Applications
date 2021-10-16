using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;

namespace OSDevGrp.OSIntranet.WebApi.Helpers.Validators
{
    internal static class SchemaValidator
    {
        internal static void Validate(ModelStateDictionary modelStateDictionary)
        {
            NullGuard.NotNull(modelStateDictionary, nameof(modelStateDictionary));

            if (modelStateDictionary.IsValid)
            {
                return;
            }

            ValidationProblemDetails validationProblemDetails = new ValidationProblemDetails(modelStateDictionary);
            if (validationProblemDetails.Errors.Count == 0)
            {
                return;
            }

            string errorDetails = JsonSerializer.Serialize(validationProblemDetails.Errors);
            throw new IntranetExceptionBuilder(ErrorCode.SubmittedMessageInvalid, errorDetails).Build();
        }
    }
}