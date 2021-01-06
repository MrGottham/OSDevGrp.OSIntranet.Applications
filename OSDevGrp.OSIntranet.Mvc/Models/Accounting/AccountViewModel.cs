using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class AccountViewModel : AccountCoreDataViewModel
    {
        [Display(Name = "Kontogruppe", ShortName = "Gruppe", Description = "Kontogruppe")]
        [Required(ErrorMessage = "Der skal vælges en kontogruppe.")]
        public AccountGroupViewModel AccountGroup { get; set; }

        [Display(Name = "Kreditoplysninger pr. dags dato", ShortName = "Kreditopl. pr. dags dato", Description = "Kreditoplysninger pr. dags dato")]
        public CreditInfoValuesViewModel ValuesAtStatusDate { get; set; }

        [Display(Name = "Kreditoplysninger ved sidste måneds afslutning", ShortName = "Kreditopl. ved sidste måneds afslutning", Description = "Kreditoplysninger ved sidste måneds afslutning")]
        public CreditInfoValuesViewModel ValuesAtEndOfLastMonthFromStatusDate { get; set; }

        [Display(Name = "Kreditoplysninger ved sidste års afslutning", ShortName = "Kreditopl. ved sidste års afslutning", Description = "Kreditoplysninger ved sidste års afslutning")]
        public CreditInfoValuesViewModel ValuesAtEndOfLastYearFromStatusDate { get; set; }
    }
}