using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;

namespace OSDevGrp.OSIntranet.WebApi.Helpers.Validators
{
    internal static class SchemaValidator
    {
        internal static void Validate(object obj)
        {
            List<ValidationResult> validationResultCollection = new List<ValidationResult>();

            bool isValid = new DataAnnotationsValidator.DataAnnotationsValidator().TryValidateObjectRecursive(obj, validationResultCollection);
            if (isValid)
            {
                return;
            }

            string errorDetails = JsonSerializer.Serialize(BuildErrorDetails(validationResultCollection));
            throw BuildException(errorDetails, obj?.GetType());
        }

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

        private static IDictionary<string, IList<string>> BuildErrorDetails(IEnumerable<ValidationResult> validationResultCollection)
        {
            NullGuard.NotNull(validationResultCollection, nameof(validationResultCollection));

            IDictionary<string, IList<string>> errorDetails = new ConcurrentDictionary<string, IList<string>>();
            foreach (ValidationResult validationResult in validationResultCollection.Where(m => string.IsNullOrWhiteSpace(m.ErrorMessage) == false))
            {
                foreach (string memberName in validationResult.MemberNames.Where(m => string.IsNullOrWhiteSpace(m) == false))
                {
                    if (errorDetails.ContainsKey(memberName))
                    {
                        errorDetails[memberName].Add(validationResult.ErrorMessage);
                        continue;
                    }

                    errorDetails.Add(memberName, new List<string> {validationResult.ErrorMessage});
                }
            }

            return errorDetails;
        }

        private static IntranetExceptionBase BuildException(string errorDetails, Type type = null)
        {
            NullGuard.NotNullOrWhiteSpace(errorDetails, nameof(errorDetails));

            IntranetExceptionBuilder intranetExceptionBuilder = new IntranetExceptionBuilder(ErrorCode.SubmittedMessageInvalid, errorDetails);
            if (type != null)
            {
                intranetExceptionBuilder.WithValidatingType(type);
            }

            return intranetExceptionBuilder.Build();
        }
    }
}