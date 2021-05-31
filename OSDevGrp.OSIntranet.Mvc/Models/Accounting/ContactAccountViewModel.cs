using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Mvc.Models.Core;

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

        [Display(Name = "Saldooplysninger pr. dags dato", ShortName = "Saldoopl. pr. dags dato", Description = "Saldooplysninger pr. dags dato")]
        public BalanceInfoValuesViewModel ValuesAtStatusDate { get; set; }

        [Display(Name = "Saldooplysninger ved sidste måneds afslutning", ShortName = "Saldoopl. ved sidste måneds afslutning", Description = "Saldooplysninger ved sidste måneds afslutning")]
        public BalanceInfoValuesViewModel ValuesAtEndOfLastMonthFromStatusDate { get; set; }

        [Display(Name = "Saldooplysninger ved sidste års afslutning", ShortName = "Saldoopl. ved sidste års afslutning", Description = "Saldooplysninger ved sidste års afslutning")]
        public BalanceInfoValuesViewModel ValuesAtEndOfLastYearFromStatusDate { get; set; }

        [Display(Name = "Saldooplysninger", ShortName = "Saldoopl.", Description = "Saldooplysninger")]
        public BalanceInfoDictionaryViewModel BalanceInfos { get; set; }

        public IReadOnlyCollection<PaymentTermViewModel> PaymentTerms { get; set; }
    }

    public static class ContactAccountViewModelExtensions
    {
        public static string GetAction(this ContactAccountViewModel contactAccountViewModel)
        {
            NullGuard.NotNull(contactAccountViewModel, nameof(contactAccountViewModel));

            return contactAccountViewModel.EditMode == EditMode.Create ? "CreateContactAccount" : "UpdateContactAccount";
        }
    }
}