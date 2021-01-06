using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class ContactAccountViewModel : AccountCoreDataViewModel
    {
        [Display(Name = "Mailadresse", ShortName = "Mail", Description = "Mailadresse")]
        [StringLength(256, MinimumLength = 0, ErrorMessage = "Længden på mailadressen skal være mellem {2} og {1} tegn.")]
        [RegularExpression(ValidationRegexPatterns.MailAddressRegexPattern, ErrorMessage = "Mailadressen følger ikke det lovlige mønster: {1}")]
        public string MailAddress { get; set; }

        [Display(Name = "Primær telefonnummer", ShortName = "Primær telefonnr.", Description = "Primær telefonnummer")]
        [StringLength(32, MinimumLength = 0, ErrorMessage = "Længden på det primære telefonnummer skal være mellem {2} og {1} tegn.")]
        [RegularExpression(ValidationRegexPatterns.PhoneNumberRegexPattern, ErrorMessage = "Det primære telefonummer følger ikke det lovlige mønster: {1}")]
        public string PrimaryPhone { get; set; }

        [Display(Name = "Sekundær telefonnummer", ShortName = "Sekundær telefonnr.", Description = "Sekundær telefonnummer")]
        [StringLength(32, MinimumLength = 0, ErrorMessage = "Længden på det sekundære telefonnummer skal være mellem {2} og {1} tegn.")]
        [RegularExpression(ValidationRegexPatterns.PhoneNumberRegexPattern, ErrorMessage = "Det sekundære telefonummer følger ikke det lovlige mønster: {1}")]
        public string SecondaryPhone { get; set; }

        [Display(Name = "Betalingsbetingelse", ShortName = "Betalingsbetingelse", Description = "Betalingsbetingelse")]
        [Required(ErrorMessage = "Der skal vælges en betalingsbetingelse.")]
        public PaymentTermViewModel PaymentTerm { get; set; }
    }
}