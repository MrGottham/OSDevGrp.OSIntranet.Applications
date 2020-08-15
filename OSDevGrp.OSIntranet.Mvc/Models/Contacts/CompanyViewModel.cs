using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Mvc.Models.Contacts
{
    public class CompanyViewModel
    {
        [Display(Name = "Firmanavn", ShortName = "Firmanavn", Description = "Firmaets navn")]
        [Required(ErrorMessage = "Firmanavn skal udfyldes.", AllowEmptyStrings = false)]
        public string CompanyName { get; set; }

        [Display(Name = "Adresse", ShortName = "Adresse", Description = "Adresseoplysninger")]
        [Required(ErrorMessage = "Adressen skal angives.")]
        public AddressViewModel Address { get; set; }

        [Display(Name = "Primær telefonnummer", ShortName = "Primær telefonnr.", Description = "Primær telefonnummer til firmaet")]
        [RegularExpression(ValidationRegexPatterns.PhoneNumberRegexPattern, ErrorMessage = "Det primære telefonummer til firmaet følger ikke det lovlige mønster: {1}")]
        public string PrimaryPhone { get; set; }

        [Display(Name = "Sekundær telefonnummer", ShortName = "Sekundær telefonnr.", Description = "Sekundær telefonnummer til firmaet")]
        [RegularExpression(ValidationRegexPatterns.PhoneNumberRegexPattern, ErrorMessage = "Det sekundære telefonummer til firmaet følger ikke det lovlige mønster: {1}")]
        public string SecondaryPhone { get; set; }

        [Display(Name = "Webside", ShortName = "Webside", Description = "Webside")]
        [StringLength(256, MinimumLength = 0, ErrorMessage = "Længden på firmaets webside skal være mellem {2} og {1} tegn.")]
        [RegularExpression(ValidationRegexPatterns.UrlRegexPattern, ErrorMessage = "Frimaets Webside følger ikke det lovlige mønster: {1}")]
        public string HomePage { get; set; }

        public bool IsEmpty => string.IsNullOrWhiteSpace(CompanyName) &&
                               (Address == null || Address.IsEmpty) &&
                               string.IsNullOrWhiteSpace(PrimaryPhone) &&
                               string.IsNullOrWhiteSpace(SecondaryPhone) &&
                               string.IsNullOrWhiteSpace(HomePage);
    }
}