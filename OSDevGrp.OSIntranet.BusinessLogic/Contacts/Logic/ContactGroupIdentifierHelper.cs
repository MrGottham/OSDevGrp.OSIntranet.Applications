using System;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.Logic
{
    internal static class ContactGroupIdentifierHelper
    {
        #region Methods

        internal static IValidator ValidateContactGroupIdentifier(this IValidator validator, int value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            return validator.Integer.ShouldBeBetween(value, 1, 99, validatingType, validatingField);
        }

        internal static IContactGroup GetContactGroup(this int number, IContactRepository contactRepository, ref IContactGroup contactGroup)
        {
            NullGuard.NotNull(contactRepository, nameof(contactRepository));

            return contactGroup ??= contactRepository.GetContactGroupAsync(number).GetAwaiter().GetResult();
        }

        #endregion
    }
}