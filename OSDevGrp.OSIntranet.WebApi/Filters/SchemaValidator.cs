using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;

namespace OSDevGrp.OSIntranet.WebApi.Filters
{
    internal class SchemaValidator
    {
        internal void Validate(object obj)
        {
            List<ValidationResult> validationResultCollection = new List<ValidationResult>();

            bool isValid = new DataAnnotationsValidator.DataAnnotationsValidator().TryValidateObjectRecursive(obj, validationResultCollection);
            if (isValid)
            {
                return;
            }

            string errorDetails = JsonSerializer.Serialize(BuildErrorDetails(validationResultCollection));

            throw new IntranetExceptionBuilder(ErrorCode.SubmittedMessageInvalid, errorDetails).Build();
        }

        private IDictionary<string, IList<string>> BuildErrorDetails(IEnumerable<ValidationResult> validationResultCollection)
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
    }
}