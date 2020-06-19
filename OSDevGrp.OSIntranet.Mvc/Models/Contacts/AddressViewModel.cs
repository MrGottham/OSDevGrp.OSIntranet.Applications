using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Mvc.Helpers;

namespace OSDevGrp.OSIntranet.Mvc.Models.Contacts
{
    public class AddressViewModel
    {
        [Display(Name = "Adresse", ShortName = "Adresse", Description = "Adresse (linje 1)")]
        public string StreetLine1 { get; set; }

        [Display(Name = "Adresse", ShortName = "Adresse", Description = "Adresse (linje 1)")]
        public string StreetLine2 { get; set; }

        [Display(Name = "Postnummer", ShortName = "Postnr.", Description = "Postnummer")]
        [StringLength(16, MinimumLength = 0, ErrorMessage = "Længden på postnummeret skal være mellem {2} og {1} tegn.")]
        [RegularExpression(ValidationRegexPatterns.PostalCodeRegexPattern, ErrorMessage = "Postnummeret følger ikke det lovlige mønster: {1}")]
        public string PostalCode { get; set; }

        [Display(Name = "By", ShortName = "By", Description = "Bynavn")]
        public string City { get; set; }

        [Display(Name = "Stat", ShortName = "Stat", Description = "Stat")]
        public string State { get; set; }

        [Display(Name = "Land", ShortName = "Land", Description = "Landenavn")]
        public string Country { get; set; }

        [Display(Name = "Adresse", ShortName = "Adresse", Description = "Adresseoplysninger")]
        public string DisplayAddress { get; set; }

        public bool IsEmpty => string.IsNullOrWhiteSpace(StreetLine1) &&
                               string.IsNullOrWhiteSpace(StreetLine2) &&
                               string.IsNullOrWhiteSpace(PostalCode) &&
                               string.IsNullOrWhiteSpace(City) &&
                               string.IsNullOrWhiteSpace(State) &&
                               string.IsNullOrWhiteSpace(Country);
    }

    public static class AddressViewModelExtensions
    {
        public static string GetResolvePostalCodeUrl(this AddressViewModel addressViewModel, IUrlHelper urlHelper)
        {
            NullGuard.NotNull(addressViewModel, nameof(addressViewModel))
                .NotNull(urlHelper, nameof(urlHelper));

            return urlHelper.AbsoluteAction("ResolvePostalCode", "Contact", new {countryCode = "{countryCode}", postalCode = "{postalCode}"});
        }
    }
}