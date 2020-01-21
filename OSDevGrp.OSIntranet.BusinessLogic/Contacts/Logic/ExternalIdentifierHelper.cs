using System;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.Logic
{
    internal static class ExternalIdentifierHelper
    {
        internal static IValidator ValidateExternalIdentifier(this IValidator validator, string value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            return validator.String.ShouldNotBeNullOrWhiteSpace(value, validatingType, validatingField);
        }
    }
}