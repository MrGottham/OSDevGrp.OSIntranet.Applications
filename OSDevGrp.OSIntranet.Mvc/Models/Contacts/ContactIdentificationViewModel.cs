using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Mvc.Helpers;
using OSDevGrp.OSIntranet.Mvc.Models.Core;

namespace OSDevGrp.OSIntranet.Mvc.Models.Contacts
{
    public class ContactIdentificationViewModel : AuditableViewModelBase
    {
        public string ExternalIdentifier { get; set; }

        public string DisplayName { get; set; }

        [Display(Name = "Kontakttype", ShortName = "Kontakttype", Description = "Typen på kontakten")]
        [Required(ErrorMessage = "Typen på kontakten skal vælges.")]
        public ContactType ContactType { get; set; }
    }

    public static class ContactIdentificationViewModelExtensions
    {
        public static string GetStartLoadingContactUrl(this ContactIdentificationViewModel contactIdentificationViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(contactIdentificationViewModel, nameof(contactIdentificationViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("StartLoadingContact", "Contact", new {contactIdentificationViewModel.ExternalIdentifier, CountryCode = "{countryCode}"});
        }

        public static string GetLoadContactUrl(this ContactIdentificationViewModel contactIdentificationViewModel, IUrlHelper urlHelper, string countryCode)
        {
            NullGuard.NotNull(contactIdentificationViewModel, nameof(contactIdentificationViewModel))
                .NotNull(urlHelper, nameof(urlHelper))
                .NotNullOrWhiteSpace(countryCode, nameof(countryCode));

            return urlHelper.AbsoluteAction("LoadContact", "Contact", new {contactIdentificationViewModel.ExternalIdentifier, CountryCode = countryCode});
        }
    }
}