using System;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.Logic
{
    internal static class ExternalIdentifierHelper
    {
        #region Methods

        internal static IValidator ValidateExternalIdentifier(this IValidator validator, string value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            return validator.String.ShouldNotBeNullOrWhiteSpace(value, validatingType, validatingField);
        }

        internal static IContact GetContact(this string externalIdentifier, IRefreshableToken token, IMicrosoftGraphRepository microsoftGraphRepository, IContactRepository contactRepository, ref IContact contact)
        {
            NullGuard.NotNullOrWhiteSpace(externalIdentifier, nameof(externalIdentifier))
                .NotNull(token, nameof(token))
                .NotNull(microsoftGraphRepository, nameof(microsoftGraphRepository))
                .NotNull(contactRepository, nameof(contactRepository));

            if (contact != null)
            {
                return contact;
            }

            contact = microsoftGraphRepository.GetContactAsync(token, externalIdentifier).GetAwaiter().GetResult();
            
            return contact == null ? null : contactRepository.ApplyContactSupplementAsync(contact).GetAwaiter().GetResult();
        }

        #endregion
    }
}